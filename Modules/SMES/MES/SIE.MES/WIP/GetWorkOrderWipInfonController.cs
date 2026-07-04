using SIE.Core.Common;
using SIE.Domain;
using SIE.EventMessages.MES.WIP;
using SIE.EventMessages.MES.WIP.Models;
using SIE.MES.BatchWIP.Products;
using SIE.MES.WIP.Products;
using SIE.MES.WorkOrders;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.WIP
{
    /// <summary>
    /// 批次生产通用报表-QMS-接口实现
    /// </summary>
    public class GetWorkOrderWipInfoController : DomainController, IGetWorkOrderWipInfo
    {
        /// <summary>
        /// 获取正在进行的工单列表
        /// </summary>
        /// <param name="lineIds"></param>
        /// <param name="itemIds"></param>        
        /// <returns></returns>
        [Api.ApiService]
        public virtual List<WorkOrderWipInfo> GetWorkOrderWipInfoList(List<double> lineIds, List<double> itemIds)
        {
            List<WorkOrderWipInfo> workOrderWipInfos = new List<WorkOrderWipInfo>();
            workOrderWipInfos.AddRange(GetBatchWipInfo(lineIds, itemIds));
            workOrderWipInfos.AddRange(GetWipInfo(lineIds, itemIds));
            return workOrderWipInfos;
        }

        private List<WorkOrderWipInfo> GetBatchWipInfo(List<double> lineIds, List<double> itemIds)
        {
            var query = Query<BatchWipProductVersion>()
                .Join<WorkOrder>((batch, workOrder) => batch.WorkOrderId == workOrder.Id)
                .Join<BatchWipProductProcess>((batch, currentProcess) => batch.CurrentProcessId == currentProcess.Id)
                .Where(batch => batch.IsPause == YesNo.No && !batch.IsFinish)
                .Where<WorkOrder>((batch, workOrder) => workOrder.State == Core.WorkOrders.WorkOrderState.Finish
                    || workOrder.State == Core.WorkOrders.WorkOrderState.Producing);

            if (lineIds != null && lineIds.Any())
            {
                var expLineId = lineIds.Select(x=>(double?)x).ToList().CreateContainsExpression1<BatchWipProductVersion, WorkOrder>(
                    typeof(WorkOrder), "batch", "workOrder", nameof(WorkOrder.ResourceId));
                if (expLineId != null)
                {
                    query.Where(expLineId);
                }
            }

            if (itemIds != null && itemIds.Any())
            {
                var expItemId = itemIds.CreateContainsExpression<BatchWipProductVersion, WorkOrder>(
                    typeof(WorkOrder), "batch", "workOrder1", nameof(WorkOrder.ProductId));
                if (expItemId != null)
                {
                    query.Where(expItemId);
                }
            }

            var result = query
                .GroupBy<BatchWipProductProcess, WorkOrder>((batch, currentProcess, workOrder) => new
                {
                    workOrder.Id,
                    currentProcess.ProcessId,
                    workOrder.No,
                    workOrder.ProductId,
                    workOrder.FactoryId,
                    workOrder.WorkShopId,
                    workOrder.ResourceId,
                })
                .Select<BatchWipProductProcess, WorkOrder>((batch, currentProcess, workOrder) => new
                {
                    WorkOrderId = workOrder.Id,
                    WorkOrderNo = workOrder.No,
                    ItemId = workOrder.ProductId,
                    workOrder.FactoryId,
                    workOrder.WorkShopId,
                    LineId = workOrder.ResourceId,
                    currentProcess.ProcessId,
                    qty = batch.Qty.SUM(),
                }).ToList<WorkOrderWipInfo>().ToList();
            return result;
        }

        private List<WorkOrderWipInfo> GetWipInfo(List<double> lineIds, List<double> itemIds)
        {
            var query = Query<WipProductVersion>()
                            .Join<WorkOrder>((wip, workOrder) => wip.WorkOrderId == workOrder.Id)
                            .Join<WipProductProcess>((wip, currentProcess) => wip.CurrentProcessId == currentProcess.Id)
                            .Where(wip => wip.IsPause == YesNo.No && !wip.IsFinish)
                            .Where<WorkOrder>((wip, workOrder) => workOrder.State == Core.WorkOrders.WorkOrderState.Finish
                                || workOrder.State == Core.WorkOrders.WorkOrderState.Producing);

            if (lineIds != null && lineIds.Any())
            {
                var expLineId = lineIds.Select(x => (double?)x).ToList().CreateContainsExpression1<WipProductVersion, WorkOrder>(
                    typeof(WorkOrder), "wip", "workOrder", nameof(WorkOrder.ResourceId));
                if (expLineId != null)
                {
                    query.Where(expLineId);
                }
            }

            if (itemIds != null && itemIds.Any())
            {
                var expItemId = itemIds.CreateContainsExpression<WipProductVersion, WorkOrder>(
                    typeof(WorkOrder), "wip", "workOrder1", nameof(WorkOrder.ProductId));
                if (expItemId != null)
                {
                    query.Where(expItemId);
                }
            }

            var result = query
                .GroupBy<WipProductProcess, WorkOrder>((wip, currentProcess, workOrder) => new
                {
                    workOrder.Id,
                    currentProcess.ProcessId,
                    workOrder.No,
                    workOrder.ProductId,
                    workOrder.FactoryId,
                    workOrder.WorkShopId,
                    workOrder.ResourceId,
                })
                .Select<WipProductProcess, WorkOrder>((wip, currentProcess, workOrder) => new
                {
                    WorkOrderId = workOrder.Id,
                    WorkOrderNo = workOrder.No,
                    ItemId = workOrder.ProductId,
                    workOrder.FactoryId,
                    workOrder.WorkShopId,
                    LineId = workOrder.ResourceId,
                    currentProcess.ProcessId,
                    qty = wip.Id.COUNT(),
                }).ToList<WorkOrderWipInfo>().ToList();
            return result;
        }
    }
}
