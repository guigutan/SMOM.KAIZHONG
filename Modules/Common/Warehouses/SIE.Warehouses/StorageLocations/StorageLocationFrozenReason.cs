using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Warehouses
{
    /// <summary>
    /// 库位冻结原因
    /// </summary>
    [RootEntity, Serializable]
    [Label("库位冻结原因")]
    public class StorageLocationFrozenReason : DataEntity
    {
        #region 库位 StorageLocation
        /// <summary>
        /// 库位Id
        /// </summary>
        [Label("库位")]
        [NotDuplicate]
        public static readonly IRefIdProperty StorageLocationIdProperty = P<StorageLocationFrozenReason>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Parent);

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
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty = P<StorageLocationFrozenReason>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return GetRefEntity(StorageLocationProperty); }
            set { SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion

        #region 冻结原因 FrozenReason
        /// <summary>
        /// 冻结原因
        /// </summary>
        [Label("冻结原因")]
        public static readonly Property<FrozenReason> FrozenReasonProperty = P<StorageLocationFrozenReason>.Register(e => e.FrozenReason);

        /// <summary>
        /// 冻结原因
        /// </summary>
        public FrozenReason FrozenReason
        {
            get { return GetProperty(FrozenReasonProperty); }
            set { SetProperty(FrozenReasonProperty, value); }
        }
        #endregion

        #region 原因描述 ReasonDesc
        /// <summary>
        /// 原因描述
        /// </summary>
        [Label("原因描述")]
        public static readonly Property<string> ReasonDescProperty = P<StorageLocationFrozenReason>.Register(e => e.ReasonDesc);

        /// <summary>
        /// 原因描述
        /// </summary>
        public string ReasonDesc
        {
            get { return GetProperty(ReasonDescProperty); }
            set { SetProperty(ReasonDescProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 操作管理 实体配置
    /// </summary>
    internal class StorageLocationFrozenReasonConfig : EntityConfig<StorageLocationFrozenReason>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WH_LOC_FROZEN_REASON").MapAllProperties();
            Meta.Property(StorageLocationFrozenReason.StorageLocationIdProperty).ColumnMeta.HasIndex(IndexTypeMeta.Indexed);
            Meta.EnablePhantoms();
        }
    }
}
