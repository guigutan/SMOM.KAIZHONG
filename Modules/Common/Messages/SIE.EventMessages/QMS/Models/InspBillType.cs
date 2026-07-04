using SIE.ObjectModel;

namespace SIE.EventMessages.QMS.Models
{
	/// <summary>
	/// 单据类别
	/// </summary>
	public enum InspBillType
	{
		/// <summary>
		/// 常规
		/// </summary>
		[Label("常规")]
		Normal,
		/// <summary>
		/// 复检
		/// </summary>
		[Label("复检")]
		Recheck,
		/// <summary>
		/// 加抽
		/// </summary>
		[Label("加抽")]
		Resample,
	}
}