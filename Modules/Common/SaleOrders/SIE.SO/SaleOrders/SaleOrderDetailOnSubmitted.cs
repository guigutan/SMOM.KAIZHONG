using SIE.Domain;
using System.ComponentModel;

namespace SIE.SO.SaleOrders
{
    /// <summary>
    /// 销售订单明细提交后事件
    /// </summary>
    [DisplayName("销售订单明细提交后事件")]
    [Description("订单明细反写特殊工艺字段")]
    public class SaleOrderDetailOnSubmitted : OnSubmitted<SaleOrderDetail>
    {
        /// <summary>
        /// 重写实现业务
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        protected override void Invoke(SaleOrderDetail entity, EntitySubmittedEventArgs e)
        {
            RT.Service.Resolve<SaleOrderDetailController>().SaveSpecialProcessStr(entity);
        }
    }
}
