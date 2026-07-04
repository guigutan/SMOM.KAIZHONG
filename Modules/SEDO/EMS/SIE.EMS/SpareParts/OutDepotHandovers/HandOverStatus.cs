using SIE; 
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.SpareParts.OutDepotHandovers
{
	/// <summary>
	/// 备件出库交接状态
	/// </summary>
	public enum HandOverStatus
	{
		/// <summary>
		/// 待交接
		/// </summary>
		[Label("待交接")]
		Pending = 10,
		/// <summary>
		/// 部分交接
		/// </summary>
		[Label("部分交接")]
		Receiving = 20,
		/// <summary>
		/// 已交接
		/// </summary>
		[Label("已交接")]
		Received = 30,
	}
}