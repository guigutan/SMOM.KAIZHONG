using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Enums
{
	/// <summary>
	/// 审核结果
	/// </summary>
	public enum AuditResult
	{
		/// <summary>
		/// 提交
		/// </summary>
		[Label("提交")]
		Submit = 10,

		/// <summary>
		/// 撤回
		/// </summary>
		[Label("撤回")]
		Cancel = 20,

		/// <summary>
		/// 通过
		/// </summary>
		[Label("通过")]
		Pass = 30,

		/// <summary>
		/// 驳回
		/// </summary>
		[Label("驳回")]
		Reject = 40,
	}
}
