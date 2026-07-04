using SIE; 
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.AbnormalInfo.Common
{
	/// <summary>
	/// 单位
	/// </summary>
	public enum UnitType
	{
		/// <summary>
		/// 分钟
		/// </summary>
		[Label("分钟")]
		Minute,
		/// <summary>
		/// 小时
		/// </summary>
		[Label("小时")]
		Hours,
		/// <summary>
		/// 天
		/// </summary>
		[Label("天")]
		Days,
	}
}