using SIE; 
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Equipments.Enums
{
	/// <summary>
	/// 保养状态
	/// </summary>
	public enum MaintainStatus
	{
		/// <summary>
		/// 待保养
		/// </summary>
		[Label("待保养")]
		ToDo = 10,
		/// <summary>
		/// 保养完成
		/// </summary>
		[Label("保养完成")]
		Done = 20,
	}
}