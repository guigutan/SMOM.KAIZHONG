using SIE.ObjectModel;

namespace SIE.MES.TeamManagement.ScoreRecords
{
    /// <summary>
    /// 运输符
    /// </summary>
    public enum Operator
    {
        /// <summary>
        /// 小于
        /// </summary>
        [Label("小于")]
        Less = 0,

        /// <summary>
        /// 大于
        /// </summary>
        [Label("大于")]
        Greater = 1,

        /// <summary>
        /// 介于 (闭合,例如大于等于60且小于等于80)
        /// </summary>
        [Label("介于")]
        Between = 2,

        /// <summary>
        /// 小于等于
        /// </summary>
        [Label("小于等于")]
        LessEqual = 3,

        /// <summary>
        /// 大于等于
        /// </summary>
        [Label("大于等于")]
        GreaterEqual = 4,
    }
}