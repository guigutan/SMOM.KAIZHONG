using SIE.Domain;
using SIE.Items;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Tech.Stations;
using System;

namespace SIE.Kit.MES.CallMaterials
{
    /// <summary>
    /// 物料接收
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("物料接收")]
    [DisplayMember(nameof(Label))]
    public partial class CallMaterialReceive : DataEntity
    {
        #region 标签号 Label
        /// <summary>
        /// 标签号
        /// </summary>
        [Label("标签号")]
        [Required]
        public static readonly Property<string> LabelProperty = P<CallMaterialReceive>.Register(e => e.Label);

        /// <summary>
        /// 标签号
        /// </summary>
        public string Label
        {
            get { return GetProperty(LabelProperty); }
            set { SetProperty(LabelProperty, value); }
        }
        #endregion

        #region 物料批次 BatchNo
        /// <summary>
        /// 物料批次
        /// </summary>
        [Label("物料批次")]
        public static readonly Property<string> BatchNoProperty = P<CallMaterialReceive>.Register(e => e.BatchNo);

        /// <summary>
        /// 物料批次
        /// </summary>
        public string BatchNo
        {
            get { return GetProperty(BatchNoProperty); }
            set { SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 接收数量 Qty
        /// <summary>
        /// 接收数量
        /// </summary>
        [Label("接收数量")]
        [MinValue(0)]
        public static readonly Property<decimal> QtyProperty = P<CallMaterialReceive>.Register(e => e.Qty);

        /// <summary>
        /// 接收数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 是否已上料 IsLoadItem
        /// <summary>
        /// 是否已上料
        /// </summary>
        [Label("是否已上料")]
        public static readonly Property<bool> IsLoadItemProperty = P<CallMaterialReceive>.Register(e => e.IsLoadItem);

        /// <summary>
        /// 是否已上料
        /// </summary>
        public bool IsLoadItem
        {
            get { return GetProperty(IsLoadItemProperty); }
            set { SetProperty(IsLoadItemProperty, value); }
        }
        #endregion

        #region 接收时间 ReceiveDate
        /// <summary>
        /// 接收时间
        /// </summary>
        [Label("接收时间")]
        public static readonly Property<DateTime> ReceiveDateProperty = P<CallMaterialReceive>.Register(e => e.ReceiveDate);

        /// <summary>
        /// 接收时间
        /// </summary>
        public DateTime ReceiveDate
        {
            get { return GetProperty(ReceiveDateProperty); }
            set { SetProperty(ReceiveDateProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<CallMaterialReceive>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Item> ItemProperty = P<CallMaterialReceive>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 配送工位 DistStation
        /// <summary>
        /// 配送工位Id
        /// </summary>
        [Label("配送工位")]
        public static readonly IRefIdProperty DistStationIdProperty = P<CallMaterialReceive>.RegisterRefId(e => e.DistStationId, ReferenceType.Normal);

        /// <summary>
        /// 配送工位Id
        /// </summary>
        public double DistStationId
        {
            get { return (double)GetRefId(DistStationIdProperty); }
            set { SetRefId(DistStationIdProperty, value); }
        }

        /// <summary>
        /// 配送工位
        /// </summary>
        public static readonly RefEntityProperty<Station> DistStationProperty = P<CallMaterialReceive>.RegisterRef(e => e.DistStation, DistStationIdProperty);

        /// <summary>
        /// 配送工位
        /// </summary>
        public Station DistStation
        {
            get { return GetRefEntity(DistStationProperty); }
            set { SetRefEntity(DistStationProperty, value); }
        }
        #endregion

        #region 上料工位 LoadStation
        /// <summary>
        /// 上料工位Id
        /// </summary>
        [Label("上料工位")]
        public static readonly IRefIdProperty LoadStationIdProperty = P<CallMaterialReceive>.RegisterRefId(e => e.LoadStationId, ReferenceType.Normal);

        /// <summary>
        /// 上料工位Id
        /// </summary>
        public double? LoadStationId
        {
            get { return (double?)GetRefNullableId(LoadStationIdProperty); }
            set { SetRefNullableId(LoadStationIdProperty, value); }
        }

        /// <summary>
        /// 上料工位
        /// </summary>
        public static readonly RefEntityProperty<Station> LoadStationProperty = P<CallMaterialReceive>.RegisterRef(e => e.LoadStation, LoadStationIdProperty);

        /// <summary>
        /// 上料工位
        /// </summary>
        public Station LoadStation
        {
            get { return GetRefEntity(LoadStationProperty); }
            set { SetRefEntity(LoadStationProperty, value); }
        }
        #endregion

        #region 叫料单 CallMaterialBill
        /// <summary>
        /// 叫料单Id
        /// </summary>
        [Label("叫料单")]
        public static readonly IRefIdProperty CallMaterialBillIdProperty = P<CallMaterialReceive>.RegisterRefId(e => e.CallMaterialBillId, ReferenceType.Normal);

        /// <summary>
        /// 叫料单Id
        /// </summary>
        public double CallMaterialBillId
        {
            get { return (double)GetRefId(CallMaterialBillIdProperty); }
            set { SetRefId(CallMaterialBillIdProperty, value); }
        }

        /// <summary>
        /// 叫料单
        /// </summary>
        public static readonly RefEntityProperty<CallMaterialBill> CallMaterialBillProperty = P<CallMaterialReceive>.RegisterRef(e => e.CallMaterialBill, CallMaterialBillIdProperty);

        /// <summary>
        /// 叫料单
        /// </summary>
        public CallMaterialBill CallMaterialBill
        {
            get { return GetRefEntity(CallMaterialBillProperty); }
            set { SetRefEntity(CallMaterialBillProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<CallMaterialReceive>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId
        {
            get { return (double)GetRefId(WorkOrderIdProperty); }
            set { SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<CallMaterialReceive>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 接收人 ReceiveBy
        /// <summary>
        /// 接收人Id
        /// </summary>
        [Label("接收人")]
        public static readonly IRefIdProperty ReceiveByIdProperty = P<CallMaterialReceive>.RegisterRefId(e => e.ReceiveById, ReferenceType.Normal);

        /// <summary>
        /// 接收人Id
        /// </summary>
        public double ReceiveById
        {
            get { return (double)GetRefId(ReceiveByIdProperty); }
            set { SetRefId(ReceiveByIdProperty, value); }
        }

        /// <summary>
        /// 接收人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ReceiveByProperty = P<CallMaterialReceive>.RegisterRef(e => e.ReceiveBy, ReceiveByIdProperty);

        /// <summary>
        /// 接收人
        /// </summary>
        public Employee ReceiveBy
        {
            get { return GetRefEntity(ReceiveByProperty); }
            set { SetRefEntity(ReceiveByProperty, value); }
        }
        #endregion

        #region 所属资源 Resource
        /// <summary>
        /// 所属资源Id
        /// </summary>
        [Label("所属资源")]
        public static readonly IRefIdProperty ResourceIdProperty = P<CallMaterialReceive>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 所属资源Id
        /// </summary>
        public double ResourceId
        {
            get { return (double)GetRefId(ResourceIdProperty); }
            set { SetRefId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 所属资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<CallMaterialReceive>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 所属资源
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 剩余可用数量 RemainQty
        /// <summary>
        /// 剩余可用数量
        /// </summary>
        [Label("剩余可用数量")]
        public static readonly Property<decimal> RemainQtyProperty = P<CallMaterialReceive>.Register(e => e.RemainQty);

        /// <summary>
        /// 剩余可用数量
        /// </summary>
        public decimal RemainQty
        {
            get { return this.GetProperty(RemainQtyProperty); }
            set { this.SetProperty(RemainQtyProperty, value); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkShopIdProperty =
            P<CallMaterialReceive>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkShopId
        {
            get { return (double?)this.GetRefNullableId(WorkShopIdProperty); }
            set { this.SetRefNullableId(WorkShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty =
            P<CallMaterialReceive>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return this.GetRefEntity(WorkShopProperty); }
            set { this.SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<CallMaterialReceive>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<CallMaterialReceive>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 规格型号 ItemSpecification
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> ItemSpecificationProperty = P<CallMaterialReceive>.RegisterView(e => e.ItemSpecification, p => p.Item.SpecificationModel);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string ItemSpecification
        {
            get { return this.GetProperty(ItemSpecificationProperty); }
        }
        #endregion

        #region 车间名称 WorkShopName
        /// <summary>
        /// 车间名称
        /// </summary>
        [Label("车间名称")]
        public static readonly Property<string> WorkShopNameProperty = P<CallMaterialReceive>.RegisterView(e => e.WorkShopName, p => p.WorkShop.Name);

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkShopName
        {
            get { return this.GetProperty(WorkShopNameProperty); }
        }
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<CallMaterialReceive>.RegisterView(e => e.WorkOrderNo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
        }
        #endregion

        #region 叫料单号 BillNo
        /// <summary>
        /// 叫料单号
        /// </summary>
        [Label("叫料单号")]
        public static readonly Property<string> BillNoProperty = P<CallMaterialReceive>.RegisterView(e => e.BillNo, p => p.CallMaterialBill.No);

        /// <summary>
        /// 叫料单号
        /// </summary>
        public string BillNo
        {
            get { return this.GetProperty(BillNoProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 物料接收 实体配置
    /// </summary>
    internal class CallMaterialReceiveConfig : EntityConfig<CallMaterialReceive>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_CALL_RECEIVE").MapAllProperties();
            Meta.Property(CallMaterialReceive.LabelProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}