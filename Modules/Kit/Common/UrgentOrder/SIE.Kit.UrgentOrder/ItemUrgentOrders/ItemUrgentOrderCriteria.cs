using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Kit.UrgentOrder.ItemUrgentOrders
{
    /// <summary>
    /// 查询实体
    /// </summary>
    [RootEntity, Serializable]
    public class ItemUrgentOrderCriteria : Criteria
    {
        #region 加急单号 No
        /// <summary>
        /// 加急单号
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("加急单号")]
        public static readonly Property<string> NoProperty = P<ItemUrgentOrderCriteria>.Register(e => e.No);

        /// <summary>
        /// 加急单号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 加急单状态 OrderState
        /// <summary>
        /// 加急单状态
        /// </summary>
        [Label("加急单状态")]
        public static readonly Property<UrgentOrderState?> OrderStateProperty = P<ItemUrgentOrderCriteria>.Register(e => e.OrderState);

        /// <summary>
        /// 加急单状态
        /// </summary>
        public UrgentOrderState? OrderState
        {
            get { return GetProperty(OrderStateProperty); }
            set { SetProperty(OrderStateProperty, value); }
        }
        #endregion

        #region 创建时间 CreateDate
        /// <summary>
        /// 创建时间
        /// </summary>
        [Label("创建时间")]
        public static readonly Property<DateRange> CreateDateProperty = P<ItemUrgentOrderCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateRange CreateDate
        {
            get { return this.GetProperty(CreateDateProperty); }
            set { this.SetProperty(CreateDateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns>制程路线列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ItemUrgentOrderController>().GetItemUrgentOrderList(this);
        }
    }
}
