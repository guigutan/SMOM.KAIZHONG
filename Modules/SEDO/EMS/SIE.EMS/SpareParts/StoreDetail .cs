using SIE.Domain;
using SIE.EMS.SpareParts.Enums;
using SIE.EMS.SpareParts.OutDepots.Details;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.EMS.SpareParts
{
    /// <summary>
    /// 入库明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("入库明细")]
    public partial class StoreDetail : DataEntity
    {
        #region 行号 LineNo
        /// <summary>
        /// 行号
        /// </summary>
        [Label("行号")]
        public static readonly Property<string> LineNoProperty = P<StoreDetail>.Register(e => e.LineNo);

        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo
        {
            get { return GetProperty(LineNoProperty); }
            set { SetProperty(LineNoProperty, value); }
        }
        #endregion

        #region 批次号 BatchNumber
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> BatchNumberProperty = P<StoreDetail>.Register(e => e.BatchNumber);
        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNumber
        {
            get { return GetProperty(BatchNumberProperty); }
            set { SetProperty(BatchNumberProperty, value); }
        }
        #endregion

        #region 数量 Number
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<int> NumberProperty = P<StoreDetail>.Register(e => e.Number);
        /// <summary>
        /// 数量
        /// </summary>
        public int Number
        {
            get { return GetProperty(NumberProperty); }
            set { SetProperty(NumberProperty, value); }
        }
        #endregion

        #region 单价 UnitPrice
        /// <summary>
        /// 单价
        /// </summary>
        [Label("单价")]
        public static readonly Property<decimal?> UnitPriceProperty = P<StoreDetail>.Register(e => e.UnitPrice);
        /// <summary>
        /// 单价
        /// </summary>
        public decimal? UnitPrice
        {
            get { return GetProperty(UnitPriceProperty); }
            set { SetProperty(UnitPriceProperty, value); }
        }
        #endregion

        #region 备件基础数据 SparePart
        /// <summary>
        /// 备件基础数据Id
        /// </summary>
        [Label("备件编码")]
        public static readonly IRefIdProperty SparePartIdProperty = P<StoreDetail>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

        /// <summary>
        /// 备件基础数据Id
        /// </summary>
        public double SparePartId
        {
            get { return (double)GetRefId(SparePartIdProperty); }
            set { SetRefId(SparePartIdProperty, value); }
        }

        /// <summary>
        /// 备件基础数据
        /// </summary>
        public static readonly RefEntityProperty<SparePart> SparePartProperty = P<StoreDetail>.RegisterRef(e => e.SparePart, SparePartIdProperty);

        /// <summary>
        /// 备件基础数据
        /// </summary>
        public SparePart SparePart
        {
            get { return GetRefEntity(SparePartProperty); }
            set { SetRefEntity(SparePartProperty, value); }
        }
        #endregion

        #region 序列号 Sn
        /// <summary>
        /// 序列号
        /// </summary>
        [Label("序列号")]
        public static readonly Property<string> SnProperty = P<StoreDetail>.Register(e => e.Sn);

        /// <summary>
        /// 序列号
        /// </summary>
        public string Sn
        {
            get { return GetProperty(SnProperty); }
            set { SetProperty(SnProperty, value); }
        }
        #endregion

        #region 备件入库 SparePartStore
        /// <summary>
        /// 备件入库Id
        /// </summary>
        public static readonly IRefIdProperty SparePartStoreIdProperty = P<StoreDetail>.RegisterRefId(e => e.SparePartStoreId, ReferenceType.Parent);

        /// <summary>
        /// 备件入库Id
        /// </summary>
        public double SparePartStoreId
        {
            get { return (double)GetRefId(SparePartStoreIdProperty); }
            set { SetRefId(SparePartStoreIdProperty, value); }
        }

        /// <summary>
        /// 备件入库
        /// </summary>
        public static readonly RefEntityProperty<SparePartStore> SparePartStoreProperty = P<StoreDetail>.RegisterRef(e => e.SparePartStore, SparePartStoreIdProperty);

        /// <summary>
        /// 备件入库
        /// </summary>
        public SparePartStore SparePartStore
        {
            get { return GetRefEntity(SparePartStoreProperty); }
            set { SetRefEntity(SparePartStoreProperty, value); }
        }
        #endregion

        #region 出库单明细 PartOutDepotDetail
        /// <summary>
        /// 出库单明细Id
        /// </summary>
        [Label("出库单号-行号")]
        public static readonly IRefIdProperty PartOutDepotDetailIdProperty = P<StoreDetail>.RegisterRefId(e => e.PartOutDepotDetailId, ReferenceType.Normal);

        /// <summary>
        /// 出库单明细Id
        /// </summary>
        public double? PartOutDepotDetailId
        {
            get { return (double?)GetRefNullableId(PartOutDepotDetailIdProperty); }
            set { SetRefNullableId(PartOutDepotDetailIdProperty, value); }
        }

        /// <summary>
        /// 出库单明细
        /// </summary>
        public static readonly RefEntityProperty<PartOutDepotDetail> PartOutDepotDetailProperty = P<StoreDetail>.RegisterRef(e => e.PartOutDepotDetail, PartOutDepotDetailIdProperty);

        /// <summary>
        /// 出库单明细
        /// </summary>
        public PartOutDepotDetail PartOutDepotDetail
        {
            get { return GetRefEntity(PartOutDepotDetailProperty); }
            set { SetRefEntity(PartOutDepotDetailProperty, value); }
        }
        #endregion

        #region 出库单行号 OutDepotLineNo
        /// <summary>
        /// 出库单行号
        /// </summary>
        [Label("出库单号-行号")]
        public static readonly Property<string> OutDepotLineNoProperty = P<StoreDetail>.Register(e => e.OutDepotLineNo);

        /// <summary>
        /// 出库单行号
        /// </summary>
        public string OutDepotLineNo
        {
            get { return this.GetProperty(OutDepotLineNoProperty); }
            set { this.SetProperty(OutDepotLineNoProperty, value); }
        }
        #endregion

        #region 质量状态 QualityStatus
        /// <summary>
        /// 质量状态
        /// </summary>
        [Label("质量状态")]
        public static readonly Property<QualityStatus> QualityStatusProperty = P<StoreDetail>.Register(e => e.QualityStatus);

        /// <summary>
        /// 质量状态
        /// </summary>
        public QualityStatus QualityStatus
        {
            get { return GetProperty(QualityStatusProperty); }
            set { SetProperty(QualityStatusProperty, value); }
        }
        #endregion

        #region 入库库位 StorageLocation
        /// <summary>
        /// 入库库位Id
        /// </summary>
        [Label("入库库位")]
        public static readonly IRefIdProperty StorageLocationIdProperty =
            P<StoreDetail>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

        /// <summary>
        /// 入库库位Id
        /// </summary>
        public double? StorageLocationId
        {
            get { return (double?)this.GetRefNullableId(StorageLocationIdProperty); }
            set { this.SetRefNullableId(StorageLocationIdProperty, value); }
        }

        /// <summary>
        /// 入库库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty =
            P<StoreDetail>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 入库库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return this.GetRefEntity(StorageLocationProperty); }
            set { this.SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion

        #region 拆机件 IsOldPart
        /// <summary>
        /// 拆机件
        /// </summary>
        [Label("拆机件")]
        public static readonly Property<bool> IsOldPartProperty = P<StoreDetail>.Register(e => e.IsOldPart);

        /// <summary>
        /// 拆机件
        /// </summary>
        public bool IsOldPart
        {
            get { return this.GetProperty(IsOldPartProperty); }
            set { this.SetProperty(IsOldPartProperty, value); }
        }
        #endregion

        #region 状态 InboundStatus
        /// <summary>
        /// 入库状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<InboundStatus> InboundStatusProperty
            = P<StoreDetail>.Register(e => e.InboundStatus);

        /// <summary>
        /// 入库状态
        /// </summary>
        public InboundStatus InboundStatus
        {
            get { return GetProperty(InboundStatusProperty); }
            set { SetProperty(InboundStatusProperty, value); }
        }
        #endregion

        #region 采购单号 PurchaseOrderNo
        /// <summary>
        /// 采购单号
        /// </summary>
        [Label("采购单号")]
        public static readonly Property<string> PurchaseOrderNoProperty = P<StoreDetail>.Register(e => e.PurchaseOrderNo);

        /// <summary>
        /// 采购单号
        /// </summary>
        public string PurchaseOrderNo
        {
            get { return this.GetProperty(PurchaseOrderNoProperty); }
            set { this.SetProperty(PurchaseOrderNoProperty, value); }
        }
        #endregion

        #region 采购单行号 PurchaseOrderLineNo
        /// <summary>
        /// 采购单行号
        /// </summary>
        [Label("采购单行号")]
        public static readonly Property<string> PurchaseOrderLineNoProperty = P<StoreDetail>.Register(e => e.PurchaseOrderLineNo);

        /// <summary>
        /// 采购单行号
        /// </summary>
        public string PurchaseOrderLineNo
        {
            get { return this.GetProperty(PurchaseOrderLineNoProperty); }
            set { this.SetProperty(PurchaseOrderLineNoProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 备件编码 SparePartCode
        /// <summary>
        /// 备件编码
        /// </summary>
        [Label("备件编码")]
        public static readonly Property<string> SparePartCodeProperty = P<StoreDetail>.RegisterView(e => e.SparePartCode, p => p.SparePart.SparePartCode);

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparePartCode
        {
            get { return GetProperty(SparePartCodeProperty); }
            set { SetProperty(SparePartCodeProperty, value); }
        }
        #endregion

        #region 备件名称 SparePartName
        /// <summary>
        /// 备件名称
        /// </summary>
        [Label("备件名称")]
        public static readonly Property<string> SparePartNameProperty = P<StoreDetail>.RegisterView(e => e.SparePartName, p => p.SparePart.SparePartName);

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName
        {
            get { return GetProperty(SparePartNameProperty); }
            set { SetProperty(SparePartNameProperty, value); }
        }
        #endregion

        #region 规格型号 Specification
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> SpecificationProperty = P<StoreDetail>.RegisterView(e => e.Specification, p => p.SparePart.Specification);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string Specification
        {
            get { return GetProperty(SpecificationProperty); }
            set { SetProperty(SpecificationProperty, value); }
        }
        #endregion

        #region 类型 SpartType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<SparePartType> SpartTypeProperty = P<StoreDetail>.RegisterView(e => e.SpartType, p => p.SparePart.SpartType);

        /// <summary>
        /// 类型
        /// </summary>
        public SparePartType SpartType
        {
            get { return GetProperty(SpartTypeProperty); }
            set { SetProperty(SpartTypeProperty, value); }
        }
        #endregion

        #region 设备型号 EquipModelName
        /// <summary>
        /// 设备型号
        /// </summary>
        [Label("设备型号")]
        public static readonly Property<string> EquipModelNameProperty = P<StoreDetail>.RegisterView(e => e.EquipModelName, p => p.SparePart.SpartEquipModel.Name);

        /// <summary>
        /// 设备型号
        /// </summary>
        public string EquipModelName
        {
            get { return GetProperty(EquipModelNameProperty); }
            set { SetProperty(EquipModelNameProperty, value); }
        }
        #endregion

        #region 管控方式 ControlMethod
        /// <summary>
        /// 管控方式
        /// </summary>
        [Label("管控方式")]
        public static readonly Property<ControlMethod> ControlMethodProperty = P<StoreDetail>.RegisterView(e => e.ControlMethod, p => p.SparePart.ControlMethod);

        /// <summary>
        /// 管控方式
        /// </summary>
        public ControlMethod ControlMethod
        {
            get { return this.GetProperty(ControlMethodProperty); }
        }
        #endregion

        #region 以旧换新 IsReplacement
        /// <summary>
        /// 以旧换新
        /// </summary>
        [Label("以旧换新")]
        public static readonly Property<bool> IsReplacementProperty = P<StoreDetail>.RegisterView(e => e.IsReplacement, p => p.SparePart.IsReplacement);

        /// <summary>
        /// 以旧换新
        /// </summary>
        public bool IsReplacement
        {
            get { return this.GetProperty(IsReplacementProperty); }
        }
        #endregion

        #region 单价 SparePartUnitPrice
        /// <summary>
        /// 单价
        /// </summary>
        [Label("单价")]
        public static readonly Property<decimal?> SparePartUnitPriceProperty = P<StoreDetail>.RegisterView(e => e.SparePartUnitPrice, p => p.SparePart.UnitPrice);

        /// <summary>
        /// 单价
        /// </summary>
        public decimal? SparePartUnitPrice
        {
            get { return this.GetProperty(SparePartUnitPriceProperty); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<StoreDetail>.RegisterView(e => e.UnitName, p => p.SparePart.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
        }
        #endregion

        #region 仓库Id WarehouseId
        /// <summary>
        /// 仓库Id 入库统计使用
        /// </summary>
        [Label("仓库Id")]
        public static readonly Property<double> WarehouseIdProperty = P<StoreDetail>.RegisterView(e => e.WarehouseId, p => p.SparePartStore.WarehouseId);

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double WarehouseId
        {
            get { return this.GetProperty(WarehouseIdProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 入库明细 实体配置
    /// </summary>
    internal class StoreDetailConfig : EntityConfig<StoreDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_SPARE_PART_STR_DTL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
