using SIE; 
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.SpareParts.Enums
{
	/// <summary>
	/// 管控方式
	/// </summary>
	public enum ControlMethod
	{
		/// <summary>
		/// 物料编码
		/// </summary>
		[Label("物料编码")]
		ItemCode = 10,
		/// <summary>
		/// 批次
		/// </summary>
		[Label("批次")]
		Batch = 20,
		/// <summary>
		/// 序列号
		/// </summary>
		[Label("序列号")]
		Sn = 30,
	}
}