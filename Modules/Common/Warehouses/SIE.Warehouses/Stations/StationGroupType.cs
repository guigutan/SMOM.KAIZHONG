using SIE.ObjectModel;

namespace SIE.Warehouses.Stations
{
    /// <summary>
    /// 站点组类型
    /// </summary>
    public enum StationGroupType
    {
        /// <summary>
        /// 入库站台
        /// </summary>
        [Label("入库站台")]
        In = 1,

        /// <summary>
        /// 出库站台
        /// </summary>
        [Label("出库站台")]
        Out = 2,

        /// <summary>
        /// 拣选站台
        /// </summary>
        [Label("拣选站台")]
        Picking = 3,

        /// <summary>
        /// 盘点站台(滚筒线)
        /// </summary>
        [Label("盘点站台(滚筒线)")]
        RollerLine = 4,

        /// <summary>
        /// 盘点站台(平库)
        /// </summary>
        [Label("盘点站台(平库)")]
        FlatLibrary = 5,
    }
}