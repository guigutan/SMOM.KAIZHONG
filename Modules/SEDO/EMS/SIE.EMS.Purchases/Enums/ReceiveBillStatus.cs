using SIE.ObjectModel;

namespace SIE.EMS.Purchases.Enums
{
    /// <summary>
    /// 接收单状态
    /// </summary>
    public enum ReceiveBillStatus
    {
        /// <summary>
        /// 待接收
        /// </summary>
        [Label("待接收")]
        ToBeReceived = 0,

        /// <summary>
        /// 待提交
        /// </summary>
        [Label("待提交")]
        ToBeSubmitted = 10,

        /// <summary>
        /// 已完成
        /// </summary>
        [Label("已完成")]
        Completed = 20,
    }
}