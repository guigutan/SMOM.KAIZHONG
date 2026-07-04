using SIE.ObjectModel;

namespace SIE.MES.WorkOrders
{
    /// <summary>
    /// 工单状态
    /// </summary>
    public enum WorkOrderStates
    {
        /// <summary>
        /// 发放
        /// </summary>
        [Label("发放")]
        Release,

        /// <summary>
        /// 生产中
        /// </summary>
        [Label("生产中")]
        Producing,

        /// <summary>
        /// 完工
        /// </summary>
        [Label("完工")]
        Finish,

        /// <summary>
        /// 关闭
        /// </summary>
        [Label("关闭")]
        Close,

        /// <summary>
        /// 发放暂停
        /// </summary>
        [Label("发放暂停")]
        ReleasePause,

        /// <summary>
        /// 生产暂停
        /// </summary>
        [Label("生产中暂停")]
        ProducePause,
    }
}