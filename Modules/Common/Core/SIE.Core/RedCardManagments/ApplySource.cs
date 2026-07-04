using SIE; 
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Core.RedCardManagments
{
	/// <summary>
	/// 申请来源
	/// </summary>
	public enum ApplySource
	{
		/// <summary>
		/// 手工申请
		/// </summary>
		[Label("手工申请")]
		Manual = 0,
		/// <summary>
		/// 不合格审核
		/// </summary>
		[Label("不合格审核")]
		DisqualificationAudit = 1,
	}
}