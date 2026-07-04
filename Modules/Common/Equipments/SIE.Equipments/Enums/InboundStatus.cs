using SIE; 
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Equipments.Enums
{
	/// <summary>
	/// 入库状态
	/// </summary>
	public enum InboundStatus
	{
		/// <summary>
		/// 待入库
		/// </summary>
		[Label("待入库")]
		ToBe = 10,
		/// <summary>
		/// 入库中
		/// </summary>
		[Label("入库中")]
		Doing = 20,
		/// <summary>
		/// 已入库
		/// </summary>
		[Label("已入库")]
		Done = 30,
	}
}