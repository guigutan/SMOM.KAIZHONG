using SIE; 
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.AbnormalInfo.Common
{
	/// <summary>
	/// 指标取值
	/// </summary>
	public enum IndicatorValue
	{
		/// <summary>
		/// 实际值
		/// </summary>
		[Label("实际值")]
		Actual = 1,
		/// <summary>
		/// 层别值
		/// </summary>
		[Label("层别值")]
		LayerVal = 2,
		/// <summary>
		/// 统计数量
		/// </summary>
		[Label("统计数量")]
		CountAmount = 3,
	}
}