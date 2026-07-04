using SIE.ObjectModel;

namespace SIE.EMS.Projects.Enums
{
    /// <summary>
    /// 项目状态
    /// </summary>
    public enum ProjectStatus
    {
        /// <summary>
        /// 未开始
        /// </summary>
        [Label("未开始")]
        NotStarted = 10,
        /// <summary>
        /// 暂停
        /// </summary>
        [Label("暂停")]
        Pause = 20,
        /// <summary>
        /// 结项
        /// </summary>
        [Label("结项")]
        Closed = 30,
        /// <summary>
        /// 进行中
        /// </summary>
        [Label("进行中")]
        InProgress = 40,
        /// <summary>
        /// 中止
        /// </summary>
        [Label("中止")]
        Abort = 50,
        /// <summary>
        /// 变更中
        /// </summary>
        [Label("变更中")]
        Changing = 60,
    }
}
