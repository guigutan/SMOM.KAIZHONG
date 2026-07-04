using SIE.Domain;
using SIE.EMS.Checks.Plans;
using SIE.EMS.Checks.Projects;
using SIE.EMS.Checks.Records;
using SIE.EMS.Enums;
using SIE.EMS.MainenanceProjects;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System;

namespace SIE.Web.EMS.Checks.Records.ViewModels
{
    /// <summary>
    /// 设备点检记录查看明细 ViewModel 
    /// </summary>
    [RootEntity, Serializable]
    [Label("设备点检记录查看明细")]
    public class CheckRecordViewModel : ViewModel
    {
        #region 点检单号 CheckPlanNo
        /// <summary>
        /// 点检单号
        /// </summary>
        [Label("点检单号")]
        public static readonly Property<string> CheckPlanNoProperty = P<CheckRecordViewModel>.Register(e => e.CheckPlanNo);

        /// <summary>
        /// 点检单号
        /// </summary>
        public string CheckPlanNo
        {
            get { return GetProperty(CheckPlanNoProperty); }
            set { SetProperty(CheckPlanNoProperty, value); }
        }
        #endregion

        #region 设备编码 EquipAccountCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipAccountCodeProperty = P<CheckRecordViewModel>.Register(e => e.EquipAccountCode);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipAccountCode
        {
            get { return GetProperty(EquipAccountCodeProperty); }
            set { SetProperty(EquipAccountCodeProperty, value); }
        }
        #endregion

        #region 机台号 MachineNo
        /// <summary>
        /// 机台号
        /// </summary>
        [Label("机台号")]
        public static readonly Property<string> MachineNoProperty = P<CheckRecordViewModel>.Register(e => e.MachineNo);

        /// <summary>
        /// 机台号
        /// </summary>
        public string MachineNo
        {
            get { return GetProperty(MachineNoProperty); }
            set { SetProperty(MachineNoProperty, value); }
        }
        #endregion

        #region 车间 WorkShopName
        /// <summary>
        /// 车间
        /// </summary>
        [Label("车间")]
        public static readonly Property<string> WorkShopCodeProperty = P<CheckRecordViewModel>.Register(e => e.WorkShopCode);

        /// <summary>
        /// 车间
        /// </summary>
        public string WorkShopCode
        {
            get { return GetProperty(WorkShopCodeProperty); }
            set { SetProperty(WorkShopCodeProperty, value); }
        }
        #endregion

        #region 工序 ProcessName
        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        public static readonly Property<string> ProcessNameProperty = P<CheckRecordViewModel>.Register(e => e.ProcessName);

        /// <summary>
        /// 工序
        /// </summary>
        public string ProcessName
        {
            get { return GetProperty(ProcessNameProperty); }
            set { SetProperty(ProcessNameProperty, value); }
        }
        #endregion

        #region 点检周期类型 CheckCycleType
        /// <summary>
        /// 点检周期类型
        /// </summary>
        [Label("周期类型")]
        public static readonly Property<CheckCycleType> CheckCycleTypeProperty = P<CheckRecordViewModel>.Register(e => e.CheckCycleType);

        /// <summary>
        /// 点检周期类型
        /// </summary>
        public CheckCycleType CheckCycleType
        {
            get { return GetProperty(CheckCycleTypeProperty); }
            set { SetProperty(CheckCycleTypeProperty, value); }
        }
        #endregion

        #region 点检状态 ExeState
        /// <summary>
        /// 点检状态
        /// </summary>
        [Label("点检状态")]
        public static readonly Property<string> ExeStateProperty = P<CheckRecordViewModel>.Register(e => e.ExeState);

        /// <summary>
        /// 点检状态
        /// </summary>
        public string ExeState
        {
            get { return GetProperty(ExeStateProperty); }
            set { SetProperty(ExeStateProperty, value); }
        }
        #endregion

        #region 点检开始时间 CheckBeginDate
        /// <summary>
        /// 点检开始时间
        /// </summary>
        [Label("点检开始时间")]
        public static readonly Property<DateTime> CheckBeginDateProperty = P<CheckRecordViewModel>.Register(e => e.CheckBeginDate);

        /// <summary>
        /// 点检开始时间
        /// </summary>
        public DateTime CheckBeginDate
        {
            get { return GetProperty(CheckBeginDateProperty); }
            set { SetProperty(CheckBeginDateProperty, value); }
        }
        #endregion

        #region 点检结束时间 CheckEndDate
        /// <summary>
        /// 点检结束时间
        /// </summary>
        [Label("点检结束时间")]
        public static readonly Property<DateTime> CheckEndDateProperty = P<CheckRecordViewModel>.Register(e => e.CheckEndDate);

        /// <summary>
        /// 点检结束时间
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
        public static readonly Property<YesNo> WhetherAcrossDayProperty = P<CheckRecordViewModel>.Register(e => e.WhetherAcrossDay);

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
        public static readonly Property<DateTime?> ActCheckBeginDateProperty = P<CheckRecordViewModel>.Register(e => e.ActCheckBeginDate);

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
        public static readonly Property<DateTime?> ActCheckEndDateProperty = P<CheckRecordViewModel>.Register(e => e.ActCheckEndDate);

        /// <summary>
        /// 点检结束时间
        /// </summary>
        public DateTime? ActCheckEndDate
        {
            get { return GetProperty(ActCheckEndDateProperty); }
            set { SetProperty(ActCheckEndDateProperty, value); }
        }
        #endregion

        #region 点检执行人 CheckUser
        /// <summary>
        /// 点检执行人
        /// </summary>
        [Label("点检执行人")]
        public static readonly Property<string> CheckUserProperty = P<CheckRecordViewModel>.Register(e => e.CheckUser);

        /// <summary>
        /// 点检执行人
        /// </summary>
        public string CheckUser
        {
            get { return GetProperty(CheckUserProperty); }
            set { SetProperty(CheckUserProperty, value); }
        }
        #endregion

        #region 点检项目 CheckProjectList
        /// <summary>
        /// 点检项目
        /// </summary>
        public static readonly ListProperty<EntityList<CheckRecordProjectViewModel>> CheckProjectListProperty = P<CheckRecordViewModel>.RegisterList(e => e.CheckProjectList);
        /// <summary>
        /// 点检项目
        /// </summary>
        public EntityList<CheckRecordProjectViewModel> CheckProjectList
        {
            get { return this.GetLazyList(CheckProjectListProperty); }
        }
        #endregion

        #region 确认部门 ConfirmationDept
        /// <summary>
        /// 确认部门Id
        /// </summary>
        [Label("确认部门")]
        public static readonly IRefIdProperty ConfirmationDeptIdProperty = P<CheckRecordViewModel>.RegisterRefId(e => e.ConfirmationDeptId, ReferenceType.Normal);

        /// <summary>
        /// 确认部门Id
        /// </summary>
        public double? ConfirmationDeptId
        {
            get { return (double?)GetRefNullableId(ConfirmationDeptIdProperty); }
            set { SetRefNullableId(ConfirmationDeptIdProperty, value); }
        }

        /// <summary>
        /// 确认部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> ConfirmationDeptProperty = P<CheckRecordViewModel>.RegisterRef(e => e.ConfirmationDept, ConfirmationDeptIdProperty);

        /// <summary>
        /// 确认部门
        /// </summary>
        public Enterprise ConfirmationDept
        {
            get { return GetRefEntity(ConfirmationDeptProperty); }
            set { SetRefEntity(ConfirmationDeptProperty, value); }
        }
        #endregion

        #region 确认人 Confirmor
        /// <summary>
        /// 确认人Id
        /// </summary>
        [Label("确认人")]
        public static readonly IRefIdProperty ConfirmorIdProperty = P<CheckRecordViewModel>.RegisterRefId(e => e.ConfirmorId, ReferenceType.Normal);

        /// <summary>
        /// 确认人Id
        /// </summary>
        public double? ConfirmorId
        {
            get { return (double?)GetRefNullableId(ConfirmorIdProperty); }
            set { SetRefNullableId(ConfirmorIdProperty, value); }
        }

        /// <summary>
        /// 确认人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ConfirmorProperty = P<CheckRecordViewModel>.RegisterRef(e => e.Confirmor, ConfirmorIdProperty);

        /// <summary>
        /// 确认人
        /// </summary>
        public Employee Confirmor
        {
            get { return GetRefEntity(ConfirmorProperty); }
            set { SetRefEntity(ConfirmorProperty, value); }
        }
        #endregion

        #region 确认结果 ConfirmResult
        /// <summary>
        /// 确认结果
        /// </summary>
        [Label("确认结果")]
        public static readonly Property<ConfirmResult> ConfirmResultProperty = P<CheckRecordViewModel>.Register(e => e.ConfirmResult);

        /// <summary>
        /// 确认结果
        /// </summary>
        public ConfirmResult ConfirmResult
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
        public static readonly Property<string> ConfirmNoteProperty = P<CheckRecordViewModel>.Register(e => e.ConfirmNote);

        /// <summary>
        /// 确认备注
        /// </summary>
        public string ConfirmNote
        {
            get { return GetProperty(ConfirmNoteProperty); }
            set { SetProperty(ConfirmNoteProperty, value); }
        }

        #endregion

    }

    /// <summary>
    /// 设备点检记录查看明细项目 ViewModel 
    /// </summary>
    [ChildEntity, Serializable]
    [Label("设备点检记录查看明细项目")]
    public class CheckRecordProjectViewModel : ViewModel
    {
        #region 点检计划 CheckPlan
        /// <summary>
        /// 点检计划Id
        /// </summary>
        [Label("点检计划")]
        public static readonly IRefIdProperty CheckPlanIdProperty = P<CheckRecordProjectViewModel>.RegisterRefId(e => e.CheckPlanId, ReferenceType.Parent);

        /// <summary>
        /// 点检计划Id
        /// </summary>
        public double CheckPlanId
        {
            get { return (double)GetRefId(CheckPlanIdProperty); }
            set { SetRefId(CheckPlanIdProperty, value); }
        }

        /// <summary>
        /// 点检计划
        /// </summary>
        public static readonly RefEntityProperty<CheckRecordViewModel> CheckPlanProperty = P<CheckRecordProjectViewModel>.RegisterRef(e => e.CheckPlan, CheckPlanIdProperty);

        /// <summary>
        /// 点检计划
        /// </summary>
        public CheckRecordViewModel CheckPlan
        {
            get { return GetRefEntity(CheckPlanProperty); }
            set { SetRefEntity(CheckPlanProperty, value); }
        }
        #endregion

        #region 设备编号 AssetCode
        /// <summary>
        /// 资产编号
        /// </summary>
        [Label("设备编号")]
        public static readonly Property<string> AccountCodeProperty = P<CheckRecordProjectViewModel>.Register(e => e.AccountCode);

        /// <summary>
        /// 设备编号
        /// </summary>
        public string AccountCode
        {
            get { return GetProperty(AccountCodeProperty); }
            set { SetProperty(AccountCodeProperty, value); }
        }
        #endregion

        #region 设备名称 AccountName
        /// <summary>
        /// 资产名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> AccountNameProperty = P<CheckRecordProjectViewModel>.Register(e => e.AccountName);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string AccountName
        {
            get { return GetProperty(AccountNameProperty); }
            set { SetProperty(AccountNameProperty, value); }
        }
        #endregion

        #region 项目类型 ProjectType
        /// <summary>
        /// 项目类型
        /// </summary>
        [Label("项目分类名称")]
        public static readonly Property<string> ProjectTypeProperty = P<CheckRecordProjectViewModel>.Register(e => e.ProjectType);

        /// <summary>
        /// 项目类型
        /// </summary>
        public string ProjectType
        {
            get { return GetProperty(ProjectTypeProperty); }
            set { SetProperty(ProjectTypeProperty, value); }
        }
        #endregion

        #region 项目名称 CheckProjectName
        /// <summary>
        /// 项目名称
        /// </summary>
        [Label("项目名称")]
        public static readonly Property<string> CheckProjectNameProperty = P<CheckRecordProjectViewModel>.Register(e => e.CheckProjectName);

        /// <summary>
        /// 项目名称
        /// </summary>
        public string CheckProjectName
        {
            get { return GetProperty(CheckProjectNameProperty); }
            set { SetProperty(CheckProjectNameProperty, value); }
        }
        #endregion

        #region 最小值 MinValue
        /// <summary>
        /// 最小值
        /// </summary>
        [Label("最小值")]
        public static readonly Property<decimal?> MinValueProperty = P<CheckRecordProjectViewModel>.Register(e => e.MinValue);

        /// <summary>
        /// 最小值
        /// </summary>
        public decimal? MinValue
        {
            get { return GetProperty(MinValueProperty); }
            set { SetProperty(MinValueProperty, value); }
        }
        #endregion

        #region 最大值 MaxValue
        /// <summary>
        /// 最大值
        /// </summary>
        [Label("最大值")]
        public static readonly Property<decimal?> MaxValueProperty = P<CheckRecordProjectViewModel>.Register(e => e.MaxValue);

        /// <summary>
        /// 最大值
        /// </summary>
        public decimal? MaxValue
        {
            get { return GetProperty(MaxValueProperty); }
            set { SetProperty(MaxValueProperty, value); }
        }
        #endregion

        #region 项目耗材 ProjectConsumable
        /// <summary>
        /// 项目耗材
        /// </summary>
        [Label("项目耗材")]
        public static readonly Property<string> ProjectConsumableProperty = P<CheckRecordProjectViewModel>.Register(e => e.ProjectConsumable);

        /// <summary>
        /// 项目耗材
        /// </summary>
        public string ProjectConsumable
        {
            get { return GetProperty(ProjectConsumableProperty); }
            set { SetProperty(ProjectConsumableProperty, value); }
        }
        #endregion

        #region 周期类型 CycleType
        /// <summary>
        /// 周期类型
        /// </summary>
        [Label("周期类型")]
        public static readonly Property<CycleType> CycleTypeProperty = P<CheckRecordProjectViewModel>.Register(e => e.CycleType);

        /// <summary>
        /// 周期类型
        /// </summary>
        public CycleType CycleType
        {
            get { return GetProperty(CycleTypeProperty); }
            set { SetProperty(CycleTypeProperty, value); }
        }
        #endregion

        #region 项目周期 ProjectCycle
        /// <summary>
        /// 项目周期
        /// </summary>
        [Label("项目周期")]
        public static readonly Property<decimal?> ProjectCycleProperty = P<CheckRecordProjectViewModel>.Register(e => e.ProjectCycle);

        /// <summary>
        /// 项目周期
        /// </summary>
        public decimal? ProjectCycle
        {
            get { return GetProperty(ProjectCycleProperty); }
            set { SetProperty(ProjectCycleProperty, value); }
        }
        #endregion

        #region 单位 Unit
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitProperty = P<CheckRecordProjectViewModel>.Register(e => e.Unit);

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit
        {
            get { return GetProperty(UnitProperty); }
            set { SetProperty(UnitProperty, value); }
        }
        #endregion

        #region 用时(分钟) UseTime
        /// <summary>
        /// 用时(分钟)
        /// </summary>
        [Label("用时(分钟)")]
        public static readonly Property<string> UseTimeProperty = P<CheckRecordProjectViewModel>.Register(e => e.UseTime);

        /// <summary>
        /// 用时(分钟)
        /// </summary>
        public string UseTime
        {
            get { return GetProperty(UseTimeProperty); }
            set { SetProperty(UseTimeProperty, value); }
        }
        #endregion

        #region 实际值 ActualValue
        /// <summary>
        /// 实际值
        /// </summary>
        [Label("实际值")]
        public static readonly Property<decimal?> ActualValueProperty = P<CheckRecordProjectViewModel>.Register(e => e.ActualValue);

        /// <summary>
        /// 实际值
        /// </summary>
        public decimal? ActualValue
        {
            get { return GetProperty(ActualValueProperty); }
            set { SetProperty(ActualValueProperty, value); }
        }
        #endregion

        #region 点检问题点 Note
        /// <summary>
        /// 点检问题点
        /// </summary>
        [Label("点检问题点")]
        public static readonly Property<string> NoteProperty = P<CheckRecordProjectViewModel>.Register(e => e.Note);

        /// <summary>
        /// 点检问题点
        /// </summary>
        public string Note
        {
            get { return GetProperty(NoteProperty); }
            set { SetProperty(NoteProperty, value); }
        }
        #endregion

        #region 点检结果 CheckResult
        /// <summary>
        /// 点检结果
        /// </summary>
        [Label("点检结果")]
        public static readonly Property<CheckMaintainResult?> CheckResultProperty = P<CheckRecordProjectViewModel>.Register(e => e.CheckResult);

        /// <summary>
        /// 点检结果
        /// </summary>
        public CheckMaintainResult? CheckResult
        {
            get { return GetProperty(CheckResultProperty); }
            set { SetProperty(CheckResultProperty, value); }
        }
        #endregion

        #region 图片 Photo
        /// <summary>
        /// 图片
        /// </summary>
        [Label("图片")]
        public static readonly Property<byte[]> PhotoProperty = P<CheckRecordProjectViewModel>.Register(e => e.Photo);

        /// <summary>
        /// 图片
        /// </summary>
        public byte[] Photo
        {
            get { return GetProperty(PhotoProperty); }
            set { SetProperty(PhotoProperty, value); }
        }
        #endregion


        #region 是否存在图片 ExistPhoto
        /// <summary>
        /// 是否存在图片
        /// </summary>
        [Label("是否存在图片")]
        public static readonly Property<bool> ExistPhotoProperty = P<CheckRecordProjectViewModel>.Register(e => e.ExistPhoto);

        /// <summary>
        /// 是否存在图片
        /// </summary>
        public bool ExistPhoto
        {
            get { return this.GetProperty(ExistPhotoProperty); }
            set { this.SetProperty(ExistPhotoProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 设备点检记录查看明细 视图配置
    /// </summary>
    internal class CheckRecordViewModelViewConfig : WebViewConfig<CheckRecordViewModel>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(CheckRecord));
        }

        /// <summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.HasDetailColumnsCount(3);
            View.Property(p => p.CheckPlanNo).Readonly();
            View.Property(p => p.EquipAccountCode).Readonly();
            View.Property(p => p.MachineNo).Readonly();
            View.Property(p => p.WorkShopCode).Readonly();
            View.Property(p => p.ProcessName).Readonly();
            View.Property(p => p.CheckCycleType).Readonly();
            View.Property(p => p.CheckBeginDate).UseDateTimeEditor();
            View.Property(p => p.CheckEndDate).UseDateTimeEditor();
            View.Property(p => p.WhetherAcrossDay).Readonly();
            View.Property(p => p.ActCheckBeginDate).UseDateTimeEditor();
            View.Property(p => p.ActCheckEndDate).UseDateTimeEditor();
            View.Property(p => p.ExeState).Readonly();
            View.Property(p => p.CheckUser).Readonly();
            View.Property(p => p.ConfirmationDeptId).Readonly().HasLabel("确认部门");
            View.Property(p => p.ConfirmorId).Readonly().HasLabel("确认人");
            View.Property(p => p.ConfirmResult).Readonly();
            View.Property(p => p.ConfirmNote).Readonly();
            View.AttachChildrenProperty(typeof(CheckRecordProjectViewModel), e => new EntityList<CheckRecordProjectViewModel>()).Show(ChildShowInWhere.Detail).HasLabel("设备点检记录查看明细项目").Readonly();
            View.ChildrenProperty(p => p.CheckProjectList).Show(ChildShowInWhere.Hide);
        }
    }

    /// <summary>
    /// 设备点检记录查看明细项目 视图配置
    /// </summary>
    internal class CheckRecordProjectViewModelViewConfig : WebViewConfig<CheckRecordProjectViewModel>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(CheckRecord));
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseClientOrder();
            View.ClearCommands();
            View.UseCommands("SIE.Web.EMS.Checks.Records.Commands.ShowCheckPicture");
            View.Property(p => p.AccountCode).Readonly();
            View.Property(p => p.AccountName).Readonly();
            View.Property(p => p.ProjectType).Readonly();
            View.Property(p => p.CheckProjectName).Readonly();
            View.Property(p => p.MaxValue).Readonly();
            View.Property(p => p.MinValue).Readonly();
            View.Property(p => p.ProjectConsumable).Readonly();
            View.Property(p => p.CycleType).Readonly();
            View.Property(p => p.ProjectCycle).Readonly();
            View.Property(p => p.Unit).Readonly();
            View.Property(p => p.UseTime).Readonly();
            View.Property(p => p.ActualValue).Readonly();
            View.Property(p => p.CheckResult).Readonly();
            View.Property(p => p.Note).Readonly();
        }

        /// <summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.Photo).UseImageComponentEditor().HasLabel("").ShowInDetail().Readonly();
        }
    }
}
