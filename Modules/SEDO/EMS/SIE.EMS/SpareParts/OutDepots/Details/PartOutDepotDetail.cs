using SIE.Domain;
using SIE.EMS.SpareParts.Enums;
using SIE.EMS.SpareParts.OutDepotHandovers;
using SIE.EMS.SpareParts.OutDepots.Enums;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.EMS.SpareParts.OutDepots.Details
{
    /// <summary>
    /// 出库单明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("出库单明细")]
    [DisplayMember(nameof(LineNo))]
    public class PartOutDepotDetail : DataEntity
    {
        #region 备件出库单 OutDepot
        /// <summary>
        /// 备件出库单Id
        /// </summary>
        [Label("备件出库单")]
        public static readonly IRefIdProperty OutDepotIdProperty =
            P<PartOutDepotDetail>.RegisterRefId(e => e.OutDepotId, ReferenceType.Parent);

        /// <summary>
        /// 备件出库单Id
        /// </summary>
        public double OutDepotId
        {
            get { return (double)this.GetRefId(OutDepotIdProperty); }
            set { this.SetRefId(OutDepotIdProperty, value); }
        }

        /// <summary>
        /// 备件出库单
        /// </summary>
        public static readonly RefEntityProperty<OutDepot> OutDepotProperty =
            P<PartOutDepotDetail>.RegisterRef(e => e.OutDepot, OutDepotIdProperty);

        /// <summary>
        /// 备件出库单
        /// </summary>
        public OutDepot OutDepot
        {
            get { return this.GetRefEntity(OutDepotProperty); }
            set { this.SetRefEntity(OutDepotProperty, value); }
        }
        #endregion

        #region 行号 LineNo
        /// <summary>
        /// 行号
        /// </summary>
        [Label("行号")]
        public static readonly Property<int> LineNoProperty = P<PartOutDepotDetail>.Register(e => e.LineNo);

        /// <summary>
        /// 行号
        /// </summary>
        public int LineNo
        {
            get { return GetProperty(LineNoProperty); }
            set { SetProperty(LineNoProperty, value); }
        }
        #endregion

        #region 备件编码 SparePart
        /// <summary>
        /// 备件名称Id
        /// </summary>
        [Label("备件编码")]
        public static readonly IRefIdProperty SparePartIdProperty =
            P<PartOutDepotDetail>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

        /// <summary>
        /// 备件名称Id
        /// </summary>
        public double SparePartId
        {
            get { return (double)this.GetRefId(SparePartIdProperty); }
            set { this.SetRefId(SparePartIdProperty, value); }
        }

        /// <summary>
        /// 备件名称
        /// </summary>
        public static readonly RefEntityProperty<SparePart> SparePartProperty =
            P<PartOutDepotDetail>.RegisterRef(e => e.SparePart, SparePartIdProperty);

        /// <summary>
        /// 备件名称
        /// </summary>
        public SparePart SparePart
        {
            get { return this.GetRefEntity(SparePartProperty); }
            set { this.SetRefEntity(SparePartProperty, value); }
        }
        #endregion

        #region 库位 StorageLocation
        /// <summary>
        /// 库位Id
        /// </summary>
        [Label("库位")]
        public static readonly IRefIdProperty StorageLocationIdProperty = P<PartOutDepotDetail>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty = P<PartOutDepotDetail>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return GetRefEntity(StorageLocationProperty); }
            set { SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion

        #region 批次号 BatchNoRef
        /// <summary>
        /// 批次号Id
        /// </summary>
        [Label("批次号")]
        public static readonly IRefIdProperty BatchNoRefIdProperty =
            P<PartOutDepotDetail>.RegisterRefId(e => e.BatchNoRefId, ReferenceType.Normal);

        /// <summary>
        /// 批次号Id
        /// </summary>
        public double? BatchNoRefId
        {
            get { return (double?)this.GetRefNullableId(BatchNoRefIdProperty); }
            set { this.SetRefNullableId(BatchNoRefIdProperty, value); }
        }

        /// <summary>
        /// 批次号
        /// </summary>
        public static readonly RefEntityProperty<StoreSummaryLot> BatchNoRefProperty =
            P<PartOutDepotDetail>.RegisterRef(e => e.BatchNoRef, BatchNoRefIdProperty);

        /// <summary>
        /// 批次号
        /// </summary>
        public StoreSummaryLot BatchNoRef
        {
            get { return this.GetRefEntity(BatchNoRefProperty); }
            set { this.SetRefEntity(BatchNoRefProperty, value); }
        }
        #endregion

        #region 批次号 BatchNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> BatchNoProperty = P<PartOutDepotDetail>.Register(e => e.BatchNo);

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo
        {
            get { return this.GetProperty(BatchNoProperty); }
            set { this.SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 序列号 SeriaNoRef
        /// <summary>
        /// 序列号Id
        /// </summary>
        [Label("序列号")]
        public static readonly IRefIdProperty SeriaNoRefIdProperty =
            P<PartOutDepotDetail>.RegisterRefId(e => e.SeriaNoRefId, ReferenceType.Normal);

        /// <summary>
        /// 序列号Id
        /// </summary>
        public double? SeriaNoRefId
        {
            get { return (double?)this.GetRefNullableId(SeriaNoRefIdProperty); }
            set { this.SetRefNullableId(SeriaNoRefIdProperty, value); }
        }

        /// <summary>
        /// 序列号
        /// </summary>
        public static readonly RefEntityProperty<StoreSummaryDetail> SeriaNoRefProperty =
            P<PartOutDepotDetail>.RegisterRef(e => e.SeriaNoRef, SeriaNoRefIdProperty);

        /// <summary>
        /// 序列号
        /// </summary>
        public StoreSummaryDetail SeriaNoRef
        {
            get { return this.GetRefEntity(SeriaNoRefProperty); }
            set { this.SetRefEntity(SeriaNoRefProperty, value); }
        }
        #endregion

        #region 序列号 SeriaNo
        /// <summary>
        /// 序列号
        /// </summary>
        [Label("序列号")]
        public static readonly Property<string> SeriaNoProperty = P<PartOutDepotDetail>.Register(e => e.SeriaNo);

        /// <summary>
        /// 序列号
        /// </summary>
        public string SeriaNo
        {
            get { return this.GetProperty(SeriaNoProperty); }
            set { this.SetProperty(SeriaNoProperty, value); }
        }
        #endregion

        #region 出库数量 OutDepotCount
        /// <summary>
        /// 出库数量
        /// </summary>
        [Label("出库数量")]
        [Required]
        public static readonly Property<int> OutDepotCountProperty = P<PartOutDepotDetail>.Register(e => e.OutDepotCount);

        /// <summary>
        /// 出库数量
        /// </summary>
        public int OutDepotCount
        {
            get { return this.GetProperty(OutDepotCountProperty); }
            set { this.SetProperty(OutDepotCountProperty, value); }
        }
        #endregion

        #region 出库日期 OutDepotDate
        /// <summary>
        /// 出库日期
        /// </summary>
        [Label("出库日期")]
        public static readonly Property<DateTime?> OutDepotDateProperty = P<PartOutDepotDetail>.Register(e => e.OutDepotDate);

        /// <summary>
        /// 出库日期
        /// </summary>
        public DateTime? OutDepotDate
        {
            get { return this.GetProperty(OutDepotDateProperty); }
            set { this.SetProperty(OutDepotDateProperty, value); }
        }
        #endregion

        #region 状态 OutboundStatus
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<OutboundStatus?> OutboundStatusProperty = P<PartOutDepotDetail>.Register(e => e.OutboundStatus);

        /// <summary>
        /// 状态
        /// </summary>
        public OutboundStatus? OutboundStatus
        {
            get { return GetProperty(OutboundStatusProperty); }
            set { SetProperty(OutboundStatusProperty, value); }
        }
        #endregion

        #region 备件交接明细 OutDepotHandoverDetail
        /// <summary>
        /// 备件交接明细Id
        /// </summary>
        [Label("备件交接明细")]
        public static readonly IRefIdProperty OutDepotHandoverDetailIdProperty =
            P<PartOutDepotDetail>.RegisterRefId(e => e.OutDepotHandoverDetailId, ReferenceType.Normal);

        /// <summary>
        /// 备件交接明细Id
        /// </summary>
        public double? OutDepotHandoverDetailId
        {
            get { return (double?)this.GetRefNullableId(OutDepotHandoverDetailIdProperty); }
            set { this.SetRefNullableId(OutDepotHandoverDetailIdProperty, value); }
        }

        /// <summary>
        /// 备件交接明细
        /// </summary>
        public static readonly RefEntityProperty<OutDepotHandoverDetail> OutDepotHandoverDetailProperty =
            P<PartOutDepotDetail>.RegisterRef(e => e.OutDepotHandoverDetail, OutDepotHandoverDetailIdProperty);

        /// <summary>
        /// 备件交接明细
        /// </summary>
        public OutDepotHandoverDetail OutDepotHandoverDetail
        {
            get { return this.GetRefEntity(OutDepotHandoverDetailProperty); }
            set { this.SetRefEntity(OutDepotHandoverDetailProperty, value); }
        }
        #endregion

        #region 质量状态 QualityStatus
        /// <summary>
        /// 质量状态
        /// </summary>
        [Label("质量状态")]
        public static readonly Property<QualityStatus> QualityStatusProperty = P<PartOutDepotDetail>.Register(e => e.QualityStatus);

        /// <summary>
        /// 质量状态
        /// </summary>
        public QualityStatus QualityStatus
        {
            get { return this.GetProperty(QualityStatusProperty); }
            set { this.SetProperty(QualityStatusProperty, value); }
        }
        #endregion

        #region 使用数量 UseCount
        /// <summary>
        /// 使用数量
        /// </summary>
        [Label("使用数量")]
        public static readonly Property<int> UseCountProperty = P<PartOutDepotDetail>.Register(e => e.UseCount);

        /// <summary>
        /// 使用数量
        /// </summary>
        public int UseCount
        {
            get { return this.GetProperty(UseCountProperty); }
            set { this.SetProperty(UseCountProperty, value); }
        }
        #endregion

        #region 退回数 ReturnQty
        /// <summary>
        /// 退回数
        /// </summary>
        [Label("退回数")]
        public static readonly Property<int> ReturnQtyProperty = P<PartOutDepotDetail>.Register(e => e.ReturnQty);

        /// <summary>
        /// 退回数
        /// </summary>
        public int ReturnQty
        {
            get { return GetProperty(ReturnQtyProperty); }
            set { SetProperty(ReturnQtyProperty, value); }
        }
        #endregion

        #region 旧件退回数 OldReturnQty
        /// <summary>
        /// 旧件退回数
        /// </summary>
        [Label("旧件退回数")]
        public static readonly Property<int> OldReturnQtyProperty = P<PartOutDepotDetail>.Register(e => e.OldReturnQty);

        /// <summary>
        /// 旧件退回数
        /// </summary>
        public int OldReturnQty
        {
            get { return GetProperty(OldReturnQtyProperty); }
            set { SetProperty(OldReturnQtyProperty, value); }
        }
        #endregion

        #region 平均成本 UnitPrice
        /// <summary>
        /// 平均成本
        /// </summary>
        [Label("平均成本")]
        public static readonly Property<double> UnitPriceProperty = P<PartOutDepotDetail>.Register(e => e.UnitPrice);

        /// <summary>
        /// 平均成本
        /// </summary>
        public double UnitPrice
        {
            get { return this.GetProperty(UnitPriceProperty); }
            set { this.SetProperty(UnitPriceProperty, value); }
        }
        #endregion

        #region 非映射字段

        #region 剩余数量 RemainingQty
        /// <summary>
        /// 剩余数量
        /// </summary>
        [Label("剩余数量")]
        public static readonly Property<int> RemainingQtyProperty = P<PartOutDepotDetail>.RegisterReadOnly(
            e => e.RemainingQty, e => e.GetRemainingQty(), PartOutDepotDetail.OutDepotCountProperty, PartOutDepotDetail.UseCountProperty);
        /// <summary>
        /// 剩余数量
        /// </summary>

        public int RemainingQty
        {
            get { return this.GetProperty(RemainingQtyProperty); }
        }
        private int GetRemainingQty()
        {
            return OutDepotCount - UseCount;
        }
        #endregion


        #endregion

        #region 视图属性

        #region 库位 SiteCode
        /// <summary>
        /// 库位
        /// </summary>
        [Label("库位")]
        public static readonly Property<string> SiteCodeProperty = P<PartOutDepotDetail>.RegisterView(e => e.SiteCode, p => p.BatchNoRef.StorageLocation.Code);

        /// <summary>
        /// 库位
        /// </summary>
        public string SiteCode
        {
            get { return this.GetProperty(SiteCodeProperty); }
            set { this.SetProperty(SiteCodeProperty, value); }
        }
        #endregion

        #region 库位数量 GoodNumber
        /// <summary>
        /// 库位数量
        /// </summary>
        [Label("库位数量")]
        public static readonly Property<int?> GoodNumberProperty = P<PartOutDepotDetail>.RegisterView(e => e.GoodNumber, p => p.BatchNoRef.GoodNumber);

        /// <summary>
        /// 库位数量
        /// </summary>
        public int? GoodNumber
        {
            get { return this.GetProperty(GoodNumberProperty); }
            set { this.SetProperty(GoodNumberProperty, value); }
        }
        #endregion

        #region 出库状态 OutDepotState
        /// <summary>
        /// 出库状态
        /// </summary>
        [Label("出库状态")]
        public static readonly Property<OutDepotState> OutDepotStateProperty = P<PartOutDepotDetail>.RegisterView(e => e.OutDepotState, p => p.OutDepot.OutDepotState);

        /// <summary>
        /// 出库状态
        /// </summary>
        public OutDepotState OutDepotState
        {
            get { return this.GetProperty(OutDepotStateProperty); }
        }
        #endregion

        #region 备件编码 SparePartCode
        /// <summary>
        /// 备件编码
        /// </summary>
        [Label("备件编码")]
        public static readonly Property<string> SparePartCodeProperty = P<PartOutDepotDetail>.RegisterView(e => e.SparePartCode, p => p.SparePart.SparePartCode);

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparePartCode
        {
            get { return this.GetProperty(SparePartCodeProperty); }
        }
        #endregion

        #region 备件名称 SparePartName
        /// <summary>
        /// 备件名称
        /// </summary>
        [Label("备件名称")]
        public static readonly Property<string> SparePartNameProperty = P<PartOutDepotDetail>.RegisterView(e => e.SparePartName, p => p.SparePart.SparePartName);

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName
        {
            get { return this.GetProperty(SparePartNameProperty); }
        }
        #endregion

        #region 备件单位id SparePartUnitId
        /// <summary>
        /// 备件单位id
        /// </summary>
        [Label("备件单位id")]
        public static readonly Property<double> SparePartUnitIdProperty = P<PartOutDepotDetail>.RegisterView(e => e.SparePartUnitId, p => p.SparePart.UnitId);

        /// <summary>
        /// 备件单位id
        /// </summary>
        public double SparePartUnitId
        {
            get { return this.GetProperty(SparePartUnitIdProperty); }
        }
        #endregion

        #region 备件单位名称 SparePartUnitName
        /// <summary>
        /// 备件单位名称
        /// </summary>
        [Label("备件单位名称")]
        public static readonly Property<string> SparePartUnitNameProperty = P<PartOutDepotDetail>.RegisterView(e => e.SparePartUnitName, p => p.SparePart.Unit.Name);

        /// <summary>
        /// 备件单位名称
        /// </summary>
        public string SparePartUnitName
        {
            get { return this.GetProperty(SparePartUnitNameProperty); }
        }
        #endregion

        #region 规格型号 SpecificationView
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> SpecificationViewProperty = P<PartOutDepotDetail>.RegisterView(e => e.SpecificationView, p => p.SparePart.Specification);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string SpecificationView
        {
            get { return this.GetProperty(SpecificationViewProperty); }
        }
        #endregion

        #region 类型 SpartType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<SparePartType> SpartTypeProperty = P<PartOutDepotDetail>.RegisterView(e => e.SpartType, p => p.SparePart.SpartType);

        /// <summary>
        /// 类型
        /// </summary>
        public SparePartType SpartType
        {
            get { return this.GetProperty(SpartTypeProperty); }
        }
        #endregion

        #region 管控方式 ControlMethod
        /// <summary>
        /// 管控方式
        /// </summary>
        [Label("管控方式")]
        public static readonly Property<ControlMethod> ControlMethodProperty = P<PartOutDepotDetail>.RegisterView(e => e.ControlMethod, p => p.SparePart.ControlMethod);

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
        public static readonly Property<bool> IsReplacementProperty = P<PartOutDepotDetail>.RegisterView(e => e.IsReplacement, p => p.SparePart.IsReplacement);

        /// <summary>
        /// 以旧换新
        /// </summary>
        public bool IsReplacement
        {
            get { return this.GetProperty(IsReplacementProperty); }
        }
        #endregion

        #region 出库仓库 WarehouseName
        /// <summary>
        /// 出库仓库
        /// </summary>
        [Label("出库仓库")]
        public static readonly Property<string> WarehouseNameProperty = P<PartOutDepotDetail>.RegisterView(e => e.WarehouseName, p => p.StorageLocation.Warehouse.Name);

        /// <summary>
        /// 出库仓库
        /// </summary>
        public string WarehouseName
        {
            get { return this.GetProperty(WarehouseNameProperty); }
        }
        #endregion

        #region 出库仓库Id WarehouseId
        /// <summary>
        /// 出库仓库Id
        /// </summary>
        [Label("出库仓库Id")]
        public static readonly Property<double> WarehouseIdProperty = P<PartOutDepotDetail>.RegisterView(e => e.WarehouseId, p => p.StorageLocation.Warehouse.Id);

        /// <summary>
        /// 出库仓库
        /// </summary>
        public double WarehouseId
        {
            get { return this.GetProperty(WarehouseIdProperty); }
        }
        #endregion

        #region 库位 StorageLocationName
        /// <summary>
        /// 库位
        /// </summary>
        [Label("库位")]
        public static readonly Property<string> StorageLocationNameProperty = P<PartOutDepotDetail>.RegisterView(e => e.StorageLocationName, p => p.StorageLocation.Name);

        /// <summary>
        /// 库位
        /// </summary>
        public string StorageLocationName
        {
            get { return this.GetProperty(StorageLocationNameProperty); }
        }
        #endregion

        #region 出库单号 OutDepotNoView
        /// <summary>
        /// 出库单号
        /// </summary>
        [Label("出库单号")]
        public static readonly Property<string> OutDepotNoViewProperty = P<PartOutDepotDetail>.RegisterView(e => e.OutDepotNoView, p => p.OutDepot.No);

        /// <summary>
        /// 出库单号
        /// </summary>
        public string OutDepotNoView
        {
            get { return this.GetProperty(OutDepotNoViewProperty); }
        }
        #endregion

        #region 来源单号 SourceNoView
        /// <summary>
        /// 来源单号
        /// </summary>
        [Label("来源单号")]
        public static readonly Property<string> SourceNoViewProperty = P<PartOutDepotDetail>.RegisterView(e => e.SourceNoView, p => p.OutDepot.SourceNo);

        /// <summary>
        /// 来源单号
        /// </summary>
        public string SourceNoView
        {
            get { return this.GetProperty(SourceNoViewProperty); }
        }
        #endregion

        #region 申请单号 ReleDocView
        /// <summary>
        /// 申请单号
        /// </summary>
        [Label("申请单号")]
        public static readonly Property<string> ReleDocViewProperty = P<PartOutDepotDetail>.RegisterView(e => e.ReleDocView, p => p.OutDepot.ReleDoc);

        /// <summary>
        /// 申请单号
        /// </summary>
        public string ReleDocView
        {
            get { return this.GetProperty(ReleDocViewProperty); }
        }
        #endregion

        #region 批次号 BatchNoView
        /// <summary>
        /// 批次号View
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> BatchNoViewProperty = P<PartOutDepotDetail>.RegisterView(e => e.BatchNoView, p => p.BatchNoRef.BatchNumber);

        /// <summary>
        /// 批次号View
        /// </summary>
        public string BatchNoView
        {
            get { return this.GetProperty(BatchNoViewProperty); }
        }
        #endregion

        #region 序列号 SeriaNoView
        /// <summary>
        /// 序列号
        /// </summary>
        [Label("序列号")]
        public static readonly Property<string> SeriaNoViewProperty = P<PartOutDepotDetail>.RegisterView(e => e.SeriaNoView, p => p.SeriaNoRef.OrderNumberCode);

        /// <summary>
        /// 序列号
        /// </summary>
        public string SeriaNoView
        {
            get { return this.GetProperty(SeriaNoViewProperty); }
        }
        #endregion

        #region 备件类型 SparePartTypeCode
        /// <summary>
        /// 备件类型
        /// </summary>
        [Label("备件类型")]
        public static readonly Property<string> SparePartTypeCodeProperty = P<PartOutDepotDetail>.RegisterView(e => e.SparePartTypeCode, p => p.SparePart.ItemCategory.Code);

        /// <summary>
        /// 备件类型
        /// </summary>
        public string SparePartTypeCode
        {
            get { return this.GetProperty(SparePartTypeCodeProperty); }
            set { this.SetProperty(SparePartTypeCodeProperty, value); }
        }
        #endregion

        #region 备件类型名称 SparePartTypeName
        /// <summary>
        /// 备件类型名称
        /// </summary>
        [Label("备件类型名称")]
        public static readonly Property<string> SparePartTypeNameProperty = P<PartOutDepotDetail>.RegisterView(e => e.SparePartTypeName, p => p.SparePart.ItemCategory.Name);

        /// <summary>
        /// 备件类型名称
        /// </summary>
        public string SparePartTypeName
        {
            get { return this.GetProperty(SparePartTypeNameProperty); }
        }
        #endregion

        #region 交接单号 HandoverNo
        /// <summary>
        /// 交接单号
        /// </summary>
        [Label("交接单号")]
        public static readonly Property<string> HandoverNoProperty = P<PartOutDepotDetail>.RegisterView(e => e.HandoverNo, p => p.OutDepotHandoverDetail.OutDepotHandover.HandoverNo);

        /// <summary>
        /// 交接单号
        /// </summary>
        public string HandoverNo
        {
            get { return this.GetProperty(HandoverNoProperty); }
        }
        #endregion

        #region 设备台账Id EquipAccountId
        /// <summary>
        /// 设备台账Id
        /// </summary>
        [Label("设备台账Id")]
        public static readonly Property<double?> EquipAccountIdProperty = P<PartOutDepotDetail>.RegisterView(e => e.EquipAccountId, p => p.OutDepot.EquipAccountId);

        /// <summary>
        /// 设备台账Id
        /// </summary>
        public double? EquipAccountId
        {
            get { return this.GetProperty(EquipAccountIdProperty); }
        }
        #endregion

        #region 设备编码 EquipAccountCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipAccountCodeProperty = P<PartOutDepotDetail>.RegisterView(e => e.EquipAccountCode, p => p.OutDepot.EquipAccount.Code);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipAccountCode
        {
            get { return this.GetProperty(EquipAccountCodeProperty); }
        }
        #endregion

        #region 设备名称 EquipAccountName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipAccountNameProperty = P<PartOutDepotDetail>.RegisterView(e => e.EquipAccountName, p => p.OutDepot.EquipAccount.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipAccountName
        {
            get { return this.GetProperty(EquipAccountNameProperty); }
        }
        #endregion

        #region 设备型号编码 EquipModelCode
        /// <summary>
        /// 设备型号编码
        /// </summary>
        [Label("设备型号编码")]
        public static readonly Property<string> EquipModelCodeProperty = P<PartOutDepotDetail>.RegisterView(e => e.EquipModelCode, p => p.SparePart.SpartEquipModel.Code);

        /// <summary>
        /// 设备型号编码
        /// </summary>
        public string EquipModelCode
        {
            get { return this.GetProperty(EquipModelCodeProperty); }
        }
        #endregion

        #region 设备型号名称 EquipModelName
        /// <summary>
        /// 设备型号名称
        /// </summary>
        [Label("设备型号名称")]
        public static readonly Property<string> EquipModelNameProperty = P<PartOutDepotDetail>.RegisterView(e => e.EquipModelName, p => p.SparePart.SpartEquipModel.Name);

        /// <summary>
        /// 设备型号名称
        /// </summary>
        public string EquipModelName
        {
            get { return this.GetProperty(EquipModelNameProperty); }
        }
        #endregion

        #region 制造商 Manufacturer
        /// <summary>
        /// 制造商
        /// </summary>
        [Label("制造商")]
        public static readonly Property<string> ManufacturerProperty = P<PartOutDepotDetail>.RegisterView(e => e.Manufacturer, p => p.SparePart.Manufacturer);

        /// <summary>
        /// 制造商
        /// </summary>
        public string Manufacturer
        {
            get { return this.GetProperty(ManufacturerProperty); }
        }
        #endregion

        #endregion

        #region 不映射数据库的属性

        #region 备件编码 SparePartCodeView
        /// <summary>
        /// 备件编码
        /// </summary>
        [Label("备件编码")]
        public static readonly Property<string> SparePartCodeViewProperty = P<PartOutDepotDetail>.Register(e => e.SparePartCodeView);

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparePartCodeView
        {
            get { return this.GetProperty(SparePartCodeViewProperty); }
            set { this.SetProperty(SparePartCodeViewProperty, value); }
        }
        #endregion

        #region 备件名称 SparePartNameView
        /// <summary>
        /// 备件名称
        /// </summary>
        [Label("备件名称")]
        public static readonly Property<string> SparePartNameViewProperty = P<PartOutDepotDetail>.Register(e => e.SparePartNameView);

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartNameView
        {
            get { return this.GetProperty(SparePartNameViewProperty); }
            set { this.SetProperty(SparePartNameViewProperty, value); }
        }
        #endregion

        #region 管控方式 ControlMethodView
        /// <summary>
        /// 管控方式
        /// </summary>
        [Label("管控方式")]
        public static readonly Property<ControlMethod> ControlMethodViewProperty = P<PartOutDepotDetail>.Register(e => e.ControlMethodView);

        /// <summary>
        /// 管控方式
        /// </summary>
        public ControlMethod ControlMethodView
        {
            get { return this.GetProperty(ControlMethodViewProperty); }
            set { this.SetProperty(ControlMethodViewProperty, value); }
        }
        #endregion

        #region 出库单号-行号 OutDepotLineNo
        /// <summary>
        /// 出库单号-行号
        /// </summary>
        [Label("出库单号-行号")]
        public static readonly Property<string> OutDepotLineNoProperty = P<PartOutDepotDetail>.RegisterReadOnly(
            e => e.OutDepotLineNo, e => e.GetOutDepotLineNo(), OutDepotProperty, LineNoProperty);

        /// <summary>
        /// 出库单号-行号
        /// </summary>
        public string OutDepotLineNo
        {
            get { return this.GetProperty(OutDepotLineNoProperty); }
        }
        private string GetOutDepotLineNo()
        {
            return string.Format("{0}-{1}", OutDepot?.No, LineNo);
        }
        #endregion

        #region 出库可退数量 CanReturnQty
        /// <summary>
        /// 出库可退数量
        /// </summary>
        [Label("出库单可退数量")]
        public static readonly Property<int> CanReturnQtyProperty = P<PartOutDepotDetail>.Register(e => e.CanReturnQty);
        /// <summary>
        /// 出库单可退数量
        /// </summary>
        public int CanReturnQty
        {
            get { return GetProperty(CanReturnQtyProperty); }
            set { SetProperty(CanReturnQtyProperty, value); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 出库单明细实体配置
    /// </summary>
    internal class PartOutDepotDetailConfig : EntityConfig<PartOutDepotDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_PART_OUT_DEPOT_DETAIL").MapAllProperties();            
            Meta.Property(PartOutDepotDetail.QualityStatusProperty).DontMapColumn();
            Meta.Property(PartOutDepotDetail.SparePartCodeViewProperty).DontMapColumn();
            Meta.Property(PartOutDepotDetail.SparePartNameViewProperty).DontMapColumn();
            Meta.Property(PartOutDepotDetail.ControlMethodViewProperty).DontMapColumn();
            Meta.Property(PartOutDepotDetail.CanReturnQtyProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}
