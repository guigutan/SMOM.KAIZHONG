using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.AbnormalInfo.Common
{
	/// <summary>
	/// 异常任务类型
	/// </summary>
	public enum TaskType
	{
		/// <summary>
		/// 自动
		/// </summary>
		[Label("自动")]
		Auto,
		/// <summary>
		/// 手工
		/// </summary>
		[Label("手工")]
		Manual,
	}
}
