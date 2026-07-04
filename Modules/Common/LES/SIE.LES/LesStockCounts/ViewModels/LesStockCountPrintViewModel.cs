using SIE.Core.Enums;
using SIE.Domain;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.LES.LesStockCounts.ViewModels
{
    /// <summary>
    /// 线边仓盘点打印
    /// </summary>
    [Serializable, RootEntity]
    [Label("线边仓盘点打印")]
    public class LesStockCountPrintViewModel : BillPrintViewModel
    {
        #region 订单类型 OrderType
        /// <summary>
        /// 订单类型
        /// </summary>
        [Label("订单类型")]
        public static readonly Property<OrderType?> OrderTypeProperty = P<LesStockCountPrintViewModel>.Register(e => e.OrderType);

        /// <summary>
        /// 订单类型
        /// </summary>
        public OrderType? OrderType
        {
            get { return this.GetProperty(OrderTypeProperty); }
            set { this.SetProperty(OrderTypeProperty, value); }
        }
        #endregion
    }
}