using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Warehouses
{
    /// <summary>
    /// 库位仓储资料
    /// </summary>
    [RootEntity, Serializable]
    [Label("仓储资料")]
    public partial class StorageLocationLayinInfo : DataEntity
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public StorageLocationLayinInfo()
        {
            TemperatureType = TemperatureType.Custom;
            HumidityType = HumidityType.Custom;
        }

        /// <summary>
        /// RoHS快码类型
        /// </summary>
        public const string ROHSLEVEL = "ROHS_LEVEL";

        #region 储存温度上限 TemperatureUpper
        /// <summary>
        /// 储存温度上限
        /// </summary>
        [Label("储存温度上限")]
        public static readonly Property<decimal?> TemperatureUpperProperty = P<StorageLocationLayinInfo>.Register(e => e.TemperatureUpper);

        /// <summary>
        /// 储存温度上限
        /// </summary>
        public decimal? TemperatureUpper
        {
            get { return GetProperty(TemperatureUpperProperty); }
            set { SetProperty(TemperatureUpperProperty, value); }
        }
        #endregion

        #region 储存温度下限 TemperatureLower
        /// <summary>
        /// 储存温度下限
        /// </summary>
        [Label("储存温度下限")]
        public static readonly Property<decimal?> TemperatureLowerProperty = P<StorageLocationLayinInfo>.Register(e => e.TemperatureLower);

        /// <summary>
        /// 储存温度下限
        /// </summary>
        public decimal? TemperatureLower
        {
            get { return GetProperty(TemperatureLowerProperty); }
            set { SetProperty(TemperatureLowerProperty, value); }
        }
        #endregion

        #region 储存湿度上限 HumidityUpper
        /// <summary>
        /// 储存湿度上限
        /// </summary>
        [Label("储存湿度上限")]
        public static readonly Property<decimal?> HumidityUpperProperty = P<StorageLocationLayinInfo>.Register(e => e.HumidityUpper);

        /// <summary>
        /// 储存湿度上限
        /// </summary>
        public decimal? HumidityUpper
        {
            get { return GetProperty(HumidityUpperProperty); }
            set { SetProperty(HumidityUpperProperty, value); }
        }
        #endregion

        #region 储存湿度下限 HumidityLower
        /// <summary>
        /// 储存湿度下限
        /// </summary>
        [Label("储存湿度下限")]
        public static readonly Property<decimal?> HumidityLowerProperty = P<StorageLocationLayinInfo>.Register(e => e.HumidityLower);

        /// <summary>
        /// 储存湿度下限
        /// </summary>
        public decimal? HumidityLower
        {
            get { return GetProperty(HumidityLowerProperty); }
            set { SetProperty(HumidityLowerProperty, value); }
        }
        #endregion

        #region RoHS等级 RoHsGradeValue
        /// <summary>
        /// RoHS等级
        /// </summary>
        [Label("RoHS等级")]
        [MaxLength(80)]
        public static readonly Property<string> RoHsGradeValueProperty = P<StorageLocationLayinInfo>.Register(e => e.RoHsGradeValue);

        /// <summary>
        /// RoHS等级
        /// </summary>
        public string RoHsGradeValue
        {
            get { return GetProperty(RoHsGradeValueProperty); }
            set { SetProperty(RoHsGradeValueProperty, value); }
        }
        #endregion

        #region 重量限制(KG) WeightLimit
        /// <summary>
        /// 重量限制(KG)
        /// </summary>
        [Label("重量限制(KG)")]
        public static readonly Property<decimal?> WeightLimitProperty = P<StorageLocationLayinInfo>.Register(e => e.WeightLimit);

        /// <summary>
        /// 重量限制(KG)
        /// </summary>
        public decimal? WeightLimit
        {
            get { return GetProperty(WeightLimitProperty); }
            set { SetProperty(WeightLimitProperty, value); }
        }
        #endregion

        #region 体积限制(M³) VolumeLimit
        /// <summary>
        /// 体积限制(M³)
        /// </summary>
        [Label("体积限制(M³)")]
        public static readonly Property<decimal?> VolumeLimitProperty = P<StorageLocationLayinInfo>.Register(e => e.VolumeLimit);

        /// <summary>
        /// 体积限制(M³)
        /// </summary>
        public decimal? VolumeLimit
        {
            get { return GetProperty(VolumeLimitProperty); }
            set { SetProperty(VolumeLimitProperty, value); }
        }
        #endregion

        #region 箱数限制 BoxCountLimit
        /// <summary>
        /// 箱数限制
        /// </summary>
        [Label("箱数限制")]
        public static readonly Property<int?> BoxCountLimitProperty = P<StorageLocationLayinInfo>.Register(e => e.BoxCountLimit);

        /// <summary>
        /// 箱数限制
        /// </summary>
        public int? BoxCountLimit
        {
            get { return GetProperty(BoxCountLimitProperty); }
            set { SetProperty(BoxCountLimitProperty, value); }
        }
        #endregion

        #region 托数限制 TrayCountLimit
        /// <summary>
        /// 托数限制
        /// </summary>
        [Label("托数限制")]
        public static readonly Property<int?> TrayCountLimitProperty = P<StorageLocationLayinInfo>.Register(e => e.TrayCountLimit);

        /// <summary>
        /// 托数限制
        /// </summary>
        public int? TrayCountLimit
        {
            get { return GetProperty(TrayCountLimitProperty); }
            set { SetProperty(TrayCountLimitProperty, value); }
        }
        #endregion

        #region 数量限制 AmountLimit
        /// <summary>
        /// 数量限制
        /// </summary>
        [Label("数量限制")]
        public static readonly Property<int?> AmountLimitProperty = P<StorageLocationLayinInfo>.Register(e => e.AmountLimit);

        /// <summary>
        /// 数量限制
        /// </summary>
        public int? AmountLimit
        {
            get { return GetProperty(AmountLimitProperty); }
            set { SetProperty(AmountLimitProperty, value); }
        }
        #endregion

        #region 专储物料库位 IsSpecialItem
        /// <summary>
        /// 专储物料库位
        /// </summary>
        [Label("专储物料库位")]
        public static readonly Property<bool> IsSpecialItemProperty = P<StorageLocationLayinInfo>.Register(e => e.IsSpecialItem);

        /// <summary>
        /// 专储物料库位
        /// </summary>
        public bool IsSpecialItem
        {
            get { return GetProperty(IsSpecialItemProperty); }
            set { SetProperty(IsSpecialItemProperty, value); }
        }
        #endregion

        #region 仅限静电敏感物料 IsElecSenGrade
        /// <summary>
        /// 仅限静电敏感物料
        /// </summary>
        [Label("仅限静电敏感物料")]
        public static readonly Property<bool> IsElecSenGradeProperty = P<StorageLocationLayinInfo>.Register(e => e.IsElecSenGrade);

        /// <summary>
        /// 仅限静电敏感物料
        /// </summary>
        public bool IsElecSenGrade
        {
            get { return GetProperty(IsElecSenGradeProperty); }
            set { SetProperty(IsElecSenGradeProperty, value); }
        }
        #endregion

        #region 仅限湿敏器件 IsHumSenGrade
        /// <summary>
        /// 仅限湿敏器件
        /// </summary>
        [Label("仅限湿敏器件")]
        public static readonly Property<bool> IsHumSenGradeProperty = P<StorageLocationLayinInfo>.Register(e => e.IsHumSenGrade);

        /// <summary>
        /// 仅限湿敏器件
        /// </summary>
        public bool IsHumSenGrade
        {
            get { return GetProperty(IsHumSenGradeProperty); }
            set { SetProperty(IsHumSenGradeProperty, value); }
        }
        #endregion

        #region 禁止混放货主 IsBanMixed
        /// <summary>
        /// 禁止混放货主
        /// </summary>
        [Label("禁止混放货主")]
        public static readonly Property<bool> IsBanMixedProperty = P<StorageLocationLayinInfo>.Register(e => e.IsBanMixed);

        /// <summary>
        /// 禁止混放货主
        /// </summary>
        public bool IsBanMixed
        {
            get { return GetProperty(IsBanMixedProperty); }
            set { SetProperty(IsBanMixedProperty, value); }
        }
        #endregion

        #region 单一SKU储存库位 IsSingleSku
        /// <summary>
        /// 单一SKU储存库位
        /// </summary>
        [Label("单一SKU储存库位")]
        public static readonly Property<bool> IsSingleSkuProperty = P<StorageLocationLayinInfo>.Register(e => e.IsSingleSku);

        /// <summary>
        /// 单一SKU储存库位
        /// </summary>
        public bool IsSingleSku
        {
            get { return GetProperty(IsSingleSkuProperty); }
            set { SetProperty(IsSingleSkuProperty, value); }
        }
        #endregion

        #region 禁止批次属性09混放 IsBanMixedBatch09
        /// <summary>
        /// 禁止批次属性09混放
        /// </summary>
        [Label("禁止批属性09混放")]
        public static readonly Property<bool> IsBanMixedBatch09Property = P<StorageLocationLayinInfo>.Register(e => e.IsBanMixedBatch09);

        /// <summary>
        /// 禁止批次属性09混放
        /// </summary>
        public bool IsBanMixedBatch09
        {
            get { return GetProperty(IsBanMixedBatch09Property); }
            set { SetProperty(IsBanMixedBatch09Property, value); }
        }
        #endregion

        #region 禁止批次属性10混放 IsBanMixedBatch10
        /// <summary>
        /// 禁止批次属性10混放
        /// </summary>
        [Label("禁止批属性10混放")]
        public static readonly Property<bool> IsBanMixedBatch10Property = P<StorageLocationLayinInfo>.Register(e => e.IsBanMixedBatch10);

        /// <summary>
        /// 禁止批次属性10混放
        /// </summary>
        public bool IsBanMixedBatch10
        {
            get { return GetProperty(IsBanMixedBatch10Property); }
            set { SetProperty(IsBanMixedBatch10Property, value); }
        }
        #endregion

        #region 储存温度类型 TemperatureType
        /// <summary>
        /// 储存温度(℃)
        /// </summary>
        [Label("储存温度(℃)")]
        [Required]
        public static readonly Property<TemperatureType> TemperatureTypeProperty = P<StorageLocationLayinInfo>.Register(e => e.TemperatureType);

        /// <summary>
        /// 储存温度(℃)
        /// </summary>
        public TemperatureType TemperatureType
        {
            get { return GetProperty(TemperatureTypeProperty); }
            set { SetProperty(TemperatureTypeProperty, value); }
        }
        #endregion

        #region 库位 StorageLocation
        /// <summary>
        /// 库位Id
        /// </summary>
        [Label("库位")]
        [NotDuplicate]
        public static readonly IRefIdProperty StorageLocationIdProperty = P<StorageLocationLayinInfo>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty = P<StorageLocationLayinInfo>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return GetRefEntity(StorageLocationProperty); }
            set { SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion

        #region 储存湿度类型 HumidityType
        /// <summary>
        /// 储存湿度(RH%)
        /// </summary>
        [Label("储存湿度(RH%)")]
        [Required]
        public static readonly Property<HumidityType> HumidityTypeProperty = P<StorageLocationLayinInfo>.Register(e => e.HumidityType);

        /// <summary>
        /// 储存湿度(RH%)
        /// </summary>
        public HumidityType HumidityType
        {
            get { return GetProperty(HumidityTypeProperty); }
            set { SetProperty(HumidityTypeProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 仓储资料 实体配置
    /// </summary>
    internal class StorageLocationLayinInfoConfig : EntityConfig<StorageLocationLayinInfo>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WH_LOCATION_LAYIN_INFO").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
