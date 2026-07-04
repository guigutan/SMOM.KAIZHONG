using SIE; 
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Enums
{
	/// <summary>
	/// 润滑方式
	/// </summary>
	public enum LubricatingType
	{
		/// <summary>
		/// 换油
		/// </summary>
		[Label("换油")]
		Replace = 10,
		/// <summary>
		/// 加油
		/// </summary>
		[Label("加油")]
		Add = 20,
	}
}