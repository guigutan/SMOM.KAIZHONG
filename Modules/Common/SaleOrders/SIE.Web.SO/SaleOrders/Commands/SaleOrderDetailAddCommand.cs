using SIE.Items;
using SIE.Items.Units;
using SIE.SO.SaleOrders;
using SIE.Web.Command;
using System;


namespace SIE.Web.SO.SaleOrders.Commands
{
    /// <summary>
    /// 销售订单明细添加
    /// </summary>
    public class SaleOrderDetailAddCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var SalesOrderDetail = args.Data.ToJsonObject<SaleOrderDetail>();
            SalesOrderDetail.Qty = 1;
            SalesOrderDetail.LineState =  LineState.NEW;
            SalesOrderDetail.MiDateTime = DateTime.Now;
            SalesOrderDetail.MaterialPnl = 1;
            SalesOrderDetail.SetPnl = 1;
            SalesOrderDetail.PcsPnl = 1;
            SalesOrderDetail.RequireDelivery = DateTime.Today.AddDays(1);
            SalesOrderDetail.IsHangUp = false;
            SalesOrderDetail.Area = 1;
            SalesOrderDetail.SingleArea = 1;
            Unit unit = RT.Service.Resolve<UnitsController>().GetUnitFromCode("PCS");
            if (unit != null)
            {
                SalesOrderDetail.Unit = unit;
                SalesOrderDetail.ExtValues[SaleOrderDetail.UnitIdProperty.Name + "_Display"] = unit.Name;
            }
            //poOrder.ExtValues[ProductOrder.TopOrderIdProperty.Name + "_Display"] = poOrder.Code;
            //客户编号,客户名称,销售人员,备注都为空但是必填
            return SalesOrderDetail;
        }
    }
}
