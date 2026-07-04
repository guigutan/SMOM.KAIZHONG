using SIE.ObjectModel;

namespace SIE.Equipments.Enums
{
	/// <summary>
	/// 产品生产模式
	/// </summary>
	public enum ProductionMode
	{
		/// <summary>
		/// 单一产品模式
		/// </summary>
		[Label("单一产品模式")]
		SingleMode,

		/// <summary>
		/// 多产品混合模式
		/// </summary>
		[Label("多产品混合模式")]
		MixedMode,
	}
}
