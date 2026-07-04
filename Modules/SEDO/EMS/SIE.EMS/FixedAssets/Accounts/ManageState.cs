using SIE; 
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.FixedAssets.Accounts
{
	/// <summary>
	/// 管理状态
	/// </summary>
	public enum ManageState
	{
		/// <summary>
		/// 闲置
		/// </summary>
		[Label("闲置")]
		InIdle = 0,
		/// <summary>
		/// 使用中
		/// </summary>
		[Label("使用中")]
		Using = 5,
		/// <summary>
		/// 封存
		/// </summary>
		[Label("封存")]
		Archive = 10,
		/// <summary>
		/// 租借
		/// </summary>
		[Label("租借")]
		Lease = 15,
		/// <summary>
		/// 报废
		/// </summary>
		[Label("报废")]
		Scrap = 20,
	}
}