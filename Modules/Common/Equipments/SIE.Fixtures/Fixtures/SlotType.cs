using SIE.ObjectModel;

namespace SIE.Fixtures
{
    /// <summary>
	/// 槽位类型
	/// </summary>
	public enum SlotType
    {
        /// <summary>
        /// 单槽
        /// </summary>
        [Label("单槽")]
        Single = 5,

        /// <summary>
        /// 双槽
        /// </summary>
        [Label("双槽")]
        Double = 10,

        /// <summary>
        /// 三槽
        /// </summary>
        [Label("三槽")]
        Three = 15,
    }
}
