using SIE.Domain;
using SIE.SO.SaleOrders;
using SIE.Web.Command;

namespace SIE.Web.SO.SaleOrders.Commands
{
    /// <summary>
    /// 销售订单保存
    /// </summary>
    public class SaleOrderSaveCommand : SaveCommand
    {
        /// <summary>
        /// 进行保存
        /// </summary>
        /// <param name="data"></param>
        protected override void DoSave(EntityList data)
        {
            RT.Service.Resolve<SaleOrderController>().SaveSaleOrder(data);
        }
    }
}
