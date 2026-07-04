using SIE.Common;
using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.Web.Command;

namespace SIE.Web.MES.WorkOrders.Commands
{
    /// <summary>
    /// 列表添加工单命令
    /// </summary>
    [JsCommand("SIE.Web.MES.WorkOrders.AddWorkOrderCommand")]
    public class AddWorkOrderCommand : ViewCommand
    {
        public const string FullName = "SIE.Web.MES.WorkOrders.AddWorkOrderCommand";

        /// <summary>
        /// 列表添加工单命令
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            WorkOrder workOrder = args.Data.ToJsonObject<WorkOrder>();
            workOrder.Source = SourceType.Internal;
            workOrder.State = SIE.Core.WorkOrders.WorkOrderState.Release;
            workOrder.KitType = null;
            workOrder.Type = SIE.Core.WorkOrders.WorkOrderType.Mass;
            workOrder.MakerId = RT.IdentityId;
            workOrder.MakeDate = RF.Find<WorkOrder>().GetDbTime();
            workOrder.PlanBeginDate = workOrder.MakeDate.Date;
            workOrder.PlanEndDate = workOrder.MakeDate.Date;
            workOrder.No = RT.Service.Resolve<WorkOrderController>().GetWorkOrderNo();
            workOrder.GenerateId();
            return workOrder;
        }
    }
}
