using SIE.Domain; 
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Enums
{
	/// <summary>
	/// 闲置与封存业务类型
	/// </summary>
	public enum IdleArchiveType
	{
		/// <summary>
		/// 闲置
		/// </summary>
		[Label("闲置")]
		Idle = 10,
		/// <summary>
		/// 封存
		/// </summary>
		[Label("封存")]
		Archive = 20,
		/// <summary>
		/// 闲置启用
		/// </summary>
		[Label("闲置启用")]
		IdleEnabled = 30,
		/// <summary>
		/// 封存启用
		/// </summary>
		[Label("封存启用")]
		ArchiveEnabled = 40,
	}
}