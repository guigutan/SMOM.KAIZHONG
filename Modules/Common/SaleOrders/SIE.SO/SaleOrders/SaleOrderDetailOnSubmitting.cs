using SIE.Domain;
using System.Collections.Generic;
using System.ComponentModel;

namespace SIE.SO.SaleOrders
{
    /// <summary>
    /// 销售订单明细提交前事件，修改销售订单明细记录修改日志
    /// </summary>
    [DisplayName("销售订单明细提交前事件")]
    [Description("修改销售订单明细记录修改日志")]
    public class SaleOrderDetailOnSubmitting : OnSubmitting<SaleOrderDetail>
    {
        /// <summary>
        /// 销售订单明细提交前事件逻辑
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        protected override void Invoke(SaleOrderDetail entity, EntitySubmittingEventArgs e)
        {
            List<SaleOrderDetail> List = new List<SaleOrderDetail>() {
                entity
            };
            RT.Service.Resolve<SaleOrderController>().GetSalesOrderDetailWriteLog(List);
        }

       
    }
}
