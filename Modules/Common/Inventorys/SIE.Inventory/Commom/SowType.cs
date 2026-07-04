using SIE.ObjectModel;

namespace SIE.Inventory.Commom
{
    /// <summary>
	/// 播种方式
	/// </summary>
	public enum SowType
    {
        /// <summary>
        /// 先拣后分
        /// </summary>
        [Label("先拣后分")]
        FIRSTPICK,

        /// <summary>
        /// 边拣边分
        /// </summary>
        [Label("边拣边分")]
        SIDEPICK,
    }
}
