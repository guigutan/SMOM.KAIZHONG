using SIE.Domain;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.MeteringEquipment.Calibrations.Criterias
{

    /// <summary>
    /// 选择设备清单
    /// </summary>
    [QueryEntity, Serializable]
	[Label("选择设备清单")]
	public class SelCalibrationEquipmentModelCriteria: Criteria
	{
		#region 检验规程 InspectionRuleId 
		/// <summary>
		/// 检验规程Id
		/// </summary>
		[Label("检验规程Id")]
		public static readonly Property<double?> InspectionRuleIdProperty = P<SelCalibrationEquipmentModelCriteria>.Register(e => e.InspectionRuleId);

		/// <summary>
		/// 检验规程Id
		/// </summary>
		public double? InspectionRuleId
		{
			get { return GetProperty(InspectionRuleIdProperty); }
			set { SetProperty(InspectionRuleIdProperty, value); }
		}
		#endregion

		#region 设备编码 Code
		/// <summary>
		/// 设备编码
		/// </summary>
		[Label("设备编码")]
		public static readonly Property<string> CodeProperty = P<SelCalibrationEquipmentModelCriteria>.Register(e => e.Code);

		/// <summary>
		/// 设备编码
		/// </summary>
		public string Code
		{
			get { return GetProperty(CodeProperty); }
			set { SetProperty(CodeProperty, value); }
		}
		#endregion

		#region 设备名称 Name
		/// <summary>
		/// 设备名称
		/// </summary>        
		[Label("设备名称")]
		public static readonly Property<string> NameProperty = P<SelCalibrationEquipmentModelCriteria>.Register(e => e.Name);

		/// <summary>
		/// 设备名称
		/// </summary>
		public string Name
		{
			get { return GetProperty(NameProperty); }
			set { SetProperty(NameProperty, value); }
		}
		#endregion

		#region 设备型号维护 EquipModel
		/// <summary>
		/// 设备型号维护Id
		/// </summary>
		public static readonly IRefIdProperty EquipModelIdProperty = P<SelCalibrationEquipmentModelCriteria>.RegisterRefId(e => e.EquipModelId, ReferenceType.Normal);

		/// <summary>
		/// 设备型号维护Id
		/// </summary>
		public double? EquipModelId
		{
			get { return (double?)GetRefNullableId(EquipModelIdProperty); }
			set { SetRefNullableId(EquipModelIdProperty, value); }
		}

		/// <summary>
		/// 设备型号维护
		/// </summary>
		public static readonly RefEntityProperty<EquipModel> EquipModelProperty = P<SelCalibrationEquipmentModelCriteria>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

		/// <summary>
		/// 设备型号维护
		/// </summary>
		public EquipModel EquipModel
		{
			get { return GetRefEntity(EquipModelProperty); }
			set { SetRefEntity(EquipModelProperty, value); }
		}
		#endregion

		#region 设备类型 EquipType
		/// <summary>
		/// 设备类型Id
		/// </summary>
		[Label("设备类型")]
		public static readonly IRefIdProperty EquipTypeIdProperty = P<SelCalibrationEquipmentModelCriteria>.RegisterRefId(e => e.EquipTypeId, ReferenceType.Normal);

		/// <summary>
		/// 设备类型Id
		/// </summary>
		public double? EquipTypeId
		{
			get { return (double?)GetRefNullableId(EquipTypeIdProperty); }
			set { SetRefNullableId(EquipTypeIdProperty, value); }
		}

		/// <summary>
		/// 设备类型
		/// </summary>
		public static readonly RefEntityProperty<EquipType> EquipTypeProperty = P<SelCalibrationEquipmentModelCriteria>.RegisterRef(e => e.EquipType, EquipTypeIdProperty);

		/// <summary>
		/// 设备类型
		/// </summary>
		public EquipType EquipType
		{
			get { return GetRefEntity(EquipTypeProperty); }
			set { SetRefEntity(EquipTypeProperty, value); }
		}
		#endregion

		/// <summary>
		/// 获取计量类型的设备台账列表
		/// </summary>
		protected override EntityList Fetch()
		{
			return RT.Service.Resolve<CalibrationController>().GetMeteringEquipmentAccountList(this);
		}
	}
}
