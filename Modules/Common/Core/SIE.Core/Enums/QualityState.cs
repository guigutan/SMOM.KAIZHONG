using SIE.ObjectModel;

namespace SIE.Core.Enums
{
	/// <summary>
	/// 仪器状态
	/// </summary>
	public enum QualityState
	{
		/// <summary>
		/// 可用
		/// </summary>
		[Label("可用")]
		Enable,

		/// <summary>
		/// 禁用
		/// </summary>
		[Label("禁用")]
		Disable,

		/// <summary>
		/// 校验中
		/// </summary>
		[Label("校验中")]
		Calibrating,

		/// <summary>
		/// 维修中
		/// </summary>
		[Label("维修中")]
		Repairing,
		
		/// <summary>
		/// 报废
		/// </summary>
		[Label("报废")]
		Scrap,
	}
}