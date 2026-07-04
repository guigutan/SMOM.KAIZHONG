using SIE.XPCJ.Models.Attributes;

namespace SIE.XPCJ.Models.WIP
{
    /// <summary>
    /// 上料来源类型
    /// </summary>
    public enum LoadItemSourceType
    {
        /// <summary>
        /// 配送单
        /// </summary>
        [Label("配送单")]
        DistributionBill = 0,

        /// <summary>
        /// 配送标签
        /// 物料标签通过载具关联上料，与物料标签差异在于可以部分下料
        /// </summary>
        [Label("配送标签")]
        DistributionLabel = 1,

        /// <summary>
        /// 物料标签
        /// </summary>
        [Label("物料标签")]
        ItemLabel = 2,

        /// <summary>
        /// 单体标签
        /// </summary>
        [Label("单体标签")]
        SingleLabel = 3,

        /// <summary>
        /// 生产条码
        /// </summary>
        [Label("生产条码")]
        SN = 4,
    }
}
