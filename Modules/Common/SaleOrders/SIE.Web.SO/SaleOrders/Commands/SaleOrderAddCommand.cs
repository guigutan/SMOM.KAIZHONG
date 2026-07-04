using SIE.SO.SaleOrders;
using SIE.Web.Command;
using System;

namespace SIE.Web.SO.SaleOrders.Commands
{
    /// <summary>
    /// 销售订单添加
    /// </summary>
    public  class SaleOrderAddCommand : ViewCommand
    {
        /// <summary>
        /// 添加逻辑
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var salesOrder = args.Data.ToJsonObject<SaleOrder>();
            salesOrder.Code = RT.Service.Resolve<SaleOrderController>().GetSalesOrderNo();
            salesOrder.TotalQty = 0;
            salesOrder.OrderRowsQty = 0;
            salesOrder.RegisterDateTime = DateTime.Now;
            //数据来源默认自制
            salesOrder.OrderSourceType = OrderSourceType.Manual;
            //客户编号,客户名称,销售人员,备注都为空但是必填
            return salesOrder;
        }
    }
}
