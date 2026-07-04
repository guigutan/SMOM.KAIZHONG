using SIE.DataAuth;
using SIE.Domain;
using SIE.Kit.APS.EngineerPlans.Settings;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System;

namespace SIE.Kit.APS.EngineerPlan.Settings
{
    /// <summary>
    /// 工程节假日维护
    /// </summary>
    [RootEntity, Serializable]
	[ConditionQueryType(typeof(HolidaySettingCriteria))]
	[EntityDataAuthAttribute(typeof(EmployeeEnterprise), nameof(FactoryId), true)]
	[Label("工程节假日维护")]
	//[DisplayMember(nameof(Code))]
	public partial class HolidaySetting : DataEntity
	{
		#region 工厂 Factory
		/// <summary>
		/// 工厂Id
		/// </summary>
		[Label("工厂")]
		public static readonly IRefIdProperty FactoryIdProperty = P<HolidaySetting>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

		/// <summary>
		/// 工厂Id 
		/// </summary>
		public double? FactoryId
		{
			get { return (double?)GetRefNullableId(FactoryIdProperty); }
			set { SetRefNullableId(FactoryIdProperty, value); }
		}

		/// <summary>
		/// 工厂
		/// </summary>
		public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<HolidaySetting>.RegisterRef(e => e.Factory, FactoryIdProperty);

		/// <summary>
		/// 工厂
		/// </summary>
		public Enterprise Factory
		{
			get { return GetRefEntity(FactoryProperty); }
			set { SetRefEntity(FactoryProperty, value); }
		}
		#endregion

		#region 开始日期 StartDate
		/// <summary>
		/// 开始日期
		/// </summary>
		[Label("开始日期")]
		public static readonly Property<DateTime> StartDateProperty = P<HolidaySetting>.Register(e => e.StartDate);

		/// <summary>
		/// 开始日期
		/// </summary>
		public DateTime StartDate
		{
			get { return GetProperty(StartDateProperty); }
			set { SetProperty(StartDateProperty, value); }
		}
		#endregion

		#region 结束日期 EndDate
		/// <summary>
		/// 结束日期
		/// </summary>
		[Label("结束日期")]
		public static readonly Property<DateTime> EndDateProperty = P<HolidaySetting>.Register(e => e.EndDate);

		/// <summary>
		/// 结束日期
		/// </summary>
		public DateTime EndDate
		{
			get { return GetProperty(EndDateProperty); }
			set { SetProperty(EndDateProperty, value); }
		}
		#endregion

		#region 备注 Remerk
		/// <summary>
		/// 备注
		/// </summary>
		[Label("备注")]
		public static readonly Property<string> RemerkProperty = P<HolidaySetting>.Register(e => e.Remerk);

		/// <summary>
		/// 备注
		/// </summary>
		public string Remerk
		{
			get { return GetProperty(RemerkProperty); }
			set { SetProperty(RemerkProperty, value); }
		}
		#endregion
	}


	/// <summary>
	///  实体配置
	/// </summary>
	internal class HolidaySettingConfig : EntityConfig<HolidaySetting>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("APS_MSO_MI_PLAN_HDSet").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}

}
