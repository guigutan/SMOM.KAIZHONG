using SIE.ObjectModel;

namespace SIE.Warehouses
{
    /// <summary>
    /// 货区类型
    /// </summary>
    public enum StorageAreaType
    {
        /// <summary>
        /// 储存区
        /// </summary>
        [Label("储存区")]
        Storage,

        /// <summary>
        /// 接收区
        /// </summary>
        [Label("接收区")]
        Receiving,

        /// <summary>
        /// 拣货区
        /// </summary>
        [Label("拣货区")]
        Pick,

        /// <summary>
        /// 中转区
        /// </summary>
        [Label("中转区")]
        Transit,

        /// <summary>
        /// 待入库区
        /// </summary>
        [Label("待入库区")]
        WaitStorage,

        /// <summary>
        /// 待退货区
        /// </summary>
        [Label("待退货区")]
        WaitReturn,

        /// <summary>
        /// 线边区
        /// </summary>
        [Label("线边区")]
        LineStorage,

        /// <summary>
        /// 退货区
        /// </summary>
        [Label("退货区")]
        Return,

        /// <summary>
        /// 帐户区
        /// </summary>
        [Label("帐户区")]
        Account,

        /// <summary>
        /// 在途区
        /// </summary>
        [Label("在途区")]
        Route,

        /// <summary>
        /// 结算区
        /// </summary>
        [Label("结算区")]
        Settlement,

        /// <summary>
        /// 完工退回区
        /// </summary>
        [Label("完工退回区")]
        WipCompletionReturn,
    }
}