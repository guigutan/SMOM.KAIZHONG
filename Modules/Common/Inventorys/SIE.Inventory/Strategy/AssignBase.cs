using SIE.ObjectModel;

namespace SIE.Inventory.Strategy
{
    /// <summary>
    /// 分配模式
    /// </summary>
    public enum AssignBase
    {
        /// <summary>
        /// 允许拆包
        /// </summary>
        [Label("允许拆包")]
        AllowSplit = 0,
       
        /// <summary>
        /// 整包分配
        /// </summary>
        [Label("整包分配")]
        PackageAssign = 2,
    }
}
