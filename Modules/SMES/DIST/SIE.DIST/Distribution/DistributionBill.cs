using SIE.Common.Configs;
using SIE.Core.WorkOrders;
using SIE.DIST.Distribution;
using SIE.DIST.Distribution.Configs;
using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using SIE.Resources.ShiftTypes;
using SIE.Resources.WipResources;
using System;

namespace SIE.DIST
{
    /// <summary>
    /// 配送单
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(DistributionBillCriteria))]
    [EntityWithConfig(typeof(ReturnBillNoConfig))]
    [EntityWithConfig(typeof(BillLabelConfig))]
    [EntityWithConfig(typeof(BillNoConfig))]
    [Label("扫描明细")]
    [DisplayMember(nameof(DistributionBill.ContainerNo))]
    public partial class DistributionBill : DataEntity
    {
        #region 载具号 ContainerNo
        /// <summary>
        /// 载具号
        /// </summary>
        [Required]
        [MaxLength(40)]
        [Label("载具号")]
        public static readonly Property<string> ContainerNoProperty = P<DistributionBill>.Register(e => e.ContainerNo);

        /// <summary>
        /// 载具号
        /// </summary>
        public string ContainerNo
        {
            get { return GetProperty(ContainerNoProperty); }
            set { SetProperty(ContainerNoProperty, value); }
        }
        #endregion

        #region 配送单号 No
        /// <summary>
        /// 配送单号
        /// </summary>
        [Label("配送单号")]
        public static readonly Property<string> NoProperty = P<DistributionBill>.Register(e => e.No);

        /// <summary>
        /// 配送单号
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        [Required]
        public static readonly Property<decimal> QtyProperty = P<DistributionBill>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 良品数量 OkQty
        /// <summary>
        /// 良品数量
        /// </summary>
        [Required]
        [Label("良品数量")]
        public static readonly Property<decimal> OkQtyProperty = P<DistributionBill>.Register(e => e.OkQty);

        /// <summary>
        /// 良品数量
        /// </summary>
        public decimal OkQty
        {
            get { return GetProperty(OkQtyProperty); }
            set { SetProperty(OkQtyProperty, value); }
        }
        #endregion        

        #region 正常退料数量 ReturnQty
        /// <summary>
        /// 正常退料数量
        /// </summary>
        [Label("正常退料数量")]
        public static readonly Property<decimal> ReturnQtyProperty = P<DistributionBill>.Register(e => e.ReturnQty);

        /// <summary>
        /// 正常退料数量
        /// </summary>
        public decimal ReturnQty
        {
            get { return this.GetProperty(ReturnQtyProperty); }
            set { this.SetProperty(ReturnQtyProperty, value); }
        }
        #endregion

        #region 不良退料数量 NgReturnQty
        /// <summary>
        /// 不良退料数量
        /// </summary>
        [Label("不良退料数量")]
        public static readonly Property<decimal> NgReturnQtyProperty = P<DistributionBill>.Register(e => e.NgReturnQty);

        /// <summary>
        /// 不良退料数量
        /// </summary>
        public decimal NgReturnQty
        {
            get { return this.GetProperty(NgReturnQtyProperty); }
            set { this.SetProperty(NgReturnQtyProperty, value); }
        }
        #endregion

        #region 打印数量 PrintQty
        /// <summary>
        /// 打印数量
        /// </summary>
        [Label("已打印数量")]
        public static readonly Property<decimal> PrintQtyProperty = P<DistributionBill>.Register(e => e.PrintQty);

        /// <summary>
        /// 打印数量
        /// </summary>
        public decimal PrintQty
        {
            get { return this.GetProperty(PrintQtyProperty); }
            set { this.SetProperty(PrintQtyProperty, value); }
        }
        #endregion

        #region 剩余数量 RemainderQty
        /// <summary>
        /// 剩余数量
        /// </summary>
        [Label("剩余数量")]
        [Required]
        public static readonly Property<decimal> RemainderQtyProperty = P<DistributionBill>.Register(e => e.RemainderQty);

        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal RemainderQty
        {
            get { return GetProperty(RemainderQtyProperty); }
            set { SetProperty(RemainderQtyProperty, value); }
        }
        #endregion

        #region 绑定日期 BindingDate
        /// <summary>
        /// 绑定日期
        /// </summary>
        [Required]
        [Label("绑定日期")]
        public static readonly Property<DateTime> BindingDateProperty = P<DistributionBill>.Register(e => e.BindingDate);

        /// <summary>
        /// 绑定日期
        /// </summary>
        public DateTime BindingDate
        {
            get { return GetProperty(BindingDateProperty); }
            set { SetProperty(BindingDateProperty, value); }
        }
        #endregion

        #region 箱号列表 DetailList
        /// <summary>
        /// 箱号列表
        /// </summary>
        [Label("箱号列表")]
        public static readonly ListProperty<EntityList<DistributionBillDetail>> DetailListProperty = P<DistributionBill>.RegisterList(e => e.DetailList);

        /// <summary>
        /// 箱号列表
        /// </summary>
        public EntityList<DistributionBillDetail> DetailList
        {
            get { return this.GetLazyList(DetailListProperty); }
        }
        #endregion

        #region 属性值列表 PropertyValueList
        /// <summary>
        /// 属性值列表
        /// </summary>
        [Label("属性值列表")]
        public static readonly ListProperty<EntityList<DistributionBillPropertyValue>> PropertyValueListProperty = P<DistributionBill>.RegisterList(e => e.PropertyValueList);

        /// <summary>
        /// 属性值列表
        /// </summary>
        public EntityList<DistributionBillPropertyValue> PropertyValueList
        {
            get { return this.GetLazyList(PropertyValueListProperty); }
        }
        #endregion

        #region 班次 Shift
        /// <summary>
        /// 班次Id
        /// </summary>
        [Label("班次")]
        public static readonly IRefIdProperty ShiftIdProperty = P<DistributionBill>.RegisterRefId(e => e.ShiftId, ReferenceType.Normal);

        /// <summary>
        /// 班次Id
        /// </summary>
        public double ShiftId
        {
            get { return (double)GetRefId(ShiftIdProperty); }
            set { SetRefId(ShiftIdProperty, value); }
        }

        /// <summary>
        /// 班次
        /// </summary>
        public static readonly RefEntityProperty<Shift> ShiftProperty = P<DistributionBill>.RegisterRef(e => e.Shift, ShiftIdProperty);

        /// <summary>
        /// 班次
        /// </summary>
        public Shift Shift
        {
            get { return GetRefEntity(ShiftProperty); }
            set { SetRefEntity(ShiftProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<DistributionBill>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<DistributionBill>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty = P<DistributionBill>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源
        /// </summary>
        public double ResourceId
        {
            get { return (double)GetRefId(ResourceIdProperty); }
            set { SetRefId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<DistributionBill>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<DistributionBill>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Item> ItemProperty = P<DistributionBill>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 绑定人 BindingBy
        /// <summary>
        /// 绑定人Id
        /// </summary>
        [Label("绑定人")]
        public static readonly IRefIdProperty BindingByIdProperty = P<DistributionBill>.RegisterRefId(e => e.BindingById, ReferenceType.Normal);

        /// <summary>
        /// 绑定人Id
        /// </summary>
        public double BindingById
        {
            get { return (double)GetRefId(BindingByIdProperty); }
            set { SetRefId(BindingByIdProperty, value); }
        }

        /// <summary>
        /// 绑定人
        /// </summary>
        public static readonly RefEntityProperty<Employee> BindingByProperty = P<DistributionBill>.RegisterRef(e => e.BindingBy, BindingByIdProperty);

        /// <summary>
        /// 绑定人
        /// </summary>
        public Employee BindingBy
        {
            get { return GetRefEntity(BindingByProperty); }
            set { SetRefEntity(BindingByProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<DistributionState> StateProperty = P<DistributionBill>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public DistributionState State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 来源ID SourceId
        /// <summary>
        /// 来源ID
        /// </summary>
        [Label("来源")]
        public static readonly Property<double?> SourceIdProperty = P<DistributionBill>.Register(e => e.SourceId);

        /// <summary>
        /// 来源ID
        /// </summary>
        public double? SourceId
        {
            get { return this.GetProperty(SourceIdProperty); }
            set { this.SetProperty(SourceIdProperty, value); }
        }
        #endregion

        #region 来源类型 SourceType
        /// <summary>
        /// 来源类型
        /// </summary>
        [Label("来源类型")]
        public static readonly Property<string> SourceTypeProperty = P<DistributionBill>.Register(e => e.SourceType);

        /// <summary>
        /// 来源类型
        /// </summary>
        public string SourceType
        {
            get { return this.GetProperty(SourceTypeProperty); }
            set { this.SetProperty(SourceTypeProperty, value); }
        }
        #endregion

        #region 注册视图属性(关联实体属性平铺显示，一般用于Web)

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<DistributionBill>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<DistributionBill>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 工单 WorkOrderNo
        /// <summary>
        /// 工单
        /// </summary>
        [Label("工单")]
        public static readonly Property<string> WorkOrderNoProperty = P<DistributionBill>.RegisterView(e => e.WorkOrderNo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    ///  实体配置
    /// </summary>
    internal class DistributionBillConfig : EntityConfig<DistributionBill>
    {
        /// <summary>
        /// Meta属性配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_DIST_BILL").MapAllProperties();
            Meta.Property(DistributionBill.SourceIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}