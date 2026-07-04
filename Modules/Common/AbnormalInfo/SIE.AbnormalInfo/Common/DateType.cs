using SIE.ObjectModel;

namespace SIE.AbnormalInfo.Common
{
	/// <summary>
	/// 编辑类型
	/// </summary>
	public enum DateType
	{
		/// <summary>
		/// 当天
		/// </summary>
		[Label("当天")]
		Day = 1,
		/// <summary>
		/// 本周
		/// </summary>
		[Label("本周")]
		Week = 2,
		/// <summary>
		/// 本月
		/// </summary>
		[Label("本月")]
		Month = 3,
		/// <summary>
		/// 本年
		/// </summary>
		[Label("本年")]
		Year = 4,
	}
}