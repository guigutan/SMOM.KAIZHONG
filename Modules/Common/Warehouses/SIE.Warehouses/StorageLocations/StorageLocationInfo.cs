using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Warehouses
{
    /// <summary>
    /// 库位基本资料
    /// </summary>
    [RootEntity, Serializable]
    [Label("库位基本资料")]
    public partial class StorageLocationInfo : DataEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public StorageLocationInfo() { LayerCount = 1; }

        /// <summary>
        /// 快码类型：库位形式
        /// </summary>
        public const string LOCATIONFORM = "LOCATION_FORM";

        #region 长(M) Length
        /// <summary>
        /// 长(M)
        /// </summary>
        [Label("长(M)")]
        public static readonly Property<decimal?> LengthProperty = P<StorageLocationInfo>.Register(e => e.Length);

        /// <summary>
        /// 长(M)
        /// </summary>
        public decimal? Length
        {
            get { return GetProperty(LengthProperty); }
            set { SetProperty(LengthProperty, value); }
        }
        #endregion

        #region 宽(M) Width
        /// <summary>
        /// 宽(M)
        /// </summary>
        [Label("宽(M)")]
        public static readonly Property<decimal?> WidthProperty = P<StorageLocationInfo>.Register(e => e.Width);

        /// <summary>
        /// 宽(M)
        /// </summary>
        public decimal? Width
        {
            get { return GetProperty(WidthProperty); }
            set { SetProperty(WidthProperty, value); }
        }
        #endregion

        #region 高(M) Height
        /// <summary>
        /// 高(M)
        /// </summary>
        [Label("高(M)")]
        public static readonly Property<decimal?> HeightProperty = P<StorageLocationInfo>.Register(e => e.Height);

        /// <summary>
        /// 高(M)
        /// </summary>
        public decimal? Height
        {
            get { return GetProperty(HeightProperty); }
            set { SetProperty(HeightProperty, value); }
        }
        #endregion

        #region 起点X坐标 StartPointX
        /// <summary>
        /// 起点X坐标
        /// </summary>
        [Label("起点X坐标")]
        public static readonly Property<decimal?> StartPointXProperty = P<StorageLocationInfo>.Register(e => e.StartPointX);

        /// <summary>
        /// 起点X坐标
        /// </summary>
        public decimal? StartPointX
        {
            get { return GetProperty(StartPointXProperty); }
            set { SetProperty(StartPointXProperty, value); }
        }
        #endregion

        #region 起点Y坐标 StartPointY
        /// <summary>
        /// 起点Y坐标
        /// </summary>
        [Label("起点Y坐标")]
        public static readonly Property<decimal?> StartPointYProperty = P<StorageLocationInfo>.Register(e => e.StartPointY);

        /// <summary>
        /// 起点Y坐标
        /// </summary>
        public decimal? StartPointY
        {
            get { return GetProperty(StartPointYProperty); }
            set { SetProperty(StartPointYProperty, value); }
        }
        #endregion

        #region 起点Z坐标 StartPointZ
        /// <summary>
        /// 起点Z坐标
        /// </summary>
        [Label("起点Z坐标")]
        public static readonly Property<decimal?> StartPointZProperty = P<StorageLocationInfo>.Register(e => e.StartPointZ);

        /// <summary>
        /// 起点Z坐标
        /// </summary>
        public decimal? StartPointZ
        {
            get { return GetProperty(StartPointZProperty); }
            set { SetProperty(StartPointZProperty, value); }
        }
        #endregion

        #region 库位层数 LayerCount
        /// <summary>
        /// 库位层数
        /// </summary>
        [Label("库位层数")]
        [Required]
        public static readonly Property<int> LayerCountProperty = P<StorageLocationInfo>.Register(e => e.LayerCount);

        /// <summary>
        /// 库位层数
        /// </summary>
        public int LayerCount
        {
            get { return GetProperty(LayerCountProperty); }
            set { SetProperty(LayerCountProperty, value); }
        }
        #endregion

        #region 库位形式 Form
        /// <summary>
        /// 库位形式
        /// </summary>
        [Label("库位形式")]
        [MaxLength(80)]
        public static readonly Property<string> FormProperty = P<StorageLocationInfo>.Register(e => e.Form);

        /// <summary>
        /// 库位形式
        /// </summary>
        public string Form
        {
            get { return GetProperty(FormProperty); }
            set { SetProperty(FormProperty, value); }
        }
        #endregion

        #region 保税库位 IsBonded
        /// <summary>
        /// 保税库位
        /// </summary>
        [Label("保税库位")]
        public static readonly Property<bool> IsBondedProperty = P<StorageLocationInfo>.Register(e => e.IsBonded);

        /// <summary>
        /// 保税库位
        /// </summary>
        public bool IsBonded
        {
            get { return GetProperty(IsBondedProperty); }
            set { SetProperty(IsBondedProperty, value); }
        }
        #endregion

        #region 库位 StorageLocation
        /// <summary>
        /// 库位Id
        /// </summary>
        [Label("库位")]
        [NotDuplicate]
        public static readonly IRefIdProperty StorageLocationIdProperty = P<StorageLocationInfo>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

        /// <summary>
        /// 库位Id
        /// </summary>
        public double StorageLocationId
        {
            get { return (double)GetRefId(StorageLocationIdProperty); }
            set { SetRefId(StorageLocationIdProperty, value); }
        }

        /// <summary>
        /// 库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty = P<StorageLocationInfo>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return GetRefEntity(StorageLocationProperty); }
            set { SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 库位基本资料 实体配置
    /// </summary>
    internal class StorageLocationInfoConfig : EntityConfig<StorageLocationInfo>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WH_LOCATION_INFO").MapAllProperties();
            Meta.Property(StorageLocationInfo.StorageLocationIdProperty).ColumnMeta.HasIndex(IndexTypeMeta.Indexed);
            Meta.EnablePhantoms();
        }
    }
}
