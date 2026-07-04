using SIE;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.RedCardManagment.RedCardApplyBills
{
	/// <summary>
	/// 状态
	/// </summary>
	public enum BillStatus
	{

		/// <summary>
		/// 待发起
		/// </summary>
		[Label("待发起")]
		ToDo = 0,
		/// <summary>
		/// 已发起
		/// </summary>
		[Label("已发起")]
		Doing = 1,
		/// <summary>
		/// 完成
		/// </summary>
		[Label("完成")]
		Done = 2,
		/// <summary>
		/// 取消
		/// </summary>
		[Label("取消")]
		Cancel = 3,

		/// <summary>
		/// 驳回
		/// </summary>
		[Label("驳回")]
		Reject = 4,
	}
}