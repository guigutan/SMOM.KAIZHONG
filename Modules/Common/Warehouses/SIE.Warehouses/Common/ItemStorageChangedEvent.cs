using SIE.Common.Queues;

namespace SIE.Warehouses
{
    /// <summary>
    /// 货位库存变更事件
    /// </summary>
    public class ItemStorageChangedEvent : ContextEvent
    {
        /// <summary>
        /// 是否车间仓库
        /// </summary>
        public bool IsWip { get; set; }

        /// <summary>
        /// 物料编号
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 货位
        /// </summary>
        public double LocationId { get; set; }
    }
}
