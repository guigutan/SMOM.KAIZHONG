using SIE.Common.Sort;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.MES.WorkOrders.Events;
using System.Linq;

namespace SIE.MES.WorkOrders
{
    /// <summary>
    /// 工单帮助类
    /// </summary>
    [IgnoreProxy]
    public class WorkOrderHelper : DomainController
    {
        /// <summary>
        /// 能否恢复工单
        /// </summary>
        /// <param name="workOrder">工单</param>
        /// <returns>bool</returns>
        public virtual bool CanResume(WorkOrder workOrder)
        {
            if (workOrder == null || workOrder.IsPause == YesNo.No || workOrder.State == WorkOrderState.CancelRelease)
                return false;
            return workOrder.State == WorkOrderState.Release || workOrder.State == WorkOrderState.Producing;
        }

        /// <summary>
        /// 能否暂停工单
        /// </summary>
        /// <param name="wokerOrder">工单</param>
        /// <returns>bool</returns>
        public virtual bool CanPause(WorkOrder wokerOrder)
        {
            return wokerOrder != null && wokerOrder.IsPause == YesNo.No && (wokerOrder.State == WorkOrderState.Release || wokerOrder.State == WorkOrderState.Producing);
        }

        /// <summary>
        /// 能否强制关闭工单
        /// </summary>
        /// <param name="wokerOrder">工单</param>
        /// <returns>bool</returns>
        public virtual bool CanClose(WorkOrder wokerOrder)
        {
            if (wokerOrder == null)
                return false;
            if (wokerOrder.State == WorkOrderState.Close || wokerOrder.State == WorkOrderState.CancelRelease)
                return false;
            return wokerOrder.IsPause == YesNo.Yes || wokerOrder.State == WorkOrderState.Finish;
        }

        /// <summary>
        /// 能否取消发放工单
        /// </summary>
        /// <param name="workerOrder">工单</param>
        /// <returns>true/false</returns>
        public virtual bool CanCancelRelease(WorkOrder workerOrder)
        {
            if (workerOrder == null)
                return false;
            return workerOrder.State == WorkOrderState.Release;
        }

        /// <summary>
        /// 复制工单
        /// </summary>
        /// <param name="workOrderId">工单</param>
        /// <param name="targetWorkOrder">目标工单</param>
        public virtual void CopyWorkOrder(double workOrderId, WorkOrder targetWorkOrder)
        {
            var workOrder = RF.Find<WorkOrder>().GetById(workOrderId) as WorkOrder;
            if (workOrder == null)
                throw new EntityNotFoundException(typeof(WorkOrder), workOrderId);
            targetWorkOrder.Clone(workOrder, CloneOptions.NewComposition());
            targetWorkOrder.WorkOrderLogList.Clear();

            //生成子从孙的Id
            foreach (var bom in targetWorkOrder.BomList)
            {
                bom.GenerateId();
                bom.WorkOrderId = targetWorkOrder.Id;               
            }

            foreach (var routingProcess in targetWorkOrder.RoutingProcessList)
            {
                routingProcess.GenerateId();
                foreach (var bomConfig in routingProcess.BomConfigList)
                {
                    bomConfig.GenerateId();
                    bomConfig.ProcessId = routingProcess.Id;
                }

                foreach (var parameter in routingProcess.ParameterList)
                {
                    parameter.GenerateId();
                    parameter.ProcessId = routingProcess.Id;
                }

                routingProcess.WorkOrderId = targetWorkOrder.Id;
            }

            foreach (var processBom in targetWorkOrder.ProcessBomList)
            {
                processBom.GenerateId();
                processBom.WorkOrderId = targetWorkOrder.Id;
               
            }

            double index = 1;
            foreach (var rule in targetWorkOrder.PackageRuleDetailList)
            {
                rule.GenerateId();
                rule.SetIndex(index++);
                rule.WorkOrderId = targetWorkOrder.Id;
            }

            if (targetWorkOrder.VersionId.HasValue && targetWorkOrder.Version != null)
            {
                var workOrderPropertyChanged = RT.Service.Resolve<ErpWorkOrderPropertyChanged>();
                var routingProcesses = workOrder.Version.ProcessList;
                var routingProcessParameters = workOrder.Version.ProcessList.SelectMany(p => p.ParameterList);
                workOrderPropertyChanged.GenerateParameterNextProcess(routingProcessParameters,
                    routingProcesses, targetWorkOrder.RoutingProcessList);
            }
        }
    }
}
