using SIE.ObjectModel;

namespace SIE.MES.TeamManagement.ScoreRecords
{
    /// <summary>
    /// 申述处理方式
    /// </summary>
    public enum StateProcessMode
    {
        /// <summary>
        /// 调整评判
        /// </summary>
        [Label("调整评判")]
        Adjust,

        /// <summary>
        /// 拒绝申诉
        /// </summary>
        [Label("拒绝申诉")]
        Refuse,

        /// <summary>
        /// 撤销评分
        /// </summary>
        [Label("撤销评分")]
        Repeal,
    }
}