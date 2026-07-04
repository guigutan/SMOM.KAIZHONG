using SIE; 
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.SpareParts
{
	/// <summary>
	/// 入库类型
	/// </summary>
	public enum StoreType
	{
		/// <summary>
		/// 采购入库
		/// </summary>
		[Label("采购入库")]
		StockStore,
		/// <summary>
		/// 正常退库
		/// </summary>
		[Label("正常退库")]
		NormalQuit,
		/// <summary>
		/// 不良退库
		/// </summary>
		[Label("不良退库")]
		BadQuit,
		/// <summary>
		/// 外修返回
		/// </summary>
		[Label("外修返回")]
		RepairReturn,
	}
}