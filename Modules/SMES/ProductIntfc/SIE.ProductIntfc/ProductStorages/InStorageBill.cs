using SIE.Common.Configs;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.ProductIntfc.Configs;
using SIE.Warehouses;
using System;

namespace SIE.ProductIntfc.ProductStorages
{
    /// <summary>
    /// 成品入库单
    /// </summary>
    [RootEntity, Serializable]
    [Label("成品入库单")]
    public partial class InStorageBill : DataEntity
    {
        #region 成品入库单号 No
        /// <summary>
        /// 成品入库单号
        /// </summary>
        [Label("成品入库单号")]
        public static readonly Property<string> NoProperty = P<InStorageBill>.Register(e => e.No);

        /// <summary>
        /// 成品入库单号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 入库数量 Qty
        /// <summary>
        /// 入库数量
        /// </summary>
        [Label("入库数量")]
        public static readonly Property<decimal> QtyProperty = P<InStorageBill>.Register(e => e.Qty);

        /// <summary>
        /// 入库数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 接收时间 ReceiveDate
        /// <summary>
        /// 接收时间
        /// </summary>
        [Label("接收时间")]
        public static readonly Property<DateTime?> ReceiveDateProperty = P<InStorageBill>.Register(e => e.ReceiveDate);

        /// <summary>
        /// 接收时间
        /// </summary>
        public DateTime? ReceiveDate
        {
            get { return GetProperty(ReceiveDateProperty); }
            set { SetProperty(ReceiveDateProperty, value); }
        }
        #endregion

        #region 接收状态 ReceiveState
        /// <summary>
        /// 接收状态
        /// </summary>
        [Label("接收状态")]
        public static readonly Property<ReceiveState> ReceiveStateProperty = P<InStorageBill>.Register(e => e.ReceiveState);

        /// <summary>
        /// 接收状态
        /// </summary>
        public ReceiveState ReceiveState
        {
            get { return GetProperty(ReceiveStateProperty); }
            set { SetProperty(ReceiveStateProperty, value); }
        }
        #endregion

        #region 入库工单 StorageWorkOrder
        /// <summary>
        /// 入库工单Id
        /// </summary>
        public static readonly IRefIdProperty StorageWorkOrderIdProperty = P<InStorageBill>.RegisterRefId(e => e.StorageWorkOrderId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<StorageWorkOrder> StorageWorkOrderProperty = P<InStorageBill>.RegisterRef(e => e.StorageWorkOrder, StorageWorkOrderIdProperty);

        /// <summary>
        /// 入库工单
        /// </summary>
        public StorageWorkOrder StorageWorkOrder
        {
            get { return GetRefEntity(StorageWorkOrderProperty); }
            set { SetRefEntity(StorageWorkOrderProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        public static readonly IRefIdProperty WarehouseIdProperty = P<InStorageBill>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<InStorageBill>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 入库明细列表 InStorageBarcodeDetailList
        /// <summary>
        /// 入库明细列表
        /// </summary>
        [Label("入库明细")]
        public static readonly ListProperty<EntityList<InStorageBarcodeDetail>> InStorageBarcodeDetailListProperty = P<InStorageBill>.RegisterList(e => e.InStorageBarcodeDetailList);

        /// <summary>
        /// 入库明细列表
        /// </summary>
        public EntityList<InStorageBarcodeDetail> InStorageBarcodeDetailList
        {
            get { return this.GetLazyList(InStorageBarcodeDetailListProperty); }
        }
        #endregion

        #region ASN单号 ASN
        /// <summary>
        /// WMS 回写的 ASN单号
        /// </summary>
        [Label("ASN单号")]
        public static readonly Property<string> AsnNoProperty = P<InStorageBill>.Register(e => e.AsnNo);

        /// <summary>
        /// WMS 回写的 ASN单号
        /// </summary>
        public string AsnNo
        {
            get { return this.GetProperty(AsnNoProperty); }
            set { this.SetProperty(AsnNoProperty, value); }
        }
        #endregion


        #region 视图属性

        #region 物料编号 ItemCode
        /// <summary>
        /// 物料编号
        /// </summary>
        [Label("物料编号")]
        public static readonly Property<string> ItemCodeProperty = P<InStorageBill>.RegisterView(e => e.ItemCode, p => p.StorageWorkOrder.Product.Code);

        /// <summary>
        /// 物料编号
        /// </summary>
        public string ItemCode => GetProperty(ItemCodeProperty);
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<InStorageBill>.RegisterView(e => e.ItemName, p => p.StorageWorkOrder.Product.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName => GetProperty(ItemNameProperty);
        #endregion

        #region 型号规格 SpecificationModel
        /// <summary>
        /// 型号规格
        /// </summary>
        [Label("型号规格")]
        public static readonly Property<string> SpecificationModelProperty = P<InStorageBill>.RegisterView(e => e.SpecificationModel, p => p.StorageWorkOrder.Product.SpecificationModel);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string SpecificationModel => GetProperty(SpecificationModelProperty);
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<InStorageBill>.RegisterView(e => e.WorkOrderNo, p => p.StorageWorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo => GetProperty(WorkOrderNoProperty);
        #endregion


        #region 物料单位 ItemUnit
        /// <summary>
        /// 物料单位
        /// </summary>
        [Label("物料单位")]
        public static readonly Property<string> ItemUnitProperty = P<InStorageBill>.RegisterView(e => e.ItemUnit, p => p.StorageWorkOrder.Product.Unit.Name);

        /// <summary>
        /// 物料单位
        /// </summary>
        public string ItemUnit
        {
            get { return this.GetProperty(ItemUnitProperty); }
        }
        #endregion

        #endregion

    }

    /// <summary>
    /// 成品入库单 实体配置
    /// </summary>
    internal class InStorageBillConfig : EntityConfig<InStorageBill>
    {
        /// <summary>
        /// 数据表Config
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INF_IN_STO_BILL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}