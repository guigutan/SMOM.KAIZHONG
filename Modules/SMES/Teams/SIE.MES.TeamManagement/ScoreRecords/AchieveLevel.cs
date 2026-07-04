using SIE.ObjectModel;

namespace SIE.MES.TeamManagement.ScoreRecords
{
    /// <summary>
    /// 绩效等级
    /// </summary>
    public enum AchieveLevel
    {
        /// <summary>
        /// 优秀
        /// </summary>
        [Label("优秀")]
        Great = 0,

        /// <summary>
        /// 良好
        /// </summary>
        [Label("良好")]
        Good = 1,

        /// <summary>
        /// 及格
        /// </summary>
        [Label("及格")]
        Pass = 2,

        /// <summary>
        /// 不良
        /// </summary>
        [Label("不良")]
        Bad = 3,
    }
}