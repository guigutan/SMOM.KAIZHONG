using SIE.Domain;
using SIE.Items;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;

namespace SIE.MES.TaskManagement.Reports
{
    /// <summary>
    /// 派工任务查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("派工任务查询实体")]
    public class ReportDispatchTaskCriteria : Criteria
    {

        #region 任务单号 No
        /// <summary>
        /// 任务单号
        /// </summary>        
        [Label("任务单号")]
        public static readonly Property<string> NoProperty = P<ReportDispatchTaskCriteria>.Register(e => e.No);

        /// <summary>
        /// 任务单号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 产品 Product
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品")]
        public static readonly IRefIdProperty ProductIdProperty = P<ReportDispatchTaskCriteria>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double? ProductId
        {
            get { return (double?)GetRefNullableId(ProductIdProperty); }
            set { SetRefNullableId(ProductIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public static readonly RefEntityProperty<Item> ProductProperty = P<ReportDispatchTaskCriteria>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Product
        {
            get { return GetRefEntity(ProductProperty); }
            set { SetRefEntity(ProductProperty, value); }
        }
        #endregion        

        #region 资源 Resource
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty = P<ReportDispatchTaskCriteria>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<ReportDispatchTaskCriteria>.RegisterRef(e => e.Resource, ResourceIdProperty);

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
        public static readonly IRefIdProperty WorkShopIdProperty = P<ReportDispatchTaskCriteria>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty = P<ReportDispatchTaskCriteria>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return this.GetRefEntity(WorkShopProperty); }
            set { this.SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单编号")]
        public static readonly Property<string> WorkOrderNoProperty = P<ReportDispatchTaskCriteria>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单号
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
        public static readonly Property<DateRange> PlanBeginTimeProperty = P<ReportDispatchTaskCriteria>.Register(e => e.PlanBeginTime, new PropertyMetadata<DateRange>() { DateTimePart = DateTimePart.Date, });

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateRange PlanBeginTime
        {
            get { return GetProperty(PlanBeginTimeProperty); }
            set { SetProperty(PlanBeginTimeProperty, value); }
        }
        #endregion

        #region 计划完成时间 PlanEndTime
        /// <summary>
        /// 计划完成时间
        /// </summary>
        [Label("计划完成时间")]
        public static readonly Property<DateRange> PlanEndTimeProperty = P<ReportDispatchTaskCriteria>.Register(e => e.PlanEndTime, new PropertyMetadata<DateRange>() { DateTimePart = DateTimePart.Date, });

        /// <summary>
        /// 计划完成时间
        /// </summary>
        public DateRange PlanEndTime
        {
            get { return GetProperty(PlanEndTimeProperty); }
            set { SetProperty(PlanEndTimeProperty, value); }
        }
        #endregion

        #region 任务单状态 TaskStatus
        /// <summary>
        /// 任务单状态
        /// </summary>
        [Label("任务单状态")]
        public static readonly Property<DispatchTaskStatus?> TaskStatusProperty = P<ReportDispatchTaskCriteria>.Register(e => e.TaskStatus);

        /// <summary>
        /// 任务单状态
        /// </summary>
        public DispatchTaskStatus? TaskStatus
        {
            get { return GetProperty(TaskStatusProperty); }
            set { SetProperty(TaskStatusProperty, value); }
        }
        #endregion

        #region 报工方式 ReportMode
        /// <summary>
        /// 报工方式
        /// </summary>
        [Label("报工方式")]
        public static readonly Property<ReportMode?> ReportModeProperty = P<ReportDispatchTaskCriteria>.Register(e => e.ReportMode);

        /// <summary>
        /// 报工方式
        /// </summary>
        public ReportMode? ReportMode
        {
            get { return this.GetProperty(ReportModeProperty); }
            set { this.SetProperty(ReportModeProperty, value); }
        }
        #endregion

        #region 显示可报工 IsShowDispatchTask
        /// <summary>
        /// 显示可报工
        /// </summary>
        [Label("显示可报工")]
        public static readonly Property<bool> IsShowDispatchTaskProperty = P<ReportDispatchTaskCriteria>.Register(e => e.IsShowDispatchTask);

        /// <summary>
        /// 显示可报工
        /// </summary>
        public bool IsShowDispatchTask
        {
            get { return this.GetProperty(IsShowDispatchTaskProperty); }
            set { this.SetProperty(IsShowDispatchTaskProperty, value); }
        }
        #endregion

        #region 未派工、派工中 IsUnscheduledInProgress 
        /// <summary>
        /// 未派工、派工中
        /// </summary>
        [Label("显示未派工、派工中")]
        public static readonly Property<bool> IsUnscheduledInProgressProperty = P<ReportDispatchTaskCriteria>.Register(e => e.IsUnscheduledInProgress);

        /// <summary>
        /// 显示未派工、派工中
        /// </summary>
        public bool IsUnscheduledInProgress
        {
            get { return this.GetProperty(IsUnscheduledInProgressProperty); }
            set { this.SetProperty(IsUnscheduledInProgressProperty, value); }
        }
        #endregion

        /// <summary>
        /// 获取派工管理列表
        /// </summary>
        /// <returns>派工管理列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ReportController>().GetReportDispatchTaskList(this);
        }
    }
}
