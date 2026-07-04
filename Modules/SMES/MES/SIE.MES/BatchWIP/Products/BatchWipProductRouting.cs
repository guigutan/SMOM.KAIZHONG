using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.BatchWIP.Products
{
    /// <summary>
    /// 批次产品工艺路线
    /// </summary>
    [RootEntity, Serializable]
    [Label("批次产品工艺路线")]
    public partial class BatchWipProductRouting : DataEntity
    {
        #region 批次关系 Relation
        /// <summary>
        /// 批次关系Id
        /// </summary>
        [Label("批次关系")]
        public static readonly IRefIdProperty RelationIdProperty = P<BatchWipProductRouting>.RegisterRefId(e => e.RelationId, ReferenceType.Normal);

        /// <summary>
        /// 批次关系Id
        /// </summary>
        public double RelationId
        {
            get { return (double)GetRefId(RelationIdProperty); }
            set { SetRefId(RelationIdProperty, value); }
        }

        /// <summary>
        /// 批次关系
        /// </summary>
        public static readonly RefEntityProperty<BatchRelation> RelationProperty = P<BatchWipProductRouting>.RegisterRef(e => e.Relation, RelationIdProperty);

        /// <summary>
        /// 批次关系
        /// </summary>
        public BatchRelation Relation
        {
            get { return GetRefEntity(RelationProperty); }
            set { SetRefEntity(RelationProperty, value); }
        }
        #endregion

        #region 产品工艺路线布局 Layout
        /// <summary>
        /// 产品工艺路线布局Id
        /// </summary>
        [Label("产品工艺路线布局")]
        public static readonly IRefIdProperty LayoutIdProperty = P<BatchWipProductRouting>.RegisterRefId(e => e.LayoutId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<BatchWipProductRoutingLayout> LayoutProperty = P<BatchWipProductRouting>.RegisterRef(e => e.Layout, LayoutIdProperty);

        /// <summary>
        /// 产品工艺路线布局
        /// </summary>
        public BatchWipProductRoutingLayout Layout
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
        public static readonly IRefIdProperty WorkOrderIdProperty = P<BatchWipProductRouting>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<BatchWipProductRouting>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

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
	/// 批次产品工艺路线 实体配置
	/// </summary>
	internal class BatchWipProductRoutingConfig : EntityConfig<BatchWipProductRouting>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("BWIP_PROD_ROUTING").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}