using SIE.Domain;
using SIE.EMS.Purchases.PurchaseOrders;
using SIE.Web.Command;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.Purchases.PurchaseOrders.Commands
{
    /// <summary>
    /// 删除采购订单
    /// </summary>
    [JsCommand("SIE.Web.EMS.Purchases.PurchaseOrders.Commands.DeletePurOrderCommand")]
    public class DeletePurOrderCommand : DeleteCommand
    {
        /// <summary>
        /// 保存前动作
        /// </summary>
        /// <param name="data"></param>
        protected override void OnSaving(EntityList data)
        {
            base.OnSaving(data);
            var ids = new List<double>();
            if (data != null)
            {
                data.DeletedList.ForEach(p =>
                {
                    var model = p as PurchaseOrder;
                    if (model != null)
                    {
                        ids.Add(model.Id);
                    }
                });
            }
            RT.Service.Resolve<PurchaseOrderController>().DeletePurchaseOrder(ids);
        }
    }
}
