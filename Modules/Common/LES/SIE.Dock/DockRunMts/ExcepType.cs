using SIE; 
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Dock.DockRunMts
{
	/// <summary>
	/// 例外类型
	/// </summary>
	public enum ExcepType
	{
		/// <summary>
		/// 启用
		/// </summary>
		[Label("启用")]
		Enable,
		/// <summary>
		/// 停用
		/// </summary>
		[Label("停用")]
		Disable,
	}
}