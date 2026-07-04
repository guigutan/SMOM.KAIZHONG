using SIE.ObjectModel;

namespace SIE.ERPInterface.Common.Enums
{
    /// <summary>
    /// 任务模式
    /// </summary>
    public enum JobMode
    {
        /// <summary>
        /// 拉式
        /// </summary>
        [Label("拉式")]
        Pull = 0,
        /// <summary>
        /// 推式
        /// </summary>
        [Label("推式")]
        Push = 1,
    }
}