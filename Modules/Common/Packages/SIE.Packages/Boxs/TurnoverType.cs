using SIE; 
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Packages.Boxs
{
	/// <summary>
	/// 周转操作类型
	/// </summary>
	public enum TurnoverType
	{
		/// <summary>
		/// 回收
		/// </summary>
		[Label("回收")]
		Recycle = 5,
		/// <summary>
		/// 维修
		/// </summary>
		[Label("维修")]
		Repair = 10,
		/// <summary>
		/// 报废
		/// </summary>
		[Label("报废")]
		Scrap = 15,
		/// <summary>
		/// 绑定
		/// </summary>
		[Label("绑定")]
		Binding = 20,
		/// <summary>
		/// 解绑
		/// </summary>
		[Label("解绑")]
		UnBinding = 25,
		/// <summary>
		/// 入库
		/// </summary>
		[Label("入库")]
		InStorage = 30,
		/// <summary>
		/// 出库
		/// </summary>
		[Label("出库")]
		OutStorage = 35,
		/// <summary>
		/// 清洗
		/// </summary>
		[Label("清洗")]
		Cleaning = 40,
	}
}