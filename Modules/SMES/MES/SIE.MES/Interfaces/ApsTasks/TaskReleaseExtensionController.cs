using SIE.EventMessages.Release;
using SIE.MES.WorkOrders;

namespace SIE.MES.Interfaces.ApsTasks
{
    /// <summary>
    /// 下达/取消下达扩展控制器类
    /// </summary>
    public class TaskReleaseExtensionController : DomainController
    {
        /// <summary>
        /// 工单扩展属性赋值
        /// </summary>
        /// <param name="workOrder">工单</param>
        /// <param name="curReleasePlanDetail">下达明细</param>
        [IgnoreProxy]
        public virtual void WorkOrderExtensionAssign(WorkOrder workOrder, ReleasePlanDetail curReleasePlanDetail)
        {
        }

        /// <summary>
        /// 工单BOM扩展属性赋值
        /// </summary>
        /// <param name="workOrderBom">工单BOM</param>
        /// <param name="bomDetail">下达BOM明细</param>
        [IgnoreProxy]
        public virtual void WorkOrderBomExtensionAssign(WorkOrderBom workOrderBom, BomDetail bomDetail)
        {
        }

        /// <summary>
        /// 判断上线状态
        /// </summary>
        /// <param name="workOrder">工单</param>
        /// <param name="detailId">明细ID</param>
        [IgnoreProxy]
        public virtual string CheckWorkOrderUplineState(WorkOrder workOrder, string detailId)
        {
            return "";
        }
    }
}