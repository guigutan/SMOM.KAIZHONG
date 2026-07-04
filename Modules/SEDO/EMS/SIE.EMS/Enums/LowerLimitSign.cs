using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Enums
{
	/// <summary>
	/// 下限符号
	/// </summary>
	public enum LowerLimitSign
	{
		/// <summary>
		/// 大于
		/// </summary>
		[Label("＞")]
		Greater = 1,
		/// <summary>
		/// 大于等于
		/// </summary>
		[Label("≥")]
		GreaterOrEqual = 2
	}
}
