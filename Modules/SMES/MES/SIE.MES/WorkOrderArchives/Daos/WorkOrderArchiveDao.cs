using SIE.Core.Common.Dao;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.BatchWIP.Products;
using SIE.MES.WIP.Products;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.MES.WorkOrderArchives.Daos
{
    /// <summary>
    /// 工单制造档案DAO层
    /// </summary>
    public class WorkOrderArchiveDao : BaseDao<WorkOrderArchive>
    {
        /// <summary>
        /// 工单制造档案查询
        /// </summary>
        /// <param name="workOrderArchiveCriteria"></param>
        /// <returns></returns>
        public virtual EntityList<WorkOrderArchive> QueryWorkOrderArchiveList(WorkOrderArchiveCriteria workOrderArchiveCriteria)
        {
            var query = Query();
            if (workOrderArchiveCriteria == null)
            {
                throw new ValidationException("工单制造档案查询实体异常！".L10N());
            }
            if (!workOrderArchiveCriteria.No.IsNullOrEmpty())
            {
                query.Where(p => p.No.Contains(workOrderArchiveCriteria.No));
            }
            if (!workOrderArchiveCriteria.ProCode.IsNullOrEmpty())
            {
                query.Where(p => p.Product.Code.Contains(workOrderArchiveCriteria.ProCode));
            }
            if (!workOrderArchiveCriteria.ProName.IsNullOrEmpty())
            {
                query.Where(p => p.Product.Name.Contains(workOrderArchiveCriteria.ProName));
            }
            if (workOrderArchiveCriteria.State.IsNotEmpty())
            {
                var criteriaState = new List<int>();
                workOrderArchiveCriteria.State.Split(',').ForEach(state =>
                {
                    criteriaState.Add(int.Parse(state));
                });
                query.Where(p =>  criteriaState.Contains((int)p.WoState));
            }
            if (!workOrderArchiveCriteria.BarCode.IsNullOrEmpty())
            {
                // 条码Exists子表查询
                query.Exists<WipProductVersion>((x, y) => y.Where(p => p.Sn.Contains(workOrderArchiveCriteria.BarCode) && x.Id == p.WorkOrderId));
            }
            if (!workOrderArchiveCriteria.BatchNo.IsNullOrEmpty())
            {
                // 批次号Exists子表查询
                query.Exists<BatchWipProductVersion>((x, y) => y.Where(p => p.BatchNo.Contains(workOrderArchiveCriteria.BatchNo) && x.Id == p.WorkOrderId));
            }
            if (workOrderArchiveCriteria.FactoryId != null && workOrderArchiveCriteria.FactoryId != 0)
            {
                query.Where(p => p.FactoryId == workOrderArchiveCriteria.FactoryId);
            }
            if (workOrderArchiveCriteria.WorkShopId != null && workOrderArchiveCriteria.WorkShopId != 0)
            {
                query.Where(p => p.WorkShopId == workOrderArchiveCriteria.WorkShopId);
            }
            if (workOrderArchiveCriteria.ResourceId != null && workOrderArchiveCriteria.ResourceId != 0)
            {
                query.Where(p => p.ResourceId == workOrderArchiveCriteria.ResourceId);
            }
            if (workOrderArchiveCriteria.PlanBeginDate.BeginValue.HasValue)
            {
                query.Where(p => p.PlanBeginDate >= workOrderArchiveCriteria.PlanBeginDate.BeginValue.Value);
            }
            if (workOrderArchiveCriteria.PlanBeginDate.EndValue.HasValue)
            {
                query.Where(p => p.PlanBeginDate <= workOrderArchiveCriteria.PlanBeginDate.EndValue.Value);
            }
            if (workOrderArchiveCriteria.ActuFinishDate.BeginValue.HasValue)
            {
                query.Where(p => p.ActuFinishDate >= workOrderArchiveCriteria.ActuFinishDate.BeginValue.Value);
            }
            if (workOrderArchiveCriteria.ActuFinishDate.EndValue.HasValue)
            {
                query.Where(p => p.ActuFinishDate <= workOrderArchiveCriteria.ActuFinishDate.EndValue.Value);
            }
            query.Exists<EmployeeEnterprise>((x, y) => y.Where(p => p.EnterpriseId == x.FactoryId && p.EmployeeId == RT.IdentityId));
            return query.OrderByDescending(p => p.No).OrderBy(workOrderArchiveCriteria.OrderInfoList).ToList(workOrderArchiveCriteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
    }
}
