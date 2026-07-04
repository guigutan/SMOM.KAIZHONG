using SIE.Domain; 
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Enums
{
	/// <summary>
	/// 定标类型
	/// </summary>
	public enum StandardType
	{
		/// <summary>
		/// 运行公里
		/// </summary>
		[Label("运行公里")]
		Km = 10,
		/// <summary>
		/// 运行时间
		/// </summary>
		[Label("运行时间")]
		Time = 20,
		/// <summary>
		/// 使用次数
		/// </summary>
		[Label("使用次数")]
		Count = 30,
		/// <summary>
		/// 时间周期
		/// </summary>
		[Label("时间周期")]
		Period = 40,
	}
}