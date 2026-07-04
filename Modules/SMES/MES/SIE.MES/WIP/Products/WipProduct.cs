using SIE.Common;
using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WIP.Products
{
    /// <summary>
    /// 生产产品
    /// </summary>
    [RootEntity, Serializable]
    [Label("生产产品")]
    [DisplayMember(nameof(Puid))]
    public partial class WipProduct : DataEntity
    {
        #region 产品ID Puid
        /// <summary>
        /// 产品ID
        /// </summary>
        [Required]
        [NotDuplicate]
        [MaxLength(40)]
        [Label("产品ID")]
        public static readonly Property<string> PuidProperty = P<WipProduct>.Register(e => e.Puid);

        /// <summary>
        /// 产品ID
        /// </summary>
        public string Puid
        {
            get { return GetProperty(PuidProperty); }
            set { SetProperty(PuidProperty, value); }
        }
        #endregion

        #region 型号 Model
        /// <summary>
        /// 型号
        /// </summary>
        [Label("型号")]
        public static readonly Property<string> ModelProperty = P<WipProduct>.Register(e => e.Model);

        /// <summary>
        /// 型号
        /// </summary>
        public string Model
        {
            get { return GetProperty(ModelProperty); }
            set { SetProperty(ModelProperty, value); }
        }
        #endregion

        #region 是否保留,保留状态不能过终检 IsHold
        /// <summary>
        /// 是否保留,保留状态不能过终检
        /// </summary>
        [Required]
        [Label("是否保留，保留状态不能过终检")]
        public static readonly Property<bool> IsHoldProperty = P<WipProduct>.Register(e => e.IsHold);

        /// <summary>
        /// 是否保留,保留状态不能过终检
        /// </summary>
        public bool IsHold
        {
            get { return GetProperty(IsHoldProperty); }
            set { SetProperty(IsHoldProperty, value); }
        }
        #endregion

        #region 是否返修过 IsFixed
        /// <summary>
        /// 是否返修过
        /// </summary>
        [Required]
        [Label("是否返修过")]
        public static readonly Property<bool> IsFixedProperty = P<WipProduct>.Register(e => e.IsFixed);

        /// <summary>
        /// 是否返修过
        /// </summary>
        public bool IsFixed
        {
            get { return GetProperty(IsFixedProperty); }
            set { SetProperty(IsFixedProperty, value); }
        }
        #endregion

        #region 是否让步 IsConcession
        /// <summary>
        /// 是否让步
        /// </summary>
        [Required]
        [Label("是否让步")]
        public static readonly Property<bool> IsConcessionProperty = P<WipProduct>.Register(e => e.IsConcession);

        /// <summary>
        /// 是否让步
        /// </summary>
        public bool IsConcession
        {
            get { return GetProperty(IsConcessionProperty); }
            set { SetProperty(IsConcessionProperty, value); }
        }
        #endregion

        #region 批数量 BatchQty
        /// <summary>
        /// 批数量
        /// </summary>
        [Required]
        [MinValue(0)]
        [Label("批数量")]
        public static readonly Property<decimal> BatchQtyProperty = P<WipProduct>.Register(e => e.BatchQty);

        /// <summary>
        /// 批数量
        /// </summary>
        public decimal BatchQty
        {
            get { return GetProperty(BatchQtyProperty); }
            set { SetProperty(BatchQtyProperty, value); }
        }
        #endregion

        #region 不合格数量 NgQty
        /// <summary>
        /// 不合格数量
        /// </summary>
        [Label("不合格数量")]
        public static readonly Property<decimal> NgQtyProperty = P<WipProduct>.Register(e => e.NgQty);

        /// <summary>
        /// 不合格数量
        /// </summary>
        public decimal NgQty
        {
            get { return this.GetProperty(NgQtyProperty); }
            set { this.SetProperty(NgQtyProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<WipProduct>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Item> ItemProperty = P<WipProduct>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 版本列表 VersionList
        /// <summary>
        /// 版本列表
        /// </summary>
        [Label("版本列表")]
        public static readonly ListProperty<EntityList<WipProductVersion>> VersionListProperty = P<WipProduct>.RegisterList(e => e.VersionList);

        /// <summary>
        /// 版本列表
        /// </summary>
        public EntityList<WipProductVersion> VersionList
        {
            get { return this.GetLazyList(VersionListProperty); }
        }
        #endregion

        #region 当前版本 CurrentVersion
        /// <summary>
        /// 当前版本Id
        /// </summary>
        [Label("当前版本")]
        public static readonly IRefIdProperty CurrentVersionIdProperty = P<WipProduct>.RegisterRefId(e => e.CurrentVersionId, ReferenceType.Normal);

        /// <summary>
        /// 当前版本Id
        /// </summary>
        public double? CurrentVersionId
        {
            get { return (double?)GetRefNullableId(CurrentVersionIdProperty); }
            set { SetRefNullableId(CurrentVersionIdProperty, value); }
        }

        /// <summary>
        /// 当前版本
        /// </summary>
        public static readonly RefEntityProperty<WipProductVersion> CurrentVersionProperty = P<WipProduct>.RegisterRef(e => e.CurrentVersion, CurrentVersionIdProperty);

        /// <summary>
        /// 当前版本
        /// </summary>
        public WipProductVersion CurrentVersion
        {
            get { return GetRefEntity(CurrentVersionProperty); }
            set { SetRefEntity(CurrentVersionProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<WipProductState> StateProperty = P<WipProduct>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public WipProductState State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 采集结果 Result
        /// <summary>
        /// 采集结果
        /// </summary>
        [Label("采集结果")]
        public static readonly Property<ResultType> ResultProperty = P<WipProduct>.Register(e => e.Result);

        /// <summary>
        /// 采集结果
        /// </summary>
        public ResultType Result
        {
            get { return GetProperty(ResultProperty); }
            set { SetProperty(ResultProperty, value); }
        }
        #endregion

        #region 产品等级 Grade
        /// <summary>
        /// 产品等级
        /// </summary>
        [Label("产品等级")]
        public static readonly Property<ProductGrade> GradeProperty = P<WipProduct>.Register(e => e.Grade);

        /// <summary>
        /// 产品等级
        /// </summary>
        public ProductGrade Grade
        {
            get { return GetProperty(GradeProperty); }
            set { SetProperty(GradeProperty, value); }
        }
        #endregion

        #region 扩展属性 ItemExtProp
        /// <summary>
        /// 扩展属性
        /// </summary>
        [Label("扩展属性")]
        public static readonly Property<string> ItemExtPropProperty = P<WipProduct>.Register(e => e.ItemExtProp);

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
        public static readonly Property<string> ItemExtPropNameProperty = P<WipProduct>.Register(e => e.ItemExtPropName);

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
    /// 生产产品 实体配置
    /// </summary>
    internal class WipProductConfig : EntityConfig<WipProduct>
    {
        /// <summary>
        /// 配置数据库映射
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_PRODUCT").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.Property(WipProduct.CurrentVersionIdProperty).ColumnMeta.IgnoreFK().IsNullable();
            Meta.Property(WipProduct.PuidProperty).ColumnMeta.HasIndex();
            Meta.Property(WipProduct.CurrentVersionIdProperty).ColumnMeta.HasIndex();
        }
    }
}