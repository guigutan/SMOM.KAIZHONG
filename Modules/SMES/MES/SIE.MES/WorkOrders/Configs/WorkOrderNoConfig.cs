using SIE.Common.Configs;

namespace SIE.MES.WorkOrders.Configs
{
    /// <summary>
    /// 工单编号配置
    /// </summary>
    [System.ComponentModel.DisplayName("工单号生成规则")]
    [System.ComponentModel.Description("用于选择工单号生成的具体规则,具体规则详细请在条码规则进行配置")]
    public class WorkOrderNoConfig : ModuleConfig<WorkOrderNoConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly WorkOrderNoConfigValue defaultValue = new WorkOrderNoConfigValue { BacodeRule = null };

        /// <summary>
        /// 默认值 
        /// </summary>
        public override WorkOrderNoConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
