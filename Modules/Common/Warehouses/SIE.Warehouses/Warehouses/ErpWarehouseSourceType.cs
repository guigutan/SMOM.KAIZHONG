using SIE.ObjectModel;

namespace SIE.Warehouses
{
    /// <summary>
    /// 来源类型
    /// </summary>
    public enum ErpWarehouseSourceType
    {
        /// <summary>
        /// 自建
        /// </summary>
        [Label("自建")]
        SelfBuild,

        /// <summary>
        /// 外部
        /// </summary>
        [Label("ERP数据")]
        Erp,
    }
}
