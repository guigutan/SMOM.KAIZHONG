using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.Web.Command;
using System;

namespace SIE.Web.MES.WorkOrders.Commands
{
    /// <summary>
    /// 复制工单命令
    /// </summary>
    [JsCommand("SIE.Web.MES.WorkOrders.CopyWorkOrderCommand")]
    public class CopyWorkOrderCommand : ViewCommand
    {
        public const string FullName = "SIE.Web.MES.WorkOrders.CopyWorkOrderCommand";

        /// <summary>
        /// 列表复制添加工单命令
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>工单</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var workOrder = args.Data.ToJsonObject<WorkOrder>();
            workOrder.MakerId = RT.IdentityId;
            workOrder.MakeDate = RF.Find<WorkOrder>().GetDbTime();
            workOrder.No = RT.Service.Resolve<WorkOrderController>().GetWorkOrderNo();
            workOrder.PlanBeginDate = DateTime.Parse(workOrder.MakeDate.ToShortDateString());
            workOrder.PlanEndDate = workOrder.PlanBeginDate;
            workOrder.WorkOrderMpType = null;
            var labelTemplate = new SIE.Core.Items.LabelPrintTemplate();
            labelTemplate.GenerateId();
            workOrder.TemplateId = labelTemplate.Id;
            workOrder.GenerateId();
            return workOrder;
        }
    }
}
