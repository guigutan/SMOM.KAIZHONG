using SIE.Core.Items;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;

namespace SIE.MES.DashBoard.WorkOrderReachs
{
    /// <summary>
    /// 工单达成率明细
    /// </summary>
    [RootEntity, Serializable]
    [Label("工单达成率明细")]
    public class WoReachDetailViewModel : ViewModel
    {
        #region 工单号 No
        /// <summary>
        /// 工单号
        /// </summary>      
        [Label("工单编号")]
        public static readonly Property<string> NoProperty = P<WoReachDetailViewModel>.Register(e => e.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 计划开始时间 PlanBeginDate
        /// <summary>
        /// 计划开始时间
        /// </summary>
        [Label("计划开始时间")]
        public static readonly Property<DateTime> PlanBeginDateProperty = P<WoReachDetailViewModel>.Register(e => e.PlanBeginDate, new PropertyMetadata<DateTime>()
        {
            DateTimePart = DateTimePart.Date,
        });

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime PlanBeginDate
        {
            get { return GetProperty(PlanBeginDateProperty); }
            set { SetProperty(PlanBeginDateProperty, value); }
        }
        #endregion

        #region 计划完成时间 PlanEndDate
        /// <summary>
        /// 计划完成时间
        /// </summary>
        [Label("计划完成时间")]
        public static readonly Property<DateTime> PlanEndDateProperty = P<WoReachDetailViewModel>.Register(e => e.PlanEndDate, new PropertyMetadata<DateTime>()
        {
            DateTimePart = DateTimePart.Date,
        });

        /// <summary>
        /// 计划完成时间
        /// </summary>
        public DateTime PlanEndDate
        {
            get { return GetProperty(PlanEndDateProperty); }
            set { SetProperty(PlanEndDateProperty, value); }
        }
        #endregion

        #region 计划数量 PlanQty
        /// <summary>
        /// 计划数量
        /// </summary>        
        [Label("计划数量")]
        public static readonly Property<decimal> PlanQtyProperty = P<WoReachDetailViewModel>.Register(e => e.PlanQty);

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanQty
        {
            get { return GetProperty(PlanQtyProperty); }
            set { SetProperty(PlanQtyProperty, value); }
        }
        #endregion

        #region 实际完成时间 ActuFinishDate
        /// <summary>
        /// 实际完成时间
        /// </summary>
        [Label("实际完成时间")]
        public static readonly Property<DateTime?> ActuFinishDateProperty = P<WoReachDetailViewModel>.Register(e => e.ActuFinishDate);

        /// <summary>
        /// 实际完成时间
        /// </summary>
        public DateTime? ActuFinishDate
        {
            get { return GetProperty(ActuFinishDateProperty); }
            set { SetProperty(ActuFinishDateProperty, value); }
        }
        #endregion

        #region 产品 Product
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品编码")]
        public static readonly IRefIdProperty ProductIdProperty = P<WoReachDetailViewModel>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ProductId
        {
            get { return (double)GetRefNullableId(ProductIdProperty); }
            set { SetRefNullableId(ProductIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public static readonly RefEntityProperty<Item> ProductProperty = P<WoReachDetailViewModel>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Product
        {
            get { return GetRefEntity(ProductProperty); }
            set { SetRefEntity(ProductProperty, value); }
        }
        #endregion

        #region 工单状态 State
        /// <summary>
        /// 工单状态
        /// </summary>
        [Label("工单状态")]
        public static readonly Property<WorkOrderState> StateProperty = P<WoReachDetailViewModel>.Register(e => e.State);

        /// <summary>
        /// 工单状态
        /// </summary>
        public WorkOrderState State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty = P<WoReachDetailViewModel>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)GetRefNullableId(ResourceIdProperty); }
            set { SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        [Label("资源")]
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<WoReachDetailViewModel>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
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
            P<WoReachDetailViewModel>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

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
            P<WoReachDetailViewModel>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return this.GetRefEntity(WorkShopProperty); }
            set { this.SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 完工数量 FinishQty
        /// <summary>
        /// 完工数量
        /// </summary>       
        [Label("完工数量")]
        public static readonly Property<decimal> FinishQtyProperty = P<WoReachDetailViewModel>.Register(e => e.FinishQty);

        /// <summary>
        /// 完工数量
        /// </summary>
        public decimal FinishQty
        {
            get { return this.GetProperty(FinishQtyProperty); }
            set { this.SetProperty(FinishQtyProperty, value); }
        }
        #endregion

        #region 实际开始时间 ActuStartDate
        /// <summary>
        /// 实际开始时间
        /// </summary>
        [Label("实际开始时间")]
        public static readonly Property<DateTime?> ActuStartDateProperty = P<WoReachDetailViewModel>.Register(e => e.ActuStartDate);

        /// <summary>
        /// 实际开始时间
        /// </summary>
        public DateTime? ActuStartDate
        {
            get { return GetProperty(ActuStartDateProperty); }
            set { SetProperty(ActuStartDateProperty, value); }
        }
        #endregion

        #region 达成状态 ReachState
        /// <summary>
        /// 达成状态
        /// </summary>
        [Label("达成状态")]
        public static readonly Property<ReachState> ReachStateProperty = P<WoReachDetailViewModel>.Register(e => e.ReachState);

        /// <summary>
        /// 达成状态
        /// </summary>
        public ReachState ReachState
        {
            get { return GetProperty(ReachStateProperty); }
            set { SetProperty(ReachStateProperty, value); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<WoReachDetailViewModel>.Register(e => e.ProductCode);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
            set { this.SetProperty(ProductCodeProperty, value); }
        }
        #endregion

        #region 资源名称 ResourceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源")]
        public static readonly Property<string> ResourceNameProperty = P<WoReachDetailViewModel>.Register(e => e.ResourceName);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
            set { this.SetProperty(ResourceNameProperty, value); }
        }
        #endregion

        #region 车间名称 WorkShopName
        /// <summary>
        /// 车间名称
        /// </summary>
        [Label("车间")]
        public static readonly Property<string> WorkShopNameProperty = P<WoReachDetailViewModel>.Register(e => e.WorkShopName);

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkShopName
        {
            get { return this.GetProperty(WorkShopNameProperty); }
            set { this.SetProperty(WorkShopNameProperty, value); }
        }
        #endregion 
    }

    /// <summary>
    /// 达成状态
    /// </summary>
    public enum ReachState
    {
        /// <summary>
        /// 已超时
        /// </summary>
        [Label("已超时")]
        IsLate,

        /// <summary>
        /// 未超时
        /// </summary>
        [Label("未超时")]
        IsNotLate,
    }
}
