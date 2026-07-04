using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.SpareParts.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.EMS.SpareParts
{
    /// <summary>
    /// 批次明细
    /// </summary>
    [ChildEntity, Serializable]
    [DisplayMember(nameof(BatchNumber))]
    [Label("批次明细")]
    public partial class StoreSummaryLot : DataEntity
    {
        #region 备件库存 StoreSummary
        /// <summary>
        /// 备件库存Id
        /// </summary>
        public static readonly IRefIdProperty StoreSummaryIdProperty = P<StoreSummaryLot>.RegisterRefId(e => e.StoreSummaryId, ReferenceType.Parent);

        /// <summary>
        /// 备件库存Id
        /// </summary>
        public double StoreSummaryId
        {
            get { return (double)GetRefId(StoreSummaryIdProperty); }
            set { SetRefId(StoreSummaryIdProperty, value); }
        }

        /// <summary>
        /// 备件库存
        /// </summary>
        public static readonly RefEntityProperty<StoreSummary> StoreSummaryProperty = P<StoreSummaryLot>.RegisterRef(e => e.StoreSummary, StoreSummaryIdProperty);

        /// <summary>
        /// 备件库存
        /// </summary>
        public StoreSummary StoreSummary
        {
            get { return GetRefEntity(StoreSummaryProperty); }
            set { SetRefEntity(StoreSummaryProperty, value); }
        }
        #endregion
        
        #region 批次号 BatchNumber
        /// <summary>
        /// 批次号
        /// </summary>
        [Required]
        [Label("批次号")]
        public static readonly Property<string> BatchNumberProperty = P<StoreSummaryLot>.Register(e => e.BatchNumber);
        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNumber
        {
            get { return GetProperty(BatchNumberProperty); }
            set { SetProperty(BatchNumberProperty, value); }
        }
        #endregion

        #region 不良品数 RotNumber
        /// <summary>
        /// 不良品数
        /// </summary>
        [Label("不良品数")]
        public static readonly Property<int> RotNumberProperty = P<StoreSummaryLot>.Register(e => e.RotNumber);

        /// <summary>
        /// 不良品数
        /// </summary>
        public int RotNumber
        {
            get { return GetProperty(RotNumberProperty); }
            set { SetProperty(RotNumberProperty, value); }
        }
        #endregion

        #region 可用库存 GoodNumber
        /// <summary>
        /// 可用库存
        /// </summary>
        [Label("可用库存")]
        public static readonly Property<int> GoodNumberProperty = P<StoreSummaryLot>.Register(e => e.GoodNumber);

        /// <summary>
        /// 可用库存
        /// </summary>
        public int GoodNumber
        {
            get { return GetProperty(GoodNumberProperty); }
            set { SetProperty(GoodNumberProperty, value); }
        }
        #endregion

        #region 总库存 SumNumber
        /// <summary>
        /// 总库存
        /// </summary>
        [Label("总库存")]
        public static readonly Property<int> SumNumberProperty = P<StoreSummaryLot>.Register(e => e.SumNumber);

        /// <summary>
        /// 总库存
        /// </summary>
        public int SumNumber
        {
            get { return GetProperty(SumNumberProperty); }
            set { SetProperty(SumNumberProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库编码")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<StoreSummaryLot>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double WarehouseId
        {
            get { return (double)GetRefId(WarehouseIdProperty); }
            set { SetRefId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<StoreSummaryLot>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 零成本仓 IsZeroCost
        /// <summary>
        /// 零成本仓
        /// </summary>
        [Label("零成本仓")]
        public static readonly Property<bool> IsZeroCostProperty = P<StoreSummaryLot>.Register(e => e.IsZeroCost);

        /// <summary>
        /// 零成本仓
        /// </summary>
        public bool IsZeroCost
        {
            get { return this.GetProperty(IsZeroCostProperty); }
            set { this.SetProperty(IsZeroCostProperty, value); }
        }
        #endregion

        #region 库位 StorageLocation
        /// <summary>
        /// 库位Id
        /// </summary>
        [Label("库位编码")]
        public static readonly IRefIdProperty StorageLocationIdProperty = P<StoreSummaryLot>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty = P<StoreSummaryLot>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return GetRefEntity(StorageLocationProperty); }
            set { SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion

        #region 入库日期 InboundDate
        /// <summary>
        /// 入库日期
        /// </summary>
        [Label("原始入库日期")]
        public static readonly Property<DateTime?> InboundDateProperty = P<StoreSummaryLot>.Register(e => e.InboundDate);

        /// <summary>
        /// 入库日期
        /// </summary>
        public DateTime? InboundDate
        {
            get { return GetProperty(InboundDateProperty); }
            set { SetProperty(InboundDateProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 仓库编码 DepotCode
        /// <summary>
        /// 仓库编码
        /// </summary>
        [Label("仓库编码")]
        public static readonly Property<string> DepotCodeProperty = P<StoreSummaryLot>.RegisterView(e => e.DepotCode, p => p.Warehouse.Name);

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string DepotCode
        {
            get { return this.GetProperty(DepotCodeProperty); }
        }
        #endregion

        #region 库位编码 SiteCode
        /// <summary>
        /// 库位编码
        /// </summary>
        [Label("库位编码")]
        public static readonly Property<string> SiteCodeProperty = P<StoreSummaryLot>.RegisterView(e => e.SiteCode, p => p.StorageLocation.Code);

        /// <summary>
        /// 库位编码
        /// </summary>
        public string SiteCode
        {
            get { return this.GetProperty(SiteCodeProperty); }
        }
        #endregion

        #region 仓库名称 WarehouseName
        /// <summary>
        /// 仓库名称
        /// </summary>
        [Label("仓库名称")]
        public static readonly Property<string> WarehouseNameProperty
            = P<StoreSummaryLot>.RegisterView(e => e.WarehouseName, p => p.Warehouse.Name);

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName
        {
            get { return this.GetProperty(WarehouseNameProperty); }
        }
        #endregion

        #region 分类 LibraryType
        /// <summary>
        /// 分类
        /// </summary>
        [Label("分类")]
        public static readonly Property<LibraryType> LibraryTypeProperty = P<StoreSummaryLot>.RegisterView(e => e.LibraryType, p => p.Warehouse.LibraryType);

        /// <summary>
        /// 分类
        /// </summary>
        public LibraryType LibraryType
        {
            get { return this.GetProperty(LibraryTypeProperty); }
        }
        #endregion

        #region 库位名称 StorageLocationName
        /// <summary>
        /// 库位名称
        /// </summary>
        [Label("库位名称")]
        public static readonly Property<string> StorageLocationNameProperty
            = P<StoreSummaryLot>.RegisterView(e => e.StorageLocationName, p => p.StorageLocation.Name);

        /// <summary>
        /// 库位名称
        /// </summary>
        public string StorageLocationName
        {
            get { return this.GetProperty(StorageLocationNameProperty); }
        }
        #endregion

        #region 备件Id SparePartId
        /// <summary>
        /// 备件Id
        /// </summary>
        [Label("备件ID")]
        public static readonly Property<double> SparePartIdProperty = P<StoreSummaryLot>.RegisterView(e => e.SparePartId, p => p.StoreSummary.SparePartId);

        /// <summary>
        /// 备件Id
        /// </summary>
        public double SparePartId
        {
            get { return this.GetProperty(SparePartIdProperty); }
        }
        #endregion

        #region 备件编码 SparePartCode
        /// <summary>
        /// 备件编码
        /// </summary>
        [Label("备件编码")]
        public static readonly Property<string> SparePartCodeProperty = P<StoreSummaryLot>.RegisterView(e => e.SparePartCode, p => p.StoreSummary.SparePart.SparePartCode);

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparePartCode
        {
            get { return this.GetProperty(SparePartCodeProperty); }            
            set { SetProperty(SparePartCodeProperty, value); }
        }
        #endregion


        #region 备件名称 SparePartName
        /// <summary>
        /// 备件名称
        /// </summary>
        [Label("备件名称")]
        public static readonly Property<string> SparePartNameProperty = P<StoreSummaryLot>.RegisterView(e => e.SparePartName, p => p.StoreSummary.SparePart.SparePartName);

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName
        {
            get { return this.GetProperty(SparePartNameProperty); }
            set { SetProperty(SparePartNameProperty, value); }
        }
        #endregion

        #region 管控方式 ControlMethod
        /// <summary>
        /// 管控方式
        /// </summary>
        [Label("管控方式")]
        public static readonly Property<ControlMethod> ControlMethodProperty = P<StoreSummaryLot>.RegisterView(e => e.ControlMethod, p => p.StoreSummary.SparePart.ControlMethod);

        /// <summary>
        /// 管控方式
        /// </summary>
        public ControlMethod ControlMethod
        {
            get { return this.GetProperty(ControlMethodProperty); }
        }
        #endregion


        #endregion
    }

    /// <summary>
    /// 仓库明细 实体配置
    /// </summary>
    internal class StoreSummaryDepotConfig : EntityConfig<StoreSummaryLot>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(StoreSummaryLot.GoodNumberProperty, new NumberRangeRule() { Min = 0 });
            rules.AddRule(StoreSummaryLot.SumNumberProperty, new NumberRangeRule() { Min = 0 });
        }

        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_SPARE_PART_SUMR_DPO").MapAllProperties();
            Meta.Property(StoreSummaryLot.IsZeroCostProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}
