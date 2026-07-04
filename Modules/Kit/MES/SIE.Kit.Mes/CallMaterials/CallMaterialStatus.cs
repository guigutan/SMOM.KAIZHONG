using SIE.ObjectModel;

namespace SIE.Kit.MES.CallMaterials
{
    /// <summary>
    /// 叫料单状态
    /// </summary>
    public enum CallMaterialStatus
    {
        /// <summary>
        /// 待处理
        /// </summary>
        [Label("待处理")]
        Pending,

        /// <summary>
        /// 待接收
        /// </summary>
        [Label("待接收")]
        ToReceive,

        /// <summary>
        /// 已接收
        /// </summary>
        [Label("已接收")]
        Received,

        /// <summary>
        /// 已超时
        /// </summary>
        [Label("已超时")]
        Timeout,

        /// <summary>
		/// 已取消
		/// </summary>
		[Label("已取消")]
        Cancel,
    }
}