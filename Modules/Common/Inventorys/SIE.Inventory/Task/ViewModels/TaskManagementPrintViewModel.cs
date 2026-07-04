using SIE.Common.Prints;
using SIE.Common.Properties;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.Inventory.Task.ViewModels
{
    /// <summary>
    /// 任务管理打印
    /// </summary>
    [Serializable, RootEntity]
    [Label("任务管理打印")]
    public class TaskManagementPrintViewModel : BillPrintViewModel
    {
        #region 订单类型 OrderType
        /// <summary>
        /// 订单类型
        /// </summary>
        [Label("订单类型")]
        public static readonly Property<OrderType?> OrderTypeProperty = P<TaskManagementPrintViewModel>.Register(e => e.OrderType);

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