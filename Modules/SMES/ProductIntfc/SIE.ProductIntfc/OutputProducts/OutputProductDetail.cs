using SIE.Common.Configs;
using SIE.Domain;
using SIE.Items;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.ProductIntfc.Configs;
using SIE.Resources;
using SIE.Warehouses;
using System;

namespace SIE.ProductIntfc.OutputProducts
{
    /// <summary>
    /// 联/副产品入库单
    /// </summary>
    [ChildEntity, Serializable]
    [Label("联/副产品入库单")]
    public partial class OutputProductDetail : DataEntity
    {
        #region 入库单号 NO
        /// <summary>
        /// 入库单号
        /// </summary>
        [Label("入库单号")]
        public static readonly Property<string> NOProperty = P<OutputProductDetail>.Register(e => e.NO);

        /// <summary>
        /// 入库单号
        /// </summary>
        public string NO
        {
            get { return this.GetProperty(NOProperty); }
            set { this.SetProperty(NOProperty, value); }
        }
        #endregion

        #region 产出类型 OutPutType
        /// <summary>
        /// 产出类型
        /// </summary>
        [Label("产出类型")]
        public static readonly Property<OutputListType> OutPutTypeProperty = P<OutputProductDetail>.Register(e => e.OutPutType);

        /// <summary>
        /// 产出类型
        /// </summary>
        public OutputListType OutPutType
        {
            get { return this.GetProperty(OutPutTypeProperty); }
            set { this.SetProperty(OutPutTypeProperty, value); }
        }
        #endregion

        #region 工单产出物 WorkOrderOutputProduct
        /// <summary>
        /// 工单产出物Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty WorkOrderOutputProductIdProperty =
            P<OutputProductDetail>.RegisterRefId(e => e.WorkOrderOutputProductId, ReferenceType.Normal);

        /// <summary>
        /// 工单产出物Id
        /// </summary>
        public double WorkOrderOutputProductId
        {
            get { return (double)this.GetRefId(WorkOrderOutputProductIdProperty); }
            set { this.SetRefId(WorkOrderOutputProductIdProperty, value); }
        }

        /// <summary>
        /// 工单产出物
        /// </summary>
        public static readonly RefEntityProperty<WorkOrderOutputProduct> WorkOrderOutputProductProperty =
            P<OutputProductDetail>.RegisterRef(e => e.WorkOrderOutputProduct, WorkOrderOutputProductIdProperty);

        /// <summary>
        /// 工单产出物
        /// </summary>
        public WorkOrderOutputProduct WorkOrderOutputProduct
        {
            get { return this.GetRefEntity(WorkOrderOutputProductProperty); }
            set { this.SetRefEntity(WorkOrderOutputProductProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<OutputProductDetail>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return (double)this.GetRefId(ItemIdProperty); }
            set { this.SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<OutputProductDetail>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 扩展属性 ItemExtPropValue
        /// <summary>
        /// 扩展属性
        /// </summary>
        [Label("扩展属性")]
        public static readonly Property<string> ItemExtPropNameProperty = P<OutputProductDetail>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ItemExtPropName
        {
            get { return this.GetProperty(ItemExtPropNameProperty); }
            set { this.SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 扩展属性 ItemExtProp
        /// <summary>
        /// 扩展属性
        /// </summary>
        [Label("扩展属性")]
        public static readonly Property<string> ItemExtPropProperty = P<OutputProductDetail>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get { return this.GetProperty(ItemExtPropProperty); }
            set { this.SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 入库数量 InStorageQty
        /// <summary>
        /// 入库数量
        /// </summary>
        [Label("入库数量")]
        public static readonly Property<decimal> InStorageQtyProperty = P<OutputProductDetail>.Register(e => e.InStorageQty);

        /// <summary>
        /// 入库数量
        /// </summary>
        public decimal InStorageQty
        {
            get { return this.GetProperty(InStorageQtyProperty); }
            set { this.SetProperty(InStorageQtyProperty, value); }
        }
        #endregion

        #region 入库条码 Barcode
        /// <summary>
        /// 入库条码
        /// </summary>
        [Label("入库条码")]
        public static readonly Property<string> BarcodeProperty = P<OutputProductDetail>.Register(e => e.Barcode);

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode
        {
            get { return this.GetProperty(BarcodeProperty); }
            set { this.SetProperty(BarcodeProperty, value); }
        }
        #endregion

        #region 生产批次 Lot
        /// <summary>
        /// 生产批次
        /// </summary>
        [Label("生产批次")]
        public static readonly Property<string> LotProperty = P<OutputProductDetail>.Register(e => e.Lot);

        /// <summary>
        /// 生产批次
        /// </summary>
        public string Lot
        {
            get { return this.GetProperty(LotProperty); }
            set { this.SetProperty(LotProperty, value); }
        }
        #endregion

        #region 入库操作人 Operator
        /// <summary>
        /// 入库操作人Id
        /// </summary>
        [Label("入库操作人")]
        public static readonly IRefIdProperty OperatorIdProperty =
            P<OutputProductDetail>.RegisterRefId(e => e.OperatorId, ReferenceType.Normal);

        /// <summary>
        /// 入库操作人Id
        /// </summary>
        public double? OperatorId
        {
            get { return (double?)this.GetRefNullableId(OperatorIdProperty); }
            set { this.SetRefNullableId(OperatorIdProperty, value); }
        }

        /// <summary>
        /// 入库操作人
        /// </summary>
        public static readonly RefEntityProperty<Employee> OperatorProperty =
            P<OutputProductDetail>.RegisterRef(e => e.Operator, OperatorIdProperty);

        /// <summary>
        /// 入库操作人
        /// </summary>
        public Employee Operator
        {
            get { return this.GetRefEntity(OperatorProperty); }
            set { this.SetRefEntity(OperatorProperty, value); }
        }
        #endregion

        #region 入库操作时间 OperatorTime
        /// <summary>
        /// 入库操作时间
        /// </summary>
        [Label("入库操作时间")]
        public static readonly Property<DateTime?> OperatorTimeProperty = P<OutputProductDetail>.Register(e => e.OperatorTime);

        /// <summary>
        /// 入库操作时间
        /// </summary>
        public DateTime? OperatorTime
        {
            get { return this.GetProperty(OperatorTimeProperty); }
            set { this.SetProperty(OperatorTimeProperty, value); }
        }
        #endregion


        #region 接收时间 ReceiveDate
        /// <summary>
        /// 接收时间
        /// </summary>
        [Label("接收时间")]
        public static readonly Property<DateTime?> ReceiveDateProperty = P<OutputProductDetail>.Register(e => e.ReceiveDate);

        /// <summary>
        /// 接收时间
        /// </summary>
        public DateTime? ReceiveDate
        {
            get { return GetProperty(ReceiveDateProperty); }
            set { SetProperty(ReceiveDateProperty, value); }
        }
        #endregion

        #region 接收情况 ReceiveState
        /// <summary>
        /// 接收情况
        /// </summary>
        [Label("接收情况")]
        public static readonly Property<ReceiveState?> ReceiveStateProperty = P<OutputProductDetail>.Register(e => e.ReceiveState);

        /// <summary>
        /// 接收状态
        /// </summary>
        public ReceiveState? ReceiveState
        {
            get { return GetProperty(ReceiveStateProperty); }
            set { SetProperty(ReceiveStateProperty, value); }
        }
        #endregion

        #region 入库工单 OutputProduct
        /// <summary>
        /// 入库工单Id
        /// </summary>
        public static readonly IRefIdProperty StorageWorkOrderIdProperty = P<OutputProductDetail>.RegisterRefId(e => e.StorageWorkOrderId, ReferenceType.Parent);

        /// <summary>
        /// 入库工单Id
        /// </summary>
        public double StorageWorkOrderId
        {
            get { return (double)GetRefId(StorageWorkOrderIdProperty); }
            set { SetRefId(StorageWorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 入库工单
        /// </summary>
        public static readonly RefEntityProperty<OutputProduct> StorageWorkOrderProperty = P<OutputProductDetail>.RegisterRef(e => e.StorageWorkOrder, StorageWorkOrderIdProperty);

        /// <summary>
        /// 入库工单
        /// </summary>
        public OutputProduct StorageWorkOrder
        {
            get { return GetRefEntity(StorageWorkOrderProperty); }
            set { SetRefEntity(StorageWorkOrderProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        public static readonly IRefIdProperty WarehouseIdProperty = P<OutputProductDetail>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<OutputProductDetail>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region ASN单号 ASN
        /// <summary>
        /// WMS 回写的 ASN单号
        /// </summary>
        [Label("ASN单号")]
        public static readonly Property<string> AsnNoProperty = P<OutputProductDetail>.Register(e => e.AsnNo);

        /// <summary>
        /// WMS 回写的 ASN单号
        /// </summary>
        public string AsnNo
        {
            get { return this.GetProperty(AsnNoProperty); }
            set { this.SetProperty(AsnNoProperty, value); }
        }
        #endregion

        #region 入库状态 InStorageState
        /// <summary>
        /// 入库状态
        /// </summary>
        [Label("入库状态")]
        public static readonly Property<InStorageState?> InStorageStateProperty = P<OutputProductDetail>.Register(e => e.InStorageState);

        /// <summary>
        /// 入库状态
        /// </summary>
        public InStorageState? InStorageState
        {
            get { return this.GetProperty(InStorageStateProperty); }
            set { this.SetProperty(InStorageStateProperty, value); }
        }
        #endregion


        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<OutputProductDetail>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 是批次控制 IsBatchCtrl
        /// <summary>
        /// 是批次控制
        /// </summary>
        [Label("是批次控制")]
        public static readonly Property<bool?> IsBatchCtrlProperty = P<OutputProductDetail>.Register(e => e.IsBatchCtrl);

        /// <summary>
        /// 是批次控制
        /// </summary>
        public bool? IsBatchCtrl
        {
            get { return this.GetProperty(IsBatchCtrlProperty); }
            set { this.SetProperty(IsBatchCtrlProperty, value); }
        }
        #endregion


        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<OutputProductDetail>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion


        #region 请求Id RequireId
        /// <summary>
        /// 请求Id 存储同一仓库为同一请求Id 后台字段
        /// </summary>
        [Label("请求Id")]
        public static readonly Property<double> RequireIdProperty = P<OutputProductDetail>.Register(e => e.RequireId);

        /// <summary>
        /// 请求Id
        /// </summary>
        public double RequireId
        {
            get { return this.GetProperty(RequireIdProperty); }
            set { this.SetProperty(RequireIdProperty, value); }
        }
        #endregion


    }

    /// <summary>
    /// 成品入库单 实体配置
    /// </summary>
    internal class InStorageBillConfig : EntityConfig<OutputProductDetail>
    {
        /// <summary>
        /// 数据表Config
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("OUTPUT_PRO_DTL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}