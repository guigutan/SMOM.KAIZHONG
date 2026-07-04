using SIE.Common.Configs;

namespace SIE.Kit.UrgentOrder.ItemUrgentOrders.Configs
{
    /// <summary>
    /// 配置需求时间和创建加急单时间，最小间隔时数(小时)
    /// </summary>
    [System.ComponentModel.DisplayName("物料加急单配置时间项")]
    [System.ComponentModel.Description("配置需求时间和创建加急单时间，最小间隔时数(小时)")]
    public class ItemUrgentOrderDateConfig : ModuleConfig<ItemUrgentOrderDateConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        ItemUrgentOrderDateConfigValue defaultValue = new ItemUrgentOrderDateConfigValue() { Time = 0 };

        /// <summary>
        /// 默认值
        /// </summary>
        public override ItemUrgentOrderDateConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }
    }
}
