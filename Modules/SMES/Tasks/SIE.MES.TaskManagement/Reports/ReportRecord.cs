using SIE.Common;
using SIE.Common.Configs;
using SIE.Domain;
using SIE.MES.PackingQC;
using SIE.MES.TaskManagement.Configs;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Reports.Enums;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.ShiftTypes;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using System;
using System.Collections.Generic;
using SourceType = SIE.MES.TaskManagement.Reports.Enums.SourceType;

namespace SIE.MES.TaskManagement.Reports
{
    /// <summary>
    /// 报工记录
    /// </summary>
    [ChildEntity, Serializable]
    [EntityWithConfig(typeof(ReportRecordDetailConfig))]
    [Label("报工记录")]
    public partial class ReportRecord : DataEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ReportRecord()
        {
            UploadFlag = false;
        }

        #region 报工数 ReportQty
        /// <summary>
        /// 报工数
        /// </summary>
        [Label("报工数")]
        public static readonly Property<decimal> ReportQtyProperty = P<ReportRecord>.Register(e => e.ReportQty);

        /// <summary>
        /// 报工数
        /// </summary>
        public decimal ReportQty
        {
            get { return GetProperty(ReportQtyProperty); }
            set { SetProperty(ReportQtyProperty, value); }
        }
        #endregion

        #region 合格数 OkQty
        /// <summary>
        /// 合格数
        /// </summary>
        [Label("合格数")]
        public static readonly Property<decimal> OkQtyProperty = P<ReportRecord>.Register(e => e.OkQty);

        /// <summary>
        /// 合格数
        /// </summary>
        public decimal OkQty
        {
            get { return GetProperty(OkQtyProperty); }
            set { SetProperty(OkQtyProperty, value); }
        }
        #endregion

        #region 不合格数 NgQty
        /// <summary>
        /// 不合格数
        /// </summary>
        [Label("不合格数")]
        public static readonly Property<decimal> NgQtyProperty = P<ReportRecord>.Register(e => e.NgQty);

        /// <summary>
        /// 不合格数
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
        public static readonly Property<decimal> ReworkQtyProperty = P<ReportRecord>.Register(e => e.ReworkQty);

        /// <summary>
        /// 返工数量
        /// </summary>
        public decimal ReworkQty
        {
            get { return this.GetProperty(ReworkQtyProperty); }
            set { this.SetProperty(ReworkQtyProperty, value); }
        }
        #endregion

        #region 返工标识 IsRework
        /// <summary>
        /// 返工标识
        /// </summary>
        [Label("返工标识")]
        public static readonly Property<bool> IsReworkProperty = P<ReportRecord>.Register(e => e.IsRework);

        /// <summary>
        /// 返工标识
        /// </summary>
        public bool IsRework
        {
            get { return this.GetProperty(IsReworkProperty); }
            set { this.SetProperty(IsReworkProperty, value); }
        }
        #endregion


        #region 统计工时（小时） Hour
        /// <summary>
        /// 统计工时（小时）
        /// </summary>
        [Label("统计工时（小时）")]
        public static readonly Property<decimal> HourProperty = P<ReportRecord>.Register(e => e.Hour);

        /// <summary>
        /// 统计工时（小时）
        /// </summary>
        public decimal Hour
        {
            get { return GetProperty(HourProperty); }
            set { SetProperty(HourProperty, value); }
        }
        #endregion

        #region 批次号 BatchNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> BatchNoProperty = P<ReportRecord>.Register(e => e.BatchNo);

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo
        {
            get { return GetProperty(BatchNoProperty); }
            set { SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 报工时间 ReportTime
        /// <summary>
        /// 报工时间
        /// </summary>
        [Label("报工时间")]
        public static readonly Property<DateTime?> ReportTimeProperty = P<ReportRecord>.Register(e => e.ReportTime);

        /// <summary>
        /// 报工时间
        /// </summary>
        public DateTime? ReportTime
        {
            get { return GetProperty(ReportTimeProperty); }
            set { SetProperty(ReportTimeProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(2400)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<ReportRecord>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 是否已报工 IsReport
        /// <summary>
        /// 是否已报工
        /// </summary>
        [Label("是否已报工")]
        public static readonly Property<bool> IsReportProperty = P<ReportRecord>.Register(e => e.IsReport);

        /// <summary>
        /// 是否已报工
        /// </summary>
        public bool IsReport
        {
            get { return GetProperty(IsReportProperty); }
            set { SetProperty(IsReportProperty, value); }
        }
        #endregion

        #region 班次 Shift
        /// <summary>
        /// 班次Id
        /// </summary>
        [Label("班次")]
        public static readonly IRefIdProperty ShiftIdProperty = P<ReportRecord>.RegisterRefId(e => e.ShiftId, ReferenceType.Normal);

        /// <summary>
        /// 班次Id
        /// </summary>
        public double? ShiftId
        {
            get { return (double?)GetRefNullableId(ShiftIdProperty); }
            set { SetRefNullableId(ShiftIdProperty, value); }
        }

        /// <summary>
        /// 班次
        /// </summary>
        public static readonly RefEntityProperty<Shift> ShiftProperty = P<ReportRecord>.RegisterRef(e => e.Shift, ShiftIdProperty);

        /// <summary>
        /// 班次
        /// </summary>
        public Shift Shift
        {
            get { return GetRefEntity(ShiftProperty); }
            set { SetRefEntity(ShiftProperty, value); }
        }
        #endregion

        #region 责任人 Principal
        /// <summary>
        /// 责任人Id
        /// </summary>
        [Label("责任人")]
        public static readonly IRefIdProperty PrincipalIdProperty = P<ReportRecord>.RegisterRefId(e => e.PrincipalId, ReferenceType.Normal);

        /// <summary>
        /// 责任人Id
        /// </summary>
        public double PrincipalId
        {
            get { return (double)GetRefId(PrincipalIdProperty); }
            set { SetRefId(PrincipalIdProperty, value); }
        }

        /// <summary>
        /// 责任人
        /// </summary>
        public static readonly RefEntityProperty<Employee> PrincipalProperty = P<ReportRecord>.RegisterRef(e => e.Principal, PrincipalIdProperty);

        /// <summary>
        /// 责任人
        /// </summary>
        public Employee Principal
        {
            get { return GetRefEntity(PrincipalProperty); }
            set { SetRefEntity(PrincipalProperty, value); }
        }
        #endregion

        #region 班组 WorkGroup
        /// <summary>
        /// 班组Id
        /// </summary>
        [Label("班组")]
        public static readonly IRefIdProperty WorkGroupIdProperty = P<ReportRecord>.RegisterRefId(e => e.WorkGroupId, ReferenceType.Normal);

        /// <summary>
        /// 班组Id
        /// </summary>
        public double? WorkGroupId
        {
            get { return (double?)GetRefNullableId(WorkGroupIdProperty); }
            set { SetRefNullableId(WorkGroupIdProperty, value); }
        }

        /// <summary>
        /// 班组
        /// </summary>
        public static readonly RefEntityProperty<WorkGroup> WorkGroupProperty = P<ReportRecord>.RegisterRef(e => e.WorkGroup, WorkGroupIdProperty);

        /// <summary>
        /// 班组
        /// </summary>
        public WorkGroup WorkGroup
        {
            get { return GetRefEntity(WorkGroupProperty); }
            set { SetRefEntity(WorkGroupProperty, value); }
        }
        #endregion

        #region 报工工单 WorkOrder
        /// <summary>
        /// 报工工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<ReportRecord>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId
        {
            get { return (double?)GetRefNullableId(WorkOrderIdProperty); }
            set { SetRefNullableId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 报工工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<ReportRecord>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 报工工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 报工缺陷列表 Defects
        /// <summary>
        /// 报工缺陷列表
        /// </summary>
        public static readonly ListProperty<EntityList<ReportDefect>> DefectsProperty = P<ReportRecord>.RegisterList(e => e.Defects);

        /// <summary>
        /// 报工缺陷列表
        /// </summary>
        public EntityList<ReportDefect> Defects
        {
            get { return this.GetLazyList(DefectsProperty); }
        }
        #endregion

        #region 报工批次标签 ReportWipBatchs
        /// <summary>
        /// 报工批次标签
        /// </summary>
        [Label("报工批次标签")]
        public static readonly ListProperty<EntityList<ReportWipBatch>> ReportWipBatchsProperty = P<ReportRecord>.RegisterList(e => e.ReportWipBatchs);

        /// <summary>
        /// 报工批次标签
        /// </summary>
        public EntityList<ReportWipBatch> ReportWipBatchs
        {
            get { return this.GetLazyList(ReportWipBatchsProperty); }
        }
        #endregion

        #region 派工任务 DispatchTask
        /// <summary>
        /// 派工任务Id
        /// </summary>
        public static readonly IRefIdProperty DispatchTaskIdProperty = P<ReportRecord>.RegisterRefId(e => e.DispatchTaskId, ReferenceType.Parent);

        /// <summary>
        /// 派工任务Id
        /// </summary>
        public double DispatchTaskId
        {
            get { return (double)GetRefId(DispatchTaskIdProperty); }
            set { SetRefId(DispatchTaskIdProperty, value); }
        }

        /// <summary>
        /// 派工任务
        /// </summary>
        public static readonly RefEntityProperty<DispatchTask> DispatchTaskProperty = P<ReportRecord>.RegisterRef(e => e.DispatchTask, DispatchTaskIdProperty);

        /// <summary>
        /// 派工任务
        /// </summary>
        public DispatchTask DispatchTask
        {
            get { return GetRefEntity(DispatchTaskProperty); }
            set { SetRefEntity(DispatchTaskProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<ReportRecord>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

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
            P<ReportRecord>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 工位 Station
        /// <summary>
        /// 工位Id
        /// </summary>
        [Label("工位")]
        public static readonly IRefIdProperty StationIdProperty =
            P<ReportRecord>.RegisterRefId(e => e.StationId, ReferenceType.Normal);

        /// <summary>
        /// 工位Id
        /// </summary>
        public double? StationId
        {
            get { return (double?)this.GetRefNullableId(StationIdProperty); }
            set { this.SetRefNullableId(StationIdProperty, value); }
        }

        /// <summary>
        /// 工位
        /// </summary>
        public static readonly RefEntityProperty<Station> StationProperty =
            P<ReportRecord>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 工位
        /// </summary>
        public Station Station
        {
            get { return this.GetRefEntity(StationProperty); }
            set { this.SetRefEntity(StationProperty, value); }
        }
        #endregion

        #region 主报工任务单Id MainTaskId
        /// <summary>
        /// 主任务单Id
        /// </summary>
        [Label("主任务单Id")]
        public static readonly Property<double> MainTaskIdProperty = P<ReportRecord>.Register(e => e.MainTaskId);

        /// <summary>
        /// 主任务单Id
        /// </summary>
        public double MainTaskId
        {
            get { return this.GetProperty(MainTaskIdProperty); }
            set { this.SetProperty(MainTaskIdProperty, value); }
        }
        #endregion

        #region 审核状态 ExamineState
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ReportRecordExamineState> ExamineStateProperty = P<ReportRecord>.Register(e => e.ExamineState);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ReportRecordExamineState ExamineState
        {
            get { return this.GetProperty(ExamineStateProperty); }
            set { this.SetProperty(ExamineStateProperty, value); }
        }
        #endregion

        #region 检验状态 InspectionStatus
        /// <summary>
        /// 检验状态
        /// </summary>
        [Label("检验状态")]
        public static readonly Property<InspectionStatus?> InspectionStatusProperty = P<ReportRecord>.Register(e => e.InspectionStatus);

        /// <summary>
        /// 检验状态
        /// </summary>
        public InspectionStatus? InspectionStatus
        {
            get { return this.GetProperty(InspectionStatusProperty); }
            set { this.SetProperty(InspectionStatusProperty, value); }
        }
        #endregion

        #region 检验结果 InspectionResult
        /// <summary>
        /// 检验结果
        /// </summary>
        [Label("检验结果")]
        public static readonly Property<InspectionResult?> InspectionResultProperty = P<ReportRecord>.Register(e => e.InspectionResult);

        /// <summary>
        /// 检验结果
        /// </summary>
        public InspectionResult? InspectionResult
        {
            get { return this.GetProperty(InspectionResultProperty); }
            set { this.SetProperty(InspectionResultProperty, value); }
        }
        #endregion

        #region 处理方式 ProcessMode
        /// <summary>
        /// 处理方式
        /// </summary>
        [Label("处理方式")]
        public static readonly Property<string> ProcessModeProperty = P<ReportRecord>.Register(e => e.ProcessMode);

        /// <summary>
        /// 处理方式
        /// </summary>
        public string ProcessMode
        {
            get { return this.GetProperty(ProcessModeProperty); }
            set { this.SetProperty(ProcessModeProperty, value); }
        }
        #endregion

        #region 合并来源Id SourceId
        /// <summary>
        /// 合并来源Id
        /// </summary>
        [Label("合并来源Id")]
        public static readonly Property<double?> SourceIdProperty = P<ReportRecord>.Register(e => e.SourceId);

        /// <summary>
        /// 合并来源Id
        /// </summary>
        public double? SourceId
        {
            get { return this.GetProperty(SourceIdProperty); }
            set { this.SetProperty(SourceIdProperty, value); }
        }
        #endregion

        #region 报工方式 SourceType
        /// <summary>
        /// 报工方式
        /// </summary>
        [Label("报工方式")]
        public static readonly Property<SourceType?> SourceTypeProperty = P<ReportRecord>.Register(e => e.SourceType);

        /// <summary>
        /// 报工方式
        /// </summary>
        public SourceType? SourceType
        {
            get { return this.GetProperty(SourceTypeProperty); }
            set { this.SetProperty(SourceTypeProperty, value); }
        }
        #endregion

        #region 凯中新增字段

        #region 工序流水码 Vornr
        /// <summary>
        /// 工序流水码
        /// </summary>
        [Label("工序流水码")]
        public static readonly Property<string> VornrProperty = P<ReportRecord>.Register(e => e.Vornr);

        /// <summary>
        /// 工序流水码
        /// </summary>
        public string Vornr
        {
            get { return this.GetProperty(VornrProperty); }
            set { this.SetProperty(VornrProperty, value); }
        }
        #endregion

        #region 控制码(工序控制码) Steus
        /// <summary>
        /// 控制码(工序控制码)
        /// </summary>
        [Label("控制码(工序控制码)")]
        public static readonly Property<string> SteusProperty = P<ReportRecord>.Register(e => e.Steus);

        /// <summary>
        /// 控制码(工序控制码)
        /// </summary>
        public string Steus
        {
            get { return this.GetProperty(SteusProperty); }
            set { this.SetProperty(SteusProperty, value); }
        }
        #endregion

        #region 分单数量 Zcode
        /// <summary>
        /// 分单数量
        /// </summary>
        [Label("分单数量")]
        public static readonly Property<decimal> ZcodeProperty = P<ReportRecord>.Register(e => e.Zcode);

        /// <summary>
        /// 分单数量
        /// </summary>
        public decimal Zcode
        {
            get { return this.GetProperty(ZcodeProperty); }
            set { this.SetProperty(ZcodeProperty, value); }
        }
        #endregion

        #region 是否已上传 UploadFlag
        /// <summary>
        /// 是否已上传
        /// </summary>
        [Label("是否已上传")]
        public static readonly Property<bool?> UploadFlagProperty = P<ReportRecord>.Register(e => e.UploadFlag);

        /// <summary>
        /// 是否已上传
        /// </summary>
        public bool? UploadFlag
        {
            get { return this.GetProperty(UploadFlagProperty); }
            set { this.SetProperty(UploadFlagProperty, value); }
        }
        #endregion

        #region 上传结果 UploadResult
        /// <summary>
        /// 上传结果
        /// </summary>
        [Label("上传结果")]
        public static readonly Property<string> UploadResultProperty = P<ReportRecord>.Register(e => e.UploadResult);

        /// <summary>
        /// 上传结果
        /// </summary>
        public string UploadResult
        {
            get { return this.GetProperty(UploadResultProperty); }
            set { this.SetProperty(UploadResultProperty, value); }
        }
        #endregion

        #region 可疑品数量 SuspectQty
        /// <summary>
        /// 可疑品数量
        /// </summary>
        [Label("可疑品数量")]
        public static readonly Property<decimal?> SuspectQtyProperty = P<ReportRecord>.Register(e => e.SuspectQty);

        /// <summary>
        /// 可疑品数量
        /// </summary>
        public decimal? SuspectQty
        {
            get { return this.GetProperty(SuspectQtyProperty); }
            set { this.SetProperty(SuspectQtyProperty, value); }
        }
        #endregion

        #endregion

        #region 视图属性 
        #region 责任人名称 PrincipalName
        /// <summary>
        /// 责任人名称
        /// </summary>
        [Label("责任人")]
        public static readonly Property<string> PrincipalNameProperty = P<ReportRecord>.RegisterView(e => e.PrincipalName, p => p.Principal.Name);

        /// <summary>
        /// 责任人名称
        /// </summary>
        public string PrincipalName
        {
            get { return this.GetProperty(PrincipalNameProperty); }
            set { SetProperty(PrincipalNameProperty, value); }
        }
        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<ReportRecord>.RegisterView(e => e.ProcessName, p => p.Process.Name);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }
        #endregion

        #region 工位名称 StationName
        /// <summary>
        /// 工位名称
        /// </summary>
        [Label("工位")]
        public static readonly Property<string> StationNameProperty = P<ReportRecord>.RegisterView(e => e.StationName, p => p.Station.Name);

        /// <summary>
        /// 工位名称
        /// </summary>
        public string StationName
        {
            get { return this.GetProperty(StationNameProperty); }
        }
        #endregion

        #region 班组名称 WorkGroupName
        /// <summary>
        /// 班组名称
        /// </summary>
        [Label("班组")]
        public static readonly Property<string> WorkGroupNameProperty = P<ReportRecord>.RegisterView(e => e.WorkGroupName, p => p.WorkGroup.Name);

        /// <summary>
        /// 班组名称
        /// </summary>
        public string WorkGroupName
        {
            get { return this.GetProperty(WorkGroupNameProperty); }
        }
        #endregion

        #region 班次名称 ShiftName
        /// <summary>
        /// 班次名称
        /// </summary>
        [Label("班次")]
        public static readonly Property<string> ShiftNameProperty = P<ReportRecord>.RegisterView(e => e.ShiftName, p => p.Shift.Name);

        /// <summary>
        /// 班次名称
        /// </summary>
        public string ShiftName
        {
            get { return this.GetProperty(ShiftNameProperty); }
        }
        #endregion

        #region 任务单号 DispatchTaskNo
        /// <summary>
        /// 任务单号
        /// </summary>
        [Label("任务单号")]
        public static readonly Property<string> DispatchTaskNoProperty = P<ReportRecord>.RegisterView(e => e.DispatchTaskNo, p => p.DispatchTask.No);

        /// <summary>
        /// 任务单号
        /// </summary>
        public string DispatchTaskNo
        {
            get { return this.GetProperty(DispatchTaskNoProperty); }
            set { SetProperty(DispatchTaskNoProperty, value); }
        }
        #endregion

        #region 任务单数量 DispatchTaskQty
        /// <summary>
        /// 任务单数量
        /// </summary>
        [Label("任务单数量")]
        public static readonly Property<decimal> DispatchTaskQtyProperty = P<ReportRecord>.RegisterView(e => e.DispatchTaskQty, p => p.DispatchTask.DispatchQty);

        /// <summary>
        /// 任务单数量
        /// </summary>
        public decimal DispatchTaskQty
        {
            get { return this.GetProperty(DispatchTaskQtyProperty); }
            set { SetProperty(DispatchTaskQtyProperty, value); }
        }
        #endregion

        #region 任务单共模模具数 DispatchTaskProportion
        /// <summary>
        /// 任务单共模模具数
        /// </summary>
        [Label("任务单共模模具数")]
        public static readonly Property<double> DispatchTaskProportionProperty = P<ReportRecord>.RegisterView(e => e.DispatchTaskProportion, p => p.DispatchTask.Proportion);

        /// <summary>
        /// 任务单共模模具数
        /// </summary>
        public double DispatchTaskProportion
        {
            get { return this.GetProperty(DispatchTaskProportionProperty); }
            set { SetProperty(DispatchTaskProportionProperty, value); }
        }
        #endregion

        #region 任务单已报数量 DispatchTaskReportQty
        /// <summary>
        /// 任务单数量
        /// </summary>
        [Label("任务单数量")]
        public static readonly Property<decimal> DispatchTaskReportQtyProperty = P<ReportRecord>.RegisterView(e => e.DispatchTaskReportQty, p => p.DispatchTask.ReportQty);

        /// <summary>
        /// 任务单数量
        /// </summary>
        public decimal DispatchTaskReportQty
        {
            get { return this.GetProperty(DispatchTaskReportQtyProperty); }
            set { SetProperty(DispatchTaskReportQtyProperty, value); }
        }
        #endregion

        #region 任务单状态 TaskStatus
        /// <summary>
        /// 任务单号
        /// </summary>
        [Label("任务单号")]
        public static readonly Property<DispatchTaskStatus> TaskNoProperty = P<ReportRecord>.RegisterView(e => e.TaskStatus, p => p.DispatchTask.TaskStatus);

        /// <summary>
        /// 任务单号
        /// </summary>
        public DispatchTaskStatus TaskStatus
        {
            get { return this.GetProperty(TaskNoProperty); }
        }
        #endregion

        #region 产品ID ProductId
        /// <summary>
        /// 产品ID
        /// </summary>
        [Label("产品ID")]
        public static readonly Property<double> ProductIdProperty = P<ReportRecord>.RegisterView(e => e.ProductId, p => p.WorkOrder.ProductId);

        /// <summary>
        /// 产品ID
        /// </summary>
        public double ProductId
        {
            get { return this.GetProperty(ProductIdProperty); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<ReportRecord>.RegisterView(e => e.ProductCode, p => p.WorkOrder.Product.Code);

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
        [Label("属性名")]
        public static readonly Property<string> ProductNameProperty = P<ReportRecord>.RegisterView(e => e.ProductName, p => p.WorkOrder.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }
        #endregion

        #region 旧物料号 ShortDescription
        /// <summary>
        /// 旧物料号
        /// </summary>
        [Label("旧物料号")]
        public static readonly Property<string> ShortDescriptionProperty = P<ReportRecord>.RegisterView(e => e.ShortDescription, p => p.WorkOrder.Product.ShortDescription);

        /// <summary>
        /// 旧物料号
        /// </summary>
        public string ShortDescription
        {
            get { return this.GetProperty(ShortDescriptionProperty); }
        }

        #endregion

        #region MRP控制者 MrpController
        /// <summary>
        /// MRP控制者
        /// </summary>
        [Label("MRP控制者")]
        public static readonly Property<string> MrpControllerProperty = P<ReportRecord>.RegisterView(e => e.MrpController, p => p.WorkOrder.Product.MrpController);

        /// <summary>
        /// MRP控制者
        /// </summary>
        public string MrpController
        {
            get { return this.GetProperty(MrpControllerProperty); }
        }

        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<ReportRecord>.RegisterView(e => e.WorkOrderNo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
        }
        #endregion

        #region 资源编码 ResourceCode
        /// <summary>
        /// 资源编码
        /// </summary>
        [Label("资源编码")]
        public static readonly Property<string> ResourceCodeProperty = P<ReportRecord>.RegisterView(e => e.ResourceCode, p => p.DispatchTask.Resource.Code);

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
        public static readonly Property<string> ResourceNameProperty = P<ReportRecord>.RegisterView(e => e.ResourceName, p => p.DispatchTask.Resource.Name);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
        }

        #endregion

        #region 工序编码 ProcessCode
        /// <summary>
        /// 工序编码
        /// </summary>
        [Label("工序编码")]
        public static readonly Property<string> ProcessCodeProperty = P<ReportRecord>.RegisterView(e => e.ProcessCode, p => p.Process.Code);

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
        }
        #endregion

        #region 工序号 Seq
        /// <summary>
        /// 工序号
        /// </summary>
        [Label("工序号")]
        public static readonly Property<int> SeqProperty = P<ReportRecord>.RegisterView(e => e.Seq, p => p.DispatchTask.Seq);

        /// <summary>
        /// 工序号
        /// </summary>
        public int Seq
        {
            get { return this.GetProperty(SeqProperty); }
        }

        #endregion

        #endregion

        #region 不映射数据库
        #region 累计合格数 TotalOkQty
        /// <summary>
        /// 累计合格数
        /// </summary>
        [Label("累计合格数")]
        public static readonly Property<decimal> TotalOkQtyProperty = P<ReportRecord>.Register(e => e.TotalOkQty);

        /// <summary>
        /// 累计合格数
        /// </summary>
        public decimal TotalOkQty
        {
            get { return this.GetProperty(TotalOkQtyProperty); }
            set { this.SetProperty(TotalOkQtyProperty, value); }
        }
        #endregion

        #region 累计不合格数 TotalNgQty
        /// <summary>
        /// 累计不合格数
        /// </summary>
        [Label("累计不合格数")]
        public static readonly Property<decimal> TotalNgQtyProperty = P<ReportRecord>.Register(e => e.TotalNgQty);

        /// <summary>
        /// 累计不合格数
        /// </summary>
        public decimal TotalNgQty
        {
            get { return this.GetProperty(TotalNgQtyProperty); }
            set { this.SetProperty(TotalNgQtyProperty, value); }
        }
        #endregion

        #region 缺陷Id集合 DefectIds
        /// <summary>
        /// 缺陷Id集合
        /// </summary>
        [Label("缺陷Id集合")]
        public static readonly Property<List<double>> DefectIdsProperty = P<ReportRecord>.Register(e => e.DefectIds);

        /// <summary>
        /// 缺陷Id集合
        /// </summary>
        public List<double> DefectIds
        {
            get { return this.GetProperty(DefectIdsProperty); }
            set { this.SetProperty(DefectIdsProperty, value); }
        }
        #endregion 

        #region 缺陷值 DefectNames
        /// <summary>
        /// 缺陷值
        /// </summary>
        [Label("缺陷值")]
        public static readonly Property<string> DefectNamesProperty = P<ReportRecord>.Register(e => e.DefectNames);

        /// <summary>
        /// 缺陷值
        /// </summary>
        public string DefectNames
        {
            get { return this.GetProperty(DefectNamesProperty); }
            set { this.SetProperty(DefectNamesProperty, value); }
        }
        #endregion

        #region 缺陷值 DefectNamesDisplay
        /// <summary>
        /// 缺陷值
        /// </summary>
        //[Label("缺陷值")]
        public static readonly Property<string> DefectNamesDisplayProperty = P<ReportRecord>.Register(e => e.DefectNamesDisplay);

        /// <summary>
        /// 缺陷值
        /// </summary>
        public string DefectNamesDisplay
        {
            get { return this.GetProperty(DefectNamesDisplayProperty); }
            set { this.SetProperty(DefectNamesDisplayProperty, value); }
        }
        #endregion

        #region 批次标签 WipBatchNos
        /// <summary>
        /// 批次标签
        /// </summary>
        [Label("标签号")]
        public static readonly Property<string> WipBatchNosProperty = P<ReportRecord>.Register(e => e.WipBatchNos);

        /// <summary>
        /// 批次标签
        /// </summary>
        public string WipBatchNos
        {
            get { return this.GetProperty(WipBatchNosProperty); }
            set { this.SetProperty(WipBatchNosProperty, value); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 报工记录 实体配置
    /// </summary>
    internal class ReportRecordConfig : EntityConfig<ReportRecord>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TM_REPORT_RECORD").MapAllProperties();
            Meta.Property(ReportRecord.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.Property(ReportRecord.UploadResultProperty).ColumnMeta.HasLength(1000);
            Meta.Property(ReportRecord.TotalOkQtyProperty).DontMapColumn();
            Meta.Property(ReportRecord.TotalNgQtyProperty).DontMapColumn();
            Meta.Property(ReportRecord.DefectNamesProperty).DontMapColumn();
            Meta.Property(ReportRecord.DefectIdsProperty).DontMapColumn();
            Meta.Property(ReportRecord.DefectNamesDisplayProperty).DontMapColumn();
            Meta.Property(ReportRecord.WipBatchNosProperty).DontMapColumn();
            Meta.Property(ReportRecord.ShiftIdProperty).ColumnMeta.IsNullable();
            Meta.EnablePhantoms();
        }
    }
}