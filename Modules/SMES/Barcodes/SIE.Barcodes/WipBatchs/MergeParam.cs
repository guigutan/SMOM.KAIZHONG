using SIE.ObjectModel;

namespace SIE.Barcodes.WipBatchs
{
    /// <summary>
	/// 合并参数
	/// </summary>
	public enum MergeParam
    {
        /// <summary>
        /// 工单
        /// </summary>
        [Label("工单")]
        WorkOrder,

        /// <summary>
        /// 自定义
        /// </summary>
        [Label("自定义")]
        Custom,
    }
}