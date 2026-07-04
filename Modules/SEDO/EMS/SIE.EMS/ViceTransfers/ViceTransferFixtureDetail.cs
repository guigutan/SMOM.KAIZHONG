using SIE.Domain;
using SIE.EMS.Enums;
using SIE.Fixtures;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.EMS.ViceTransfers
{
    /// <summary>
    /// 工治具调拨明细
    /// </summary>
    [ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("工治具调拨明细")]
    public partial class ViceTransferFixtureDetail : DataEntity
    {
        #region 调拨数量 TransferQty
        /// <summary>
        /// 调拨数量
        /// </summary>
        [Label("调拨数量")]
        public static readonly Property<int> TransferQtyProperty = P<ViceTransferFixtureDetail>.Register(e => e.TransferQty);

        /// <summary>
        /// 调拨数量
        /// </summary>
        public int TransferQty
        {
            get { return GetProperty(TransferQtyProperty); }
            set { SetProperty(TransferQtyProperty, value); }
        }
        #endregion

        #region 需求行 ViceTransferFixture
        /// <summary>
        /// 需求行号Id
        /// </summary>
        [Label("需求行号")]
        public static readonly IRefIdProperty ViceTransferFixtureIdProperty = P<ViceTransferFixtureDetail>.RegisterRefId(e => e.ViceTransferFixtureId, ReferenceType.Normal);

        /// <summary>
        /// 需求行号Id
        /// </summary>
        public double ViceTransferFixtureId
        {
            get { return (double)GetRefId(ViceTransferFixtureIdProperty); }
            set { SetRefId(ViceTransferFixtureIdProperty, value); }
        }

        /// <summary>
        /// 需求行
        /// </summary>
        public static readonly RefEntityProperty<ViceTransferFixture> ViceTransferFixtureProperty = P<ViceTransferFixtureDetail>.RegisterRef(e => e.ViceTransferFixture, ViceTransferFixtureIdProperty);

        /// <summary>
        /// 需求行号
        /// </summary>
        public ViceTransferFixture ViceTransferFixture
        {
            get { return GetRefEntity(ViceTransferFixtureProperty); }
            set { SetRefEntity(ViceTransferFixtureProperty, value); }
        }
        #endregion

        #region 序列号 FixtureIDAccount
        /// <summary>
        /// 序列号Id
        /// </summary>
        [Label("序列号")]
        public static readonly IRefIdProperty FixtureIDAccountIdProperty = P<ViceTransferFixtureDetail>.RegisterRefId(e => e.FixtureIDAccountId, ReferenceType.Normal);

        /// <summary>
        /// 序列号Id
        /// </summary>
        public double? FixtureIDAccountId
        {
            get { return (double?)GetRefNullableId(FixtureIDAccountIdProperty); }
            set { SetRefNullableId(FixtureIDAccountIdProperty, value); }
        }

        /// <summary>
        /// 序列号
        /// </summary>
        public static readonly RefEntityProperty<FixtureIDAccount> FixtureIDAccountProperty = P<ViceTransferFixtureDetail>.RegisterRef(e => e.FixtureIDAccount, FixtureIDAccountIdProperty);

        /// <summary>
        /// 序列号
        /// </summary>
        public FixtureIDAccount FixtureIDAccount
        {
            get { return GetRefEntity(FixtureIDAccountProperty); }
            set { SetRefEntity(FixtureIDAccountProperty, value); }
        }
        #endregion

        #region 目标库位 Target
        /// <summary>
        /// 目标库位Id
        /// </summary>
        [Label("目标库位")]
        public static readonly IRefIdProperty TargetIdProperty = P<ViceTransferFixtureDetail>.RegisterRefId(e => e.TargetId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<StorageLocation> TargetProperty = P<ViceTransferFixtureDetail>.RegisterRef(e => e.Target, TargetIdProperty);

        /// <summary>
        /// 目标库位
        /// </summary>
        public StorageLocation Target
        {
            get { return GetRefEntity(TargetProperty); }
            set { SetRefEntity(TargetProperty, value); }
        }
        #endregion

        #region 来源库位 StorageLocation
        /// <summary>
        /// 来源库位Id
        /// </summary>
        [Label("来源库位")]
        public static readonly IRefIdProperty StorageLocationIdProperty = P<ViceTransferFixtureDetail>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

        /// <summary>
        /// 来源库位Id
        /// </summary>
        public double? StorageLocationId
        {
            get { return (double?)GetRefNullableId(StorageLocationIdProperty); }
            set { SetRefNullableId(StorageLocationIdProperty, value); }
        }

        /// <summary>
        /// 来源库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty = P<ViceTransferFixtureDetail>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 来源库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return GetRefEntity(StorageLocationProperty); }
            set { SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion

        #region 工治具调拨明细 ViceTransfer
        /// <summary>
        /// 工治具调拨明细Id
        /// </summary>
        [Label("工治具调拨明细")]
        public static readonly IRefIdProperty ViceTransferIdProperty = P<ViceTransferFixtureDetail>.RegisterRefId(e => e.ViceTransferId, ReferenceType.Parent);

        /// <summary>
        /// 工治具调拨明细Id
        /// </summary>
        public double ViceTransferId
        {
            get { return (double)GetRefId(ViceTransferIdProperty); }
            set { SetRefId(ViceTransferIdProperty, value); }
        }

        /// <summary>
        /// 工治具调拨明细
        /// </summary>
        public static readonly RefEntityProperty<ViceTransfer> ViceTransferProperty = P<ViceTransferFixtureDetail>.RegisterRef(e => e.ViceTransfer, ViceTransferIdProperty);

        /// <summary>
        /// 工治具调拨明细
        /// </summary>
        public ViceTransfer ViceTransfer
        {
            get { return GetRefEntity(ViceTransferProperty); }
            set { SetRefEntity(ViceTransferProperty, value); }
        }
        #endregion


        #region 工治具编码 FixtureEncodeCode
        /// <summary>
        /// 工治具编码
        /// </summary>
        [Label("工治具编码")]
        public static readonly Property<string> FixtureEncodeCodeProperty = P<ViceTransferFixtureDetail>.RegisterView(e => e.FixtureEncodeCode, p => p.ViceTransferFixture.FixtureEncode.Code);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public string FixtureEncodeCode
        {
            get { return this.GetProperty(FixtureEncodeCodeProperty); }
            set { this.SetProperty(FixtureEncodeCodeProperty, value); }
        }
        #endregion


        #region 型号编码 ModelCode
        /// <summary>
        /// 型号编码
        /// </summary>
        [Label("型号编码")]
        public static readonly Property<string> ModelCodeProperty = P<ViceTransferFixtureDetail>.RegisterView(e => e.ModelCode, p => p.ViceTransferFixture.FixtureEncode.FixtureModel.Code);

        /// <summary>
        /// 型号编码
        /// </summary>
        public string ModelCode
        {
            get { return this.GetProperty(ModelCodeProperty); }
            set { this.SetProperty(ModelCodeProperty, value); }
        }
        #endregion


        #region 型号名称 ModelName
        /// <summary>
        /// 型号名称
        /// </summary>
        [Label("型号名称")]
        public static readonly Property<string> ModelNameProperty = P<ViceTransferFixtureDetail>.RegisterView(e => e.ModelName, p => p.ViceTransferFixture.FixtureEncode.FixtureModel.Name);

        /// <summary>
        /// 型号编码
        /// </summary>
        public string ModelName
        {
            get { return this.GetProperty(ModelNameProperty); }
            set { this.SetProperty(ModelNameProperty, value); }
        }
        #endregion


        #region 管控模式 ManageMode
        /// <summary>
        /// 管理模式
        /// </summary>
        [Label("管控模式")]
        public static readonly Property<ManageMode> ManageModeProperty = P<ViceTransferFixtureDetail>.RegisterView(e => e.ManageMode, p => p.ViceTransferFixture.FixtureEncode.FixtureModel.ManageMode);

        /// <summary>
        /// 
        /// </summary>
        public ManageMode ManageMode
        {
            get { return this.GetProperty(ManageModeProperty); }
            set { this.SetProperty(ManageModeProperty, value); }
        }
        #endregion


        #region 工治具类型 FixtureTypeCode
        /// <summary>
        /// 工治具类型
        /// </summary>
        [Label("工治具类型")]
        public static readonly Property<string> FixtureTypeCodeProperty = P<ViceTransferFixtureDetail>.RegisterView(e => e.FixtureTypeCode, p => p.ViceTransferFixture.FixtureEncode.FixtureModel.FixtureType.Code);

        /// <summary>
        /// 工治具类型
        /// </summary>
        public string FixtureTypeCode
        {
            get { return this.GetProperty(FixtureTypeCodeProperty); }
            set { this.SetProperty(FixtureTypeCodeProperty, value); }
        }
        #endregion

        #region 单位 UintName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UintNameProperty = P<ViceTransferFixtureDetail>.RegisterView(e => e.UintName, p => p.ViceTransferFixture.FixtureEncode.FixtureModel.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string UintName
        {
            get { return this.GetProperty(UintNameProperty); }
            set { this.SetProperty(UintNameProperty, value); }
        }
        #endregion

        #region 剩余需求数量 RemainingDemandQty
        /// <summary>
        /// 剩余需求数量
        /// </summary>
        [Label("剩余需求数量")]
        public static readonly Property<decimal> RemainingDemandQtyProperty = P<ViceTransferFixtureDetail>.RegisterReadOnly(
            e => e.RemainingDemandQty, e => e.GetRemainingDemandQty(), ViceTransferFixtureProperty);
        /// <summary>
        /// 剩余需求数量
        /// </summary>

        public decimal RemainingDemandQty
        {
            get { return this.GetProperty(RemainingDemandQtyProperty); }
        }
        private decimal GetRemainingDemandQty()
        {
            return ViceTransferFixture.Qty - ViceTransferFixture.TransferQty;
        }
        #endregion

        #region 质量状态 FixtureQualityState
        /// <summary>
        /// 质量状态
        /// </summary>
        [Label("质量状态")]
        public static readonly Property<FixtureQualityState> FixtureQualityStateProperty = P<ViceTransferFixtureDetail>.Register(e => e.FixtureQualityState);

        /// <summary>
        /// 质量状态
        /// </summary>
        public FixtureQualityState FixtureQualityState
        {
            get { return this.GetProperty(FixtureQualityStateProperty); }
            set { this.SetProperty(FixtureQualityStateProperty, value); }
        }
        #endregion

        #region 在线数 OnlineQty  
        /// <summary>
        /// 在线数
        /// </summary>
        [Label("在线数")]
        public static readonly Property<decimal> OnlineQtyProperty = P<ViceTransferFixtureDetail>.Register(e => e.OnlineQty);

        /// <summary>
        /// 在线数
        /// </summary>
        public decimal OnlineQty
        {
            get { return this.GetProperty(OnlineQtyProperty); }
            set { this.SetProperty(OnlineQtyProperty, value); }
        }
        #endregion

        #region 在线/在库 FixtureStatus
        /// <summary>
        /// 在线/在库
        /// </summary>
        [Label("在线/在库")]
        public static readonly Property<FixtureStatus> FixtureStatusProperty = P<ViceTransferFixtureDetail>.Register(e => e.FixtureStatus);

        /// <summary>
        /// 在线/在库
        /// </summary>
        public FixtureStatus FixtureStatus
        {
            get { return this.GetProperty(FixtureStatusProperty); }
            set { this.SetProperty(FixtureStatusProperty, value); }
        }
        #endregion


        #region 行号 LineNo
        /// <summary>
        /// 行号
        /// </summary>
        [Label("行号")]
        public static readonly Property<string> LineNoProperty = P<ViceTransferFixtureDetail>.RegisterView(e => e.LineNo, p => p.ViceTransferFixture.LineNo);

        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo
        {
            get { return this.GetProperty(LineNoProperty); }
            set { this.SetProperty(LineNoProperty, value); }
        }
        #endregion


        #region 来源仓库Id WarehouseId
        /// <summary>
        /// 来源仓库Id
        /// </summary>
        [Label("来源仓库Id")]
        public static readonly Property<double> WarehouseIdProperty = P<ViceTransferFixtureDetail>.Register(e => e.WarehouseId);

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
        public static readonly Property<double> TargetWarehouseIdProperty = P<ViceTransferFixtureDetail>.Register(e => e.TargetWarehouseId);

        /// <summary>
        /// 目标仓库Id
        /// </summary>
        public double TargetWarehouseId
        {
            get { return this.GetProperty(TargetWarehouseIdProperty); }
            set { this.SetProperty(TargetWarehouseIdProperty, value); }
        }
        #endregion

        #region 来源库位库存数 SourceInventoryQty
        /// <summary>
        /// 来源库位库存数
        /// </summary>
        [Label("来源库位库存数")]
        public static readonly Property<int> SourceInventoryQtyProperty = P<ViceTransferFixtureDetail>.Register(e => e.SourceInventoryQty);

        /// <summary>
        /// 来源库位库存数
        /// </summary>
        public int SourceInventoryQty
        {
            get { return this.GetProperty(SourceInventoryQtyProperty); }
            set { this.SetProperty(SourceInventoryQtyProperty, value); }
        }
        #endregion


        #region 车间 WorkShop
        /// <summary>
        /// 车间
        /// </summary>
        [Label("车间")]
        public static readonly Property<string> WorkShopProperty = P<ViceTransferFixtureDetail>.Register(e => e.WorkShop);

        /// <summary>
        /// 车间
        /// </summary>
        public string WorkShop
        {
            get { return this.GetProperty(WorkShopProperty); }
            set { this.SetProperty(WorkShopProperty, value); }
        }
        #endregion



        #region 产线 Resoures
        /// <summary>
        /// 产线
        /// </summary>
        [Label("产线")]
        public static readonly Property<string> ResouresProperty = P<ViceTransferFixtureDetail>.Register(e => e.Resoures);

        /// <summary>
        /// 产线
        /// </summary>
        public string Resoures
        {
            get { return this.GetProperty(ResouresProperty); }
            set { this.SetProperty(ResouresProperty, value); }
        }
        #endregion


    }

    /// <summary>
    /// 工治具调拨明细 实体配置
    /// </summary>
    internal class ViceTransferFixtureDetailConfig : EntityConfig<ViceTransferFixtureDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_VICE_TRAN_FIX_DTL").MapAllPropertiesExcept(ViceTransferFixtureDetail.OnlineQtyProperty,
                ViceTransferFixtureDetail.TargetWarehouseIdProperty, ViceTransferFixtureDetail.WarehouseIdProperty,
                ViceTransferFixtureDetail.SourceInventoryQtyProperty
                );
            Meta.EnablePhantoms();
        }
    }
}