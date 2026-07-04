using SIE.Common.Configs;
using SIE.LES.StockOrders.Configs;

namespace SIE.LES.MaterialPreparations.Configs
{
    /// <summary>
    /// 推式备料是否限制最高存量
    /// </summary>
    [System.ComponentModel.DisplayName("推式备料是否限制最高存量")]
    [System.ComponentModel.Description("推式备料是否限制最高存量")]
    public class LimitedPrepareMaxConfig : ModuleConfig<LimitedPrepareMaxConfigValue>
    {
        private readonly LimitedPrepareMaxConfigValue defaultValue = new LimitedPrepareMaxConfigValue();

        /// <summary>
        /// 默认值
        /// </summary>
        public override LimitedPrepareMaxConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
