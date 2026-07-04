using SIE.Domain; 
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.EMS.RunStandards
{
	/// <summary>
	/// 设备运行定标操作记录
	/// </summary>
	[ChildEntity, Serializable]	
	[Label("设备运行定标操作记录")]
	public partial class RunStandardLog : DataEntity
	{
		#region 操作类型 OperationTypeText
		/// <summary>
		/// 操作类型
		/// </summary>
		[Label("操作类型")]
		public static readonly Property<string> OperationTypeTextProperty = P<RunStandardLog>.Register(e => e.OperationTypeText);

		/// <summary>
		/// 操作类型
		/// </summary>
		public string OperationTypeText
		{
			get { return GetProperty(OperationTypeTextProperty); }
			set { SetProperty(OperationTypeTextProperty, value); }
		}
		#endregion

		#region 操作时间 OperationDateTime
		/// <summary>
		/// 操作时间
		/// </summary>
		[Label("操作时间")]
		public static readonly Property<DateTime> OperationDateTimeProperty = P<RunStandardLog>.Register(e => e.OperationDateTime);

		/// <summary>
		/// 操作时间
		/// </summary>
		public DateTime OperationDateTime
		{
			get { return GetProperty(OperationDateTimeProperty); }
			set { SetProperty(OperationDateTimeProperty, value); }
		}
		#endregion

		#region 操作人 Operator
		/// <summary>
		/// 操作人Id
		/// </summary>
		[Label("操作人")]
		public static readonly IRefIdProperty OperatorIdProperty = P<RunStandardLog>.RegisterRefId(e => e.OperatorId, ReferenceType.Normal);

		/// <summary>
		/// 操作人Id
		/// </summary>
		public double OperatorId
		{
			get { return (double)GetRefId(OperatorIdProperty); }
			set { SetRefId(OperatorIdProperty, value); }
		}

		/// <summary>
		/// 操作人
		/// </summary>
		public static readonly RefEntityProperty<Employee> OperatorProperty = P<RunStandardLog>.RegisterRef(e => e.Operator, OperatorIdProperty);

		/// <summary>
		/// 操作人
		/// </summary>
		public Employee Operator
		{
			get { return GetRefEntity(OperatorProperty); }
			set { SetRefEntity(OperatorProperty, value); }
		}
		#endregion

		#region 设备运行定标 RunStandard
		/// <summary>
		/// 设备运行定标Id
		/// </summary>
		[Label("设备运行定标")]
		public static readonly IRefIdProperty RunStandardIdProperty = P<RunStandardLog>.RegisterRefId(e => e.RunStandardId, ReferenceType.Parent);

		/// <summary>
		/// 设备运行定标Id
		/// </summary>
		public double RunStandardId
		{
			get { return (double)GetRefId(RunStandardIdProperty); }
			set { SetRefId(RunStandardIdProperty, value); }
		}

		/// <summary>
		/// 设备运行定标
		/// </summary>
		public static readonly RefEntityProperty<RunStandard> RunStandardProperty = P<RunStandardLog>.RegisterRef(e => e.RunStandard, RunStandardIdProperty);

		/// <summary>
		/// 设备运行定标
		/// </summary>
		public RunStandard RunStandard
		{
			get { return GetRefEntity(RunStandardProperty); }
			set { SetRefEntity(RunStandardProperty, value); }
		}
		#endregion
	}

	/// <summary>
	///  实体配置
	/// </summary>
	internal class RunStandardLogConfig : EntityConfig<RunStandardLog>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("RunStandardLog").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}