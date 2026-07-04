using SIE.Common.Configs;

namespace SIE.MES.WorkOrders.Configs
{
    /// <summary>
    /// 工单创建是否自动发起追溯流程配置
    /// </summary>
    [System.ComponentModel.DisplayName("工单创建是否自动发起追溯流程 配置")]
    [System.ComponentModel.Description("工单创建是否自动发起追溯流程 配置")]
    public class AutoStartWoDataTraceWorkFlowConfig : ModuleConfig<AutoStartWoDataTraceWorkFlowConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly AutoStartWoDataTraceWorkFlowConfigValue defaultValue = new AutoStartWoDataTraceWorkFlowConfigValue
        {
        };

        /// <summary>
        /// 默认值 
        /// </summary>
        public override AutoStartWoDataTraceWorkFlowConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
