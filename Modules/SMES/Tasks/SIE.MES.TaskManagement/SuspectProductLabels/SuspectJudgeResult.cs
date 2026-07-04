using SIE.ObjectModel;

namespace SIE.MES.TaskManagement.SuspectProductLabels
{
    /// <summary>
    /// 可疑品判定结果
    /// </summary>
    public enum SuspectJudgeResult
    {
        /// <summary>
        /// 良品
        /// </summary>
        [Label("良品")]
        Good,
        /// <summary>
        /// 报废
        /// </summary>
        [Label("报废")]
        Scrap,
        /// <summary>
        /// 返工
        /// </summary>
        [Label("返工")]
        Repair,
    }
}
