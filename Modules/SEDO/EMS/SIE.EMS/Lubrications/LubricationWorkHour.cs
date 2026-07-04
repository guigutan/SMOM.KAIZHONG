using SIE;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.EMS.Lubrications
{
	/// <summary>
	/// 润滑工时登记
	/// </summary>
	[ChildEntity, Serializable]
	//[CriteriaQuery]
	[Label("润滑工时登记")]
	public partial class LubricationWorkHour : DataEntity
	{
		#region 润滑开始时间 StartDateTime
		/// <summary>
		/// 润滑开始时间
		/// </summary>
		[Label("润滑开始时间")]
		public static readonly Property<DateTime> StartDateTimeProperty = P<LubricationWorkHour>.Register(e => e.StartDateTime);

		/// <summary>
		/// 润滑开始时间
		/// </summary>
		public DateTime StartDateTime
		{
			get { return GetProperty(StartDateTimeProperty); }
			set { SetProperty(StartDateTimeProperty, value); }
		}
		#endregion

		#region 润滑结束时间 EndDateTime
		/// <summary>
		/// 润滑结束时间
		/// </summary>
		[Label("润滑结束时间")]
		public static readonly Property<DateTime> EndDateTimeProperty = P<LubricationWorkHour>.Register(e => e.EndDateTime);

		/// <summary>
		/// 润滑结束时间
		/// </summary>
		public DateTime EndDateTime
		{
			get { return GetProperty(EndDateTimeProperty); }
			set { SetProperty(EndDateTimeProperty, value); }
		}
		#endregion

		#region 工时(H) Hours
		/// <summary>
		/// 工时(H)
		/// </summary>
		[Label("工时(H)")]
		public static readonly Property<decimal> HoursProperty = P<LubricationWorkHour>.Register(e => e.Hours);

		/// <summary>
		/// 工时(H)
		/// </summary>
		public decimal Hours
		{
			get { return GetProperty(HoursProperty); }
			set { SetProperty(HoursProperty, value); }
		}
		#endregion

		#region 执行人 Executor
		/// <summary>
		/// 执行人Id
		/// </summary>
		public static readonly IRefIdProperty ExecutorIdProperty = P<LubricationWorkHour>.RegisterRefId(e => e.ExecutorId, ReferenceType.Normal);

		/// <summary>
		/// 执行人Id
		/// </summary>
		public double ExecutorId
		{
			get { return (double)GetRefId(ExecutorIdProperty); }
			set { SetRefId(ExecutorIdProperty, value); }
		}

		/// <summary>
		/// 执行人
		/// </summary>
		public static readonly RefEntityProperty<Employee> ExecutorProperty = P<LubricationWorkHour>.RegisterRef(e => e.Executor, ExecutorIdProperty);

		/// <summary>
		/// 执行人
		/// </summary>
		public Employee Executor
		{
			get { return GetRefEntity(ExecutorProperty); }
			set { SetRefEntity(ExecutorProperty, value); }
		}
		#endregion

		#region 执行人名称 ExecutorName
		/// <summary>
		/// 执行人名称
		/// </summary>
		[Label("执行人")]
		public static readonly Property<string> ExecutorNameProperty = P<LubricationWorkHour>.RegisterView(e => e.ExecutorName, p => p.Executor.Name);

		/// <summary>
		/// 设备型号
		/// </summary>
		public string ExecutorName
		{
			get { return this.GetProperty(ExecutorNameProperty); }
		}
		#endregion

		#region 工时登记 Lubrication
		/// <summary>
		/// 工时登记Id
		/// </summary>
		public static readonly IRefIdProperty LubricationIdProperty = P<LubricationWorkHour>.RegisterRefId(e => e.LubricationId, ReferenceType.Parent);

		/// <summary>
		/// 工时登记Id
		/// </summary>
		public double LubricationId
		{
			get { return (double)GetRefId(LubricationIdProperty); }
			set { SetRefId(LubricationIdProperty, value); }
		}

		/// <summary>
		/// 工时登记
		/// </summary>
		public static readonly RefEntityProperty<Lubrication> LubricationProperty = P<LubricationWorkHour>.RegisterRef(e => e.Lubrication, LubricationIdProperty);

		/// <summary>
		/// 工时登记
		/// </summary>
		public Lubrication Lubrication
		{
			get { return GetRefEntity(LubricationProperty); }
			set { SetRefEntity(LubricationProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// 润滑工时登记 实体配置
	/// </summary>
	internal class LubricationWorkHourConfig : EntityConfig<LubricationWorkHour>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_LUBR_WH").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}