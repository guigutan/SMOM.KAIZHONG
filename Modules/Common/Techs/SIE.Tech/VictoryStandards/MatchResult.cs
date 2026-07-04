using SIE.ObjectModel;

namespace SIE.Tech.VictoryStandards
{
    /// <summary>
    /// 胜制匹配结果
    /// </summary>
    public enum MatchResult
    {
        /// <summary>
        /// 完全匹配
        /// </summary>
        [Label("完全匹配")]
        ExactMatch,

        /// <summary>
        /// 部分匹配，头部匹配
        /// </summary>
        [Label("部分匹配")]
        PartialMatch,

        /// <summary>
        /// 不匹配
        /// </summary>
        [Label("不匹配")]
        UnMatch,
    }
}