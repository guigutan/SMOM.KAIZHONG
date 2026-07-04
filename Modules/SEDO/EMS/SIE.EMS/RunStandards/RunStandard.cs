using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Domain;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipModels;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.RunStandards
{
	/// <summary>
	/// 设备运行定标
	/// </summary>
	[RootEntity, Serializable]	
	[Label("设备运行定标")]
	[ConditionQueryType(typeof(RunStandardCriteria))]
	[DisplayMember(nameof(No))]
	[EntityWithConfig(typeof(ApprovalConfig))]
	[EntityWithConfig(typeof(NoConfig), "设备运行定标单号配置项", "设备运行定标单号生成规则")]
	public partial class RunStandard : DataEntity
	{
		#region 定标单号 No
		/// <summary>
		/// 定标单号
		/// </summary>
		[Label("定标单号")]
		public static readonly Property<string> NoProperty = P<RunStandard>.Register(e => e.No);

		/// <summary>
		/// 定标单号
		/// </summary>
		public string No
		{
			get { return GetProperty(NoProperty); }
			set { SetProperty(NoProperty, value); }
		}
		#endregion

		#region 定标名称 Name
		/// <summary>
		/// 定标名称
		/// </summary>
		[Label("定标名称")]
		[Required]
		public static readonly Property<string> NameProperty = P<RunStandard>.Register(e => e.Name);

		/// <summary>
		/// 定标名称
		/// </summary>
		public string Name
		{
			get { return GetProperty(NameProperty); }
			set { SetProperty(NameProperty, value); }
		}
		#endregion

		#region 设备型号 EquipModel
		/// <summary>
		/// 设备型号Id
		/// </summary>
		[Label("设备型号")]
		public static readonly IRefIdProperty EquipModelIdProperty = P<RunStandard>.RegisterRefId(e => e.EquipModelId, ReferenceType.Normal);

		/// <summary>
		/// 设备型号Id
		/// </summary>
		public double EquipModelId
		{
			get { return (double)GetRefId(EquipModelIdProperty); }
			set { SetRefId(EquipModelIdProperty, value); }
		}

		/// <summary>
		/// 设备型号
		/// </summary>
		public static readonly RefEntityProperty<EquipModel> EquipModelProperty = P<RunStandard>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

		/// <summary>
		/// 设备型号
		/// </summary>
		public EquipModel EquipModel
		{
			get { return GetRefEntity(EquipModelProperty); }
			set { SetRefEntity(EquipModelProperty, value); }
		}
		#endregion

		#region 维修工时 RepairHours
		/// <summary>
		/// 维修工时
		/// </summary>
		[Label("维修工时")]
		public static readonly Property<string> RepairHoursProperty = P<RunStandard>.Register(e => e.RepairHours);

		/// <summary>
		/// 维修工时
		/// </summary>
		public string RepairHours
		{
			get { return GetProperty(RepairHoursProperty); }
			set { SetProperty(RepairHoursProperty, value); }
		}
		#endregion

		#region 备注 Remark
		/// <summary>
		/// 备注
		/// </summary>
		[MaxLength(1000)]
		[Label("备注")]
		public static readonly Property<string> RemarkProperty = P<RunStandard>.Register(e => e.Remark);

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark
		{
			get { return GetProperty(RemarkProperty); }
			set { SetProperty(RemarkProperty, value); }
		}
		#endregion

		#region 操作记录 RunStandardLogList
		/// <summary>
		/// 操作记录
		/// </summary>
		[Label("操作记录")]
		public static readonly ListProperty<EntityList<RunStandardLog>> RunStandardLogListProperty = P<RunStandard>.RegisterList(e => e.RunStandardLogList);
		/// <summary>
		/// 操作记录
		/// </summary>
		public EntityList<RunStandardLog> RunStandardLogList
		{
			get { return this.GetLazyList(RunStandardLogListProperty); }
		}
		#endregion

		#region 设备清单 RunStandardEquipmentList
		/// <summary>
		/// 设备清单
		/// </summary>
		[Label("设备清单")]
		public static readonly ListProperty<EntityList<RunStandardEquipment>> RunStandardEquipmentListProperty = P<RunStandard>.RegisterList(e => e.RunStandardEquipmentList);
		/// <summary>
		/// 设备清单
		/// </summary>
		public EntityList<RunStandardEquipment> RunStandardEquipmentList
		{
			get { return this.GetLazyList(RunStandardEquipmentListProperty); }
		}
		#endregion

		#region 维修标准 RunStandardProjectList
		/// <summary>
		/// 维修标准
		/// </summary>
		[Label("维修标准")]
		public static readonly ListProperty<EntityList<RunStandardProject>> RunStandardProjectListProperty = P<RunStandard>.RegisterList(e => e.RunStandardProjectList);
		/// <summary>
		/// 维修标准
		/// </summary>
		public EntityList<RunStandardProject> RunStandardProjectList
		{
			get { return this.GetLazyList(RunStandardProjectListProperty); }
		}
		#endregion

		#region 定标量 RunStandardValueList
		/// <summary>
		/// 定标量
		/// </summary>
		[Label("定标量")]
		public static readonly ListProperty<EntityList<RunStandardValue>> RunStandardValueListProperty = P<RunStandard>.RegisterList(e => e.RunStandardValueList);
		/// <summary>
		/// 定标量
		/// </summary>
		public EntityList<RunStandardValue> RunStandardValueList
		{
			get { return this.GetLazyList(RunStandardValueListProperty); }
		}
		#endregion

		#region 审核状态 ApprovalStatus
		/// <summary>
		/// 审核状态
		/// </summary>
		[Label("审核状态")]
		public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<RunStandard>.Register(e => e.ApprovalStatus);

		/// <summary>
		/// 审核状态
		/// </summary>
		public ApprovalStatus ApprovalStatus
		{
			get { return GetProperty(ApprovalStatusProperty); }
			set { SetProperty(ApprovalStatusProperty, value); }
		}
		#endregion

		#region 制定人 CreateName
		/// <summary>
		/// 制定人
		/// </summary>
		[Label("制定人")]
        public static readonly Property<string> CreateNameProperty = P<RunStandard>.Register(e => e.CreateName);

		/// <summary>
		/// 制定人
		/// </summary>
		public string CreateName
		{
            get { return this.GetProperty(CreateNameProperty); }
            set { this.SetProperty(CreateNameProperty, value); }
        }
        #endregion

    }

	/// <summary>
	/// 设备运行定标 实体配置
	/// </summary>
	internal class RunStandardConfig : EntityConfig<RunStandard>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_RUN_STD").MapAllPropertiesExcept(RunStandard.CreateNameProperty);
			Meta.Property(RunStandard.RemarkProperty).ColumnMeta.HasLength(4000);
			Meta.EnablePhantoms();
		}
	}
}