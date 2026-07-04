using SIE.Domain;
using SIE.Items;
using SIE.MES.WIP.Products;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.BatchWIP.Products
{
    /// <summary>
    /// 批次生产产品
    /// </summary>
    [RootEntity, Serializable]
    [Label("批次生产产品")]
    public partial class BatchWipProduct : DataEntity
    {
        #region 批次ID Buid
        /// <summary>
        /// 批次ID
        /// </summary>
        [Label("批次ID")]
        public static readonly Property<string> BuidProperty = P<BatchWipProduct>.Register(e => e.Buid);

        /// <summary>
        /// 批次ID
        /// </summary>
        public string Buid
        {
            get { return GetProperty(BuidProperty); }
            set { SetProperty(BuidProperty, value); }
        }
        #endregion

        #region 型号 Model
        /// <summary>
        /// 型号
        /// </summary>
        [Label("型号")]
        public static readonly Property<string> ModelProperty = P<BatchWipProduct>.Register(e => e.Model);

        /// <summary>
        /// 型号
        /// </summary>
        public string Model
        {
            get { return GetProperty(ModelProperty); }
            set { SetProperty(ModelProperty, value); }
        }
        #endregion

        #region 批次数量 Qty
        /// <summary>
        /// 批次数量
        /// </summary>
        [Label("批次数量")]
        public static readonly Property<decimal> QtyProperty = P<BatchWipProduct>.Register(e => e.Qty);

        /// <summary>
        /// 批次数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 产品生产状态 State
        /// <summary>
        /// 产品生产状态
        /// </summary>
        [Label("产品生产状态")]
        public static readonly Property<WipProductState> StateProperty = P<BatchWipProduct>.Register(e => e.State);

        /// <summary>
        /// 产品生产状态
        /// </summary>
        public WipProductState State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 当前版本 CurrentVersion
        /// <summary>
        /// 当前版本Id
        /// </summary>
		[Label("当前版本")]
        public static readonly IRefIdProperty CurrentVersionIdProperty = P<BatchWipProduct>.RegisterRefId(e => e.CurrentVersionId, ReferenceType.Normal);

        /// <summary>
        /// 当前版本Id
        /// </summary>
        public double CurrentVersionId
        {
            get { return (double)GetRefId(CurrentVersionIdProperty); }
            set { SetRefId(CurrentVersionIdProperty, value); }
        }

        /// <summary>
        /// 当前版本
        /// </summary>
        public static readonly RefEntityProperty<BatchWipProductVersion> CurrentVersionProperty = P<BatchWipProduct>.RegisterRef(e => e.CurrentVersion, CurrentVersionIdProperty);

        /// <summary>
        /// 当前版本
        /// </summary>
        public BatchWipProductVersion CurrentVersion
        {
            get { return GetRefEntity(CurrentVersionProperty); }
            set { SetRefEntity(CurrentVersionProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
		[Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<BatchWipProduct>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Item> ItemProperty = P<BatchWipProduct>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 属性值 ValueList
        /// <summary>
        /// 属性值
        /// </summary>
        public static readonly ListProperty<EntityList<BatchWipProductProperty>> ValueListProperty = P<BatchWipProduct>.RegisterList(e => e.ValueList);

        /// <summary>
        /// 属性值
        /// </summary>
        public EntityList<BatchWipProductProperty> ValueList
        {
            get { return this.GetLazyList(ValueListProperty); }
        }
        #endregion

        #region 版本列表 VersionList
        /// <summary>
        /// 版本列表
        /// </summary>
        public static readonly ListProperty<EntityList<BatchWipProductVersion>> VersionListProperty = P<BatchWipProduct>.RegisterList(e => e.VersionList);

        /// <summary>
        /// 版本列表
        /// </summary>
        public EntityList<BatchWipProductVersion> VersionList
        {
            get { return this.GetLazyList(VersionListProperty); }
        }
        #endregion

        #region 扩展属性 ItemExtProp
        /// <summary>
        /// 扩展属性
        /// </summary>
        [Label("扩展属性")]
        public static readonly Property<string> ItemExtPropProperty = P<BatchWipProduct>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get { return GetProperty(ItemExtPropProperty); }
            set { SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 扩展属性值 ItemExtPropName
        /// <summary>
        /// 扩展属性值
        /// </summary>
        [Label("扩展属性值")]
        public static readonly Property<string> ItemExtPropNameProperty = P<BatchWipProduct>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 扩展属性值
        /// </summary>
        public string ItemExtPropName
        {
            get { return GetProperty(ItemExtPropNameProperty); }
            set { SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 批次生产产品 实体配置
    /// </summary>
    internal class BatchWipProductConfig : EntityConfig<BatchWipProduct>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("BWIP_PRODUCT").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.Property(BatchWipProduct.CurrentVersionIdProperty).ColumnMeta.IgnoreFK().IsNullable();
        }
    }
}