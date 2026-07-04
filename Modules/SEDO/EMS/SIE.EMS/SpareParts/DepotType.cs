using SIE; 
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.SpareParts
{
	/// <summary>
	/// 类型
	/// </summary>
	public enum DepotType
	{
		/// <summary>
		/// 备件
		/// </summary>
		[Label("备件")]
		SparePart,
		/// <summary>
		/// 工治具
		/// </summary>
		[Label("工治具")]
		Work,
	}
}