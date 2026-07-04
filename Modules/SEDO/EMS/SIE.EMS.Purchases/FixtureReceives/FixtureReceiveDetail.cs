using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.EMS.Purchases.PurchaseOrders;
using SIE.Equipments.Enums;
using SIE.Fixtures;
using SIE.Fixtures.Models;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.EMS.Purchases.FixtureReceives
{
    /// <summary>
    /// 工治具接收明细
    /// </summary>
    [ChildEntity, Serializable]
    [DisplayMember(nameof(LineNo))]
    [Label("工治具接收明细")]
    public partial class FixtureReceiveDetail : DataEntity
    {
        #region 行号 LineNo
        /// <summary>
        /// 行号
        /// </summary>
        [Label("行号")]
        public static readonly Property<string> LineNoProperty = P<FixtureReceiveDetail>.Register(e => e.LineNo);

        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo
        {
            get { return GetProperty(LineNoProperty); }
            set { SetProperty(LineNoProperty, value); }
        }
        #endregion

        #region 赠品 Giveaway
        /// <summary>
        /// 赠品
        /// </summary>
        [Label("赠品")]
        public static readonly Property<bool> GiveawayProperty = P<FixtureReceiveDetail>.Register(e => e.Giveaway);

        /// <summary>
        /// 赠品
        /// </summary>
        public bool Giveaway
        {
            get { return GetProperty(GiveawayProperty); }
            set { SetProperty(GiveawayProperty, value); }
        }
        #endregion

        #region 单价（含税） Price
        /// <summary>
        /// 单价（含税）
        /// </summary>
        [Label("单价（含税）")]
        [MinValue(0)]
        [Required]
        public static readonly Property<decimal> PriceProperty = P<FixtureReceiveDetail>.Register(e => e.Price);

        /// <summary>
        /// 单价（含税）
        /// </summary>
        public decimal Price
        {
            get { return GetProperty(PriceProperty); }
            set { SetProperty(PriceProperty, value); }
        }
        #endregion

        #region 税率(%) TaxRate
        /// <summary>
        /// 税率(%)
        /// </summary>
        [Label("税率(%)")]
        [MinValue(0)]
        [MaxValue(100)]
        [Required]
        public static readonly Property<decimal> TaxRateProperty = P<FixtureReceiveDetail>.Register(e => e.TaxRate);

        /// <summary>
        /// 税率(%)
        /// </summary>
        public decimal TaxRate
        {
            get { return GetProperty(TaxRateProperty); }
            set { SetProperty(TaxRateProperty, value); }
        }
        #endregion

        #region 单位 Unit
        /// <summary>
        /// 单位Id
        /// </summary>
        [Label("单位")]
        public static readonly IRefIdProperty UnitIdProperty =
            P<FixtureReceiveDetail>.RegisterRefId(e => e.UnitId, ReferenceType.Normal);

        /// <summary>
        /// 单位Id
        /// </summary>
        public double UnitId
        {
            get { return (double)this.GetRefId(UnitIdProperty); }
            set { this.SetRefId(UnitIdProperty, value); }
        }

        /// <summary>
        /// 单位
        /// </summary>
        public static readonly RefEntityProperty<Unit> UnitProperty =
            P<FixtureReceiveDetail>.RegisterRef(e => e.Unit, UnitIdProperty);

        /// <summary>
        /// 单位
        /// </summary>
        public Unit Unit
        {
            get { return this.GetRefEntity(UnitProperty); }
            set { this.SetRefEntity(UnitProperty, value); }
        }
        #endregion

        #region 接收数量 Qty
        /// <summary>
        /// 接收数量
        /// </summary>
        [Label("接收数量")]
        public static readonly Property<int> QtyProperty = P<FixtureReceiveDetail>.Register(e => e.Qty);

        /// <summary>
        /// 接收数量
        /// </summary>
        public int Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 已接收数量 RecivedQty
        /// <summary>
        /// 已接收数量
        /// </summary>
        [Label("已接收数量")]
        public static readonly Property<int> RecivedQtyProperty = P<FixtureReceiveDetail>.Register(e => e.RecivedQty);

        /// <summary>
        /// 已接收数量
        /// </summary>
        public int RecivedQty
        {
            get { return GetProperty(RecivedQtyProperty); }
            set { SetProperty(RecivedQtyProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(1000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<FixtureReceiveDetail>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 客户 Customer
        /// <summary>
        /// 客户Id
        /// </summary>
        [Label("客户")]
        public static readonly IRefIdProperty CustomerIdProperty = P<FixtureReceiveDetail>.RegisterRefId(e => e.CustomerId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Customer> CustomerProperty = P<FixtureReceiveDetail>.RegisterRef(e => e.Customer, CustomerIdProperty);

        /// <summary>
        /// 客户
        /// </summary>
        public Customer Customer
        {
            get { return GetRefEntity(CustomerProperty); }
            set { SetRefEntity(CustomerProperty, value); }
        }
        #endregion

        #region 供应商 Supplier
        /// <summary>
        /// 供应商Id
        /// </summary>
        [Label("供应商")]
        public static readonly IRefIdProperty SupplierIdProperty = P<FixtureReceiveDetail>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Supplier> SupplierProperty = P<FixtureReceiveDetail>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier Supplier
        {
            get { return GetRefEntity(SupplierProperty); }
            set { SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 采购订单明细 PurchaseOrderItem
        /// <summary>
        /// 采购订单明细Id
        /// </summary>
        public static readonly IRefIdProperty PurchaseOrderItemIdProperty = P<FixtureReceiveDetail>.RegisterRefId(e => e.PurchaseOrderItemId, ReferenceType.Normal);

        /// <summary>
        /// 采购订单明细Id
        /// </summary>
        public double? PurchaseOrderItemId
        {
            get { return (double?)GetRefNullableId(PurchaseOrderItemIdProperty); }
            set { SetRefNullableId(PurchaseOrderItemIdProperty, value); }
        }

        /// <summary>
        /// 采购订单明细
        /// </summary>
        public static readonly RefEntityProperty<PurchaseOrderItem> PurchaseOrderItemProperty = P<FixtureReceiveDetail>.RegisterRef(e => e.PurchaseOrderItem, PurchaseOrderItemIdProperty);

        /// <summary>
        /// 采购订单明细
        /// </summary>
        public PurchaseOrderItem PurchaseOrderItem
        {
            get { return GetRefEntity(PurchaseOrderItemProperty); }
            set { SetRefEntity(PurchaseOrderItemProperty, value); }
        }
        #endregion

        #region 采购订单 PurchaseOrder
        /// <summary>
        /// 采购订单Id
        /// </summary>
        [Label("采购订单")]
        public static readonly IRefIdProperty PurchaseOrderIdProperty = P<FixtureReceiveDetail>.RegisterRefId(e => e.PurchaseOrderId, ReferenceType.Normal);

        /// <summary>
        /// 采购订单Id
        /// </summary>
        public double? PurchaseOrderId
        {
            get { return (double?)GetRefNullableId(PurchaseOrderIdProperty); }
            set { SetRefNullableId(PurchaseOrderIdProperty, value); }
        }

        /// <summary>
        /// 采购订单
        /// </summary>
        public static readonly RefEntityProperty<PurchaseOrder> PurchaseOrderProperty = P<FixtureReceiveDetail>.RegisterRef(e => e.PurchaseOrder, PurchaseOrderIdProperty);

        /// <summary>
        /// 采购订单
        /// </summary>
        public PurchaseOrder PurchaseOrder
        {
            get { return GetRefEntity(PurchaseOrderProperty); }
            set { SetRefEntity(PurchaseOrderProperty, value); }
        }
        #endregion

        #region 工治具编码 FixtureEncode
        /// <summary>
        /// 工治具编码Id
        /// </summary>
        [Label("工治具编码")]
        public static readonly IRefIdProperty FixtureEncodeIdProperty = P<FixtureReceiveDetail>.RegisterRefId(e => e.FixtureEncodeId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<FixtureEncode> FixtureEncodeProperty = P<FixtureReceiveDetail>.RegisterRef(e => e.FixtureEncode, FixtureEncodeIdProperty);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public FixtureEncode FixtureEncode
        {
            get { return GetRefEntity(FixtureEncodeProperty); }
            set { SetRefEntity(FixtureEncodeProperty, value); }
        }
        #endregion

        #region 工治具接收序列号明细列表 FixtureReceiveSnList
        /// <summary>
        /// 工治具接收序列号明细列表
        /// </summary>
        public static readonly ListProperty<EntityList<FixtureReceiveSn>> FixtureReceiveSnListProperty = P<FixtureReceiveDetail>.RegisterList(e => e.FixtureReceiveSnList);
        /// <summary>
        /// 工治具接收序列号明细列表
        /// </summary>
        public EntityList<FixtureReceiveSn> FixtureReceiveSnList
        {
            get { return this.GetLazyList(FixtureReceiveSnListProperty); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<FixtureReceiveDetail>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<FixtureReceiveDetail>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 工治具接收 FixtureReceive
        /// <summary>
        /// 工治具接收Id
        /// </summary>
        public static readonly IRefIdProperty FixtureReceiveIdProperty = P<FixtureReceiveDetail>.RegisterRefId(e => e.FixtureReceiveId, ReferenceType.Parent);

        /// <summary>
        /// 工治具接收Id
        /// </summary>
        public double FixtureReceiveId
        {
            get { return (double)GetRefId(FixtureReceiveIdProperty); }
            set { SetRefId(FixtureReceiveIdProperty, value); }
        }

        /// <summary>
        /// 工治具接收
        /// </summary>
        public static readonly RefEntityProperty<FixtureReceive> FixtureReceiveProperty = P<FixtureReceiveDetail>.RegisterRef(e => e.FixtureReceive, FixtureReceiveIdProperty);

        /// <summary>
        /// 工治具接收
        /// </summary>
        public FixtureReceive FixtureReceive
        {
            get { return GetRefEntity(FixtureReceiveProperty); }
            set { SetRefEntity(FixtureReceiveProperty, value); }
        }
        #endregion


        #region 视图属性

        #region 供应商名称 SupplierName	
        /// <summary>
        /// 供应商名称
        /// </summary>
        [Label("供应商名称")]
        public static readonly Property<string> SupplierNameProperty = P<FixtureReceiveDetail>.RegisterView(e => e.SupplierName, p => p.Supplier.Name);

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
        public static readonly Property<string> CustomerNameProperty = P<FixtureReceiveDetail>.RegisterView(e => e.CustomerName, p => p.Customer.Name);

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName
        {
            get { return this.GetProperty(CustomerNameProperty); }
        }
        #endregion

        #region 采购单号 PurOrderNo
        /// <summary>
        /// 采购单号
        /// </summary>
        [Label("采购单号")]
        public static readonly Property<string> PurOrderNoProperty = P<FixtureReceiveDetail>.RegisterView(e => e.PurOrderNo, p => p.PurchaseOrder.OrderNo);

        /// <summary>
        /// 采购单号
        /// </summary>
        public string PurOrderNo
        {
            get { return this.GetProperty(PurOrderNoProperty); }
        }
        #endregion

        #region 工厂id FactoryId
        /// <summary>
        /// 工厂id
        /// </summary>
        [Label("工厂id")]
        public static readonly Property<double> FactoryIdProperty = P<FixtureReceiveDetail>.RegisterView(e => e.FactoryId, p => p.FixtureReceive.FactoryId);

        /// <summary>
        /// 工厂id
        /// </summary>
        public double FactoryId
        {
            get { return this.GetProperty(FactoryIdProperty); }
        }
        #endregion

        #region 部门id DepartmentId
        /// <summary>
        /// 部门id
        /// </summary>
        [Label("部门id")]
        public static readonly Property<double> DepartmentIdProperty = P<FixtureReceiveDetail>.RegisterView(e => e.DepartmentId, p => p.FixtureReceive.DepartmentId);

        /// <summary>
        /// 部门id
        /// </summary>
        public double DepartmentId
        {
            get { return this.GetProperty(DepartmentIdProperty); }
        }
        #endregion

        #region 接收类型 ReceiveType
        /// <summary>
        /// 接收类型
        /// </summary>
        [Label("接收类型")]
        public static readonly Property<ReceiveType> ReceiveTypeProperty = P<FixtureReceiveDetail>.RegisterView(e => e.ReceiveType, p => p.FixtureReceive.ReceiveType);

        /// <summary>
        /// 接收类型
        /// </summary>
        public ReceiveType ReceiveType
        {
            get { return this.GetProperty(ReceiveTypeProperty); }
        }
        #endregion


        #region 型号编码 ModelCode
        /// <summary>
        /// 型号编码
        /// </summary>
        [Label("型号编码")]
        public static readonly Property<string> ModelCodeProperty = P<FixtureReceiveDetail>.RegisterView(e => e.ModelCode, p => p.FixtureEncode.FixtureModel.Code);

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
        public static readonly Property<string> ModelNameProperty = P<FixtureReceiveDetail>.RegisterView(e => e.ModelName, p => p.FixtureEncode.FixtureModel.Name);

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
        public static readonly Property<ManageMode> ManageModeProperty = P<FixtureReceiveDetail>.RegisterView(e => e.ManageMode, p => p.FixtureEncode.FixtureModel.ManageMode);

        /// <summary>
        /// 
        /// </summary>
        public ManageMode ManageMode
        {
            get { return this.GetProperty(ManageModeProperty); }
        }
        #endregion

        #region 免检 ExemptionInspect
        /// <summary>
        /// 免检
        /// </summary>
        [Label("免检")]
        public static readonly Property<bool> ExemptionInspectProperty = P<FixtureReceiveDetail>.RegisterView(e => e.ExemptionInspect, p => p.FixtureEncode.Exemption);

        /// <summary>
        /// 免检
        /// </summary>
        public bool ExemptionInspect
        {
            get { return this.GetProperty(ExemptionInspectProperty); }
        }
        #endregion

        #region 单价(不含税) PriceNoTax
        /// <summary>
        /// 单价(不含税)
        /// </summary>
        [Label("单价(不含税)")]
        public static readonly Property<decimal> PriceNoTaxProperty = P<FixtureReceiveDetail>.RegisterReadOnly(
            e => e.PriceNoTax, e => e.GetPriceNoTax(), PriceProperty);
        /// <summary>
        /// 单价(不含税)
        /// </summary>

        public decimal PriceNoTax
        {
            get { return this.GetProperty(PriceNoTaxProperty); }
        }
        private decimal GetPriceNoTax()
        {
            return Math.Round(Price * (1 - (TaxRate / 100)), 2);
        }
        #endregion


        #region 工治具编码 FixtureEncodeCode	
        /// <summary>
        /// 工治具编码
        /// </summary>
        [Label("工治具编码")]
        public static readonly Property<string> FixtureEncodeCodeProperty = P<FixtureReceiveDetail>.RegisterView(e => e.FixtureEncodeCode, p => p.FixtureEncode.Code);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public string FixtureEncodeCode
        {
            get { return this.GetProperty(FixtureEncodeCodeProperty); }
        }
        #endregion

        #region 采购订单号-行号 PuOrderLineNo
        /// <summary>
        /// 采购订单号
        /// </summary>
        [Label("采购订单")]
        public static readonly Property<string> PuOrderLineNoProperty = P<FixtureReceiveDetail>.RegisterReadOnly(
            e => e.PuOrderLineNo, e => e.GetPuOrderLineNo(), LineNoProperty, PurchaseOrderProperty);
        /// <summary>
        /// 采购订单号
        /// </summary>

        public string PuOrderLineNo
        {
            get { return this.GetProperty(PuOrderLineNoProperty); }
        }
        private string GetPuOrderLineNo()
        {
            if (PurchaseOrderId.HasValue)
            {
                return PurchaseOrder.OrderNo + "-" + LineNo;
            }
            return "";

        }
        #endregion

        #endregion


        #region 供应商编码 SupplierCode
        /// <summary>
        /// 供应商编码
        /// </summary>
        [Label("供应商编码")]
        public static readonly Property<string> SupplierCodeProperty = P<FixtureReceiveDetail>.RegisterView(e => e.SupplierCode, p => p.Supplier.Code);

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierCode
        {
            get { return this.GetProperty(SupplierCodeProperty); }
        }
        #endregion

        #region 仓库 WareHouseCode
        /// <summary>
        /// 仓库
        /// </summary>
        [Label("仓库")]
        public static readonly Property<string> WareHouseCodeProperty = P<FixtureReceiveDetail>.RegisterView(e => e.WareHouseCode, p => p.Warehouse.Code);

        /// <summary>
        /// 仓库
        /// </summary>
        public string WareHouseCode
        {
            get { return this.GetProperty(WareHouseCodeProperty); }
        }
        #endregion


        #region 客户编码 CustomerCode 
        /// <summary>
        /// 客户编码
        /// </summary>
        [Label("客户编码")]
        public static readonly Property<string> CustomerCodeProperty = P<FixtureReceiveDetail>.RegisterView(e => e.CustomerCode, p => p.Customer.Code);

        /// <summary>
        /// 客户编码
        /// </summary>
        public string CustomerCode
        {
            get { return this.GetProperty(CustomerCodeProperty); }
        }
        #endregion


        #region 单位 UnitName 
        /// <summary>
        /// 仓库
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<FixtureReceiveDetail>.RegisterView(e => e.UnitName, p => p.Unit.Name);

        /// <summary>
        /// 仓库
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
        }
        #endregion


        #region 工治具编码只读 FixtureEncodeIdReadOnly
        /// <summary>
        /// 工治具编码只读
        /// </summary>
        [Label("工治具编码只读")]
        public static readonly Property<bool> FixtureEncodeIdReadOnlyProperty = P<FixtureReceiveDetail>.Register(e => e.FixtureEncodeIdReadOnly);

        /// <summary>
        /// 工治具编码只读
        /// </summary>
        public bool FixtureEncodeIdReadOnly
        {
            get { return this.GetProperty(FixtureEncodeIdReadOnlyProperty); }
            set { this.SetProperty(FixtureEncodeIdReadOnlyProperty, value); }
        }
        #endregion



    }

    /// <summary>
    /// 工治具接收明细 实体配置
    /// </summary>
    internal class FixtureReceiveDetailConfig : EntityConfig<FixtureReceiveDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_FIXT_RECV_DTL").MapAllProperties();
            Meta.Property(FixtureReceiveDetail.RemarkProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}