using SIE.Core.Enums;
using SIE.ShipPlan;
using SIE.Web.Command;

namespace SIE.Web.ShipPlan.Commands
{
    /// <summary>
    /// 添加命令
    /// </summary>
    public class AddAssignWarehouseRuleCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            if (args == null)
            {
                return null;
            }

            var data = args.Data.ToJsonObject<AssignWarehouseRule>();
            data.OrderType = OrderType.SaleOut;
            data.ItemType = SIE.Items.ItemType.Product;
            data.Priority = 5;
            return data;
        }
    }
}
