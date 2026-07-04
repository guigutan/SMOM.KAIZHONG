using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.BatchWIP.Products
{
    /// <summary>
    /// 批次产品运行时工艺路线
    /// 批次产品工艺路线功能使用
    /// </summary>
    [RootEntity, Serializable]
    [Label("批次产品运行时工艺路线")]
    public partial class BatchWipRTProductRouting : DataEntity
    {
        #region 批次号 BatchNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> BatchNoProperty = P<BatchWipRTProductRouting>.Register(e => e.BatchNo);

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo
        {
            get { return this.GetProperty(BatchNoProperty); }
            set { this.SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<BatchWipRTProductRouting>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId
        {
            get { return (double)this.GetRefId(WorkOrderIdProperty); }
            set { this.SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty =
            P<BatchWipRTProductRouting>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion 

        #region 布局 Layout
        /// <summary>
        /// 布局Id
        /// </summary>
        [Label("布局")]
        public static readonly IRefIdProperty LayoutIdProperty =
            P<BatchWipRTProductRouting>.RegisterRefId(e => e.LayoutId, ReferenceType.Normal);

        /// <summary>
        /// 布局Id
        /// </summary>
        public double LayoutId
        {
            get { return (double)this.GetRefId(LayoutIdProperty); }
            set { this.SetRefId(LayoutIdProperty, value); }
        }

        /// <summary>
        /// 布局
        /// </summary>
        public static readonly RefEntityProperty<BatchWipRTProductRoutingLayout> LayoutProperty =
            P<BatchWipRTProductRouting>.RegisterRef(e => e.Layout, LayoutIdProperty);

        /// <summary>
        /// 布局
        /// </summary>
        public BatchWipRTProductRoutingLayout Layout
        {
            get { return this.GetRefEntity(LayoutProperty); }
            set { this.SetRefEntity(LayoutProperty, value); }
        }
        #endregion  
    }

    /// <summary>
	/// 批次产品运行时工艺路线 实体配置
	/// </summary>
	internal class BatchWipRTProductRoutingConfig : EntityConfig<BatchWipRTProductRouting>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("BWIP_PROD_RT_ROUTING").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}