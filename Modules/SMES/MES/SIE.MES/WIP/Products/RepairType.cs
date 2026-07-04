using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.WIP.Products
{
	/// <summary>
	/// 维修类型
	/// </summary>
	public enum RepairType
	{
		/// <summary>
		/// 外观
		/// </summary>
		[Label("外观")]
		Exterior = 10,
		/// <summary>
		/// 功能
		/// </summary>
		[Label("功能")]
		Function = 20,
		/// <summary>
		/// BGA维修
		/// </summary>
		[Label("BGA维修")]
		BGA = 30,
	}
}
