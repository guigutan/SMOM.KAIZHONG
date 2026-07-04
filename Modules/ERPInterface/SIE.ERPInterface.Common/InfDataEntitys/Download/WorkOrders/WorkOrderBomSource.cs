using SIE.ObjectModel;

namespace SIE.ERPInterface.Common.InfDataEntitys.Download
{
    /// <summary>
    /// 工单BOM来源
    /// </summary>
    public enum WorkOrderBomSource
    {
        /// <summary>
        /// ERP
        /// </summary>
        [Label("ERP")]
        Erp = 0,
        /// <summary>
        /// 产品BOM
        /// </summary>
        [Label("产品BOM")]
        ProductBom = 1,
    }
}