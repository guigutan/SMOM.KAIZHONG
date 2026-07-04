using SIE.ObjectModel;

namespace SIE.Packages.ItemLabels
{
    /// <summary>
    /// 标签状态
    /// </summary>
    public enum LabelStatus
    {
        /// <summary>
        /// 已配送
        /// </summary>
        [Label("已配送")]
        Distribution = 5,

        /// <summary>
        /// 已接收
        /// </summary>
        [Label("已接收")]
        Received = 10,

        /// <summary>
        /// 已上料
        /// </summary>
        [Label("已上料")]
        LoadItem = 15,

        /// <summary>
        /// 已下料
        /// </summary>
        [Label("已下料")]
        UnLoadItem = 20,

        /// <summary>
        /// 用毕
        /// </summary>
        [Label("用毕")]
        UseUp = 25,

        /// <summary>
        /// 待退
        /// </summary>
        [Label("待退")]
        StaBack = 30,

        /// <summary>
        /// 待转
        /// </summary>
        [Label("待转")]
        ToTurn = 35,

        /// <summary>
        /// 已转
        /// </summary>
        [Label("已转")]
        Turned = 40,

        /// <summary>
        /// 已退料
        /// </summary>
        [Label("已退料")]
        Withdrawal = 45,

        /// <summary>
        /// 已发货
        /// </summary>
        [Label("已发货")]
        Delivery = 50,
    }
}