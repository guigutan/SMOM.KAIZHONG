using SIE.Domain; 
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Enums
{
	/// <summary>
	/// 
	/// </summary>
	public enum StandardUnit
	{
		/// <summary>
		/// 公里
		/// </summary>
		[Label("公里")]
		Km = 10,
		/// <summary>
		/// 小时
		/// </summary>
		[Label("小时")]
		Hour = 20,
		/// <summary>
		/// 次数
		/// </summary>
		[Label("次数")]
		Times = 30,
		/// <summary>
		/// 日
		/// </summary>
		[Label("日")]
		Day = 40,
	}
}