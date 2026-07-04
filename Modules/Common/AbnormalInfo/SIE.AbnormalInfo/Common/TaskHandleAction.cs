using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.AbnormalInfo.Common
{
	/// <summary>
	/// 异常任务类型
	/// </summary>
	public enum TaskHandleAction
	{
		/// <summary>
		/// 消息
		/// </summary>
		[Label("消息")]
		Message,
		/// <summary>
		/// 动作
		/// </summary>
		[Label("动作")]
		Action,
	}
}
