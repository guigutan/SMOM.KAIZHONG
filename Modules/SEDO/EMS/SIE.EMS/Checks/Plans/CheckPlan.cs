using SIE.Domain;
using SIE.EMS.Checks.Confirmations;
using SIE.EMS.Checks.Projects;
using SIE.EMS.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.Checks.Plans
{
    /// <summary>
    /// 点检计划维护
    /// </summary>
    [RootEntity, Serializable]
    [Label("点检计划")]
    public partial class CheckPlan : DataEntity
    {
        #region 点检单号 CheckPlanNo
        /// <summary>
        /// 点检单号
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("点检单号")]
        public static readonly Property<string> CheckPlanNoProperty = P<CheckPlan>.Register(e => e.CheckPlanNo);

        /// <summary>
        /// 点检单号
        /// </summary>
        public string CheckPlanNo
        {
            get { return GetProperty(CheckPlanNoProperty); }
            set { SetProperty(CheckPlanNoProperty, value); }
        }
        #endregion

        #region 点检执行时间 CheckDate
        /// <summary>
        /// 点检执行时间
        /// </summary>
        [Label("点检执行时间")]
        public static readonly Property<DateTime?> CheckDateProperty = P<CheckPlan>.Register(e => e.CheckDate);

        /// <summary>
        /// 点检执行时间
        /// </summary>
        public DateTime? CheckDate
        {
            get { return GetProperty(CheckDateProperty); }
            set { SetProperty(CheckDateProperty, value); }
        }
        #endregion

        #region 点检确认时间 ConfirmDate
        /// <summary>
        /// 点检确认时间
        /// </summary>
        [Label("点检确认时间")]
        public static readonly Property<DateTime?> ConfirmDateProperty = P<CheckPlan>.Register(e => e.ConfirmDate);

        /// <summary>
        /// 点检确认时间
        /// </summary>
        public DateTime? ConfirmDate
        {
            get { return this.GetProperty(ConfirmDateProperty); }
            set { this.SetProperty(ConfirmDateProperty, value); }
        }
        #endregion

        #region 设备编码 EquipAccount
        /// <summary>
        /// 设备编码Id
        /// </summary>
        [Label("设备编码Id")]
        public static readonly IRefIdProperty EquipAccountIdProperty = P<CheckPlan>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备编码Id
        /// </summary>
        public double EquipAccountId
        {
            get { return (double)GetRefId(EquipAccountIdProperty); }
            set { SetRefId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备编码Id
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty = P<CheckPlan>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备编码Id
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return GetRefEntity(EquipAccountProperty); }
            set { SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 设备名称 MachineNo
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> MachineNoProperty = P<CheckPlan>.RegisterView(e => e.MachineNo, p => p.EquipAccount.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string MachineNo
        {
            get { return GetProperty(MachineNoProperty); }
            set { SetProperty(MachineNoProperty, value); }
        }
        #endregion

        #region 点检周期类型 CheckCycleType
        /// <summary>
        /// 点检周期类型
        /// </summary>
        [Label("点检周期类型")]
        public static readonly Property<CheckCycleType> CheckCycleTypeProperty = P<CheckPlan>.Register(e => e.CheckCycleType);

        /// <summary>
        /// 点检周期类型
        /// </summary>
        public CheckCycleType CheckCycleType
        {
            get { return GetProperty(CheckCycleTypeProperty); }
            set { SetProperty(CheckCycleTypeProperty, value); }
        }
        #endregion

        #region 点检计划类型 CheckPlanType
        /// <summary>
        /// 点检计划类型
        /// </summary>
        [Label("点检计划类型")]
        public static readonly Property<CheckPlanType> CheckPlanTypeProperty = P<CheckPlan>.Register(e => e.CheckPlanType);

        /// <summary>
        /// 点检计划类型
        /// </summary>
        public CheckPlanType CheckPlanType
        {
            get { return this.GetProperty(CheckPlanTypeProperty); }
            set { this.SetProperty(CheckPlanTypeProperty, value); }
        }
        #endregion

        #region 点检执行状态 ExeState
        /// <summary>
        /// 点检执行状态
        /// </summary>
        [Label("执行状态")]
        public static readonly Property<CheckExeState> ExeStateProperty = P<CheckPlan>.Register(e => e.ExeState);

        /// <summary>
        /// 点检执行状态
        /// </summary>
        public CheckExeState ExeState
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
        public static readonly Property<YesNo?> IsExsitNgProjectProperty = P<CheckPlan>.Register(e => e.IsExsitNgProject);

        /// <summary>
        /// 是否存在NG项目
        /// </summary>
        public YesNo? IsExsitNgProject
        {
            get { return GetProperty(IsExsitNgProjectProperty); }
            set { SetProperty(IsExsitNgProjectProperty, value); }
        }
        #endregion

        #region 点检项目 CheckProjectList
        /// <summary>
        /// 点检项目
        /// </summary>
        public static readonly ListProperty<EntityList<CheckProject>> CheckProjectListProperty = P<CheckPlan>.RegisterList(e => e.CheckProjectList);
        /// <summary>
        /// 点检项目
        /// </summary>
        public EntityList<CheckProject> CheckProjectList
        {
            get { return this.GetLazyList(CheckProjectListProperty); }
        }
        #endregion

        #region 图片上传 CheckPlanAttachmentList
        /// <summary>
        /// 图片上传
        /// </summary>
        [Label("图片上传")]
        public static readonly ListProperty<EntityList<CheckPlanAttachment>> CheckPlanAttachmentListProperty = P<CheckPlan>.RegisterList(e => e.CheckPlanAttachmentList);

        /// <summary>
        /// 图片上传
        /// </summary>
        public EntityList<CheckPlanAttachment> CheckPlanAttachmentList
        {
            get { return this.GetLazyList(CheckPlanAttachmentListProperty); }
        }
        #endregion

        #region 备件更换 CheckPlanSparePartList
        /// <summary>
        /// 备件更换
        /// </summary>
        [Label("备件更换")]
        public static readonly ListProperty<EntityList<CheckPlanSparePart>> CheckPlanSparePartListProperty = P<CheckPlan>.RegisterList(e => e.CheckPlanSparePartList);

        /// <summary>
        /// 备件更换
        /// </summary>
        public EntityList<CheckPlanSparePart> CheckPlanSparePartList
        {
            get { return this.GetLazyList(CheckPlanSparePartListProperty); }
        }
        #endregion

        #region 备件申请 CheckPlanSparePartAplList
        /// <summary>
        /// 备件申请
        /// </summary>
        [Label("备件申请")]
        public static readonly ListProperty<EntityList<CheckPlanSparePartApl>> CheckPlanSparePartAplListProperty = P<CheckPlan>.RegisterList(e => e.CheckPlanSparePartAplList);

        /// <summary>
        /// 备件申请
        /// </summary>
        public EntityList<CheckPlanSparePartApl> CheckPlanSparePartAplList
        {
            get { return this.GetLazyList(CheckPlanSparePartAplListProperty); }
        }
        #endregion

        #region 计划开始时间 CheckBeginDate
        /// <summary>
        /// 计划开始时间
        /// </summary>
        [Label("计划开始时间")]
        public static readonly Property<DateTime> CheckBeginDateProperty = P<CheckPlan>.Register(e => e.CheckBeginDate);

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime CheckBeginDate
        {
            get { return GetProperty(CheckBeginDateProperty); }
            set { SetProperty(CheckBeginDateProperty, value); }
        }
        #endregion

        #region 计划结束时间 CheckEndDate
        /// <summary>
        /// 计划结束时间
        /// </summary>
        [Label("计划结束时间")]
        public static readonly Property<DateTime> CheckEndDateProperty = P<CheckPlan>.Register(e => e.CheckEndDate);

        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime CheckEndDate
        {
            get { return GetProperty(CheckEndDateProperty); }
            set { SetProperty(CheckEndDateProperty, value); }
        }
        #endregion

        #region 是否跨日 WhetherAcrossDay
        /// <summary>
        /// 是否跨日
        /// </summary>
        [Label("是否跨日")]
        public static readonly Property<YesNo> WhetherAcrossDayProperty = P<CheckPlan>.Register(e => e.WhetherAcrossDay);

        /// <summary>
        /// 是否跨日
        /// </summary>
        public YesNo WhetherAcrossDay
        {
            get { return GetProperty(WhetherAcrossDayProperty); }
            set { SetProperty(WhetherAcrossDayProperty, value); }
        }
        #endregion

        #region 任务开始时间 ActCheckBeginDate
        /// <summary>
        /// 任务开始时间
        /// </summary>
        [Label("任务开始时间")]
        public static readonly Property<DateTime?> ActCheckBeginDateProperty = P<CheckPlan>.Register(e => e.ActCheckBeginDate);

        /// <summary>
        /// 任务开始时间
        /// </summary>
        public DateTime? ActCheckBeginDate
        {
            get { return GetProperty(ActCheckBeginDateProperty); }
            set { SetProperty(ActCheckBeginDateProperty, value); }
        }
        #endregion

        #region 任务结束时间 CheckEndDate
        /// <summary>
        /// 任务结束时间
        /// </summary>
        [Label("任务结束时间")]
        public static readonly Property<DateTime?> ActCheckEndDateProperty = P<CheckPlan>.Register(e => e.ActCheckEndDate);

        /// <summary>
        /// 点检结束时间
        /// </summary>
        public DateTime? ActCheckEndDate
        {
            get { return GetProperty(ActCheckEndDateProperty); }
            set { SetProperty(ActCheckEndDateProperty, value); }
        }
        #endregion

        #region 责任部门 Department
        /// <summary>
        /// 责任部门Id
        /// </summary>
        [Label("责任部门")]
        public static readonly IRefIdProperty DepartmentIdProperty = P<CheckPlan>.RegisterRefId(e => e.DepartmentId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> DepartmentProperty = P<CheckPlan>.RegisterRef(e => e.Department, DepartmentIdProperty);

        /// <summary>
        /// 责任部门
        /// </summary>
        public Enterprise Department
        {
            get { return GetRefEntity(DepartmentProperty); }
            set { SetRefEntity(DepartmentProperty, value); }
        }
        #endregion

        #region 点检时长(min) CheckTime
        /// <summary>
        /// 点检时长(min)
        /// </summary>
        [Label("点检时长(min)")]
        public static readonly Property<int?> CheckTimeProperty = P<CheckPlan>.Register(e => e.CheckTime);

        /// <summary>
        /// 点检时长(min)
        /// </summary>
        public int? CheckTime
        {
            get { return this.GetProperty(CheckTimeProperty); }
            set { this.SetProperty(CheckTimeProperty, value); }
        }
        #endregion

        #region 设备点检类型 EquipCheckType
        /// <summary>
        /// 设备点检类型
        /// </summary>
        [Label("设备点检类型")]
        public static readonly Property<EquipCheckType> EquipCheckTypeProperty = P<CheckPlan>.Register(e => e.EquipCheckType);

        /// <summary>
        /// 设备点检类型
        /// </summary>
        public EquipCheckType EquipCheckType
        {
            get { return this.GetProperty(EquipCheckTypeProperty); }
            set { this.SetProperty(EquipCheckTypeProperty, value); }
        }
        #endregion

        #region 执行结果 ExeResult
        /// <summary>
        /// 执行结果
        /// </summary>
        [Label("执行结果")]
        public static readonly Property<ExeResult?> ExeResultProperty = P<CheckPlan>.Register(e => e.ExeResult);

        /// <summary>
        /// 执行结果
        /// </summary>
        public ExeResult? ExeResult
        {
            get { return this.GetProperty(ExeResultProperty); }
            set { this.SetProperty(ExeResultProperty, value); }
        }
        #endregion

        #region 点检人 CheckEmployee
        /// <summary>
        /// 点检人Id
        /// </summary>
        [Label("点检人")]
        public static readonly IRefIdProperty CheckEmployeeIdProperty =
            P<CheckPlan>.RegisterRefId(e => e.CheckEmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 点检人Id
        /// </summary>
        public double? CheckEmployeeId
        {
            get { return (double?)this.GetRefId(CheckEmployeeIdProperty); }
            set { this.SetRefId(CheckEmployeeIdProperty, value); }
        }

        /// <summary>
        /// 点检人
        /// </summary>
        public static readonly RefEntityProperty<Employee> CheckEmployeeProperty =
            P<CheckPlan>.RegisterRef(e => e.CheckEmployee, CheckEmployeeIdProperty);

        /// <summary>
        /// 点检人
        /// </summary>
        public Employee CheckEmployee
        {
            get { return this.GetRefEntity(CheckEmployeeProperty); }
            set { this.SetRefEntity(CheckEmployeeProperty, value); }
        }
        #endregion

        #region 点检小结 CheckSummary
        /// <summary>
        /// 点检小结
        /// </summary>
        [Label("点检小结")]
        [MaxLength(2000)]
        public static readonly Property<string> CheckSummaryProperty = P<CheckPlan>.Register(e => e.CheckSummary);

        /// <summary>
        /// 点检小结
        /// </summary>
        public string CheckSummary
        {
            get { return this.GetProperty(CheckSummaryProperty); }
            set { this.SetProperty(CheckSummaryProperty, value); }
        }
        #endregion

        #region 是否已报修 WhetherRepair
        /// <summary>
        /// 是否已报修
        /// </summary>
        [Label("是否已报修")]
        public static readonly Property<YesNo> WhetherRepairProperty = P<CheckPlan>.Register(e => e.WhetherRepair);

        /// <summary>
        /// 是否已报修
        /// </summary>
        public YesNo WhetherRepair
        {
            get { return this.GetProperty(WhetherRepairProperty); }
            set { this.SetProperty(WhetherRepairProperty, value); }
        }
        #endregion

        #region 来源 CheckSourceType
        /// <summary>
        /// 来源
        /// </summary>
        [Label("来源")]
        public static readonly Property<CheckSourceType> CheckSourceTypeProperty = P<CheckPlan>.Register(e => e.CheckSourceType);

        /// <summary>
        /// 来源
        /// </summary>
        public CheckSourceType CheckSourceType
        {
            get { return this.GetProperty(CheckSourceTypeProperty); }
            set { this.SetProperty(CheckSourceTypeProperty, value); }
        }
        #endregion

        #region 点检确认信息 CheckConfirmationList
        /// <summary>
        /// 点检确认信息
        /// </summary>
        [Label("点检确认信息")]
        public static readonly ListProperty<EntityList<CheckConfirmation>> CheckConfirmationListProperty = P<CheckPlan>.RegisterList(e => e.CheckConfirmationList);

        /// <summary>
        /// 点检确认信息
        /// </summary>
        /// <returns>点检确认信息</returns>
        public EntityList<CheckConfirmation> CheckConfirmationList
        {
            get { return GetLazyList(CheckConfirmationListProperty); }
        }
        #endregion

        #region 确认结果 ConfirmResult
        /// <summary>
        /// 确认结果
        /// </summary>
        [Label("确认结果")]
        public static readonly Property<ConfirmResult?> ConfirmResultProperty = P<CheckPlan>.Register(e => e.ConfirmResult);

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
        public static readonly Property<string> ConfirmNoteProperty = P<CheckPlan>.Register(e => e.ConfirmNote);

        /// <summary>
        /// 确认备注
        /// </summary>
        public string ConfirmNote
        {
            get { return GetProperty(ConfirmNoteProperty); }
            set { SetProperty(ConfirmNoteProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 设备编码 EquipAccountCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipAccountCodeProperty = P<CheckPlan>.RegisterView(e => e.EquipAccountCode, p => p.EquipAccount.Code);

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
        public static readonly Property<string> EquipAccountNameProperty = P<CheckPlan>.RegisterView(e => e.EquipAccountName, p => p.EquipAccount.Name);

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
        public static readonly Property<double> EquipModelIdProperty = P<CheckPlan>.RegisterView(e => e.EquipModelId, p => p.EquipAccount.EquipModelId);

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
        public static readonly Property<string> EquipModelCodeProperty = P<CheckPlan>.RegisterView(e => e.EquipModelCode, p => p.EquipAccount.EquipModel.Code);

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
        public static readonly Property<string> EquipModelNameProperty = P<CheckPlan>.RegisterView(e => e.EquipModelName, p => p.EquipAccount.EquipModel.Name);

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
        public static readonly Property<double> EquipTypeIdProperty = P<CheckPlan>.RegisterView(e => e.EquipTypeId, p => p.EquipAccount.EquipModel.EquipTypeId);

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
        public static readonly Property<string> EquipTypeCodeProperty = P<CheckPlan>.RegisterView(e => e.EquipTypeCode, p => p.EquipAccount.EquipModel.EquipType.TypeCode);

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
        public static readonly Property<string> EquipTypeNameProperty = P<CheckPlan>.RegisterView(e => e.EquipTypeName, p => p.EquipAccount.EquipModel.EquipType.TypeName);

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
        public static readonly Property<string> WorkShopNameProperty = P<CheckPlan>.RegisterView(e => e.WorkShopName, p => p.EquipAccount.WorkShop.Name);

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
        public static readonly Property<string> ResourceNameProperty = P<CheckPlan>.RegisterView(e => e.ResourceName, p => p.EquipAccount.Resource.Name);

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
        public static readonly Property<string> DepartmentCodeProperty = P<CheckPlan>.RegisterView(e => e.DepartmentCode, p => p.Department.Code);

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
        public static readonly Property<string> DepartmentNameProperty = P<CheckPlan>.RegisterView(e => e.DepartmentName, p => p.Department.Name);

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName
        {
            get { return this.GetProperty(DepartmentNameProperty); }
        }
        #endregion

        #region 点检人 CheckEmployeeName
        /// <summary>
        /// 点检人
        /// </summary>
        [Label("点检人")]
        public static readonly Property<string> CheckEmployeeNameProperty = P<CheckPlan>.RegisterView(e => e.CheckEmployeeName, p => p.CheckEmployee.Name);

        /// <summary>
        /// 点检人
        /// </summary>
        public string CheckEmployeeName
        {
            get { return this.GetProperty(CheckEmployeeNameProperty); }
        }
        #endregion

        #region 设备使用部门Id EquipUseDeptId
        /// <summary>
        /// 设备使用部门Id
        /// </summary>
        [Label("设备使用部门Id")]
        public static readonly Property<double> EquipUseDeptIdProperty = P<CheckPlan>.RegisterView(e => e.EquipUseDeptId, p => p.EquipAccount.UseDepartmentId);

        /// <summary>
        /// 设备使用部门Id
        /// </summary>
        public double EquipUseDeptId
        {
            get { return this.GetProperty(EquipUseDeptIdProperty); }
        }
        #endregion

        #region 设备使用部门名称 EquipUseDeptName
        /// <summary>
        /// 设备使用部门名称
        /// </summary>
        [Label("设备使用部门名称")]
        public static readonly Property<string> EquipUseDeptNameProperty = P<CheckPlan>.RegisterView(e => e.EquipUseDeptName, p => p.EquipAccount.UseDepartment.Name);

        /// <summary>
        /// 设备使用部门名称
        /// </summary>
        public string EquipUseDeptName
        {
            get { return this.GetProperty(EquipUseDeptNameProperty); }
        }
        #endregion

        #region 设备RFID RFID
        /// <summary>
        /// 设备RFID
        /// </summary>
        [Label("设备RFID")]
        public static readonly Property<string> RFIDProperty = P<CheckPlan>.RegisterView(e => e.RFID, p => p.EquipAccount.RFID);

        /// <summary>
        /// 设备RFID
        /// </summary>
        public string RFID
        {
            get { return this.GetProperty(RFIDProperty); }
        }
        #endregion
        #endregion

        #region 临时字段，不映射数据库

        #region 上一次点检小结 LastCheckSummary
        /// <summary>
        /// 上一次点检小结
        /// </summary>
        [Label("上一次点检小结")]
        public static readonly Property<string> LastCheckSummaryProperty = P<CheckPlan>.Register(e => e.LastCheckSummary);

        /// <summary>
        /// 上一次点检小结
        /// </summary>
        public string LastCheckSummary
        {
            get { return this.GetProperty(LastCheckSummaryProperty); }
            set { this.SetProperty(LastCheckSummaryProperty, value); }
        }

        #endregion

        #region 是否异常推送 IsAbnormalInfoPush
        /// <summary>
        /// 是否异常推送
        /// </summary>
        [Label("是否异常推送")]
        public static readonly Property<bool> IsAbnormalInfoPushProperty = P<CheckPlan>.Register(e => e.IsAbnormalInfoPush);

        /// <summary>
        /// 是否异常推送
        /// </summary>
        public bool IsAbnormalInfoPush
        {
            get { return this.GetProperty(IsAbnormalInfoPushProperty); }
            set { this.SetProperty(IsAbnormalInfoPushProperty, value); }
        }
        #endregion

        #region 确认部门 ConfirmDept
        /// <summary>
        /// 确认部门Id
        /// </summary>
        [Label("确认部门")]
        public static readonly Property<double?> ConfirmDeptIdProperty = P<CheckPlan>.Register(e => e.ConfirmDeptId);

        /// <summary>
        /// 确认部门Id
        /// </summary>
        public double? ConfirmDeptId
        {
            get { return GetProperty(ConfirmDeptIdProperty); }
            set { SetProperty(ConfirmDeptIdProperty, value); }
        }
        #endregion

        #region 确认部门 ConfirmDeptDisplay
        /// <summary>
        /// 确认部门
        /// </summary>
        [Label("确认部门")]
        public static readonly Property<string> ConfirmDeptDisplayProperty = P<CheckPlan>.Register(e => e.ConfirmDeptDisplay);

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
    }

    /// <summary>
    /// 点检计划维护 实体配置
    /// </summary>
    internal class CheckPlanConfig : EntityConfig<CheckPlan>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_CHECK_PLAN").MapAllProperties();
            Meta.Property(CheckPlan.CheckSummaryProperty).ColumnMeta.HasLength(4000);
            Meta.Property(CheckPlan.LastCheckSummaryProperty).DontMapColumn();            
            Meta.Property(CheckPlan.IsAbnormalInfoPushProperty).DontMapColumn();
            Meta.Property(CheckPlan.ConfirmDeptIdProperty).DontMapColumn();
            Meta.Property(CheckPlan.ConfirmDeptDisplayProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}