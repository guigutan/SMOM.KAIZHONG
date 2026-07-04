using SIE.Barcodes.WipBatchs;
using SIE.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items.Items;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Reports;
using SIE.MES.TaskManagement.SuspectProductLabels;
using SIE.MES.WorkOrders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.TaskManagement.WipProgress
{
    /// <summary>
    /// 控制器
    /// </summary>
    public class WipProgressViewModelController : DomainController
    {

        /// <summary>
        /// 查询在制工序数据
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<WipProgressViewModel> GetList(WipProgressViewModelCriteria criteria)
        {
            if (criteria.BatchNo.IsNullOrEmpty() && criteria.WorkOrderNo.IsNullOrEmpty())
                throw new ValidationException("请输入工单号或标签号");

            var vmList = new EntityList<WipProgressViewModel>();
            WorkOrder wo = null;
            EntityList<ReportRecord> records = new EntityList<ReportRecord>();
            EntityList<ReportWipBatch> recordWipBatchs = new EntityList<ReportWipBatch>();

            // 新增：定义物料相关变量
            string oldItem = string.Empty;
            string parentOldItem = string.Empty;

            if (criteria.BatchNo.IsNotEmpty())
            {
                var wipbatch = RT.Service.Resolve<WipBatchController>().GetWipBatch(criteria.BatchNo);
                if (wipbatch == null)
                    return vmList;
                wo = RF.GetById<WorkOrder>(wipbatch.WorkOrderId);
                if (wo == null)
                    return vmList;

                recordWipBatchs = Query<ReportWipBatch>().Where(p => p.WipBatch.BatchNo == criteria.BatchNo).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                var recordIds = recordWipBatchs.Select(p => p.ReportRecordId).ToList();
                records = Query<ReportRecord>().Where(p => recordIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            }
            else if (criteria.WorkOrderNo.IsNotEmpty())
            {
                wo = RT.Service.Resolve<WorkOrderController>().GetWorkOrder(criteria.WorkOrderNo);
                if (wo == null)
                    return vmList;
                recordWipBatchs = Query<ReportWipBatch>().Where(p => p.ReportRecord.WorkOrderId == wo.Id).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                records = Query<ReportRecord>().Where(p => p.WorkOrderId == wo.Id).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            }

            // 新增：查询物料信息
            if (wo != null && wo.ProductId > 0)
            {
                // 查询当前物料信息
                var item = RF.GetById<SIE.Items.Item>(wo.ProductId);
                if (item != null)
                {
                    oldItem = item.ShortDescription; // 旧物料号

                    // 查询父级物料信息
                    var parentItem = Query<ParentItem>().Where(p => p.ItemId == item.Id).FirstOrDefault();
                    if (parentItem != null)
                    {
                        parentOldItem = parentItem.Bismt; // 父级旧物料号
                    }
                }
            }

            var tasks = RT.Service.Resolve<DispatchController>().GetDispatchTasksByWorkOrderIds(new List<double>() { wo.Id });

            //根据报工记录,按工序汇总
            var groups = records.GroupBy(p => new { p.Seq, p.ProcessId, p.ProcessCode, p.ProcessName, p.DispatchTaskQty, p.TaskStatus }).OrderBy(p => p.Key.Seq);

            //统计每个工序的生产数量
            var layoutInfos = wo.LayoutInfoList.OrderBy(p => int.Parse(p.Vornr));
            foreach (var layoutInfo in layoutInfos)
            {
                var list = new List<ReportRecord>();
                var taskList = tasks.Where(p => p.ProcessCode == layoutInfo.ProcessCode && p.IsSchedulingInfReturn != YesNo.Yes).ToList();
                if (taskList.Count == 0)
                    continue;
                var startProcessCode = taskList.FirstOrDefault(p => p.StartProcess == true)?.ProcessCode;
                var g = groups.FirstOrDefault(p => p.Key.ProcessCode == layoutInfo.ProcessCode);
                if (g != null)
                    list = g.ToList();
                var vm = new WipProgressViewModel()
                {
                    Id = layoutInfo.Id.ToString(),
                    WorkOrderNo = wo.No,
                    ProcessSeq = layoutInfo.Vornr,
                    ProcessId = g?.Key.ProcessId ?? 0,
                    ProcessCode = layoutInfo.ProcessCode,
                    ProcessName = g?.Key.ProcessName ?? layoutInfo.ProcessCode,
                    IsStartProcess = layoutInfo.ProcessCode == startProcessCode,
                    ProcessStatus = taskList.All(p => p.TaskStatus == DispatchTaskStatus.Finished) ? "已完成" : "执行中",
                    PlanQty = taskList.Sum(p => p.DispatchQty),
                    OkQty = taskList.Sum(p => p.OkQty),
                    FinishQty = recordWipBatchs.Where(p => p.ProcessCode == g?.Key.ProcessCode).Sum(p => p.Qty),
                    BatchNo = g != null && criteria.BatchNo.IsNotEmpty() ? criteria.BatchNo : "",
                    // 新增：设置旧物料号和父级旧物料号
                    OldItem = oldItem,
                    ParentOldItem = parentOldItem,
                };
                vmList.Add(vm);
            }

            //计算前置工序完工数量
            foreach (var vm in vmList)
            {
                if (vm.PlanQty == 0)
                    continue;
                var pre = vmList.LastOrDefault(p => p.PlanQty > 0 && int.Parse(p.ProcessSeq) < int.Parse(vm.ProcessSeq));
                if (pre == null)
                {
                    vm.PreFinishQty = vm.FinishQty;
                }
                else
                {
                    vm.PreProcessCode = pre.ProcessCode;
                    vm.PreFinishQty = pre.FinishQty;
                    vm.PreOkQty = pre.OkQty;
                }
            }
            vmList.SetTotalCount(vmList.Count);
            return vmList;
        }

        /// <summary>
        /// 查询标签列表
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<WipProgressWipBatch> GetWipProgressWipBatchs(WipProgressWipBatchCriteria criteria)
        {
            var batchList = new EntityList<WipProgressWipBatch>();

            var q = Query<WipProgressWipBatch>();
            if (criteria.WorkOrderNo.IsNotEmpty())
                q.Where(p => p.WorkOrder.No.Contains(criteria.WorkOrderNo));
            if (criteria.BatchNo.IsNotEmpty())
                q.Where(p => p.BatchNo.Contains(criteria.BatchNo));

            batchList = q.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            if (batchList.Count == 0)
                return batchList;

            var woId = batchList.FirstOrDefault().WorkOrderId;
            var tasks = RT.Service.Resolve<DispatchController>().GetDispatchTasksByWorkOrderIds(new List<double>() { woId }).Where(p => p.IsSchedulingInfReturn != YesNo.Yes).ToList();
            var taskIds = tasks.Select(p => p.Id).ToList();   //需要显示的任务数据
            var currTask = tasks.FirstOrDefault(p => p.ProcessCode == criteria.ProcessCode);    //当前工序任务;
            if (currTask != null)
                taskIds = tasks.Where(p => p.Seq <= currTask.Seq).Select(p => p.Id).ToList();   //只显示当前任务及前置任务数据

            var recordWipBatchs = Query<ReportWipBatch>().Where(p => p.ReportRecord.WorkOrderId == woId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var suspectLabels = Query<SuspectProductLabel>().Where(p => p.WorkOrderId == woId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var batchNos = suspectLabels.Where(p => taskIds.Contains(p.DispatchTaskId ?? 0)).Select(p => p.BatchNo).Distinct().ToList();
            var batchIds = recordWipBatchs.Where(p => taskIds.Contains(p.DispatchTaskId)).Select(p => p.WipBatchId).Distinct().ToList();    //只显示该任务范围的标签数据
            batchList = batchList.Where(p => (batchIds.Contains(p.Id) || batchNos.Contains(p.BatchNo))).OrderBy(p => p.BatchNo).AsEntityList();
            foreach (var batch in batchList)
            {
                batch.IsSuspectProduct = batch.IsSuspectProduct == YesNo.Yes ? YesNo.Yes : null;
                batch.ReportQty = recordWipBatchs.Where(p => p.WipBatchId == batch.Id && p.ProcessCode == criteria.ProcessCode).Sum(p => p.Qty);
                if (batch.IsScraped || batch.IsSuspectProduct == YesNo.Yes)
                {
                    //batch.PreReportQty = batch.Qty;
                }
                else
                {
                    if (criteria.PreProcessCode.IsNotEmpty())
                        batch.PreReportQty = recordWipBatchs.Where(p => p.WipBatchId == batch.Id && p.ProcessCode == criteria.PreProcessCode).Sum(p => p.Qty);
                    else
                        batch.PreReportQty = batch.ReportQty;
                }

            }

            batchList.SetTotalCount(batchList.Count);
            return batchList;
        }

    }
}
