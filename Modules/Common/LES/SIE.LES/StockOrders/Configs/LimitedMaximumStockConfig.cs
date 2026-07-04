using SIE.Common.Configs;

namespace SIE.LES.StockOrders.Configs
{
    /// <summary>
    /// 推式备料是否限制最高存量
    /// </summary>
    [System.ComponentModel.DisplayName("推式备料是否限制最高存量")]
    [System.ComponentModel.Description("推式备料是否限制最高存量")]
    public class LimitedMaximumStockConfig : ModuleConfig<LimitedMaximumStockConfigValue>
    {
        private readonly LimitedMaximumStockConfigValue defaultValue = new LimitedMaximumStockConfigValue();

        /// <summary>
        /// 默认值
        /// </summary>
        public override LimitedMaximumStockConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
