using SIE.Common.Configs;
using SIE.Core.Equipments;
using SIE.Domain;
using SIE.Items;
using SIE.KZ.Base.SmomControl;
using SIE.MES.TaskManagement.Configs;
using SIE.MES.TaskManagement.Reports;
using SIE.MES.TaskManagement.Specifications;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using System;
using System.Linq;

namespace SIE.MES.TaskManagement.Dispatchs
{
    /// <summary>
    /// 派工任务
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(DispatchTaskCriteria))]
    [EntityWithConfig(typeof(DispatchConfig))]
    [EntityWithConfig(typeof(ItemTypeConfig))]
    [DisplayMember(nameof(No))]
    //[BillPrintable(typeof(DispatchTaskBillPrintable))]
    //[SIE.DataAuth.EntityDataAuth(typeof(Resources.Employees.EmployeeEnterprise), nameof(FactoryId), true)]
    [Label("派工任务")]
    public partial class DispatchTask : DataEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DispatchTask()
        {
            SingleQty = 1;
        }

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        [Required]
        public static readonly IRefIdProperty FactoryIdProperty =
            P<DispatchTask>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double? FactoryId
        {
            get { return (double?)this.GetRefNullableId(FactoryIdProperty); }
            set { this.SetRefNullableId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> FactoryProperty =
            P<DispatchTask>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return this.GetRefEntity(FactoryProperty); }
            set { this.SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 任务单号 No
        /// <summary>
        /// 任务单号
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("任务单号")]
        public static readonly Property<string> NoProperty = P<DispatchTask>.Register(e => e.No);

        /// <summary>
        /// 任务单号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 任务数量 DispatchQty
        /// <summary>
        /// 任务数量
        /// </summary>
        [Label("任务数量")]
        public static readonly Property<decimal> DispatchQtyProperty = P<DispatchTask>.Register(e => e.DispatchQty);

        /// <summary>
        /// 任务数量
        /// </summary>
        public decimal DispatchQty
        {
            get { return GetProperty(DispatchQtyProperty); }
            set { SetProperty(DispatchQtyProperty, value); }
        }
        #endregion

        #region 可疑品数量 SuspectQty
        /// <summary>
        /// 可疑品数量
        /// </summary>
        [Label("可疑品数量")]
        public static readonly Property<decimal> SuspectQtyProperty = P<DispatchTask>.Register(e => e.SuspectQty);

        /// <summary>
        /// 可疑品数量
        /// </summary>
        public decimal SuspectQty
        {
            get { return this.GetProperty(SuspectQtyProperty); }
            set { this.SetProperty(SuspectQtyProperty, value); }
        }
        #endregion

        #region 已派工数量 SendQty
        /// <summary>
        /// 已派工数量
        /// </summary>
        [Label("已派工数量")]
        public static readonly Property<decimal> SendQtyProperty = P<DispatchTask>.Register(e => e.SendQty);

        /// <summary>
        /// 已派工数量
        /// </summary>
        public decimal SendQty
        {
            get { return GetProperty(SendQtyProperty); }
            set { SetProperty(SendQtyProperty, value); }
        }
        #endregion

        #region 已报工数量 ReportQty
        /// <summary>
        /// 已报工数量
        /// </summary>
        [Label("已报工数量")]
        public static readonly Property<decimal> ReportQtyProperty = P<DispatchTask>.Register(e => e.ReportQty);

        /// <summary>
        /// 已报工数量
        /// </summary>
        public decimal ReportQty
        {
            get { return GetProperty(ReportQtyProperty); }
            set { SetProperty(ReportQtyProperty, value); }
        }
        #endregion

        #region 合格数量 OkQty
        /// <summary>
        /// 合格数量
        /// </summary>
        [Label("合格数量")]
        public static readonly Property<decimal> OkQtyProperty = P<DispatchTask>.Register(e => e.OkQty);

        /// <summary>
        /// 合格数量
        /// </summary>
        public decimal OkQty
        {
            get { return GetProperty(OkQtyProperty); }
            set { SetProperty(OkQtyProperty, value); }
        }
        #endregion

        #region 不合格数量 NgQty
        /// <summary>
        /// 不合格数量
        /// </summary>
        [Label("不合格数量")]
        public static readonly Property<decimal> NgQtyProperty = P<DispatchTask>.Register(e => e.NgQty);

        /// <summary>
        /// 不合格数量
        /// </summary>
        public decimal NgQty
        {
            get { return GetProperty(NgQtyProperty); }
            set { SetProperty(NgQtyProperty, value); }
        }
        #endregion

        #region 返工数量 ReworkQty
        /// <summary>
        /// 返工数量
        /// </summary>
        [Label("返工数量")]
        public static readonly Property<decimal> ReworkQtyProperty = P<DispatchTask>.Register(e => e.ReworkQty);

        /// <summary>
        /// 返工数量
        /// </summary>
        public decimal ReworkQty
        {
            get { return this.GetProperty(ReworkQtyProperty); }
            set { this.SetProperty(ReworkQtyProperty, value); }
        }
        #endregion

        #region 任务执行对象 TaskPerformer
        /// <summary>
        /// 任务执行对象
        /// </summary>
        [MaxLength(1000)]
        [Label("任务执行对象")]
        public static readonly Property<string> TaskPerformerProperty = P<DispatchTask>.Register(e => e.TaskPerformer);

        /// <summary>
        /// 任务执行对象
        /// </summary>
        public string TaskPerformer
        {
            get { return GetProperty(TaskPerformerProperty); }
            set { SetProperty(TaskPerformerProperty, value); }
        }
        #endregion

        #region 排程开始时间 PlanBeginTime
        /// <summary>
        /// 排程开始时间
        /// </summary>
        [Label("排程开始时间")]
        public static readonly Property<DateTime> PlanBeginTimeProperty = P<DispatchTask>.Register(e => e.PlanBeginTime);

        /// <summary>
        /// 排程开始时间
        /// </summary>
        public DateTime PlanBeginTime
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
        public static readonly Property<DateTime> PlanEndTimeProperty = P<DispatchTask>.Register(e => e.PlanEndTime);

        /// <summary>
        /// 排程完成时间
        /// </summary>
        public DateTime PlanEndTime
        {
            get { return GetProperty(PlanEndTimeProperty); }
            set { SetProperty(PlanEndTimeProperty, value); }
        }
        #endregion

        #region 开始时间 BeginTime
        /// <summary>
        /// 开始时间
        /// </summary>
        [Label("开始时间")]
        public static readonly Property<DateTime?> BeginTimeProperty = P<DispatchTask>.Register(e => e.BeginTime);

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginTime
        {
            get { return GetProperty(BeginTimeProperty); }
            set { SetProperty(BeginTimeProperty, value); }
        }
        #endregion

        #region 完成时间 EndTime
        /// <summary>
        /// 完成时间
        /// </summary>
        [Label("完成时间")]
        public static readonly Property<DateTime?> EndTimeProperty = P<DispatchTask>.Register(e => e.EndTime);

        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime? EndTime
        {
            get { return GetProperty(EndTimeProperty); }
            set { SetProperty(EndTimeProperty, value); }
        }
        #endregion

        #region 关联工单 AssociatedWorkOrder
        /// <summary>
        /// 关联工单
        /// </summary>
        [Label("关联工单")]
        public static readonly Property<string> AssociatedWorkOrderProperty = P<DispatchTask>.Register(e => e.AssociatedWorkOrder);

        /// <summary>
        /// 关联工单
        /// </summary>
        public string AssociatedWorkOrder
        {
            get { return GetProperty(AssociatedWorkOrderProperty); }
            set { SetProperty(AssociatedWorkOrderProperty, value); }
        }
        #endregion

        #region 是否虚拟件 IsVirtualPart
        /// <summary>
        /// 是否虚拟件
        /// </summary>
        [Label("是否虚拟件")]
        public static readonly Property<bool> IsVirtualPartProperty = P<DispatchTask>.Register(e => e.IsVirtualPart);

        /// <summary>
        /// 是否虚拟件
        /// </summary>
        public bool IsVirtualPart
        {
            get { return GetProperty(IsVirtualPartProperty); }
            set { SetProperty(IsVirtualPartProperty, value); }
        }
        #endregion

        #region 单机定额 SingleQty
        /// <summary>
        /// 单机定额
        /// 除虚拟件和规格件任务单外单机定额为1
        /// </summary>
        [Label("单机定额")]
        public static readonly Property<decimal> SingleQtyProperty = P<DispatchTask>.Register(e => e.SingleQty);

        /// <summary>
        /// 单机定额
        /// </summary>
        public decimal SingleQty
        {
            get { return this.GetProperty(SingleQtyProperty); }
            set { this.SetProperty(SingleQtyProperty, value); }
        }
        #endregion

        #region 虚拟件编码 VirtualPartCode
        /// <summary>
        /// 虚拟件编码
        /// </summary>
        [Label("虚拟件编码")]
        public static readonly Property<string> VirtualPartCodeProperty = P<DispatchTask>.Register(e => e.VirtualPartCode);

        /// <summary>
        /// 虚拟件编码
        /// </summary>
        public string VirtualPartCode
        {
            get { return GetProperty(VirtualPartCodeProperty); }
            set { SetProperty(VirtualPartCodeProperty, value); }
        }
        #endregion

        #region 虚拟件名称 VirtualPartName
        /// <summary>
        /// 虚拟件名称
        /// </summary>
        [Label("虚拟件名称")]
        public static readonly Property<string> VirtualPartNameProperty = P<DispatchTask>.Register(e => e.VirtualPartName);

        /// <summary>
        /// 虚拟件名称
        /// </summary>
        public string VirtualPartName
        {
            get { return GetProperty(VirtualPartNameProperty); }
            set { SetProperty(VirtualPartNameProperty, value); }
        }
        #endregion

        #region 是否共模 IsSyntype
        /// <summary>
        /// 是否共模
        /// </summary>
        [Label("是否共模")]
        public static readonly Property<bool> IsSyntypeProperty = P<DispatchTask>.Register(e => e.IsSyntype);

        /// <summary>
        /// 是否共模
        /// </summary>
        public bool IsSyntype
        {
            get { return GetProperty(IsSyntypeProperty); }
            set { SetProperty(IsSyntypeProperty, value); }
        }
        #endregion

        #region 工艺单编号 TechNo
        /// <summary>
        /// 工艺单编号
        /// </summary>
        [Label("工艺单编号")]
        public static readonly Property<string> TechNoProperty = P<DispatchTask>.Register(e => e.TechNo);

        /// <summary>
        /// 工艺单编号
        /// </summary>
        public string TechNo
        {
            get { return GetProperty(TechNoProperty); }
            set { SetProperty(TechNoProperty, value); }
        }
        #endregion

        #region 是否任务单 IsMainTask
        /// <summary>
        /// 是否任务单
        /// </summary>
        [Label("是否任务单")]
        public static readonly Property<bool> IsMainTaskProperty = P<DispatchTask>.Register(e => e.IsMainTask);

        /// <summary>
        /// 是否任务单
        /// </summary>
        public bool IsMainTask
        {
            get { return this.GetProperty(IsMainTaskProperty); }
            set { this.SetProperty(IsMainTaskProperty, value); }
        }
        #endregion 

        #region 共模模具数 Proportion
        /// <summary>   
        /// 模具数
        /// </summary>
        [Label("模具数")]
        public static readonly Property<double> ProportionProperty = P<DispatchTask>.Register(e => e.Proportion);

        /// <summary>
        /// 模具数
        /// </summary>
        public double Proportion
        {
            get { return this.GetProperty(ProportionProperty); }
            set { this.SetProperty(ProportionProperty, value); }
        }
        #endregion

        #region 关联关系 Associated
        /// <summary>
        /// 关联关系
        /// </summary>
        [Label("关联关系")]
        public static readonly Property<Associated?> AssociatedProperty = P<DispatchTask>.Register(e => e.Associated);

        /// <summary>
        /// 关联关系
        /// </summary>
        public Associated? Associated
        {
            get { return GetProperty(AssociatedProperty); }
            set { SetProperty(AssociatedProperty, value); }
        }
        #endregion

        #region 旧关联关系 OldAssociated
        /// <summary>
        /// 旧关联关系,撤销合并使用
        /// </summary>
        [Label("旧关联关系")]
        public static readonly Property<Associated?> OldAssociatedProperty = P<DispatchTask>.Register(e => e.OldAssociated);

        /// <summary>
        /// 旧关联关系
        /// </summary>
        public Associated? OldAssociated
        {
            get { return GetProperty(OldAssociatedProperty); }
            set { SetProperty(OldAssociatedProperty, value); }
        }
        #endregion

        #region 合并状态 MergedStatus
        /// <summary>
        /// 合并状态
        /// </summary>
        [Label("合并状态")]
        public static readonly Property<MergedStatus> MergedStatusProperty = P<DispatchTask>.Register(e => e.MergedStatus);

        /// <summary>
        /// 合并状态
        /// </summary>
        public MergedStatus MergedStatus
        {
            get { return this.GetProperty(MergedStatusProperty); }
            set { this.SetProperty(MergedStatusProperty, value); }
        }
        #endregion

        #region 报工方式 ReportMode
        /// <summary>
        /// 报工方式
        /// </summary>
        [Label("报工方式")]
        public static readonly Property<ReportMode> ReportModeProperty = P<DispatchTask>.Register(e => e.ReportMode);

        /// <summary>
        /// 报工方式
        /// </summary>
        public ReportMode ReportMode
        {
            get { return GetProperty(ReportModeProperty); }
            set { SetProperty(ReportModeProperty, value); }
        }
        #endregion

        #region 优先级 Priority
        /// <summary>
        /// 优先级
        /// </summary>
        [Label("优先级")]
        public static readonly Property<DispatchTaskPriority> PriorityProperty = P<DispatchTask>.Register(e => e.Priority);

        /// <summary>
        /// 优先级
        /// </summary>
        public DispatchTaskPriority Priority
        {
            get { return GetProperty(PriorityProperty); }
            set { SetProperty(PriorityProperty, value); }
        }
        #endregion

        #region 任务单状态 TaskStatus
        /// <summary>
        /// 任务单状态
        /// </summary>
        [Label("任务单状态")]
        public static readonly Property<DispatchTaskStatus> TaskStatusProperty = P<DispatchTask>.Register(e => e.TaskStatus);

        /// <summary>
        /// 任务单状态
        /// </summary>
        public DispatchTaskStatus TaskStatus
        {
            get { return GetProperty(TaskStatusProperty); }
            set { SetProperty(TaskStatusProperty, value); }
        }
        #endregion

        #region 旧任务单状态 OldTaskStatus
        /// <summary>
        /// 旧任务单状态
        /// </summary>
        [Label("旧任务单状态")]
        public static readonly Property<DispatchTaskStatus?> OldTaskStatusProperty = P<DispatchTask>.Register(e => e.OldTaskStatus);

        /// <summary>
        /// 旧任务单状态
        /// </summary>
        public DispatchTaskStatus? OldTaskStatus
        {
            get { return GetProperty(OldTaskStatusProperty); }
            set { SetProperty(OldTaskStatusProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<DispatchTask>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)GetRefNullableId(ProcessIdProperty); }
            set { SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<DispatchTask>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 产品 Product
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品")]
        public static readonly IRefIdProperty ProductIdProperty = P<DispatchTask>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ProductId
        {
            get { return (double)GetRefId(ProductIdProperty); }
            set { SetRefId(ProductIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public static readonly RefEntityProperty<Item> ProductProperty = P<DispatchTask>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Product
        {
            get { return GetRefEntity(ProductProperty); }
            set { SetRefEntity(ProductProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<DispatchTask>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<DispatchTask>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion


        #region 工单工序清单 RoutingProcess
        /// <summary>
        /// 工单工序清单Id
        /// </summary>
        [Label("工单工序清单")]
        public static readonly IRefIdProperty RoutingProcessIdProperty = P<DispatchTask>.RegisterRefId(e => e.RoutingProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工单工序清单Id
        /// </summary>
        public double? RoutingProcessId
        {
            get { return (double?)GetRefNullableId(RoutingProcessIdProperty); }
            set { SetRefNullableId(RoutingProcessIdProperty, value); }
        }

        /// <summary>
        /// 工单工序清单
        /// </summary>
        [Label("工单工序清单")]
        public static readonly RefEntityProperty<WorkOrderRoutingProcess> RoutingProcessProperty = P<DispatchTask>.RegisterRef(e => e.RoutingProcess, RoutingProcessIdProperty);

        /// <summary>
        /// 工单工序清单
        /// </summary>
        public WorkOrderRoutingProcess RoutingProcess
        {
            get { return GetRefEntity(RoutingProcessProperty); }
            set { SetRefEntity(RoutingProcessProperty, value); }
        }
        #endregion

        #region 规格件 Specification
        /// <summary>
        /// 规格件Id
        /// </summary>
        [Label("规格件")]
        public static readonly IRefIdProperty SpecificationIdProperty = P<DispatchTask>.RegisterRefId(e => e.SpecificationId, ReferenceType.Normal);

        /// <summary>
        /// 规格件Id
        /// </summary>
        public double? SpecificationId
        {
            get { return (double?)GetRefNullableId(SpecificationIdProperty); }
            set { SetRefNullableId(SpecificationIdProperty, value); }
        }

        /// <summary>
        /// 规格件
        /// </summary>
        public static readonly RefEntityProperty<Specification> SpecificationProperty = P<DispatchTask>.RegisterRef(e => e.Specification, SpecificationIdProperty);

        /// <summary>
        /// 规格件
        /// </summary>
        public Specification Specification
        {
            get { return GetRefEntity(SpecificationProperty); }
            set { SetRefEntity(SpecificationProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty = P<DispatchTask>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<DispatchTask>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 旧资源 OldResource
        /// <summary>
        /// 旧资源Id
        /// </summary>
        [Label("旧资源")]
        public static readonly IRefIdProperty OldResourceIdProperty =
            P<DispatchTask>.RegisterRefId(e => e.OldResourceId, ReferenceType.Normal);

        /// <summary>
        /// 旧资源Id
        /// </summary>
        public double? OldResourceId
        {
            get { return (double?)this.GetRefNullableId(OldResourceIdProperty); }
            set { this.SetRefNullableId(OldResourceIdProperty, value); }
        }

        /// <summary>
        /// 旧资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> OldResourceProperty =
            P<DispatchTask>.RegisterRef(e => e.OldResource, OldResourceIdProperty);

        /// <summary>
        /// 旧资源
        /// </summary>
        public WipResource OldResource
        {
            get { return this.GetRefEntity(OldResourceProperty); }
            set { this.SetRefEntity(OldResourceProperty, value); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间ID
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkShopIdProperty = P<DispatchTask>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty = P<DispatchTask>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return this.GetRefEntity(WorkShopProperty); }
            set { this.SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 任务顺序 Seq
        /// <summary>
        /// 任务顺序
        /// 工序任务Seq>0且根据工序顺序依次累加
        /// 其他任务类型Seq=0
        /// </summary>
        [Label("任务顺序")]
        public static readonly Property<int> SeqProperty = P<DispatchTask>.Register(e => e.Seq);

        /// <summary>
        /// 任务顺序
        /// </summary>
        public int Seq
        {
            get { return this.GetProperty(SeqProperty); }
            set { this.SetProperty(SeqProperty, value); }
        }
        #endregion

        #region 是否共模比报工 IsSyntypeReport
        /// <summary>
        /// 是否共模比报工
        /// </summary>
        [Label("共模比报工")]
        public static readonly Property<bool> IsSyntypeReportProperty = P<DispatchTask>.Register(e => e.IsSyntypeReport);

        /// <summary>
        /// 是否共模比报工
        /// </summary>
        public bool IsSyntypeReport
        {
            get { return this.GetProperty(IsSyntypeReportProperty); }
            set { this.SetProperty(IsSyntypeReportProperty, value); }
        }
        #endregion

        #region 首工序 StartProcess 
        /// <summary>
        /// 首工序
        /// </summary>
        [Label("首工序")]
        public static readonly Property<bool?> StartProcessProperty = P<DispatchTask>.Register(e => e.StartProcess);

        /// <summary>
        /// 首工序
        /// </summary>
        public bool? StartProcess
        {
            get { return this.GetProperty(StartProcessProperty); }
            set { this.SetProperty(StartProcessProperty, value); }
        }
        #endregion

        #region 末工序 EndProcess
        /// <summary>
        /// 末工序
        /// </summary>
        [Label("末工序")]
        public static readonly Property<bool?> EndProcessProperty = P<DispatchTask>.Register(e => e.EndProcess);

        /// <summary>
        /// 末工序
        /// </summary>
        public bool? EndProcess
        {
            get { return this.GetProperty(EndProcessProperty); }
            set { this.SetProperty(EndProcessProperty, value); }
        }
        #endregion

        #region 辅助设备 EquipAccount
        /// <summary>
        /// 辅助设备Id
        /// </summary>
        [Label("辅助设备")]
        public static readonly IRefIdProperty EquipAccountIdProperty =
            P<DispatchTask>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 辅助设备Id
        /// </summary>
        public double? EquipAccountId
        {
            get { return (double?)this.GetRefNullableId(EquipAccountIdProperty); }
            set { this.SetRefNullableId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 辅助设备
        /// </summary>
        public static readonly RefEntityProperty<EquipAccount> EquipAccountProperty =
            P<DispatchTask>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 辅助设备
        /// </summary>
        public EquipAccount EquipAccount
        {
            get { return this.GetRefEntity(EquipAccountProperty); }
            set { this.SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 关联任务列表 TaskList
        /// <summary>
        /// 关联任务列表
        /// </summary>
        [Label("关联任务列表")]
        public static readonly ListProperty<EntityList<AssociatedTask>> TaskListProperty = P<DispatchTask>.RegisterList(e => e.TaskList);

        /// <summary>
        /// 关联任务列表
        /// </summary>
        public EntityList<AssociatedTask> TaskList
        {
            get { return this.GetLazyList(TaskListProperty); }
        }
        #endregion

        #region 明细列表 Details
        /// <summary>
        /// 明细列表
        /// </summary>
        public static readonly ListProperty<EntityList<DispatchTaskDetail>> DetailsProperty = P<DispatchTask>.RegisterList(e => e.Details);

        /// <summary>
        /// 明细列表
        /// </summary>
        public EntityList<DispatchTaskDetail> Details
        {
            get { return this.GetLazyList(DetailsProperty); }
        }
        #endregion

        #region 工序BOM列表 Boms
        /// <summary>
        /// 工序BOM列表
        /// </summary>
        public static readonly ListProperty<EntityList<TaskProcessBom>> BomsProperty = P<DispatchTask>.RegisterList(e => e.Boms);

        /// <summary>
        /// 工序BOM列表
        /// </summary>
        public EntityList<TaskProcessBom> Boms
        {
            get { return this.GetLazyList(BomsProperty); }
        }
        #endregion

        #region 执行记录 OptLogList
        /// <summary>
        /// 执行记录
        /// </summary>
        [Label("执行记录")]
        public static readonly ListProperty<EntityList<ReportOperateLog>> OptLogListProperty = P<DispatchTask>.RegisterList(e => e.OptLogList);

        /// <summary>
        /// 执行记录
        /// </summary>
        public EntityList<ReportOperateLog> OptLogList
        {
            get { return this.GetLazyList(OptLogListProperty); }
        }
        #endregion

        #region 报工记录列表 Records
        /// <summary>
        /// 报工记录列表
        /// </summary>
        public static readonly ListProperty<EntityList<ReportRecord>> RecordsProperty = P<DispatchTask>.RegisterList(e => e.Records);

        /// <summary>
        /// 报工记录列表
        /// </summary>
        public EntityList<ReportRecord> Records
        {
            get { return this.GetLazyList(RecordsProperty); }
        }
        #endregion

        #region 转入标签列表 TransferLabels
        /// <summary>
        /// 转入标签列表
        /// </summary>
        public static readonly ListProperty<EntityList<ReportTransferLabel>> TransferLabelsProperty = P<DispatchTask>.RegisterList(e => e.TransferLabels);

        /// <summary>
        /// 转入标签列表
        /// </summary>
        public EntityList<ReportTransferLabel> TransferLabels
        {
            get { return this.GetLazyList(TransferLabelsProperty); }
        }
        #endregion

        #region 视图属性
        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<DispatchTask>.RegisterView(e => e.WorkOrderNo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
        }
        #endregion 

        #region 工单状态 WorkOrderState
        /// <summary>
        /// 工单状态
        /// </summary>
        [Label("工单状态")]
        public static readonly Property<Core.WorkOrders.WorkOrderState> WorkOrderStateProperty = P<DispatchTask>.RegisterView(e => e.WorkOrderState, p => p.WorkOrder.State);

        /// <summary>
        /// 工单状态
        /// </summary>
        public Core.WorkOrders.WorkOrderState WorkOrderState
        {
            get { return this.GetProperty(WorkOrderStateProperty); }
        }
        #endregion

        #region 工单计划数 WorkOrderPlanQty
        /// <summary>
        /// 工单计划数
        /// </summary>
        [Label("工单计划数")]
        public static readonly Property<decimal> WorkOrderPlanQtyProperty = P<DispatchTask>.RegisterView(e => e.WorkOrderPlanQty, p => p.WorkOrder.PlanQty);

        /// <summary>
        /// 工单计划数
        /// </summary>
        public decimal WorkOrderPlanQty
        {
            get { return this.GetProperty(WorkOrderPlanQtyProperty); }
        }
        #endregion

        #region 工单是否暂停 IsPause
        /// <summary>
        /// 工单是否暂停
        /// </summary>
        [Label("是否暂停")]
        public static readonly Property<YesNo> IsPauseProperty = P<DispatchTask>.RegisterView(e => e.IsPause, p => p.WorkOrder.IsPause);

        /// <summary>
        /// 工单是否暂停
        /// </summary>
        public YesNo IsPause
        {
            get { return this.GetProperty(IsPauseProperty); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<DispatchTask>.RegisterView(e => e.ProductCode, p => p.Product.Code);

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
        public static readonly Property<string> ProductNameProperty = P<DispatchTask>.RegisterView(e => e.ProductName, p => p.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }
        #endregion

        #region 产品物料类型 ProductMtart
        /// <summary>
        /// 产品物料类型
        /// </summary>
        [Label("产品物料类型")]
        public static readonly Property<string> ProductMtartProperty = P<DispatchTask>.RegisterView(e => e.ProductMtart, p => p.Product.Mtart);

        /// <summary>
        /// 产品物料类型
        /// </summary>
        public string ProductMtart
        {
            get { return this.GetProperty(ProductMtartProperty); }
        }
        #endregion

        #region 单位名称 UnitName
        /// <summary>
        /// 单位名称
        /// </summary>
        [Label("单位名称")]
        public static readonly Property<string> UnitNameProperty = P<DispatchTask>.RegisterView(e => e.UnitName, p => p.Product.Unit.Name);

        /// <summary>
        /// 单位名称
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
        }

        #endregion

        #region 旧物料号 ShortDescription
        /// <summary>
        /// 旧物料号
        /// </summary>
        [Label("旧物料号")]
        public static readonly Property<string> ShortDescriptionProperty = P<DispatchTask>.RegisterView(e => e.ShortDescription, p => p.Product.ShortDescription);

        /// <summary>
        /// 旧物料号
        /// </summary>
        public string ShortDescription
        {
            get { return this.GetProperty(ShortDescriptionProperty); }
        }

        #endregion

        #region 饼重 ItemWeight
        /// <summary>
        /// 饼重
        /// </summary>
        [Label("饼重")]
        public static readonly Property<string> ItemWeightProperty = P<DispatchTask>.RegisterView(e => e.ItemWeight, p => p.Product.Weight);

        /// <summary>
        /// 饼重
        /// </summary>
        public string ItemWeight
        {
            get { return this.GetProperty(ItemWeightProperty); }
        }

        #endregion

        #region 净重单位 WeightUnit
        /// <summary>
        /// 净重单位
        /// </summary>
        [Label("净重单位")]
        public static readonly Property<string> WeightUnitProperty = P<DispatchTask>.RegisterView(e => e.WeightUnit, p => p.Product.WeightUnit);

        /// <summary>
        /// 净重单位
        /// </summary>
        public string WeightUnit
        {
            get { return this.GetProperty(WeightUnitProperty); }
        }

        #endregion

        #region 物料扩展属性 ItemExtPropName
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        public static readonly Property<string> ItemExtPropNameProperty = P<DispatchTask>.RegisterView(e => e.ItemExtPropName, p => p.WorkOrder.ItemExtPropName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ItemExtPropName
        {
            get { return this.GetProperty(ItemExtPropNameProperty); }
        }
        #endregion

        #region MRP控制者 MrpController
        /// <summary>
        /// MRP控制者
        /// </summary>
        [Label("MRP控制者")]
        public static readonly Property<string> MrpControllerProperty = P<DispatchTask>.RegisterView(e => e.MrpController, p => p.Product.MrpController);

        /// <summary>
        /// MRP控制者
        /// </summary>
        public string MrpController
        {
            get { return this.GetProperty(MrpControllerProperty); }
        }

        #endregion

        #region 工序编码 ProcessCode
        /// <summary>
        /// 工序编码
        /// </summary>
        [Label("工序编码")]
        public static readonly Property<string> ProcessCodeProperty = P<DispatchTask>.RegisterView(e => e.ProcessCode, p => p.Process.Code);

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
        }

        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<DispatchTask>.RegisterView(e => e.ProcessName, p => p.Process.Name);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }
        #endregion

        #region 是否按分单数量报工 IsReportByZcode
        /// <summary>
        /// 是否按分单数量报工
        /// </summary>
        [Label("是否按分单数量报工")]
        public static readonly Property<bool?> IsReportByZcodeProperty = P<DispatchTask>.RegisterView(e => e.IsReportByZcode, p => p.Process.IsReportByZcode);

        /// <summary>
        /// 是否按分单数量报工
        /// </summary>
        public bool? IsReportByZcode
        {
            get { return this.GetProperty(IsReportByZcodeProperty); }
        }

        #endregion

        #region 报工分单数阀值(%) ZcodeThreshold
        /// <summary>
        /// 报工分单数阀值(%)
        /// </summary>
        [Label("报工分单数阀值(%)")]
        public static readonly Property<decimal> ZcodeThresholdProperty = P<DispatchTask>.RegisterView(e => e.ZcodeThreshold, p => p.Process.ZcodeThreshold);

        /// <summary>
        /// 报工分单数阀值(%)
        /// </summary>
        public decimal ZcodeThreshold
        {
            get { return this.GetProperty(ZcodeThresholdProperty); }
        }

        #endregion

        #region 是否需要辅助设备 IsNeedEquipment
        /// <summary>
        /// 是否需要辅助设备
        /// </summary>
        [Label("是否需要辅助设备")]
        public static readonly Property<bool?> IsNeedEquipmentProperty = P<DispatchTask>.RegisterView(e => e.IsNeedEquipment, p => p.Process.IsNeedEquipment);

        /// <summary>
        /// 是否需要辅助设备
        /// </summary>
        public bool? IsNeedEquipment
        {
            get { return this.GetProperty(IsNeedEquipmentProperty); }
        }

        #endregion

        #region 规格件编码 SpecificationCode
        /// <summary>
        /// 规格件编码
        /// </summary>
        [Label("规格件编码")]
        public static readonly Property<string> SpecificationCodeProperty = P<DispatchTask>.RegisterView(e => e.SpecificationCode, p => p.Specification.Code);

        /// <summary>
        /// 规格件编码
        /// </summary>
        public string SpecificationCode
        {
            get { return this.GetProperty(SpecificationCodeProperty); }
        }
        #endregion

        #region 规格件名称 SpecificationName
        /// <summary>
        /// 规格件名称
        /// </summary>
        [Label("规格件名称")]
        public static readonly Property<string> SpecificationNameProperty = P<DispatchTask>.RegisterView(e => e.SpecificationName, p => p.Specification.Name);

        /// <summary>
        /// 规格件名称
        /// </summary>
        public string SpecificationName
        {
            get { return this.GetProperty(SpecificationNameProperty); }
        }
        #endregion

        #region 产品规格型号名称 SpecificationModelName
        /// <summary>
        /// 产品规格型号名称
        /// </summary>
        [Label("产品规格型号名称")]
        public static readonly Property<string> SpecificationModelNameProperty = P<DispatchTask>.RegisterView(e => e.SpecificationModelName, p => p.Product.SpecificationModel);

        /// <summary>
        /// 产品规格型号名称
        /// </summary>
        public string SpecificationModelName
        {
            get { return this.GetProperty(SpecificationModelNameProperty); }
        }
        #endregion

        #region 辅助设备名称 EquipAccountName
        /// <summary>
        /// 辅助设备名称
        /// </summary>
        [Label("辅助设备名称")]
        public static readonly Property<string> EquipAccountNameProperty = P<DispatchTask>.RegisterView(e => e.EquipAccountName, p => p.EquipAccount.Name);

        /// <summary>
        /// 辅助设备名称
        /// </summary>
        public string EquipAccountName
        {
            get { return this.GetProperty(EquipAccountNameProperty); }
        }
        #endregion


        #region 超报工比例 ExcessReportRatio
        /// <summary>
        /// 超报工比例
        /// </summary>
        [Label("超额比例%")]
        public static readonly Property<decimal?> ExcessReportRatioProperty = P<DispatchTask>.RegisterView(e => e.ExcessReportRatio, p => p.Product.ExcessReportRatio);

        /// <summary>
        /// 超报工比例
        /// </summary>
        public decimal? ExcessReportRatio
        {
            get { return this.GetProperty(ExcessReportRatioProperty); }
        }
        #endregion


        #region 工序标准工时（分） ProcessStandardHour
        /// <summary>
        /// 工序标准工时（分）
        /// </summary>
        [Label("工序标准工时（分）")]
        public static readonly Property<decimal?> ProcessStandardHourProperty = P<DispatchTask>.Register(e => e.ProcessStandardHour);

        /// <summary>
        /// 工序标准工时
        /// </summary>
        public decimal? ProcessStandardHour
        {
            get { return GetProperty(ProcessStandardHourProperty); }
            set { SetProperty(ProcessStandardHourProperty, value); }
        }
        #endregion

        #region 附加合计工时（分） ProcessHourSum
        /// <summary>
        /// 附加合计工时（分）
        /// </summary>
        [Label("附加合计工时（分）")]
        public static readonly Property<decimal?> ProcessHourSumProperty = P<DispatchTask>.Register(e => e.ProcessHourSum);

        /// <summary>
        /// 附加合计工时
        /// </summary>
        public decimal? ProcessHourSum
        {
            get { return GetProperty(ProcessHourSumProperty); }
            set { SetProperty(ProcessHourSumProperty, value); }
        }
        #endregion

        #region 预计生产时长（H） ExpectedProductionTime
        /// <summary>
        /// 预计生产时长（H）
        /// </summary>
        [Label("预计生产时长（H）")]
        public static readonly Property<decimal?> ExpectedProductionTimeProperty = P<DispatchTask>.Register(e => e.ExpectedProductionTime);

        /// <summary>
        /// 预计生产时长（H）
        /// </summary>
        public decimal? ExpectedProductionTime
        {
            get { return GetProperty(ExpectedProductionTimeProperty); }
            set { SetProperty(ExpectedProductionTimeProperty, value); }
        }
        #endregion

        #region 车间编码 WorkShopCode
        /// <summary>
        /// 车间编码
        /// </summary>
        [Label("车间编码")]
        public static readonly Property<string> WorkShopCodeProperty = P<DispatchTask>.RegisterView(e => e.WorkShopCode, p => p.WorkShop.Code);

        /// <summary>
        /// 车间编码
        /// </summary>
        public string WorkShopCode
        {
            get { return this.GetProperty(WorkShopCodeProperty); }
        }
        #endregion

        #region 车间名称 WorkShopName
        /// <summary>
        /// 车间名称
        /// </summary>
        [Label("车间名称")]
        public static readonly Property<string> WorkShopNameProperty = P<DispatchTask>.RegisterView(e => e.WorkShopName, p => p.WorkShop.Name);

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkShopName
        {
            get { return this.GetProperty(WorkShopNameProperty); }
        }
        #endregion

        #region 资源编码 ResourceCode
        /// <summary>
        /// 资源编码
        /// </summary>
        [Label("资源编码")]
        public static readonly Property<string> ResourceCodeProperty = P<DispatchTask>.RegisterView(e => e.ResourceCode, p => p.Resource.Code);

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode
        {
            get { return this.GetProperty(ResourceCodeProperty); }
        }
        #endregion

        #region 资源名称 ResourceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源名称")]
        public static readonly Property<string> ResourceNameProperty = P<DispatchTask>.RegisterView(e => e.ResourceName, p => p.Resource.Name);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
        }
        #endregion

        #region 生产管理者 Fevor
        /// <summary>
        /// 生产管理者
        /// </summary>
        [Label("生产管理者")]
        public static readonly Property<string> FevorProperty = P<DispatchTask>.RegisterView(e => e.Fevor, p => p.WorkOrder.Fevor);

        /// <summary>
        /// 生产管理者
        /// </summary>
        public string Fevor
        {
            get { return this.GetProperty(FevorProperty); }
        }
        #endregion

        #region 交货容差 Uebto
        /// <summary>
        /// 交货容差
        /// </summary>
        [Label("交货容差")]
        public static readonly Property<string> UebtoProperty = P<DispatchTask>.RegisterView(e => e.Uebto, p => p.WorkOrder.Uebto);

        /// <summary>
        /// 交货容差
        /// </summary>
        public string Uebto
        {
            get { return this.GetProperty(UebtoProperty); }
        }

        #endregion

        #region 资源类型 ResourceSourceType
        /// <summary>
        /// 资源类型
        /// </summary>
        [Label("资源类型")]
        public static readonly Property<SyncSourceType?> ResourceSourceTypeProperty = P<DispatchTask>.RegisterView(e => e.ResourceSourceType, p => p.Resource.SourceType);

        /// <summary>
        /// 资源类型
        /// </summary>
        public SyncSourceType? ResourceSourceType
        {
            get { return this.GetProperty(ResourceSourceTypeProperty); }
        }
        #endregion

        #region 制卡数量 Ztfl
        /// <summary>
        /// 制卡数量
        /// </summary>
        [Label("制卡数量")]
        public static readonly Property<decimal?> ZtflProperty = P<DispatchTask>.RegisterView(e => e.Ztfl, p => p.WorkOrder.Ztfl);

        /// <summary>
        /// 制卡数量
        /// </summary>
        public decimal? Ztfl
        {
            get { return this.GetProperty(ZtflProperty); }
        }
        #endregion


        #endregion

        #region 超额数量 ExcessReportQty
        /// <summary>
        /// 超额数量
        /// </summary>
        [Label("超额数量")]
        public static readonly Property<decimal> ToReceiveQtyProperty = P<DispatchTask>.RegisterReadOnly(
            e => e.ExcessReportQty, e => e.GetExcessReportQty(), ExcessReportRatioProperty, DispatchQtyProperty);
        /// <summary>
        /// 超额数量
        /// </summary>

        public decimal ExcessReportQty
        {
            get { return this.GetProperty(ToReceiveQtyProperty); }
        }
        private decimal GetExcessReportQty()
        {
            return Math.Round((ExcessReportRatio.HasValue ? ExcessReportRatio.Value : 0) * DispatchQty * 0.01m, 0);
        }
        #endregion

        #region 最大可报工数 MaxReportQty
        /// <summary>
        /// 最大可报工数
        /// </summary>
        [Label("最大可报工数")]
        public static readonly Property<decimal> MaxReportQtyProperty = P<DispatchTask>.RegisterReadOnly(
            e => e.MaxReportQty, e => e.GetMaxReportQty(), UebtoProperty);
        /// <summary>
        /// 最大可报工数
        /// </summary>

        public decimal MaxReportQty
        {
            get { return this.GetProperty(MaxReportQtyProperty); }
        }
        private decimal GetMaxReportQty()
        {
            //单位非PCS的不需要取整，单位为PCS的向上取整
            if (Uebto.IsNullOrEmpty())
                return string.Equals(UnitName, "PCS", StringComparison.OrdinalIgnoreCase) ? Math.Ceiling(DispatchQty) : DispatchQty;
            else
            {
                decimal uebto = 0;
                decimal.TryParse(Uebto, out uebto);

                decimal qty = 0;

                qty = DispatchQty * (1 + uebto / 100);
                return string.Equals(UnitName, "PCS", StringComparison.OrdinalIgnoreCase) ? Math.Ceiling(qty) : qty;
            }
        }
        #endregion

        #region 剩余可报工数 RemainQty
        /// <summary>
        /// 剩余可报工数
        /// </summary>
        [Label("剩余可报工数")]
        public static readonly Property<decimal> RemainQtyProperty = P<DispatchTask>.RegisterReadOnly(
            e => e.RemainQty, e => e.GetRemainQty(), DispatchQtyProperty);
        /// <summary>
        /// 剩余可报工数
        /// </summary>

        public decimal RemainQty
        {
            get { return this.GetProperty(RemainQtyProperty); }
        }
        private decimal GetRemainQty()
        {
            var qty = (DispatchQty - ReportQty - SuspectQty);
            if (qty < 0) qty = 0;
            return qty;
        }
        #endregion

        #region 剩余可报工数(容差) MaxRemainQty
        /// <summary>
        /// 剩余可报工数(容差)
        /// </summary>
        [Label("剩余可报工数(容差)")]
        public static readonly Property<int> MaxRemainQtyProperty = P<DispatchTask>.RegisterReadOnly(
            e => e.MaxRemainQty, e => e.GetMaxRemainQty(), MaxReportQtyProperty);
        /// <summary>
        /// 剩余可报工数(容差)
        /// </summary>

        public int MaxRemainQty
        {
            get { return this.GetProperty(MaxRemainQtyProperty); }
        }
        private int GetMaxRemainQty()
        {
            var qty = (int)Math.Ceiling(MaxReportQty - ReportQty - SuspectQty);
            if (qty < 0) qty = 0;
            return qty;
        }
        #endregion

        #region 实际报工数 ActualReportQty
        /// <summary>
        /// 实际报工数
        /// </summary>
        [Label("实际报工数")]
        public static readonly Property<decimal> ActualReportQtyProperty = P<DispatchTask>.RegisterReadOnly(
            e => e.ActualReportQty, e => e.GetActualReportQty(), ReportQtyProperty, SuspectQtyProperty);
        /// <summary>
        /// 实际报工数
        /// </summary>

        public decimal ActualReportQty
        {
            get { return this.GetProperty(ActualReportQtyProperty); }
        }
        private decimal GetActualReportQty()
        {
            return ReportQty + SuspectQty;
        }
        #endregion

        #region 报工进度 ReportProgress
        /// <summary>
        /// 报工进度
        /// </summary>
        [Label("报工进度")]
        public static readonly Property<decimal> ReportProgressProperty = P<DispatchTask>.RegisterReadOnly(
            e => e.ReportProgress, e => e.GetReportProgress(), ReportQtyProperty, SuspectQtyProperty, DispatchQtyProperty);
        /// <summary>
        /// 报工进度
        /// </summary>

        public decimal ReportProgress
        {
            get { return this.GetProperty(ReportProgressProperty); }
        }
        private decimal GetReportProgress()
        {
            if (DispatchQty == 0)
                return 0;
            return Math.Round(ActualReportQty * 100 / DispatchQty, 2);
        }
        #endregion

        #region 不映射数据库的属性

        #region 报检数量 InspQty
        /// <summary>
        /// 报检数量
        /// </summary>
        [Label("报检数量")]
        public static readonly Property<int> InspQtyProperty = P<DispatchTask>.Register(e => e.InspQty);

        /// <summary>
        /// 报检数量
        /// </summary>
        public int InspQty
        {
            get { return this.GetProperty(InspQtyProperty); }
            set { this.SetProperty(InspQtyProperty, value); }
        }
        #endregion

        #region 父级旧料号 ParShortDescription
        /// <summary>
        /// 父级旧料号
        /// </summary>
        [Label("父级旧料号")]
        public static readonly Property<string> ParShortDescriptionProperty = P<DispatchTask>.Register(e => e.ParShortDescription);

        /// <summary>
        /// 父级旧料号
        /// </summary>
        public string ParShortDescription
        {
            get { return this.GetProperty(ParShortDescriptionProperty); }
            set { this.SetProperty(ParShortDescriptionProperty, value); }
        }
        #endregion


        #endregion

        #region 班次 Classes
        /// <summary>
        /// 班次
        /// </summary>
        [Label("班次")]
        public static readonly Property<ClassesType?> ClassesProperty = P<DispatchTask>.Register(e => e.Classes);

        /// <summary>
        /// 班次
        /// </summary>
        public ClassesType? Classes
        {
            get { return this.GetProperty(ClassesProperty); }
            set { this.SetProperty(ClassesProperty, value); }
        }
        #endregion

        #region 来源类型 SourceType
        /// <summary>
        /// 来源类型
        /// </summary>
        [Label("来源类型")]
        public static readonly Property<SourceType?> SourceTypeProperty = P<DispatchTask>.Register(e => e.SourceType);

        /// <summary>
        /// 来源类型
        /// </summary>
        public SourceType? SourceType
        {
            get { return this.GetProperty(SourceTypeProperty); }
            set { this.SetProperty(SourceTypeProperty, value); }
        }
        #endregion

        #region 是否上料关闭 IsFeedingClose
        /// <summary>
        /// 是否上料关闭(以后在PDA上料上不再显示这张任务单)
        /// </summary>
        [Label("是否上料关闭")]
        public static readonly Property<bool?> IsFeedingCloseProperty = P<DispatchTask>.Register(e => e.IsFeedingClose);

        /// <summary>
        /// 是否上料关闭
        /// </summary>
        public bool? IsFeedingClose
        {
            get { return this.GetProperty(IsFeedingCloseProperty); }
            set { this.SetProperty(IsFeedingCloseProperty, value); }
        }
        #endregion

        #region 是否排程退回 IsSchedulingInfReturn
        /// <summary>
        /// 是否排程退回
        /// </summary>
        [Label("是否排程退回")]
        public static readonly Property<YesNo?> IsSchedulingInfReturnProperty = P<DispatchTask>.Register(e => e.IsSchedulingInfReturn);

        /// <summary>
        /// 是否排程退回
        /// </summary>
        public YesNo? IsSchedulingInfReturn
        {
            get { return this.GetProperty(IsSchedulingInfReturnProperty); }
            set { this.SetProperty(IsSchedulingInfReturnProperty, value); }
        }
        #endregion

        #region 排程退回原因 SchedulingInfReturnReason
        /// <summary>
        /// 排程退回原因
        /// </summary>
        [Label("排程退回原因")]
        public static readonly Property<string> SchedulingInfReturnReasonProperty = P<DispatchTask>.Register(e => e.SchedulingInfReturnReason);

        /// <summary>
        /// 排程退回原因
        /// </summary>
        public string SchedulingInfReturnReason
        {
            get { return this.GetProperty(SchedulingInfReturnReasonProperty); }
            set { this.SetProperty(SchedulingInfReturnReasonProperty, value); }
        }
        #endregion

        #region 排程导入时间 ImportTime
        /// <summary>
        /// 排程导入时间
        /// </summary>
        [Label("排程导入时间")]
        public static readonly Property<DateTime?> ImportTimeProperty = P<DispatchTask>.Register(e => e.ImportTime);

        /// <summary>
        /// 排程导入时间
        /// </summary>
        public DateTime? ImportTime
        {
            get { return this.GetProperty(ImportTimeProperty); }
            set { this.SetProperty(ImportTimeProperty, value); }
        }
        #endregion

        #region 是否委外任务单 IsOutsourcing
        /// <summary>
        /// 是否委外任务单
        /// </summary>
        [Label("是否委外任务单")]
        public static readonly Property<bool?> IsOutsourcingProperty = P<DispatchTask>.Register(e => e.IsOutsourcing);

        /// <summary>
        /// 是否委外任务单
        /// </summary>
        public bool? IsOutsourcing
        {
            get { return this.GetProperty(IsOutsourcingProperty); }
            set { this.SetProperty(IsOutsourcingProperty, value); }
        }
        #endregion
        
        #region 手工报工数量 ManualReportQty
        /// <summary>
        /// 手工报工数量
        /// </summary>
        [Label("手工报工数量")]
        public static readonly Property<decimal> ManualReportQtyProperty = P<DispatchTask>.Register(e => e.ManualReportQty);

        /// <summary>
        /// 手工报工数量
        /// </summary>
        public decimal ManualReportQty
        {
            get { return this.GetProperty(ManualReportQtyProperty); }
            set { this.SetProperty(ManualReportQtyProperty, value); }
        }
        #endregion

        #region IOT设备数量 IotQty
        /// <summary>
        /// IOT设备数量
        /// </summary>
        [Label("IOT设备数量")]
        public static readonly Property<decimal> IotQtyProperty = P<DispatchTask>.Register(e => e.IotQty);

        /// <summary>
        /// IOT设备数量
        /// </summary>
        public decimal IotQty
        {
            get { return this.GetProperty(IotQtyProperty); }
            set { this.SetProperty(IotQtyProperty, value); }
        }
        #endregion

        #region IOT状态 IotStatus
        /// <summary>
        /// IOT状态
        /// </summary>
        [Label("IOT状态")]
        public static readonly Property<IotStatus> IotStatusProperty = P<DispatchTask>.Register(e => e.IotStatus);

        /// <summary>
        /// IOT状态
        /// </summary>
        public IotStatus IotStatus
        {
            get { return this.GetProperty(IotStatusProperty); }
            set { this.SetProperty(IotStatusProperty, value); }
        }
        #endregion

        #region 产品穴位 CavityCount
        /// <summary>
        /// 产品穴位
        /// </summary>
        [Label("产品穴位")]
        public static readonly Property<int> CavityCountProperty = P<DispatchTask>.Register(e => e.CavityCount);

        /// <summary>
        /// 产品穴位
        /// </summary>
        public int CavityCount
        {
            get { return this.GetProperty(CavityCountProperty); }
            set { this.SetProperty(CavityCountProperty, value); }
        }
        #endregion

        #region 打印数量 PrintedQty
        /// <summary>
        /// 打印数量
        /// </summary>
        [Label("打印数量")]
        public static readonly Property<int> PrintedQtyProperty = P<DispatchTask>.Register(e => e.PrintedQty);

        /// <summary>
        /// 打印数量
        /// </summary>
        public int PrintedQty
        {
            get { return this.GetProperty(PrintedQtyProperty); }
            set { this.SetProperty(PrintedQtyProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 派工任务 实体配置
    /// </summary>
    internal class DispatchTaskEntityConfig : EntityConfig<DispatchTask>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TM_DISP_TASK").MapAllProperties();
            Meta.Property(DispatchTask.TaskPerformerProperty).ColumnMeta.HasLength(2000);
            Meta.Property(DispatchTask.WorkOrderIdProperty).ColumnMeta.HasIndex();
            Meta.Property(DispatchTask.InspQtyProperty).DontMapColumn();
            Meta.Property(DispatchTask.ParShortDescriptionProperty).DontMapColumn();
            Meta.Property(DispatchTask.SchedulingInfReturnReasonProperty).MapColumn().HasLength("4000");

            Meta.EnablePhantoms();
        }
    }
}