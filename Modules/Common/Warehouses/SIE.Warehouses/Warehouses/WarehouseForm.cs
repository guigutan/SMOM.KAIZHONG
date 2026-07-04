using SIE.ObjectModel;

namespace SIE.Warehouses
{
    /// <summary>
    /// 仓库形式
    /// </summary>
    public enum WarehouseForm
    {
        /// <summary>
        /// 平面仓库
        /// </summary>
        [Label("平面仓库")]
        Plane = 0,

        /// <summary>
        /// 多层仓库
        /// </summary>
        [Label("多层仓库")]
        Multilayer = 1,

        /// <summary>
        /// 立体仓库
        /// </summary>
        [Label("立体仓库")]
        Solid = 2,

        /// <summary>
        /// 流质槽罐
        /// </summary>
        [Label("流质槽罐")]
        liquid = 3,

        /// <summary>
        /// 混合仓库
        /// </summary>
        [Label("混合仓库")]
        Mix = 4,
    }
}