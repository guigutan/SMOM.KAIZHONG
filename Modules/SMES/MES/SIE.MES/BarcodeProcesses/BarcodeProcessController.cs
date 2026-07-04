using SIE.Core.ApiModels;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.BarcodeProcesses.DataModels;
using SIE.MES.WorkOrders;
using SIE.Rbac.Users;
using SIE.Resources.Employees;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.BarcodeProcesses
{
    /// <summary>
    /// 条码工序指派
    /// </summary>
    public class BarcodeProcessController : DomainController
    {
        /// <summary>
        /// 获取明细
        /// </summary>
        /// <param name="barProId"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual EntityList<BarcodeProDetail> GetProDetails(double barProId, PagingInfo pagingInfo)
        {
            return Query<BarcodeProDetail>().Where(p => p.BarcodeProcessId == barProId).OrderBy(p => p.NumberIndex).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取日志
        /// </summary>
        /// <param name="barProId"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<BarcodeProOptLog> GetProLogs(double barProId, PagingInfo pagingInfo)
        {
            return Query<BarcodeProOptLog>().Where(p => p.BarcodeProcessId == barProId).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<BarcodeProcess> QueryBarcodeProcess(BarcodeProcessCriteria criteria)
        {
            List<SIE.Core.WorkOrders.WorkOrderState> states = new List<WorkOrderState> { WorkOrderState.Release, WorkOrderState.Producing };
            if (criteria == null)
            {
                return new EntityList<BarcodeProcess>();
            }
            var query = Query<BarcodeProcess>();
            if (criteria.WorkOrderNo.IsNotEmpty())
            {
                query.Where(p => p.WorkOrder.No.Contains(criteria.WorkOrderNo));
            }
            if (criteria.Sn.IsNotEmpty())
            {
                query.Where(p => p.Sn.Contains(criteria.Sn));
            }
            if (criteria.AssignState.HasValue)
            {
                query.Where(p => p.AssignState == criteria.AssignState);
            }
            if (criteria.CrtTime.BeginValue.HasValue)
            {
                query.Where(p => p.CreateDate >= criteria.CrtTime.BeginValue);
            }
            if (criteria.CrtTime.EndValue.HasValue)
            {
                query.Where(p => p.CreateDate <= criteria.CrtTime.EndValue);
            }
            //if (!criteria.OrderInfoList.Any())
            //{
            //    criteria.OrderInfoList.Add(new OrderInfo() { Property = nameof(BarcodeProcess.AssignStateProperty), SortIndex = 0, SortOrder = System.ComponentModel.ListSortDirection.Ascending });
            //    criteria.OrderInfoList.Add(new OrderInfo() { Property = nameof(BarcodeProcess.AssignStateProperty), SortIndex = 1, SortOrder = System.ComponentModel.ListSortDirection.Ascending });
            //}
            query.Where(p => !p.IsScraped)
                .Exists<WorkOrders.WorkOrder>((x, y) => y.Where(w => x.WorkOrderId == w.Id && states.Contains(w.State)).Exists<EmployeeEnterprise>((a, b) => b.Where(f => a.FactoryId == f.EnterpriseId && f.EmployeeId == RT.IdentityId)));
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 同步工序清单
        /// </summary>
        /// <param name="barcodeId">条码id</param>
        /// <param name="woId">工单id</param>
        public virtual void SynWoProcessListQuery(double barcodeId, double? woId)
        {
            if (woId == null)
            {
                throw new ValidationException("条码工单为空！".L10N());
            }
            // 工单工序清单
            var woProcessList = Query<WorkOrderRoutingProcess>().Where(p => p.WorkOrderId == woId && p.ProcessId != null).Select(p => new { ProcessId = p.ProcessId, Index = p.Index }).ToList<WoProcessData>();
            // 已添加的工序清单
            var hasProDetails = Query<BarcodeProDetail>().Where(p => p.BarcodeProcessId == barcodeId).ToList();
            EntityList<BarcodeProDetail> proDetails = new EntityList<BarcodeProDetail>();
            // 判断是否存在差异
            var hasEdit = false;
            foreach (var woProcess in woProcessList)
            {
                var hasAdd = hasProDetails.FirstOrDefault(p => p.ProcessId == woProcess.ProcessId && p.NumberIndex == woProcess.Index) != null;
                if (hasAdd)
                {
                    continue;
                }
                BarcodeProDetail barcodeProDetail = new BarcodeProDetail
                {
                    BarcodeProcessId = barcodeId,
                    ProcessId = woProcess.ProcessId,
                    NumberIndex = woProcess.Index,
                    IsCheck = true,
                };
                proDetails.Add(barcodeProDetail);
                hasEdit = true;
            }
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                RF.BatchInsert(proDetails);
                if (hasEdit)
                {
                    UpdateBarcodeAssignState(barcodeId);
                }
                tran.Complete();
            }
        }

        /// <summary>
        /// 单体工序查询
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<SingleProcess> QuerySingleProcess(SingleProcessCriteria criteria)
        {
            if (criteria == null)
            {
                return new EntityList<SingleProcess>();
            }
            List<ProcessType?> processTypes = new List<ProcessType?> { ProcessType.Pqc, ProcessType.Fix, ProcessType.Rework, ProcessType.Assembly, ProcessType.Packing, ProcessType.Ageing };
            var query = Query<SingleProcess>();
            if (criteria.Code.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(criteria.Code));
            }
            if (criteria.Name.IsNotEmpty())
            {
                query.Where(p => p.Name.Contains(criteria.Name));
            }
            if (criteria.ProcessType.HasValue)
            {
                query.Where(p => p.Type == criteria.ProcessType.Value);
            }
            if (criteria.ProductFamilyId.HasValue)
            {
                query.Where(p => p.ProductFamilyId == criteria.ProductFamilyId.Value);
            }
            query.Where(p => processTypes.Contains(p.Type));
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 新增工序明细
        /// </summary>
        /// <param name="barcodeId">条码Id</param>
        /// <param name="processIds">工序ids</param>
        /// <returns></returns>
        public virtual EntityList<BarcodeProDetail> AfterAddProDetails(double? barcodeId, List<double?> processIds)
        {
            if (barcodeId == null)
            {
                throw new ValidationException("条码工单为空！".L10N());
            }
            List<BaseDataInfo> processBaseInfos = new List<BaseDataInfo>();
            processIds.SplitDataExecute(tempIds =>
            {
                var list = Query<Process>().Where(p => tempIds.Contains(p.Id)).Select(p => new
                {
                    Id = p.Id,
                    Code = p.Code,
                    Name = p.Name,
                }).ToList<BaseDataInfo>().ToList();
                processBaseInfos.AddRange(list);
            });
            EntityList<BarcodeProDetail> barcodeProDetails = new EntityList<BarcodeProDetail>();
            foreach (var processId in processIds)
            {
                var process = processBaseInfos.FirstOrDefault(p => p.Id == processId);
                if (process == null)
                {
                    continue;
                }
                BarcodeProDetail detail = new BarcodeProDetail
                {
                    BarcodeProcessId = barcodeId.Value,
                    ProcessId = processId.Value,
                    ProcessCode = process.Code,
                    ProcessName = process.Name,
                    IsCheck = true,
                };
                barcodeProDetails.Add(detail);
            }
            return barcodeProDetails;
        }

        /// <summary>
        /// 保存校验
        /// </summary>
        /// <param name="editDetails">新增数据</param>
        public virtual void SaveWoProcessDetailValidate(EntityList<BarcodeProDetail> editDetails)
        {
            if (editDetails == null)
            {
                return;
            }
            // 非空校验
            if (editDetails.Any(p => p.NumberIndex == null))
            {
                throw new ValidationException("工序顺序必填".L10N());
            }

            // 重复校验
            var barcodeIds = editDetails.Select(p => p.BarcodeProcessId).ToList();
            var ids = editDetails.Select(p => p.Id).ToList();

            var baseDetails = Query<BarcodeProDetail>().Where(p => barcodeIds.Contains(p.BarcodeProcessId) && !ids.Contains(p.Id)).ToList();
            baseDetails.AddRange(editDetails);
            if (baseDetails.GroupBy(p => new { p.NumberIndex, p.ProcessId }).Any(p => p.Count() > 1))
            {
                throw new ValidationException("顺序 + 工序重复".L10N());
            }
        }

        /// <summary>
        /// 保存更新主表指派状态
        /// </summary>
        /// <param name="barcodeId">主表id</param>
        public virtual void UpdateBarcodeAssignState(double barcodeId)
        {
            // 主表数据
            var barcode = RF.GetById<BarcodeProcess>(barcodeId);
            // 保存后子表
            var proDetail = Query<BarcodeProDetail>().Where(p => p.BarcodeProcessId == barcodeId).ToList();
            if (proDetail.Count() <= 0)
            {
                barcode.AssignState = Barcodes.Barcodes.Enums.AssignState.UnAssign;
            }
            else
            {
                if (proDetail.All(p => p.EmployeeJoinNames.IsNullOrEmpty()))
                {
                    barcode.AssignState = Barcodes.Barcodes.Enums.AssignState.UnAssign;
                }
                else if (proDetail.Any(p => p.EmployeeJoinNames.IsNullOrEmpty()))
                {
                    barcode.AssignState = Barcodes.Barcodes.Enums.AssignState.PartAssign;
                }
                else
                {
                    barcode.AssignState = Barcodes.Barcodes.Enums.AssignState.Assigned;
                }
            }
            RF.Save(barcode);
        }

        /// <summary>
        /// 创建日志
        /// </summary>
        /// <param name="barcodeId"></param>
        /// <param name="delDetail"></param>
        /// <param name="editDetail"></param>
        public virtual void CreateEmployeeChangeLog(double barcodeId, EntityList<BarcodeProDetail> delDetail, EntityList<BarcodeProDetail> editDetail)
        {
            // 当前时间
            var dbDateTime = RF.Find<BarcodeProOptLog>().GetDbTime();
            // 操作人
            var opterId = RT.IdentityId;
            // 保存前子表
            var beforeSaveDetail = Query<BarcodeProDetail>().Where(p => p.BarcodeProcessId == barcodeId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            EntityList<BarcodeProOptLog> logs = new EntityList<BarcodeProOptLog>();

            // 工序Id
            var processIds = editDetail.Select(p => p.ProcessId).ToList();
            var processList = Query<Process>().Where(p => processIds.Contains(p.Id)).ToList();

            foreach (var item in delDetail)
            {
                var oldItem = beforeSaveDetail.FirstOrDefault(p => p.Id == item.Id);
                if (oldItem == null) return;
                string content = "顺序[{0}]工序[{1}]已删除，员工[{2}]".L10nFormat(oldItem.NumberIndex, oldItem.ProcessCode, oldItem.EmployeeJoinNames);
                BarcodeProOptLog barcodeProOptLog = new BarcodeProOptLog
                {
                    BarcodeProcessId = barcodeId,
                    OptTime = dbDateTime,
                    OpterId = opterId,
                    Content = content,
                };
                logs.Add(barcodeProOptLog);
            }
            foreach (var item in editDetail)
            {
                var oldItem = beforeSaveDetail.FirstOrDefault(p => p.Id == item.Id);
                var process = processList.FirstOrDefault(p => p.Id == item.ProcessId);
                BarcodeProOptLog barcodeProOptLog = new BarcodeProOptLog
                {
                    BarcodeProcessId = barcodeId,
                    OptTime = dbDateTime,
                    OpterId = opterId,
                };
                if (oldItem == null) // 新增
                {
                    string content = "新增顺序[{0}]工序[{1}]员工[{2}]".L10nFormat(item.NumberIndex, process.Code, item.EmployeeJoinNames);
                    barcodeProOptLog.Content = content;
                }
                else
                {
                    string content = "顺序[{0}]工序[{1}]员工[{2}]是否检验[{6}] -> 顺序[{3}]工序[{4}]员工[{5}]是否检验[{7}]".L10nFormat(oldItem.NumberIndex, oldItem.ProcessCode, oldItem.EmployeeJoinNames, item.NumberIndex, process.Code, item.EmployeeJoinNames, oldItem.IsCheck ? "是" : "否", item.IsCheck ? "是" : "否");
                    barcodeProOptLog.Content = content;
                }
                logs.Add(barcodeProOptLog);
            }
            RF.Save(logs);
        }

        /// <summary>
        /// 保存工序明细
        /// </summary>
        /// <param name="delDetail"></param>
        /// <param name="editDetail"></param>
        public virtual void SaveWoProcessDetail(EntityList<BarcodeProDetail> delDetail, EntityList<BarcodeProDetail> editDetail)
        {
            // 主表
            double? barcodeId = 0;
            if (delDetail != null)
            {
                barcodeId = delDetail.FirstOrDefault()?.BarcodeProcessId;
            }
            if (barcodeId == null || barcodeId == 0 && editDetail != null)
            {
                barcodeId = editDetail.FirstOrDefault()?.BarcodeProcessId;
            }
            if (barcodeId == null || barcodeId == 0)
            {
                throw new ValidationException("主表条码信息有误".L10N());
            }
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                // 创建日志
                CreateEmployeeChangeLog(barcodeId.Value, delDetail, editDetail);
                // 保存
                RF.Save(delDetail);
                RF.Save(editDetail);
                // 更新主表状态
                UpdateBarcodeAssignState(barcodeId.Value);
                tran.Complete();
            }
        }

        /// <summary>
        /// 过站验证
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="processId"></param>
        /// <returns>true:允许过站 false:不允许过站</returns>
        public virtual bool MoveBarcodeValiOpter(string barcode, double processId)
        {
            var barcodeProDetail = Query<BarcodeProDetail>().Exists<BarcodeProcess>((x, y) => y.Where(p => x.BarcodeProcessId == p.Id && p.Sn == barcode)).Where(b => b.ProcessId == processId && b.IsCheck).FirstOrDefault();
            if (barcodeProDetail != null && barcodeProDetail.EmployeeIds.Any())
            {
                return barcodeProDetail.EmployeeIds.Contains(RT.IdentityId.ToString());
            }
            else
            {
                return true;
            }
        }
    }
}
