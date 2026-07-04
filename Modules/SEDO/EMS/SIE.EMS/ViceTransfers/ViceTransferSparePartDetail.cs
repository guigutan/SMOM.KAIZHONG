using SIE.Domain;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Enums;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.EMS.ViceTransfers
{
    /// <summary>
    /// 备件调拨明细
    /// </summary>
    [ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("备件调拨明细")]
    public partial class ViceTransferSparePartDetail : DataEntity
    {
        #region 调拨数量 TransferQty
        /// <summary>
        /// 调拨数量
        /// </summary>
        [Label("调拨数量")]
        public static readonly Property<decimal> TransferQtyProperty = P<ViceTransferSparePartDetail>.Register(e => e.TransferQty);

        /// <summary>
        /// 调拨数量
        /// </summary>
        public decimal TransferQty
        {
            get { return GetProperty(TransferQtyProperty); }
            set { SetProperty(TransferQtyProperty, value); }
        }
        #endregion

        #region 需求行 ViceTransferSparePart
        /// <summary>
        /// 需求行Id
        /// </summary>
        [Label("需求行号")]
        public static readonly IRefIdProperty ViceTransferSparePartIdProperty = P<ViceTransferSparePartDetail>.RegisterRefId(e => e.ViceTransferSparePartId, ReferenceType.Normal);

        /// <summary>
        /// 需求行Id
        /// </summary>
        public double ViceTransferSparePartId
        {
            get { return (double)GetRefId(ViceTransferSparePartIdProperty); }
            set { SetRefId(ViceTransferSparePartIdProperty, value); }
        }

        /// <summary>
        /// 需求行
        /// </summary>
        public static readonly RefEntityProperty<ViceTransferSparePart> ViceTransferSparePartProperty = P<ViceTransferSparePartDetail>.RegisterRef(e => e.ViceTransferSparePart, ViceTransferSparePartIdProperty);

        /// <summary>
        /// 需求行
        /// </summary>
        public ViceTransferSparePart ViceTransferSparePart
        {
            get { return GetRefEntity(ViceTransferSparePartProperty); }
            set { SetRefEntity(ViceTransferSparePartProperty, value); }
        }
        #endregion

        #region 批次号 StoreSummaryLot
        /// <summary>
        /// 批次号Id
        /// </summary>
        [Label("批次号")]
        public static readonly IRefIdProperty StoreSummaryLotIdProperty = P<ViceTransferSparePartDetail>.RegisterRefId(e => e.StoreSummaryLotId, ReferenceType.Normal);

        /// <summary>
        /// 批次号Id
        /// </summary>
        public double? StoreSummaryLotId
        {
            get { return (double?)GetRefNullableId(StoreSummaryLotIdProperty); }
            set { SetRefNullableId(StoreSummaryLotIdProperty, value); }
        }

        /// <summary>
        /// 批次号
        /// </summary>
        public static readonly RefEntityProperty<StoreSummaryLot> StoreSummaryLotProperty = P<ViceTransferSparePartDetail>.RegisterRef(e => e.StoreSummaryLot, StoreSummaryLotIdProperty);

        /// <summary>
        /// 批次号
        /// </summary>
        public StoreSummaryLot StoreSummaryLot
        {
            get { return GetRefEntity(StoreSummaryLotProperty); }
            set { SetRefEntity(StoreSummaryLotProperty, value); }
        }
        #endregion

        #region 序列号 StoreSummaryDetail
        /// <summary>
        /// 序列号Id
        /// </summary>
        [Label("序列号")]
        public static readonly IRefIdProperty StoreSummaryDetailIdProperty = P<ViceTransferSparePartDetail>.RegisterRefId(e => e.StoreSummaryDetailId, ReferenceType.Normal);

        /// <summary>
        /// 序列号Id
        /// </summary>
        public double? StoreSummaryDetailId
        {
            get { return (double?)GetRefNullableId(StoreSummaryDetailIdProperty); }
            set { SetRefNullableId(StoreSummaryDetailIdProperty, value); }
        }

        /// <summary>
        /// 序列号
        /// </summary>
        public static readonly RefEntityProperty<StoreSummaryDetail> StoreSummaryDetailProperty = P<ViceTransferSparePartDetail>.RegisterRef(e => e.StoreSummaryDetail, StoreSummaryDetailIdProperty);

        /// <summary>
        /// 序列号
        /// </summary>
        public StoreSummaryDetail StoreSummaryDetail
        {
            get { return GetRefEntity(StoreSummaryDetailProperty); }
            set { SetRefEntity(StoreSummaryDetailProperty, value); }
        }
        #endregion

        #region 质量状态 QualityStatus
        /// <summary>
        /// 质量状态
        /// </summary>
        [Label("质量状态")]
        public static readonly Property<QualityStatus> QualityStatusProperty = P<ViceTransferSparePartDetail>.Register(e => e.QualityStatus);

        /// <summary>
        /// 质量状态
        /// </summary>
        public QualityStatus QualityStatus
        {
            get { return GetProperty(QualityStatusProperty); }
            set { SetProperty(QualityStatusProperty, value); }
        }
        #endregion

        #region 来源库位 StorageLocation
        /// <summary>
        /// 来源库位Id
        /// </summary>
        [Label("来源库位")]
        public static readonly IRefIdProperty StorageLocationIdProperty = P<ViceTransferSparePartDetail>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

        /// <summary>
        /// 来源库位Id
        /// </summary>
        public double StorageLocationId
        {
            get { return (double)GetRefId(StorageLocationIdProperty); }
            set { SetRefId(StorageLocationIdProperty, value); }
        }

        /// <summary>
        /// 来源库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty = P<ViceTransferSparePartDetail>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 来源库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return GetRefEntity(StorageLocationProperty); }
            set { SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion

        #region 目标库位 Target
        /// <summary>
        /// 目标库位Id
        /// </summary>
        [Label("目标库位")]
        public static readonly IRefIdProperty TargetIdProperty = P<ViceTransferSparePartDetail>.RegisterRefId(e => e.TargetId, ReferenceType.Normal);

        /// <summary>
        /// 目标库位Id
        /// </summary>
        public double TargetId
        {
            get { return (double)GetRefId(TargetIdProperty); }
            set { SetRefId(TargetIdProperty, value); }
        }

        /// <summary>
        /// 目标库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> TargetProperty = P<ViceTransferSparePartDetail>.RegisterRef(e => e.Target, TargetIdProperty);

        /// <summary>
        /// 目标库位
        /// </summary>
        public StorageLocation Target
        {
            get { return GetRefEntity(TargetProperty); }
            set { SetRefEntity(TargetProperty, value); }
        }
        #endregion

        #region 备件调拨明细 ViceTransfer
        /// <summary>
        /// 备件调拨明细Id
        /// </summary>
        [Label("备件调拨明细")]
        public static readonly IRefIdProperty ViceTransferIdProperty = P<ViceTransferSparePartDetail>.RegisterRefId(e => e.ViceTransferId, ReferenceType.Parent);

        /// <summary>
        /// 备件调拨明细Id
        /// </summary>
        public double ViceTransferId
        {
            get { return (double)GetRefId(ViceTransferIdProperty); }
            set { SetRefId(ViceTransferIdProperty, value); }
        }

        /// <summary>
        /// 备件调拨明细
        /// </summary>
        public static readonly RefEntityProperty<ViceTransfer> ViceTransferProperty = P<ViceTransferSparePartDetail>.RegisterRef(e => e.ViceTransfer, ViceTransferIdProperty);

        /// <summary>
        /// 备件调拨明细
        /// </summary>
        public ViceTransfer ViceTransfer
        {
            get { return GetRefEntity(ViceTransferProperty); }
            set { SetRefEntity(ViceTransferProperty, value); }
        }
        #endregion


        #region 来源库位库存数 SourceInventoryQty
        /// <summary>
        /// 来源库位库存数
        /// </summary>
        [Label("来源库位库存数")]
        public static readonly Property<int> SourceInventoryQtyProperty = P<ViceTransferSparePartDetail>.Register(e => e.SourceInventoryQty);

        /// <summary>
        /// 来源库位库存数
        /// </summary>
        public int SourceInventoryQty
        {
            get { return this.GetProperty(SourceInventoryQtyProperty); }
            set { this.SetProperty(SourceInventoryQtyProperty, value); }
        }
        #endregion


        #region 视图属性
        #region 需求行号 ViceTransferSparePartLineNo
        /// <summary>
        /// 需求行号
        /// </summary>
        [Label("需求行号")]
        public static readonly Property<string> ViceTransferSparePartLineNoProperty = P<ViceTransferSparePartDetail>.RegisterView(e => e.LineNo, p => p.ViceTransferSparePart.LineNo);

        /// <summary>
        /// 需求行号
        /// </summary>
        public string LineNo
        {
            get { return this.GetProperty(ViceTransferSparePartLineNoProperty); }
            set { SetProperty(ViceTransferSparePartLineNoProperty, value); }
        }
        #endregion

        #region 备件编码 SparePartCode
        /// <summary>
        /// 备件编码
        /// </summary>
        [Label("备件编码")]
        public static readonly Property<string> SparePartCodeProperty = P<ViceTransferSparePartDetail>.RegisterView(e => e.SparePartCode, p => p.ViceTransferSparePart.SparePart.SparePartCode);

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
        public static readonly Property<string> SparePartNameProperty = P<ViceTransferSparePartDetail>.RegisterView(e => e.SparePartName, p => p.ViceTransferSparePart.SparePart.SparePartName);

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparePartName
        {
            get { return this.GetProperty(SparePartNameProperty); }
            set { SetProperty(SparePartNameProperty, value); }
        }
        #endregion

        #region 规格型号 Specification
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> SpecificationProperty = P<ViceTransferSparePartDetail>.RegisterView(e => e.Specification, p => p.ViceTransferSparePart.SparePart.Specification);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string Specification
        {
            get { return this.GetProperty(SpecificationProperty); }
            set { SetProperty(SpecificationProperty, value); }
        }
        #endregion

        #region 类型 SparePartType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<SparePartType> SparePartTypeProperty = P<ViceTransferSparePartDetail>.RegisterView(e => e.SparePartType, p => p.ViceTransferSparePart.SparePart.SpartType);

        /// <summary>
        /// 类型
        /// </summary>
        public SparePartType SparePartType
        {
            get { return this.GetProperty(SparePartTypeProperty); }
            set { SetProperty(SparePartTypeProperty, value); }
        }
        #endregion

        #region 管控方式 ControlMethod
        /// <summary>
        /// 管控方式
        /// </summary>
        [Label("管控方式")]
        public static readonly Property<ControlMethod> ControlMethodProperty = P<ViceTransferSparePartDetail>.RegisterView(e => e.ControlMethod, p => p.ViceTransferSparePart.SparePart.ControlMethod);

        /// <summary>
        /// 管控方式
        /// </summary>
        public ControlMethod ControlMethod
        {
            get { return this.GetProperty(ControlMethodProperty); }
            set { SetProperty(ControlMethodProperty, value); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<ViceTransferSparePartDetail>.RegisterView(e => e.UnitName, p => p.ViceTransferSparePart.SparePart.Unit.Name);

        /// <summary>
        /// 管控方式
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
            set { SetProperty(UnitNameProperty, value); }
        }
        #endregion

        #region 剩余需求数量 RemainingDemandQty
        /// <summary>
        /// 剩余需求数量
        /// </summary>
        [Label("剩余需求数量")]
        public static readonly Property<decimal> RemainingDemandQtyProperty = P<ViceTransferSparePartDetail>.RegisterReadOnly(
            e => e.RemainingDemandQty, e => e.GetRemainingDemandQty(), ViceTransferSparePartProperty);
        /// <summary>
        /// 剩余需求数量
        /// </summary>

        public decimal RemainingDemandQty
        {
            get { return this.GetProperty(RemainingDemandQtyProperty); }
        }
        private decimal GetRemainingDemandQty()
        {
            return ViceTransferSparePart.Qty - ViceTransferSparePart.TransferQty;
        }
        #endregion


        #region 来源仓库Id WarehouseId
        /// <summary>
        /// 来源仓库Id
        /// </summary>
        [Label("来源仓库Id")]
        public static readonly Property<double> WarehouseIdProperty = P<ViceTransferSparePartDetail>.Register(e => e.WarehouseId);

        /// <summary>
        /// 来源仓库Id
        /// </summary>
        public double WarehouseId
        {
            get { return this.GetProperty(WarehouseIdProperty); }
            set { this.SetProperty(WarehouseIdProperty, value); }
        }
        #endregion


        #region 目标仓库Id TargetWarehouseId
        /// <summary>
        /// 目标仓库Id
        /// </summary>
        [Label("目标仓库Id")]
        public static readonly Property<double> TargetWarehouseIdProperty = P<ViceTransferSparePartDetail>.Register(e => e.TargetWarehouseId);

        /// <summary>
        /// 来源仓库Id
        /// </summary>
        public double TargetWarehouseId
        {
            get { return this.GetProperty(TargetWarehouseIdProperty); }
            set { this.SetProperty(TargetWarehouseIdProperty, value); }
        }
        #endregion


        #endregion
    }

    /// <summary>
    /// 备件调拨明细 实体配置
    /// </summary>
    internal class ViceTransferSparePartDetailConfig : EntityConfig<ViceTransferSparePartDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_VICE_TRAN_SP_DTL").MapAllPropertiesExcept(ViceTransferSparePartDetail.WarehouseIdProperty,
                ViceTransferSparePartDetail.TargetWarehouseIdProperty,
                ViceTransferSparePartDetail.SourceInventoryQtyProperty);
            Meta.EnablePhantoms();
        }
    }
}