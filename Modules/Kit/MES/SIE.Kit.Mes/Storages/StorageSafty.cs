using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Kit.MES.Storages
{
    /// <summary>
    /// 产线库存
    /// </summary>
    [RootEntity, Serializable]
    [Label("物料库存")]
    public partial class StorageSafty : DataEntity
    {
        #region 容量 MaxQty
        /// <summary>
        /// 容量
        /// </summary>
        [Label("容量")]
        public static readonly Property<decimal> MaxQtyProperty = P<StorageSafty>.Register(e => e.MaxQty);

        /// <summary>
        /// 容量
        /// </summary>
        public decimal MaxQty
        {
            get { return GetProperty(MaxQtyProperty); }
            set { SetProperty(MaxQtyProperty, value); }
        }
        #endregion

        #region 安全库存 SafetyQty
        /// <summary>
        /// 安全库存
        /// </summary>
        [Label("安全库存")]
        public static readonly Property<decimal> SafetyQtyProperty = P<StorageSafty>.Register(e => e.SafetyQty);

        /// <summary>
        /// 安全库存
        /// </summary>
        public decimal SafetyQty
        {
            get { return GetProperty(SafetyQtyProperty); }
            set { SetProperty(SafetyQtyProperty, value); }
        }
        #endregion

        #region 货区 StorageArea
        /// <summary>
        /// 货区
        /// </summary>
        [Label("货区")]
        public static readonly IRefIdProperty StorageAreaIdProperty = P<StorageSafty>.RegisterRefId(e => e.StorageAreaId, ReferenceType.Parent);

        /// <summary>
        /// 货区
        /// </summary>
        public double StorageAreaId
        {
            get { return (double)GetRefId(StorageAreaIdProperty); }
            set { SetRefId(StorageAreaIdProperty, value); }
        }

        /// <summary>
        /// 货区
        /// </summary>
        public static readonly RefEntityProperty<StorageArea> StorageAreaProperty = P<StorageSafty>.RegisterRef(e => e.StorageArea, StorageAreaIdProperty);

        /// <summary>
        /// 货区
        /// </summary>
        public StorageArea StorageArea
        {
            get { return GetRefEntity(StorageAreaProperty); }
            set { SetRefEntity(StorageAreaProperty, value); }
        }
        #endregion

        #region 货位 StorageLocation
        /// <summary>
        /// 货位Id
        /// </summary>
        [Label("货位")]
        public static readonly IRefIdProperty StorageLocationIdProperty =
            P<StorageSafty>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

        /// <summary>
        /// 货位Id
        /// </summary>
        public double StorageLocationId
        {
            get { return (double)this.GetRefId(StorageLocationIdProperty); }
            set { this.SetRefId(StorageLocationIdProperty, value); }
        }

        /// <summary>
        /// 货位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty =
            P<StorageSafty>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 货位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return this.GetRefEntity(StorageLocationProperty); }
            set { this.SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<StorageSafty>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料
        /// </summary>
        public double ItemId
        {
            get { return (double)GetRefId(ItemIdProperty); }
            set { SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<StorageSafty>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion 

        #region 送货数量 DeliveryQty
        /// <summary>
        /// 送货数量
        /// </summary>
        [Label("配送数量")]
        public static readonly Property<decimal> DeliveryQtyProperty = P<StorageSafty>.Register(e => e.DeliveryQty);

        /// <summary>
        /// 送货数量
        /// </summary>
        public decimal DeliveryQty
        {
            get { return this.GetProperty(DeliveryQtyProperty); }
            set { this.SetProperty(DeliveryQtyProperty, value); }
        }
        #endregion

        #region 剩余库存（货位实际库存） SurplusQty
        /// <summary>
        /// 剩余库存（货位实际库存）
        /// </summary>
        [Label("剩余库存")]
        public static readonly Property<decimal> SurplusQtyProperty = P<StorageSafty>.Register(e => e.SurplusQty);

        /// <summary>
        /// 剩余库存（货位实际库存）
        /// </summary>
        public decimal SurplusQty
        {
            get { return this.GetProperty(SurplusQtyProperty); }
            set { this.SetProperty(SurplusQtyProperty, value); }
        }
        #endregion

        #region 配送中数量（叫料单已发货未接收的数量） DeliveryingQty
        /// <summary>
        /// 配送中数量（叫料单已发货未接收的数量）
        /// </summary>
        [Label("配送中数量")]
        public static readonly Property<decimal> DeliveryingQtyProperty = P<StorageSafty>.Register(e => e.DeliveryingQty);

        /// <summary>
        /// 配送中数量（叫料单已发货未接收的数量）
        /// </summary>
        public decimal DeliveryingQty
        {
            get { return this.GetProperty(DeliveryingQtyProperty); }
            set { this.SetProperty(DeliveryingQtyProperty, value); }
        }
        #endregion

        #region 属性值列表 PropertyValueList
        /// <summary>
        /// 属性值列表
        /// </summary>
        [Label("属性值列表")]
        public static readonly ListProperty<EntityList<StorageSaftyPropertyValue>> PropertyValueListProperty = P<StorageSafty>.RegisterList(e => e.PropertyValueList);

        /// <summary>
        /// 属性值列表
        /// </summary>
        public EntityList<StorageSaftyPropertyValue> PropertyValueList
        {
            get { return this.GetLazyList(PropertyValueListProperty); }
        }
        #endregion

        #region 注册视图
        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<StorageSafty>.RegisterView(e => e.ItemCode, e => e.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<StorageSafty>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 货区编码 StorageAreaCode
        /// <summary>
        /// 货区编码
        /// </summary>
        [Label("货区编码")]
        public static readonly Property<string> StorageAreaCodeProperty = P<StorageSafty>.RegisterView(e => e.StorageAreaCode, p => p.StorageArea.Code);

        /// <summary>
        /// 货区编码
        /// </summary>
        public string StorageAreaCode
        {
            get { return this.GetProperty(StorageAreaCodeProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 产线库存 实体配置
    /// </summary>
    internal class ItemStorageConfig : EntityConfig<StorageSafty>
    {
        /// <summary>
        /// 配置数据库的映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_WH_SAFTY").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.Property(StorageSafty.StorageAreaIdProperty).ColumnMeta.HasIndex();
            Meta.IndexGroupOnProperties(StorageSafty.ItemIdProperty, StorageSafty.StorageAreaIdProperty, StorageSafty.StorageLocationIdProperty);
        }
    }
}