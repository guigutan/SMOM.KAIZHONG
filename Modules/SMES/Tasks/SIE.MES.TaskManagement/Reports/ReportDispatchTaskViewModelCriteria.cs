using SIE.Core.Items;
using SIE.Domain;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;

namespace SIE.MES.TaskManagement.Reports
{
    /// <summary>
    /// 生产任务统计报表查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("任务统计报表查询实体")]
    public class ReportDispatchTaskViewModelCriteria : Criteria
    {
        #region 资源 Resource
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty = P<ReportDispatchTaskViewModelCriteria>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<ReportDispatchTaskViewModelCriteria>.RegisterRef(e => e.Resource, ResourceIdProperty);

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
        public static readonly IRefIdProperty WorkShopIdProperty = P<ReportDispatchTaskViewModelCriteria>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty = P<ReportDispatchTaskViewModelCriteria>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return this.GetRefEntity(WorkShopProperty); }
            set { this.SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 产品 Product
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品")]
        public static readonly IRefIdProperty ProductIdProperty = P<ReportDispatchTaskViewModelCriteria>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Item> ProductProperty = P<ReportDispatchTaskViewModelCriteria>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Product
        {
            get { return GetRefEntity(ProductProperty); }
            set { SetRefEntity(ProductProperty, value); }
        }
        #endregion        

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单编号")]
        public static readonly Property<string> WorkOrderNoProperty = P<ReportDispatchTaskViewModelCriteria>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
            set { this.SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 工单状态 WorkOrderState
        /// <summary>
        /// 工单状态
        /// </summary>
        [Label("工单状态")]
        public static readonly Property<Core.WorkOrders.WorkOrderState?> WorkOrderStateProperty = P<ReportDispatchTaskViewModelCriteria>.Register(e => e.WorkOrderState);

        /// <summary>
        /// 工单状态
        /// </summary>
        public Core.WorkOrders.WorkOrderState? WorkOrderState
        {
            get { return GetProperty(WorkOrderStateProperty); }
            set { SetProperty(WorkOrderStateProperty, value); }
        }
        #endregion

        #region 任务单号 No
        /// <summary>
        /// 任务单号
        /// </summary>        
        [Label("任务单号")]
        public static readonly Property<string> NoProperty = P<ReportDispatchTaskViewModelCriteria>.Register(e => e.No);

        /// <summary>
        /// 任务单号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 任务单状态 TaskStatus
        /// <summary>
        /// 任务单状态
        /// </summary>
        [Label("任务单状态")]
        public static readonly Property<DispatchTaskStatus?> TaskStatusProperty = P<ReportDispatchTaskViewModelCriteria>.Register(e => e.TaskStatus);

        /// <summary>
        /// 任务单状态
        /// </summary>
        public DispatchTaskStatus? TaskStatus
        {
            get { return GetProperty(TaskStatusProperty); }
            set { SetProperty(TaskStatusProperty, value); }
        }
        #endregion

        #region 开始时间 BeginTime
        /// <summary>
        /// 开始时间
        /// </summary>
        [Label("开始时间")]
        public static readonly Property<DateRange> BeginTimeProperty = P<ReportDispatchTaskViewModelCriteria>.Register(e => e.BeginTime, new PropertyMetadata<DateRange>() { DateTimePart = DateTimePart.Date, });

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateRange BeginTime
        {
            get { return GetProperty(BeginTimeProperty); }
            set { SetProperty(BeginTimeProperty, value); }
        }
        #endregion

        #region 班组 WorkGroup
        /// <summary>
        /// 班组Id
        /// </summary>
        [Label("班组")]
        [Required]
        public static readonly IRefIdProperty WorkGroupIdProperty =
            P<ReportDispatchTaskViewModelCriteria>.RegisterRefId(e => e.WorkGroupId, ReferenceType.Normal);

        /// <summary>
        /// 班组Id
        /// </summary>
        public double? WorkGroupId
        {
            get { return (double)this.GetRefId(WorkGroupIdProperty); }
            set { this.SetRefId(WorkGroupIdProperty, value); }
        }

        /// <summary>
        /// 班组
        /// </summary>
        public static readonly RefEntityProperty<SIE.Resources.Employees.WorkGroup> WorkGroupProperty =
            P<ReportDispatchTaskViewModelCriteria>.RegisterRef(e => e.WorkGroup, WorkGroupIdProperty);

        /// <summary>
        /// 班组
        /// </summary>
        public SIE.Resources.Employees.WorkGroup WorkGroup
        {
            get { return this.GetRefEntity(WorkGroupProperty); }
            set { this.SetRefEntity(WorkGroupProperty, value); }
        }
        #endregion

        #region 员工 Employee
        /// <summary>
        /// 员工Id
        /// </summary>
        [Label("员工")]
        [Required]
        public static readonly IRefIdProperty EmployeeIdProperty =
            P<ReportDispatchTaskViewModelCriteria>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 员工Id
        /// </summary>
        public double? EmployeeId
        {
            get { return (double)this.GetRefId(EmployeeIdProperty); }
            set { this.SetRefId(EmployeeIdProperty, value); }
        }

        /// <summary>
        /// 员工
        /// </summary>
        public static readonly RefEntityProperty<SIE.Resources.Employees.Employee> EmployeeProperty =
            P<ReportDispatchTaskViewModelCriteria>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 员工
        /// </summary>
        public SIE.Resources.Employees.Employee Employee
        {
            get { return this.GetRefEntity(EmployeeProperty); }
            set { this.SetRefEntity(EmployeeProperty, value); }
        }
        #endregion

        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ReportController>().GetReportDispatchTaskList(this);
        }
    }
}
