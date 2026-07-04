using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Enums;
using SIE.Inventory.Commom;
using SIE.Inventory.Onhands;
using SIE.Inventory.TransactionProcessing;
using SIE.Inventory.Transactions;
using SIE.MES.TaskManagement.Reports;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ERPInterface.Common.Logs
{
    /// <summary>
    /// 事务上传
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(UploadTransactionCriteria))]

    [Label("事务上传")]
    public partial class UploadTransaction : DataEntity
    {
        #region 交易日期 TransactionDate
        /// <summary>
        /// 交易日期
        /// </summary>
        [Label("交易日期")]
        public static readonly Property<DateTime> TransactionDateProperty = P<UploadTransaction>.Register(e => e.TransactionDate);

        /// <summary>
        /// 交易日期
        /// </summary>
        public DateTime TransactionDate
        {
            get { return this.GetProperty(TransactionDateProperty); }
            set { this.SetProperty(TransactionDateProperty, value); }
        }
        #endregion

        #region 单据大类 OrderType
        /// <summary>
        /// 单据大类
        /// </summary>
        [Label("单据大类")]
        public static readonly Property<OrderType> OrderTypeProperty = P<UploadTransaction>.Register(e => e.OrderType);

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
        /// 单据小类
        /// </summary>
        [Label("单据小类")]
        public static readonly IRefIdProperty TransactionIdProperty =
            P<UploadTransaction>.RegisterRefId(e => e.TransactionId, ReferenceType.Normal);

        /// <summary>
        /// 单据小类
        /// </summary>
        public double? TransactionId
        {
            get { return (double)this.GetRefId(TransactionIdProperty); }
            set { this.SetRefId(TransactionIdProperty, value); }
        }

        /// <summary>
        /// 单据小类
        /// </summary>
        public static readonly RefEntityProperty<Transaction> TransactionProperty =
            P<UploadTransaction>.RegisterRef(e => e.Transaction, TransactionIdProperty);

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
        public static readonly Property<TransactionType> TransactionTypeProperty = P<UploadTransaction>.Register(e => e.TransactionType);

        /// <summary>
        /// 交易类型
        /// </summary>
        public TransactionType TransactionType
        {
            get { return GetProperty(TransactionTypeProperty); }
            set { SetProperty(TransactionTypeProperty, value); }
        }
        #endregion

        #region 处理状态 ProcessState
        /// <summary>
        /// 处理状态
        /// </summary>
        [Label("处理状态")]
        public static readonly Property<ProcessState> StateProperty = P<UploadTransaction>.Register(e => e.State);

        /// <summary>
        /// 处理状态
        /// </summary>
        public ProcessState State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 处理信息 ProcessMessage
        /// <summary>
        /// 处理信息
        /// </summary>
        [Label("处理信息")]
        public static readonly Property<string> ProcessMessageProperty = P<UploadTransaction>.Register(e => e.ProcessMessage);

        /// <summary>
        /// 处理信息
        /// </summary>
        public string ProcessMessage
        {
            get { return this.GetProperty(ProcessMessageProperty); }
            set { this.SetProperty(ProcessMessageProperty, value); }
        }
        #endregion

        #region 校验信息 ValidateMessage
        /// <summary>
        /// 校验信息
        /// </summary>
        [Label("校验信息")]
        public static readonly Property<string> ValidateMessageProperty = P<UploadTransaction>.Register(e => e.ValidateMessage);

        /// <summary>
        /// 校验信息
        /// </summary>
        public string ValidateMessage
        {
            get { return this.GetProperty(ValidateMessageProperty); }
            set { this.SetProperty(ValidateMessageProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<UploadTransaction>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
            set { this.SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<UploadTransaction>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 物料ID ItemId
        /// <summary>
        /// 物料ID
        /// </summary>
        [Label("物料ID")]
        public static readonly Property<double?> ItemIdProperty = P<UploadTransaction>.Register(e => e.ItemId);

        /// <summary>
        /// 物料ID
        /// </summary>
        public double? ItemId
        {
            get { return this.GetProperty(ItemIdProperty); }
            set { this.SetProperty(ItemIdProperty, value); }
        }
        #endregion

        #region 物料ERP主键 ItemErpKey
        /// <summary>
        /// 物料ERP主键
        /// </summary>
        [Label("物料ERP主键")]
        public static readonly Property<string> ItemErpKeyProperty = P<UploadTransaction>.Register(e => e.ItemErpKey);

        /// <summary>
        /// 物料ERP主键
        /// </summary>
        public string ItemErpKey
        {
            get { return this.GetProperty(ItemErpKeyProperty); }
            set { this.SetProperty(ItemErpKeyProperty, value); }
        }
        #endregion

        #region 批次 LotCode
        /// <summary>
        /// 批次
        /// </summary>
        [Label("批次")]
        public static readonly Property<string> LotCodeProperty = P<UploadTransaction>.Register(e => e.LotCode);

        /// <summary>
        /// 批次
        /// </summary>
        public string LotCode
        {
            get { return this.GetProperty(LotCodeProperty); }
            set { this.SetProperty(LotCodeProperty, value); }
        }
        #endregion

        #region 数量 Quantity
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QuantityProperty = P<UploadTransaction>.Register(e => e.Quantity);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quantity
        {
            get { return GetProperty(QuantityProperty); }
            set { SetProperty(QuantityProperty, value); }
        }
        #endregion

        #region 单据号 BillNo
        /// <summary>
        /// 单据号
        /// </summary>
        [Label("单据号")]
        public static readonly Property<string> BillNoProperty = P<UploadTransaction>.Register(e => e.BillNo);

        /// <summary>
        /// 单据号
        /// </summary>
        public string BillNo
        {
            get { return this.GetProperty(BillNoProperty); }
            set { this.SetProperty(BillNoProperty, value); }
        }
        #endregion

        #region 单据ID BillId
        /// <summary>
        /// 单据ID
        /// </summary>
        [Label("单据ID")]
        public static readonly Property<double?> BillIdProperty = P<UploadTransaction>.Register(e => e.BillId);

        /// <summary>
        /// 单据ID
        /// </summary>
        public double? BillId
        {
            get { return this.GetProperty(BillIdProperty); }
            set { this.SetProperty(BillIdProperty, value); }
        }
        #endregion

        #region 单据ERP主键 BillErpKey
        /// <summary>
        /// 单据ERP主键
        /// </summary>
        [Label("单据ERP主键")]
        public static readonly Property<string> BillErpKeyProperty = P<UploadTransaction>.Register(e => e.BillErpKey);

        /// <summary>
        /// 单据ERPID
        /// </summary>
        public string BillErpKey
        {
            get { return this.GetProperty(BillErpKeyProperty); }
            set { this.SetProperty(BillErpKeyProperty, value); }
        }
        #endregion

        #region 单据行ID BillLineId
        /// <summary>
        /// 单据行ID
        /// </summary>
        [Label("单据行ID")]
        public static readonly Property<double?> BillLineIdProperty = P<UploadTransaction>.Register(e => e.BillLineId);

        /// <summary>
        /// 单据行ID
        /// </summary>
        public double? BillLineId
        {
            get { return this.GetProperty(BillLineIdProperty); }
            set { this.SetProperty(BillLineIdProperty, value); }
        }
        #endregion

        #region 单据行号 BillLineNo
        /// <summary>
        /// 单据行号
        /// </summary>
        [Label("单据行号")]
        public static readonly Property<string> BillLineNoProperty = P<UploadTransaction>.Register(e => e.BillLineNo);

        /// <summary>
        /// 单据行号
        /// </summary>
        public string BillLineNo
        {
            get { return this.GetProperty(BillLineNoProperty); }
            set { this.SetProperty(BillLineNoProperty, value); }
        }
        #endregion

        #region 单据行ERP主键 BillLineErpKey
        /// <summary>
        /// 单据行ERP主键（对应单据的ErpDetailId上传ERP的LINE_ID）
        /// </summary>
        [Label("单据行ERP主键")]
        public static readonly Property<string> BillLineErpKeyProperty = P<UploadTransaction>.Register(e => e.BillLineErpKey);

        /// <summary>
        /// 单据行ERP主键（对应单据的ErpDetailId上传ERP的LINE_ID）
        /// </summary>
        public string BillLineErpKey
        {
            get { return this.GetProperty(BillLineErpKeyProperty); }
            set { this.SetProperty(BillLineErpKeyProperty, value); }
        }
        #endregion

        #region 采购订单号 PoNo
        /// <summary>
        /// 采购订单号
        /// </summary>
        [Label("采购订单号")]
        public static readonly Property<string> PoNoProperty = P<UploadTransaction>.Register(e => e.PoNo);

        /// <summary>
        /// 采购订单号
        /// </summary>
        public string PoNo
        {
            get { return this.GetProperty(PoNoProperty); }
            set { this.SetProperty(PoNoProperty, value); }
        }
        #endregion

        #region 采购订单行号 PoLineNo
        /// <summary>
        /// 采购订单行号
        /// </summary>
        [Label("采购订单行号")]
        public static readonly Property<string> PoLineNoProperty = P<UploadTransaction>.Register(e => e.PoLineNo);

        /// <summary>
        /// 采购订单行号
        /// </summary>
        public string PoLineNo
        {
            get { return this.GetProperty(PoLineNoProperty); }
            set { this.SetProperty(PoLineNoProperty, value); }
        }
        #endregion

        #region 采购订单ID PoId
        /// <summary>
        /// 采购订单ID
        /// </summary>
        [Label("采购订单ID")]
        public static readonly Property<double?> PoIdProperty = P<UploadTransaction>.Register(e => e.PoId);

        /// <summary>
        /// 采购订单ID
        /// </summary>
        public double? PoId
        {
            get { return this.GetProperty(PoIdProperty); }
            set { this.SetProperty(PoIdProperty, value); }
        }
        #endregion

        #region 采购订单行ID PoLineId
        /// <summary>
        /// 采购订单行ID
        /// </summary>
        [Label("采购订单行ID")]
        public static readonly Property<double?> PoLineIdProperty = P<UploadTransaction>.Register(e => e.PoLineId);

        /// <summary>
        /// 采购订单行ID
        /// </summary>
        public double? PoLineId
        {
            get { return this.GetProperty(PoLineIdProperty); }
            set { this.SetProperty(PoLineIdProperty, value); }
        }
        #endregion

        #region 采购订单ERP主键 PoErpKey
        /// <summary>
        /// 采购订单ERP主键
        /// </summary>
        [Label("采购订单ERP主键")]
        public static readonly Property<string> PoErpKeyProperty = P<UploadTransaction>.Register(e => e.PoErpKey);

        /// <summary>
        /// 采购订单ERP主键
        /// </summary>
        public string PoErpKey
        {
            get { return this.GetProperty(PoErpKeyProperty); }
            set { this.SetProperty(PoErpKeyProperty, value); }
        }
        #endregion
        
        #region 采购订单航ERP主键 PoLineErpKey
        /// <summary>
        /// 采购订单航ERP主键
        /// </summary>
        [Label("采购订单行ERP主键")]
        public static readonly Property<string> PoLineErpKeyProperty = P<UploadTransaction>.Register(e => e.PoLineErpKey);

        /// <summary>
        /// 采购订单航ERP主键
        /// </summary>
        public string PoLineErpKey
        {
            get { return this.GetProperty(PoLineErpKeyProperty); }
            set { this.SetProperty(PoLineErpKeyProperty, value); }
        }
        #endregion

        #region 采购单分配行ErpID PodistributionId
        /// <summary>
        /// 采购单分配行ErpID
        /// </summary>
        [Label("采购单分配行ErpID")]
        public static readonly Property<string> PodistributionIdProperty = P<UploadTransaction>.Register(e => e.PodistributionId);

        /// <summary>
        /// 采购单分配行ErpID
        /// </summary>
        public string PodistributionId
        {
            get { return GetProperty(PodistributionIdProperty); }
            set { SetProperty(PodistributionIdProperty, value); }
        }
        #endregion

        #region 采购单发运行ErpId PoLinelocationId
        /// <summary>
        /// 采购单发运行ErpId
        /// </summary>
        [Label("采购单发运行ErpId")]
        public static readonly Property<string> PoLinelocationIdProperty = P<UploadTransaction>.Register(e => e.PoLinelocationId);

        /// <summary>
        /// 采购单发运行ErpId
        /// </summary>
        public string PoLinelocationId
        {
            get { return GetProperty(PoLinelocationIdProperty); }
            set { SetProperty(PoLinelocationIdProperty, value); }
        }
        #endregion

        #region 采购暂收是否回传 PoIsReturnErp
        /// <summary>
        /// 采购暂收是否回传
        /// </summary>
        [Label("采购暂收是否回传")]
        public static readonly Property<bool?> PoIsReturnErpProperty = P<UploadTransaction>.Register(e => e.PoIsReturnErp);

        /// <summary>
        /// 采购暂收是否回传
        /// </summary>
        public bool? PoIsReturnErp
        {
            get { return this.GetProperty(PoIsReturnErpProperty); }
            set { this.SetProperty(PoIsReturnErpProperty, value); }
        }
        #endregion

        #region 工单号 WoNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WoNoProperty = P<UploadTransaction>.Register(e => e.WoNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNo
        {
            get { return GetProperty(WoNoProperty); }
            set { SetProperty(WoNoProperty, value); }
        }
        #endregion

        #region 工单ID WoId
        /// <summary>
        /// 工单ID
        /// </summary>
        [Label("工单ID")]
        public static readonly Property<double?> WoIdProperty = P<UploadTransaction>.Register(e => e.WoId);

        /// <summary>
        /// 工单ID
        /// </summary>
        public double? WoId
        {
            get { return this.GetProperty(WoIdProperty); }
            set { this.SetProperty(WoIdProperty, value); }
        }
        #endregion

        #region 工单ERP主键 WoErpKey
        /// <summary>
        /// 工单ERP主键
        /// </summary>
        [Label("工单ERP主键")]
        public static readonly Property<string> WoErpKeyProperty = P<UploadTransaction>.Register(e => e.WoErpKey);

        /// <summary>
        /// 工单ERP主键
        /// </summary>
        public string WoErpKey
        {
            get { return this.GetProperty(WoErpKeyProperty); }
            set { this.SetProperty(WoErpKeyProperty, value); }
        }
        #endregion

        #region 自库位编码 FromLocationCode
        /// <summary>
        /// 自库位编码
        /// </summary>
        [Label("自库位编码")]
        public static readonly Property<string> FromLocationCodeProperty = P<UploadTransaction>.Register(e => e.FromLocationCode);

        /// <summary>
        /// 自库位编码
        /// </summary>
        public string FromLocationCode
        {
            get { return this.GetProperty(FromLocationCodeProperty); }
            set { this.SetProperty(FromLocationCodeProperty, value); }
        }
        #endregion

        #region 自库位名称 FromLocationName
        /// <summary>
        /// 自库位名称
        /// </summary>
        [Label("自库位名称")]
        public static readonly Property<string> FromLocationNameProperty = P<UploadTransaction>.Register(e => e.FromLocationName);

        /// <summary>
        /// 自库位名称
        /// </summary>
        public string FromLocationName
        {
            get { return this.GetProperty(FromLocationNameProperty); }
            set { this.SetProperty(FromLocationNameProperty, value); }
        }
        #endregion

        #region 自库位ID FromLocationId
        /// <summary>
        /// 自库位ID
        /// </summary>
        [Label("自库位ID")]
        public static readonly Property<double?> FromLocationIdProperty = P<UploadTransaction>.Register(e => e.FromLocationId);

        /// <summary>
        /// 自库位ID
        /// </summary>
        public double? FromLocationId
        {
            get { return this.GetProperty(FromLocationIdProperty); }
            set { this.SetProperty(FromLocationIdProperty, value); }
        }
        #endregion

        #region 自库位ERP主键 FromLocationErpKey
        /// <summary>
        /// 自库位ERP主键
        /// </summary>
        [Label("自库位ERP主键")]
        public static readonly Property<string> FromLocationErpKeyProperty = P<UploadTransaction>.Register(e => e.FromLocationErpKey);

        /// <summary>
        /// 自库位ERP主键
        /// </summary>
        public string FromLocationErpKey
        {
            get { return this.GetProperty(FromLocationErpKeyProperty); }
            set { this.SetProperty(FromLocationErpKeyProperty, value); }
        }
        #endregion

        #region 自仓库编码 FromWarehouseCode
        /// <summary>
        /// 自仓库编码
        /// </summary>
        [Label("自仓库编码")]
        public static readonly Property<string> FromWarehouseCodeProperty = P<UploadTransaction>.Register(e => e.FromWarehouseCode);

        /// <summary>
        /// 自仓库编码
        /// </summary>
        public string FromWarehouseCode
        {
            get { return this.GetProperty(FromWarehouseCodeProperty); }
            set { this.SetProperty(FromWarehouseCodeProperty, value); }
        }
        #endregion

        #region 自仓库名称 FromWarehouseName
        /// <summary>
        /// 自仓库名称
        /// </summary>
        [Label("自仓库名称")]
        public static readonly Property<string> FromWarehouseNameProperty = P<UploadTransaction>.Register(e => e.FromWarehouseName);

        /// <summary>
        /// 自仓库名称
        /// </summary>
        public string FromWarehouseName
        {
            get { return this.GetProperty(FromWarehouseNameProperty); }
            set { this.SetProperty(FromWarehouseNameProperty, value); }
        }
        #endregion

        #region 自仓库ID FromWarehouseId
        /// <summary>
        /// 自仓库ID
        /// </summary>
        [Label("自仓库ID")]
        public static readonly Property<double?> FromWarehouseIdProperty = P<UploadTransaction>.Register(e => e.FromWarehouseId);

        /// <summary>
        /// 自仓库ID
        /// </summary>
        public double? FromWarehouseId
        {
            get { return this.GetProperty(FromWarehouseIdProperty); }
            set { this.SetProperty(FromWarehouseIdProperty, value); }
        }
        #endregion

        #region 自仓库ERP主键 FromWarehouseErpKey
        /// <summary>
        /// 自仓库ERP主键
        /// </summary>
        [Label("自仓库ERP主键")]
        public static readonly Property<string> FromWarehouseErpKeyProperty = P<UploadTransaction>.Register(e => e.FromWarehouseErpKey);

        /// <summary>
        /// 自仓库ERP主键
        /// </summary>
        public string FromWarehouseErpKey
        {
            get { return this.GetProperty(FromWarehouseErpKeyProperty); }
            set { this.SetProperty(FromWarehouseErpKeyProperty, value); }
        }
        #endregion

        #region 至库位编码 ToLocationCode
        /// <summary>
        /// 至库位编码
        /// </summary>
        [Label("至库位编码")]
        public static readonly Property<string> ToLocationCodeProperty = P<UploadTransaction>.Register(e => e.ToLocationCode);

        /// <summary>
        /// 至库位编码
        /// </summary>
        public string ToLocationCode
        {
            get { return this.GetProperty(ToLocationCodeProperty); }
            set { this.SetProperty(ToLocationCodeProperty, value); }
        }
        #endregion

        #region 至库位名称 ToLocationName
        /// <summary>
        /// 至库位名称
        /// </summary>
        [Label("至库位名称")]
        public static readonly Property<string> ToLocationNameProperty = P<UploadTransaction>.Register(e => e.ToLocationName);

        /// <summary>
        /// 至库位名称
        /// </summary>
        public string ToLocationName
        {
            get { return this.GetProperty(ToLocationNameProperty); }
            set { this.SetProperty(ToLocationNameProperty, value); }
        }
        #endregion

        #region 至库位ID ToLocationId
        /// <summary>
        /// 至库位ID
        /// </summary>
        [Label("至库位ID")]
        public static readonly Property<double?> ToLocationIdProperty = P<UploadTransaction>.Register(e => e.ToLocationId);

        /// <summary>
        /// 至库位ID
        /// </summary>
        public double? ToLocationId
        {
            get { return this.GetProperty(ToLocationIdProperty); }
            set { this.SetProperty(ToLocationIdProperty, value); }
        }
        #endregion

        #region 至库位ERP主键 ToLocationErpKey
        /// <summary>
        /// 至库位ERP主键
        /// </summary>
        [Label("至库位ERP主键")]
        public static readonly Property<string> ToLocationErpKeyProperty = P<UploadTransaction>.Register(e => e.ToLocationErpKey);

        /// <summary>
        /// 至库位ERP主键
        /// </summary>
        public string ToLocationErpKey
        {
            get { return this.GetProperty(ToLocationErpKeyProperty); }
            set { this.SetProperty(ToLocationErpKeyProperty, value); }
        }
        #endregion

        #region 至仓库编码 ToWarehouseCode
        /// <summary>
        /// 至仓库编码
        /// </summary>
        [Label("至仓库编码")]
        public static readonly Property<string> ToWarehouseCodeProperty = P<UploadTransaction>.Register(e => e.ToWarehouseCode);

        /// <summary>
        /// 至仓库编码
        /// </summary>
        public string ToWarehouseCode
        {
            get { return this.GetProperty(ToWarehouseCodeProperty); }
            set { this.SetProperty(ToWarehouseCodeProperty, value); }
        }
        #endregion

        #region 至仓库名称 ToWarehouseName
        /// <summary>
        /// 至仓库名称
        /// </summary>
        [Label("至仓库名称")]
        public static readonly Property<string> ToWarehouseNameProperty = P<UploadTransaction>.Register(e => e.ToWarehouseName);

        /// <summary>
        /// 至仓库名称
        /// </summary>
        public string ToWarehouseName
        {
            get { return this.GetProperty(ToWarehouseNameProperty); }
            set { this.SetProperty(ToWarehouseNameProperty, value); }
        }
        #endregion

        #region 至仓库ID ToWarehouseId
        /// <summary>
        /// 至仓库ID
        /// </summary>
        [Label("至仓库ID")]
        public static readonly Property<double?> ToWarehouseIdProperty = P<UploadTransaction>.Register(e => e.ToWarehouseId);

        /// <summary>
        /// 至仓库ID
        /// </summary>
        public double? ToWarehouseId
        {
            get { return this.GetProperty(ToWarehouseIdProperty); }
            set { this.SetProperty(ToWarehouseIdProperty, value); }
        }
        #endregion

        #region 至仓库ERP主键 ToWarehouseErpKey
        /// <summary>
        /// 至仓库ERP主键
        /// </summary>
        [Label("至仓库ERP主键")]
        public static readonly Property<string> ToWarehouseErpKeyProperty = P<UploadTransaction>.Register(e => e.ToWarehouseErpKey);

        /// <summary>
        /// 至仓库ERP主键
        /// </summary>
        public string ToWarehouseErpKey
        {
            get { return this.GetProperty(ToWarehouseErpKeyProperty); }
            set { this.SetProperty(ToWarehouseErpKeyProperty, value); }
        }
        #endregion

        #region 库存事务ID InvTransactionId
        /// <summary>
        /// 库存事务ID
        /// </summary>
        [Label("库存事务ID")]
        public static readonly Property<double> InvTransactionIdProperty = P<UploadTransaction>.Register(e => e.InvTransactionId);

        /// <summary>
        /// 库存事务ID
        /// </summary>
        public double InvTransactionId
        {
            get { return this.GetProperty(InvTransactionIdProperty); }
            set { this.SetProperty(InvTransactionIdProperty, value); }
        }
        #endregion

        #region 供应商ID SupplierId
        /// <summary>
        /// 供应商ID
        /// </summary>
        [Label("供应商ID")]
        public static readonly Property<double?> SupplierIdProperty = P<UploadTransaction>.Register(e => e.SupplierId);

        /// <summary>
        /// 供应商ID
        /// </summary>
        public double? SupplierId
        {
            get { return this.GetProperty(SupplierIdProperty); }
            set { this.SetProperty(SupplierIdProperty, value); }
        }
        #endregion

        #region 供应商ERP主键 SupplierErpKey
        /// <summary>
        /// 供应商ERP主键
        /// </summary>
        [Label("供应商ERP主键")]
        public static readonly Property<string> SupplierErpKeyProperty = P<UploadTransaction>.Register(e => e.SupplierErpKey);

        /// <summary>
        /// 供应商ERP主键
        /// </summary>
        public string SupplierErpKey
        {
            get { return this.GetProperty(SupplierErpKeyProperty); }
            set { this.SetProperty(SupplierErpKeyProperty, value); }
        }
        #endregion

        #region 供应商地址ID SupplierAddressId
        /// <summary>
        /// 注释
        /// </summary>
        [Label("供应商地址ID")]
        public static readonly Property<double?> SupplierAddressIdProperty = P<UploadTransaction>.Register(e => e.SupplierAddressId);

        /// <summary>
        /// 注释
        /// </summary>
        public double? SupplierAddressId
        {
            get { return this.GetProperty(SupplierAddressIdProperty); }
            set { this.SetProperty(SupplierAddressIdProperty, value); }
        }
        #endregion

        #region 供应商地址ERP主键 SupplierAddrErpKey
        /// <summary>
        /// 供应商地址ERP主键
        /// </summary>
        [Label("供应商地址ERP主键")]
        public static readonly Property<string> SupplierAddrErpKeyProperty = P<UploadTransaction>.Register(e => e.SupplierAddrErpKey);

        /// <summary>
        /// 供应商地址ERP主键
        /// </summary>
        public string SupplierAddrErpKey
        {
            get { return this.GetProperty(SupplierAddrErpKeyProperty); }
            set { this.SetProperty(SupplierAddrErpKeyProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<UploadTransaction>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 单位 UnitCode
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitCodeProperty = P<UploadTransaction>.Register(e => e.UnitCode);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitCode
        {
            get { return this.GetProperty(UnitCodeProperty); }
            set { this.SetProperty(UnitCodeProperty, value); }
        }
        #endregion

        #region 单位名称 UnitName
        /// <summary>
        /// 单位名称
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<UploadTransaction>.Register(e => e.UnitName);

        /// <summary>
        /// 单位名称
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
            set { this.SetProperty(UnitNameProperty, value); }
        }
        #endregion

        #region 原因ID ReasonId
        /// <summary>
        /// 原因ID
        /// </summary>
        [Label("原因ID")]
        public static readonly Property<double?> ReasonIdProperty = P<UploadTransaction>.Register(e => e.ReasonId);

        /// <summary>
        /// 原因ID
        /// </summary>
        public double? ReasonId
        {
            get { return this.GetProperty(ReasonIdProperty); }
            set { this.SetProperty(ReasonIdProperty, value); }
        }
        #endregion

        #region 原因名称 ReasonName
        /// <summary>
        /// 原因名称
        /// </summary>
        [Label("原因名称")]
        public static readonly Property<string> ReasonNameProperty = P<UploadTransaction>.Register(e => e.ReasonName);

        /// <summary>
        /// 原因名称
        /// </summary>
        public string ReasonName
        {
            get { return this.GetProperty(ReasonNameProperty); }
            set { this.SetProperty(ReasonNameProperty, value); }
        }
        #endregion

        #region 原因ERP主键 ReasonErpKey
        /// <summary>
        /// 原因ERP主键
        /// </summary>
        [Label("原因ERP主键")]
        public static readonly Property<string> ReasonErpKeyProperty = P<UploadTransaction>.Register(e => e.ReasonErpKey);

        /// <summary>
        /// 原因ERP主键
        /// </summary>
        public string ReasonErpKey
        {
            get { return this.GetProperty(ReasonErpKeyProperty); }
            set { this.SetProperty(ReasonErpKeyProperty, value); }
        }
        #endregion

        #region 自库存状态 FromOnhandState
        /// <summary>
        /// 自库存状态
        /// </summary>
        [Label("自库存状态")]
        public static readonly Property<OnhandState?> FromOnhandStateProperty = P<UploadTransaction>.Register(e => e.FromOnhandState);

        /// <summary>
        /// 自库存状态
        /// </summary>
        public OnhandState? FromOnhandState
        {
            get { return this.GetProperty(FromOnhandStateProperty); }
            set { this.SetProperty(FromOnhandStateProperty, value); }
        }
        #endregion

        #region 子库编码 ErpWarehouseCode
        /// <summary>
        /// 子库编码
        /// </summary>
        [Label("子库编码")]
        public static readonly Property<string> ErpWarehouseCodeProperty = P<UploadTransaction>.Register(e => e.ErpWarehouseCode);

        /// <summary>
        /// 子库编码
        /// </summary>
        public string ErpWarehouseCode
        {
            get { return this.GetProperty(ErpWarehouseCodeProperty); }
            set { this.SetProperty(ErpWarehouseCodeProperty, value); }
        }
        #endregion

        #region Erp账户别名 ErpAccount
        /// <summary>
        /// Erp账户别名
        /// </summary>
        [Label("Erp账户别名")]
        public static readonly Property<string> ErpAccountProperty = P<UploadTransaction>.Register(e => e.ErpAccount);

        /// <summary>
        /// Erp账户别名
        /// </summary>
        public string ErpAccount
        {
            get { return this.GetProperty(ErpAccountProperty); }
            set { this.SetProperty(ErpAccountProperty, value); }
        }
        #endregion

        #region Erp库存组织名称 ErpOrganizationName
        /// <summary>
        /// Erp库存组织名称
        /// </summary>
        [Label("Erp库存组织名称")]
        public static readonly Property<string> ErpOrganizationNameProperty = P<UploadTransaction>.Register(e => e.ErpOrganizationName);

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
        [Label("Erp业务实体名词")]
        public static readonly Property<string> ErpOrgNameProperty = P<UploadTransaction>.Register(e => e.ErpOrgName);

        /// <summary>
        /// Erp业务实体名词
        /// </summary>
        public string ErpOrgName
        {
            get { return this.GetProperty(ErpOrgNameProperty); }
            set { this.SetProperty(ErpOrgNameProperty, value); }
        }
        #endregion

        #region 生产批次 ProductBatch
        /// <summary>
        /// 生产批次
        /// </summary>
        [Label("生产批次")]
        public static readonly Property<string> ProductBatchProperty = P<UploadTransaction>.Register(e => e.ProductBatch);

        /// <summary>
        /// 生产批次
        /// </summary>
        public string ProductBatch
        {
            get { return this.GetProperty(ProductBatchProperty); }
            set { this.SetProperty(ProductBatchProperty, value); }
        }
        #endregion

        #region 目标子库编码 TargetErpWarehouseCode
        /// <summary>
        /// 目标子库编码
        /// </summary>
        [Label("目标子库编码")]
        public static readonly Property<string> TargetErpWarehouseCodeProperty = P<UploadTransaction>.Register(e => e.TargetErpWarehouseCode);

        /// <summary>
        /// 目标子库编码
        /// </summary>
        public string TargetErpWarehouseCode
        {
            get { return this.GetProperty(TargetErpWarehouseCodeProperty); }
            set { this.SetProperty(TargetErpWarehouseCodeProperty, value); }
        }
        #endregion

        #region Asn单号 AsnNo
        /// <summary>
        /// Asn单号
        /// </summary>
        [Label("Asn单号")]
        public static readonly Property<string> AsnNoProperty = P<UploadTransaction>.Register(e => e.AsnNo);

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
        /// Asn单号
        /// </summary>
        [Label("Asn单号")]
        public static readonly Property<string> AsnLineNoProperty = P<UploadTransaction>.Register(e => e.AsnLineNo);

        /// <summary>
        /// Asn单号
        /// </summary>
        public string AsnLineNo
        {
            get { return this.GetProperty(AsnLineNoProperty); }
            set { this.SetProperty(AsnLineNoProperty, value); }
        }
        #endregion

        #region 发运单号 SoNo
        /// <summary>
        /// 发运单号
        /// </summary>
        [Label("发运单号")]
        public static readonly Property<string> SoNoProperty = P<UploadTransaction>.Register(e => e.SoNo);

        /// <summary>
        /// Asn单号
        /// </summary>
        public string SoNo
        {
            get { return this.GetProperty(SoNoProperty); }
            set { this.SetProperty(SoNoProperty, value); }
        }
        #endregion

        #region 调拨模式 AllotModel
        /// <summary>
        /// 调拨模式
        /// </summary>
        [Label("调拨模式")]
        public static readonly Property<AllotModel?> AllotModelProperty = P<UploadTransaction>.Register(e => e.AllotModel);

        /// <summary>
        /// 调拨模式
        /// </summary>
        public AllotModel? AllotModel
        {
            get { return GetProperty(AllotModelProperty); }
            set { SetProperty(AllotModelProperty, value); }
        }
        #endregion

        #region 目标Erp库存组织名称 TargetErpOrganizationName
        /// <summary>
        /// 目标Erp库存组织名称
        /// </summary>
        [Label("目标Erp库存组织名称")]
        public static readonly Property<string> TargetErpOrganizationNameProperty = P<UploadTransaction>.Register(e => e.TargetErpOrganizationName);

        /// <summary>
        /// 目标Erp库存组织名称
        /// </summary>
        public string TargetErpOrganizationName
        {
            get { return this.GetProperty(TargetErpOrganizationNameProperty); }
            set { this.SetProperty(TargetErpOrganizationNameProperty, value); }
        }
        #endregion

        #region 上传次数 UploadCount
        /// <summary>
        /// 上传次数
        /// </summary>
        [Label("上传次数")]
        public static readonly Property<int?> UploadCountProperty = P<UploadTransaction>.Register(e => e.UploadCount);

        /// <summary>
        /// 上传次数
        /// </summary>
        public int? UploadCount
        {
            get { return this.GetProperty(UploadCountProperty); }
            set { this.SetProperty(UploadCountProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 单据小类编码 TransactionCode
        /// <summary>
        /// 单据小类编码
        /// </summary>
        [Label("单据小类编码")]
        public static readonly Property<string> TransactionCodeProperty = P<UploadTransaction>.RegisterView(e => e.TransactionCode, p => p.Transaction.Code);

        /// <summary>
        /// 单据小类编码
        /// </summary>
        public string TransactionCode
        {
            get { return this.GetProperty(TransactionCodeProperty); }
        }
        #endregion

        #region 单据小类名称 TransactionName
        /// <summary>
        /// 单据小类名称
        /// </summary>
        [Label("单据小类名称")]
        public static readonly Property<string> TransactionNameProperty = P<UploadTransaction>.RegisterView(e => e.TransactionName, p => p.Transaction.Name);

        /// <summary>
        /// 单据小类名称
        /// </summary>
        public string TransactionName
        {
            get { return this.GetProperty(TransactionNameProperty); }
        }
        #endregion

        #endregion

        #region 不映射数据库

        #region 旧料号 ShortDescription
        /// <summary>
        /// 旧料号
        /// </summary>
        [Label("旧料号")]
        public static readonly Property<string> ShortDescriptionProperty = P<UploadTransaction>.Register(e => e.ShortDescription);

        /// <summary>
        /// 旧料号
        /// </summary>
        public string ShortDescription
        {
            get { return this.GetProperty(ShortDescriptionProperty); }
            set { this.SetProperty(ShortDescriptionProperty, value); }
        }
        #endregion

        #region 父级旧料号 Bismt
        /// <summary>
        /// 父级旧料号
        /// </summary>
        [Label("父级旧料号")]
        public static readonly Property<string> BismtProperty = P<UploadTransaction>.Register(e => e.Bismt);

        /// <summary>
        /// 父级旧料号
        /// </summary>
        public string Bismt
        {
            get { return this.GetProperty(BismtProperty); }
            set { this.SetProperty(BismtProperty, value); }
        }
        #endregion

        #region 工单车间 WorkShopName
        /// <summary>
        /// 工单车间
        /// </summary>
        [Label("工单车间")]
        public static readonly Property<string> WorkShopNameProperty = P<UploadTransaction>.Register(e => e.WorkShopName);

        /// <summary>
        /// 工单车间
        /// </summary>
        public string WorkShopName
        {
            get { return this.GetProperty(WorkShopNameProperty); }
            set { this.SetProperty(WorkShopNameProperty, value); }
        }
        #endregion

        #region 车间编码 WorkShopCode
        /// <summary>
        /// 车间编码
        /// </summary>
        [Label("车间编码")]
        public static readonly Property<string> WorkShopCodeProperty = P<UploadTransaction>.Register(e => e.WorkShopCode);

        /// <summary>
        /// 车间编码
        /// </summary>
        public string WorkShopCode
        {
            get { return this.GetProperty(WorkShopCodeProperty); }
            set { this.SetProperty(WorkShopCodeProperty, value); }
        }
        #endregion

        #endregion

        #region 来源Id SourceId
        /// <summary>
        /// 来源Id
        /// </summary>
        [Label("来源Id")]
        public static readonly Property<string> SourceIdProperty = P<UploadTransaction>.Register(e => e.SourceId);

        /// <summary>
        /// 来源Id
        /// </summary>
        public string SourceId
        {
            get { return this.GetProperty(SourceIdProperty); }
            set { this.SetProperty(SourceIdProperty, value); }
        }
        #endregion

        #region SAP相关

        #region SAP物料号 MANTR    
        /// <summary>
        /// SAP物料号
        /// </summary>
        [Label("SAP物料号")]
        public static readonly Property<string> MANTRProperty = P<UploadTransaction>.Register(e => e.MANTR);

        /// <summary>
        /// SAP物料号
        /// </summary>
        public string MANTR
        {
            get { return this.GetProperty(MANTRProperty); }
            set { this.SetProperty(MANTRProperty, value); }
        }
        #endregion

        #region Sap工厂 WERKS
        /// <summary>
        /// Sap工厂
        /// </summary>
        [Label("Sap工厂")]
        public static readonly Property<string> WERKSProperty = P<UploadTransaction>.Register(e => e.WERKS);

        /// <summary>
        /// Sap工厂
        /// </summary>
        public string WERKS
        {
            get { return this.GetProperty(WERKSProperty); }
            set { this.SetProperty(WERKSProperty, value); }
        }
        #endregion

        #region 数据Key DataKey
        /// <summary>
        /// 数据Key
        /// </summary>
        [Label("数据Key")]
        public static readonly Property<string> DataKeyProperty = P<UploadTransaction>.Register(e => e.DataKey);

        /// <summary>
        /// 数据Key
        /// </summary>
        public string DataKey
        {
            get { return this.GetProperty(DataKeyProperty); }
            set { this.SetProperty(DataKeyProperty, value); }
        }
        #endregion

        #region 单据Key OrdKey
        /// <summary>
        /// 单据Key
        /// </summary>
        [Label("单据Key")]
        public static readonly Property<string> OrdKeyProperty = P<UploadTransaction>.Register(e => e.OrdKey);

        /// <summary>
        /// 单据Key
        /// </summary>
        public string OrdKey
        {
            get { return this.GetProperty(OrdKeyProperty); }
            set { this.SetProperty(OrdKeyProperty, value); }
        }
        #endregion

        #region SAP事务ID Zuid
        /// <summary>
        /// SAP事务ID
        /// </summary>
        [Label("SAP事务ID")]
        public static readonly Property<string> ZuidProperty = P<UploadTransaction>.Register(e => e.Zuid);

        /// <summary>
        /// SAP事务ID
        /// </summary>
        public string Zuid
        {
            get { return this.GetProperty(ZuidProperty); }
            set { this.SetProperty(ZuidProperty, value); }
        }
        #endregion

        #region 工序流水码 Vornr
        /// <summary>
        /// 工序流水码
        /// </summary>
        [Label("工序流水码")]
        public static readonly Property<string> VornrProperty = P<UploadTransaction>.Register(e => e.Vornr);

        /// <summary>
        /// 工序流水码
        /// </summary>
        public string Vornr
        {
            get { return this.GetProperty(VornrProperty); }
            set { this.SetProperty(VornrProperty, value); }
        }
        #endregion

        #region 工序编码 ProcessCode
        /// <summary>
        /// 工序编码
        /// </summary>
        [Label("工序编码")]
        public static readonly Property<string> ProcessCodeProperty = P<UploadTransaction>.Register(e => e.ProcessCode);

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
            set { this.SetProperty(ProcessCodeProperty, value); }
        }
        #endregion

        #region 工作中心 WorkCenter
        /// <summary>
        /// 工作中心
        /// </summary>
        [Label("工作中心")]
        public static readonly Property<string> WorkCenterProperty = P<UploadTransaction>.Register(e => e.WorkCenter);

        /// <summary>
        /// 工作中心
        /// </summary>
        public string WorkCenter
        {
            get { return this.GetProperty(WorkCenterProperty); }
            set { this.SetProperty(WorkCenterProperty, value); }
        }
        #endregion

        #region 报工记录ID ReportRecordId
        /// <summary>
        /// 报工记录ID
        /// </summary>
        [Label("报工记录ID")]
        public static readonly Property<double?> ReportRecordIdProperty = P<UploadTransaction>.Register(e => e.ReportRecordId);

        /// <summary>
        /// 报工记录ID
        /// </summary>
        public double? ReportRecordId
        {
            get { return this.GetProperty(ReportRecordIdProperty); }
            set { this.SetProperty(ReportRecordIdProperty, value); }
        }
        #endregion

        #region 良品数量 OkQty
        /// <summary>
        /// 良品数量
        /// </summary>
        [Label("良品数量")]
        public static readonly Property<decimal?> OkQtyProperty = P<UploadTransaction>.Register(e => e.OkQty);

        /// <summary>
        /// 良品数量
        /// </summary>
        public decimal? OkQty
        {
            get { return this.GetProperty(OkQtyProperty); }
            set { this.SetProperty(OkQtyProperty, value); }
        }
        #endregion

        #region 不良品数量 NgQty
        /// <summary>
        /// 不良品数量
        /// </summary>
        [Label("不良品数量")]
        public static readonly Property<decimal?> NgQtyProperty = P<UploadTransaction>.Register(e => e.NgQty);

        /// <summary>
        /// 不良品数量
        /// </summary>
        public decimal? NgQty
        {
            get { return this.GetProperty(NgQtyProperty); }
            set { this.SetProperty(NgQtyProperty, value); }
        }
        #endregion

        #region 返工数量 ReworkQty
        /// <summary>
        /// 返工数量
        /// </summary>
        [Label("返工数量")]
        public static readonly Property<decimal?> ReworkQtyProperty = P<UploadTransaction>.Register(e => e.ReworkQty);

        /// <summary>
        /// 返工数量
        /// </summary>
        public decimal? ReworkQty
        {
            get { return this.GetProperty(ReworkQtyProperty); }
            set { this.SetProperty(ReworkQtyProperty, value); }
        }
        #endregion

        #region 可疑品数量 SuspectQty
        /// <summary>
        /// 可疑品数量
        /// </summary>
        [Label("可疑品数量")]
        public static readonly Property<decimal?> SuspectQtyProperty = P<UploadTransaction>.Register(e => e.SuspectQty);

        /// <summary>
        /// 可疑品数量
        /// </summary>
        public decimal? SuspectQty
        {
            get { return this.GetProperty(SuspectQtyProperty); }
            set { this.SetProperty(SuspectQtyProperty, value); }
        }
        #endregion

        #region 返回物料凭证 Mblnr
        /// <summary>
        /// 返回物料凭证
        /// </summary>
        [Label("返回物料凭证")]
        public static readonly Property<string> MblnrProperty = P<UploadTransaction>.Register(e => e.Mblnr);

        /// <summary>
        /// 返回物料凭证
        /// </summary>
        public string Mblnr
        {
            get { return this.GetProperty(MblnrProperty); }
            set { this.SetProperty(MblnrProperty, value); }
        }
        #endregion

        #region 返回物料凭证年度 Mjahr
        /// <summary>
        /// 返回物料凭证年度
        /// </summary>
        [Label("返回物料凭证年度")]
        public static readonly Property<string> MjahrProperty = P<UploadTransaction>.Register(e => e.Mjahr);

        /// <summary>
        /// 返回物料凭证年度
        /// </summary>
        public string Mjahr
        {
            get { return this.GetProperty(MjahrProperty); }
            set { this.SetProperty(MjahrProperty, value); }
        }
        #endregion

        #region 部门 Department
        /// <summary>
        /// 部门
        /// </summary>
        [Label("部门")]
        public static readonly Property<string> DepartmentProperty = P<UploadTransaction>.Register(e => e.Department);

        /// <summary>
        /// 部门
        /// </summary>
        public string Department
        {
            get { return this.GetProperty(DepartmentProperty); }
            set { this.SetProperty(DepartmentProperty, value); }
        }
        #endregion

        #region 生产版本 Version
        /// <summary>
        /// 生产版本
        /// </summary>
        [Label("生产版本")]
        public static readonly Property<string> VersionProperty = P<UploadTransaction>.Register(e => e.Version);

        /// <summary>
        /// 生产版本
        /// </summary>
        public string Version
        {
            get { return this.GetProperty(VersionProperty); }
            set { this.SetProperty(VersionProperty, value); }
        }
        #endregion

        #endregion

    }

    /// <summary>
    /// 事务上传 实体配置
    /// </summary>
    internal class UploadTransactionConfig : EntityConfig<UploadTransaction>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(UploadTransaction.ValidateMessageProperty, new StringLengthRangeRule() { Max = 1000 });
            rules.AddRule(UploadTransaction.ProcessMessageProperty, new StringLengthRangeRule() { Max = 1000 });
            rules.AddRule(UploadTransaction.RemarkProperty, new StringLengthRangeRule() { Max = 1000 });
        }

        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("UPLOAD_TRANS").MapAllProperties();
            Meta.Property(UploadTransaction.ProcessMessageProperty).ColumnMeta.HasLength(1000);
            Meta.Property(UploadTransaction.ValidateMessageProperty).ColumnMeta.HasLength(1000);
            Meta.Property(UploadTransaction.ProcessMessageProperty).ColumnMeta.HasLength(1000);
            Meta.Property(UploadTransaction.RemarkProperty).ColumnMeta.HasLength(1000);
            Meta.Property(UploadTransaction.OrdKeyProperty).ColumnMeta.HasIndex();
            Meta.Property(UploadTransaction.SourceIdProperty).ColumnMeta.HasLength(1000);
            Meta.Property(UploadTransaction.ShortDescriptionProperty).DontMapColumn();
            Meta.Property(UploadTransaction.BismtProperty).DontMapColumn();
            Meta.Property(UploadTransaction.WorkShopNameProperty).DontMapColumn();
            Meta.Property(UploadTransaction.WorkShopCodeProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}