using SIE.ObjectModel;

namespace SIE.Equipments.Enums
{
	/// <summary>
	/// 老化方式
	/// </summary>
	public enum  AgingMode
    {
		/// <summary>
		/// 按卡位
		/// </summary>
		[Label("按卡位")]
		CardPosition,

		/// <summary>
		/// 按设备
		/// </summary>
		[Label("按设备")]
		Equipment,
    }
}