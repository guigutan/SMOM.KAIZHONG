using SIE.EventMessages.MES.WorkOrders.Models;
using SIE.MES.WorkOrders;
using SIE.Tech.Routings.Technologys;
using SIE.Web.Command;

namespace SIE.Web.MES.WorkOrders.Commands
{
    /// <summary>
    /// 修改工艺路线
    /// </summary>
    [JsCommand("SIE.Web.MES.WorkOrders.Commands.UpdateRoutingCommand")]
    public class UpdateRoutingCommand : ViewCommand
    {
        /// <summary>
        /// 修改工艺路线执行方法
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>保存结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var res = args.Data.ToJsonObject<WorkOrderLayout>();
            var layout = res.Layout;
            IContainer container = new ContainerModel();
            container.Deserialize(layout);
            container.ValidateSave();
            RT.Service.Resolve<WorkOrderController>().UpdateLayout(res.WorkOrderId, layout);
            return true;
        }

        /// <summary>
        /// 工单工艺路线
        /// </summary>
        class WorkOrderLayout
        {
            /// <summary>
            /// 工单ID （此处不能删掉set;否则序列化错误）
            /// </summary>
#pragma warning disable S1144 // Unused private types or members should be removed
            public double WorkOrderId { get; set; }
#pragma warning restore S1144 // Unused private types or members should be removed

            /// <summary>
            /// 工单号 （此处不能删掉set;否则序列化错误）
            /// </summary>
#pragma warning disable S1144 // Unused private types or members should be removed
            public string WorkOrderNo { get; set; }
#pragma warning restore S1144 // Unused private types or members should be removed

            /// <summary>
            /// 布局（此处不能删掉set;否则序列化错误）
            /// </summary>
#pragma warning disable S1144 // Unused private types or members should be removed
            public string Layout { get; set; }
#pragma warning restore S1144 // Unused private types or members should be removed
        }
    }
}