using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using System;

namespace SIE.MES.TaskManagement.Dispatchs
{
    /// <summary>
    /// 派工任务查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("派工任务查询实体")]
    public class DispatchTaskCriteria : Criteria
    {
        #region 任务单号 No
        /// <summary>
        /// 任务单号
        /// </summary>        
        [Label("任务单号")]
        public static readonly Property<string> NoProperty = P<DispatchTaskCriteria>.Register(e => e.No);

        /// <summary>
        /// 任务单号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 资源名称 ResourceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源名称")]
        public static readonly Property<string> ResourceNameProperty = P<DispatchTaskCriteria>.Register(e => e.ResourceName);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return GetProperty(ResourceNameProperty); }
            set { SetProperty(ResourceNameProperty, value); }
        }
        #endregion

        #region 旧物料号 OldItem
        /// <summary>
        /// 旧物料号
        /// </summary>
        [Label("旧物料号")]
        public static readonly Property<string> OldItemProperty = P<DispatchTaskCriteria>.Register(e => e.OldItem);

        /// <summary>
        /// 旧物料号
        /// </summary>
        public string OldItem
        {
            get { return GetProperty(OldItemProperty); }
            set { SetProperty(OldItemProperty, value); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<DispatchTaskCriteria>.Register(e => e.ProductCode);

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
        public static readonly Property<string> ProductNameProperty = P<DispatchTaskCriteria>.Register(e => e.ProductName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return GetProperty(ProductNameProperty); }
            set { SetProperty(ProductNameProperty, value); }
        }
        #endregion

        //#region 资源 Resource
        ///// <summary>
        ///// 资源Id
        ///// </summary>
        //[Label("资源")]
        //public static readonly IRefIdProperty ResourceIdProperty = P<DispatchTaskCriteria>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        ///// <summary>
        ///// 资源Id
        ///// </summary>
        //public double? ResourceId
        //{
        //    get { return (double?)GetRefNullableId(ResourceIdProperty); }
        //    set { SetRefNullableId(ResourceIdProperty, value); }
        //}

        ///// <summary>
        ///// 资源
        ///// </summary>
        //public static readonly RefEntityProperty<WipResource> ResourceProperty = P<DispatchTaskCriteria>.RegisterRef(e => e.Resource, ResourceIdProperty);

        ///// <summary>
        ///// 资源
        ///// </summary>
        //public WipResource Resource
        //{
        //    get { return GetRefEntity(ResourceProperty); }
        //    set { SetRefEntity(ResourceProperty, value); }
        //}
        //#endregion

        #region 资源编码 ResourceCode
        /// <summary>
        /// 资源编码
        /// </summary>
        [Label("资源编码")]
        public static readonly Property<string> ResourceCodeProperty = P<DispatchTaskCriteria>.Register(e => e.ResourceCode);

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode
        {
            get { return this.GetProperty(ResourceCodeProperty); }
            set { this.SetProperty(ResourceCodeProperty, value); }
        }
        #endregion

        #region 车间编码 WorkShop
        /// <summary>
        /// 车间编码
        /// </summary>
        [Label("车间编码")]
        public static readonly Property<string> WorkShopProperty = P<DispatchTaskCriteria>.Register(e => e.WorkShop);

        /// <summary>
        /// 车间编码
        /// </summary>
        public string WorkShop
        {
            get { return this.GetProperty(WorkShopProperty); }
            set { this.SetProperty(WorkShopProperty, value); }
        }
        #endregion


        //#region 车间 WorkShop
        ///// <summary>
        ///// 车间ID
        ///// </summary>
        //[Label("车间")]
        //public static readonly IRefIdProperty WorkShopIdProperty = P<DispatchTaskCriteria>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

        ///// <summary>
        ///// 车间ID
        ///// </summary>
        //public double? WorkShopId
        //{
        //    get { return (double?)this.GetRefNullableId(WorkShopIdProperty); }
        //    set { this.SetRefNullableId(WorkShopIdProperty, value); }
        //}

        ///// <summary>
        ///// 车间
        ///// </summary>
        //public static readonly RefEntityProperty<Enterprise> WorkShopProperty = P<DispatchTaskCriteria>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        ///// <summary>
        ///// 车间
        ///// </summary>
        //public Enterprise WorkShop
        //{
        //    get { return this.GetRefEntity(WorkShopProperty); }
        //    set { this.SetRefEntity(WorkShopProperty, value); }
        //}
        //#endregion

        #region 关联工单 WorkOrder
        /// <summary>
        /// 关联工单Id
        /// </summary>
        [Label("关联工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<DispatchTaskCriteria>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 关联工单Id
        /// </summary>
        public double? WorkOrderId
        {
            get { return (double?)GetRefNullableId(WorkOrderIdProperty); }
            set { SetRefNullableId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 关联工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<DispatchTaskCriteria>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 关联工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<DispatchTaskCriteria>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
            set { this.SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion


        #region 关联工序 Process
        /// <summary>
        /// 关联工序Id
        /// </summary>
        [Label("关联工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<DispatchTaskCriteria>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 关联工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)GetRefNullableId(ProcessIdProperty); }
            set { SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 关联工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<DispatchTaskCriteria>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 关联工序
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 排程开始时间 PlanBeginTime
        /// <summary>
        /// 排程开始时间
        /// </summary>
        [Label("排程开始时间")]
        public static readonly Property<DateRange> PlanBeginTimeProperty = P<DispatchTaskCriteria>.Register(e => e.PlanBeginTime, new PropertyMetadata<DateRange>() { DateTimePart = DateTimePart.Date, });

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
        public static readonly Property<DateRange> PlanEndTimeProperty = P<DispatchTaskCriteria>.Register(e => e.PlanEndTime, new PropertyMetadata<DateRange>() { DateTimePart = DateTimePart.Date, });

        /// <summary>
        /// 排程完成时间
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
        public static readonly Property<string> TaskStatusProperty = P<DispatchTaskCriteria>.Register(e => e.TaskStatus);

        /// <summary>
        /// 任务单状态
        /// </summary>
        public string TaskStatus
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
        public static readonly Property<ReportMode?> ReportModeProperty = P<DispatchTaskCriteria>.Register(e => e.ReportMode);

        /// <summary>
        /// 报工方式
        /// </summary>
        public ReportMode? ReportMode
        {
            get { return GetProperty(ReportModeProperty); }
            set { SetProperty(ReportModeProperty, value); }
        }
        #endregion

        #region 是否共模 IsSyntype
        /// <summary>
        /// 是否共模
        /// </summary>
        [Label("是否共模")]
        public static readonly Property<bool> IsSyntypeProperty = P<DispatchTaskCriteria>.Register(e => e.IsSyntype);

        /// <summary>
        /// 是否共模
        /// </summary>
        public bool IsSyntype
        {
            get { return GetProperty(IsSyntypeProperty); }
            set { SetProperty(IsSyntypeProperty, value); }
        }
        #endregion

        #region 显示虚拟件 IsVirtualPart
        /// <summary>
        /// 显示虚拟件
        /// </summary>
        [Label("显示虚拟件")]
        public static readonly Property<bool> IsVirtualPartProperty = P<DispatchTaskCriteria>.Register(e => e.IsVirtualPart);

        /// <summary>
        /// 显示虚拟件
        /// </summary>
        public bool IsVirtualPart
        {
            get { return GetProperty(IsVirtualPartProperty); }
            set { SetProperty(IsVirtualPartProperty, value); }
        }
        #endregion

        #region 显示可派工 IsShowDispatchTask
        /// <summary>
        /// 显示可派工
        /// </summary>
        [Label("显示可派工")]
        public static readonly Property<bool> IsShowDispatchTaskProperty = P<DispatchTaskCriteria>.Register(e => e.IsShowDispatchTask);

        /// <summary>
        /// 显示可派工
        /// </summary>
        public bool IsShowDispatchTask
        {
            get { return GetProperty(IsShowDispatchTaskProperty); }
            set { SetProperty(IsShowDispatchTaskProperty, value); }
        }
        #endregion

        #region 任务执行对象 AdoName
        /// <summary>
        /// 任务执行对象
        /// </summary>
        [Label("任务执行对象")]
        public static readonly Property<string> AdoNameProperty = P<DispatchTaskCriteria>.Register(e => e.AdoName);

        /// <summary>
        /// 任务执行对象
        /// </summary>
        public string AdoName
        {
            get { return this.GetProperty(AdoNameProperty); }
            set { this.SetProperty(AdoNameProperty, value); }
        }
        #endregion

        #region 是否排程退回 IsSchedulingInfReturn
        /// <summary>
        /// 是否排程退回
        /// </summary>
        [Label("是否排程退回")]
        public static readonly Property<bool?> IsSchedulingInfReturnProperty = P<DispatchTaskCriteria>.Register(e => e.IsSchedulingInfReturn);

        /// <summary>
        /// 是否排程退回
        /// </summary>
        public bool? IsSchedulingInfReturn
        {
            get { return this.GetProperty(IsSchedulingInfReturnProperty); }
            set { this.SetProperty(IsSchedulingInfReturnProperty, value); }
        }
        #endregion

        #region 生产管理者 Fevor
        /// <summary>
        /// 生产管理者
        /// </summary>
        [Label("生产管理者")]
        public static readonly Property<string> FevorProperty = P<DispatchTaskCriteria>.Register(e => e.Fevor);

        /// <summary>
        /// 生产管理者
        /// </summary>
        public string Fevor
        {
            get { return this.GetProperty(FevorProperty); }
            set { this.SetProperty(FevorProperty, value); }
        }
        #endregion

        #region 是否显示关闭任务单 IsClose
        /// <summary>
        /// 是否显示关闭任务单
        /// </summary>
        [Label("是否显示关闭任务单")]
        public static readonly Property<bool?> IsCloseProperty = P<DispatchTaskCriteria>.Register(e => e.IsClose);

        /// <summary>
        /// 是否显示关闭任务单
        /// </summary>
        public bool? IsClose
        {
            get { return this.GetProperty(IsCloseProperty); }
            set { this.SetProperty(IsCloseProperty, value); }
        }
        #endregion

        #region 排程导入时间 ImportTime
        /// <summary>
        /// 排程导入时间
        /// </summary>
        [Label("排程导入时间")]
        public static readonly Property<DateRange> ImportTimeProperty = P<DispatchTaskCriteria>.Register(e => e.ImportTime);

        /// <summary>
        /// 排程导入时间
        /// </summary>
        public DateRange ImportTime
        {
            get { return this.GetProperty(ImportTimeProperty); }
            set { this.SetProperty(ImportTimeProperty, value); }
        }
        #endregion


        /// <summary>
        /// 获取派工管理列表
        /// </summary>
        /// <returns>派工管理列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<DispatchController>().GetDispatchTaskList(this);
        }
    }
}
