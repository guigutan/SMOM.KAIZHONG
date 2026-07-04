using SIE.Kit.UrgentOrder.ItemUrgentOrders;
using SIE.Web.Command;
using System;

namespace SIE.Web.Kit.UrgentOrder.ItemUrgentOrders.Commands
{
    /// <summary>
    /// 物料加急单添加命令
    /// </summary>
    public class ItemUrgentOrderAddCommand : ViewCommand
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<ItemUrgentOrder>();
            data.No = RT.Service.Resolve<ItemUrgentOrderController>().GetItemUrgentOrderNo();
            data.OrderState = UrgentOrderState.Valid;
            data.Qty = 1000;
            data.DemandTime = DateTime.Now;
            data.CreateDate = DateTime.Now;
            return data;
        }
    }
}
