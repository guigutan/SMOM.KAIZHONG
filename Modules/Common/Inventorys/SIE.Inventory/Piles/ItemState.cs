using SIE; 
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Inventory.Piles
{
	/// <summary>
	/// 物料状态
	/// </summary>
	public enum ItemState
	{
		/// <summary>
		/// 创建
		/// </summary>
		[Label("创建")]
		Create = 0,
		/// <summary>
		/// 在库
		/// </summary>
		[Label("在库")]
		InStore = 1,
		/// <summary>
		/// 出库
		/// </summary>
		[Label("出库")]
		OutStore = 2,
	}
}