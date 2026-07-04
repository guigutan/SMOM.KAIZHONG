using SIE.Common.Configs;

namespace SIE.Kit.UrgentOrder.ItemUrgentOrders.Configs
{
    /// <summary>
    /// 物料加急单号配置
    /// </summary>
    [System.ComponentModel.DisplayName("物料加急单号生成规则")]
    [System.ComponentModel.Description("用于物料加急单号生成的具体规则,具体规则详细请在编码规则进行配置")]
    public class ItemUrgentOrderNoConfig : ModuleConfig<ItemUrgentOrderNoConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        ItemUrgentOrderNoConfigValue defaultValue = new ItemUrgentOrderNoConfigValue();

        /// <summary>
        /// 默认值
        /// </summary>
        public override ItemUrgentOrderNoConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public override string Description
        {
            get
            {
                return "用于配置物料加急单号生成规则";
            }
        }
    }
}
