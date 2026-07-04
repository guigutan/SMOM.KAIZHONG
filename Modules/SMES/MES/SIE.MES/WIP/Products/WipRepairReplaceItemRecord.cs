using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WIP.Products
{
    /// <summary>
    /// 维修换料记录
    /// </summary>
    [RootEntity, Serializable]
    [Label("维修换料记录")]
    public class WipRepairReplaceItemRecord : DataEntity
    {
        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<WipRepairReplaceItemRecord>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

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
            P<WipRepairReplaceItemRecord>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 产品条码 Sn
        /// <summary>
        /// 产品条码
        /// </summary>
        [Label("产品条码")]
        public static readonly Property<string> SnProperty = P<WipRepairReplaceItemRecord>.Register(e => e.Sn);

        /// <summary>
        /// 产品条码
        /// </summary>
        public string Sn
        {
            get { return this.GetProperty(SnProperty); }
            set { this.SetProperty(SnProperty, value); }
        }
        #endregion

        #region 旧标签 OldLabel
        /// <summary>
        /// 旧标签
        /// </summary>
        [Label("旧标签")]
        public static readonly Property<string> OldLabelProperty = P<WipRepairReplaceItemRecord>.Register(e => e.OldLabel);

        /// <summary>
        /// 旧标签
        /// </summary>
        public string OldLabel
        {
            get { return this.GetProperty(OldLabelProperty); }
            set { this.SetProperty(OldLabelProperty, value); }
        }
        #endregion

        #region 标签号 Label
        /// <summary>
        /// 标签号
        /// </summary>
        [Label("标签号")]
        public static readonly Property<string> LabelProperty = P<WipRepairReplaceItemRecord>.Register(e => e.Label);

        /// <summary>
        /// 标签号
        /// </summary>
        public string Label
        {
            get { return this.GetProperty(LabelProperty); }
            set { this.SetProperty(LabelProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<WipRepairReplaceItemRecord>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 维修位置 RepairLocation
        /// <summary>
        /// 维修位置
        /// </summary>
        [Label("维修位置")]
        public static readonly Property<string> RepairLocationProperty = P<WipRepairReplaceItemRecord>.Register(e => e.RepairLocation);

        /// <summary>
        /// 维修位置
        /// </summary>
        public string RepairLocation
        {
            get { return this.GetProperty(RepairLocationProperty); }
            set { this.SetProperty(RepairLocationProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 维修换料记录 实体配置
    /// </summary>
    internal class WipRepairReplaceItemRecordConfig : EntityConfig<WipRepairReplaceItemRecord>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_PROD_KEY_ITEM_REPLACE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}