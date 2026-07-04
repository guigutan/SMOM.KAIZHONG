using SIE; 
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.RedCardManagment.RedCards
{
	/// <summary>
	/// 红牌状态
	/// </summary>
	public enum RedCardState
	{
		/// <summary>
		/// 禁用
		/// </summary>
		[Label("禁用")]
		Disable = 0,
		/// <summary>
		/// 启用
		/// </summary>
		[Label("启用")]
		Enable = 1,
        /// <summary>
        /// 部分启用
        /// </summary>
        [Label("部分启用")]
        PartialEnable = 2,
	}
}