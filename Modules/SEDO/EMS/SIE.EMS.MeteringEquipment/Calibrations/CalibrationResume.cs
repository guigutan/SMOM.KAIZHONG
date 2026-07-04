using SIE;
using SIE.Common;
using SIE.Domain;
using SIE.EMS.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.EMS.MeteringEquipment.Calibrations
{
	/// <summary>
	/// 计量设备定检操作记录
	/// </summary>
	[ChildEntity, Serializable]
	//[CriteriaQuery]
	[Label("操作记录")]
	public partial class CalibrationResume : DataEntity
	{

		#region 检验结果 InspectionResult
		/// <summary>
		/// 检验结果
		/// </summary>
		[Label("检验结果")]
		public static readonly Property<InspectionResult?> InspectionResultProperty = P<CalibrationResume>.Register(e => e.InspectionResult);

		/// <summary>
		/// 检验结果
		/// </summary>
		public InspectionResult? InspectionResult
		{
			get { return GetProperty(InspectionResultProperty); }
			set { SetProperty(InspectionResultProperty, value); }
		}
		#endregion

		#region 操作类型 OperationType
		/// <summary>
		/// 操作类型
		/// </summary>
		[Label("操作类型")]
		public static readonly Property<OperationType> OperationTypeProperty = P<CalibrationResume>.Register(e => e.OperationType);

		/// <summary>
		/// 操作类型
		/// </summary>
		public OperationType OperationType
		{
			get { return GetProperty(OperationTypeProperty); }
			set { SetProperty(OperationTypeProperty, value); }
		}
		#endregion

		#region 操作时间 OperationDateTime
		/// <summary>
		/// 操作时间
		/// </summary>
		[Label("操作时间")]
		public static readonly Property<DateTime> OperationDateTimeProperty = P<CalibrationResume>.Register(e => e.OperationDateTime);

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
		public static readonly IRefIdProperty OperatorIdProperty = P<CalibrationResume>.RegisterRefId(e => e.OperatorId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<Employee> OperatorProperty = P<CalibrationResume>.RegisterRef(e => e.Operator, OperatorIdProperty);

		/// <summary>
		/// 操作人
		/// </summary>
		public Employee Operator
		{
			get { return GetRefEntity(OperatorProperty); }
			set { SetRefEntity(OperatorProperty, value); }
		}
		#endregion

		#region 计量设备定检 Calibration
		/// <summary>
		/// 计量设备定检Id
		/// </summary>
		public static readonly IRefIdProperty CalibrationIdProperty = P<CalibrationResume>.RegisterRefId(e => e.CalibrationId, ReferenceType.Parent);

		/// <summary>
		/// 计量设备定检Id
		/// </summary>
		public double CalibrationId
		{
			get { return (double)GetRefId(CalibrationIdProperty); }
			set { SetRefId(CalibrationIdProperty, value); }
		}

		/// <summary>
		/// 计量设备定检
		/// </summary>
		public static readonly RefEntityProperty<Calibration> CalibrationProperty = P<CalibrationResume>.RegisterRef(e => e.Calibration, CalibrationIdProperty);

		/// <summary>
		/// 计量设备定检
		/// </summary>
		public Calibration Calibration
		{
			get { return GetRefEntity(CalibrationProperty); }
			set { SetRefEntity(CalibrationProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// 计量设备定检操作记录 实体配置
	/// </summary>
	internal class CalibrationResumeConfig : EntityConfig<CalibrationResume>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_CAL_RESU").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}