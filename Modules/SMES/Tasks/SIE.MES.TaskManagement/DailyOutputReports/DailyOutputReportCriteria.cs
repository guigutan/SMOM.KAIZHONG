using SIE.Domain;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;

namespace SIE.MES.TaskManagement.DailyOutputReports
{
    /// <summary>
    /// 日产出报表查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("日产出报表查询实体")]
    public class DailyOutputReportCriteria : Criteria
    {

        #region 资源编码 ResourceCode
        /// <summary>
        /// 资源编码
        /// </summary>
        [Label("资源编码")]
        public static readonly Property<string> ResourceCodeProperty = P<DailyOutputReportCriteria>.Register(e => e.ResourceCode);

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode
        {
            get { return this.GetProperty(ResourceCodeProperty); }
            set { this.SetProperty(ResourceCodeProperty, value); }
        }
        #endregion

        #region 资源名称 ResourceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源名称")]
        public static readonly Property<string> ResourceNameProperty = P<DailyOutputReportCriteria>.Register(e => e.ResourceName);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return GetProperty(ResourceNameProperty); }
            set { SetProperty(ResourceNameProperty, value); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<DailyOutputReportCriteria>.Register(e => e.ProductCode);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return GetProperty(ProductCodeProperty); }
            set { SetProperty(ProductCodeProperty, value); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<DailyOutputReportCriteria>.Register(e => e.ProductName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return GetProperty(ProductNameProperty); }
            set { SetProperty(ProductNameProperty, value); }
        }
        #endregion

        #region MRP控制者 MrpController
        /// <summary>
        /// MRP控制者
        /// </summary>
        [Label("MRP控制者")]
        public static readonly Property<string> MrpControllerProperty = P<DailyOutputReportCriteria>.Register(e => e.MrpController);

        /// <summary>
        /// MRP控制者
        /// </summary>
        public string MrpController
        {
            get { return this.GetProperty(MrpControllerProperty); }
            set { this.SetProperty(MrpControllerProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<DailyOutputReportCriteria>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId
        {
            get { return (double?)this.GetRefNullableId(WorkOrderIdProperty); }
            set { this.SetRefNullableId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty =
            P<DailyOutputReportCriteria>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<DailyOutputReportCriteria>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)this.GetRefNullableId(ProcessIdProperty); }
            set { this.SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty =
            P<DailyOutputReportCriteria>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 班次 Classes
        /// <summary>
        /// 班次
        /// </summary>
        [Label("班次")]
        public static readonly Property<ClassesType?> ClassesProperty = P<DailyOutputReportCriteria>.Register(e => e.Classes);

        /// <summary>
        /// 班次
        /// </summary>
        public ClassesType? Classes
        {
            get { return this.GetProperty(ClassesProperty); }
            set { this.SetProperty(ClassesProperty, value); }
        }
        #endregion

        #region 排程开始时间 PlanBeginTime
        /// <summary>
        /// 排程开始时间
        /// </summary>
        [Label("排程开始时间")]
        public static readonly Property<DateRange> PlanBeginTimeProperty = P<DailyOutputReportCriteria>.Register(e => e.PlanBeginTime, new PropertyMetadata<DateRange>() { DateTimePart = DateTimePart.Date, });

        /// <summary>
        /// 排程开始时间
        /// </summary>
        public DateRange PlanBeginTime
        {
            get { return GetProperty(PlanBeginTimeProperty); }
            set { SetProperty(PlanBeginTimeProperty, value); }
        }
        #endregion

        #region 排程完成时间 PlanEndTime
        /// <summary>
        /// 排程完成时间
        /// </summary>
        [Label("排程完成时间")]
        public static readonly Property<DateRange> PlanEndTimeProperty = P<DailyOutputReportCriteria>.Register(e => e.PlanEndTime, new PropertyMetadata<DateRange>() { DateTimePart = DateTimePart.Date, });

        /// <summary>
        /// 排程完成时间
        /// </summary>
        public DateRange PlanEndTime
        {
            get { return GetProperty(PlanEndTimeProperty); }
            set { SetProperty(PlanEndTimeProperty, value); }
        }
        #endregion

        #region 事业部 Division
        /// <summary>
        /// 事业部
        /// </summary>
        [Label("事业部")]
        public static readonly Property<string> DivisionProperty = P<DailyOutputReportCriteria>.Register(e => e.Division);

        /// <summary>
        /// 事业部
        /// </summary>
        public string Division
        {
            get { return this.GetProperty(DivisionProperty); }
            set { this.SetProperty(DivisionProperty, value); }
        }
        #endregion

        #region 部门 Department
        /// <summary>
        /// 部门
        /// </summary>
        [Label("部门")]
        public static readonly Property<string> DepartmentProperty = P<DailyOutputReportCriteria>.Register(e => e.Department);

        /// <summary>
        /// 部门
        /// </summary>
        public string Department
        {
            get { return this.GetProperty(DepartmentProperty); }
            set { this.SetProperty(DepartmentProperty, value); }
        }
        #endregion

        #region 旧物料号 ShortDescription
        /// <summary>
        /// 旧物料号
        /// </summary>
        [Label("旧物料号")]
        public static readonly Property<string> ShortDescriptionProperty = P<DailyOutputReportCriteria>.Register(e => e.ShortDescription);

        /// <summary>
        /// 旧物料号
        /// </summary>
        public string ShortDescription
        {
            get { return this.GetProperty(ShortDescriptionProperty); }
            set { this.SetProperty(ShortDescriptionProperty, value); }
        }
        #endregion

        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<DailyOutputReportController>().GetDailyOutputReports(this);
        }
    }
}
