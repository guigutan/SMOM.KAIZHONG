using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.EMS.Enums;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.EMS.EquipRepair.EquipRepairs.Enums;
using SIE.EMS.EquipRepairs.Enums;
using SIE.EMS.Projects;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.EquipRepair.PlanRepairs
{
	/// <summary>
	/// 计划维修
	/// </summary>
	[RootEntity, Serializable]
	[Label("计划维修")]
	[ConditionQueryType(typeof(PlanRepairCriteria))]
	[DisplayMember(nameof(No))]
	[EntityWithConfig(typeof(ApprovalConfig))]
	[EntityWithConfig(typeof(NoConfig), "计划维修单号配置项", "计划维修单号生成规则")]
	public partial class PlanRepair : DataEntity
	{
		#region 计划单号 No
		/// <summary>
		/// 计划单号
		/// </summary>
		[Label("计划单号")]
		public static readonly Property<string> NoProperty = P<PlanRepair>.Register(e => e.No);

		/// <summary>
		/// 计划单号
		/// </summary>
		public string No
		{
			get { return GetProperty(NoProperty); }
			set { SetProperty(NoProperty, value); }
		}
		#endregion

		#region 计划名称 Name
		/// <summary>
		/// 计划名称
		/// </summary>
		[Required]
		[Label("计划名称")]
		public static readonly Property<string> NameProperty = P<PlanRepair>.Register(e => e.Name);

		/// <summary>
		/// 计划名称
		/// </summary>
		public string Name
		{
			get { return GetProperty(NameProperty); }
			set { SetProperty(NameProperty, value); }
		}
		#endregion

		#region 维修工时 RepairHours
		/// <summary>
		/// 维修工时
		/// </summary>
		[Label("维修工时")]
		public static readonly Property<decimal> RepairHoursProperty = P<PlanRepair>.Register(e => e.RepairHours);

		/// <summary>
		/// 维修工时
		/// </summary>
		public decimal RepairHours
		{
			get { return GetProperty(RepairHoursProperty); }
			set { SetProperty(RepairHoursProperty, value); }
		}
		#endregion

		#region 周期量 Amount
		/// <summary>
		/// 周期量
		/// </summary>
		[Label("周期量")]
		public static readonly Property<int?> AmountProperty = P<PlanRepair>.Register(e => e.Amount);

		/// <summary>
		/// 周期量
		/// </summary>
		public int? Amount
		{
			get { return GetProperty(AmountProperty); }
			set { SetProperty(AmountProperty, value); }
		}
		#endregion

		#region 累加数 RoundAmount
		/// <summary>
		/// 累加数
		/// </summary>
		[Label("累加数")]
		public static readonly Property<int?> RoundAmountProperty = P<PlanRepair>.Register(e => e.RoundAmount);

		/// <summary>
		/// 累加数
		/// </summary>
		public int? RoundAmount
		{
			get { return GetProperty(RoundAmountProperty); }
			set { SetProperty(RoundAmountProperty, value); }
		}
		#endregion

		#region 上次执行日期 LastExecuteDate
		/// <summary>
		/// 上次执行日期
		/// </summary>
		[Label("上次执行日期")]
		public static readonly Property<DateTime?> LastExecuteDateProperty = P<PlanRepair>.Register(e => e.LastExecuteDate);

		/// <summary>
		/// 上次执行日期
		/// </summary>
		public DateTime? LastExecuteDate
		{
			get { return GetProperty(LastExecuteDateProperty); }
			set { SetProperty(LastExecuteDateProperty, value); }
		}
		#endregion

		#region 预警期 LeadTime
		/// <summary>
		/// 预警期
		/// </summary>
		[Label("预警期")]
		public static readonly Property<int?> LeadTimeProperty = P<PlanRepair>.Register(e => e.LeadTime);

		/// <summary>
		/// 预警期
		/// </summary>
		public int? LeadTime
		{
			get { return GetProperty(LeadTimeProperty); }
			set { SetProperty(LeadTimeProperty, value); }
		}
		#endregion

		#region 备注 Remark
		/// <summary>
		/// 备注
		/// </summary>
		[MaxLength(1000)]
		[Label("备注")]
		public static readonly Property<string> RemarkProperty = P<PlanRepair>.Register(e => e.Remark);

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark
		{
			get { return GetProperty(RemarkProperty); }
			set { SetProperty(RemarkProperty, value); }
		}
		#endregion

		#region 单据来源 BillSourceType
		/// <summary>
		/// 单据来源
		/// </summary>
		[Label("单据来源")]
		public static readonly Property<BillSourceType> BillSourceTypeProperty = P<PlanRepair>.Register(e => e.BillSourceType);

		/// <summary>
		/// 单据来源
		/// </summary>
		public BillSourceType BillSourceType
		{
			get { return GetProperty(BillSourceTypeProperty); }
			set { SetProperty(BillSourceTypeProperty, value); }
		}
		#endregion

		#region 项目编码 Project
		/// <summary>
		/// 项目编码Id
		/// </summary>
		[Label("项目编码")]
		public static readonly IRefIdProperty ProjectIdProperty = P<PlanRepair>.RegisterRefId(e => e.ProjectId, ReferenceType.Normal);

		/// <summary>
		/// 项目编码Id
		/// </summary>
		public double? ProjectId
		{
			get { return (double?)GetRefNullableId(ProjectIdProperty); }
			set { SetRefNullableId(ProjectIdProperty, value); }
		}

		/// <summary>
		/// 项目编码
		/// </summary>
		public static readonly RefEntityProperty<Project> ProjectProperty = P<PlanRepair>.RegisterRef(e => e.Project, ProjectIdProperty);

		/// <summary>
		/// 项目编码
		/// </summary>
		public Project Project
		{
			get { return GetRefEntity(ProjectProperty); }
			set { SetRefEntity(ProjectProperty, value); }
		}
		#endregion

		#region 项目事项 ProjectKeyItem
		/// <summary>
		/// 项目事项Id
		/// </summary>
		[Label("项目事项")]
		public static readonly IRefIdProperty ProjectKeyItemIdProperty = P<PlanRepair>.RegisterRefId(e => e.ProjectKeyItemId, ReferenceType.Normal);

		/// <summary>
		/// 项目事项Id
		/// </summary>
		public double? ProjectKeyItemId
		{
			get { return (double?)GetRefNullableId(ProjectKeyItemIdProperty); }
			set { SetRefNullableId(ProjectKeyItemIdProperty, value); }
		}

		/// <summary>
		/// 项目事项
		/// </summary>
		public static readonly RefEntityProperty<ProjectKeyItem> ProjectKeyItemProperty = P<PlanRepair>.RegisterRef(e => e.ProjectKeyItem, ProjectKeyItemIdProperty);

		/// <summary>
		/// 项目事项
		/// </summary>
		public ProjectKeyItem ProjectKeyItem
		{
			get { return GetRefEntity(ProjectKeyItemProperty); }
			set { SetRefEntity(ProjectKeyItemProperty, value); }
		}
		#endregion

		#region 设备 EquipAccount
		/// <summary>
		/// 设备Id
		/// </summary>
		[Label("设备编码")]
		public static readonly IRefIdProperty EquipAccountIdProperty = P<PlanRepair>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

		/// <summary>
		/// 设备Id
		/// </summary>
		public double EquipAccountId
		{
			get { return (double)GetRefId(EquipAccountIdProperty); }
			set { SetRefId(EquipAccountIdProperty, value); }
		}

		/// <summary>
		/// 设备
		/// </summary>
		public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty = P<PlanRepair>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

		/// <summary>
		/// 设备
		/// </summary>
		public EquipAccountSelect EquipAccount
		{
			get { return GetRefEntity(EquipAccountProperty); }
			set { SetRefEntity(EquipAccountProperty, value); }
		}
		#endregion

		#region 审核状态 ApprovalStatus
		/// <summary>
		/// 审核状态
		/// </summary>
		[Label("审核状态")]
		public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<PlanRepair>.Register(e => e.ApprovalStatus);

		/// <summary>
		/// 审核状态
		/// </summary>
		public ApprovalStatus ApprovalStatus
		{
			get { return GetProperty(ApprovalStatusProperty); }
			set { SetProperty(ApprovalStatusProperty, value); }
		}
		#endregion

		#region 维修定标 EquipAccountRepairStandard
		/// <summary>
		/// 维修定标Id
		/// </summary>
		[Label("维修定标")]
		public static readonly IRefIdProperty EquipAccRepairStandardIdProperty = P<PlanRepair>.RegisterRefId(e => e.EquipAccRepairStandardId, ReferenceType.Normal);

		/// <summary>
		/// 维修定标Id
		/// </summary>
		public double? EquipAccRepairStandardId
		{
			get { return (double?)GetRefNullableId(EquipAccRepairStandardIdProperty); }
			set { SetRefNullableId(EquipAccRepairStandardIdProperty, value); }
		}

		/// <summary>
		/// 维修定标
		/// </summary>
		public static readonly RefEntityProperty<EquipAccountRepairStandard> EquipAccountRepairStandardProperty = P<PlanRepair>.RegisterRef(e => e.EquipAccountRepairStandard, EquipAccRepairStandardIdProperty);

		/// <summary>
		/// 维修定标
		/// </summary>
		public EquipAccountRepairStandard EquipAccountRepairStandard
		{
			get { return GetRefEntity(EquipAccountRepairStandardProperty); }
			set { SetRefEntity(EquipAccountRepairStandardProperty, value); }
		}
		#endregion

		#region 定标类型 StandardType
		/// <summary>
		/// 定标类型
		/// </summary>
		[Label("定标类型")]
		public static readonly Property<StandardType?> StandardTypeProperty = P<PlanRepair>.Register(e => e.StandardType);

		/// <summary>
		/// 定标类型
		/// </summary>
		public StandardType? StandardType
		{
			get { return GetProperty(StandardTypeProperty); }
			set { SetProperty(StandardTypeProperty, value); }
		}
		#endregion

		#region 维修规程 PlanRepairProjectList
		/// <summary>
		/// 维修规程
		/// </summary>
		[Label("维修规程")]
		public static readonly ListProperty<EntityList<PlanRepairProject>> PlanRepairProjectListProperty = P<PlanRepair>.RegisterList(e => e.PlanRepairProjectList);
		/// <summary>
		/// 维修规程
		/// </summary>
		public EntityList<PlanRepairProject> PlanRepairProjectList
		{
			get { return this.GetLazyList(PlanRepairProjectListProperty); }
		}
		#endregion

		#region 维修单 EquipRepairBill
		/// <summary>
		/// 维修单Id
		/// </summary>
		[Label("维修单")]
		public static readonly IRefIdProperty EquipRepairBillIdProperty = P<PlanRepair>.RegisterRefId(e => e.EquipRepairBillId, ReferenceType.Normal);

		/// <summary>
		/// 维修单Id
		/// </summary>
		public double? EquipRepairBillId
		{
			get { return (double?)GetRefNullableId(EquipRepairBillIdProperty); }
			set { SetRefNullableId(EquipRepairBillIdProperty, value); }
		}

		/// <summary>
		/// 维修单
		/// </summary>
		public static readonly RefEntityProperty<EquipRepairBill> EquipRepairBillProperty = P<PlanRepair>.RegisterRef(e => e.EquipRepairBill, EquipRepairBillIdProperty);

		/// <summary>
		/// 维修单
		/// </summary>
		public EquipRepairBill EquipRepairBill
		{
			get { return GetRefEntity(EquipRepairBillProperty); }
			set { SetRefEntity(EquipRepairBillProperty, value); }
		}
		#endregion

		#region 定标单号 RunStandardNo	
		/// <summary>
		/// 定标单号
		/// </summary>
		[Label("定标单号")]
		public static readonly Property<string> RunStandardNoProperty = P<PlanRepair>.Register(e => e.RunStandardNo);

		/// <summary>
		/// 定标单号
		/// </summary>
		public string RunStandardNo
		{
			get { return this.GetProperty(RunStandardNoProperty); }
			set { this.SetProperty(RunStandardNoProperty, value); }
		}
		#endregion

		#region 设备名称 EquipName
		/// <summary>
		/// 设备名称
		/// </summary>
		[Label("设备名称")]
		public static readonly Property<string> EquipNameProperty = P<PlanRepair>.RegisterView(e => e.EquipName, p => p.EquipAccount.Name);

		/// <summary>
		/// 设备名称
		/// </summary>
		public string EquipName
		{
			get { return this.GetProperty(EquipNameProperty); }
		}
		#endregion

		#region 设备型号 EquipAccountModelId
		/// <summary>
		/// 设备型号
		/// </summary>
		[Label("设备型号")]
		public static readonly Property<double?> EquipAccountModelIdProperty = P<PlanRepair>.RegisterView(e => e.EquipAccountModelId, p => p.EquipAccount.EquipModelId);

		/// <summary>
		/// 设备型号
		/// </summary>
		public double? EquipAccountModelId
		{
			get { return this.GetProperty(EquipAccountModelIdProperty); }
		}
		#endregion

		#region 周期单位 Unit
		/// <summary>
		/// 周期单位
		/// </summary>
		[Label("周期单位")]
		public static readonly Property<StandardUnit?> StandardUnitProperty = P<PlanRepair>.Register(e => e.StandardUnit);

		/// <summary>
		/// 周期单位
		/// </summary>
		public StandardUnit? StandardUnit
		{
			get { return this.GetProperty(StandardUnitProperty); }
			set { this.SetProperty(StandardUnitProperty, value); }
		}
		#endregion

		#region 维修状态 RepairState
		/// <summary>
		/// 维修状态
		/// </summary>
		[Label("维修状态")]
		public static readonly Property<EquipRepairState?> RepairStateProperty = P<PlanRepair>.RegisterView(e => e.RepairState, p => p.EquipRepairBill.RepairState);

		/// <summary>
		/// 维修状态
		/// </summary>
		public EquipRepairState? RepairState
		{
			get { return this.GetProperty(RepairStateProperty); }
		}
		#endregion

		#region 已关闭 Close
		/// <summary>
		/// 已关闭
		/// </summary>
		[Label("已关闭")]
		public static readonly Property<YesNo> CloseProperty = P<PlanRepair>.Register(e => e.Close);

		/// <summary>
		/// 已关闭
		/// </summary>
		public YesNo Close
		{
			get { return this.GetProperty(CloseProperty); }
			set { this.SetProperty(CloseProperty, value); }
		}
		#endregion

		#region 设备状态 EquipAccountState
		/// <summary>
		/// 设备状态
		/// </summary>
		[Label("设备状态")]
		public static readonly Property<AccountState> EquipAccountStateProperty = P<PlanRepair>.RegisterView(e => e.EquipAccountState, e => e.EquipAccount.State);

		/// <summary>
		/// 设备状态
		/// </summary>
		public AccountState EquipAccountState
		{
			get { return GetProperty(EquipAccountStateProperty); }
			set { SetProperty(EquipAccountStateProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// 计划维修 实体配置
	/// </summary>
	internal class PlanRepairConfig : EntityConfig<PlanRepair>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_PLAN_REPR").MapAllProperties();
			Meta.Property(PlanRepair.RemarkProperty).ColumnMeta.HasLength(2000);
			Meta.EnablePhantoms();
		}
	}
}