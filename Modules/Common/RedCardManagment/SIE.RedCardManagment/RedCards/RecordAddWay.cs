using SIE; 
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.RedCardManagment.RedCards
{
	/// <summary>
	/// 添加方式
	/// </summary>
	public enum RecordAddWay
	{
		/// <summary>
		/// 自动
		/// </summary>
		[Label("自动")]
		Auto = 1,
		/// <summary>
		/// 手动
		/// </summary>
		[Label("手动")]
		Manual = 2,
	}
}