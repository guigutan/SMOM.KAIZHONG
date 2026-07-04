using SIE.ObjectModel;

namespace SIE.Warehouses
{
    /// <summary>
    /// 冻结原因
    /// </summary>
    public enum FrozenReason
    {
        /// <summary>
        /// 仓库冻结
        /// </summary>
        [Label("仓库冻结")]
        WarehouseFrozen,

        /// <summary>
        /// 库区冻结
        /// </summary>
        [Label("库区冻结")]
        AreaFrozen,

        /// <summary>
        /// 库位冻结
        /// </summary>
        [Label("库位冻结")]
        LocFrozen,

        /// <summary>
        /// 盘点异常冻结
        /// </summary>
        [Label("盘点异常冻结")]
        CountAbnormalFrozen,

        /// <summary>
        /// 出库取空冻结
        /// </summary>
        [Label("出库取空冻结")]
        OutTakeEmptyFrozen,

        /// <summary>
        /// 空库位有货冻结
        /// </summary>
        [Label("空库位有货冻结")]
        EmptyLocInStockFrozen,

        /// <summary>
        /// 设备异常冻结
        /// </summary>
        [Label("设备异常冻结")]
        EquipAbnormalFrozen,
    }
}
