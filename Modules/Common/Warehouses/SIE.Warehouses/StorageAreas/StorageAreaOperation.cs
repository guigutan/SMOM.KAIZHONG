using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Warehouses
{
    /// <summary>
    /// 库区操作管理
    /// </summary>
    [RootEntity, Serializable]
    [Label("库区操作管理")]
    [DisplayMember(nameof(StorageAreaOperation.Id))]
    public partial class StorageAreaOperation : DataEntity
    {
        #region 下架过渡库位 DownTransitLocation
        /// <summary>
        /// 下架过渡库位Id
        /// </summary>
        [Label("下架过渡库位")]
        public static readonly IRefIdProperty DownTransitLocationIdProperty = P<StorageAreaOperation>.RegisterRefId(e => e.DownTransitLocationId, ReferenceType.Normal);

        /// <summary>
        /// 下架过渡库位Id
        /// </summary>
        public double? DownTransitLocationId
        {
            get { return (double?)GetRefNullableId(DownTransitLocationIdProperty); }
            set { SetRefNullableId(DownTransitLocationIdProperty, value); }
        }

        /// <summary>
        /// 下架过渡库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> DownTransitLocationProperty = P<StorageAreaOperation>.RegisterRef(e => e.DownTransitLocation, DownTransitLocationIdProperty);

        /// <summary>
        /// 下架过渡库位
        /// </summary>
        public StorageLocation DownTransitLocation
        {
            get { return GetRefEntity(DownTransitLocationProperty); }
            set { SetRefEntity(DownTransitLocationProperty, value); }
        }
        #endregion

        #region 库区 StorageArea
        /// <summary>
        /// 库区Id
        /// </summary>
        [Label("库区")]
        public static readonly IRefIdProperty StorageAreaIdProperty = P<StorageAreaOperation>.RegisterRefId(e => e.StorageAreaId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<StorageArea> StorageAreaProperty = P<StorageAreaOperation>.RegisterRef(e => e.StorageArea, StorageAreaIdProperty);

        /// <summary>
        /// 库区
        /// </summary>
        public StorageArea StorageArea
        {
            get { return GetRefEntity(StorageAreaProperty); }
            set { SetRefEntity(StorageAreaProperty, value); }
        }
        #endregion

        #region 上架过渡库位 UpTransitLocation
        /// <summary>
        /// 上架过渡库位Id
        /// </summary>
        [Label("上架过渡库位")]
        public static readonly IRefIdProperty UpTransitLocationIdProperty = P<StorageAreaOperation>.RegisterRefId(e => e.UpTransitLocationId, ReferenceType.Normal);

        /// <summary>
        /// 上架过渡库位Id
        /// </summary>
        public double? UpTransitLocationId
        {
            get { return (double?)GetRefNullableId(UpTransitLocationIdProperty); }
            set { SetRefNullableId(UpTransitLocationIdProperty, value); }
        }

        /// <summary>
        /// 上架过渡库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> UpTransitLocationProperty = P<StorageAreaOperation>.RegisterRef(e => e.UpTransitLocation, UpTransitLocationIdProperty);

        /// <summary>
        /// 上架过渡库位
        /// </summary>
        public StorageLocation UpTransitLocation
        {
            get { return GetRefEntity(UpTransitLocationProperty); }
            set { SetRefEntity(UpTransitLocationProperty, value); }
        }
        #endregion
    }

    /// <summary>
    ///  实体配置
    /// </summary>
    internal class StorageAreaOperationConfig : EntityConfig<StorageAreaOperation>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WH_AREA_OPER").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}