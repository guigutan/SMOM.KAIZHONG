using SIE.MES.WorkOrders;
using SIE.Web.Command;
using SIE.Web.MES.WorkOrders.ViewModels;

namespace SIE.Web.MES.WorkOrders.Commands
{
    /// <summary>
    /// 恢复工单
    /// </summary>   
    [JsCommand("SIE.Web.MES.WorkOrders.WorkOrderResumeCommand")]
    public class WorkOrderResumeCommand : ViewCommand
    {
        /// <summary>
        /// 恢复工单执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var m = args.Data.ToJsonObject<WorkOrderChangeStatus>();
            RT.Service.Resolve<WorkOrderController>().Resume(m.WorkOrderId, m.Reason);
            return true;
        }
    }

    /// <summary>
    /// 暂停工单
    /// </summary>    
    [JsCommand("SIE.Web.MES.WorkOrders.WorkOrderPauseCommand")]
    public class WorkOrderPauseCommand : ViewCommand
    {
        /// <summary>
        /// 暂停工单执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var m = args.Data.ToJsonObject<WorkOrderChangeStatus>();
            RT.Service.Resolve<WorkOrderController>().Pause(m.WorkOrderId, m.Reason);
            return true;
        }
    }

    /// <summary>
    /// 关闭工单
    /// </summary>  
    [JsCommand("SIE.Web.MES.WorkOrders.WorkOrderCloseCommand")]
    public class WorkOrderCloseCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        protected override object Excute(ViewArgs args, string scope)
        {
            var m = args.Data.ToJsonObject<WorkOrderChangeStatus>();
            RT.Service.Resolve<WorkOrderController>().Close(m.WorkOrderId, m.Reason);
            return true;
        }
    }

    /// <summary>
    /// 提前完工
    /// </summary>
    [JsCommand("SIE.Web.MES.WorkOrders.WorkOrderAdvanceCommand")]
    public class WorkOrderAdvanceCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        protected override object Excute(ViewArgs args, string scope)
        {
            var m = args.Data.ToJsonObject<WorkOrderChangeStatus>();
            RT.Service.Resolve<WorkOrderController>().Advance(m.WorkOrderId, m.Reason);
            return true;
        }
    }
}