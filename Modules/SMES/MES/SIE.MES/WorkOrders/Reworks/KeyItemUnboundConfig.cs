using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WorkOrders.Reworks
{
    /// <summary>
    /// 关键件解绑配置
    /// </summary>
    [RootEntity, Serializable]
    [Label("关键件解绑配置")]
    public partial class KeyItemUnboundConfig : DataEntity
    {
        #region 是否解绑 IsUnbound
        /// <summary>
        /// 是否解绑
        /// </summary>
        [Label("是否解绑")]
        public static readonly Property<bool> IsUnboundProperty = P<KeyItemUnboundConfig>.Register(e => e.IsUnbound);

        /// <summary>
        /// 是否解绑
        /// </summary>
        public bool IsUnbound
        {
            get { return GetProperty(IsUnboundProperty); }
            set { SetProperty(IsUnboundProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<KeyItemUnboundConfig>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return (double)GetRefId(ItemIdProperty); }
            set { SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<KeyItemUnboundConfig>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 单位 Unit
        /// <summary>
        /// 单位Id
        /// </summary>
        [Label("单位")]
        public static readonly IRefIdProperty UnitIdProperty =
            P<KeyItemUnboundConfig>.RegisterRefId(e => e.UnitId, ReferenceType.Normal);

        /// <summary>
        /// 单位Id
        /// </summary>
        public double? UnitId
        {
            get { return (double?)this.GetRefNullableId(UnitIdProperty); }
            set { this.SetRefNullableId(UnitIdProperty, value); }
        }

        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly RefEntityProperty<Unit> UnitProperty =
            P<KeyItemUnboundConfig>.RegisterRef(e => e.Unit, UnitIdProperty);

        /// <summary>
        /// 单位
        /// </summary>
        public Unit Unit
        {
            get { return this.GetRefEntity(UnitProperty); }
            set { this.SetRefEntity(UnitProperty, value); }
        }
        #endregion

        #region 单机定额 SingleQty
        /// <summary>
        /// 单机定额
        /// </summary>
        [MinValue(0)]
        [Label("单机定额")]
        public static readonly Property<decimal> SingleQtyProperty = P<KeyItemUnboundConfig>.Register(e => e.SingleQty);

        /// <summary>
        /// 单机定额
        /// </summary>
        public decimal SingleQty
        {
            get { return GetProperty(SingleQtyProperty); }
            set { SetProperty(SingleQtyProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<KeyItemUnboundConfig>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

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
            P<KeyItemUnboundConfig>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 物料 ItemCode
        /// <summary>
        /// 物料
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<KeyItemUnboundConfig>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
            set { this.SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<KeyItemUnboundConfig>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<KeyItemUnboundConfig>.Register(e => e.UnitName);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
            set { this.SetProperty(UnitNameProperty, value); }
        }
        #endregion

        #region 条码工单Id OldWorkOrderId
        /// <summary>
        /// 条码工单Id
        /// </summary>
        [Label("条码工单Id")]
        public static readonly Property<double> OldWorkOrderIdProperty = P<KeyItemUnboundConfig>.Register(e => e.OldWorkOrderId);

        /// <summary>
        /// 条码工单Id
        /// </summary>
        public double OldWorkOrderId
        {
            get { return this.GetProperty(OldWorkOrderIdProperty); }
            set { this.SetProperty(OldWorkOrderIdProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 关键件解绑配置 实体配置
    /// </summary>
    internal class KeyItemUnboundConfigConfig : EntityConfig<KeyItemUnboundConfig>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_KEY_ITEM_CONFIG").MapAllProperties();
            Meta.Property(KeyItemUnboundConfig.ItemCodeProperty).DontMapColumn();
            Meta.Property(KeyItemUnboundConfig.ItemNameProperty).DontMapColumn();
            Meta.Property(KeyItemUnboundConfig.UnitNameProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}