using SIE.ObjectModel;

namespace SIE.LES.LesStockCounts
{
    /// <summary>
    /// 盘点状态
    /// </summary>
    public enum LesCountState
    {
        /// <summary>
        /// 创建
        /// </summary>
        [Label("创建")]
        Create = 0,

        /// <summary>
        /// 待盘点
        /// </summary>
        [Label("待盘点")]
        Audit = 10,

        /// <summary>
        /// 部分盘点
        /// </summary>
        [Label("部分盘点")]
        PartCount = 30,

        /// <summary>
        /// 已盘点
        /// </summary>
        [Label("已盘点")]
        FinishCount = 40,

        /// <summary>
        /// 完工
        /// </summary>
        [Label("完工")]
        Finished = 50,

        /// <summary>
        /// 关闭
        /// </summary>
        [Label("关闭")]
        Close = 60,
    }
}
