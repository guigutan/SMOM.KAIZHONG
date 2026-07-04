using SIE; 
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.AbnormalInfo.Common
{
	/// <summary>
	/// 推送对象
	/// </summary>
	public enum PushTargetEnum
	{
		/// <summary>
		/// 员工
		/// </summary>
		[Label("员工")]
		Staff = 1,
		/// <summary>
		/// 角色
		/// </summary>
		[Label("角色")]
		Role = 2,
		/// <summary>
		/// 用户组
		/// </summary>
		[Label("用户组")]
		UserGroup = 3,
		/// <summary>
		/// 部门
		/// </summary>
		[Label("部门")]
		Department = 4,
		/// <summary>
		/// 自定义
		/// </summary>
		[Label("自定义")]
		Custom = 5,
	}
}