using SIE.ObjectModel;

namespace SIE.LES.StockOrders
{
    /// <summary>
    /// 备料状态
    /// </summary>
    public enum StockState
    {
        /// <summary>
        /// 待提交
        /// </summary>
        [Label("待提交")]
        Created = 0,

        /// <summary>
        /// 已撤回
        /// </summary>
        [Label("已撤销")]
        ReCall = 5,

        /// <summary>
        /// 待审核
        /// </summary>
        [Label("待审核")]
        Audit = 10,

        /// <summary>
        /// 已提交
        /// </summary>
        [Label("已提交")]
        Submitted = 20,

        /// <summary>
        /// 拣配中
        /// </summary>
        [Label("拣配中")]
        PickStocking = 25,

        /// <summary>
        /// 待接收
        /// </summary>
        [Label("待接收")]
        TobeReceive = 40,


        /// <summary>
        /// 已接收
        /// </summary>
        [Label("已接收")]
        Received = 50,

        ///// <summary>
        ///// 部分接收
        ///// </summary>
        //[Label("部分接收")]
        //PartReceived = 51,

        /// <summary>
        /// 已关闭
        /// </summary>
        [Label("已关闭")]
        Closed = 60,

        /// <summary>
        /// 已下发
        /// </summary>
        [Label("已下发")]
        Issued = 70,
    }
}
