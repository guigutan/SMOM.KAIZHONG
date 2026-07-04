using SIE.Core.Enums;
using SIE.Domain;
using SIE.Core.Enums;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.WMS.Statistics.MonthlyBalances
{
    /// <summary>
    /// 月结库存
    /// </summary>
    [RootEntity, Serializable]
    [Label("月结库存")]
    public partial class MonthlyBalance : DataEntity
    {
        #region 账期 AccountPeriod
        /// <summary>
        /// 账期
        /// </summary>
        [Label("账期")]
        public static readonly Property<string> AccountPeriodProperty = P<MonthlyBalance>.Register(e => e.AccountPeriod);

        /// <summary>
        /// 账期
        /// </summary>
        public string AccountPeriod
        {
            get { return GetProperty(AccountPeriodProperty); }
            set { SetProperty(AccountPeriodProperty, value); }
        }
        #endregion

        #region 货主 StorerCode
        /// <summary>
        /// 货主
        /// </summary>
        [Label("货主")]
        public static readonly Property<string> StorerCodeProperty = P<MonthlyBalance>.Register(e => e.StorerCode);

        /// <summary>
        /// 货主
        /// </summary>
        public string StorerCode
        {
            get { return GetProperty(StorerCodeProperty); }
            set { SetProperty(StorerCodeProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<MonthlyBalance>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 期初库存 OpeningInvQty
        /// <summary>
        /// 期初库存
        /// </summary>
        [Label("期初库存")]
        public static readonly Property<decimal> OpeningInvQtyProperty = P<MonthlyBalance>.Register(e => e.OpeningInvQty);

        /// <summary>
        /// 期初库存
        /// </summary>
        public decimal OpeningInvQty
        {
            get { return GetProperty(OpeningInvQtyProperty); }
            set { SetProperty(OpeningInvQtyProperty, value); }
        }
        #endregion

        #region 期末库存 EndingInvQty
        /// <summary>
        /// 期末库存
        /// </summary>
        [Label("期末库存")]
        public static readonly Property<decimal> EndingInvQtyProperty = P<MonthlyBalance>.Register(e => e.EndingInvQty);

        /// <summary>
        /// 期末库存
        /// </summary>
        public decimal EndingInvQty
        {
            get { return GetProperty(EndingInvQtyProperty); }
            set { SetProperty(EndingInvQtyProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        public static readonly IRefIdProperty WarehouseIdProperty = P<MonthlyBalance>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double WarehouseId
        {
            get { return (double)GetRefId(WarehouseIdProperty); }
            set { SetRefId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<MonthlyBalance>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        public static readonly IRefIdProperty ItemIdProperty = P<MonthlyBalance>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Item> ItemProperty = P<MonthlyBalance>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 订单类型 OrderType
        /// <summary>
        /// 订单类型
        /// </summary>
        [Label("订单类型")]
        public static readonly Property<OrderType> OrderTypeProperty = P<MonthlyBalance>.Register(e => e.OrderType);

        /// <summary>
        /// 订单类型
        /// </summary>
        public OrderType OrderType
        {
            get { return GetProperty(OrderTypeProperty); }
            set { SetProperty(OrderTypeProperty, value); }
        }
        #endregion

        #region 生成日期 GenerationDate
        /// <summary>
        /// 生成日期
        /// </summary>
        [Label("生成日期")]
        public static readonly Property<DateTime> GenerationDateProperty = P<MonthlyBalance>.Register(e => e.GenerationDate);

        /// <summary>
        /// 生成日期
        /// </summary>
        public DateTime GenerationDate
        {
            get { return this.GetProperty(GenerationDateProperty); }
            set { this.SetProperty(GenerationDateProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("属性名")]
        public static readonly Property<string> ItemCodeProperty = P<MonthlyBalance>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<MonthlyBalance>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 仓库编码 WarehouseCode
        /// <summary>
        /// 仓库编码
        /// </summary>
        [Label("仓库编码")]
        public static readonly Property<string> WarehouseCodeProperty = P<MonthlyBalance>.RegisterView(e => e.WarehouseCode, p => p.Warehouse.Code);

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode
        {
            get { return this.GetProperty(WarehouseCodeProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 库存月结 实体配置
    /// </summary>
    internal class MonthlyBalanceConfig : EntityConfig<MonthlyBalance>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INV_MONTHLY_BALANCE").MapAllProperties();
            Meta.Property(MonthlyBalance.AccountPeriodProperty).MapColumn().HasIndex(IndexTypeMeta.Indexed);
            Meta.EnablePhantoms();
            Meta.DisableDataSync();
        }
    }
}