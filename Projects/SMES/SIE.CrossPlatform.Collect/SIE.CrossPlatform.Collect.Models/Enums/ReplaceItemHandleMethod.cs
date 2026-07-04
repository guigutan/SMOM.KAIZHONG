using SIE.CrossPlatform.Collect.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.CrossPlatform.Collect.Models.Enums
{
	/// <summary>
	/// 置换后处理方式
	/// </summary>
	public enum ReplaceItemHandleMethod
	{
		/// <summary>
		/// 置换后作废
		/// </summary>
		[Label("置换后作废")]
		Scrap = 10,

		/// <summary>
		/// 置换后不良下料
		/// </summary>
		[Label("置换后不良下料")]
		Recycle = 20,
	}
}
