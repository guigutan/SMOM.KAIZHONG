using SIE; 
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.ComponentModel;

namespace SIE.AbnormalInfo.Common
{
	/// <summary>
	/// 异常任务状态
	/// </summary>
	public enum TaskStateEnum
	{
		/// <summary>
		/// 未开始
		/// </summary>
		[Label("未开始")]
		[Category("PushUpgradeRuleNode")]
		ToDo = 0,
		/// <summary>
		/// 进行中
		/// </summary>
		[Label("进行中")]
		[Category("PushUpgradeRuleNode")]
		Doing = 1,
		/// <summary>
		/// 完成
		/// </summary>
		[Label("完成")]
		[Category("PushUpgradeRuleNode")]
		Done = 2,

		/// <summary>
		/// 取消
		/// </summary>
		[Label("取消")]
		[Category("PushUpgradeRuleNode")]
		Cancel = 3,
	}
}