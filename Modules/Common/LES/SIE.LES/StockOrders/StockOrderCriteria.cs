using SIE.Domain;
using SIE.LES.Commons;
using SIE.LES.StockOrders.Service;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;

namespace SIE.LES.StockOrders
{
    /// <summary>
    /// 备料单查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("备料单查询实体")]
    public class StockOrderCriteria : Criteria
    {
        #region 备料单号 No
        /// <summary>
        /// 备料单号
        /// </summary>
        [Label("备料单号")]
        public static readonly Property<string> NoProperty = P<StockOrderCriteria>.Register(e => e.No);

        /// <summary>
        /// 备料单号
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 工单 WorkOrderNo
        /// <summary>
        /// 工单
        /// </summary>
        [Label("工单")]
        public static readonly Property<string> WorkOrderNoProperty = P<StockOrderCriteria>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
            set { this.SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 计划开始时间 PlanBeginTime
        /// <summary>
        /// 计划开始时间
        /// </summary>
        [Label("计划开始时间")]
        public static readonly Property<DateRange> PlanBeginTimeProperty = P<StockOrderCriteria>.Register(e => e.PlanBeginTime);

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateRange PlanBeginTime
        {
            get { return this.GetProperty(PlanBeginTimeProperty); }
            set { this.SetProperty(PlanBeginTimeProperty, value); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<StockOrderCriteria>.Register(e => e.ProductCode);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
            set { this.SetProperty(ProductCodeProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<StockOrderCriteria>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
            set { this.SetProperty(ItemCodeProperty, value); }
        }
        #endregion


        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<StockOrderCriteria>.Register(e => e.ProductName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
            set { this.SetProperty(ProductNameProperty, value); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间ID
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkShopIdProperty =
            P<StockOrderCriteria>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间ID
        /// </summary>
        public double? WorkShopId
        {
            get { return (double?)this.GetRefNullableId(WorkShopIdProperty); }
            set { this.SetRefNullableId(WorkShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        [Label("车间")]
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty =
            P<StockOrderCriteria>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return this.GetRefEntity(WorkShopProperty); }
            set { this.SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 生产资源 Resource
        /// <summary>
        /// 生产资源Id
        /// </summary>
        [Label("生产资源")]
        public static readonly IRefIdProperty ResourceIdProperty = P<StockOrderCriteria>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<StockOrderCriteria>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 生产资源
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 单据状态 StockState
        /// <summary>
        /// 单据状态
        /// </summary>
        [Label("单据状态")]
        public static readonly Property<string> StockStateProperty = P<StockOrderCriteria>.Register(e => e.StockState);

        /// <summary>
        /// 单据状态
        /// </summary>
        public string StockState
        {
            get { return GetProperty(StockStateProperty); }
            set { SetProperty(StockStateProperty, value); }
        }
        #endregion

        #region 单据来源 BillSource
        /// <summary>
        /// 单据来源
        /// </summary>
        [Label("单据来源")]
        public static readonly Property<BillSource?> BillSourceProperty = P<StockOrderCriteria>.Register(e => e.BillSource);

        /// <summary>
        /// 单据来源
        /// </summary>
        public BillSource? BillSource
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
        public static readonly Property<PrepareItemType?> StockTypeProperty = P<StockOrderCriteria>.Register(e => e.StockType);

        /// <summary>
        /// 备料模式
        /// </summary>
        public PrepareItemType? StockType
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
        public static readonly Property<TriggerMode?> TriggerModeProperty = P<StockOrderCriteria>.Register(e => e.TriggerMode);

        /// <summary>
        /// 触发方式
        /// </summary>
        public TriggerMode? TriggerMode
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
        public static readonly Property<DemandMode?> DemandModeProperty = P<StockOrderCriteria>.Register(e => e.DemandMode);

        /// <summary>
        /// 需求计算方式
        /// </summary>
        public DemandMode? DemandMode
        {
            get { return GetProperty(DemandModeProperty); }
            set { SetProperty(DemandModeProperty, value); }
        }
        #endregion

        #region 创建日期 CreateDate
        /// <summary>
        /// 创建日期
        /// </summary>
        [Label("创建日期")]
        public static readonly Property<DateRange> CreateDateProperty = P<StockOrderCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateRange CreateDate
        {
            get { return this.GetProperty(CreateDateProperty); }
            set { this.SetProperty(CreateDateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<StockOrderService>().GetStockOrders(this);
        }
    }
}
