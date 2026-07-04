using SIE.ObjectModel;

namespace SIE.Fixtures.Enums
{
    /// <summary>
    /// 单据来源
    /// </summary>
    public enum BillSource
    {
        /// <summary>
		/// 手动
		/// </summary>
		[Label("手动")]
        Manual = 5,
        /// <summary>
		/// 手动
		/// </summary>
		[Label("自动")]
        Auto = 10,
    }
}
