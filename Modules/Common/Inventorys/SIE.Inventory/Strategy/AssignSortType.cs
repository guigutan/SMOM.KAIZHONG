using SIE.ObjectModel;

namespace SIE.Inventory.Strategy
{
    /// <summary>
    /// 分配顺序
    /// </summary>
    public enum AssignSortType
    {
        /// <summary>
        /// 周转规则
        /// </summary>
        [Label("周转规则")]
        TurnOver = 10,

        /// <summary>
        /// 拣货顺序
        /// </summary>
        [Label("拣货顺序")]
        PickOrder = 20,

        /// <summary>
        /// 可用数量
        /// </summary>
        [Label("可用数量")]
        LpnQty = 30,

        /// <summary>
        /// 立库优先
        /// </summary>
        [Label("立库优先")]
        AutomatedFirst = 40,

        /// <summary>
        /// 立库靠后
        /// </summary>
        [Label("立库靠后")]
        AutomatedLast = 50,

        /// <summary>
        /// LPN优先
        /// </summary>
        [Label("LPN优先")]
        LpnFirst = 60,

        /// <summary>
        /// LPN靠后 
        /// </summary>
        [Label("LPN靠后 ")]
        LpnLast = 70,
    }
}