using SIE.Common.Configs;
using SIE.Domain;
using SIE.Kit.UrgentOrder.ItemUrgentOrders.Configs;
using SIE.Items;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Kit.UrgentOrder.ItemUrgentOrders
{
    /// <summary>
    /// 物料加急单
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(ItemUrgentOrderCriteria))]
    [EntityWithConfig(typeof(ItemUrgentOrderNoConfig))]
    [EntityWithConfig(typeof(ItemUrgentOrderDateConfig))]
    [Label("物料加急单")]
    public partial class ItemUrgentOrder : DataEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ItemUrgentOrder()
        {
            CreateBy = RT.IdentityId;
            CreateDate = DateTime.Now;
            UpdateBy = RT.IdentityId;
            UpdateDate = DateTime.Now;
        }

        #region 单号 No
        /// <summary>
        /// 单号
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("单号")]
        public static readonly Property<string> NoProperty = P<ItemUrgentOrder>.Register(e => e.No);

        /// <summary>
        /// 单号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 需求数量 Qty
        /// <summary>
        /// 需求数量
        /// </summary>
        [Label("需求数量")]
        public static readonly Property<decimal> QtyProperty = P<ItemUrgentOrder>.Register(e => e.Qty);

        /// <summary>
        /// 需求数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 需求时间 DemandTime
        /// <summary>
        /// 需求时间
        /// </summary>
        [Label("需求时间")]
        public static readonly Property<DateTime> DemandTimeProperty = P<ItemUrgentOrder>.Register(e => e.DemandTime);

        /// <summary>
        /// 需求时间
        /// </summary>
        public DateTime DemandTime
        {
            get { return GetProperty(DemandTimeProperty); }
            set { SetProperty(DemandTimeProperty, value); }
        }
        #endregion

        #region 收料 IsReceive
        /// <summary>
        /// 收料
        /// </summary>
        [Label("收料")]
        public static readonly Property<bool> IsReceiveProperty = P<ItemUrgentOrder>.Register(e => e.IsReceive);

        /// <summary>
        /// 收料
        /// </summary>
        public bool IsReceive
        {
            get { return GetProperty(IsReceiveProperty); }
            set { SetProperty(IsReceiveProperty, value); }
        }
        #endregion

        #region 收料看板数量 ReceiveQty
        /// <summary>
        /// 收料看板数量
        /// </summary>
        [Label("收料看板数量")]
        public static readonly Property<decimal?> ReceiveQtyProperty = P<ItemUrgentOrder>.Register(e => e.ReceiveQty);

        /// <summary>
        /// 收料看板数量
        /// </summary>
        public decimal? ReceiveQty
        {
            get { return GetProperty(ReceiveQtyProperty); }
            set { SetProperty(ReceiveQtyProperty, value); }
        }
        #endregion

        #region IQC检验 IsInspectIqc
        /// <summary>
        /// IQC检验
        /// </summary>
        [Label("IQC检验")]
        public static readonly Property<bool> IsInspectIqcProperty = P<ItemUrgentOrder>.Register(e => e.IsInspectIqc);

        /// <summary>
        /// IQC检验
        /// </summary>
        public bool IsInspectIqc
        {
            get { return GetProperty(IsInspectIqcProperty); }
            set { SetProperty(IsInspectIqcProperty, value); }
        }
        #endregion

        #region IQC检验看板数量 InspectIqcQty
        /// <summary>
        /// IQC检验看板数量
        /// </summary>
        [Label("IQC检验看板数量")]
        public static readonly Property<decimal?> InspectIqcQtyProperty = P<ItemUrgentOrder>.Register(e => e.InspectIqcQty);

        /// <summary>
        /// IQC检验看板数量
        /// </summary>
        public decimal? InspectIqcQty
        {
            get { return GetProperty(InspectIqcQtyProperty); }
            set { SetProperty(InspectIqcQtyProperty, value); }
        }
        #endregion

        #region 入库 IsInstorage
        /// <summary>
        /// 入库
        /// </summary>
        [Label("入库")]
        public static readonly Property<bool> IsInstorageProperty = P<ItemUrgentOrder>.Register(e => e.IsInstorage);

        /// <summary>
        /// 入库
        /// </summary>
        public bool IsInstorage
        {
            get { return GetProperty(IsInstorageProperty); }
            set { SetProperty(IsInstorageProperty, value); }
        }
        #endregion

        #region 入库看板数量 InstorageQty
        /// <summary>
        /// 入库看板数量
        /// </summary>
        [Label("入库看板数量")]
        public static readonly Property<decimal?> InstorageQtyProperty = P<ItemUrgentOrder>.Register(e => e.InstorageQty);

        /// <summary>
        /// 入库看板数量
        /// </summary>
        public decimal? InstorageQty
        {
            get { return GetProperty(InstorageQtyProperty); }
            set { SetProperty(InstorageQtyProperty, value); }
        }
        #endregion

        #region 备料 IsStockUp
        /// <summary>
        /// 备料
        /// </summary>
        [Label("备料")]
        public static readonly Property<bool> IsStockUpProperty = P<ItemUrgentOrder>.Register(e => e.IsStockUp);

        /// <summary>
        /// 备料
        /// </summary>
        public bool IsStockUp
        {
            get { return GetProperty(IsStockUpProperty); }
            set { SetProperty(IsStockUpProperty, value); }
        }
        #endregion

        #region 备料看板数量 StockUpQty
        /// <summary>
        /// 备料看板数量
        /// </summary>
        [Label("备料看板数量")]
        public static readonly Property<decimal?> StockUpQtyProperty = P<ItemUrgentOrder>.Register(e => e.StockUpQty);

        /// <summary>
        /// 备料看板数量
        /// </summary>
        public decimal? StockUpQty
        {
            get { return GetProperty(StockUpQtyProperty); }
            set { SetProperty(StockUpQtyProperty, value); }
        }
        #endregion

        #region 备料工单 StockUpNo
        /// <summary>
        /// 备料工单
        /// </summary>
        [Label("备料工单")]
        public static readonly Property<string> StockUpNoProperty = P<ItemUrgentOrder>.Register(e => e.StockUpNo);

        /// <summary>
        /// 备料工单
        /// </summary>
        public string StockUpNo
        {
            get { return GetProperty(StockUpNoProperty); }
            set { SetProperty(StockUpNoProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        public static readonly IRefIdProperty ItemIdProperty = P<ItemUrgentOrder>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Item> ItemProperty = P<ItemUrgentOrder>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 加急单状态 OrderState
        /// <summary>
        /// 加急单状态
        /// </summary>
        [Label("加急单状态")]
        public static readonly Property<UrgentOrderState> OrderStateProperty = P<ItemUrgentOrder>.Register(e => e.OrderState);

        /// <summary>
        /// 加急单状态
        /// </summary>
        public UrgentOrderState OrderState
        {
            get { return GetProperty(OrderStateProperty); }
            set { SetProperty(OrderStateProperty, value); }
        }
        #endregion

        #region 备料看板状态 StockUpState
        /// <summary>
        /// 备料看板状态
        /// </summary>
        [Label("备料看板状态")]
        public static readonly Property<UrgentOrderState> StockUpStateProperty = P<ItemUrgentOrder>.Register(e => e.StockUpState);

        /// <summary>
        /// 备料看板状态
        /// </summary>
        public UrgentOrderState StockUpState
        {
            get { return GetProperty(StockUpStateProperty); }
            set { SetProperty(StockUpStateProperty, value); }
        }
        #endregion

        #region 入库看板状态 InstorageState
        /// <summary>
        /// 入库看板状态
        /// </summary>
        [Label("入库看板状态")]
        public static readonly Property<UrgentOrderState> InstorageStateProperty = P<ItemUrgentOrder>.Register(e => e.InstorageState);

        /// <summary>
        /// 入库看板状态
        /// </summary>
        public UrgentOrderState InstorageState
        {
            get { return GetProperty(InstorageStateProperty); }
            set { SetProperty(InstorageStateProperty, value); }
        }
        #endregion

        #region IQC检验看板状态 InspectIqcState
        /// <summary>
        /// IQC检验看板状态
        /// </summary>
        [Label("IQC检验看板状态")]
        public static readonly Property<UrgentOrderState> InspectIqcStateProperty = P<ItemUrgentOrder>.Register(e => e.InspectIqcState);

        /// <summary>
        /// IQC检验看板状态
        /// </summary>
        public UrgentOrderState InspectIqcState
        {
            get { return GetProperty(InspectIqcStateProperty); }
            set { SetProperty(InspectIqcStateProperty, value); }
        }
        #endregion

        #region 收料看板状态 ReceiveState
        /// <summary>
        /// 收料看板状态
        /// </summary>
        [Label("收料看板状态")]
        public static readonly Property<UrgentOrderState> ReceiveStateProperty = P<ItemUrgentOrder>.Register(e => e.ReceiveState);

        /// <summary>
        /// 收料看板状态
        /// </summary>
        public UrgentOrderState ReceiveState
        {
            get { return GetProperty(ReceiveStateProperty); }
            set { SetProperty(ReceiveStateProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<ItemUrgentOrder>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { SetProperty(ItemNameProperty, value); }
        }
        #endregion



        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPropertyChanged(ManagedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (IsInspectIqc) {
                InspectIqcQty = Qty;
            }
            
        }


    }

    /// <summary>
    /// 物料加急单 实体配置
    /// </summary>
    internal class ItemUrgentOrderConfig : EntityConfig<ItemUrgentOrder>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("APS_ITEM_URGENT_ORDER").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
