using SIE.Common.Configs;

namespace SIE.Inventory.Task.Configs
{
    /// <summary>
    /// 任务管理参数
    /// </summary>
    [System.ComponentModel.DisplayName("任务管理参数配置")]
    [System.ComponentModel.Description("任务管理参数配置")]
    internal class TaskParameterConfig : ModuleConfig<TaskParameterConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly TaskParameterConfigValue defaultValue = new TaskParameterConfigValue
        {
            ExecuteTimeout = 60,
            NotGetTimeout = 60,
            UntreatedTimeout = 60,
            RefreshCycle = 5,
            UrgentMaxCount = 5
        };

        /// <summary>
        /// 默认值
        /// </summary>
        public override TaskParameterConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
