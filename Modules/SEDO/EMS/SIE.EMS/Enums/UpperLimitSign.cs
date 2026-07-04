using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Enums
{
	/// <summary>
	/// 上限符号
	/// </summary>
	public enum UpperLimitSign
	{
		/// <summary>
		/// 小于
		/// </summary>
		[Label("＜")]
		Less = 1,
		/// <summary>
		/// 小于等于
		/// </summary>
		[Label("≤")]
		LessOrEqual = 2
	}
}
