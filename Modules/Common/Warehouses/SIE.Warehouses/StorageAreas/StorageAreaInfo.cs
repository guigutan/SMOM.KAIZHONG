using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Warehouses
{
    /// <summary>
    /// 库区基本资料
    /// </summary>
    [RootEntity, Serializable]
    [Label("库区基本资料")]
    [DisplayMember(nameof(StorageAreaInfo.Id))]
    public partial class StorageAreaInfo : DataEntity
    {
        #region 库区面积(㎡) Area
        /// <summary>
        /// 库区面积(㎡)
        /// </summary>
        [MinValue(0)]
        [Label("库区面积(㎡)")]
        public static readonly Property<decimal?> AreaProperty = P<StorageAreaInfo>.Register(e => e.Area);

        /// <summary>
        /// 库区面积(㎡)
        /// </summary>
        public decimal? Area
        {
            get { return GetProperty(AreaProperty); }
            set { SetProperty(AreaProperty, value); }
        }
        #endregion

        #region 库区容积(CBM) Volume
        /// <summary>
        /// 库区容积(CBM)
        /// </summary>
        [MinValue(0)]
        [Label("库区容积(CBM)")]
        public static readonly Property<decimal?> VolumeProperty = P<StorageAreaInfo>.Register(e => e.Volume);

        /// <summary>
        /// 库区容积(CBM)
        /// </summary>
        public decimal? Volume
        {
            get { return GetProperty(VolumeProperty); }
            set { SetProperty(VolumeProperty, value); }
        }
        #endregion

        #region 起点X坐标 StartPointX
        /// <summary>
        /// 起点X坐标
        /// </summary>
        [Label("起点X坐标")]
        public static readonly Property<decimal?> StartPointXProperty = P<StorageAreaInfo>.Register(e => e.StartPointX);

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
        public static readonly Property<decimal?> StartPointYProperty = P<StorageAreaInfo>.Register(e => e.StartPointY);

        /// <summary>
        /// 起点Y坐标
        /// </summary>
        public decimal? StartPointY
        {
            get { return GetProperty(StartPointYProperty); }
            set { SetProperty(StartPointYProperty, value); }
        }
        #endregion

        #region 库区 StorageArea
        /// <summary>
        /// 库区Id
        /// </summary>
        [Label("库区")]
        public static readonly IRefIdProperty StorageAreaIdProperty = P<StorageAreaInfo>.RegisterRefId(e => e.StorageAreaId, ReferenceType.Normal);

        /// <summary>
        /// 库区Id
        /// </summary>
        public double StorageAreaId
        {
            get { return (double)GetRefId(StorageAreaIdProperty); }
            set { SetRefId(StorageAreaIdProperty, value); }
        }

        /// <summary>
        /// 库区
        /// </summary>
        public static readonly RefEntityProperty<StorageArea> StorageAreaProperty = P<StorageAreaInfo>.RegisterRef(e => e.StorageArea, StorageAreaIdProperty);

        /// <summary>
        /// 库区
        /// </summary>
        public StorageArea StorageArea
        {
            get { return GetRefEntity(StorageAreaProperty); }
            set { SetRefEntity(StorageAreaProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 库区基本资料 实体配置
    /// </summary>
    internal class StorageAreaInfoConfig : EntityConfig<StorageAreaInfo>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WH_AREA_INFO").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}