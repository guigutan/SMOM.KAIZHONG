using SIE;
using SIE.Common.Configs.CommonConfigs;
using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Equipments.Enums;
using SIE.Fixtures.FixtureRecords;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.MaintainTasks;
using SIE.Fixtures.Models;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.Fixtures.InboundOrders
{
    /// <summary>
    /// 工治具入库
    /// </summary>
    [RootEntity, Serializable]
    //[CriteriaQuery]
    [Label("工治具入库")]
    [Common.Configs.EntityWithConfig(typeof(NoConfig), "工治具入库单号", "工治具入库单号配置项")]
    [ConditionQueryType(typeof(InboundOrderCriteria))]
    public partial class InboundOrder : DataEntity
    {
        #region 入库单号 No
        /// <summary>
        /// 单号
        /// </summary>
        [Label("入库单号")]
        [NotDuplicate]
        [Required]
        public static readonly Property<string> NoProperty = P<InboundOrder>.Register(e => e.No);

        /// <summary>
        /// 入库单号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 相关单号 RelevantOrderNo
        /// <summary>
        /// 相关单号
        /// </summary>
        [Label("相关单号")]
        public static readonly Property<string> RelevantOrderNoProperty = P<InboundOrder>.Register(e => e.RelevantOrderNo);

        /// <summary>
        /// 相关单号
        /// </summary>
        public string RelevantOrderNo
        {
            get { return this.GetProperty(RelevantOrderNoProperty); }
            set { this.SetProperty(RelevantOrderNoProperty, value); }
        }
        #endregion

        #region 工治具接收单号 ReceiptOrderNo
        /// <summary>
        /// 接收单号
        /// </summary>
        [Label("接收单号")]
        public static readonly Property<string> ReceiptOrderNoProperty = P<InboundOrder>.Register(e => e.ReceiptOrderNo);

        /// <summary>
        /// 接收单号
        /// </summary>
        public string ReceiptOrderNo
        {
            get { return GetProperty(ReceiptOrderNoProperty); }
            set { SetProperty(ReceiptOrderNoProperty, value); }
        }
        #endregion

        #region 工治具验收单号 AcceptanceOrderNo
        /// <summary>
        /// 验收单号
        /// </summary>
        [Label("验收单号")]
        public static readonly Property<string> AcceptanceOrderNoProperty = P<InboundOrder>.Register(e => e.AcceptanceOrderNo);

        /// <summary>
        /// 工治具验收单号
        /// </summary>
        public string AcceptanceOrderNo
        {
            get { return GetProperty(AcceptanceOrderNoProperty); }
            set { SetProperty(AcceptanceOrderNoProperty, value); }
        }
        #endregion

        #region 入库数 Qty
        /// <summary>
        /// 入库数
        /// </summary>
        [Label("入库数")]
        public static readonly Property<decimal> QtyProperty = P<InboundOrder>.Register(e => e.Qty);

        /// <summary>
        /// 入库数
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 入库日期 InboundDate
        /// <summary>
        /// 入库日期
        /// </summary>
        [Label("入库日期")]
        public static readonly Property<DateTime?> InboundDateProperty = P<InboundOrder>.Register(e => e.InboundDate);

        /// <summary>
        /// 入库日期
        /// </summary>
        public DateTime? InboundDate
        {
            get { return GetProperty(InboundDateProperty); }
            set { SetProperty(InboundDateProperty, value); }
        }
        #endregion

        #region 工治具入库-编码类入库明细列表 InboundOrderFixtureCodeAccountList
        /// <summary>
        /// 工治具入库-编码类入库明细列表
        /// </summary>
        public static readonly ListProperty<EntityList<InboundOrderFixtureCodeAccount>> InboundOrderFixtureCodeAccountListProperty = P<InboundOrder>.RegisterList(e => e.InboundOrderFixtureCodeAccountList);
        /// <summary>
        /// 工治具入库-编码类入库明细列表
        /// </summary>
        public EntityList<InboundOrderFixtureCodeAccount> InboundOrderFixtureCodeAccountList
        {
            get { return this.GetLazyList(InboundOrderFixtureCodeAccountListProperty); }
        }
        #endregion

        #region 工治具入库采购信息列表 InboundOrderPurchaseList
        /// <summary>
        /// 工治具入库采购信息列表
        /// </summary>
        public static readonly ListProperty<EntityList<InboundOrderPurchase>> InboundOrderPurchaseListProperty = P<InboundOrder>.RegisterList(e => e.InboundOrderPurchaseList);
        /// <summary>
        /// 工治具入库采购信息列表
        /// </summary>
        public EntityList<InboundOrderPurchase> InboundOrderPurchaseList
        {
            get { return this.GetLazyList(InboundOrderPurchaseListProperty); }
        }
        #endregion

        #region 工治具入库-ID类入库明细列表 InboundOrderFixtureIdAccountList
        /// <summary>
        /// 工治具入库-ID类入库明细列表
        /// </summary>
        public static readonly ListProperty<EntityList<InboundOrderFixtureIdAccount>> InboundOrderFixtureIdAccountListProperty = P<InboundOrder>.RegisterList(e => e.InboundOrderFixtureIdAccountList);
        /// <summary>
        /// 工治具入库-ID类入库明细列表
        /// </summary>
        public EntityList<InboundOrderFixtureIdAccount> InboundOrderFixtureIdAccountList
        {
            get { return this.GetLazyList(InboundOrderFixtureIdAccountListProperty); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        public static readonly IRefIdProperty WarehouseIdProperty = P<InboundOrder>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)GetRefNullableId(WarehouseIdProperty); }
            set { SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<InboundOrder>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 客户 Customer
        /// <summary>
        /// 客户Id
        /// </summary>
        public static readonly IRefIdProperty CustomerIdProperty = P<InboundOrder>.RegisterRefId(e => e.CustomerId, ReferenceType.Normal);

        /// <summary>
        /// 客户Id
        /// </summary>
        public double? CustomerId
        {
            get { return (double?)GetRefNullableId(CustomerIdProperty); }
            set { SetRefNullableId(CustomerIdProperty, value); }
        }

        /// <summary>
        /// 客户
        /// </summary>
        public static readonly RefEntityProperty<Customer> CustomerProperty = P<InboundOrder>.RegisterRef(e => e.Customer, CustomerIdProperty);

        /// <summary>
        /// 客户
        /// </summary>
        public Customer Customer
        {
            get { return GetRefEntity(CustomerProperty); }
            set { SetRefEntity(CustomerProperty, value); }
        }
        #endregion

        #region 保养单号 MaintainTask
        /// <summary>
        /// 保养单号Id
        /// </summary>
        [Label("保养单号")]
        public static readonly IRefIdProperty MaintainTaskIdProperty =
            P<InboundOrder>.RegisterRefId(e => e.MaintainTaskId, ReferenceType.Normal);

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
            P<InboundOrder>.RegisterRef(e => e.MaintainTask, MaintainTaskIdProperty);

        /// <summary>
        /// 保养单号
        /// </summary>
        public MaintainTask MaintainTask
        {
            get { return this.GetRefEntity(MaintainTaskProperty); }
            set { this.SetRefEntity(MaintainTaskProperty, value); }
        }
        #endregion

        #region 保养状态 MaintainStatus
        /// <summary>
        /// 保养状态
        /// </summary>
        [Label("保养状态")]
        public static readonly Property<MaintainState?> MaintainStatusProperty = P<InboundOrder>.RegisterView(e => e.MaintainStatus, p => p.MaintainTask.State);

        /// <summary>
        /// 保养状态
        /// </summary>
        public MaintainState? MaintainStatus
        {
            get { return this.GetProperty(MaintainStatusProperty); }
        }
        #endregion

        #region 供应商 Supplier
        /// <summary>
        /// 供应商Id
        /// </summary>
        public static readonly IRefIdProperty SupplierIdProperty = P<InboundOrder>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

        /// <summary>
        /// 供应商Id
        /// </summary>
        public double? SupplierId
        {
            get { return (double?)GetRefNullableId(SupplierIdProperty); }
            set { SetRefNullableId(SupplierIdProperty, value); }
        }

        /// <summary>
        /// 供应商
        /// </summary>
        public static readonly RefEntityProperty<Supplier> SupplierProperty = P<InboundOrder>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier Supplier
        {
            get { return GetRefEntity(SupplierProperty); }
            set { SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 工治具编码 FixtureEncode
        /// <summary>
        /// 工治具编码Id
        /// </summary>
        public static readonly IRefIdProperty FixtureEncodeIdProperty = P<InboundOrder>.RegisterRefId(e => e.FixtureEncodeId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<FixtureEncode> FixtureEncodeProperty = P<InboundOrder>.RegisterRef(e => e.FixtureEncode, FixtureEncodeIdProperty);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public FixtureEncode FixtureEncode
        {
            get { return GetRefEntity(FixtureEncodeProperty); }
            set { SetRefEntity(FixtureEncodeProperty, value); }
        }
        #endregion

        #region 入库状态 InboundStatus
        /// <summary>
        /// 入库状态
        /// </summary>
        [Label("入库状态")]
        public static readonly Property<InboundStatus> InboundStatusProperty = P<InboundOrder>.Register(e => e.InboundStatus);

        /// <summary>
        /// 入库状态
        /// </summary>
        public InboundStatus InboundStatus
        {
            get { return GetProperty(InboundStatusProperty); }
            set { SetProperty(InboundStatusProperty, value); }
        }
        #endregion

        #region 产权归属 Proprietorship
        /// <summary>
        /// 产权归属
        /// </summary>
        [Label("产权归属")]
        public static readonly Property<Proprietorship> ProprietorshipProperty = P<InboundOrder>.Register(e => e.Proprietorship);

        /// <summary>
        /// 产权归属
        /// </summary>
        public Proprietorship Proprietorship
        {
            get { return GetProperty(ProprietorshipProperty); }
            set { SetProperty(ProprietorshipProperty, value); }
        }
        #endregion

        #region 管理方式 ManageMode
        /// <summary>
        /// 管理方式
        /// </summary>
        [Required]
        [Label("管理方式")]

        public static readonly Property<ManageMode> ManageModeProperty = P<InboundOrder>.RegisterView(e =>e.ManageMode, p=>p.FixtureEncode.FixtureModel.ManageMode);

        /// <summary>
        /// 管理方式
        /// </summary>
        public ManageMode ManageMode
        {
            get { return GetProperty(ManageModeProperty); }
        }
        #endregion

        #region 入库类型 InboundType
        /// <summary>
        /// 入库类型
        /// </summary>
        [Label("入库类型")]
        public static readonly Property<FixtureInboundType> InboundTypeProperty = P<InboundOrder>.Register(e => e.InboundType);

        /// <summary>
        /// 入库类型
        /// </summary>
        public FixtureInboundType InboundType
        {
            get { return GetProperty(InboundTypeProperty); }
            set { SetProperty(InboundTypeProperty, value); }
        }
        #endregion

        #region 工治具类型 FixtureType
        /// <summary>
        /// 工治具类型
        /// </summary>
        [Label("工治具类型")]
        public static readonly Property<string> FixtureTypeProperty = P<InboundOrder>.RegisterView(e => e.FixtureType, p => p.FixtureEncode.FixtureModel.FixtureType.Code);

        /// <summary>
        /// 工治具类型
        /// </summary>
        public string FixtureType
        {
            get { return this.GetProperty(FixtureTypeProperty); }
        }
        #endregion

        #region 供应商名称 SupplierName
        /// <summary>
        /// 供应商名称
        /// </summary>
        [Label("供应商名称")]
        public static readonly Property<string> SupplierNameProperty = P<InboundOrder>.RegisterView(e => e.SupplierName, p => p.Supplier.Name);

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName
        {
            get { return this.GetProperty(SupplierNameProperty); }
        }
        #endregion

        #region 客户名称 CustomerName
        /// <summary>
        /// 客户名称    
        /// </summary>
        [Label("客户名称")]
        public static readonly Property<string> CustomerNameProperty = P<InboundOrder>.RegisterView(e => e.CustomerName, p => p.Customer.Name);

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName
        {
            get { return this.GetProperty(CustomerNameProperty); }
        }
        #endregion

        #region 已扫数量 ScanedNum
        /// <summary>
        /// 
        /// </summary>
        [Label("已扫数量")]
        public static readonly Property<int> ScanedNumProperty = P<InboundOrder>.Register(e => e.ScanedNum);

        /// <summary>
        /// 已扫数量
        /// </summary>
        public int ScanedNum
        {
            get { return this.GetProperty(ScanedNumProperty); }
            set { this.SetProperty(ScanedNumProperty, value); }
        }
        #endregion

        #region 质量状态 QualityState
        /// <summary>
        /// 质量状态
        /// </summary>
        [Label("质量状态")]
        public static readonly Property<FixtureQualityState?> QualityStateProperty = P<InboundOrder>.Register(e => e.QualityState);

        /// <summary>
        /// 质量状态
        /// </summary>
        public FixtureQualityState? QualityState
        {
            get { return GetProperty(QualityStateProperty); }
            set { SetProperty(QualityStateProperty, value); }
        }
        #endregion

        #region 入库完成时间 FinishDate
        /// <summary>
        /// 入库完成时间
        /// </summary>
        [Label("入库完成时间")]
        public static readonly Property<DateTime?> FinishDateProperty = P<InboundOrder>.Register(e => e.FinishDate);

        /// <summary>
        /// 入库完成时间
        /// </summary>
        public DateTime? FinishDate
        {
            get { return this.GetProperty(FinishDateProperty); }
            set { this.SetProperty(FinishDateProperty, value); }
        }
        #endregion

    }

    /// <summary>
    ///  实体配置
    /// </summary>
    internal class InboundOrderConfig : EntityConfig<InboundOrder>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_FIXTURE_IN_ORDER").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}