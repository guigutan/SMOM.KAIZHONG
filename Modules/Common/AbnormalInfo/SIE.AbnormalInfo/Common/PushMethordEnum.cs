using SIE; 
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.AbnormalInfo.Common
{
	/// <summary>
	/// 推送方式
	/// </summary>
	public enum PushMethordEnum
	{
		/// <summary>
		/// 发起PDCA改善
		/// </summary>
		[Label("发起PDCA改善")]
		PDCA = 1,
		/// <summary>
		/// 8D
		/// </summary>
		[Label("8D")]
		EightD = 2,
	}
}