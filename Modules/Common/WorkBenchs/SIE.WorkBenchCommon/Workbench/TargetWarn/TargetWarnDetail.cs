using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.WorkBenchCommon.Workbench.TargetWarn
{
    /// <summary>
    /// 达成率区间
    /// </summary>
    [ChildEntity, Serializable]
	[Label("达成率区间")]
	public partial class TargetWarnDetail : DataEntity
	{
		#region 最小值 MinValue
		/// <summary>
		/// 最小值
		/// </summary>
		[Label("最小值")]
		public static readonly Property<decimal?> MinValueProperty = P<TargetWarnDetail>.Register(e => e.MinValue);

		/// <summary>
		/// 最小值
		/// </summary>
		public decimal? MinValue
		{
			get { return GetProperty(MinValueProperty); }
			set { SetProperty(MinValueProperty, value); }
		}
		#endregion

		#region 最大值 MaxValue
		/// <summary>
		/// 最大值
		/// </summary>
		[Label("最大值")]
		public static readonly Property<decimal?> MaxValueProperty = P<TargetWarnDetail>.Register(e => e.MaxValue);

		/// <summary>
		/// 最大值
		/// </summary>
		public decimal? MaxValue
		{
			get { return GetProperty(MaxValueProperty); }
			set { SetProperty(MaxValueProperty, value); }
		}
		#endregion

		#region 目标条件 TargetOpetators
		/// <summary>
		/// 目标条件
		/// </summary>
		[Label("目标条件")]
		public static readonly Property<TargetOpetators> TargetOpetatorsProperty = P<TargetWarnDetail>.Register(e => e.TargetOpetators);

		/// <summary>
		/// 目标条件
		/// </summary>
		public TargetOpetators TargetOpetators
		{
			get { return GetProperty(TargetOpetatorsProperty); }
			set { SetProperty(TargetOpetatorsProperty, value); }
		}
		#endregion

		#region 目标颜色 TargetColor
		/// <summary>
		/// 目标颜色
		/// </summary>
		[Label("目标颜色")]
		public static readonly Property<TargetColor> TargetColorProperty = P<TargetWarnDetail>.Register(e => e.TargetColor);

		/// <summary>
		/// 目标颜色
		/// </summary>
		public TargetColor TargetColor
		{
			get { return GetProperty(TargetColorProperty); }
			set { SetProperty(TargetColorProperty, value); }
		}
		#endregion

		#region 预警设定 TargetWarnSetting
		/// <summary>
		/// 预警设定Id
		/// </summary>
		public static readonly IRefIdProperty TargetWarnSettingIdProperty = P<TargetWarnDetail>.RegisterRefId(e => e.TargetWarnSettingId, ReferenceType.Parent);

		/// <summary>
		/// 预警设定Id
		/// </summary>
		public double TargetWarnSettingId
		{
			get { return (double)GetRefId(TargetWarnSettingIdProperty); }
			set { SetRefId(TargetWarnSettingIdProperty, value); }
		}

		/// <summary>
		/// 预警设定
		/// </summary>
		public static readonly RefEntityProperty<TargetWarnSetting> TargetWarnSettingProperty = P<TargetWarnDetail>.RegisterRef(e => e.TargetWarnSetting, TargetWarnSettingIdProperty);

		/// <summary>
		/// 预警设定
		/// </summary>
		public TargetWarnSetting TargetWarnSetting
		{
			get { return GetRefEntity(TargetWarnSettingProperty); }
			set { SetRefEntity(TargetWarnSettingProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// 达成率区间 实体配置
	/// </summary>
	internal class TargetWarnDetailConfig : EntityConfig<TargetWarnDetail>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("TARGET_WARN_DTL").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}