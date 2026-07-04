using SIE.ObjectModel;

namespace SIE.EMS.Enums
{
    /// <summary>
    /// 点检保养结果
    /// </summary>
    public enum CheckMaintainResult
    {
        /// <summary>
        /// 不合格
        /// </summary>
        [Label("不合格")]
        NG = 0,

        /// <summary>
		/// 合格
		/// </summary>
		[Label("合格")]
        OK = 1,

        /// <summary>
        /// 不适用
        /// </summary>
        [Label("不适用")]
        Unright = 2,
    }
}
