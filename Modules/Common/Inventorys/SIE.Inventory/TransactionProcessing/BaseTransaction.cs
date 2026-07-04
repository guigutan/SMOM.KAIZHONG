using SIE.Core.Enums;
using SIE.Domain;
using SIE.Inventory.Commom;
using SIE.Inventory.Onhands;
using SIE.Inventory.Transactions;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.Inventory.TransactionProcessing
{
    /// <summary>
    /// 库存交易
    /// </summary>
    [RootEntity, Serializable]
    [Label("库存交易")]
    public partial class BaseTransaction : DataEntity
    {
        #region 货主 StorerCode
        /// <summary>
        /// 货主
        /// </summary>
        [Label("货主")]
        public static readonly Property<string> StorerCodeProperty = P<BaseTransaction>.Register(e => e.StorerCode);

        /// <summary>
        /// 货主
        /// </summary>
        public string StorerCode
        {
            get { return GetProperty(StorerCodeProperty); }
            set { SetProperty(StorerCodeProperty, value); }
        }
        #endregion

        #region 批次 LotCode
        /// <summary>
        /// 批次
        /// </summary>
        [Label("批次")]
        public static readonly Property<string> LotCodeProperty = P<BaseTransaction>.Register(e => e.LotCode);

        /// <summary>
        /// 批次
        /// </summary>
        public string LotCode
        {
            get { return GetProperty(LotCodeProperty); }
            set { SetProperty(LotCodeProperty, value); }
        }
        #endregion

        #region 批次 Lot
        /// <summary>
        /// 批次
        /// </summary>
        [Label("批次")]
        public static readonly IRefIdProperty LotIdProperty =
            P<BaseTransaction>.RegisterRefId(e => e.LotId, ReferenceType.Normal);

        /// <summary>
        /// 批次
        /// </summary>
        public double LotId
        {
            get { return (double)this.GetRefId(LotIdProperty); }
            set { this.SetRefId(LotIdProperty, value); }
        }

        /// <summary>
        /// 批次
        /// </summary>
        public static readonly RefEntityProperty<Lot> LotProperty =
            P<BaseTransaction>.RegisterRef(e => e.Lot, LotIdProperty);

        /// <summary>
        /// 批次
        /// </summary>
        public Lot Lot
        {
            get { return this.GetRefEntity(LotProperty); }
            set { this.SetRefEntity(LotProperty, value); }
        }
        #endregion

        #region 自LPN FromLpn
        /// <summary>
        /// 自LPN
        /// </summary>
        [Label("自LPN")]
        public static readonly Property<string> FromLpnProperty = P<BaseTransaction>.Register(e => e.FromLpn);

        /// <summary>
        /// 自LPN
        /// </summary>
        public string FromLpn
        {
            get { return GetProperty(FromLpnProperty); }
            set { SetProperty(FromLpnProperty, value); }
        }
        #endregion

        #region 自库存状态 FromOnhandSate
        /// <summary>
        /// 自库存状态
        /// </summary>
        [Label("自库存状态")]
        public static readonly Property<OnhandState?> FromOnhandSateProperty = P<BaseTransaction>.Register(e => e.FromOnhandSate);

        /// <summary>
        /// 自库存状态
        /// </summary>
        public OnhandState? FromOnhandSate
        {
            get { return this.GetProperty(FromOnhandSateProperty); }
            set { this.SetProperty(FromOnhandSateProperty, value); }
        }
        #endregion

        #region 至LPN ToLpn
        /// <summary>
        /// 至LPN
        /// </summary>
        [Label("至LPN")]
        public static readonly Property<string> ToLpnProperty = P<BaseTransaction>.Register(e => e.ToLpn);

        /// <summary>
        /// 至LPN
        /// </summary>
        public string ToLpn
        {
            get { return GetProperty(ToLpnProperty); }
            set { SetProperty(ToLpnProperty, value); }
        }
        #endregion

        #region 至库存状态 ToOnhandState
        /// <summary>
        /// 至库存状态
        /// </summary>
        [Label("至库存状态")]
        public static readonly Property<OnhandState?> ToOnhandStateProperty = P<BaseTransaction>.Register(e => e.ToOnhandState);

        /// <summary>
        /// 至库存状态
        /// </summary>
        public OnhandState? ToOnhandState
        {
            get { return this.GetProperty(ToOnhandStateProperty); }
            set { this.SetProperty(ToOnhandStateProperty, value); }
        }
        #endregion

        #region 数量(主) Qty
        /// <summary>
        /// 数量(主)
        /// </summary>
        [Label("数量(主)")]
        public static readonly Property<decimal> QtyProperty = P<BaseTransaction>.Register(e => e.Qty);

        /// <summary>
        /// 数量(主)
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 交易日期 TransactionDate
        /// <summary>
        /// 交易日期
        /// </summary>
        [Label("交易日期")]
        public static readonly Property<DateTime> TransactionDateProperty = P<BaseTransaction>.Register(e => e.TransactionDate);

        /// <summary>
        /// 交易日期
        /// </summary>
        public DateTime TransactionDate
        {
            get { return GetProperty(TransactionDateProperty); }
            set { SetProperty(TransactionDateProperty, value); }
        }
        #endregion

        #region 上传标识 UploadFlag
        /// <summary>
        /// 上传标识
        /// </summary>
        [Label("上传标识")]
        public static readonly Property<bool> UploadFlagProperty = P<BaseTransaction>.Register(e => e.UploadFlag);

        /// <summary>
        /// 上传标识
        /// </summary>
        public bool UploadFlag
        {
            get { return GetProperty(UploadFlagProperty); }
            set { SetProperty(UploadFlagProperty, value); }
        }
        #endregion

        #region 项目号 ProjectNo
        /// <summary>
        /// 项目号
        /// </summary>
        [Label("项目号")]
        public static readonly Property<string> ProjectNoProperty = P<BaseTransaction>.Register(e => e.ProjectNo);

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectNo
        {
            get { return GetProperty(ProjectNoProperty); }
            set { SetProperty(ProjectNoProperty, value); }
        }
        #endregion

        #region 任务号 TaskNo
        /// <summary>
        /// 任务号
        /// </summary>
        [Label("任务号")]
        public static readonly Property<string> TaskNoProperty = P<BaseTransaction>.Register(e => e.TaskNo);

        /// <summary>
        /// 任务号
        /// </summary>
        public string TaskNo
        {
            get { return GetProperty(TaskNoProperty); }
            set { SetProperty(TaskNoProperty, value); }
        }
        #endregion

        #region 相关订单号 OrderNo
        /// <summary>
        /// 相关订单号
        /// </summary>
        [Label("相关订单号")]
        public static readonly Property<string> OrderNoProperty = P<BaseTransaction>.Register(e => e.OrderNo);

        /// <summary>
        /// 相关订单号
        /// </summary>
        public string OrderNo
        {
            get { return this.GetProperty(OrderNoProperty); }
            set { this.SetProperty(OrderNoProperty, value); }
        }
        #endregion

        #region 相关订单行号 OrderLineNo
        /// <summary>
        /// 相关订单行号
        /// </summary>
        [Label("相关订单行号")]
        public static readonly Property<string> OrderLineNoProperty = P<BaseTransaction>.Register(e => e.OrderLineNo);

        /// <summary>
        /// 相关订单行号
        /// </summary>
        public string OrderLineNo
        {
            get { return this.GetProperty(OrderLineNoProperty); }
            set { this.SetProperty(OrderLineNoProperty, value); }
        }
        #endregion

        #region 单据号 BillNo
        /// <summary>
        /// 单据号
        /// </summary>
        [Label("单据号")]
        public static readonly Property<string> BillNoProperty = P<BaseTransaction>.Register(e => e.BillNo);

        /// <summary>
        /// 单据号
        /// </summary>
        public string BillNo
        {
            get { return GetProperty(BillNoProperty); }
            set { SetProperty(BillNoProperty, value); }
        }
        #endregion

        #region 单据ID BillId
        /// <summary>
        /// 单据ID
        /// </summary>
        [Label("单据ID")]
        public static readonly Property<double> BillIdProperty = P<BaseTransaction>.Register(e => e.BillId);

        /// <summary>
        /// 单据ID
        /// </summary>
        public double BillId
        {
            get { return GetProperty(BillIdProperty); }
            set { SetProperty(BillIdProperty, value); }
        }
        #endregion

        #region 单据明细号 BillDtlNo
        /// <summary>
        /// 单据明细号
        /// </summary>
        [Label("单据明细号")]
        public static readonly Property<string> BillDtlNoProperty = P<BaseTransaction>.Register(e => e.BillDtlNo);

        /// <summary>
        /// 单据明细号
        /// </summary>
        public string BillDtlNo
        {
            get { return GetProperty(BillDtlNoProperty); }
            set { SetProperty(BillDtlNoProperty, value); }
        }
        #endregion

        #region 单据明细ID BillDtlId
        /// <summary>
        /// 单据明细ID
        /// </summary>
        [Label("单据明细ID")]
        public static readonly Property<double> BillDtlIdProperty = P<BaseTransaction>.Register(e => e.BillDtlId);

        /// <summary>
        /// 单据明细ID
        /// </summary>
        public double BillDtlId
        {
            get { return GetProperty(BillDtlIdProperty); }
            set { SetProperty(BillDtlIdProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(2000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<BaseTransaction>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 来源单号（ERP） SourceBillNo
        /// <summary>
        /// 来源单号（ERP）
        /// </summary>
        [Label("来源单号（ERP）")]
        public static readonly Property<string> SourceBillNoProperty = P<BaseTransaction>.Register(e => e.SourceBillNo);

        /// <summary>
        /// 来源单号（ERP）
        /// </summary>
        public string SourceBillNo
        {
            get { return GetProperty(SourceBillNoProperty); }
            set { SetProperty(SourceBillNoProperty, value); }
        }
        #endregion

        #region 来源单号ID（ERP） SourceBillId
        /// <summary>
        /// 来源单号ID（ERP）
        /// </summary>
        [Label("来源单号ID（ERP）")]
        public static readonly Property<string> SourceBillIdProperty = P<BaseTransaction>.Register(e => e.SourceBillId);

        /// <summary>
        /// 来源单号ID（ERP）
        /// </summary>
        public string SourceBillId
        {
            get { return GetProperty(SourceBillIdProperty); }
            set { SetProperty(SourceBillIdProperty, value); }
        }
        #endregion

        #region 来源单行（ERP） SourceBillDtlNo
        /// <summary>
        /// 来源单行（ERP）
        /// </summary>
        [Label("来源单行（ERP）")]
        public static readonly Property<string> SourceBillDtlNoProperty = P<BaseTransaction>.Register(e => e.SourceBillDtlNo);

        /// <summary>
        /// 来源单行（ERP）
        /// </summary>
        public string SourceBillDtlNo
        {
            get { return GetProperty(SourceBillDtlNoProperty); }
            set { SetProperty(SourceBillDtlNoProperty, value); }
        }
        #endregion

        #region 来源单行ID（ERP，SAP） SourceBillDtlId
        /// <summary>
        /// 来源单行ID（ERP，SAP）
        /// </summary>
        [Label("来源单行ID（ERP，SAP）")]
        public static readonly Property<string> SourceBillDtlIdProperty = P<BaseTransaction>.Register(e => e.SourceBillDtlId);

        /// <summary>
        /// 来源单行ID（ERP，SAP）
        /// </summary>
        public string SourceBillDtlId
        {
            get { return GetProperty(SourceBillDtlIdProperty); }
            set { SetProperty(SourceBillDtlIdProperty, value); }
        }
        #endregion

        #region 员工工号 EmployeeNo
        /// <summary>
        /// 员工工号
        /// </summary>
        [Label("员工工号")]
        public static readonly Property<string> EmployeeNoProperty = P<BaseTransaction>.Register(e => e.EmployeeNo);

        /// <summary>
        /// 员工工号
        /// </summary>
        public string EmployeeNo
        {
            get { return GetProperty(EmployeeNoProperty); }
            set { SetProperty(EmployeeNoProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<BaseTransaction>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return GetProperty(ItemCodeProperty); }
            set { SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [MaxLength(240)]
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<BaseTransaction>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return GetProperty(ItemNameProperty); }
            set { SetProperty(ItemNameProperty, value); }
        }
        #endregion
        
        #region 单位 Unit
        /// <summary>
        /// 单位Id
        /// </summary>
        public static readonly IRefIdProperty UnitIdProperty = P<BaseTransaction>.RegisterRefId(e => e.UnitId, ReferenceType.Normal);

        /// <summary>
        /// 单位Id
        /// </summary>
        public double UnitId
        {
            get { return (double)GetRefId(UnitIdProperty); }
            set { SetRefId(UnitIdProperty, value); }
        }

        /// <summary>
        /// 单位
        /// </summary>
        public static readonly RefEntityProperty<Unit> UnitProperty = P<BaseTransaction>.RegisterRef(e => e.Unit, UnitIdProperty);

        /// <summary>
        /// 单位
        /// </summary>
        public Unit Unit
        {
            get { return GetRefEntity(UnitProperty); }
            set { SetRefEntity(UnitProperty, value); }
        }
        #endregion

        #region 自库位 FromLocation
        /// <summary>
        /// 自库位Id
        /// </summary>
        [Label("自库位")]
        public static readonly IRefIdProperty FromLocationIdProperty = P<BaseTransaction>.RegisterRefId(e => e.FromLocationId, ReferenceType.Normal);

        /// <summary>
        /// 自库位Id
        /// </summary>
        public double? FromLocationId
        {
            get { return (double?)GetRefNullableId(FromLocationIdProperty); }
            set { SetRefNullableId(FromLocationIdProperty, value); }
        }

        /// <summary>
        /// 自库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> FromLocationProperty = P<BaseTransaction>.RegisterRef(e => e.FromLocation, FromLocationIdProperty);

        /// <summary>
        /// 自库位
        /// </summary>
        public StorageLocation FromLocation
        {
            get { return GetRefEntity(FromLocationProperty); }
            set { SetRefEntity(FromLocationProperty, value); }
        }
        #endregion

        #region 自仓库 FromWarehouse
        /// <summary>
        /// 自仓库Id
        /// </summary>
        [Label("自仓库")]
        public static readonly IRefIdProperty FromWarehouseIdProperty = P<BaseTransaction>.RegisterRefId(e => e.FromWarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 自仓库Id
        /// </summary>
        public double? FromWarehouseId
        {
            get { return (double?)GetRefNullableId(FromWarehouseIdProperty); }
            set { SetRefNullableId(FromWarehouseIdProperty, value); }
        }

        /// <summary>
        /// 自仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> FromWarehouseProperty = P<BaseTransaction>.RegisterRef(e => e.FromWarehouse, FromWarehouseIdProperty);

        /// <summary>
        /// 自仓库
        /// </summary>
        public Warehouse FromWarehouse
        {
            get { return GetRefEntity(FromWarehouseProperty); }
            set { SetRefEntity(FromWarehouseProperty, value); }
        }
        #endregion

        #region 自库区 FromArea
        /// <summary>
        /// 自库区Id
        /// </summary>
        [Label("自库区")]
        public static readonly IRefIdProperty FromAreaIdProperty =
            P<BaseTransaction>.RegisterRefId(e => e.FromAreaId, ReferenceType.Normal);

        /// <summary>
        /// 自库区Id
        /// </summary>
        public double? FromAreaId
        {
            get { return (double?)this.GetRefNullableId(FromAreaIdProperty); }
            set { this.SetRefNullableId(FromAreaIdProperty, value); }
        }

        /// <summary>
        /// 自库区
        /// </summary>
        public static readonly RefEntityProperty<StorageArea> FromAreaProperty =
            P<BaseTransaction>.RegisterRef(e => e.FromArea, FromAreaIdProperty);

        /// <summary>
        /// 自库区
        /// </summary>
        public StorageArea FromArea
        {
            get { return this.GetRefEntity(FromAreaProperty); }
            set { this.SetRefEntity(FromAreaProperty, value); }
        }
        #endregion        

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        public static readonly IRefIdProperty ItemIdProperty = P<BaseTransaction>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return (double)GetRefId(ItemIdProperty); }
            set { SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<BaseTransaction>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 至仓库 ToWarehouse
        /// <summary>
        /// 至仓库Id
        /// </summary>
        [Label("至仓库")]
        public static readonly IRefIdProperty ToWarehouseIdProperty = P<BaseTransaction>.RegisterRefId(e => e.ToWarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 至仓库Id
        /// </summary>
        public double? ToWarehouseId
        {
            get { return (double?)GetRefNullableId(ToWarehouseIdProperty); }
            set { SetRefNullableId(ToWarehouseIdProperty, value); }
        }

        /// <summary>
        /// 至仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> ToWarehouseProperty = P<BaseTransaction>.RegisterRef(e => e.ToWarehouse, ToWarehouseIdProperty);

        /// <summary>
        /// 至仓库
        /// </summary>
        public Warehouse ToWarehouse
        {
            get { return GetRefEntity(ToWarehouseProperty); }
            set { SetRefEntity(ToWarehouseProperty, value); }
        }
        #endregion

        #region 至库位 ToLocation
        /// <summary>
        /// 至库位Id
        /// </summary>
        [Label("至库位")]
        public static readonly IRefIdProperty ToLocationIdProperty = P<BaseTransaction>.RegisterRefId(e => e.ToLocationId, ReferenceType.Normal);

        /// <summary>
        /// 至库位Id
        /// </summary>
        public double? ToLocationId
        {
            get { return (double?)GetRefNullableId(ToLocationIdProperty); }
            set { SetRefNullableId(ToLocationIdProperty, value); }
        }

        /// <summary>
        /// 至库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> ToLocationProperty = P<BaseTransaction>.RegisterRef(e => e.ToLocation, ToLocationIdProperty);

        /// <summary>
        /// 至库位
        /// </summary>
        public StorageLocation ToLocation
        {
            get { return GetRefEntity(ToLocationProperty); }
            set { SetRefEntity(ToLocationProperty, value); }
        }
        #endregion

        #region 至库区 ToArea
        /// <summary>
        /// 至库区
        /// </summary>
        [Label("至库区")]
        public static readonly IRefIdProperty ToAreaIdProperty =
            P<BaseTransaction>.RegisterRefId(e => e.ToAreaId, ReferenceType.Normal);

        /// <summary>
        /// 至库区Id
        /// </summary>
        public double? ToAreaId
        {
            get { return (double?)this.GetRefNullableId(ToAreaIdProperty); }
            set { this.SetRefNullableId(ToAreaIdProperty, value); }
        }

        /// <summary>
        /// 至库区
        /// </summary>
        public static readonly RefEntityProperty<StorageArea> ToAreaProperty =
            P<BaseTransaction>.RegisterRef(e => e.ToArea, ToAreaIdProperty);

        /// <summary>
        /// 至库区
        /// </summary>
        public StorageArea ToArea
        {
            get { return this.GetRefEntity(ToAreaProperty); }
            set { this.SetRefEntity(ToAreaProperty, value); }
        }
        #endregion

        #region 至货主 ToStorerCode
        /// <summary>
        /// 至货主
        /// </summary>
        [Label("至货主")]
        public static readonly Property<string> ToStorerCodeProperty = P<BaseTransaction>.Register(e => e.ToStorerCode);

        /// <summary>
        /// 至货主
        /// </summary>
        public string ToStorerCode
        {
            get { return this.GetProperty(ToStorerCodeProperty); }
            set { this.SetProperty(ToStorerCodeProperty, value); }
        }
        #endregion

        #region 原因 Reason
        /// <summary>
        /// 原因
        /// </summary>
        [MaxLength(2000)]
        [Label("原因")]
        public static readonly Property<string> ReasonProperty = P<BaseTransaction>.Register(e => e.Reason);

        /// <summary>
        /// 原因
        /// </summary>
        public string Reason
        {
            get { return GetProperty(ReasonProperty); }
            set { SetProperty(ReasonProperty, value); }
        }
        #endregion

        #region 单据大类 OrderType
        /// <summary>
        /// 单据大类
        /// </summary>
        [Label("单据大类")]
        public static readonly Property<OrderType> OrderTypeProperty = P<BaseTransaction>.Register(e => e.OrderType);

        /// <summary>
        /// 单据大类
        /// </summary>
        public OrderType OrderType
        {
            get { return GetProperty(OrderTypeProperty); }
            set { SetProperty(OrderTypeProperty, value); }
        }
        #endregion

        #region 单据小类 Transaction
        /// <summary>
        /// 单据小类Id
        /// </summary>
        [Label("单据小类")]
        public static readonly IRefIdProperty TransactionIdProperty =
            P<BaseTransaction>.RegisterRefId(e => e.TransactionId, ReferenceType.Normal);

        /// <summary>
        /// 单据小类Id
        /// </summary>
        public double? TransactionId
        {
            get { return (double?)this.GetRefNullableId(TransactionIdProperty); }
            set { this.SetRefNullableId(TransactionIdProperty, value); }
        }

        /// <summary>
        /// 单据小类
        /// </summary>
        public static readonly RefEntityProperty<Transaction> TransactionProperty =
            P<BaseTransaction>.RegisterRef(e => e.Transaction, TransactionIdProperty);

        /// <summary>
        /// 单据小类
        /// </summary>
        public Transaction Transaction
        {
            get { return this.GetRefEntity(TransactionProperty); }
            set { this.SetRefEntity(TransactionProperty, value); }
        }
        #endregion

        #region 交易类型 TransactionType
        /// <summary>
        /// 交易类型
        /// </summary>
        [Label("交易类型")]
        public static readonly Property<TransactionType> TransactionTypeProperty = P<BaseTransaction>.Register(e => e.TransactionType);

        /// <summary>
        /// 交易类型
        /// </summary>
        public TransactionType TransactionType
        {
            get { return GetProperty(TransactionTypeProperty); }
            set { SetProperty(TransactionTypeProperty, value); }
        }
        #endregion

        #region 规格型号 ItemSpecificationModel
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> ItemSpecificationModelProperty = P<BaseTransaction>.RegisterView(e => e.ItemSpecificationModel, p => p.Item.SpecificationModel);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string ItemSpecificationModel
        {
            get { return this.GetProperty(ItemSpecificationModelProperty); }
        }
        #endregion

        #region 单位(主) UnitCode
        /// <summary>
        /// 单位(主)
        /// </summary>
        [Label("单位(主)")]
        public static readonly Property<string> UnitCodeProperty = P<BaseTransaction>.RegisterView(e => e.UnitCode, p => p.Unit.Code);

        /// <summary>
        /// 单位(主)
        /// </summary>
        public string UnitCode
        {
            get { return this.GetProperty(UnitCodeProperty); }
        }
        #endregion

        #region 单位名称(主) UnitName
        /// <summary>
        /// 单位名称(主)
        /// </summary>
        [Label("单位(主)")]
        public static readonly Property<string> UnitNameProperty = P<BaseTransaction>.RegisterView(e => e.UnitName, p => p.Unit.Name);

        /// <summary>
        /// 单位名称(主)
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
        }
        #endregion

        #region 特殊事务标记 SpecialTransMark
        /// <summary>
        /// 特殊事务标记
        /// </summary>
        [Label("特殊事务标记")]
        public static readonly Property<SpecialTransMark> SpecialTransMarkProperty = P<BaseTransaction>.Register(e => e.SpecialTransMark);

        /// <summary>
        /// 特殊事务标记
        /// </summary>
        public SpecialTransMark SpecialTransMark
        {
            get { return GetProperty(SpecialTransMarkProperty); }
            set { SetProperty(SpecialTransMarkProperty, value); }
        }
        #endregion

        #region 数量(采) PurchaseQty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量(采)")]
        [MinValue(0)]
        public static readonly Property<decimal?> PurchaseQtyProperty = P<BaseTransaction>.Register(e => e.PurchaseQty);

        /// <summary>
        /// 数量(采)
        /// </summary>
        public decimal? PurchaseQty
        {
            get { return GetProperty(PurchaseQtyProperty); }
            set { SetProperty(PurchaseQtyProperty, value); }
        }
        #endregion

        #region 单位(采) PurchaseUnit
        /// <summary>
        /// 单位(采)
        /// </summary>
        [Label("单位(采)")]
        [MaxLength(80)]
        public static readonly Property<string> PurchaseUnitProperty = P<BaseTransaction>.Register(e => e.PurchaseUnit);

        /// <summary>
        /// 单位(采)
        /// </summary>
        public string PurchaseUnit
        {
            get { return this.GetProperty(PurchaseUnitProperty); }
            set { SetProperty(PurchaseUnitProperty, value); }
        }
        #endregion

        #region 采购转主转换率 ConvertFigre
        /// <summary>
        /// 采购转主转换率
        /// </summary>
        [Label("转换率")]
        public static readonly Property<decimal> ConvertFigreProperty = P<BaseTransaction>.Register(e => e.ConvertFigre);

        /// <summary>
        /// 采购转主转换率
        /// </summary>
        public decimal ConvertFigre
        {
            get { return GetProperty(ConvertFigreProperty); }
            set { SetProperty(ConvertFigreProperty, value); }
        }
        #endregion

        #region 自仓库名称 FromWarehouseName
        /// <summary>
        /// 自仓库名称
        /// </summary>
        [Label("自仓库名称")]
        public static readonly Property<string> FromWarehouseNameProperty = P<BaseTransaction>.RegisterView(e => e.FromWarehouseName, p => p.FromWarehouse.Name);

        /// <summary>
        /// 自仓库名称
        /// </summary>
        public string FromWarehouseName
        {
            get { return this.GetProperty(FromWarehouseNameProperty); }
        }
        #endregion

        #region 至仓库名称 ToWarehouseName
        /// <summary>
        /// 至仓库名称
        /// </summary>
        [Label("至仓库名称")]
        public static readonly Property<string> ToWarehouseNameProperty = P<BaseTransaction>.RegisterView(e => e.ToWarehouseName, p => p.ToWarehouse.Name);

        /// <summary>
        /// 至仓库名称
        /// </summary>
        public string ToWarehouseName
        {
            get { return this.GetProperty(ToWarehouseNameProperty); }
        }
        #endregion

        #region 物料类型 ItemType
        /// <summary>
        /// 物料类型
        /// </summary>
        [Label("物料类型")]
        public static readonly Property<ItemType> ItemTypeProperty = P<BaseTransaction>.RegisterView(e => e.ItemType, p => p.Item.Type);

        /// <summary>
        /// 物料类型
        /// </summary>
        public ItemType ItemType
        {
            get { return this.GetProperty(ItemTypeProperty); }
        }
        #endregion

        #region Asn单号 AsnNo
        /// <summary>
        /// Asn单号
        /// </summary>
        [Label("Asn单号")]
        public static readonly Property<string> AsnNoProperty = P<BaseTransaction>.Register(e => e.AsnNo);

        /// <summary>
        /// Asn单号
        /// </summary>
        public string AsnNo
        {
            get { return this.GetProperty(AsnNoProperty); }
            set { this.SetProperty(AsnNoProperty, value); }
        }
        #endregion

        #region Asn单行号 AsnLineNo
        /// <summary>
        /// Asn单行号
        /// </summary>
        [Label("Asn单行号")]
        public static readonly Property<string> AsnLineNoProperty = P<BaseTransaction>.Register(e => e.AsnLineNo);

        /// <summary>
        /// Asn单号
        /// </summary>
        public string AsnLineNo
        {
            get { return this.GetProperty(AsnLineNoProperty); }
            set { this.SetProperty(AsnLineNoProperty, value); }
        }
        #endregion

        #region 采购单ID PoId
        /// <summary>
        /// 采购单ID
        /// </summary>
        [Label("采购单ID")]
        public static readonly Property<double?> PoIdProperty = P<BaseTransaction>.Register(e => e.PoId);

        /// <summary>
        /// 采购单ID
        /// </summary>
        public double? PoId
        {
            get { return GetProperty(PoIdProperty); }
            set { SetProperty(PoIdProperty, value); }
        }
        #endregion

        #region 采购单号 PoNo
        /// <summary>
        /// 采购单号
        /// </summary>
        [Label("采购单号")]
        public static readonly Property<string> PoNoProperty = P<BaseTransaction>.Register(e => e.PoNo);

        /// <summary>
        /// 采购单号
        /// </summary>
        public string PoNo
        {
            get { return GetProperty(PoNoProperty); }
            set { SetProperty(PoNoProperty, value); }
        }
        #endregion

        #region 采购单行ID PoLineId
        /// <summary>
        /// 采购单行ID
        /// </summary>
        [Label("采购单行ID")]
        public static readonly Property<double?> PoLineIdProperty = P<BaseTransaction>.Register(e => e.PoLineId);

        /// <summary>
        /// 采购单行ID
        /// </summary>
        public double? PoLineId
        {
            get { return GetProperty(PoLineIdProperty); }
            set { SetProperty(PoLineIdProperty, value); }
        }
        #endregion

        #region 采购单行号 PoLineNo
        /// <summary>
        /// 采购单行号
        /// </summary>
        [Label("采购单行号")]
        public static readonly Property<string> PoLineNoProperty = P<BaseTransaction>.Register(e => e.PoLineNo);

        /// <summary>
        /// 采购单行号
        /// </summary>
        public string PoLineNo
        {
            get { return GetProperty(PoLineNoProperty); }
            set { SetProperty(PoLineNoProperty, value); }
        }
        #endregion        

        #region 调拨模式 AllotModel
        /// <summary>
        /// 调拨模式
        /// </summary>
        [Label("调拨模式")]
        public static readonly Property<AllotModel?> AllotModelProperty = P<BaseTransaction>.Register(e => e.AllotModel);

        /// <summary>
        /// 调拨模式
        /// </summary>
        public AllotModel? AllotModel
        {
            get { return GetProperty(AllotModelProperty); }
            set { SetProperty(AllotModelProperty, value); }
        }
        #endregion

        #region 二级明细Id SecondDtlId
        /// <summary>
        /// 二级明细Id
        /// </summary>
        [Label("二级明细Id")]
        public static readonly Property<double?> SecondDtlIdProperty = P<BaseTransaction>.Register(e => e.SecondDtlId);

        /// <summary>
        /// 二级明细Id
        /// </summary>
        public double? SecondDtlId
        {
            get { return this.GetProperty(SecondDtlIdProperty); }
            set { this.SetProperty(SecondDtlIdProperty, value); }
        }
        #endregion

        #region 二级明细行号 SecondDtlLineNo
        /// <summary>
        /// 二级明细行号
        /// </summary>
        [Label("二级明细行号")]
        public static readonly Property<string> SecondDtlLineNoProperty = P<BaseTransaction>.Register(e => e.SecondDtlLineNo);

        /// <summary>
        /// 二级明细行号
        /// </summary>
        public string SecondDtlLineNo
        {
            get { return this.GetProperty(SecondDtlLineNoProperty); }
            set { this.SetProperty(SecondDtlLineNoProperty, value); }
        }
        #endregion

        #region ERP相关字段

        #region Erp库存组织名称 ErpOrganizationName
        /// <summary>
        /// Erp库存组织名称
        /// </summary>
        [Label("ERP库存组织名称")]
        public static readonly Property<string> ErpOrganizationNameProperty = P<BaseTransaction>.Register(e => e.ErpOrganizationName);

        /// <summary>
        /// Erp库存组织名称
        /// </summary>
        public string ErpOrganizationName
        {
            get { return this.GetProperty(ErpOrganizationNameProperty); }
            set { this.SetProperty(ErpOrganizationNameProperty, value); }
        }
        #endregion

        #region Erp业务实体名词 ErpOrgName
        /// <summary>
        /// Erp业务实体名词
        /// </summary>
        [Label("ERP业务实体名词")]
        public static readonly Property<string> ErpOrgNameProperty = P<BaseTransaction>.Register(e => e.ErpOrgName);

        /// <summary>
        /// Erp业务实体名词
        /// </summary>
        public string ErpOrgName
        {
            get { return this.GetProperty(ErpOrgNameProperty); }
            set { this.SetProperty(ErpOrgNameProperty, value); }
        }
        #endregion

        #region Erp子库 ErpWarehouseCode
        /// <summary>
        /// Erp子库
        /// </summary>
        [Label("ERP子库")]
        public static readonly Property<string> ErpWarehouseCodeProperty = P<BaseTransaction>.Register(e => e.ErpWarehouseCode);

        /// <summary>
        /// Erp子库
        /// </summary>
        public string ErpWarehouseCode
        {
            get { return this.GetProperty(ErpWarehouseCodeProperty); }
            set { this.SetProperty(ErpWarehouseCodeProperty, value); }
        }
        #endregion

        #region 目标ERP子库 TargetErpWarehouseCode
        /// <summary>
        /// Erp子库
        /// </summary>
        [Label("目标ERP子库")]
        public static readonly Property<string> TargetErpWarehouseCodeProperty = P<BaseTransaction>.Register(e => e.TargetErpWarehouseCode);

        /// <summary>
        /// 目标ERP子库,调拨用
        /// </summary>
        public string TargetErpWarehouseCode
        {
            get { return this.GetProperty(TargetErpWarehouseCodeProperty); }
            set { this.SetProperty(TargetErpWarehouseCodeProperty, value); }
        }
        #endregion

        #region Erp账户别名 ErpAccount
        /// <summary>
        /// Erp账户别名
        /// </summary>
        [Label("Erp账户别名")]
        public static readonly Property<string> ErpAccountProperty = P<BaseTransaction>.Register(e => e.ErpAccount);

        /// <summary>
        /// Erp账户别名
        /// </summary>
        public string ErpAccount
        {
            get { return this.GetProperty(ErpAccountProperty); }
            set { this.SetProperty(ErpAccountProperty, value); }
        }
        #endregion

        #region 目标库存组织名称 TargetErpOrganizationName
        /// <summary>
        /// 目标库存组织名称
        /// </summary>
        [Label("目标库存组织名称")]
        public static readonly Property<string> TargetErpOrganizationNameProperty = P<BaseTransaction>.Register(e => e.TargetErpOrganizationName);

        /// <summary>
        /// 目标库存组织名称
        /// </summary>
        public string TargetErpOrganizationName
        {
            get { return this.GetProperty(TargetErpOrganizationNameProperty); }
            set { this.SetProperty(TargetErpOrganizationNameProperty, value); }
        }
        #endregion

        #region Sap暂收物料凭证号 SapTempReceiveNo
        /// <summary>
        /// Sap暂收物料凭证号
        /// </summary>
        [Label("Sap暂收物料凭证号")]
        public static readonly Property<string> SapTempReceiveNoProperty = P<BaseTransaction>.Register(e => e.SapTempReceiveNo);

        /// <summary>
        /// Sap暂收物料凭证号
        /// </summary>
        public string SapTempReceiveNo
        {
            get { return GetProperty(SapTempReceiveNoProperty); }
            set { SetProperty(SapTempReceiveNoProperty, value); }
        }
        #endregion

        #region Sap入库凭证号 SapInStorageNo
        /// <summary>
        /// Sap入库凭证号
        /// </summary>
        [Label("Sap入库凭证号")]
        public static readonly Property<string> SapInStorageNoProperty = P<BaseTransaction>.Register(e => e.SapInStorageNo);

        /// <summary>
        /// Sap入库凭证号
        /// </summary>
        public string SapInStorageNo
        {
            get { return GetProperty(SapInStorageNoProperty); }
            set { SetProperty(SapInStorageNoProperty, value); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 库存交易 实体配置
    /// </summary>
    internal class BaseTransactionConfig : EntityConfig<BaseTransaction>
    {
        /// <summary>
        /// 子类重写此方法，并完成对 Meta 属性的配置。
        /// 注意：
        /// * 为了给当前类的子类也运行同样的配置，这个方法可能会被调用多次。
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INV_TRAN").MapAllProperties();
            Meta.Property(BaseTransaction.RemarkProperty).ColumnMeta.HasLength(4000);
            ////Meta.Property(BaseTransaction.ItemDescProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}