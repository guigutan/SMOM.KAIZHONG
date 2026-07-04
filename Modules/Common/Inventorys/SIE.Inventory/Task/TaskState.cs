using SIE.ObjectModel;

namespace SIE.Inventory.Task
{
    /// <summary>
    /// 任务状态
    /// </summary>
    public enum TaskState
    {
        /// <summary>
        /// 创建
        /// </summary>
        [Label("创建")]
        Create = 0,

        /// <summary>
        /// 释放
        /// </summary>
        [Label("释放")]
        Release = 1,

        /// <summary>
        /// 指派
        /// </summary>
        [Label("指派")]
        Appoint = 2,

        /// <summary>
        /// 冻结
        /// </summary>
        [Label("冻结")]
        Frozen = 3,

        /// <summary>
        /// 挂起
        /// </summary>
        [Label("挂起")]
        HangUp = 4,

        /// <summary>
        /// 完工
        /// </summary>
        [Label("完工")]
        Finish = 5,

        /// <summary>
        /// 关闭
        /// </summary>
        [Label("关闭")]
        Close = 6,

        /// <summary>
        /// 执行中
        /// </summary>
        [Label("执行中")]
        Executing = 7,

        /// <summary>
        /// 异常
        /// </summary>
        [Label("异常")]
        Abnormal = 8,

        /// <summary>
        /// 自动完工
        /// </summary>
        [Label("自动完工")]
        AutoFinish = 9
    }
}