using SIE;
using SIE.Domain;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.MaintainTasks;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.Fixtures.InboundOrders
{
    /// <summary>
    /// 工治具入库-ID类入库明细
    /// </summary>
    [ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("工治具入库-ID类入库明细")]
    public partial class InboundOrderFixtureIdAccount : DataEntity
    {
        #region 行号 LineNo
        /// <summary>
        /// 行号
        /// </summary>
        [Label("行号")]
        public static readonly Property<string> LineNoProperty = P<InboundOrderFixtureIdAccount>.Register(e => e.LineNo);

        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo
        {
            get { return GetProperty(LineNoProperty); }
            set { SetProperty(LineNoProperty, value); }
        }
        #endregion

        #region RFID号 Rfid
        /// <summary>
        /// RFID号
        /// </summary>
        [Label("RFID号")]
        public static readonly Property<string> RfidProperty = P<InboundOrderFixtureIdAccount>.Register(e => e.Rfid);

        /// <summary>
        /// RFID号
        /// </summary>
        public string Rfid
        {
            get { return GetProperty(RfidProperty); }
            set { SetProperty(RfidProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<InboundOrderFixtureIdAccount>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 采购订单号 PoNo
        /// <summary>
        /// 采购订单号
        /// </summary>
        [Label("采购订单号")]
        public static readonly Property<string> PoNoProperty = P<InboundOrderFixtureIdAccount>.Register(e => e.PoNo);

        /// <summary>
        /// 采购订单号
        /// </summary>
        public string PoNo
        {
            get { return GetProperty(PoNoProperty); }
            set { SetProperty(PoNoProperty, value); }
        }
        #endregion

        #region 采购订单行号 PoLineNo
        /// <summary>
        /// 采购订单行号
        /// </summary>
        [Label("采购订单行号")]
        public static readonly Property<string> PoLineNoProperty = P<InboundOrderFixtureIdAccount>.Register(e => e.PoLineNo);

        /// <summary>
        /// 采购订单行号
        /// </summary>
        public string PoLineNo
        {
            get { return GetProperty(PoLineNoProperty); }
            set { SetProperty(PoLineNoProperty, value); }
        }
        #endregion


        #region 保养单号 MaintainTask
        /// <summary>
        /// 保养单号Id
        /// </summary>
        [Label("保养单号")]
        public static readonly IRefIdProperty MaintainTaskIdProperty =
            P<InboundOrderFixtureIdAccount>.RegisterRefId(e => e.MaintainTaskId, ReferenceType.Normal);

        /// <summary>
        /// 保养单号Id
        /// </summary>
        public double? MaintainTaskId
        {
            get { return (double?)this.GetRefNullableId(MaintainTaskIdProperty); }
            set { this.SetRefNullableId(MaintainTaskIdProperty, value); }
        }

        /// <summary>
        /// 保养单号
        /// </summary>
        public static readonly RefEntityProperty<MaintainTask> MaintainTaskProperty =
            P<InboundOrderFixtureIdAccount>.RegisterRef(e => e.MaintainTask, MaintainTaskIdProperty);

        /// <summary>
        /// 保养单号
        /// </summary>
        public MaintainTask MaintainTask
        {
            get { return this.GetRefEntity(MaintainTaskProperty); }
            set { this.SetRefEntity(MaintainTaskProperty, value); }
        }
        #endregion


        #region 保养状态 MaintainState
        /// <summary>
        /// 保养状态
        /// </summary>
        [Label("保养状态")]
        public static readonly Property<MaintainState?> MaintainStateProperty = P<InboundOrderFixtureIdAccount>.RegisterView(e => e.MaintainState, p => p.MaintainTask.State);

        /// <summary>
        /// 保养状态
        /// </summary>
        public MaintainState? MaintainState
        {
            get { return this.GetProperty(MaintainStateProperty); }
        }
        #endregion

        #region 单价 Price
        /// <summary>
        /// 单价
        /// </summary>
        [Label("单价")]
        public static readonly Property<decimal> PriceProperty = P<InboundOrderFixtureIdAccount>.Register(e => e.Price);

        /// <summary>
        /// 单价
        /// </summary>
        public decimal Price
        {
            get { return GetProperty(PriceProperty); }
            set { SetProperty(PriceProperty, value); }
        }
        #endregion

        #region 原厂序列号 OriginalSerialNumber
        /// <summary>
        /// 原厂序列号
        /// </summary>
        [Label("原厂序列号")]
        public static readonly Property<string> OriginalSerialNumberProperty = P<InboundOrderFixtureIdAccount>.Register(e => e.OriginalSerialNumber);

        /// <summary>
        /// 原厂序列号
        /// </summary>
        public string OriginalSerialNumber
        {
            get { return GetProperty(OriginalSerialNumberProperty); }
            set { SetProperty(OriginalSerialNumberProperty, value); }
        }
        #endregion

        #region 工治具台账（ID管理） FixtureIDAccount
        /// <summary>
        /// 工治具台账（ID管理）Id
        /// </summary>
        [Label("工治具台账")]
        public static readonly IRefIdProperty FixtureIDAccountIdProperty = P<InboundOrderFixtureIdAccount>.RegisterRefId(e => e.FixtureIDAccountId, ReferenceType.Normal);

        /// <summary>
        /// 工治具台账（ID管理）Id
        /// </summary>
        public double FixtureIDAccountId
        {
            get { return (double)GetRefId(FixtureIDAccountIdProperty); }
            set { SetRefId(FixtureIDAccountIdProperty, value); }
        }

        /// <summary>
        /// 工治具台账（ID管理）
        /// </summary>
        public static readonly RefEntityProperty<FixtureIDAccount> FixtureIDAccountProperty = P<InboundOrderFixtureIdAccount>.RegisterRef(e => e.FixtureIDAccount, FixtureIDAccountIdProperty);

        /// <summary>
        /// 工治具台账（ID管理）
        /// </summary>
        public FixtureIDAccount FixtureIDAccount
        {
            get { return GetRefEntity(FixtureIDAccountProperty); }
            set { SetRefEntity(FixtureIDAccountProperty, value); }
        }
        #endregion

        #region 库位 StorageLocation
        /// <summary>
        /// 库位Id
        /// </summary>
        public static readonly IRefIdProperty StorageLocationIdProperty = P<InboundOrderFixtureIdAccount>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

        /// <summary>
        /// 库位Id
        /// </summary>
        public double? StorageLocationId
        {
            get { return (double?)GetRefNullableId(StorageLocationIdProperty); }
            set { SetRefNullableId(StorageLocationIdProperty, value); }
        }

        /// <summary>
        /// 库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty = P<InboundOrderFixtureIdAccount>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return GetRefEntity(StorageLocationProperty); }
            set { SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion

        #region  InboundOrder
        /// <summary>
        /// Id
        /// </summary>
        public static readonly IRefIdProperty InboundOrderIdProperty = P<InboundOrderFixtureIdAccount>.RegisterRefId(e => e.InboundOrderId, ReferenceType.Parent);

        /// <summary>
        /// Id
        /// </summary>
        public double InboundOrderId
        {
            get { return (double)GetRefId(InboundOrderIdProperty); }
            set { SetRefId(InboundOrderIdProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly RefEntityProperty<InboundOrder> InboundOrderProperty = P<InboundOrderFixtureIdAccount>.RegisterRef(e => e.InboundOrder, InboundOrderIdProperty);

        /// <summary>
        /// 
        /// </summary>
        public InboundOrder InboundOrder
        {
            get { return GetRefEntity(InboundOrderProperty); }
            set { SetRefEntity(InboundOrderProperty, value); }
        }
        #endregion


        #region 工治具ID IDCode
        /// <summary>
        /// 工治具ID
        /// </summary>
        [Label("工治具ID")]
        public static readonly Property<string> IDCodeProperty = P<InboundOrderFixtureIdAccount>.RegisterView(e => e.IDCode, p => p.FixtureIDAccount.Code);

        /// <summary>
        /// IDCode
        /// </summary>
        public string IDCode
        {
            get { return this.GetProperty(IDCodeProperty); }
        }
        #endregion


        #region 资产编码 AssetCode	
        /// <summary>
        /// 资产编码
        /// </summary>
        [Label("资产编码")]
        public static readonly Property<string> AssetCodeProperty = P<InboundOrderFixtureIdAccount>.RegisterView(e => e.AssetCode, p => p.FixtureIDAccount.AssetCode);

        /// <summary>
        /// 资产编码
        /// </summary>
        public string AssetCode
        {
            get { return this.GetProperty(AssetCodeProperty); }
        }
        #endregion


        #region 生产日期 ProductionDate	
        /// <summary>
        /// 生产日期
        /// </summary>
        [Label("生产日期")]
        public static readonly Property<DateTime?> ProductionDateProperty = P<InboundOrderFixtureIdAccount>.RegisterView(e => e.ProductionDate, p => p.FixtureIDAccount.ProductionDate);

        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime? ProductionDate
        {
            get { return this.GetProperty(ProductionDateProperty); }
        }
        #endregion

        #region 厂商名称 Manufacturer	
        /// <summary>
        /// 产商名称
        /// </summary>
        [Label("厂商名称")]
        public static readonly Property<string> ManufacturerProperty = P<InboundOrderFixtureIdAccount>.RegisterView(e => e.Manufacturer, p => p.FixtureIDAccount.Manufacturer);

        /// <summary>
        /// 产商名称
        /// </summary>
        public string Manufacturer
        {
            get { return this.GetProperty(ManufacturerProperty); }
        }
        #endregion



    }

    /// <summary>
    /// 工治具入库-ID类入库明细 实体配置
    /// </summary>
    internal class InboundOrderFixtureIdAccountConfig : EntityConfig<InboundOrderFixtureIdAccount>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_FIXTURE_IN_ID").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}