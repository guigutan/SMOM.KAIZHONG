using SIE.Domain; 
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Enums
{
	/// <summary>
	/// 盘点资产来源
	/// </summary>
	public enum InventoryAssetSource
	{
		/// <summary>
		/// 账内资产
		/// </summary>
		[Label("账内资产")]
		Account = 10,
		/// <summary>
		/// 盘盈新增
		/// </summary>
		[Label("盘盈新增")]
		Profit = 20,
	}
}