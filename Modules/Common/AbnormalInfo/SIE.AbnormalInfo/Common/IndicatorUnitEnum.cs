using SIE; 
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.AbnormalInfo.Common
{
	/// <summary>
	/// 指标单位
	/// </summary>
	public enum IndicatorUnit
	{
		/// <summary>
		/// 数值
		/// </summary>
		[Label("数值")]
		Number = 1,
		/// <summary>
		/// 百分比(%)
		/// </summary>
		[Label("百分比(%)")]
		Scale = 2,

		/// <summary>
		/// 天
		/// </summary>
		[Label("天")]
		Day = 3,
		/// <summary>
		/// 时
		/// </summary>
		[Label("时")]
		Hour = 4,
		/// <summary>
		/// 分
		/// </summary>
		[Label("分")]
		minutes = 5
	}
}