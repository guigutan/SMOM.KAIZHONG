using SIE.ObjectModel;

namespace SIE.EMS.Enums
{
    /// <summary>
    /// 盘点范围
    /// </summary>
    public enum InventoryScope
    {
        /// <summary>
        /// 所有设备
        /// </summary>
        [Label("所有设备")]
        All = 10,
        /// <summary>
        /// 自有管理
        /// </summary>
        [Label("自有管理")]
        Own = 20,
    }
}