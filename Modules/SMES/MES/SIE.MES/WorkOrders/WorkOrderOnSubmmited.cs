using SIE.Domain;
using SIE.EventMessages.MES.WorkOrders.Models;
using SIE.Tech.Routings;

namespace SIE.MES.WorkOrders
{
    /// <summary>
    /// 提交后事件
    /// </summary>
    public class WorkOrderOnSubmmited : OnSubmitted<WorkOrder>
    {
        /// <summary>
        /// 实现保存生产批次
        /// </summary>
        /// <param name="entity">工单实体</param>
        /// <param name="e">参数</param>
        protected override void Invoke(WorkOrder entity, EntitySubmittedEventArgs e)
        {
            if (e.Action == SubmitAction.Update)
            {
                UpdateWorkOrderBatch(entity);
            }

            //推送工单变更消息到边端
            RT.EventBus.Publish<WorkOrderInfo>(new WorkOrderInfo() { WorkOrderId = entity.Id, WorkOrderNo = entity.No, MsgType = "1" });

            if (e.Action == SubmitAction.Delete && entity.VersionId.HasValue)
            {
                RT.Service.Resolve<RoutingController>().UpdateVersionRefTimes(entity.VersionId.Value, -1);
            }
        }

        /// <summary>
        /// 更新工单批次
        /// </summary>
        /// <param name="entity">工单</param>
        private void UpdateWorkOrderBatch(WorkOrder entity)
        {
            var batch = WoWipBatchExt.GetAttacWoWipBatch(entity);
            if (batch != null)
            {
                if (batch.PersistenceStatus == PersistenceStatus.Unchanged)
                    batch.PersistenceStatus = entity.PersistenceStatus;
                batch.WorkOrderId = entity.Id;
                RF.Save(batch);
            }
        }

    }
}