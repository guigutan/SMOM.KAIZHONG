using SIE;
using SIE.AbnormalInfo.Common;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.AbnormalInfo.AbnormalMonitors
{
	/// <summary>
	/// 异常任务处理日志
	/// </summary>
	[RootEntity, Serializable]
	//[CriteriaQuery]
	[Label("异常任务处理日志")]
	public partial class AbnormalMonitorTaskLog : DataEntity
	{
		#region 异常监控任务 AbnormalMonitorTask
		/// <summary>
		/// 异常监控任务Id
		/// </summary>
		public static readonly IRefIdProperty AbnormalMonitorTaskIdProperty = P<AbnormalMonitorTaskLog>.RegisterRefId(e => e.AbnormalMonitorTaskId, ReferenceType.Normal);

		/// <summary>
		/// 异常监控任务Id
		/// </summary>
		public double AbnormalMonitorTaskId
		{
			get { return (double)GetRefId(AbnormalMonitorTaskIdProperty); }
			set { SetRefId(AbnormalMonitorTaskIdProperty, value); }
		}

		/// <summary>
		/// 异常监控任务
		/// </summary>
		public static readonly RefEntityProperty<AbnormalMonitorTask> AbnormalMonitorTaskProperty = P<AbnormalMonitorTaskLog>.RegisterRef(e => e.AbnormalMonitorTask, AbnormalMonitorTaskIdProperty);

		/// <summary>
		/// 异常监控任务
		/// </summary>
		public AbnormalMonitorTask AbnormalMonitorTask
		{
			get { return GetRefEntity(AbnormalMonitorTaskProperty); }
			set { SetRefEntity(AbnormalMonitorTaskProperty, value); }
		}
		#endregion

		#region 执行动作 HandleAction
		/// <summary>
		/// 执行动作
		/// </summary>
		[Label("执行动作")]
		public static readonly Property<TaskHandleAction> TaskHandleActionProperty = P<AbnormalMonitorTaskLog>.Register(e => e.TaskHandleAction);

		/// <summary>
		/// 执行动作
		/// </summary>
		public TaskHandleAction TaskHandleAction
		{
			get { return GetProperty(TaskHandleActionProperty); }
			set { SetProperty(TaskHandleActionProperty, value); }
		}
		#endregion

		#region 处理节点 HandleNode
		/// <summary>
		/// 处理节点
		/// </summary>
		[Label("节点")]
		public static readonly Property<TaskStateEnum> HandleNodeProperty = P<AbnormalMonitorTaskLog>.Register(e => e.HandleNode);

		/// <summary>
		/// 处理节点
		/// </summary>
		public TaskStateEnum HandleNode
		{
			get { return GetProperty(HandleNodeProperty); }
			set { SetProperty(HandleNodeProperty, value); }
		}
		#endregion

		#region 内容 Content
		/// <summary>
		/// 内容
		/// </summary>
		[Label("内容")]
		[MaxLength(1000)]
		public static readonly Property<string> ContentProperty = P<AbnormalMonitorTaskLog>.Register(e => e.Content);

		/// <summary>
		/// 内容
		/// </summary>
		public string Content
		{
			get { return GetProperty(ContentProperty); }
			set { SetProperty(ContentProperty, value); }
		}
		#endregion

		#region 处理人 Handler
		/// <summary>
		/// 处理人Id
		/// </summary>
		[Label("处理人")]
		public static readonly IRefIdProperty HandlerIdProperty = P<AbnormalMonitorTaskLog>.RegisterRefId(e => e.HandlerId, ReferenceType.Normal);

		/// <summary>
		/// 处理人Id
		/// </summary>
		public double HandlerId
		{
			get { return (double)GetRefId(HandlerIdProperty); }
			set { SetRefId(HandlerIdProperty, value); }
		}

		/// <summary>
		/// 处理人
		/// </summary>
		public static readonly RefEntityProperty<Employee> HandlerProperty = P<AbnormalMonitorTaskLog>.RegisterRef(e => e.Handler, HandlerIdProperty);

		/// <summary>
		/// 处理人
		/// </summary>
		public Employee Handler
		{
			get { return GetRefEntity(HandlerProperty); }
			set { SetRefEntity(HandlerProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// 异常任务处理日志 实体配置
	/// </summary>
	internal class AbnormalMonitorTaskLogConfig : EntityConfig<AbnormalMonitorTaskLog>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("ABNORMAL_TASK_LOG").MapAllProperties();
			Meta.Property(AbnormalMonitorTaskLog.ContentProperty).ColumnMeta.HasLength(3000);
			Meta.EnablePhantoms();
		}
	}
}