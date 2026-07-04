using SIE.ObjectModel;

namespace SIE.EMS.Purchases.Enums
{
    /// <summary>
    /// 安装调试状态
    /// </summary>
    public enum SetupStatus
    {
        /// <summary>
        /// 待执行
        /// </summary>
        [Label("待执行")]
        ToBe = 10,
        /// <summary>
        /// 执行中
        /// </summary>
        [Label("执行中")]
        Doing = 20,
        /// <summary>
        /// 已完成
        /// </summary>
        [Label("已完成")]
        Done = 30,
        /// <summary>
        /// 交机确认
        /// </summary>
        [Label("交机确认")]
        DeliveryConfirm = 40,
    }
}