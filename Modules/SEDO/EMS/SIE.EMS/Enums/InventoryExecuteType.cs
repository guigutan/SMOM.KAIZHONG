using SIE.ObjectModel;

namespace SIE.EMS.Enums
{
    /// <summary>
    /// 执行类型
    /// </summary>
    public enum InventoryExecuteType
    {
        /// <summary>
        /// 明盘
        /// </summary>
        [Label("明盘")]
        Bright = 10,
        /// <summary>
        /// 盲盘
        /// </summary>
        [Label("盲盘")]
        Blind = 20,
    }
}