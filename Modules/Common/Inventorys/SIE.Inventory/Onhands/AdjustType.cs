using SIE.ObjectModel;

namespace SIE.Inventory.Onhands
{
    /// <summary>
    /// 库存调整类型
    /// </summary>
    public enum AdjustType
    {
        /// <summary>
		/// 调整来源
		/// </summary>
		[Label("来源")]
        From = 1,

        /// <summary>
		/// 调整目标
		/// </summary>
		[Label("目标")]
        To = 2,
    }
}
