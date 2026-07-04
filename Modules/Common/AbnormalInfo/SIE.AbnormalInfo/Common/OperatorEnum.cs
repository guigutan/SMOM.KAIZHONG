using SIE; 
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.AbnormalInfo.Common
{
	/// <summary>
	/// 
	/// </summary>
	public enum Operator
	{
		/// <summary>
		/// 大于
		/// </summary>
		[Label("大于")]
		Greater = 1,
		/// <summary>
		/// 大于等于
		/// </summary>
		[Label("大于等于")]
		GreaterEqual = 2,
		/// <summary>
		/// 小于
		/// </summary>
		[Label("小于")]
		Less = 3,
		/// <summary>
		/// 小于等于
		/// </summary>
		[Label("小于等于")]
		LessEqual = 4,
		/// <summary>
		/// 等于
		/// </summary>
		[Label("等于")]
		Equal = 5,
		/// <summary>
		/// 介于..之间
		/// </summary>
		[Label("介于..之间")]
		between = 6,
	}
}