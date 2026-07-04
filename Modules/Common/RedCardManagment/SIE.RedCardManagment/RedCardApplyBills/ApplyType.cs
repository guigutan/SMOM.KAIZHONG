using SIE; 
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.RedCardManagment.RedCardApplyBills
{
	/// <summary>
	/// 申请类型
	/// </summary>
	public enum ApplyType
	{
		/// <summary>
		/// 手工创建
		/// </summary>
		[Label("手工创建")]
		Manual = 0,
		/// <summary>
		/// 自动创建
		/// </summary>
		[Label("自动创建")]
		Auto = 1,
	}
}