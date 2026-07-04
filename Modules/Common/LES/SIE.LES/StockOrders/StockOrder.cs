using NPOI.Util;
using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.LES.Commons;
using SIE.LES.StockOrders.Configs;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;

namespace SIE.LES.StockOrders
{
    /// <summary>
    /// 备料单
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(StockOrderCriteria))]
    [EntityWithConfig(typeof(NoConfig))]
    [EntityWithConfig(typeof(StockOrderIsAuditConfig))]
    [EntityWithConfig(typeof(StockReceiveTypeConfig))]
    [EntityWithConfig(typeof(LimitedMaximumStockConfig))]
    //[EntityWithConfig(typeof(PushedSchedulingMethodsConfig))]
    [EntityWithConfig(typeof(SchedulingTriggeredStatusConfig))]
    [Label("备料单")]
    [DisplayMember(nameof(No))]
    public partial class StockOrder : DataEntity
    {
        #region 单号 No
        /// <summary>
        /// 单号
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("单号")]
        public static readonly Property<string> NoProperty = P<StockOrder>.Register(e => e.No);

        /// <summary>
        /// 单号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<StockOrder>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<StockOrder>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId
        {
            get { return (double?)GetRefNullableId(WorkOrderIdProperty); }
            set { SetRefNullableId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<StockOrder>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 单据来源 BillSource
        /// <summary>
        /// 单据来源
        /// </summary>
        [Label("单据来源")]
        public static readonly Property<BillSource> BillSourceProperty = P<StockOrder>.Register(e => e.BillSource);

        /// <summary>
        /// 单据来源
        /// </summary>
        public BillSource BillSource
        {
            get { return GetProperty(BillSourceProperty); }
            set { SetProperty(BillSourceProperty, value); }
        }
        #endregion

        #region 备料模式 StockType
        /// <summary>
        /// 备料模式
        /// </summary>
        [Label("备料模式")]
        public static readonly Property<PrepareItemType> StockTypeProperty = P<StockOrder>.Register(e => e.StockType);

        /// <summary>
        /// 备料模式
        /// </summary>
        public PrepareItemType StockType
        {
            get { return GetProperty(StockTypeProperty); }
            set { SetProperty(StockTypeProperty, value); }
        }
        #endregion

        #region 触发方式 TriggerMode
        /// <summary>
        /// 触发方式
        /// </summary>
        [Label("触发方式")]
        public static readonly Property<TriggerMode> TriggerModeProperty = P<StockOrder>.Register(e => e.TriggerMode);

        /// <summary>
        /// 触发方式
        /// </summary>
        public TriggerMode TriggerMode
        {
            get { return GetProperty(TriggerModeProperty); }
            set { SetProperty(TriggerModeProperty, value); }
        }
        #endregion

        #region 需求计算方式 DemandMode
        /// <summary>
        /// 需求计算方式
        /// </summary>
        [Label("需求计算方式")]
        public static readonly Property<DemandMode> DemandModeProperty = P<StockOrder>.Register(e => e.DemandMode);

        /// <summary>
        /// 需求计算方式
        /// </summary>
        public DemandMode DemandMode
        {
            get { return GetProperty(DemandModeProperty); }
            set { SetProperty(DemandModeProperty, value); }
        }
        #endregion
        #region 产品套数 DemandMode
        /// <summary>
        /// 产品套数
        /// </summary>
        [Label("产品套数")]
        public static readonly Property<decimal> NumberOfSetsProperty = P<StockOrder>.Register(e => e.NumberOfSets);

        /// <summary>
        /// 产品套数
        /// </summary>
        public decimal NumberOfSets
        {
            get { return GetProperty(NumberOfSetsProperty); }
            set { SetProperty(NumberOfSetsProperty, value); }
        }
        #endregion

        



        #region 备料状态 StockState
        /// <summary>
        /// 备料状态
        /// </summary>
        [Label("备料状态")]
        public static readonly Property<StockState> StockStateProperty = P<StockOrder>.Register(e => e.StockState);

        /// <summary>
        /// 备料状态
        /// </summary>
        public StockState StockState
        {
            get { return GetProperty(StockStateProperty); }
            set { SetProperty(StockStateProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        [Required]
        public static readonly IRefIdProperty FactoryIdProperty =
            P<StockOrder>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double FactoryId
        {
            get { return (double)this.GetRefId(FactoryIdProperty); }
            set { this.SetRefId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> FactoryProperty =
            P<StockOrder>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return this.GetRefEntity(FactoryProperty); }
            set { this.SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 生产资源 Resource
        /// <summary>
        /// 生产资源Id
        /// </summary>
        [Label("生产资源")]
        public static readonly IRefIdProperty ResourceIdProperty = P<StockOrder>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 生产资源Id
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)GetRefNullableId(ResourceIdProperty); }
            set { SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 生产资源
        /// </summary>
        [Label("生产资源")]
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<StockOrder>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 生产资源
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间ID
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkShopIdProperty =
            P<StockOrder>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间ID
        /// </summary>
        public double WorkShopId
        {
            get { return (double)GetRefId(WorkShopIdProperty); }
            set { SetRefId(WorkShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        [Label("车间")]
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty =
            P<StockOrder>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return this.GetRefEntity(WorkShopProperty); }
            set { this.SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 物料需求列表 StockOrderDetailList
        /// <summary>
        /// 物料需求列表
        /// </summary>
        [Label("物料需求")]
        public static readonly ListProperty<EntityList<StockOrderDetail>> StockOrderDetailListProperty = P<StockOrder>.RegisterList(e => e.StockOrderDetailList);

        /// <summary>
        /// 物料需求列表
        /// </summary>
        public EntityList<StockOrderDetail> StockOrderDetailList
        {
            get { return this.GetLazyList(StockOrderDetailListProperty); }
        }
        #endregion

        #region 视图属性
        #region 计划开始时间 PlanBeginDate
        /// <summary>
        /// 计划开始时间
        /// </summary>
        [Label("计划开始时间")]
        public static readonly Property<DateTime?> PlanBeginDateProperty = P<StockOrder>.RegisterView(e => e.PlanBeginDate, p => p.WorkOrder.PlanBeginDate);

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime? PlanBeginDate
        {
            get { return this.GetProperty(PlanBeginDateProperty); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<StockOrder>.RegisterView(e => e.ProductCode, p => p.WorkOrder.Product.Code);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<StockOrder>.RegisterView(e => e.ProductName, p => p.WorkOrder.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }
        #endregion

        #region 工单数量 WoQty
        /// <summary>
        /// 工单数量
        /// </summary>
        [Label("工单数量")]
        public static readonly Property<decimal> WoQtyProperty = P<StockOrder>.RegisterView(e => e.WoQty, p => p.WorkOrder.PlanQty);

        /// <summary>
        /// 工单数量
        /// </summary>
        public decimal WoQty
        {
            get { return this.GetProperty(WoQtyProperty); }
        }
        #endregion

        #region 工单号 WoNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WoNoProperty = P<StockOrder>.RegisterView(e => e.WoNo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNo
        {
            get { return this.GetProperty(WoNoProperty); }
        }
        #endregion

        #region 工厂名称 FactoryName
        /// <summary>
        /// 工厂名称
        /// </summary>
        [Label("工厂名称")]
        public static readonly Property<string> FactoryNameProperty = P<StockOrder>.RegisterView(e => e.FactoryName, p => p.Factory.Name);

        /// <summary>
        /// 工厂名称
        /// </summary>
        public string FactoryName
        {
            get { return this.GetProperty(FactoryNameProperty); }
        }
        #endregion

        #region 产线名称 ResourceName
        /// <summary>
        /// 产线名称
        /// </summary>
        [Label("产线名称")]
        public static readonly Property<string> ResourceNameProperty = P<StockOrder>.RegisterView(e => e.ResourceName, p => p.Resource.Name);

        /// <summary>
        /// 产线名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
        }
        #endregion

        #endregion

        #region 不映射数据栏位
        #region 推式需求计算方式 PushDemandMode
        /// <summary>
        /// 推式需求计算方式
        /// </summary>
        [Label("需求计算方式")]
        public static readonly Property<PushDemandMode> PushDemandModeProperty = P<StockOrder>.Register(e => e.PushDemandMode);

        /// <summary>
        /// 推式需求计算方式
        /// </summary>
        public PushDemandMode PushDemandMode
        {
            get { return GetProperty(PushDemandModeProperty); }
            set { SetProperty(PushDemandModeProperty, value); }
        }
        #endregion

        #region 拉式需求计算方式 PullDemandMode
        /// <summary>
        /// 拉式需求计算方式
        /// </summary>
        [Label("需求计算方式")]
        public static readonly Property<PullDemandMode> PullDemandModeProperty = P<StockOrder>.Register(e => e.PullDemandMode);

        /// <summary>
        /// 拉式需求计算方式
        /// </summary>
        public PullDemandMode PullDemandMode
        {
            get { return GetProperty(PullDemandModeProperty); }
            set { SetProperty(PullDemandModeProperty, value); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    ///  实体配置
    /// </summary>
    internal class StockOrderConfig : EntityConfig<StockOrder>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("STOCK_ORDER").MapAllProperties();
            Meta.Property(StockOrder.NumberOfSetsProperty).DontMapColumn();
            Meta.Property(StockOrder.PushDemandModeProperty).DontMapColumn();
            Meta.Property(StockOrder.PullDemandModeProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}