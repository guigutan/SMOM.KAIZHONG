using SIE.ObjectModel;

namespace SIE.WorkBenchCommon.Workbench.KPI
{
    /// <summary>
    /// KPI指标维度
    /// </summary>
    public enum KPIDimension
    {
        /// <summary>
        /// 库存组织
        /// </summary>
        [Label("库存组织")]
        InvOrg = 0,

        /// <summary>
        /// 企业层级
        /// </summary>
        [Label("企业层级")]
        Enterprise = 1
    }
}
