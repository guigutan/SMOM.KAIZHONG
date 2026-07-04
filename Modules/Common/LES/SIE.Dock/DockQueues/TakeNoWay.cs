using SIE; 
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Dock.DockQueues
{
	/// <summary>
	/// 取号方式
	/// </summary>
	public enum TakeNoWay
	{
		/// <summary>
		/// 现场取号
		/// </summary>
		[Label("现场取号")]
		Scene,
		/// <summary>
		/// 预约取号
		/// </summary>
		[Label("预约取号")]
		Appoint,
	}
}