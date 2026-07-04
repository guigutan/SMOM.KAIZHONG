using SIE; 
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.AbnormalInfo.Common
{
	/// <summary>
	/// 逻辑运算符
	/// </summary>
	public enum LogicalOperator
	{
		/// <summary>
		/// 且
		/// </summary>
		[Label("且")]
		AND = 1,
		/// <summary>
		/// 或
		/// </summary>
		[Label("或")]
		OR = 2,
	}
}