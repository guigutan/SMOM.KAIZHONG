using SIE.ObjectModel;

namespace SIE.LES.Distributions
{
    /// <summary>
    /// 订单状态
    /// </summary>
    public enum OrderState
    {
        /// <summary>
        /// 待配送
        /// </summary>
        [Label("待配送")]
        WaitDelivery = 0,

        /// <summary>
        /// 配送中
        /// </summary>
        [Label("配送中")]
        Delivery = 1,

        /// <summary>
        /// 已送达
        /// </summary>
        [Label("已送达")]
        Receipt = 2,


        /// <summary>
        /// 已送达(2023-10-28 将已取消改为已送达)
        /// </summary>
        [Label("已送达")]
        Cancel = 3,

        /// <summary>
        /// 关闭
        /// </summary>
        [Label("关闭")]
        Close = 4,

    }
}
