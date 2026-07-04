using SIE.Common.Configs;

namespace SIE.LES.StockOrders.Configs
{
    /// <summary>
    /// 调度触发的备料单状态
    /// </summary>
    [System.ComponentModel.DisplayName("调度触发的备料单状态")]
    [System.ComponentModel.Description("调度触发的备料单状态")]
    public class SchedulingTriggeredStatusConfig : ModuleConfig<SchedulingTriggeredStatusConfigValue>
    {
        private readonly SchedulingTriggeredStatusConfigValue defaultValue = new SchedulingTriggeredStatusConfigValue();

        /// <summary>
        /// 默认值
        /// </summary>
        public override SchedulingTriggeredStatusConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
