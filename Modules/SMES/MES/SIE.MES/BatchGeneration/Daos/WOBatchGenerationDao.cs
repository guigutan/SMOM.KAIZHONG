using Newtonsoft.Json;
using SIE.Barcodes.WipBatchs;
using SIE.Core.Common.Dao;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.BatchWIP.Products;
using SIE.MES.WIP.Products;
using SIE.MES.WorkOrders;
using SIE.Resources.Employees;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SIE.MES.BatchGeneration.Daos
{
    /// <summary>
    ///批次生成并过站DAO层
    /// </summary>
    public class WOBatchGenerationDao : BaseDao<WOBatchGeneration>
    {
        /// <summary>
        /// 批次生成并过站DAO层
        /// </summary>
        /// <param name="wOBatchGenerationCriteria"></param>
        /// <returns></returns>
        public virtual EntityList<WOBatchGeneration> QueryWorkOrderArchiveList(WOBatchGenerationCriteria wOBatchGenerationCriteria)
        {
            var query = Query();
            if (wOBatchGenerationCriteria == null)
            {
                throw new ValidationException("批次生成并打印实体异常！".L10N());
            }
            if (!wOBatchGenerationCriteria.No.IsNullOrEmpty())
            {
                query.Where(p => p.No.Contains(wOBatchGenerationCriteria.No));
            }
            if (!wOBatchGenerationCriteria.ProCode.IsNullOrEmpty())
            {
                query.Where(p => p.Product.Code.Contains(wOBatchGenerationCriteria.ProCode));
            }
            if (!wOBatchGenerationCriteria.ProName.IsNullOrEmpty())
            {
                query.Where(p => p.Product.Name.Contains(wOBatchGenerationCriteria.ProName));
            }
            if (wOBatchGenerationCriteria.State.IsNotEmpty())
            {
                var criteriaState = new List<int>();
                wOBatchGenerationCriteria.State.Split(',').ForEach(state =>
                {
                    criteriaState.Add(int.Parse(state));
                });
                query.Where(p => criteriaState.Contains((int)p.WoState));
            }
            if (!wOBatchGenerationCriteria.BatchNo.IsNullOrEmpty())
            {
                // 批次号Exists子表查询
                query.Exists<WipBatch>((x, y) => y.Where(p => p.BatchNo.Contains(wOBatchGenerationCriteria.BatchNo) && x.Id == p.WorkOrderId));
            }
            if (wOBatchGenerationCriteria.FactoryId != null && wOBatchGenerationCriteria.FactoryId != 0)
            {
                query.Where(p => p.FactoryId == wOBatchGenerationCriteria.FactoryId);
            }
            if (wOBatchGenerationCriteria.WorkShopId != null && wOBatchGenerationCriteria.WorkShopId != 0)
            {
                query.Where(p => p.WorkShopId == wOBatchGenerationCriteria.WorkShopId);
            }
            if (wOBatchGenerationCriteria.ResourceId != null && wOBatchGenerationCriteria.ResourceId != 0)
            {
                query.Where(p => p.ResourceId == wOBatchGenerationCriteria.ResourceId);
            }
            if (wOBatchGenerationCriteria.PlanBeginDate.BeginValue.HasValue)
            {
                query.Where(p => p.PlanBeginDate >= wOBatchGenerationCriteria.PlanBeginDate.BeginValue.Value);
            }
            if (wOBatchGenerationCriteria.PlanBeginDate.EndValue.HasValue)
            {
                query.Where(p => p.PlanBeginDate <= wOBatchGenerationCriteria.PlanBeginDate.EndValue.Value);
            }
            if (wOBatchGenerationCriteria.CreateDate.BeginValue.HasValue)
            {
                query.Where(p => p.CreateDate >= wOBatchGenerationCriteria.CreateDate.BeginValue.Value);
            }
            if (wOBatchGenerationCriteria.CreateDate.EndValue.HasValue)
            {
                query.Where(p => p.CreateDate <= wOBatchGenerationCriteria.CreateDate.EndValue.Value);
            }
            query.Exists<EmployeeEnterprise>((x, y) => y.Where(p => p.EnterpriseId == x.FactoryId && p.EmployeeId == RT.IdentityId));
            return query.OrderByDescending(p => p.No).OrderBy(wOBatchGenerationCriteria.OrderInfoList).ToList(wOBatchGenerationCriteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取批次模型数据
        /// </summary>
        /// <param name="workorderId"></param>
        /// <param name="isChild"></param>
        /// <param name="sortInfo"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<WipBatchViewModel> GetWipBatchsViewModelByWorkOrder(double workorderId, bool? isChild, List<OrderInfo> sortInfo, PagingInfo pagingInfo)
        {
            EntityList<WipBatchViewModel> wipBatchViewModels = new EntityList<WipBatchViewModel>();
            var query = DB.Query<WipBatch>().Where(p => p.WorkOrderId == workorderId);
            //if (isChild.HasValue)
            //    query.Where(p => p.IsChild == isChild.Value);
            var results = query.OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            foreach (var item in results)
            {
                var wipBatchViewModel = new WipBatchViewModel()
                {
                    Id = item.Id.ToString(),
                    BatchNo = item.BatchNo,
                    BatchState = item.BatchState,
                    BoxesQty = item.BoxesQty,
                    PrintTimes = item.PrintTimes,
                    PrintByName = item.PrintByName,
                    PrintDate = item.PrintDate,
                    PrintedState = item.PrintedState,
                    Qty = item.Qty,
                    ScrapQty = item.ScrapQty,
                    CreateBy = item.CreateByName,
                    CreateDate = item.CreateDate,
                    UpdateBy = item.UpdateByName,
                    UpdateDate = item.UpdateDate,
                };
                wipBatchViewModels.Add(wipBatchViewModel);
            }
            wipBatchViewModels.SetTotalCount(results.TotalCount);
            return wipBatchViewModels;
        }

        /// <summary>
        /// 根据工单ID获取指定类型工序集合
        /// </summary>
        /// <param name="woId"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <param name="processType"></param>
        /// <param name="isStart"></param>
        /// <returns></returns>
        public virtual EntityList<Process> GetProcessesByWoId(double woId, PagingInfo pagingInfo, string keyword, ProcessType processType, bool isStart = true)
        {
            var query = DB.Query<Process>()
            .Join<WorkOrderRoutingProcess>((x, k) => x.Id == k.ProcessId && k.WorkOrderId == woId)
            .Exists<ProcessEmployee>((a, b) => b.Where(f => f.ProcessId == a.Id && f.EmployeeId == RT.IdentityId))
            .Where(p => p.Type == processType);
            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.Name.Contains(keyword) || p.Code.Contains(keyword));
            }
            if (isStart)
            {
                query.Where<WorkOrderRoutingProcess>((a, k) => k.Sign == Tech.Routings.RoutingProcessSign.Start);
            }
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工单和当前人获取首工序
        /// </summary>
        /// <returns></returns>
        public virtual Process GetFirstProcessesByWoId(double woId, ProcessType processType)
        {
            return DB.Query<Process>()
                .Join<WorkOrderRoutingProcess>((x, k) => x.Id == k.ProcessId && k.WorkOrderId == woId && k.Sign == Tech.Routings.RoutingProcessSign.Start)
                .Exists<ProcessEmployee>((a, b) => b.Where(f => f.ProcessId == a.Id && f.EmployeeId == RT.IdentityId))
                .Where(p => p.Type == processType).FirstOrDefault();
        }
    }
}
