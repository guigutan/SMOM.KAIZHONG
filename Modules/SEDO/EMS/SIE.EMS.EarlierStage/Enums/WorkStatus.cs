using SIE.ObjectModel;

namespace SIE.EMS.EarlierStage.Enums
{
    /// <summary>
    /// 工作状态
    /// </summary>
    public enum WorkStatus
    {
        /// <summary>
        /// 未开始
        /// </summary>
        [Label("未开始")]
        NotStarted = 10,
        /// <summary>
        /// 进行中
        /// </summary>
        [Label("进行中")]
        InProgress = 20,
        /// <summary>
        /// 已完成
        /// </summary>
        [Label("已完成")]
        Finish = 30,
    }
}