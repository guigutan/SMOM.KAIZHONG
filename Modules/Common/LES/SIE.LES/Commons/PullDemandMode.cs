using SIE.ObjectModel;

namespace SIE.LES
{
    /// <summary>
    /// 拉式需求计算方式
    /// </summary>
    public enum PullDemandMode
	{
        /// <summary>
        /// 手工填写
        /// </summary>
        [Label("手工填写")]
        ManualFillIn = 0,

		/// <summary>
		/// 固定量
		/// </summary>
		[Label("固定量")]
		FixedQuantity = 4,

        /// <summary>
        /// 补充到最高存量
        /// </summary>
        [Label("补充到最高存量")]
        MaxStock = 5,
    }
}