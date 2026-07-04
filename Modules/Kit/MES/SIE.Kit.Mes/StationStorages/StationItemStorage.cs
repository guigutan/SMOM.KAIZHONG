using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Kit.MES.StationStorages
{
    /// <summary>
    /// 工位物料库存
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工位物料库存")]
    public class StationItemStorage : DataEntity
    {
        #region 工位工单库存 WoStorage
        /// <summary>
        /// 工位工单库存Id
        /// </summary>
        [Label("工位工单库存")]
        public static readonly IRefIdProperty WoStorageIdProperty =
            P<StationItemStorage>.RegisterRefId(e => e.WoStorageId, ReferenceType.Parent);

        /// <summary>
        /// 工位工单库存Id
        /// </summary>
        public double WoStorageId
        {
            get { return (double)this.GetRefId(WoStorageIdProperty); }
            set { this.SetRefId(WoStorageIdProperty, value); }
        }

        /// <summary>
        /// 工位工单库存
        /// </summary>
        public static readonly RefEntityProperty<WoStationStorage> WoStorageProperty =
            P<StationItemStorage>.RegisterRef(e => e.WoStorage, WoStorageIdProperty);

        /// <summary>
        /// 工位工单库存
        /// </summary>
        public WoStationStorage WoStorage
        {
            get { return this.GetRefEntity(WoStorageProperty); }
            set { this.SetRefEntity(WoStorageProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<StationItemStorage>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return (double)this.GetRefId(ItemIdProperty); }
            set { this.SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<StationItemStorage>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 实际库存数 ActStoreQty
        /// <summary>
        /// 实际库存数
        /// </summary>
        [Label("实际库存数")]
        public static readonly Property<decimal> ActStoreQtyProperty = P<StationItemStorage>.Register(e => e.ActStoreQty);

        /// <summary>
        /// 实际库存数
        /// </summary>
        public decimal ActStoreQty
        {
            get { return this.GetProperty(ActStoreQtyProperty); }
            set { this.SetProperty(ActStoreQtyProperty, value); }
        }
        #endregion

        #region 预库存 BudgetQty
        /// <summary>
        /// 预库存
        /// </summary>
        [Label("预库存数")]
        public static readonly Property<decimal> BudgetQtyProperty = P<StationItemStorage>.Register(e => e.BudgetQty);

        /// <summary>
        /// 预库存
        /// </summary>
        public decimal BudgetQty
        {
            get { return this.GetProperty(BudgetQtyProperty); }
            set { this.SetProperty(BudgetQtyProperty, value); }
        }
        #endregion

        #region 在途数量 SendingQty
        /// <summary>
        /// 在途数量
        /// </summary>
        [Label("在途数")]
        public static readonly Property<decimal> SendingQtyProperty = P<StationItemStorage>.Register(e => e.SendingQty);

        /// <summary>
        /// 在途数量
        /// </summary>
        public decimal SendingQty
        {
            get { return this.GetProperty(SendingQtyProperty); }
            set { this.SetProperty(SendingQtyProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<StationItemStorage>.RegisterView(e => e.ItemCode, p => p.Item.Code);

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
        public static readonly Property<string> ItemNameProperty = P<StationItemStorage>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 工位物料库存 实体配置
    /// </summary>
    internal class StationItemStorageEntityConfig : EntityConfig<StationItemStorage>
    {
        /// <summary>
        /// 配置数据库的映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_ITEM_STORAGE").MapAllProperties();
            Meta.IndexGroupOnProperties(StationItemStorage.WoStorageIdProperty, StationItemStorage.ItemIdProperty);
            Meta.Property(StationItemStorage.ItemIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}