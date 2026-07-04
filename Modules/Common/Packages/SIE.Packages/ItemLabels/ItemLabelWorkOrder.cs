using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Packages.ItemLabels
{
    /// <summary>
    /// 物料标签投入工单
    /// </summary>
    [ChildEntity, Serializable]
    [Label("物料标签投入工单")]
    public class ItemLabelWorkOrder : DataEntity
    {
        #region 物料标签 ItemLabel
        /// <summary>
        /// 物料标签Id
        /// </summary>
        [Label("物料标签")]
        public static readonly IRefIdProperty ItemLabelIdProperty =
            P<ItemLabelWorkOrder>.RegisterRefId(e => e.ItemLabelId, ReferenceType.Parent);

        /// <summary>
        /// 物料标签Id
        /// </summary>
        public double ItemLabelId
        {
            get { return (double)this.GetRefId(ItemLabelIdProperty); }
            set { this.SetRefId(ItemLabelIdProperty, value); }
        }

        /// <summary>
        /// 物料标签
        /// </summary>
        public static readonly RefEntityProperty<ItemLabel> ItemLabelProperty =
            P<ItemLabelWorkOrder>.RegisterRef(e => e.ItemLabel, ItemLabelIdProperty);

        /// <summary>
        /// 物料标签
        /// </summary>
        public ItemLabel ItemLabel
        {
            get { return this.GetRefEntity(ItemLabelProperty); }
            set { this.SetRefEntity(ItemLabelProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<ItemLabelWorkOrder>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

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
            P<ItemLabelWorkOrder>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<ItemLabelWorkOrder>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 物料标签物料Id ItemId
        /// <summary>
        /// 物料标签物料Id
        /// </summary>
        [Label("物料标签物料Id")]
        public static readonly Property<double> ItemIdProperty
            = P<ItemLabelWorkOrder>.RegisterView(e => e.ItemId, p => p.ItemLabel.ItemId);

        /// <summary>
        /// 物料标签物料Id
        /// </summary>
        public double ItemId
        {
            get { return this.GetProperty(ItemIdProperty); }
        }
        #endregion

        #region 物料标签扩展属性 ItemExtProp
        /// <summary>
        /// 物料标签扩展属性
        /// </summary>
        [Label("物料标签扩展属性")]
        public static readonly Property<string> ItemExtPropProperty
            = P<ItemLabelWorkOrder>.RegisterView(e => e.ItemExtProp, p => p.ItemLabel.ItemExtProp);

        /// <summary>
        /// 物料标签扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get { return this.GetProperty(ItemExtPropProperty); }
        }
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<ItemLabelWorkOrder>.RegisterView(e => e.WorkOrderNo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
        }
        #endregion

    }

    /// <summary>
    /// 物料标签投入工单 实体配置
    /// </summary>
    internal class ItemLabelWorkOrderConfig : EntityConfig<ItemLabelWorkOrder>
    {
        /// <summary>
        /// Meta属性配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ITEM_LABEL_WO").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
