using SIE;
using SIE.AbnormalInfo.Common;
using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;

namespace SIE.AbnormalInfo.AbnormalMonitors
{
	/// <summary>
	/// 异常监控任务
	/// </summary>
	[RootEntity, Serializable]
	[ConditionQueryType(typeof(AbnormalMonitorTaskCriteria))]
	[EntityWithConfig(typeof(NoConfig), "异常任务编码配置项", "异常任务编码配置规则")]
	[Label("异常任务")]
	public partial class AbnormalMonitorTask : AbnormalMonitorTaskBase
	{
		#region 异常原因 AbnormalReason
		/// <summary>
		/// 异常原因
		/// </summary>
		[Label("异常原因")]
		[MaxLength(1000)]
		public static readonly Property<string> AbnormalReasonProperty = P<AbnormalMonitorTask>.Register(e => e.AbnormalReason);

		/// <summary>
		/// 异常原因
		/// </summary>
		public string AbnormalReason
		{
			get { return GetProperty(AbnormalReasonProperty); }
			set { SetProperty(AbnormalReasonProperty, value); }
		}
		#endregion

		#region 临时对策 TempMeasures
		/// <summary>
		/// 临时对策
		/// </summary>
		[Label("临时对策")]
		[MaxLength(1000)]
		public static readonly Property<string> TempMeasuresProperty = P<AbnormalMonitorTask>.Register(e => e.TempMeasures);

		/// <summary>
		/// 临时对策
		/// </summary>
		public string TempMeasures
		{
			get { return GetProperty(TempMeasuresProperty); }
			set { SetProperty(TempMeasuresProperty, value); }
		}
		#endregion

		#region 长期对策 LongMeasures
		/// <summary>
		/// 长期对策
		/// </summary>
		[Label("长期对策")]
		[MaxLength(1000)]
		public static readonly Property<string> LongMeasuresProperty = P<AbnormalMonitorTask>.Register(e => e.LongMeasures);

		/// <summary>
		/// 长期对策
		/// </summary>
		public string LongMeasures
		{
			get { return GetProperty(LongMeasuresProperty); }
			set { SetProperty(LongMeasuresProperty, value); }
		}
		#endregion

		#region 附件 AttachmentList
		/// <summary>
		/// 附件
		/// </summary>
		public static readonly ListProperty<EntityList<TaskAttachment>> AttachmentListProperty = P<AbnormalMonitorTask>.RegisterList(e => e.AttachmentList);
		/// <summary>
		/// 附件
		/// </summary>
		public EntityList<TaskAttachment> AttachmentList
		{
			get { return this.GetLazyList(AttachmentListProperty); }
		}
		#endregion

		#region 车间 WorkShop
		/// <summary>
		/// 车间Id
		/// </summary>
		[Label("车间")]
		public static readonly IRefIdProperty WorkShopIdProperty = P<AbnormalMonitorTask>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

		/// <summary>
		/// 车间Id
		/// </summary>
		public double? WorkShopId
		{
			get { return (double?)GetRefNullableId(WorkShopIdProperty); }
			set { SetRefNullableId(WorkShopIdProperty, value); }
		}

		/// <summary>
		/// 车间
		/// </summary>
		public static readonly RefEntityProperty<WorkShop> WorkShopProperty = P<AbnormalMonitorTask>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

		/// <summary>
		/// 车间
		/// </summary>
		public WorkShop WorkShop
		{
			get { return GetRefEntity(WorkShopProperty); }
			set { SetRefEntity(WorkShopProperty, value); }
		}
		#endregion

		#region 任务状态 TaskState
		/// <summary>
		/// 任务状态
		/// </summary>
		[Label("任务状态")]
		public static readonly Property<TaskStateEnum> TaskStateProperty = P<AbnormalMonitorTask>.Register(e => e.TaskState);

		/// <summary>
		/// 任务状态
		/// </summary>
		public TaskStateEnum TaskState
		{
			get { return GetProperty(TaskStateProperty); }
			set { SetProperty(TaskStateProperty, value); }
		}
		#endregion

		#region 产线 Line
		/// <summary>
		/// 产线Id
		/// </summary>
		[Label("产线")]
		public static readonly IRefIdProperty LineIdProperty = P<AbnormalMonitorTask>.RegisterRefId(e => e.LineId, ReferenceType.Normal);

		/// <summary>
		/// 产线Id
		/// </summary>
		public double? LineId
		{
			get { return (double?)GetRefNullableId(LineIdProperty); }
			set { SetRefNullableId(LineIdProperty, value); }
		}

		/// <summary>
		/// 产线
		/// </summary>
		public static readonly RefEntityProperty<WipResource> LineProperty = P<AbnormalMonitorTask>.RegisterRef(e => e.Line, LineIdProperty);

		/// <summary>
		/// 产线
		/// </summary>
		public WipResource Line
		{
			get { return GetRefEntity(LineProperty); }
			set { SetRefEntity(LineProperty, value); }
		}
		#endregion

		#region 触发任务 PushMethord
		/// <summary>
		/// 触发任务
		/// </summary>
		[Label("触发任务")]
		public static readonly Property<PushMethordEnum?> PushMethordProperty = P<AbnormalMonitorTask>.Register(e => e.PushMethord);

		/// <summary>
		/// 推送方式
		/// </summary>
		public PushMethordEnum? PushMethord
		{
			get { return GetProperty(PushMethordProperty); }
			set { SetProperty(PushMethordProperty, value); }
		}
		#endregion

		#region 任务处理人  TaskHandler
		/// <summary>
		/// 任务处理人Id
		/// </summary>
		[Label("任务处理人")]
		public static readonly IRefIdProperty TaskHandlerIdProperty = P<AbnormalMonitorTask>.RegisterRefId(e => e.TaskHandlerId, ReferenceType.Normal);

		/// <summary>
		/// 任务处理人Id
		/// </summary>
		public double? TaskHandlerId
		{
			get { return (double?)GetRefNullableId(TaskHandlerIdProperty); }
			set { SetRefNullableId(TaskHandlerIdProperty, value); }
		}

		/// <summary>
		///任务处理人
		/// </summary>
		public static readonly RefEntityProperty<Employee> TaskHandlerProperty = P<AbnormalMonitorTask>.RegisterRef(e => e.TaskHandler, TaskHandlerIdProperty);

		/// <summary>
		/// 任务处理人
		/// </summary>
		public Employee TaskHandler
		{
			get { return GetRefEntity(TaskHandlerProperty); }
			set { SetRefEntity(TaskHandlerProperty, value); }
		}
		#endregion

		#region 任务单号 TriggerNo
		/// <summary>
		/// 任务单号
		/// </summary>
		[Label("任务单号")]
		public static readonly Property<string> TriggerNoProperty = P<AbnormalMonitorTask>.Register(e => e.TriggerNo);

		/// <summary>
		/// 任务单号
		/// </summary>
		public string TriggerNo
		{
			get { return GetProperty(TriggerNoProperty); }
			set { SetProperty(TriggerNoProperty, value); }
		}
		#endregion

		#region 取消原因 CancelReason
		/// <summary>
		/// 取消原因
		/// </summary>
		[MaxLength(1000)]
		[Label("取消原因")]
		public static readonly Property<string> CancelReasonProperty = P<AbnormalMonitorTask>.Register(e => e.CancelReason);

		/// <summary>
		/// 取消原因
		/// </summary>
		public string CancelReason
		{
			get { return GetProperty(CancelReasonProperty); }
			set { SetProperty(CancelReasonProperty, value); }
		}
		#endregion

		#region 异常任务自定义推送对象列表 CustomPushTargetList
		/// <summary>
		/// 异常任务自定义推送对象列表
		/// </summary>
		public static readonly ListProperty<EntityList<TaskCustomPushTarget>> CustomPushTargetListProperty = P<AbnormalMonitorTask>.RegisterList(e => e.CustomPushTargetList);
		/// <summary>
		/// 异常任务自定义推送对象列表
		/// </summary>
		public EntityList<TaskCustomPushTarget> CustomPushTargetList
		{
			get { return this.GetLazyList(CustomPushTargetListProperty); }
		}
		#endregion

		#region 任务类型 TaskType
		/// <summary>
		/// 任务类型
		/// </summary>
		[Label("任务类型")]
		public static readonly Property<TaskType> TaskTypeProperty = P<AbnormalMonitorTask>.Register(e => e.TaskType);

		/// <summary>
		/// 任务类型
		/// </summary>
		public TaskType TaskType
		{
			get { return GetProperty(TaskTypeProperty); }
			set { SetProperty(TaskTypeProperty, value); }
		}
		#endregion

		#region 预警次数 WarnTimes
		/// <summary>
		/// 预警次数
		/// </summary>
		[Label("预警次数")]
		public static readonly Property<int?> WarnTimesProperty = P<AbnormalMonitorTask>.Register(e => e.WarnTimes);

		/// <summary>
		/// 预警次数
		/// </summary>
		public int? WarnTimes
		{
			get { return GetProperty(WarnTimesProperty); }
			set { SetProperty(WarnTimesProperty, value); }
		}
		#endregion

		#region 缺陷代码Id JoinDefectIds
		/// <summary>
		/// 缺陷代码Id
		/// </summary>
		[MaxLength(500)]
		[Label("缺陷代码")]
		public static readonly Property<string> JoinDefectIdsProperty = P<AbnormalMonitorTask>.Register(e => e.JoinDefectIds);

		/// <summary>
		/// 缺陷代码Id
		/// </summary>
		public string JoinDefectIds
		{
			get { return this.GetProperty(JoinDefectIdsProperty); }
			set { this.SetProperty(JoinDefectIdsProperty, value); }
		}
		#endregion

		#region 缺陷代码 JoinDefectCodes
		/// <summary>
		/// 缺陷代码
		/// </summary>
		[Label("缺陷代码")]
		[MaxLength(1000)]
		public static readonly Property<string> JoinDefectCodesProperty = P<AbnormalMonitorTask>.Register(e => e.JoinDefectCodes);

		/// <summary>
		/// 缺陷代码
		/// </summary>
		public string JoinDefectCodes
		{
			get { return this.GetProperty(JoinDefectCodesProperty); }
			set { this.SetProperty(JoinDefectCodesProperty, value); }
		}
		#endregion

		#region 缺陷代码描述 JoinDefectCodeDescriptions
		/// <summary>
		/// 缺陷代码描述
		/// </summary>
		[Label("缺陷代码描述")]
		[MaxLength(1000)]
		public static readonly Property<string> JoinDefectCodeDescriptionsProperty = P<AbnormalMonitorTask>.Register(e => e.JoinDefectCodeDescriptions);

		/// <summary>
		/// 缺陷代码描述
		/// </summary>
		[Label("缺陷代码描述")]
		public string JoinDefectCodeDescriptions
		{
			get { return this.GetProperty(JoinDefectCodeDescriptionsProperty); }
			set { this.SetProperty(JoinDefectCodeDescriptionsProperty, value); }
		}
		#endregion

		#region 视图属性


		#region 车间名称 WorkShopName
		/// <summary>
		/// 车间名称
		/// </summary>
		[Label("车间")]
		public static readonly Property<string> WorkShopNameProperty = P<AbnormalMonitorTask>.RegisterView(e => e.WorkShopName, p => p.WorkShop.Name);

		/// <summary>
		/// 车间名称
		/// </summary>
		public string WorkShopName
		{
			get { return this.GetProperty(WorkShopNameProperty); }
		}
		#endregion

		#region 产线名称 LineName
		/// <summary>
		/// 产线名称
		/// </summary>
		[Label("产线")]
		public static readonly Property<string> LineNameProperty = P<AbnormalMonitorTask>.RegisterView(e => e.LineName, p => p.Line.Name);

		/// <summary>
		/// 产线名称
		/// </summary>
		public string LineName
		{
			get { return this.GetProperty(LineNameProperty); }
		}
		#endregion

		#region 监控类型 MonitorType
		/// <summary>
		/// 监控类型
		/// </summary>
		[Label("监控类型")]
		public static readonly Property<string> MonitorTypeProperty = P<AbnormalMonitorTask>.RegisterView(e => e.MonitorType, p => p.AbnormalDefine.AbnormalRule.AbnomalSource.AbnormalEntityMetadata.Type);

		/// <summary>
		/// 监控类型
		/// </summary>
		public string MonitorType
		{
			get { return this.GetProperty(MonitorTypeProperty); }
		}
		#endregion

		#endregion

	}

	/// <summary>
	/// 异常监控任务 实体配置
	/// </summary>
	internal class AbnormalMonitorTaskConfig : EntityConfig<AbnormalMonitorTask>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("ABNORMAL_MONITOR_TASK").MapAllProperties();
			Meta.Property(AbnormalMonitorTask.ProblemConditionProperty).ColumnMeta.HasLength(2000);
			Meta.Property(AbnormalMonitorTask.ProblemDescriptionProperty).ColumnMeta.HasLength(3000);
			Meta.Property(AbnormalMonitorTask.AbnormalReasonProperty).ColumnMeta.HasLength(3000);
			Meta.Property(AbnormalMonitorTask.TempMeasuresProperty).ColumnMeta.HasLength(3000);
			Meta.Property(AbnormalMonitorTask.LongMeasuresProperty).ColumnMeta.HasLength(3000);
			Meta.Property(AbnormalMonitorTask.CancelReasonProperty).ColumnMeta.HasLength(3000);
			Meta.Property(AbnormalMonitorTask.JoinDefectIdsProperty).ColumnMeta.HasLength(1500);
			Meta.Property(AbnormalMonitorTask.JoinDefectCodesProperty).ColumnMeta.HasLength(3000);
			Meta.Property(AbnormalMonitorTask.JoinDefectCodeDescriptionsProperty).ColumnMeta.HasLength(3000);
			Meta.EnablePhantoms();
		}
	}
}