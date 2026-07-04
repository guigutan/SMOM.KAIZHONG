using SIE.ObjectModel;

namespace SIE.EMS.Purchases.Enums
{
    /// <summary>
    /// 付款条件
    /// </summary>
    public enum PaymentCondition
    {
        /// <summary>
        /// 合同签订
        /// </summary>
        [Label("合同签订")]
        Contracts = 0,
        /// <summary>
        /// 采购接收
        /// </summary>
        [Label("采购接收")]
        Receiving = 1,
        /// <summary>
        /// 采购验收
        /// </summary>
        [Label("采购验收")]
        AcceptanceCheck = 2,
        /// <summary>
        /// 入库
        /// </summary>
        [Label("入库")]
        Warehousing = 3,
        /// <summary>
        /// 项目初验
        /// </summary>
        [Label("项目初验")]
        ProInitiativeTest = 4,
        /// <summary>
        /// 项目验收
        /// </summary>
        [Label("项目验收")]
        ProjectAcceptance = 5,
        /// <summary>
        /// 质保验收
        /// </summary>
        [Label("质保验收")]
        Warranty = 6
    }
}
