using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Checks.Plans;
using SIE.EMS.Enums;
using SIE.EMS.Maintains.Confirmations;
using SIE.EMS.Maintains.Projects;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.Maintains.Plans
{
    /// <summary>
    /// 保养计划维护
    /// </summary>
    [RootEntity, Serializable]
    [DisplayMember(nameof(MaintainNo))]
    [Label("保养计划")]
    public partial class MaintainPlan : DataEntity
    {
        #region 设备编码 EquipAccount
        /// <summary>
        /// 设备编码Id
        /// </summary>
        [Label("设备编码")]
        public static readonly IRefIdProperty EquipAccountIdProperty = P<MaintainPlan>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备编码Id
        /// </summary>
        public double EquipAccountId
        {
            get { return (double)GetRefId(EquipAccountIdProperty); }
            set { SetRefId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备编码
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty
            = P<MaintainPlan>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备编码
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return GetRefEntity(EquipAccountProperty); }
            set { SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 机台号 MachineNo
        /// <summary>
        /// 机台号
        /// </summary>
        [Label("机台号")]
        public static readonly Property<string> MachineNoProperty = P<MaintainPlan>.RegisterView(e => e.MachineNo, p => p.EquipAccount.Name);

        /// <summary>
        /// 机台号
        /// </summary>
        public string MachineNo
        {
            get { return GetProperty(MachineNoProperty); }
            set { SetProperty(MachineNoProperty, value); }
        }
        #endregion

        #region 保养单号 MaintainNo
        /// <summary>
        /// 保养单号
        /// </summary>
        [Label("保养单号")]
        public static readonly Property<string> MaintainNoProperty = P<MaintainPlan>.Register(e => e.MaintainNo);

        /// <summary>
        /// 保养单号
        /// </summary>
        public string MaintainNo
        {
            get { return GetProperty(MaintainNoProperty); }
            set { SetProperty(MaintainNoProperty, value); }
        }
        #endregion

        #region 计划开始时间 PlanBeginDate
        /// <summary>
        /// 计划开始时间
        /// </summary>
        [Label("计划开始时间")]
        public static readonly Property<DateTime> PlanBeginDateProperty = P<MaintainPlan>.Register(e => e.PlanBeginDate);

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime PlanBeginDate
        {
            get { return GetProperty(PlanBeginDateProperty); }
            set { SetProperty(PlanBeginDateProperty, value); }
        }
        #endregion

        #region 计划结束时间 PlanEndDate
        /// <summary>
        /// 计划结束时间
        /// </summary>
        [Label("计划结束时间")]
        public static readonly Property<DateTime> PlanEndDateProperty = P<MaintainPlan>.Register(e => e.PlanEndDate);

        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime PlanEndDate
        {
            get { return GetProperty(PlanEndDateProperty); }
            set { SetProperty(PlanEndDateProperty, value); }
        }
        #endregion

        #region 当月保养开始日期 BeginDay
        /// <summary>
        /// 当月保养开始日期
        /// </summary>
        [Label("保养开始日")]
        public static readonly Property<int?> BeginDayProperty = P<MaintainPlan>.Register(e => e.BeginDay);

        /// <summary>
        /// 当月保养开始日期
        /// </summary>
        public int? BeginDay
        {
            get { return GetProperty(BeginDayProperty); }
            set { SetProperty(BeginDayProperty, value); }
        }
        #endregion

        #region 当月保养结束日期 EndDay
        /// <summary>
        /// 当月保养结束日期
        /// </summary>
        [Label("保养结束日")]
        public static readonly Property<int?> EndDayProperty = P<MaintainPlan>.Register(e => e.EndDay);

        /// <summary>
        /// 当月保养结束日期
        /// </summary>
        public int? EndDay
        {
            get { return GetProperty(EndDayProperty); }
            set { SetProperty(EndDayProperty, value); }
        }
        #endregion

        #region 保养执行状态 ExeState
        /// <summary>
        /// 保养执行状态
        /// </summary>
        [Label("保养执行状态")]
        public static readonly Property<MaintExeState> ExeStateProperty = P<MaintainPlan>.Register(e => e.ExeState);

        /// <summary>
        /// 保养执行状态
        /// </summary>
        public MaintExeState ExeState
        {
            get { return GetProperty(ExeStateProperty); }
            set { SetProperty(ExeStateProperty, value); }
        }
        #endregion

        #region 是否存在NG项目 IsExsitNgProject
        /// <summary>
        /// 是否存在NG项目
        /// </summary>
        [Label("是否存在NG项目")]
        public static readonly Property<YesNo?> IsExsitNgProjectProperty = P<MaintainPlan>.Register(e => e.IsExsitNgProject);

        /// <summary>
        /// 是否存在NG项目
        /// </summary>
        public YesNo? IsExsitNgProject
        {
            get { return GetProperty(IsExsitNgProjectProperty); }
            set { SetProperty(IsExsitNgProjectProperty, value); }
        }
        #endregion

        #region 保养开始时间 ActBeginDate
        /// <summary>
        /// 保养开始时间
        /// </summary>
        [Label("保养开始时间")]
        public static readonly Property<DateTime?> ActBeginDateProperty = P<MaintainPlan>.Register(e => e.ActBeginDate);

        /// <summary>
        /// 保养开始时间
        /// </summary>
        public DateTime? ActBeginDate
        {
            get { return GetProperty(ActBeginDateProperty); }
            set { SetProperty(ActBeginDateProperty, value); }
        }
        #endregion

        #region 保养结束时间 ActEndDate
        /// <summary>
        /// 保养结束时间
        /// </summary>
        [Label("保养结束时间")]
        public static readonly Property<DateTime?> ActEndDateProperty = P<MaintainPlan>.Register(e => e.ActEndDate);

        /// <summary>
        /// 保养结束时间
        /// </summary>
        public DateTime? ActEndDate
        {
            get { return GetProperty(ActEndDateProperty); }
            set { SetProperty(ActEndDateProperty, value); }
        }
        #endregion

        #region 保养执行人 ExecuteBy
        /// <summary>
        /// 保养执行人Id
        /// </summary>
        [Label("保养执行人")]
        public static readonly IRefIdProperty ExecuteByIdProperty = P<MaintainPlan>.RegisterRefId(e => e.ExecuteById, ReferenceType.Normal);

        /// <summary>
        /// 保养执行人Id
        /// </summary>
        public double? ExecuteById
        {
            get { return (double?)GetRefNullableId(ExecuteByIdProperty); }
            set { SetRefNullableId(ExecuteByIdProperty, value); }
        }

        /// <summary>
        /// 保养执行人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ExecuteByProperty = P<MaintainPlan>.RegisterRef(e => e.ExecuteBy, ExecuteByIdProperty);

        /// <summary>
        /// 保养执行人
        /// </summary>
        public Employee ExecuteBy
        {
            get { return GetRefEntity(ExecuteByProperty); }
            set { SetRefEntity(ExecuteByProperty, value); }
        }
        #endregion

        #region 评分人 ScoreBy
        /// <summary>
        /// 评分人Id
        /// </summary>
        [Label("评分人")]
        public static readonly IRefIdProperty ScoreByIdProperty = P<MaintainPlan>.RegisterRefId(e => e.ScoreById, ReferenceType.Normal);

        /// <summary>
        /// 评分人Id
        /// </summary>
        public double? ScoreById
        {
            get { return (double?)GetRefNullableId(ScoreByIdProperty); }
            set { SetRefNullableId(ScoreByIdProperty, value); }
        }

        /// <summary>
        /// 评分人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ScoreByProperty = P<MaintainPlan>.RegisterRef(e => e.ScoreBy, ScoreByIdProperty);

        /// <summary>
        /// 评分人
        /// </summary>
        public Employee ScoreBy
        {
            get { return GetRefEntity(ScoreByProperty); }
            set { SetRefEntity(ScoreByProperty, value); }
        }
        #endregion

        #region 评分时间 ScoreDate
        /// <summary>
        /// 评分时间
        /// </summary>
        [Label("评分时间")]
        public static readonly Property<DateTime?> ScoreDateProperty = P<MaintainPlan>.Register(e => e.ScoreDate);

        /// <summary>
        /// 评分时间
        /// </summary>
        public DateTime? ScoreDate
        {
            get { return GetProperty(ScoreDateProperty); }
            set { SetProperty(ScoreDateProperty, value); }
        }
        #endregion

        #region 评分 Score
        /// <summary>
        /// 评分
        /// </summary>
        [Label("评分")]
        public static readonly Property<double?> ScoreProperty = P<MaintainPlan>.Register(e => e.Score);

        /// <summary>
        /// 评分
        /// </summary>
        public double? Score
        {
            get { return GetProperty(ScoreProperty); }
            set { SetProperty(ScoreProperty, value); }
        }
        #endregion

        #region 保养项目 ProjectList
        /// <summary>
        /// 保养项目
        /// </summary>
        public static readonly ListProperty<EntityList<MaintainProject>> ProjectListProperty = P<MaintainPlan>.RegisterList(e => e.ProjectList);

        /// <summary>
        /// 保养项目
        /// </summary>
        public EntityList<MaintainProject> ProjectList
        {
            get { return this.GetLazyList(ProjectListProperty); }
        }
        #endregion

        #region 备件更换 MaintainPlanSparePartList
        /// <summary>
        /// 备件更换
        /// </summary>
        public static readonly ListProperty<EntityList<MaintainPlanSparePart>> MaintainPlanSparePartListProperty = P<MaintainPlan>.RegisterList(e => e.MaintainPlanSparePartList);

        /// <summary>
        /// 备件更换
        /// </summary>
        public EntityList<MaintainPlanSparePart> MaintainPlanSparePartList
        {
            get { return this.GetLazyList(MaintainPlanSparePartListProperty); }
        }
        #endregion

        #region 备件申请 MaintainPlanSparePartAplList
        /// <summary>
        /// 备件申请
        /// </summary>
        [Label("备件申请")]
        public static readonly ListProperty<EntityList<MaintainPlanSparePartApl>> MaintainPlanSparePartAplListProperty = P<MaintainPlan>.RegisterList(e => e.MaintainPlanSparePartAplList);

        /// <summary>
        /// 备件申请
        /// </summary>
        public EntityList<MaintainPlanSparePartApl> MaintainPlanSparePartAplList
        {
            get { return this.GetLazyList(MaintainPlanSparePartAplListProperty); }
        }
        #endregion

        #region 图片上传 MaintainPlanAttachmentList
        /// <summary>
        /// 图片上传
        /// </summary>
        [Label("图片上传")]
        public static readonly ListProperty<EntityList<MaintainPlanAttachment>> MaintainPlanAttachmentListProperty = P<MaintainPlan>.RegisterList(e => e.MaintainPlanAttachmentList);

        /// <summary>
        /// 图片上传
        /// </summary>
        public EntityList<MaintainPlanAttachment> MaintainPlanAttachmentList
        {
            get { return this.GetLazyList(MaintainPlanAttachmentListProperty); }
        }
        #endregion

        #region 工时登记 WorkHoursRegisterList
        /// <summary>
        /// 工时登记
        /// </summary>
        public static readonly ListProperty<EntityList<WorkHoursRegister>> WorkHoursRegisterListProperty = P<MaintainPlan>.RegisterList(e => e.WorkHoursRegisterList);

        /// <summary>
        /// 工时登记
        /// </summary>
        public EntityList<WorkHoursRegister> WorkHoursRegisterList
        {
            get { return this.GetLazyList(WorkHoursRegisterListProperty); }
        }
        #endregion

        #region 责任部门 Department
        /// <summary>
        /// 责任部门Id
        /// </summary>
        [Label("责任部门")]
        public static readonly IRefIdProperty DepartmentIdProperty = P<MaintainPlan>.RegisterRefId(e => e.DepartmentId, ReferenceType.Normal);

        /// <summary>
        /// 责任部门Id
        /// </summary>
        public double? DepartmentId
        {
            get { return (double?)GetRefNullableId(DepartmentIdProperty); }
            set { SetRefNullableId(DepartmentIdProperty, value); }
        }

        /// <summary>
        /// 责任部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> DepartmentProperty = P<MaintainPlan>.RegisterRef(e => e.Department, DepartmentIdProperty);

        /// <summary>
        /// 责任部门
        /// </summary>
        public Enterprise Department
        {
            get { return GetRefEntity(DepartmentProperty); }
            set { SetRefEntity(DepartmentProperty, value); }
        }
        #endregion

        #region 保养时长(H) MaintainTime
        /// <summary>
        /// 保养时长(H)
        /// </summary>
        [Label("计划保养时长(H)")]
        public static readonly Property<decimal?> MaintainTimeProperty = P<MaintainPlan>.Register(e => e.MaintainTime);

        /// <summary>
        /// 保养时长(H)
        /// </summary>
        public decimal? MaintainTime
        {
            get { return this.GetProperty(MaintainTimeProperty); }
            set { this.SetProperty(MaintainTimeProperty, value); }
        }
        #endregion

        #region 设备保养类型 EquipMaintainType
        /// <summary>
        /// 设备保养类型
        /// </summary>
        [Label("设备保养类型")]
        public static readonly Property<EquipCheckType> EquipMaintainTypeProperty = P<MaintainPlan>.Register(e => e.EquipMaintainType);

        /// <summary>
        /// 设备保养类型
        /// </summary>
        public EquipCheckType EquipMaintainType
        {
            get { return this.GetProperty(EquipMaintainTypeProperty); }
            set { this.SetProperty(EquipMaintainTypeProperty, value); }
        }
        #endregion

        #region 执行结果 ExeResult
        /// <summary>
        /// 执行结果
        /// </summary>
        [Label("执行结果")]
        public static readonly Property<ExeResult?> ExeResultProperty = P<MaintainPlan>.Register(e => e.ExeResult);

        /// <summary>
        /// 执行结果
        /// </summary>
        public ExeResult? ExeResult
        {
            get { return this.GetProperty(ExeResultProperty); }
            set { this.SetProperty(ExeResultProperty, value); }
        }
        #endregion

        #region 保养人 MaintainEmployee
        /// <summary>
        /// 保养人Id
        /// </summary>
        [Label("保养人")]
        public static readonly IRefIdProperty MaintainEmployeeIdProperty =
            P<MaintainPlan>.RegisterRefId(e => e.MaintainEmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 保养人Id
        /// </summary>
        public double? MaintainEmployeeId
        {
            get { return (double?)this.GetRefId(MaintainEmployeeIdProperty); }
            set { this.SetRefId(MaintainEmployeeIdProperty, value); }
        }

        /// <summary>
        /// 保养人
        /// </summary>
        public static readonly RefEntityProperty<Employee> MaintainEmployeeProperty =
            P<MaintainPlan>.RegisterRef(e => e.MaintainEmployee, MaintainEmployeeIdProperty);

        /// <summary>
        /// 保养人
        /// </summary>
        public Employee MaintainEmployee
        {
            get { return this.GetRefEntity(MaintainEmployeeProperty); }
            set { this.SetRefEntity(MaintainEmployeeProperty, value); }
        }
        #endregion

        #region 上次保养小结 UpMaintainSummary
        /// <summary>
        /// 上次保养小结
        /// </summary>
        [Label("上次保养小结")]
        [MaxLength(2000)]
        public static readonly Property<string> UpMaintainSummaryProperty = P<MaintainPlan>.Register(e => e.UpMaintainSummary);

        /// <summary>
        /// 保养小结
        /// </summary>
        public string UpMaintainSummary
        {
            get { return this.GetProperty(UpMaintainSummaryProperty); }
            set { this.SetProperty(UpMaintainSummaryProperty, value); }
        }
        #endregion

        #region 保养小结 MaintainSummary
        /// <summary>
        /// 保养小结
        /// </summary>
        [Label("保养小结")]
        [MaxLength(2000)]
        public static readonly Property<string> MaintainSummaryProperty = P<MaintainPlan>.Register(e => e.MaintainSummary);

        /// <summary>
        /// 保养小结
        /// </summary>
        public string MaintainSummary
        {
            get { return this.GetProperty(MaintainSummaryProperty); }
            set { this.SetProperty(MaintainSummaryProperty, value); }
        }
        #endregion

        #region 来源 MaintainSourceType
        /// <summary>
        /// 来源
        /// </summary>
        [Label("来源")]
        public static readonly Property<CheckSourceType> MaintainSourceTypeProperty = P<MaintainPlan>.Register(e => e.MaintainSourceType);

        /// <summary>
        /// 来源
        /// </summary>
        public CheckSourceType MaintainSourceType
        {
            get { return this.GetProperty(MaintainSourceTypeProperty); }
            set { this.SetProperty(MaintainSourceTypeProperty, value); }
        }
        #endregion

        #region 总工时 SumWorkHours
        /// <summary>
        /// 总工时
        /// </summary>
        [Label("总工时")]
        public static readonly Property<double> SumWorkHoursProperty = P<MaintainPlan>.Register(e => e.SumWorkHours);

        /// <summary>
        /// 总工时
        /// </summary>
        public double SumWorkHours
        {
            get { return GetProperty(SumWorkHoursProperty); }
            set { SetProperty(SumWorkHoursProperty, value); }
        }
        #endregion

        #region 年/月 YearAndMonth
        /// <summary>
        /// 年/月
        /// </summary>
        [Label("年/月")]
        public static readonly Property<DateTime?> YearAndMonthProperty = P<MaintainPlan>.Register(e => e.YearAndMonth);

        /// <summary>
        /// 年/月
        /// </summary>
        public DateTime? YearAndMonth
        {
            get { return GetProperty(YearAndMonthProperty); }
            set { SetProperty(YearAndMonthProperty, value); }
        }
        #endregion

        #region 周期 Cycle
        /// <summary>
        /// 周期
        /// </summary>
        [Label("周期")]
        public static readonly Property<int> CycleProperty = P<MaintainPlan>.Register(e => e.Cycle);

        /// <summary>
        /// 周期
        /// </summary>
        public int Cycle
        {
            get { return this.GetProperty(CycleProperty); }
            set { this.SetProperty(CycleProperty, value); }
        }
        #endregion

        #region 保养类型 MaintainType
        /// <summary>
        /// 保养类型
        /// </summary>
        [Label("保养类型")]
        public static readonly Property<MaintainType> MaintainTypeProperty = P<MaintainPlan>.Register(e => e.MaintainType);

        /// <summary>
        /// 保养类型
        /// </summary>
        public MaintainType MaintainType
        {
            get { return this.GetProperty(MaintainTypeProperty); }
            set { this.SetProperty(MaintainTypeProperty, value); }
        }
        #endregion

        #region 指定计划开始时间 PrecisePlanBeginDate
        /// <summary>
        /// 指定计划开始时间
        /// </summary>
        [Label("指定计划开始时间")]
        public static readonly Property<DateTime?> PrecisePlanBeginDateProperty = P<MaintainPlan>.Register(e => e.PrecisePlanBeginDate);

        /// <summary>
        /// 指定计划开始时间
        /// </summary>
        public DateTime? PrecisePlanBeginDate
        {
            get { return GetProperty(PrecisePlanBeginDateProperty); }
            set { SetProperty(PrecisePlanBeginDateProperty, value); }
        }
        #endregion

        #region 指定计划结束时间 PrecisePlanEndDate
        /// <summary>
        /// 指定计划结束时间
        /// </summary>
        [Label("指定计划结束时间")]
        public static readonly Property<DateTime?> PrecisePlanEndDateProperty = P<MaintainPlan>.Register(e => e.PrecisePlanEndDate);

        /// <summary>
        /// 指定计划结束时间
        /// </summary>
        public DateTime? PrecisePlanEndDate
        {
            get { return GetProperty(PrecisePlanEndDateProperty); }
            set { SetProperty(PrecisePlanEndDateProperty, value); }
        }
        #endregion

        #region 保养确认信息 MaintainConfirmationList
        /// <summary>
        /// 点检确认信息
        /// </summary>
        [Label("点检确认信息")]
        public static readonly ListProperty<EntityList<MaintainConfirmation>> MaintainConfirmationListProperty = P<MaintainPlan>.RegisterList(e => e.MaintainConfirmationList);

        /// <summary>
        /// 点检确认信息
        /// </summary>
        /// <returns>点检确认信息</returns>
        public EntityList<MaintainConfirmation> MaintainConfirmationList
        {
            get { return GetLazyList(MaintainConfirmationListProperty); }
        }
        #endregion

        #region 是否异常推送 IsAbnormalInfoPush
        /// <summary>
        /// 是否异常推送
        /// </summary>
        [Label("是否异常推送")]
        public static readonly Property<bool> IsAbnormalInfoPushProperty = P<MaintainPlan>.Register(e => e.IsAbnormalInfoPush);

        /// <summary>
        /// 是否异常推送
        /// </summary>
        public bool IsAbnormalInfoPush
        {
            get { return this.GetProperty(IsAbnormalInfoPushProperty); }
            set { this.SetProperty(IsAbnormalInfoPushProperty, value); }
        }
        #endregion

        #region 确认结果 ConfirmResult
        /// <summary>
        /// 确认结果
        /// </summary>
        [Label("确认结果")]
        public static readonly Property<ConfirmResult?> ConfirmResultProperty = P<MaintainPlan>.Register(e => e.ConfirmResult);

        /// <summary>
        /// 确认结果
        /// </summary>
        public ConfirmResult? ConfirmResult
        {
            get { return GetProperty(ConfirmResultProperty); }
            set { SetProperty(ConfirmResultProperty, value); }
        }
        #endregion

        #region 确认备注 ConfirmNote
        /// <summary>
        /// 确认备注
        /// </summary>
        [Label("确认备注")]
        [MaxLength(1000)]
        public static readonly Property<string> ConfirmNoteProperty = P<MaintainPlan>.Register(e => e.ConfirmNote);

        /// <summary>
        /// 确认备注
        /// </summary>
        public string ConfirmNote
        {
            get { return GetProperty(ConfirmNoteProperty); }
            set { SetProperty(ConfirmNoteProperty, value); }
        }
        #endregion

        #region 是否已报修 WhetherRepair
        /// <summary>
        /// 是否已报修
        /// </summary>
        [Label("是否已报修")]
        public static readonly Property<YesNo> WhetherRepairProperty = P<MaintainPlan>.Register(e => e.WhetherRepair);

        /// <summary>
        /// 是否已报修
        /// </summary>
        public YesNo WhetherRepair
        {
            get { return this.GetProperty(WhetherRepairProperty); }
            set { this.SetProperty(WhetherRepairProperty, value); }
        }
        #endregion

        #region 是否已开始报修 WhetherBegin
        /// <summary>
        /// 是否已开始报修
        /// </summary>
        [Label("是否已开始报修")]
        public static readonly Property<bool> WhetherBeginProperty = P<MaintainPlan>.Register(e => e.WhetherBegin);

        /// <summary>
        /// 是否已开始报修
        /// </summary>
        public bool WhetherBegin
        {
            get { return this.GetProperty(WhetherBeginProperty); }
            set { this.SetProperty(WhetherBeginProperty, value); }
        }
        #endregion

        #region 不映射数据库的属性      

        #region 产线 Resource
        /// <summary>
        /// 产线Id
        /// </summary>
        [Label("产线")]
        public static readonly IRefIdProperty ResourceIdProperty = P<MaintainPlan>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 产线Id
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)GetRefNullableId(ResourceIdProperty); }
            set { SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 产线
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> ResourceProperty = P<MaintainPlan>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public Enterprise Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 选择开始时间 SelectBeginTime
        /// <summary>
        /// 选择开始时间
        /// </summary>
        [Label("选择开始时间")]
        public static readonly Property<DateTime?> SelectBeginTimeProperty = P<MaintainPlan>.Register(e => e.SelectBeginTime);

        /// <summary>
        /// 选择时间
        /// </summary>
        public DateTime? SelectBeginTime
        {
            get { return this.GetProperty(SelectBeginTimeProperty); }
            set { this.SetProperty(SelectBeginTimeProperty, value); }
        }
        #endregion

        #region 选择结束时间 SelectEndTime
        /// <summary>
        /// 选择结束时间
        /// </summary>
        [Label("选择结束时间")]
        public static readonly Property<DateTime?> SelectEndTimeProperty = P<MaintainPlan>.Register(e => e.SelectEndTime);

        /// <summary>
        /// 选择结束时间
        /// </summary>
        public DateTime? SelectEndTime
        {
            get { return this.GetProperty(SelectEndTimeProperty); }
            set { this.SetProperty(SelectEndTimeProperty, value); }
        }
        #endregion

        

        #region 确认部门 ConfirmDept
        /// <summary>
        /// 确认部门Id
        /// </summary>
        [Label("确认部门")]
        public static readonly Property<double?> ConfirmDeptIdProperty = P<MaintainPlan>.Register(e => e.ConfirmDeptId);

        /// <summary>
        /// 确认部门Id
        /// </summary>
        public double? ConfirmDeptId
        {
            get { return GetProperty(ConfirmDeptIdProperty); }
            set { SetProperty(ConfirmDeptIdProperty, value); }
        }
        #endregion

        #region 周期类型 MaintainCycleType
        /// <summary>
        /// 周期类型
        /// </summary>
        [Label("周期类型")]
        public static readonly Property<MaintainCycleType> MaintainCycleTypeProperty = P<MaintainPlan>.Register(e => e.MaintainCycleType);

        /// <summary>
        /// 周期类型
        /// </summary>
        public MaintainCycleType MaintainCycleType
        {
            get { return this.GetProperty(MaintainCycleTypeProperty); }
            set { this.SetProperty(MaintainCycleTypeProperty, value); }
        }
        #endregion

        #region 保养类型 MaintainTypeInfo
        /// <summary>
        /// 保养类型Id
        /// </summary>
        [Label("保养类型")]
        public static readonly IRefIdProperty MaintainTypeInfoIdProperty =
            P<MaintainPlan>.RegisterRefId(e => e.MaintainTypeInfoId, ReferenceType.Normal);

        /// <summary>
        /// 保养类型Id
        /// </summary>
        public string MaintainTypeInfoId
        {
            get { return (string)this.GetRefId(MaintainTypeInfoIdProperty); }
            set { this.SetRefId(MaintainTypeInfoIdProperty, value); }
        }

        /// <summary>
        /// 保养类型
        /// </summary>
        public static readonly RefEntityProperty<MaintainTypeInfo> MaintainTypeInfoProperty =
            P<MaintainPlan>.RegisterRef(e => e.MaintainTypeInfo, MaintainTypeInfoIdProperty);

        /// <summary>
        /// 保养类型
        /// </summary>
        public MaintainTypeInfo MaintainTypeInfo
        {
            get { return this.GetRefEntity(MaintainTypeInfoProperty); }
            set { this.SetRefEntity(MaintainTypeInfoProperty, value); }
        }
        #endregion

        #region 确认部门 ConfirmDeptDisplay
        /// <summary>
        /// 确认部门
        /// </summary>
        [Label("确认部门")]
        public static readonly Property<string> ConfirmDeptDisplayProperty = P<MaintainPlan>.Register(e => e.ConfirmDeptDisplay);

        /// <summary>
        /// 确认部门
        /// </summary>
        public string ConfirmDeptDisplay
        {
            get { return this.GetProperty(ConfirmDeptDisplayProperty); }
            set { this.SetProperty(ConfirmDeptDisplayProperty, value); }
        }
        #endregion

        #endregion

        #region 视图属性

        #region 设备编码 EquipAccountCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipAccountCodeProperty = P<MaintainPlan>.RegisterView(e => e.EquipAccountCode, p => p.EquipAccount.Code);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipAccountCode
        {
            get { return GetProperty(EquipAccountCodeProperty); }
            set { SetProperty(EquipAccountCodeProperty, value); }
        }
        #endregion

        #region 设备名称 EquipAccountName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipAccountNameProperty = P<MaintainPlan>.RegisterView(e => e.EquipAccountName, p => p.EquipAccount.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipAccountName
        {
            get { return this.GetProperty(EquipAccountNameProperty); }
        }
        #endregion

        #region 设备型号Id EquipModelId
        /// <summary>
        /// 设备型号Id
        /// </summary>
        [Label("设备型号Id")]
        public static readonly Property<double> EquipModelIdProperty = P<MaintainPlan>.RegisterView(e => e.EquipModelId, p => p.EquipAccount.EquipModelId);

        /// <summary>
        /// 设备型号Id
        /// </summary>
        public double EquipModelId
        {
            get { return this.GetProperty(EquipModelIdProperty); }
        }
        #endregion

        #region 型号编码 EquipModelCode
        /// <summary>
        /// 型号编码
        /// </summary>
        [Label("型号编码")]
        public static readonly Property<string> EquipModelCodeProperty = P<MaintainPlan>.RegisterView(e => e.EquipModelCode, p => p.EquipAccount.EquipModel.Code);

        /// <summary>
        /// 型号编码
        /// </summary>
        public string EquipModelCode
        {
            get { return this.GetProperty(EquipModelCodeProperty); }
        }
        #endregion

        #region 型号名称 EquipModelName
        /// <summary>
        /// 型号名称
        /// </summary>
        [Label("型号名称")]
        public static readonly Property<string> EquipModelNameProperty = P<MaintainPlan>.RegisterView(e => e.EquipModelName, p => p.EquipAccount.EquipModel.Name);

        /// <summary>
        /// 型号名称
        /// </summary>
        public string EquipModelName
        {
            get { return this.GetProperty(EquipModelNameProperty); }
        }
        #endregion

        #region 设备类型ID EquipTypeId
        /// <summary>
        /// 设备类型ID
        /// </summary>
        [Label("设备类型ID")]
        public static readonly Property<double> EquipTypeIdProperty = P<MaintainPlan>.RegisterView(e => e.EquipTypeId, p => p.EquipAccount.EquipModel.EquipTypeId);

        /// <summary>
        /// 设备类型ID
        /// </summary>
        public double EquipTypeId
        {
            get { return this.GetProperty(EquipTypeIdProperty); }
        }
        #endregion

        #region 类型编码 EquipTypeCode
        /// <summary>
        /// 类型编码
        /// </summary>
        [Label("类型编码")]
        public static readonly Property<string> EquipTypeCodeProperty = P<MaintainPlan>.RegisterView(e => e.EquipTypeCode, p => p.EquipAccount.EquipModel.EquipType.TypeCode);

        /// <summary>
        /// 类型编码
        /// </summary>
        public string EquipTypeCode
        {
            get { return this.GetProperty(EquipTypeCodeProperty); }
        }
        #endregion

        #region 类型名称 EquipTypeName
        /// <summary>
        /// 类型名称
        /// </summary>
        [Label("类型名称")]
        public static readonly Property<string> EquipTypeNameProperty = P<MaintainPlan>.RegisterView(e => e.EquipTypeName, p => p.EquipAccount.EquipModel.EquipType.TypeName);

        /// <summary>
        /// 类型名称
        /// </summary>
        public string EquipTypeName
        {
            get { return this.GetProperty(EquipTypeNameProperty); }
        }
        #endregion

        #region 设备车间名称 WorkShopName
        /// <summary>
        /// 设备车间名称
        /// </summary>
        [Label("设备车间名称")]
        public static readonly Property<string> WorkShopNameProperty = P<MaintainPlan>.RegisterView(e => e.WorkShopName, p => p.EquipAccount.WorkShop.Name);

        /// <summary>
        /// 设备车间名称
        /// </summary>
        public string WorkShopName
        {
            get { return this.GetProperty(WorkShopNameProperty); }
        }
        #endregion

        #region 设备产线名称 ResourceName
        /// <summary>
        /// 设备产线名称
        /// </summary>
        [Label("设备产线名称")]
        public static readonly Property<string> ResourceNameProperty = P<MaintainPlan>.RegisterView(e => e.ResourceName, p => p.EquipAccount.Resource.Name);

        /// <summary>
        /// 设备产线名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
        }
        #endregion

        #region 部门编码 DepartmentCode
        /// <summary>
        /// 部门编码
        /// </summary>
        [Label("部门编码")]
        public static readonly Property<string> DepartmentCodeProperty = P<MaintainPlan>.RegisterView(e => e.DepartmentCode, p => p.Department.Code);

        /// <summary>
        /// 部门编码
        /// </summary>
        public string DepartmentCode
        {
            get { return this.GetProperty(DepartmentCodeProperty); }
        }
        #endregion

        #region 部门名称 DepartmentName
        /// <summary>
        /// 部门名称
        /// </summary>
        [Label("部门名称")]
        public static readonly Property<string> DepartmentNameProperty = P<MaintainPlan>.RegisterView(e => e.DepartmentName, p => p.Department.Name);

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName
        {
            get { return this.GetProperty(DepartmentNameProperty); }
        }
        #endregion

        #region 保养执行人 ExecuteByName
        /// <summary>
        /// 保养执行人
        /// </summary>
        [Label("保养执行人")]
        public static readonly Property<string> ExecuteByNameProperty = P<MaintainPlan>.RegisterView(e => e.ExecuteByName, p => p.ExecuteBy.Name);

        /// <summary>
        /// 保养执行人
        /// </summary>
        public string ExecuteByName
        {
            get { return this.GetProperty(ExecuteByNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 保养计划维护 实体配置
    /// </summary>
    internal class MaintainPlanConfig : EntityConfig<MaintainPlan>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(new HandlerRule
            {
                Handler = (o, e) =>
                {
                    var plan = o.CastTo<MaintainPlan>();
                    var errMsg = string.Empty;

                    if (plan.ActBeginDate.HasValue && plan.ActEndDate.HasValue)
                    {
                        var actBeginDate = plan.ActBeginDate.Value.Date;
                        var actEndDate = plan.ActEndDate.Value.Date;
                        if (actBeginDate > actEndDate)
                            errMsg = "保养开始时间不能少小保养结束时间。".L10N();
                    }
                    if (errMsg.IsNotEmpty())
                        e.BrokenDescription = errMsg;
                }
            });
        }

        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_MAINTAIN_PLAN").MapAllProperties();
            Meta.Property(MaintainPlan.ResourceIdProperty).DontMapColumn();
            Meta.Property(MaintainPlan.ResourceProperty).DontMapColumn();
            Meta.Property(MaintainPlan.SelectBeginTimeProperty).DontMapColumn();
            Meta.Property(MaintainPlan.SelectEndTimeProperty).DontMapColumn();
            Meta.Property(MaintainPlan.IsAbnormalInfoPushProperty).DontMapColumn();
            Meta.Property(MaintainPlan.ConfirmDeptIdProperty).DontMapColumn();
            Meta.Property(MaintainPlan.ConfirmDeptDisplayProperty).DontMapColumn();
            Meta.Property(MaintainPlan.MaintainCycleTypeProperty).DontMapColumn();
            Meta.Property(MaintainPlan.MaintainTypeInfoIdProperty).DontMapColumn();
            Meta.Property(MaintainPlan.ConfirmNoteProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}