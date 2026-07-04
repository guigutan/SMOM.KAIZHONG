using SIE.ObjectModel;

namespace SIE.Equipments.Enums
{
	/// <summary>
	/// 添加方式
	/// </summary>
	public enum ChemicalAddWay
	{
		/// <summary>
		/// 手工添加
		/// </summary>
		[Label("手工添加")]
		Manual,

		/// <summary>
		/// 自动添加
		/// </summary>
		[Label("自动添加")]
		Auto,
	}
}