using SIE.ObjectModel;

namespace SIE.MES.TeamManagement.ScoreRecords
{
    /// <summary>
    /// 评分状态
    /// </summary>
    public enum ScoreState
    {
        /// <summary>
        /// 评分创建--待申诉
        /// </summary>
        [Label("待申诉")]
        State,

        /// <summary>
        /// 申诉创建--申述中
        /// </summary>
        [Label("申诉中")]
        Stating,

        /// <summary>
        /// 申诉处理--已处理
        /// </summary>
        [Label("已处理")]
        Processed,

        /// <summary>
        /// 取消申诉
        /// </summary>
        [Label("已取消")]
        Canceled,

        /// <summary>
        /// 评分删除
        /// </summary>
        [Label("已删除")]
        Deleted,
    }
}