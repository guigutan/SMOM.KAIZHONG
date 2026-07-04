using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;


namespace SIE.Kit.APS.EngineerPlan.Settings
{
	/// <summary>
	/// 等级日排产上限
	/// </summary>
	[RootEntity, Serializable]
	[CriteriaQuery]
	[Label("等级日排产上限")]
	[DisplayMember(nameof(LevelName))]
	public partial class CustLevel : DataEntity
	{
		#region 等级 LevelName
		/// <summary>
		/// 等级
		/// </summary>
		[Label("等级")]
		public static readonly Property<string> LevelNameProperty = P<CustLevel>.Register(e => e.LevelName);

		/// <summary>
		/// 等级
		/// </summary>
		public string LevelName
		{
			get { return GetProperty(LevelNameProperty); }
			set { SetProperty(LevelNameProperty, value); }
		}
		#endregion

		#region 时效性 Hour
		/// <summary>
		/// 时效性(H)
		/// </summary>
		[Label("时效性(H)")]
		public static readonly Property<int> HourProperty = P<CustLevel>.Register(e => e.Hour);

		/// <summary>
		///  时效性(H)
		/// </summary>
		public int Hour
		{
			get { return GetProperty(HourProperty); }
			set { SetProperty(HourProperty, value); }
		}
		#endregion

		#region 日排产上限 DayWorkCapacity
		/// <summary>
		/// 日排产上限
		/// </summary>
		[Label("日排产上限")]
		public static readonly Property<int> DayWorkCapacityProperty = P<CustLevel>.Register(e => e.DayWorkCapacity);

		/// <summary>
		///  日排产上限
		/// </summary>
		public int DayWorkCapacity
		{
			get { return GetProperty(DayWorkCapacityProperty); }
			set { SetProperty(DayWorkCapacityProperty, value); }
		}
        #endregion

        #region 运算使用
		/// <summary>
		/// 日排产上限
		/// </summary>
		public decimal LessWorkCapaty { get; set; }
        #endregion
    }

    /// <summary>
    ///  实体配置
    /// </summary>
    internal class CustLevelConfig : EntityConfig<CustLevel>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("APS_MSO_MI_PLAN_CL").MapAllProperties();
			Meta.EnableSort();
			Meta.EnablePhantoms();
		}
	}
}
