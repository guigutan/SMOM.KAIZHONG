using SIE.ObjectModel;

namespace SIE.EMS.SpareParts.Enums
{
    /// <summary>
    /// 库存情况
    /// </summary>
    public enum StorageState
	{
		/// <summary>
		/// 高于安全库存(≥)
		/// </summary>
		[Label("高于安全库存(≥)")]
		HigherStorage = 5,
		/// <summary>
		/// 低于安全库存
		/// </summary>
		[Label("低于安全库存")]
		LowerStorage = 10,
	}
}
