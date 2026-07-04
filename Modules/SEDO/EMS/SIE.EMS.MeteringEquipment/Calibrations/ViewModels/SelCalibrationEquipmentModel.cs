using SIE.Core.Enums;
using SIE.Domain;
using SIE.EMS.MeteringEquipment.Calibrations.Criterias;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.MeteringEquipment.Calibrations.ViewModels
{
    /// <summary>
    /// 检验规程的检验项目的点检保养项目ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(SelCalibrationEquipmentModelCriteria))]
    [Label("计量设备台账")]
    public class SelCalibrationEquipmentModel : ViewModel
    {
		#region 台账id EquipAccountId
		/// <summary>
		/// 项目id
		/// </summary>
		[Label("项目id")]
		public static readonly Property<double> EquipAccountIdProperty = P<SelCalibrationEquipmentModel>.Register(e => e.EquipAccountId);

		/// <summary>
		/// 项目id
		/// </summary>
		public double EquipAccountId
		{
			get { return GetProperty(EquipAccountIdProperty); }
			set { SetProperty(EquipAccountIdProperty, value); }
		}
		#endregion

		#region 设备编码 Code
		/// <summary>
		/// 设备编码
		/// </summary>
		[Label("设备编码")]
		public static readonly Property<string> CodeProperty = P<SelCalibrationEquipmentModel>.Register(e => e.Code);

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
		public static readonly Property<string> NameProperty = P<SelCalibrationEquipmentModel>.Register(e => e.Name);

		/// <summary>
		/// 设备名称
		/// </summary>
		public string Name
		{
			get { return GetProperty(NameProperty); }
			set { SetProperty(NameProperty, value); }
		}
		#endregion


		#region 设备型号名称 EquipModelName
		/// <summary>
		/// 设备型号名称
		/// </summary>
		[Label("设备型号")]
		public static readonly Property<string> EquipModelNameProperty = P<SelCalibrationEquipmentModel>.Register(e => e.EquipModelName);

		/// <summary>
		/// 设备型号名称
		/// </summary>
		public string EquipModelName
		{
			get { return this.GetProperty(EquipModelNameProperty); }
			set { this.SetProperty(EquipModelNameProperty, value); }
		}
		#endregion


		#region 设备类别名称 EquipTypeName
		/// <summary>
		/// 设备类别名称
		/// </summary>
		[Label("设备类别")]
		public static readonly Property<string> EquipTypeNameProperty = P<SelCalibrationEquipmentModel>.Register(e => e.EquipTypeName);

		/// <summary>
		/// 设备类别名称
		/// </summary>
		public string EquipTypeName
		{
			get { return this.GetProperty(EquipTypeNameProperty); }
			set { this.SetProperty(EquipTypeNameProperty, value); }
		}
		#endregion

		#region 技术规格 Specifications
		/// <summary>
		/// 技术规格
		/// </summary>
		[Label("技术规格")]
		public static readonly Property<string> SpecificationsProperty = P<SelCalibrationEquipmentModel>.Register(e => e.Specifications);

		/// <summary>
		/// 技术规格
		/// </summary>
		public string Specifications
		{
			get { return this.GetProperty(SpecificationsProperty); }
			set { this.SetProperty(SpecificationsProperty, value); }
		}
		#endregion

		#region 使用部门 UseDepartmentName
		/// <summary>
		/// 使用部门
		/// </summary>
		[Label("使用部门")]
		public static readonly Property<string> UseDepartmentNameProperty = P<SelCalibrationEquipmentModel>.Register(e => e.UseDepartmentName);

		/// <summary>
		/// 使用部门
		/// </summary>
		public string UseDepartmentName
		{
			get { return this.GetProperty(UseDepartmentNameProperty); }
			set { this.SetProperty(UseDepartmentNameProperty, value); }
		}
		#endregion

		#region 生产厂家 Manufacturer
		/// <summary>
		/// 生产厂家
		/// </summary>
		[Label("生产厂家")]
		public static readonly Property<string> ManufacturerProperty
			= P<SelCalibrationEquipmentModel>.Register(e => e.Manufacturer);

		/// <summary>
		/// 生产厂家
		/// </summary>
		public string Manufacturer
		{
			get { return this.GetProperty(ManufacturerProperty); }
			set { this.SetProperty(ManufacturerProperty, value); }
		}
		#endregion

		#region 立卡日期 CardDate
		/// <summary>
		/// 立卡日期
		/// </summary>
		[Label("立卡日期")]
		public static readonly Property<DateTime?> CardDateProperty = P<SelCalibrationEquipmentModel>.Register(e => e.CardDate);

		/// <summary>
		/// 立卡日期
		/// </summary>
		public DateTime? CardDate
		{
			get { return this.GetProperty(CardDateProperty); }
			set { this.SetProperty(CardDateProperty, value); }
		}
		#endregion

		#region 管理状态 UseState 
		/// <summary>
		/// 管理状态
		/// </summary>
		[Label("管理状态")]
		public static readonly Property<AccountUseState> UseStateProperty = P<SelCalibrationEquipmentModel>.Register(e => e.UseState);

		/// <summary>
		/// 管理状态（原使用状态）
		/// </summary>
		public AccountUseState UseState
		{
			get { return GetProperty(UseStateProperty); }
			set { SetProperty(UseStateProperty, value); }
		}
		#endregion

		#region 是否降级 IsDowngrade
		/// <summary>
		/// 是否降级
		/// </summary>
		[Label("是否降级")]
		public static readonly Property<bool?> IsDowngradeProperty = P<SelCalibrationEquipmentModel>.Register(e => e.IsDowngrade);

		/// <summary>
		/// 是否降级
		/// </summary>
		public bool? IsDowngrade
		{
			get { return GetProperty(IsDowngradeProperty); }
			set { SetProperty(IsDowngradeProperty, value); }
		}
		#endregion

		#region 精度级别 PrecisionClass
		/// <summary>
		/// 精度级别
		/// </summary>
		[Label("精度级别")]
		public static readonly Property<string> PrecisionClassProperty = P<SelCalibrationEquipmentModel>.Register(e => e.PrecisionClass);

		/// <summary>
		/// 精度级别
		/// </summary>
		public string PrecisionClass
		{
			get { return GetProperty(PrecisionClassProperty); }
			set { SetProperty(PrecisionClassProperty, value); }
		}
		#endregion
	}
}
