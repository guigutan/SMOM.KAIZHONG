using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Warehouses
{
    /// <summary>
    /// 库位操作管理
    /// </summary>
    [RootEntity, Serializable]
    [Label("操作管理")]
    public partial class StorageLocationOperation : DataEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public StorageLocationOperation()
        {
            IsPick = true;
            IsLayIn = true;
            UpOrderIndex = 1;
            PickOrderIndex = 1;
            UpProcess = UpProcessType.MixedUp;
            PickProcess = PickProcessType.MixedPick;
        }
        #region 是否储存 IsLayIn
        /// <summary>
        /// 是否储存
        /// </summary>
        [Label("是否储存")]
        public static readonly Property<bool> IsLayInProperty = P<StorageLocationOperation>.Register(e => e.IsLayIn);

        /// <summary>
        /// 是否储存
        /// </summary>
        public bool IsLayIn
        {
            get { return GetProperty(IsLayInProperty); }
            set { SetProperty(IsLayInProperty, value); }
        }
        #endregion

        #region 是否拣货 IsPick
        /// <summary>
        /// 是否拣货
        /// </summary>
        [Label("是否拣货")]
        public static readonly Property<bool> IsPickProperty = P<StorageLocationOperation>.Register(e => e.IsPick);

        /// <summary>
        /// 是否拣货
        /// </summary>
        public bool IsPick
        {
            get { return GetProperty(IsPickProperty); }
            set { SetProperty(IsPickProperty, value); }
        }
        #endregion

        #region 发货暂存 IsFocus
        /// <summary>
        /// 发货暂存
        /// </summary>
        [Label("发货暂存")]
        public static readonly Property<bool> IsFocusProperty = P<StorageLocationOperation>.Register(e => e.IsFocus);

        /// <summary>
        /// 发货暂存
        /// </summary>
        public bool IsFocus
        {
            get { return GetProperty(IsFocusProperty); }
            set { SetProperty(IsFocusProperty, value); }
        }
        #endregion

        #region 收货暂存 IsTemporary
        /// <summary>
        /// 收货暂存
        /// </summary>
        [Label("收货暂存")]
        public static readonly Property<bool> IsTemporaryProperty = P<StorageLocationOperation>.Register(e => e.IsTemporary);

        /// <summary>
        /// 收货暂存
        /// </summary>
        public bool IsTemporary
        {
            get { return GetProperty(IsTemporaryProperty); }
            set { SetProperty(IsTemporaryProperty, value); }
        }
        #endregion

        #region 上架顺序 UpOrderIndex
        /// <summary>
        /// 上架顺序
        /// </summary>
        [Label("上架顺序")]
        [Required]
        public static readonly Property<int> UpOrderIndexProperty = P<StorageLocationOperation>.Register(e => e.UpOrderIndex);

        /// <summary>
        /// 上架顺序
        /// </summary>
        public int UpOrderIndex
        {
            get { return GetProperty(UpOrderIndexProperty); }
            set { SetProperty(UpOrderIndexProperty, value); }
        }
        #endregion

        #region 拣货顺序 PickOrderIndex
        /// <summary>
        /// 拣货顺序
        /// </summary>
        [Label("拣货顺序")]
        [Required]
        public static readonly Property<int> PickOrderIndexProperty = P<StorageLocationOperation>.Register(e => e.PickOrderIndex);

        /// <summary>
        /// 拣货顺序
        /// </summary>
        public int PickOrderIndex
        {
            get { return GetProperty(PickOrderIndexProperty); }
            set { SetProperty(PickOrderIndexProperty, value); }
        }
        #endregion

        #region 库位 StorageLocation
        /// <summary>
        /// 库位Id
        /// </summary>
        [Label("库位")]
        [NotDuplicate]
        public static readonly IRefIdProperty StorageLocationIdProperty = P<StorageLocationOperation>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty = P<StorageLocationOperation>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return GetRefEntity(StorageLocationProperty); }
            set { SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion

        #region 上架处理 UpProcessType
        /// <summary>
        /// 上架处理
        /// </summary>
        [Label("上架处理")]
        [Required]
        public static readonly Property<UpProcessType> UpProcessProperty = P<StorageLocationOperation>.Register(e => e.UpProcess);

        /// <summary>
        /// 上架处理
        /// </summary>
        public UpProcessType UpProcess
        {
            get { return GetProperty(UpProcessProperty); }
            set { SetProperty(UpProcessProperty, value); }
        }
        #endregion

        #region 拣货处理 PickProcessType
        /// <summary>
        /// 拣货处理
        /// </summary>
        [Label("拣货处理")]
        [Required]
        public static readonly Property<PickProcessType> PickProcessProperty = P<StorageLocationOperation>.Register(e => e.PickProcess);

        /// <summary>
        /// 拣货处理
        /// </summary>
        public PickProcessType PickProcess
        {
            get { return GetProperty(PickProcessProperty); }
            set { SetProperty(PickProcessProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 操作管理 实体配置
    /// </summary>
    internal class StorageLocationOperationConfig : EntityConfig<StorageLocationOperation>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WH_LOCATION_OPER").MapAllProperties();
            Meta.Property(StorageLocationOperation.StorageLocationIdProperty).ColumnMeta.HasIndex(IndexTypeMeta.Indexed);
            Meta.EnablePhantoms();
        }
    }
}
