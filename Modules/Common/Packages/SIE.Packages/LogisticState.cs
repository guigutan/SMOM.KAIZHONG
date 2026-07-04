using SIE.ObjectModel;

namespace SIE.Packages
{
    /// <summary>
    /// 物流状态
    /// </summary>
    public enum LogisticState
    {
        /// <summary>
		/// 未打印
		/// </summary>
		[Label("未打印")]
        UnPrinted = 0,

        /// <summary>
        /// 已打印
        /// </summary>
        [Label("已打印")]
        Printed = 1,
    }
}