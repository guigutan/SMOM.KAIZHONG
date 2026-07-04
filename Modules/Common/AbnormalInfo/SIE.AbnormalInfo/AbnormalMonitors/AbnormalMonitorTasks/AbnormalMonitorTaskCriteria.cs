using SIE.AbnormalInfo.Common;
using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.AbnormalInfo.AbnormalMonitors
{
	/// <summary>
	/// 异常任务查询实体
	/// </summary>
	[QueryEntity, Serializable]
	[Label("异常任务查询实体")]
	public partial class AbnormalMonitorTaskCriteria : Criteria
    {
		#region 异常描述 ProblemDescription
		/// <summary>
		/// 异常描述
		/// </summary>
		[Label("异常描述")]
		[MaxLength(1000)]
		public static readonly Property<string> ProblemDescriptionProperty = P<AbnormalMonitorTaskCriteria>.Register(e => e.ProblemDescription);

		/// <summary>
		/// 问题描述
		/// </summary>
		public string ProblemDescription
		{
			get { return GetProperty(ProblemDescriptionProperty); }
			set { SetProperty(ProblemDescriptionProperty, value); }
		}
		#endregion

		#region 异常定义 AbnormalDefine
		/// <summary>
		/// 异常定义Id
		/// </summary>
		[Label("异常定义")]
		public static readonly IRefIdProperty AbnormalDefineIdProperty = P<AbnormalMonitorTaskCriteria>.RegisterRefId(e => e.AbnormalDefineId, ReferenceType.Normal);

		/// <summary>
		/// 异常定义Id
		/// </summary>
		public double? AbnormalDefineId
		{
			get { return (double?)GetRefNullableId(AbnormalDefineIdProperty); }
			set { SetRefNullableId(AbnormalDefineIdProperty, value); }
		}

		/// <summary>
		/// 异常定义
		/// </summary>
		public static readonly RefEntityProperty<AbnormalDefine> AbnormalDefineProperty = P<AbnormalMonitorTaskCriteria>.RegisterRef(e => e.AbnormalDefine, AbnormalDefineIdProperty);

		/// <summary>
		/// 异常定义
		/// </summary>
		public AbnormalDefine AbnormalDefine
		{
			get { return GetRefEntity(AbnormalDefineProperty); }
			set { SetRefEntity(AbnormalDefineProperty, value); }
		}
		#endregion

		#region 编码 Code
		/// <summary>
		/// 编码
		/// </summary>
		[Label("编码")]
		public static readonly Property<string> CodeProperty = P<AbnormalMonitorTaskCriteria>.Register(e => e.Code);

		/// <summary>
		/// 编码
		/// </summary>
		public string Code
		{
			get { return GetProperty(CodeProperty); }
			set { SetProperty(CodeProperty, value); }
		}
		#endregion

		#region 异常名称 AbnormalName
		/// <summary>
		/// 异常名称
		/// </summary>
		[Label("异常名称")]
		public static readonly Property<string> AbnormalNameProperty = P<AbnormalMonitorTaskCriteria>.Register(e => e.AbnormalName);

		/// <summary>
		/// 异常名称
		/// </summary>
		public string AbnormalName
		{
			get { return GetProperty(AbnormalNameProperty); }
			set { SetProperty(AbnormalNameProperty, value); }
		}
		#endregion

		#region 车间 WorkShop
		/// <summary>
		/// 车间Id
		/// </summary>
		[Label("车间")]
		public static readonly IRefIdProperty WorkShopIdProperty = P<AbnormalMonitorTaskCriteria>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<WorkShop> WorkShopProperty = P<AbnormalMonitorTaskCriteria>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

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
		[Label("异常任务状态")]
		public static readonly Property<TaskStateEnum?> TaskStateProperty = P<AbnormalMonitorTaskCriteria>.Register(e => e.TaskState);

		/// <summary>
		/// 任务状态
		/// </summary>
		public TaskStateEnum? TaskState
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
		public static readonly IRefIdProperty LineIdProperty = P<AbnormalMonitorTaskCriteria>.RegisterRefId(e => e.LineId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<Resource> LineProperty = P<AbnormalMonitorTaskCriteria>.RegisterRef(e => e.Line, LineIdProperty);

		/// <summary>
		/// 产线
		/// </summary>
		public Resource Line
		{
			get { return GetRefEntity(LineProperty); }
			set { SetRefEntity(LineProperty, value); }
		}
		#endregion

		#region 任务类型 TaskType
		/// <summary>
		/// 任务类型
		/// </summary>
		[Label("任务类型")]
		public static readonly Property<TaskType?> TaskTypeProperty = P<AbnormalMonitorTaskCriteria>.Register(e => e.TaskType);

		/// <summary>
		/// 任务类型
		/// </summary>
		public TaskType? TaskType
		{
			get { return GetProperty(TaskTypeProperty); }
			set { SetProperty(TaskTypeProperty, value); }
		}
		#endregion

		#region 创建时间 CreateDate
		/// <summary>
		/// 创建时间
		/// </summary>
		[Label("创建时间")]
		public static readonly Property<DateRange> CreateDateProperty = P<AbnormalMonitorTaskCriteria>.Register(e => e.CreateDate);

		/// <summary>
		/// 创建时间
		/// </summary>
		public DateRange CreateDate
		{
			get { return GetProperty(CreateDateProperty); }
			set { SetProperty(CreateDateProperty, value); }
		}
		#endregion

		/// <summary>
		/// 查询异常定义
		/// </summary>
		/// <returns>来料检验单列表</returns>
		protected override EntityList Fetch()
		{
			return RT.Service.Resolve<AbnormalMonitorTaskService>().GetAbnormalTasks(this);
		}
	}
}
