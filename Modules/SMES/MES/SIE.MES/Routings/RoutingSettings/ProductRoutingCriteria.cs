using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using SIE.Tech.Routings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Routings.RoutingSettings
{
    [QueryEntity, Serializable]
    public class ProductRoutingCriteria : Criteria
    {
        #region 工单类型 OrderType
        /// <summary>
        /// 工单类型
        /// </summary>
        [Label("工单类型")]
        public static readonly Property<WorkOrderType?> OrderTypeProperty = P<ProductRoutingCriteria>.Register(e => e.OrderType);

        /// <summary>
        /// 工单类型
        /// </summary>
        public WorkOrderType? OrderType
        {
            get { return this.GetProperty(OrderTypeProperty); }
            set { this.SetProperty(OrderTypeProperty, value); }
        }
        #endregion

        #region 产品 Product
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品")]
        public static readonly IRefIdProperty ProductIdProperty =
            P<ProductRoutingCriteria>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double? ProductId
        {
            get { return (double?)this.GetRefNullableId(ProductIdProperty); }
            set { this.SetRefNullableId(ProductIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public static readonly RefEntityProperty<Item> ProductProperty =
            P<ProductRoutingCriteria>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Product
        {
            get { return this.GetRefEntity(ProductProperty); }
            set { this.SetRefEntity(ProductProperty, value); }
        }
        #endregion

        #region 工艺路线 Routing
        /// <summary>
        /// 工艺路线Id
        /// </summary>
        [Label("工艺路线")]
        public static readonly IRefIdProperty RoutingIdProperty =
            P<ProductRoutingCriteria>.RegisterRefId(e => e.RoutingId, ReferenceType.Normal);

        /// <summary>
        /// 工艺路线Id
        /// </summary>
        public double? RoutingId
        {
            get { return (double?)this.GetRefNullableId(RoutingIdProperty); }
            set { this.SetRefNullableId(RoutingIdProperty, value); }
        }

        /// <summary>
        /// 工艺路线
        /// </summary>
        public static readonly RefEntityProperty<Routing> RoutingProperty =
            P<ProductRoutingCriteria>.RegisterRef(e => e.Routing, RoutingIdProperty);

        /// <summary>
        /// 工艺路线
        /// </summary>
        public Routing Routing
        {
            get { return this.GetRefEntity(RoutingProperty); }
            set { this.SetRefEntity(RoutingProperty, value); }
        }
        #endregion

        #region 开始日期 StartDate
        /// <summary>
        /// 开始日期
        /// </summary>
        [Label("开始日期")]
        public static readonly Property<DateRange> StartDateProperty = P<ProductRoutingCriteria>.Register(e => e.StartDate);

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateRange StartDate
        {
            get { return this.GetProperty(StartDateProperty); }
            set { this.SetProperty(StartDateProperty, value); }
        }
        #endregion

        #region 结束日期 EndDate
        /// <summary>
        /// 结束日期
        /// </summary>
        [Label("结束日期")]
        public static readonly Property<DateRange> EndDateProperty = P<ProductRoutingCriteria>.Register(e => e.EndDate);

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateRange EndDate
        {
            get { return this.GetProperty(EndDateProperty); }
            set { this.SetProperty(EndDateProperty, value); }
        }
        #endregion

        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ProductRoutingController>().GetProductRoutings(this);
        }

    }
}
