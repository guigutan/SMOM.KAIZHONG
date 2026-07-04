using SIE.MES.WIP;
using SIE.Threading;
using System;
using System.Threading.Tasks;

namespace SIE.MES.WorkOrders
{
    /// <summary>
    /// 工单事件监听器
    /// </summary>
    public class WorkOrderEventListener
    {
        /// <summary>
        /// 实例
        /// </summary>
        public static WorkOrderEventListener Instance { get; } = new WorkOrderEventListener();

        /// <summary>
        /// 发布事件总线
        /// </summary>
        public void Start()
        {
            WorkOrderProductingEvent();
            WipFinishedEvent();
            WipCollectedEvent();
        }

        /// <summary>
        /// 工序过站监听
        /// </summary>
        private void WipCollectedEvent()
        {
            RT.EventBus.Subscribe<WipCollectedEvent>(this, e =>
            {
                //Task.Run(new Action(() =>
                //{
                //    //记录工单物料用料数
                //    RT.Service.Resolve<CallMaterialController>().UpdateMaterialStatistics(e);
                //}).WithCurrentThreadContext());
            });
        }

        /// <summary>
        /// 工单完工事件监听
        /// </summary>
        private void WipFinishedEvent()
        {
            RT.EventBus.Subscribe<WipFinishedEvent>(this, e =>
            {
                //Task.Run(new Action(() =>
                //{
                //    //触发下一生产工单叫料
                //    RT.Service.Resolve<CallMaterialController>().AutoAddCallMaterialBill(e);
                //}).WithCurrentThreadContext());
            });
        }

        /// <summary>
        /// 工单上线事件监听
        /// </summary>
        private void WorkOrderProductingEvent()
        {
            RT.EventBus.Subscribe<WorkOrderProductingEvent>(this, e =>
            {
                //Task.Run(new Action(() =>
                //{
                //    var controller = RT.Service.Resolve<CallMaterialController>();
                //    controller.ReSortInStartProducing(e.WorkOrder.Id);
                //    controller.VirtualStoresReturned(e.WorkOrder.Id, e.ResourceId,  e.StationId);
                //}).WithCurrentThreadContext());
            });
        }
    }
}