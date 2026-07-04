using SIE.Domain;
using SIE.Fixtures;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.Models;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.ViceTransfers
{
    /// <summary>
    /// 工治具需求清单
    /// </summary>
    [ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("工治具需求清单")]
    public partial class ViceTransferFixture : DataEntity
    {
        #region 行号 LineNo
        /// <summary>
        /// 行号
        /// </summary>
        [Label("行号")]
        public static readonly Property<string> LineNoProperty = P<ViceTransferFixture>.Register(e => e.LineNo);

        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo
        {
            get { return GetProperty(LineNoProperty); }
            set { SetProperty(LineNoProperty, value); }
        }
        #endregion

        #region 需求数量 Qty
        /// <summary>
        /// 需求数量
        /// </summary>
        [Label("需求数量")]
        public static readonly Property<int> QtyProperty = P<ViceTransferFixture>.Register(e => e.Qty);

        /// <summary>
        /// 需求数量
        /// </summary>
        public int Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 调拨数量 TransferQty
        /// <summary>
        /// 调拨数量
        /// </summary>
        [Label("调拨数量")]
        public static readonly Property<int> TransferQtyProperty = P<ViceTransferFixture>.Register(e => e.TransferQty);

        /// <summary>
        /// 调拨数量
        /// </summary>
        public int TransferQty
        {
            get { return GetProperty(TransferQtyProperty); }
            set { SetProperty(TransferQtyProperty, value); }
        }
        #endregion

        #region 工治具编码 FixtureEncode
        /// <summary>
        /// 工治具编码Id
        /// </summary>
        [Label("工治具编码")]
        public static readonly IRefIdProperty FixtureEncodeIdProperty = P<ViceTransferFixture>.RegisterRefId(e => e.FixtureEncodeId, ReferenceType.Normal);

        /// <summary>
        /// 工治具编码Id
        /// </summary>
        public double FixtureEncodeId
        {
            get { return (double)GetRefId(FixtureEncodeIdProperty); }
            set { SetRefId(FixtureEncodeIdProperty, value); }
        }

        /// <summary>
        /// 工治具编码
        /// </summary>
        public static readonly RefEntityProperty<FixtureEncode> FixtureEncodeProperty = P<ViceTransferFixture>.RegisterRef(e => e.FixtureEncode, FixtureEncodeIdProperty);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public FixtureEncode FixtureEncode
        {
            get { return GetRefEntity(FixtureEncodeProperty); }
            set { SetRefEntity(FixtureEncodeProperty, value); }
        }
        #endregion

        #region 质量状态 FixtureQualityState
        /// <summary>
        /// 质量状态
        /// </summary>
        [Label("质量状态")]
        public static readonly Property<FixtureQualityState> FixtureQualityStateProperty = P<ViceTransferFixture>.Register(e => e.FixtureQualityState);

        /// <summary>
        /// 质量状态
        /// </summary>
        public FixtureQualityState FixtureQualityState
        {
            get { return GetProperty(FixtureQualityStateProperty); }
            set { SetProperty(FixtureQualityStateProperty, value); }
        }
        #endregion

        #region 工治具需求清单 ViceTransfer
        /// <summary>
        /// 工治具需求清单Id
        /// </summary>
        [Label("工治具需求清单")]
        public static readonly IRefIdProperty ViceTransferIdProperty = P<ViceTransferFixture>.RegisterRefId(e => e.ViceTransferId, ReferenceType.Parent);

        /// <summary>
        /// 工治具需求清单Id
        /// </summary>
        public double ViceTransferId
        {
            get { return (double)GetRefId(ViceTransferIdProperty); }
            set { SetRefId(ViceTransferIdProperty, value); }
        }

        /// <summary>
        /// 工治具需求清单
        /// </summary>
        public static readonly RefEntityProperty<ViceTransfer> ViceTransferProperty = P<ViceTransferFixture>.RegisterRef(e => e.ViceTransfer, ViceTransferIdProperty);

        /// <summary>
        /// 工治具需求清单
        /// </summary>
        public ViceTransfer ViceTransfer
        {
            get { return GetRefEntity(ViceTransferProperty); }
            set { SetRefEntity(ViceTransferProperty, value); }
        }
        #endregion


        #region 型号编码 ModelCode
        /// <summary>
        /// 型号编码
        /// </summary>
        [Label("型号编码")]
        public static readonly Property<string> ModelCodeProperty = P<ViceTransferFixture>.RegisterView(e => e.ModelCode, p => p.FixtureEncode.FixtureModel.Code);

        /// <summary>
        /// 型号编码
        /// </summary>
        public string ModelCode
        {
            get { return this.GetProperty(ModelCodeProperty); }
        }
        #endregion


        #region 型号名称 ModelName
        /// <summary>
        /// 型号名称
        /// </summary>
        [Label("型号名称")]
        public static readonly Property<string> ModelNameProperty = P<ViceTransferFixture>.RegisterView(e => e.ModelName, p => p.FixtureEncode.FixtureModel.Name);

        /// <summary>
        /// 型号编码
        /// </summary>
        public string ModelName
        {
            get { return this.GetProperty(ModelNameProperty); }
        }
        #endregion


        #region 管控模式 ManageMode
        /// <summary>
        /// 管理模式
        /// </summary>
        [Label("管控模式")]
        public static readonly Property<ManageMode> ManageModeProperty = P<ViceTransferFixture>.RegisterView(e => e.ManageMode, p => p.FixtureEncode.FixtureModel.ManageMode);

        /// <summary>
        /// 
        /// </summary>
        public ManageMode ManageMode
        {
            get { return this.GetProperty(ManageModeProperty); }
        }
        #endregion


        #region 工治具类型 FixtureTypeCode
        /// <summary>
        /// 工治具类型
        /// </summary>
        [Label("工治具类型")]
        public static readonly Property<string> FixtureTypeCodeProperty = P<ViceTransferFixture>.RegisterView(e => e.FixtureTypeCode, p => p.FixtureEncode.FixtureModel.FixtureType.Code);

        /// <summary>
        /// 工治具类型
        /// </summary>
        public string FixtureTypeCode
        {
            get { return this.GetProperty(FixtureTypeCodeProperty); }
        }
        #endregion

        #region 在库数 InStorage
        /// <summary>
        /// 在库数
        /// </summary>
        [Label("在库数")]
        public static readonly Property<decimal> InStorageQtyProperty = P<ViceTransferFixture>.Register(e => e.InStorageQty);

        /// <summary>
        /// 在库数
        /// </summary>
        public decimal InStorageQty
        {
            get { return this.GetProperty(InStorageQtyProperty); }
            set { this.SetProperty(InStorageQtyProperty, value); }
        }
        #endregion

        #region 在线数 OnlineQty  
        /// <summary>
        /// 在线数
        /// </summary>
        [Label("在线数")]
        public static readonly Property<decimal> OnlineQtyProperty = P<ViceTransferFixture>.Register(e => e.OnlineQty);

        /// <summary>
        /// 在库数
        /// </summary>
        public decimal OnlineQty
        {
            get { return this.GetProperty(OnlineQtyProperty); }
            set { this.SetProperty(OnlineQtyProperty, value); }
        }
        #endregion


        #region 单位 UintName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UintNameProperty = P<ViceTransferFixture>.RegisterView(e => e.UintName, p => p.FixtureEncode.FixtureModel.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string UintName
        {
            get { return this.GetProperty(UintNameProperty); }
        }
        #endregion


        #region 工治具编码 FixtureEncodeCode
        /// <summary>
        /// 工治具编码
        /// </summary>
        [Label("工治具编码")]
        public static readonly Property<string> FixtureEncodeCodeProperty = P<ViceTransferFixture>.RegisterView(e => e.FixtureEncodeCode, p => p.FixtureEncode.Code);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public string FixtureEncodeCode
        {
            get { return this.GetProperty(FixtureEncodeCodeProperty); }
        }
        #endregion

        #region 来源仓库Id	 WarehouseId
        /// <summary>
        /// 来源仓库Id
        /// </summary>
        [Label("来源仓库Id")]
        public static readonly Property<double> WarehouseIdProperty = P<ViceTransferFixture>.RegisterView(e => e.WarehouseId, p => p.ViceTransfer.WarehouseId);

        /// <summary>
        /// 来源仓库Id
        /// </summary>
        public double WarehouseId
        {
            get { return this.GetProperty(WarehouseIdProperty); }
        }
        #endregion

        #region 目标仓库Id	 TargetWarehouseId
        /// <summary>
        /// 目标仓库Id
        /// </summary>
        [Label("目标仓库Id")]
        public static readonly Property<double> TargetWarehouseIdProperty = P<ViceTransferFixture>.RegisterView(e => e.TargetWarehouseId, p => p.ViceTransfer.TargetWarehouseId);

        /// <summary>
        /// 目标仓库Id
        /// </summary>
        public double TargetWarehouseId
        {
            get { return this.GetProperty(TargetWarehouseIdProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 工治具需求清单 实体配置
    /// </summary>
    internal class ViceTransferFixtureConfig : EntityConfig<ViceTransferFixture>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_VICE_TRAN_FIX").MapAllPropertiesExcept(ViceTransferFixture.OnlineQtyProperty, ViceTransferFixture.InStorageQtyProperty);
            Meta.EnablePhantoms();
        }
    }
}