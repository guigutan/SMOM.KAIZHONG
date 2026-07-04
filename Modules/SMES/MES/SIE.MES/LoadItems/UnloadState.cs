using SIE.ObjectModel;

namespace SIE.MES.LoadItems
{
    /// <summary>
    /// 下料接收状态
    /// </summary>
    public enum UnloadState
    {
        /// <summary>
        /// 待确认
        /// </summary>
        [Label("待确认")]
        UnConfirm,

        /// <summary>
        /// 已确认
        /// </summary>
        [Label("已确认")]
        Confirmed,

        /// <summary>
        /// 重新上料
        /// </summary>
        [Label("重新上料")]
        ReloadItem,
    }
}