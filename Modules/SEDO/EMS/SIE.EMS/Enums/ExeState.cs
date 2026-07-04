using SIE.ObjectModel;

namespace SIE.EMS.Enums
{

    /// <summary>
    /// 点检执行状态
    /// </summary>
    public enum CheckExeState
    {
        /// <summary>
        /// 未执行
        /// </summary>
        [Label("未执行")]
        NotPerformed = 0,

        /// <summary>
        /// 已执行
        /// </summary>
        [Label("已执行")]
        Performed = 1,

        /// <summary>
        /// 超期
        /// </summary>
        [Label("超期")]
        Overdue = 2,

        /// <summary>
        /// 已评分
        /// </summary>
        [Label("已评分")]
        Scored = 3,

        /// <summary>
        /// 执行中
        /// </summary>
        [Label("执行中")]
        Performing = 4,

        /// <summary>
        /// 待确认
        /// </summary>
        [Label("待确认")]
        NotConfirm = 5,

        /// <summary>
        /// 关闭
        /// </summary>
        [Label("关闭")]
        Closed = 6,

        /// <summary>
        /// 已确认
        /// </summary>
        [Label("已确认")]
        Confirmed = 7,
    }

    /// <summary>
    /// 保养执行状态
    /// </summary>
    public enum MaintExeState
    {
        /// <summary>
        /// 未执行
        /// </summary>
        [Label("未执行")]
        NotPerformed = 0,

        /// <summary>
        /// 已执行
        /// </summary>
        [Label("已执行")]
        Performed = 1,

        /// <summary>
        /// 超期
        /// </summary>
        [Label("超期")]
        Overdue = 2,

        /// <summary>
        /// 已评分
        /// </summary>
        [Label("已评分")]
        Scored = 3,

        /// <summary>
        /// 执行中
        /// </summary>
        [Label("执行中")]
        Performing = 4,

        /// <summary>
        /// 待确认
        /// </summary>
        [Label("待确认")]
        NotConfirm = 5,

        /// <summary>
        /// 关闭
        /// </summary>
        [Label("关闭")]
        Closed = 6,

        /// <summary>
        /// 已确认
        /// </summary>
        [Label("已确认")]
        Confirmed = 7,
    }

    /// <summary>
    /// 检验执行状态
    /// </summary>
    public enum CalExeState
    {
        /// <summary>
        /// 未执行
        /// </summary>
        [Label("未执行")]
        NotPerformed = 0,

        /// <summary>
        /// 已执行
        /// </summary>
        [Label("已执行")]
        Performed = 1,

        /// <summary>
        /// 超期
        /// </summary>
        [Label("超期")]
        Overdue = 2,

        /// <summary>
        /// 已评分
        /// </summary>
        [Label("已评分")]
        Scored = 3,

        /// <summary>
        /// 执行中
        /// </summary>
        [Label("执行中")]
        Performing = 4,

        /// <summary>
        /// 待确认
        /// </summary>
        [Label("待确认")]
        NotConfirm = 5,
    }
}
