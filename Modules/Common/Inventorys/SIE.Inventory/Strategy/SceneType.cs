using SIE; 
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Inventory.Strategy
{
	/// <summary>
	/// 应用场景
	/// </summary>
	public enum SceneType
	{
		/// <summary>
		/// 非立库
		/// </summary>
		[Label("非立库")]
		NotASRS = 1,

		/// <summary>
		/// 立库
		/// </summary>
		[Label("立库")]
		ASRS = 2,
	}
}