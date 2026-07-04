using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.EMS.Purchases.Enums;
using SIE.EMS.Purchases.PurchaseOrders;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Enums;
using SIE.EMS.SpareParts.OutDepots.Details;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.EMS.Purchases.SparePartReceives
{
    /// <summary>
    /// 备件接收明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("备件接收明细")]
    [DisplayMember(nameof(LineNo))]
    public partial class SparePartReceiveDetail : DataEntity
    {
        #region 备件接收 SparePartReceive
        /// <summary>
        /// 备件接收Id
        /// </summary>
        public static readonly IRefIdProperty SparePartReceiveIdProperty = P<SparePartReceiveDetail>.RegisterRefId(e => e.SparePartReceiveId, ReferenceType.Parent);

        /// <summary>
        /// 备件接收Id
        /// </summary>
        public double SparePartReceiveId
        {
            get { return (double)GetRefId(SparePartReceiveIdProperty); }
            set { SetRefId(SparePartReceiveIdProperty, value); }
        }

        /// <summary>
        /// 备件接收
        /// </summary>
        public static readonly RefEntityProperty<SparePartReceive> SparePartReceiveProperty = P<SparePartReceiveDetail>.RegisterRef(e => e.SparePartReceive, SparePartReceiveIdProperty);

        /// <summary>
        /// 备件接收
        /// </summary>
        public SparePartReceive SparePartReceive
        {
            get { return GetRefEntity(SparePartReceiveProperty); }
            set { SetRefEntity(SparePartReceiveProperty, value); }
        }
        #endregion

        #region 行号 LineNo
        /// <summary>
        /// 行号
        /// </summary>
        [Label("行号")]
        public static readonly Property<int> LineNoProperty = P<SparePartReceiveDetail>.Register(e => e.LineNo);

        /// <summary>
        /// 行号
        /// </summary>
        public int LineNo
        {
            get { return GetProperty(LineNoProperty); }
            set { SetProperty(LineNoProperty, value); }
        }
        #endregion

        #region 单价（含税） Price
        /// <summary>
        /// 单价（含税）
        /// </summary>
        [Label("单价（含税）")]
        [MinValue(0)]
        [Required]
        public static readonly Property<decimal> PriceProperty = P<SparePartReceiveDetail>.Register(e => e.Price);

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
        public static readonly Property<decimal> TaxRateProperty = P<SparePartReceiveDetail>.Register(e => e.TaxRate);

        /// <summary>
        /// 税率(%)
        /// </summary>
        public decimal TaxRate
        {
            get { return GetProperty(TaxRateProperty); }
            set { SetProperty(TaxRateProperty, value); }
        }
        #endregion

        #region 接收数量 Qty
        /// <summary>
        /// 接收数量
        /// </summary>
        [Label("接收数量")]
        [MinValue(1)]
        public static readonly Property<int> QtyProperty = P<SparePartReceiveDetail>.Register(e => e.Qty);

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
        [MinValue(0)]
        public static readonly Property<int> RecivedQtyProperty = P<SparePartReceiveDetail>.Register(e => e.RecivedQty);

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
        public static readonly Property<string> RemarkProperty = P<SparePartReceiveDetail>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 供应商 Supplier
        /// <summary>
        /// 供应商Id
        /// </summary>
        [Label("供应商")]
        public static readonly IRefIdProperty SupplierIdProperty = P<SparePartReceiveDetail>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Supplier> SupplierProperty = P<SparePartReceiveDetail>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier Supplier
        {
            get { return GetRefEntity(SupplierProperty); }
            set { SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 采购单号 PurchaseOrder
        /// <summary>
        /// 采购单号Id
        /// </summary>
        [Label("采购单号")]
        public static readonly IRefIdProperty PurchaseOrderIdProperty = P<SparePartReceiveDetail>.RegisterRefId(e => e.PurchaseOrderId, ReferenceType.Normal);

        /// <summary>
        /// 采购单号Id
        /// </summary>
        public double? PurchaseOrderId
        {
            get { return (double?)GetRefNullableId(PurchaseOrderIdProperty); }
            set { SetRefNullableId(PurchaseOrderIdProperty, value); }
        }

        /// <summary>
        /// 采购单号
        /// </summary>
        public static readonly RefEntityProperty<PurchaseOrder> PurchaseOrderProperty = P<SparePartReceiveDetail>.RegisterRef(e => e.PurchaseOrder, PurchaseOrderIdProperty);

        /// <summary>
        /// 采购单号
        /// </summary>
        public PurchaseOrder PurchaseOrder
        {
            get { return GetRefEntity(PurchaseOrderProperty); }
            set { SetRefEntity(PurchaseOrderProperty, value); }
        }
        #endregion

        #region 采购单行号 PurchaseOrderItem
        /// <summary>
        /// 采购单行号Id
        /// </summary>
        [Label("采购单行号")]
        public static readonly IRefIdProperty PurchaseOrderItemIdProperty = P<SparePartReceiveDetail>.RegisterRefId(e => e.PurchaseOrderItemId, ReferenceType.Normal);

        /// <summary>
        /// 采购单行号Id
        /// </summary>
        public double? PurchaseOrderItemId
        {
            get { return (double?)GetRefNullableId(PurchaseOrderItemIdProperty); }
            set { SetRefNullableId(PurchaseOrderItemIdProperty, value); }
        }

        /// <summary>
        /// 采购单行号
        /// </summary>
        public static readonly RefEntityProperty<PurchaseOrderItem> PurchaseOrderItemProperty = P<SparePartReceiveDetail>.RegisterRef(e => e.PurchaseOrderItem, PurchaseOrderItemIdProperty);

        /// <summary>
        /// 采购单行号
        /// </summary>
        public PurchaseOrderItem PurchaseOrderItem
        {
            get { return GetRefEntity(PurchaseOrderItemProperty); }
            set { SetRefEntity(PurchaseOrderItemProperty, value); }
        }
        #endregion

        #region 备件基础数据 SparePart
        /// <summary>
        /// 备件基础数据Id
        /// </summary>
        [Label("备件基础数据")]
        public static readonly IRefIdProperty SparePartIdProperty = P<SparePartReceiveDetail>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<SparePart> SparePartProperty = P<SparePartReceiveDetail>.RegisterRef(e => e.SparePart, SparePartIdProperty);

        /// <summary>
        /// 备件基础数据
        /// </summary>
        public SparePart SparePart
        {
            get { return GetRefEntity(SparePartProperty); }
            set { SetRefEntity(SparePartProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<SparePartReceiveDetail>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<SparePartReceiveDetail>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 备件出库单行 PartOutDepotDetail
        /// <summary>
        /// 备件出库单行Id
        /// </summary>
        [Label("备件出库单行")]
        public static readonly IRefIdProperty PartOutDepotDetailIdProperty =
            P<SparePartReceiveDetail>.RegisterRefId(e => e.PartOutDepotDetailId, ReferenceType.Normal);

        /// <summary>
        /// 备件出库单行Id
        /// </summary>
        public double? PartOutDepotDetailId
        {
            get { return (double?)this.GetRefNullableId(PartOutDepotDetailIdProperty); }
            set { this.SetRefNullableId(PartOutDepotDetailIdProperty, value); }
        }

        /// <summary>
        /// 备件出库单行
        /// </summary>
        public static readonly RefEntityProperty<PartOutDepotDetail> PartOutDepotDetailProperty =
            P<SparePartReceiveDetail>.RegisterRef(e => e.PartOutDepotDetail, PartOutDepotDetailIdProperty);

        /// <summary>
        /// 备件出库单行
        /// </summary>
        public PartOutDepotDetail PartOutDepotDetail
        {
            get { return this.GetRefEntity(PartOutDepotDetailProperty); }
            set { this.SetRefEntity(PartOutDepotDetailProperty, value); }
        }
        #endregion

        #region 备件出库单-行号 OutDepotLineNo
        /// <summary>
        /// 备件出库单-行号
        /// </summary>
        [Label("备件出库单-行号")]
        public static readonly Property<string> OutDepotLineNoProperty = P<SparePartReceiveDetail>.Register(e => e.OutDepotLineNo);

        /// <summary>
        /// 备件出库单-行号
        /// </summary>
        public string OutDepotLineNo
        {
            get { return this.GetProperty(OutDepotLineNoProperty); }
            set { this.SetProperty(OutDepotLineNoProperty, value); }
        }
        #endregion

        #region 批次明细列表 LotList
        /// <summary>
        /// 批次明细列表
        /// </summary>
        public static readonly ListProperty<EntityList<SparePartReceiveLot>> LotListProperty = P<SparePartReceiveDetail>.RegisterList(e => e.LotList);
        /// <summary>
        /// 批次明细列表
        /// </summary>
        public EntityList<SparePartReceiveLot> LotList
        {
            get { return this.GetLazyList(LotListProperty); }
        }
        #endregion

        #region 序列号明细列表 SnList
        /// <summary>
        /// 序列号明细列表
        /// </summary>
        public static readonly ListProperty<EntityList<SparePartReceiveSn>> SnListProperty = P<SparePartReceiveDetail>.RegisterList(e => e.SnList);
        /// <summary>
        /// 序列号明细列表
        /// </summary>
        public EntityList<SparePartReceiveSn> SnList
        {
            get { return this.GetLazyList(SnListProperty); }
        }
        #endregion

        #region 视图属性
        #region 单价(不含税) PriceNoTax
        /// <summary>
        /// 单价(不含税)
        /// </summary>
        [Label("单价(不含税)")]
        public static readonly Property<decimal> PriceNoTaxProperty = P<SparePartReceiveDetail>.RegisterReadOnly(
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

        #region 供应商编码 SupplierCode
        /// <summary>
        /// 供应商编码
        /// </summary>
        [Label("供应商编码")]
        public static readonly Property<string> SupplierCodeProperty = P<SparePartReceiveDetail>.RegisterView(e => e.SupplierCode, p => p.Supplier.Code);

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierCode
        {
            get { return this.GetProperty(SupplierCodeProperty); }
        }
        #endregion

        #region 供应商名称 SupplierName
        /// <summary>
        /// 供应商名称
        /// </summary>
        [Label("供应商名称")]
        public static readonly Property<string> SupplierNameProperty = P<SparePartReceiveDetail>.RegisterView(e => e.SupplierName, p => p.Supplier.Name);

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName
        {
            get { return this.GetProperty(SupplierNameProperty); }
        }
        #endregion

        #region 采购单号 PurchaseOrderNo
        /// <summary>
        /// 采购单号
        /// </summary>
        [Label("采购单号")]
        public static readonly Property<string> PurchaseOrderNoProperty = P<SparePartReceiveDetail>.RegisterView(e => e.PurchaseOrderNo, p => p.PurchaseOrder.OrderNo);

        /// <summary>
        /// 采购单号
        /// </summary>
        public string PurchaseOrderNo
        {
            get { return this.GetProperty(PurchaseOrderNoProperty); }
        }
        #endregion

        #region 采购单行号 PurchaseOrderLine
        /// <summary>
        /// 采购单行号
        /// </summary>
        [Label("采购单行号")]
        public static readonly Property<string> PurchaseOrderLineProperty = P<SparePartReceiveDetail>.RegisterView(e => e.PurchaseOrderLine, p => p.PurchaseOrderItem.LineNo);

        /// <summary>
        /// 采购单行号
        /// </summary>
        public string PurchaseOrderLine
        {
            get { return this.GetProperty(PurchaseOrderLineProperty); }
        }
        #endregion

        #region 采购对象 PurchaseObjectType
        /// <summary>
        /// 采购对象
        /// </summary>
        [Label("采购对象")]
        public static readonly Property<PurchaseObjectType?> PurchaseObjectTypeProperty = P<SparePartReceiveDetail>.RegisterView(e => e.PurchaseObjectType, p => p.PurchaseOrder.PurchaseObjectType);

        /// <summary>
        /// 采购对象
        /// </summary>
        public PurchaseObjectType? PurchaseObjectType
        {
            get { return this.GetProperty(PurchaseObjectTypeProperty); }
        }
        #endregion

        #region 备件编码 SparePartCode
        /// <summary>
        /// 备件编码
        /// </summary>
        [Label("备件编码")]
        public static readonly Property<string> SparePartCodeProperty = P<SparePartReceiveDetail>.RegisterView(e => e.SparePartCode, p => p.SparePart.SparePartCode);

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
        public static readonly Property<string> SparePartNameProperty = P<SparePartReceiveDetail>.RegisterView(e => e.SparePartName, p => p.SparePart.SparePartName);

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName
        {
            get { return this.GetProperty(SparePartNameProperty); }
        }
        #endregion

        #region 管控方式 ControlMethod
        /// <summary>
        /// 管控方式
        /// </summary>
        [Label("管控方式")]
        public static readonly Property<ControlMethod> ControlMethodProperty = P<SparePartReceiveDetail>.RegisterView(e => e.ControlMethod, p => p.SparePart.ControlMethod);

        /// <summary>
        /// 管控方式
        /// </summary>
        public ControlMethod ControlMethod
        {
            get { return this.GetProperty(ControlMethodProperty); }
        }
        #endregion

        #region 免检 ExemptionInspect
        /// <summary>
        /// 免检
        /// </summary>
        [Label("免检")]
        public static readonly Property<bool> ExemptionInspectProperty = P<SparePartReceiveDetail>.RegisterView(e => e.ExemptionInspect, p => p.SparePart.ExemptionInspect);

        /// <summary>
        /// 免检
        /// </summary>
        public bool ExemptionInspect
        {
            get { return this.GetProperty(ExemptionInspectProperty); }
        }
        #endregion


        #region 接收状态 ReceiveBillStatus
        /// <summary>
        /// 接收状态
        /// </summary>
        [Label("接收状态")]
        public static readonly Property<ReceiveBillStatus> ReceiveBillStatusProperty = P<SparePartReceiveDetail>.RegisterView(e => e.ReceiveBillStatus, p => p.SparePartReceive.ReceiveBillStatus);

        /// <summary>
        /// 接收状态
        /// </summary>
        public ReceiveBillStatus ReceiveBillStatus
        {
            get { return this.GetProperty(ReceiveBillStatusProperty); }
        }
        #endregion


        #region 仓库名称 WarehouseName
        /// <summary>
        /// 仓库名称
        /// </summary>
        [Label("仓库名称")]
        public static readonly Property<string> WarehouseNameProperty = P<SparePartReceiveDetail>.RegisterView(e => e.WarehouseName, p => p.Warehouse.Name);

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName
        {
            get { return this.GetProperty(WarehouseNameProperty); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<SparePartReceiveDetail>.RegisterView(e => e.UnitName, p => p.SparePart.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
        }
        #endregion

        #region 工厂id FactoryId
        /// <summary>
        /// 工厂id
        /// </summary>
        [Label("工厂id")]
        public static readonly Property<double> FactoryIdProperty = P<SparePartReceiveDetail>.RegisterView(e => e.FactoryId, p => p.SparePartReceive.FactoryId);

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
        public static readonly Property<double> DepartmentIdProperty = P<SparePartReceiveDetail>.RegisterView(e => e.DepartmentId, p => p.SparePartReceive.DepartmentId);

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
        public static readonly Property<ReceiveType> ReceiveTypeProperty = P<SparePartReceiveDetail>.RegisterView(e => e.ReceiveType, p => p.SparePartReceive.ReceiveType);

        /// <summary>
        /// 接收类型
        /// </summary>
        public ReceiveType ReceiveType
        {
            get { return this.GetProperty(ReceiveTypeProperty); }
        }
        #endregion

        #region 批次号(界面属性导入用) LotNo
        /// <summary>
        /// 批次号(界面属性导入用)
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> LotNoProperty = P<SparePartReceiveDetail>.Register(e => e.LotNo);

        /// <summary>
        /// 批次号(界面属性导入用)
        /// </summary>
        public string LotNo
        {
            get { return GetProperty(LotNoProperty); }
            set { SetProperty(LotNoProperty, value); }
        }
        #endregion

        #region 序列号(界面属性导入用) Sn
        /// <summary>
        /// 序列号(界面属性导入用)
        /// </summary>
        [Label("序列号")]
        public static readonly Property<string> SnProperty = P<SparePartReceiveDetail>.Register(e => e.Sn);

        /// <summary>
        /// 序列号(界面属性导入用)
        /// </summary>
        public string Sn
        {
            get { return GetProperty(SnProperty); }
            set { SetProperty(SnProperty, value); }
        }
        #endregion

        #region 原厂序列号(界面属性导入用) OriginalSn
        /// <summary>
        /// 原厂序列号
        /// </summary>
        [Label("原厂序列号")]
        public static readonly Property<string> OriginalSnProperty = P<SparePartReceiveDetail>.Register(e => e.OriginalSn);

        /// <summary>
        /// 原厂序列号
        /// </summary>
        public string OriginalSn
        {
            get { return this.GetProperty(OriginalSnProperty); }
            set { this.SetProperty(OriginalSnProperty, value); }
        }
        #endregion

        #region 数量(界面属性导入用) ImportQty
        /// <summary>
        /// 数量(界面属性导入用)
        /// </summary>
        [Label("数量")]
        public static readonly Property<int?> ImportQtyProperty = P<SparePartReceiveDetail>.Register(e => e.ImportQty);

        /// <summary>
        /// 数量(界面属性导入用)
        /// </summary>
        public int? ImportQty
        {
            get { return this.GetProperty(ImportQtyProperty); }
            set { this.SetProperty(ImportQtyProperty, value); }
        }
        #endregion

        #region 备件出库单 OutDepotNo
        /// <summary>
        /// 备件出库单
        /// </summary>
        [Label("备件出库单")]
        public static readonly Property<string> OutDepotNoProperty = P<SparePartReceiveDetail>.RegisterView(e => e.OutDepotNo, p => p.PartOutDepotDetail.OutDepot.No);

        /// <summary>
        /// 备件出库单
        /// </summary>
        public string OutDepotNo
        {
            get { return this.GetProperty(OutDepotNoProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 备件接收明细 实体配置
    /// </summary>
    internal class SparePartReceiveDetailConfig : EntityConfig<SparePartReceiveDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_SP_RECV_DTL").MapAllProperties();
            Meta.Property(SparePartReceiveDetail.LotNoProperty).DontMapColumn();
            Meta.Property(SparePartReceiveDetail.SnProperty).DontMapColumn();
            Meta.Property(SparePartReceiveDetail.OriginalSnProperty).DontMapColumn();
            Meta.Property(SparePartReceiveDetail.ImportQtyProperty).DontMapColumn();
            Meta.Property(SparePartReceiveDetail.RemarkProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}