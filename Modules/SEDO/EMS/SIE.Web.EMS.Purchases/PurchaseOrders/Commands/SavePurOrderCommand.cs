using SIE.EMS.Purchases.PurchaseOrders;
using SIE.Web.Command;

namespace SIE.Web.EMS.Purchases.PurchaseOrders.Commands
{
    /// <summary>
    /// 保存采购订单
    /// </summary>
    [JsCommand("SIE.Web.EMS.Purchases.PurchaseOrders.Commands.SavePurOrderCommand")]
    public class SavePurOrderCommand : FormSaveCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var list = GetDeserializeData(args, scope);
            var entity = list.Count > 0 ? list[0] : null;
            if (entity != null)
            {
                var pur = entity as PurchaseOrder;
                RT.Service.Resolve<PurchaseOrderController>().SavePurchaseOrder(pur);
            }
            return entity;
        }
    }
}
