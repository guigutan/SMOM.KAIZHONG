using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WIP.Products
{
    /// <summary>
    /// 产品工艺路线
    /// </summary>
    [RootEntity, Serializable]
    [Label("产品工艺路线")]
    public partial class WipProductRouting : DataEntity
    {
        #region 生产产品版本 Version
        /// <summary>
        /// 生产产品版本Id
        /// </summary>
        [Label("生产产品版本")]
        public static readonly IRefIdProperty VersionIdProperty = P<WipProductRouting>.RegisterRefId(e => e.VersionId, ReferenceType.Normal);

        /// <summary>
        /// 生产产品版本Id
        /// </summary>
        public double VersionId
        {
            get { return (double)GetRefId(VersionIdProperty); }
            set { SetRefId(VersionIdProperty, value); }
        }

        /// <summary>
        /// 生产产品版本
        /// </summary>
        public static readonly RefEntityProperty<WipProductVersion> VersionProperty = P<WipProductRouting>.RegisterRef(e => e.Version, VersionIdProperty);

        /// <summary>
        /// 生产产品版本
        /// </summary>
        public WipProductVersion Version
        {
            get { return GetRefEntity(VersionProperty); }
            set { SetRefEntity(VersionProperty, value); }
        }
        #endregion

        #region 产品工艺路线布局 Layout
        /// <summary>
        /// 产品工艺路线布局Id
        /// </summary>
        [Label("产品工艺路线布局")]
        public static readonly IRefIdProperty LayoutIdProperty = P<WipProductRouting>.RegisterRefId(e => e.LayoutId, ReferenceType.Normal);

        /// <summary>
        /// 产品工艺路线布局Id
        /// </summary>
        public double LayoutId
        {
            get { return (double)GetRefId(LayoutIdProperty); }
            set { SetRefId(LayoutIdProperty, value); }
        }

        /// <summary>
        /// 产品工艺路线布局
        /// </summary>
        public static readonly RefEntityProperty<WipProductRoutingLayout> LayoutProperty = P<WipProductRouting>.RegisterRef(e => e.Layout, LayoutIdProperty);

        /// <summary>
        /// 产品工艺路线布局
        /// </summary>
        public WipProductRoutingLayout Layout
        {
            get { return GetRefEntity(LayoutProperty); }
            set { SetRefEntity(LayoutProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<WipProductRouting>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId
        {
            get { return (double)GetRefId(WorkOrderIdProperty); }
            set { SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<WipProductRouting>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 产品工艺路线 实体配置
    /// </summary>
    internal class WipProductRoutingConfig : EntityConfig<WipProductRouting>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_PROD_ROUTING").MapAllProperties();
            Meta.Property(WipProductRouting.VersionIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}