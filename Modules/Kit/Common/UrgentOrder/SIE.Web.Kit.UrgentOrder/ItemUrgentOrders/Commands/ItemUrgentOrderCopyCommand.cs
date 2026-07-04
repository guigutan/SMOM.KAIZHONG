using SIE.Kit.UrgentOrder.ItemUrgentOrders;
using SIE.Web.Command;

namespace SIE.Web.Kit.UrgentOrder.ItemUrgentOrders.Commands
{
    /// <summary>
    /// 物料加急单复制新增命令
    /// </summary>
    public class ItemUrgentOrderCopyCommand : ViewCommand
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<ItemUrgentOrder>();
            data.No = RT.Service.Resolve<ItemUrgentOrderController>().GetItemUrgentOrderNo();
            return data;
        }
    }
}
