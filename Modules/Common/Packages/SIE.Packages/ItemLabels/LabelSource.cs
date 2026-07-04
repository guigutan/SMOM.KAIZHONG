using SIE.ObjectModel;

namespace SIE.Packages.ItemLabels
{
    /// <summary>
    /// 标签来源
    /// </summary>
    public enum LabelSource
    {
        /// <summary>
        /// 物料接收
        /// </summary>
        [Label("物料接收")]
        Receive,

        /// <summary>
        /// 生产采集
        /// </summary>
        [Label("生产采集")]
        Wip,

        /// <summary>
        /// 批次生产采集
        /// </summary>
        [Label("批次生产采集")]
        BatchWip,

        /// <summary>
        /// 载具配送
        /// </summary>
        [Label("载具配送")]
        Distribution,

        /// <summary>
        /// 盘点新增
        /// </summary>
        [Label("盘点新增")]
        Count,

        /// <summary>
        /// 外部录入
        /// </summary>
        [Label("外部录入")]
        Import,
    }
}