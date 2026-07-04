using SIE; 
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.AbnormalInfo.Common
{
	/// <summary>
	/// 编辑类型
	/// </summary>
	public enum FieldProp
	{
		/// <summary>
		/// 文本
		/// </summary>
		[Label("文本")]
		Text = 1,
		/// <summary>
		/// 时间
		/// </summary>
		[Label("时间")]
		DateTime = 2,
		/// <summary>
		/// 数值
		/// </summary>
		[Label("数值")]
		Numerical = 3,
		/// <summary>
		/// 枚举
		/// </summary>
		[Label("枚举")]
		EnumField = 4,

		/// <summary>
		/// 时间枚举
		/// </summary>
		[Label("时间枚举")]
		EnumTime = 5,
	}
}