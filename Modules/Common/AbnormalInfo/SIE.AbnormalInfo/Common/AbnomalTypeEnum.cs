using SIE; 
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.AbnormalInfo.Common
{
	/// <summary>
	/// 
	/// </summary>
	public enum AbnomalType
	{
		/// <summary>
		/// 指标类
		/// </summary>
		[Label("指标类")]
		Quota = 1,
		/// <summary>
		/// 时效类
		/// </summary>
		[Label("时效类")]
		Timeness = 2,
	}
}