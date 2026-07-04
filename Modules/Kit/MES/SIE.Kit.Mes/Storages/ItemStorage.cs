using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Kit.MES.Storages
{
    /// <summary>
    /// 产线物料货位
    /// </summary>
    [RootEntity, Serializable]
    [Label("产线物料货位")]
    public partial class ItemStorage : DataEntity
    {
        #region 物料 Item
        /// <summary>
        /// 物料
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<ItemStorage>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Item> ItemProperty = P<ItemStorage>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 货位 StorageLocation
        /// <summary>
        /// 货位Id
        /// </summary>
        [Label("货位")]
        public static readonly IRefIdProperty StorageLocationIdProperty = P<ItemStorage>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

        /// <summary>
        /// 货位Id
        /// </summary>
        public double StorageLocationId
        {
            get { return (double)GetRefId(StorageLocationIdProperty); }
            set { SetRefId(StorageLocationIdProperty, value); }
        }

        /// <summary>
        /// 货位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty = P<ItemStorage>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 货位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return GetRefEntity(StorageLocationProperty); }
            set { SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("库存")]
        public static readonly Property<decimal> QtyProperty = P<ItemStorage>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 安全水位 SafetyQty
        /// <summary>
        /// 安全水位
        /// </summary>
        [Label("安全水位")]
        [MinValue(0)]
        [Required]
        public static readonly Property<decimal> SafetyQtyProperty = P<ItemStorage>.Register(e => e.SafetyQty);

        /// <summary>
        /// 安全水位
        /// </summary>
        public decimal SafetyQty
        {
            get { return this.GetProperty(SafetyQtyProperty); }
            set { this.SetProperty(SafetyQtyProperty, value); }
        }
        #endregion

        #region 叫料批量 CallMaterialBatch
        /// <summary>
        /// 叫料批量
        /// </summary>
        [Label("叫料批量")]
        [MinValue(0)]
        [Required]
        public static readonly Property<decimal> CallMaterialBatchProperty = P<ItemStorage>.Register(e => e.CallMaterialBatch);

        /// <summary>
        /// 叫料批量
        /// </summary>
        public decimal CallMaterialBatch
        {
            get { return this.GetProperty(CallMaterialBatchProperty); }
            set { this.SetProperty(CallMaterialBatchProperty, value); }
        }
        #endregion

        #region 属性值列表 PropertyValueList
        /// <summary>
        /// 属性值列表
        /// </summary>
        [Label("属性值列表")]
        public static readonly ListProperty<EntityList<ItemStoragePropertyValue>> PropertyValueListProperty = P<ItemStorage>.RegisterList(e => e.PropertyValueList);

        /// <summary>
        /// 属性值列表
        /// </summary>
        public EntityList<ItemStoragePropertyValue> PropertyValueList
        {
            get { return this.GetLazyList(PropertyValueListProperty); }
        }
        #endregion

        #region 视图属性
        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<ItemStorage>.RegisterView(e => e.ItemCode, p => p.Item.Code);

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
        public static readonly Property<string> ItemNameProperty = P<ItemStorage>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 物料消耗类型 ItemConsumeMode
        /// <summary>
        /// 物料消耗类型
        /// </summary>
        [Label("物料消耗类型")]
        public static readonly Property<ConsumeMode> ItemConsumeModeProperty = P<ItemStorage>.RegisterView(e => e.ItemConsumeMode, p => p.Item.ConsumeMode);

        /// <summary>
        /// 物料消耗类型
        /// </summary>
        public ConsumeMode ItemConsumeMode
        {
            get { return this.GetProperty(ItemConsumeModeProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 产线物料货位 实体配置
    /// </summary>
    internal class ItemStorageLocationConfig : EntityConfig<ItemStorage>
    {
        /// <summary>
        /// 增加实体的数据验证
        /// </summary>
        /// <param name="rules">实体验证集合</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
        }

        /// <summary>
        /// 配置数据库的映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_WH_ITEM").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.Property(ItemStorage.ItemIdProperty).ColumnMeta.HasIndex();
            Meta.Property(ItemStorage.StorageLocationIdProperty).ColumnMeta.HasIndex();
        }
    }
}