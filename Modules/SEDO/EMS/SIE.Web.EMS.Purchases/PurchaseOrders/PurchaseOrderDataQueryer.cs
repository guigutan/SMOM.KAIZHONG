using SIE.EMS.Purchases.PurchaseOrders;
using SIE.Web.Data;

namespace SIE.Web.EMS.Purchases.PurchaseOrders
{
    /// <summary>
    /// 采购订单查询器
    /// </summary>
    public class PurchaseOrderDataQueryer : DataQueryer
    {
        /// <summary>
        /// 创建一个新的采购订单
        /// </summary>
        /// <returns>新的采购订单</returns>
        public PurchaseOrder GetNewPurchaseOrder()
        {
            return RT.Service.Resolve<PurchaseOrderController>().GetNewPurchaseOrder();
        }
    }
}
