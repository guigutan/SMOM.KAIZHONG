using SIE.ObjectModel;

namespace SIE.LES.Distributions.Configs
{
    /// <summary>
    /// 跳过扫描
    /// </summary>
    public enum IsNoDistributionType
    {
        /// <summary>
        /// 跳过扫描配送
        /// </summary>
        [Label("跳过扫描配送")]
        NoScan = 0,

        /// <summary>
        /// 跳过配送送达
        /// </summary>
        [Label("跳过配送送达")]
        NoRecive = 1,
    }
}
