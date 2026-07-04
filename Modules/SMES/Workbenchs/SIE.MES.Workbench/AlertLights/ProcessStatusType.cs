using SIE.ObjectModel;

namespace SIE.MES.Workbench.AlertLights
{
    /// <summary>
    /// 处理状态类型
    /// </summary>
    public enum ProcessStatusType
    {
        /// <summary>
        /// 等待中
        /// </summary>
        [Label("等待中")]
        Waitting = 1,

        /// <summary>
        /// 处理中
        /// </summary>
        [Label("处理中")]
        Processing = 2,

        /// <summary>
        /// 已完成
        /// </summary>
        [Label("已解决")]
        Closed = 3,

        /// <summary>
        /// 已取消
        /// </summary>
        [Label("已取消")]
        Cancel = 4,
    }
}
