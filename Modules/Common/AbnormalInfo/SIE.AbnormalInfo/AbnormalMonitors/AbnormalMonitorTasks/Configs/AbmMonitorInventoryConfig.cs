using SIE.Common.Configs;

namespace SIE.AbnormalInfo.AbnormalMonitors.Configs
{
	/// <summary>
	/// 异常清单自动生成异常任务配置
	/// </summary>
	[System.ComponentModel.DisplayName("异常清单自动生成异常任务配置")]
    [System.ComponentModel.Description("异常清单自动生成异常任务配置")]
    public class AbmMonitorInventoryConfig : ModuleConfig<AbmMonitorInventoryConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        public override AbmMonitorInventoryConfigValue DefaultValue { get; } = new AbmMonitorInventoryConfigValue() { };
    }
}
