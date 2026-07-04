using SIE.ObjectModel;

namespace SIE.MES.TeamManagement.ScoreRecords
{
    /// <summary>
    /// 评分记录来源
    /// </summary>
    public enum ScoreSource
    {
        /// <summary>
        /// App创建
        /// </summary>
        [Label("App创建")]
        App = 0,

        /// <summary>
        /// 调度创建
        /// </summary>
        [Label("调度创建")]
        Schedule = 1,
    }
}
