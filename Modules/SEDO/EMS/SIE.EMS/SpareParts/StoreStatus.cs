using SIE; 
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.SpareParts
{
	/// <summary>
	/// 入库状态
	/// </summary>
	public enum StoreStatus
	{
		/// <summary>
		/// 待入库
		/// </summary>
		[Label("待入库")]
		WaitStore,
		/// <summary>
		/// 已入库
		/// </summary>
		[Label("已入库")]
		EndStore,
	}
}