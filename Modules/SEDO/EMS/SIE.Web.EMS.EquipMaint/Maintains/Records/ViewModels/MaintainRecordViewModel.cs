using SIE.Domain;
using SIE.EMS.Enums;
using SIE.EMS.MainenanceProjects;
using SIE.EMS.Maintains.Records;
using SIE.Equipments.EquipAccounts;
using SIE.ObjectModel;
using System;

namespace SIE.Web.EMS.EquipMaint.Maintains.Records.ViewModels
{
    /// <summary>
    /// 设备保养记录查看明细 ViewModel 
    /// </summary>
    [RootEntity, Serializable]
    [Label("设备保养记录查看明细")]
    public class MaintainRecordViewModel : ViewModel
    {
        #region 保养单号 CheckPlanNo
        /// <summary>
        /// 保养单号
        /// </summary>
        [Label("保养单号")]
        public static readonly Property<string> CheckPlanNoProperty = P<MaintainRecordViewModel>.Register(e => e.CheckPlanNo);

        /// <summary>
        /// 保养单号
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
        public static readonly Property<string> EquipAccountCodeProperty = P<MaintainRecordViewModel>.Register(e => e.EquipAccountCode);

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
        public static readonly Property<string> MachineNoProperty = P<MaintainRecordViewModel>.Register(e => e.MachineNo);

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
        public static readonly Property<string> WorkShopCodeProperty = P<MaintainRecordViewModel>.Register(e => e.WorkShopCode);

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
        public static readonly Property<string> ProcessNameProperty = P<MaintainRecordViewModel>.Register(e => e.ProcessName);

        /// <summary>
        /// 工序
        /// </summary>
        public string ProcessName
        {
            get { return GetProperty(ProcessNameProperty); }
            set { SetProperty(ProcessNameProperty, value); }
        }
        #endregion

        #region 保养状态 ExeState
        /// <summary>
        /// 保养状态
        /// </summary>
        [Label("保养状态")]
        public static readonly Property<string> ExeStateProperty = P<MaintainRecordViewModel>.Register(e => e.ExeState);

        /// <summary>
        /// 保养状态
        /// </summary>
        public string ExeState
        {
            get { return GetProperty(ExeStateProperty); }
            set { SetProperty(ExeStateProperty, value); }
        }
        #endregion

        #region 保养开始时间 BeginDate
        /// <summary>
        /// 点检开始时间
        /// </summary>
        [Label("保养开始时间")]
        public static readonly Property<DateTime> BeginDateProperty = P<MaintainRecordViewModel>.Register(e => e.BeginDate);

        /// <summary>
        /// 保养开始时间
        /// </summary>
        public DateTime BeginDate
        {
            get { return GetProperty(BeginDateProperty); }
            set { SetProperty(BeginDateProperty, value); }
        }
        #endregion

        #region 保养结束时间 EndDate
        /// <summary>
        /// 保养结束时间
        /// </summary>
        [Label("保养结束时间")]
        public static readonly Property<DateTime> EndDateProperty = P<MaintainRecordViewModel>.Register(e => e.EndDate);

        /// <summary>
        /// 保养结束时间
        /// </summary>
        public DateTime EndDate
        {
            get { return GetProperty(EndDateProperty); }
            set { SetProperty(EndDateProperty, value); }
        }
        #endregion

        #region 任务开始时间 ActCheckBeginDate
        /// <summary>
        /// 任务开始时间
        /// </summary>
        [Label("任务开始时间")]
        public static readonly Property<DateTime?> ActCheckBeginDateProperty = P<MaintainRecordViewModel>.Register(e => e.ActCheckBeginDate);

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
        public static readonly Property<DateTime?> ActCheckEndDateProperty = P<MaintainRecordViewModel>.Register(e => e.ActCheckEndDate);

        /// <summary>
        /// 点检结束时间
        /// </summary>
        public DateTime? ActCheckEndDate
        {
            get { return GetProperty(ActCheckEndDateProperty); }
            set { SetProperty(ActCheckEndDateProperty, value); }
        }
        #endregion

        #region 保养执行人 CheckUser
        /// <summary>
        /// 保养执行人
        /// </summary>
        [Label("保养执行人")]
        public static readonly Property<string> CheckUserProperty = P<MaintainRecordViewModel>.Register(e => e.CheckUser);

        /// <summary>
        /// 保养执行人
        /// </summary>
        public string CheckUser
        {
            get { return GetProperty(CheckUserProperty); }
            set { SetProperty(CheckUserProperty, value); }
        }
        #endregion

        #region 评分人 ScoreUser
        /// <summary>
        /// 评分人
        /// </summary>
        [Label("评分人")]
        public static readonly Property<string> ScoreUserProperty = P<MaintainRecordViewModel>.Register(e => e.ScoreUser);

        /// <summary>
        /// 评分人
        /// </summary>
        public string ScoreUser
        {
            get { return GetProperty(ScoreUserProperty); }
            set { SetProperty(ScoreUserProperty, value); }
        }
        #endregion

        #region 评分时间 ScoreDate
        /// <summary>
        /// 评分时间
        /// </summary>
        [Label("评分时间")]
        public static readonly Property<DateTime?> ScoreDateProperty = P<MaintainRecordViewModel>.Register(e => e.ScoreDate);

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
        public static readonly Property<double?> ScoreProperty = P<MaintainRecordViewModel>.Register(e => e.Score);

        /// <summary>
        /// 评分
        /// </summary>
        public double? Score
        {
            get { return GetProperty(ScoreProperty); }
            set { SetProperty(ScoreProperty, value); }
        }
        #endregion

        #region 保养项目 CheckProjectList
        /// <summary>
        /// 保养项目
        /// </summary>
        public static readonly ListProperty<EntityList<MaintainRecordProjectViewModel>> CheckProjectListProperty = P<MaintainRecordViewModel>.RegisterList(e => e.CheckProjectList);
        /// <summary>
        /// 保养项目
        /// </summary>
        public EntityList<MaintainRecordProjectViewModel> CheckProjectList
        {
            get { return this.GetLazyList(CheckProjectListProperty); }
        }
        #endregion

        #region 评分明细 ScoreList
        /// <summary>
        /// 评分明细
        /// </summary>
        public static readonly ListProperty<EntityList<MaintainRecordScoreViewModel>> ScoreListProperty = P<MaintainRecordViewModel>.RegisterList(e => e.ScoreList);
        /// <summary>
        /// 评分明细
        /// </summary>
        public EntityList<MaintainRecordScoreViewModel> ScoreList
        {
            get { return this.GetLazyList(ScoreListProperty); }
        }
        #endregion

        #region 领料单明细 MateriaRquisitionDetailList
        /// <summary>
        /// 领料单明细
        /// </summary>
        public static readonly ListProperty<EntityList<MateriaRquisitionDetailViewModel>> MateriaRquisitionDetailListProperty = P<MaintainRecordViewModel>.RegisterList(e => e.MateriaRquisitionDetailList);
        /// <summary>
        /// 领料单明细
        /// </summary>
        public EntityList<MateriaRquisitionDetailViewModel> MateriaRquisitionDetailList
        {
            get { return this.GetLazyList(MateriaRquisitionDetailListProperty); }
        }
        #endregion

        #region 配件更换明细 FittingChangeDetailList
        /// <summary>
        /// 领料单明细
        /// </summary>
        public static readonly ListProperty<EntityList<FittingChangeDetailViewModel>> FittingChangeDetailListProperty = P<MaintainRecordViewModel>.RegisterList(e => e.FittingChangeDetailList);
        /// <summary>
        /// 领料单明细
        /// </summary>
        public EntityList<FittingChangeDetailViewModel> FittingChangeDetailList
        {
            get { return this.GetLazyList(FittingChangeDetailListProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 项目明细 ViewModel 
    /// </summary>
    [ChildEntity, Serializable]
    [Label("项目明细")]
    public class MaintainRecordProjectViewModel : ViewModel
    {
        #region 保养计划 CheckPlan
        /// <summary>
        /// 保养计划Id
        /// </summary>
        [Label("保养计划")]
        public static readonly IRefIdProperty CheckPlanIdProperty = P<MaintainRecordProjectViewModel>.RegisterRefId(e => e.CheckPlanId, ReferenceType.Parent);

        /// <summary>
        /// 保养计划Id
        /// </summary>
        public double CheckPlanId
        {
            get { return (double)GetRefId(CheckPlanIdProperty); }
            set { SetRefId(CheckPlanIdProperty, value); }
        }

        /// <summary>
        /// 保养计划
        /// </summary>
        public static readonly RefEntityProperty<MaintainRecordViewModel> CheckPlanProperty = P<MaintainRecordProjectViewModel>.RegisterRef(e => e.CheckPlan, CheckPlanIdProperty);

        /// <summary>
        /// 保养计划
        /// </summary>
        public MaintainRecordViewModel CheckPlan
        {
            get { return GetRefEntity(CheckPlanProperty); }
            set { SetRefEntity(CheckPlanProperty, value); }
        }
        #endregion

        #region 设备编码 AccountCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> AccountCodeProperty = P<MaintainRecordProjectViewModel>.Register(e => e.AccountCode);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string AccountCode
        {
            get { return GetProperty(AccountCodeProperty); }
            set { SetProperty(AccountCodeProperty, value); }
        }
        #endregion

        #region 设备名称 AccountName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> AccountNameProperty = P<MaintainRecordProjectViewModel>.Register(e => e.AccountName);

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
        public static readonly Property<string> ProjectTypeProperty = P<MaintainRecordProjectViewModel>.Register(e => e.ProjectType);

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
        public static readonly Property<string> CheckProjectNameProperty = P<MaintainRecordProjectViewModel>.Register(e => e.CheckProjectName);

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
        public static readonly Property<decimal?> MinValueProperty = P<MaintainRecordProjectViewModel>.Register(e => e.MinValue);

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
        public static readonly Property<decimal?> MaxValueProperty = P<MaintainRecordProjectViewModel>.Register(e => e.MaxValue);

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
        public static readonly Property<string> ProjectConsumableProperty = P<MaintainRecordProjectViewModel>.Register(e => e.ProjectConsumable);

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
        public static readonly Property<CycleType> CycleTypeProperty = P<MaintainRecordProjectViewModel>.Register(e => e.CycleType);

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
        public static readonly Property<decimal?> ProjectCycleProperty = P<MaintainRecordProjectViewModel>.Register(e => e.ProjectCycle);

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
        public static readonly Property<string> UnitProperty = P<MaintainRecordProjectViewModel>.Register(e => e.Unit);

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
        public static readonly Property<string> UseTimeProperty = P<MaintainRecordProjectViewModel>.Register(e => e.UseTime);

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
        public static readonly Property<decimal?> ActualValueProperty = P<MaintainRecordProjectViewModel>.Register(e => e.ActualValue);

        /// <summary>
        /// 实际值
        /// </summary>
        public decimal? ActualValue
        {
            get { return GetProperty(ActualValueProperty); }
            set { SetProperty(ActualValueProperty, value); }
        }
        #endregion

        #region 保养问题点 Note
        /// <summary>
        /// 保养问题点
        /// </summary>
        [Label("保养问题点")]
        public static readonly Property<string> NoteProperty = P<MaintainRecordProjectViewModel>.Register(e => e.Note);

        /// <summary>
        /// 保养问题点
        /// </summary>
        public string Note
        {
            get { return GetProperty(NoteProperty); }
            set { SetProperty(NoteProperty, value); }
        }
        #endregion

        #region 保养结果 MaintainResult
        /// <summary>
        /// 保养结果
        /// </summary>
        [Label("保养结果")]
        public static readonly Property<CheckMaintainResult?> MaintainResultProperty = P<MaintainRecordProjectViewModel>.Register(e => e.MaintainResult);

        /// <summary>
        /// 保养结果
        /// </summary>
        public CheckMaintainResult? MaintainResult
        {
            get { return GetProperty(MaintainResultProperty); }
            set { SetProperty(MaintainResultProperty, value); }
        }
        #endregion

        #region 图片 Photo
        /// <summary>
        /// 图片
        /// </summary>
        [Label("图片")]
        public static readonly Property<byte[]> PhotoProperty = P<MaintainRecordProjectViewModel>.Register(e => e.Photo);

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
        public static readonly Property<bool> ExistPhotoProperty = P<MaintainRecordProjectViewModel>.Register(e => e.ExistPhoto);

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
    /// 设备保养记录查看评分明细 ViewModel 
    /// </summary>
    [ChildEntity, Serializable]
    [Label("评分明细")]
    public class MaintainRecordScoreViewModel : ViewModel
    {
        #region 保养计划 CheckPlan
        /// <summary>
        /// 保养计划Id
        /// </summary>
        [Label("保养计划")]
        public static readonly IRefIdProperty CheckPlanIdProperty = P<MaintainRecordScoreViewModel>.RegisterRefId(e => e.CheckPlanId, ReferenceType.Parent);

        /// <summary>
        /// 保养计划Id
        /// </summary>
        public double CheckPlanId
        {
            get { return (double)GetRefId(CheckPlanIdProperty); }
            set { SetRefId(CheckPlanIdProperty, value); }
        }

        /// <summary>
        /// 保养计划
        /// </summary>
        public static readonly RefEntityProperty<MaintainRecordViewModel> CheckPlanProperty = P<MaintainRecordScoreViewModel>.RegisterRef(e => e.CheckPlan, CheckPlanIdProperty);

        /// <summary>
        /// 保养计划
        /// </summary>
        public MaintainRecordViewModel CheckPlan
        {
            get { return GetRefEntity(CheckPlanProperty); }
            set { SetRefEntity(CheckPlanProperty, value); }
        }
        #endregion

        #region 项目名称 ProjectName
        /// <summary>
        /// 项目名称
        /// </summary>
        [Required]
        [Label("项目名称")]
        public static readonly Property<string> ProjectNameProperty = P<MaintainRecordScoreViewModel>.Register(e => e.ProjectName);

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName
        {
            get { return GetProperty(ProjectNameProperty); }
            set { SetProperty(ProjectNameProperty, value); }
        }
        #endregion

        #region 检查标准 CheckStandard
        /// <summary>
        /// 检查标准
        /// </summary>
        [Required]
        [Label("检查标准")]
        public static readonly Property<string> CheckStandardProperty = P<MaintainRecordScoreViewModel>.Register(e => e.CheckStandard);

        /// <summary>
        /// 检查标准
        /// </summary>
        public string CheckStandard
        {
            get { return GetProperty(CheckStandardProperty); }
            set { SetProperty(CheckStandardProperty, value); }
        }
        #endregion

        #region 分值比（%） Rate
        /// <summary>
        /// 分值比（%）
        /// </summary>
        [Required]
        [MinValue(0), MaxValue(100)]
        [Label("分值比（%）")]
        public static readonly Property<decimal> RateProperty = P<MaintainRecordScoreViewModel>.Register(e => e.Rate);

        /// <summary>
        /// 分值比（%）
        /// </summary>
        public decimal Rate
        {
            get { return GetProperty(RateProperty); }
            set { SetProperty(RateProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(2000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<MaintainRecordScoreViewModel>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 得分 Score
        /// <summary>
        /// 得分
        /// </summary>
        [Label("得分")]
        public static readonly Property<double> ScoreProperty = P<MaintainRecordScoreViewModel>.Register(e => e.Score);

        /// <summary>
        /// 得分
        /// </summary>
        public double Score
        {
            get { return GetProperty(ScoreProperty); }
            set { SetProperty(ScoreProperty, value); }
        }
        #endregion

        #region 存在问题 ExistProblem
        /// <summary>
        /// 存在问题ExistProblem
        /// </summary>
        [MaxLength(1000)]
        [Label("存在问题")]
        public static readonly Property<string> ExistProblemProperty = P<MaintainRecordScoreViewModel>.Register(e => e.ExistProblem);

        /// <summary>
        /// 存在问题ExistProblem
        /// </summary>
        public string ExistProblem
        {
            get { return GetProperty(ExistProblemProperty); }
            set { SetProperty(ExistProblemProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 设备保养记录查看领料单明细 ViewModel 
    /// </summary>
    [ChildEntity, Serializable]
    [Label("领料单明细")]
    public class MateriaRquisitionDetailViewModel : ViewModel
    {
        #region 保养计划 CheckPlan
        /// <summary>
        /// 保养计划Id
        /// </summary>
        [Label("保养计划")]
        public static readonly IRefIdProperty CheckPlanIdProperty = P<MateriaRquisitionDetailViewModel>.RegisterRefId(e => e.CheckPlanId, ReferenceType.Parent);

        /// <summary>
        /// 保养计划Id
        /// </summary>
        public double CheckPlanId
        {
            get { return (double)GetRefId(CheckPlanIdProperty); }
            set { SetRefId(CheckPlanIdProperty, value); }
        }

        /// <summary>
        /// 保养计划
        /// </summary>
        public static readonly RefEntityProperty<MaintainRecordViewModel> CheckPlanProperty = P<MateriaRquisitionDetailViewModel>.RegisterRef(e => e.CheckPlan, CheckPlanIdProperty);

        /// <summary>
        /// 保养计划
        /// </summary>
        public MaintainRecordViewModel CheckPlan
        {
            get { return GetRefEntity(CheckPlanProperty); }
            set { SetRefEntity(CheckPlanProperty, value); }
        }
        #endregion

        #region ERP单据号 ErpNo
        /// <summary>
        /// ERP单据号
        /// </summary>
        [Label("ERP单据号")]
        public static readonly Property<string> ErpNoProperty = P<MateriaRquisitionDetailViewModel>.Register(e => e.ErpNo);

        /// <summary>
        /// ERP单据号
        /// </summary>
        public string ErpNo
        {
            get { return GetProperty(ErpNoProperty); }
            set { SetProperty(ErpNoProperty, value); }
        }
        #endregion

        #region 领料类型 TransactionTypeName
        /// <summary>
        /// 领料类型
        /// </summary>
        [Label("领料类型")]
        public static readonly Property<string> TransactionTypeNameProperty = P<MateriaRquisitionDetailViewModel>.Register(e => e.TransactionTypeName);

        /// <summary>
        /// 领料类型
        /// </summary>
        public string TransactionTypeName
        {
            get { return GetProperty(TransactionTypeNameProperty); }
            set { SetProperty(TransactionTypeNameProperty, value); }
        }
        #endregion

        #region 来源子库存 SourceWarehouseName
        /// <summary>
        /// 来源子库存
        /// </summary>
        [Label("来源子库存")]
        public static readonly Property<string> SourceWarehouseNameProperty = P<MateriaRquisitionDetailViewModel>.Register(e => e.SourceWarehouseName);

        /// <summary>
        /// 来源子库存
        /// </summary>
        public string SourceWarehouseName
        {
            get { return GetProperty(SourceWarehouseNameProperty); }
            set { SetProperty(SourceWarehouseNameProperty, value); }
        }
        #endregion

        #region 目标子库存 TargetWarehouseName
        /// <summary>
        /// 目标子库存
        /// </summary>
        [Label("目标子库存")]
        public static readonly Property<string> TargetWarehouseNameProperty = P<MateriaRquisitionDetailViewModel>.Register(e => e.TargetWarehouseName);

        /// <summary>
        /// 目标子库存
        /// </summary>
        public string TargetWarehouseName
        {
            get { return GetProperty(TargetWarehouseNameProperty); }
            set { SetProperty(TargetWarehouseNameProperty, value); }
        }
        #endregion

        #region 账户别名 GenericDispositionsDescription
        /// <summary>
        /// 账户别名
        /// </summary>
        [Label("账户别名")]
        public static readonly Property<string> GenericDispositionsDescriptionProperty = P<MateriaRquisitionDetailViewModel>.Register(e => e.GenericDispositionsDescription);

        /// <summary>
        /// 账户别名
        /// </summary>
        public string GenericDispositionsDescription
        {
            get { return GetProperty(GenericDispositionsDescriptionProperty); }
            set { SetProperty(GenericDispositionsDescriptionProperty, value); }
        }
        #endregion

        #region 行号 LineNum
        /// <summary>
        /// 行号
        /// </summary>
        [Label("行号")]
        public static readonly Property<double> LineNumProperty = P<MateriaRquisitionDetailViewModel>.Register(e => e.LineNum);

        /// <summary>
        /// 行号
        /// </summary>
        public double LineNum
        {
            get { return GetProperty(LineNumProperty); }
            set { SetProperty(LineNumProperty, value); }
        }
        #endregion

        #region 物料 ItemCode
        /// <summary>
        /// 物料
        /// </summary>
        [Label("物料")]
        public static readonly Property<string> ItemCodeProperty = P<MateriaRquisitionDetailViewModel>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料
        /// </summary>
        public string ItemCode
        {
            get { return GetProperty(ItemCodeProperty); }
            set { SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<MateriaRquisitionDetailViewModel>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return GetProperty(ItemNameProperty); }
            set { SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<MateriaRquisitionDetailViewModel>.Register(e => e.UnitName);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName
        {
            get { return GetProperty(UnitNameProperty); }
            set { SetProperty(UnitNameProperty, value); }
        }
        #endregion

        #region 申请数量 ApplyNum
        /// <summary>
        /// 申请数量
        /// </summary>
        [Label("申请数量")]
        public static readonly Property<double> ApplyNumProperty = P<MateriaRquisitionDetailViewModel>.Register(e => e.ApplyNum);

        /// <summary>
        /// 申请数量
        /// </summary>
        public double ApplyNum
        {
            get { return GetProperty(ApplyNumProperty); }
            set { SetProperty(ApplyNumProperty, value); }
        }
        #endregion

        #region 实发数量 RealSendNum
        /// <summary>
        /// 实发数量
        /// </summary>
        [Label("实发数量")]
        public static readonly Property<double> RealSendNumProperty = P<MateriaRquisitionDetailViewModel>.Register(e => e.RealSendNum);

        /// <summary>
        /// 实发数量
        /// </summary>
        public double RealSendNum
        {
            get { return GetProperty(RealSendNumProperty); }
            set { SetProperty(RealSendNumProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<MateriaRquisitionDetailViewModel>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 设备保养记录查看配件更换明细 ViewModel 
    /// </summary>
    [ChildEntity, Serializable]
    [Label("配件更换明细")]
    public class FittingChangeDetailViewModel : ViewModel
    {
        #region 保养计划 CheckPlan
        /// <summary>
        /// 保养计划Id
        /// </summary>
        [Label("保养计划")]
        public static readonly IRefIdProperty CheckPlanIdProperty = P<FittingChangeDetailViewModel>.RegisterRefId(e => e.CheckPlanId, ReferenceType.Parent);

        /// <summary>
        /// 保养计划Id
        /// </summary>
        public double CheckPlanId
        {
            get { return (double)GetRefId(CheckPlanIdProperty); }
            set { SetRefId(CheckPlanIdProperty, value); }
        }

        /// <summary>
        /// 保养计划
        /// </summary>
        public static readonly RefEntityProperty<MaintainRecordViewModel> CheckPlanProperty = P<FittingChangeDetailViewModel>.RegisterRef(e => e.CheckPlan, CheckPlanIdProperty);

        /// <summary>
        /// 保养计划
        /// </summary>
        public MaintainRecordViewModel CheckPlan
        {
            get { return GetRefEntity(CheckPlanProperty); }
            set { SetRefEntity(CheckPlanProperty, value); }
        }
        #endregion

        #region 新物料编码 ItemCode
        /// <summary>
        /// 新物料编码
        /// </summary>
        [Label("新物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<FittingChangeDetailViewModel>.Register(e => e.ItemCode);

        /// <summary>
        /// 新物料编码
        /// </summary>
        public string ItemCode
        {
            get { return GetProperty(ItemCodeProperty); }
            set { SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 新物料名称 ItemName
        /// <summary>
        /// 新物料名称
        /// </summary>
        [Label("新物料名称")]
        public static readonly Property<string> ItemNameProperty = P<FittingChangeDetailViewModel>.Register(e => e.ItemName);

        /// <summary>
        /// 新物料名称
        /// </summary>
        public string ItemName
        {
            get { return GetProperty(ItemNameProperty); }
            set { SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 新序列号 KnifeNo
        /// <summary>
        /// 新序列号
        /// </summary>
        [Label("新序列号")]
        public static readonly Property<string> KnifeNoProperty = P<FittingChangeDetailViewModel>.Register(e => e.KnifeNo);

        /// <summary>
        /// 新序列号
        /// </summary>
        public string KnifeNo
        {
            get { return GetProperty(KnifeNoProperty); }
            set { SetProperty(KnifeNoProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal?> QtyProperty = P<FittingChangeDetailViewModel>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal? Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<FittingChangeDetailViewModel>.Register(e => e.UnitName);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName
        {
            get { return GetProperty(UnitNameProperty); }
            set { SetProperty(UnitNameProperty, value); }
        }
        #endregion

        #region 设备台帐 EquipAccount
        /// <summary>
        /// 设备台帐Id
        /// </summary>
        [Label("设备台帐")]
        public static readonly IRefIdProperty EquipAccountIdProperty = P<FittingChangeDetailViewModel>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备台帐Id
        /// </summary>
        public double EquipAccountId
        {
            get { return (double)GetRefId(EquipAccountIdProperty); }
            set { SetRefId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备台帐
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty = P<FittingChangeDetailViewModel>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备台帐
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return GetRefEntity(EquipAccountProperty); }
            set { SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 原物料编码 OldItemCode
        /// <summary>
        /// 原物料编码
        /// </summary>
        [Label("原物料编码")]
        public static readonly Property<string> OldItemCodeProperty = P<FittingChangeDetailViewModel>.Register(e => e.OldItemCode);

        /// <summary>
        /// 原物料编码
        /// </summary>
        public string OldItemCode
        {
            get { return GetProperty(OldItemCodeProperty); }
            set { SetProperty(OldItemCodeProperty, value); }
        }
        #endregion

        #region 原物料名称 OldItemName
        /// <summary>
        /// 原物料名称
        /// </summary>
        [Label("原物料名称")]
        public static readonly Property<string> OldItemNameProperty = P<FittingChangeDetailViewModel>.Register(e => e.OldItemName);

        /// <summary>
        /// 原物料名称
        /// </summary>
        public string OldItemName
        {
            get { return GetProperty(OldItemNameProperty); }
            set { SetProperty(OldItemNameProperty, value); }
        }
        #endregion

        #region 原序列号 OldKnifeNo
        /// <summary>
        /// 原序列号
        /// </summary>
        [Label("原序列号")]
        public static readonly Property<string> OldKnifeNoProperty = P<FittingChangeDetailViewModel>.Register(e => e.OldKnifeNo);

        /// <summary>
        /// 原序列号
        /// </summary>
        public string OldKnifeNo
        {
            get { return GetProperty(OldKnifeNoProperty); }
            set { SetProperty(OldKnifeNoProperty, value); }
        }
        #endregion

        #region 使用时长 TimeLeng
        /// <summary>
        /// 使用时长
        /// </summary>
        [Label("使用时长")]
        public static readonly Property<string> TimeLengProperty = P<FittingChangeDetailViewModel>.Register(e => e.TimeLeng);

        /// <summary>
        /// 使用时长
        /// </summary>
        public string TimeLeng
        {
            get { return GetProperty(TimeLengProperty); }
            set { SetProperty(TimeLengProperty, value); }
        }
        #endregion

        #region 更换原因 Reason
        /// <summary>
        /// 更换原因
        /// </summary>
        [Label("更换原因")]
        public static readonly Property<string> ReasonProperty = P<FittingChangeDetailViewModel>.Register(e => e.Reason);

        /// <summary>
        /// 更换原因
        /// </summary>
        public string Reason
        {
            get { return GetProperty(ReasonProperty); }
            set { SetProperty(ReasonProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 设备名称 AccountName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> AccountNameProperty = P<FittingChangeDetailViewModel>.RegisterView(e => e.AccountName, p => p.EquipAccount.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string AccountName
        {
            get { return this.GetProperty(AccountNameProperty); }
            set { this.SetProperty(AccountNameProperty, value); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 设备点检记录查看明细 视图配置
    /// </summary>
    internal class MaintainRecordViewModelViewConfig : WebViewConfig<MaintainRecordViewModel>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(MaintainRecord));
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.HasDetailColumnsCount(3);
            View.Property(p => p.CheckPlanNo).Readonly();
            View.Property(p => p.ExeState).Readonly();
            View.Property(p => p.EquipAccountCode).Readonly();

            View.Property(p => p.MachineNo).Readonly();
            View.Property(p => p.WorkShopCode).Readonly();
            View.Property(p => p.ProcessName).Readonly();

            View.Property(p => p.BeginDate).UseDateTimeEditor();
            View.Property(p => p.EndDate).UseDateTimeEditor().ShowInDetail(columnSpan: 2, width: "50%");

            View.Property(p => p.ActCheckBeginDate).UseDateTimeEditor();
            View.Property(p => p.ActCheckEndDate).UseDateTimeEditor();
            View.Property(p => p.CheckUser).Readonly();

            View.Property(p => p.Score).UseSpinEditor(p =>
            {
                p.AllowNegative = false;
                p.AllowDecimals = true;
                p.AllowBlank = false;
                p.DecimalPrecision = 2;
            }).Readonly();
            View.Property(p => p.ScoreUser).Readonly();
            View.Property(p => p.ScoreDate).UseDateTimeEditor();
            View.ChildrenProperty(p => p.CheckProjectList);
            View.ChildrenProperty(p => p.ScoreList);
            View.ChildrenProperty(p => p.MateriaRquisitionDetailList);
            View.ChildrenProperty(p => p.FittingChangeDetailList);
        }
    }

    /// <summary>
    /// 项目明细 视图配置
    /// </summary>
    internal class MaintainRecordProjectViewModelViewConfig : WebViewConfig<MaintainRecordProjectViewModel>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(MaintainRecord));
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseClientOrder();
            View.ClearCommands();
            View.UseCommands("SIE.Web.EMS.EquipMaint.Maintains.Records.Commands.ShowMaintainPicture");
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
            View.Property(p => p.MaintainResult).Readonly();
            View.Property(p => p.Note).Readonly();
        }
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.Photo).UseImageComponentEditor(f => { f.IsReadonly = true; }).HasLabel("").Readonly();
        }
    }

    /// <summary>
    /// 评分明细 视图配置
    /// </summary>
    internal class MaintainRecordScoreViewModelViewConfig : WebViewConfig<MaintainRecordScoreViewModel>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(MaintainRecord));
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseClientOrder();
            View.ClearCommands();
            View.Property(p => p.ProjectName).Readonly();
            View.Property(p => p.CheckStandard).UseTextEditor(p => p.MaxLength = 1000).ShowInList(width: 200).Readonly();
            View.Property(p => p.Remark).Readonly();
            View.Property(p => p.Score).Readonly();
            View.Property(p => p.ExistProblem).Readonly();
        }
    }


    /// <summary>
    /// 领料单明细 视图配置
    /// </summary>
    internal class MateriaRquisitionDetailViewModelViewConfig : WebViewConfig<MateriaRquisitionDetailViewModel>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(MaintainRecord));
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseClientOrder();
            View.ClearCommands();            
            View.Property(p => p.ErpNo).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.TransactionTypeName).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.SourceWarehouseName).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.TargetWarehouseName).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.GenericDispositionsDescription).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.LineNum).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.ItemCode).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.ItemName).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.UnitName).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.ApplyNum).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.RealSendNum).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.Remark).Show(ShowInWhere.All).Readonly();
        }
    }

    /// <summary>
    /// 配件更换明细 视图配置
    /// </summary>
    internal class FittingChangeDetailViewModelViewConfig : WebViewConfig<FittingChangeDetailViewModel>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(MaintainRecord));
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseClientOrder();
            View.ClearCommands();
            View.Property(p => p.ItemCode).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.ItemName).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.KnifeNo).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.Qty).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.UnitName).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.AccountName).Show(ShowInWhere.All).Readonly();            
            View.Property(p => p.OldItemCode).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.OldItemName).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.OldKnifeNo).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.TimeLeng).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.Reason).Show(ShowInWhere.All).Readonly();
        }
    }
}
