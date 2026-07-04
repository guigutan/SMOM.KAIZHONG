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
	/// 异常任务自定义推送对象
	/// </summary>
	[ChildEntity, Serializable]
	//[CriteriaQuery]
	[Label("异常任务自定义推送对象")]
	public partial class TaskCustomPushTarget : DataEntity
	{
		#region 异常任务状态 TaskState
		/// <summary>
		/// 异常任务状态
		/// </summary>
		[Label("异常任务状态")]
		public static readonly Property<TaskStateEnum> TaskStateProperty = P<TaskCustomPushTarget>.Register(e => e.TaskState);

		/// <summary>
		/// 异常任务状态
		/// </summary>
		public TaskStateEnum TaskState
		{
			get { return GetProperty(TaskStateProperty); }
			set { SetProperty(TaskStateProperty, value); }
		}
		#endregion

		#region 员工 Employee
		/// <summary>
		/// 员工Id
		/// </summary>
		public static readonly IRefIdProperty EmployeeIdProperty = P<TaskCustomPushTarget>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

		/// <summary>
		/// 员工Id
		/// </summary>
		public double EmployeeId
		{
			get { return (double)GetRefId(EmployeeIdProperty); }
			set { SetRefId(EmployeeIdProperty, value); }
		}

		/// <summary>
		/// 员工
		/// </summary>
		public static readonly RefEntityProperty<Employee> EmployeeProperty = P<TaskCustomPushTarget>.RegisterRef(e => e.Employee, EmployeeIdProperty);

		/// <summary>
		/// 员工
		/// </summary>
		public Employee Employee
		{
			get { return GetRefEntity(EmployeeProperty); }
			set { SetRefEntity(EmployeeProperty, value); }
		}
		#endregion

		#region 异常监控任务 AbnormalMonitorTask
		/// <summary>
		/// 异常监控任务Id
		/// </summary>
		public static readonly IRefIdProperty AbnormalMonitorTaskIdProperty = P<TaskCustomPushTarget>.RegisterRefId(e => e.AbnormalMonitorTaskId, ReferenceType.Parent);

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
		public static readonly RefEntityProperty<AbnormalMonitorTask> AbnormalMonitorTaskProperty = P<TaskCustomPushTarget>.RegisterRef(e => e.AbnormalMonitorTask, AbnormalMonitorTaskIdProperty);

		/// <summary>
		/// 异常监控任务
		/// </summary>
		public AbnormalMonitorTask AbnormalMonitorTask
		{
			get { return GetRefEntity(AbnormalMonitorTaskProperty); }
			set { SetRefEntity(AbnormalMonitorTaskProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// 异常任务自定义推送对象 实体配置
	/// </summary>
	internal class TaskCustomPushTargetConfig : EntityConfig<TaskCustomPushTarget>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("ABNORMAL_TASK_CUSTOM_PUSH").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}