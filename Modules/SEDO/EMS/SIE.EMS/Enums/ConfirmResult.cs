using SIE.ObjectModel;

namespace SIE.EMS.Enums
{
    /// <summary>
    /// 确认结果
    /// </summary>
    public enum ConfirmResult
    {
        /// <summary>
        /// 合格
        /// </summary>
        [Label("合格")]
        OK = 1,

        /// <summary>
        /// 不合格
        /// </summary>
        [Label("不合格")]
        NG = 2,
    }
}