using SIE.XPCJ.Models.Attributes;

namespace SIE.XPCJ.Models.WIP
{
    /// <summary>
    /// 过站记录状态
    /// </summary>
    public enum WipProductProcessState
    {
        /// <summary>
        /// 完成
        /// </summary>
        [Label("完成")]
        Finish = 0,

        /// <summary>
        /// 开始
        /// </summary>
        [Label("开始")]
        Start = 1,
    }
}
