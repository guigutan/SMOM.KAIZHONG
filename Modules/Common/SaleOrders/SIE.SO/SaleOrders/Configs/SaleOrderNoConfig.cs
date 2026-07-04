using SIE.Common.Configs;

namespace SIE.SO.SaleOrders.Configs
{
    /// <summary>
    /// 销售订单编码配置
    /// </summary>
    [System.ComponentModel.DisplayName("销售订单编码生成规则")]
    [System.ComponentModel.Description("用于销售订单编码生成的具体规则,具体规则详细请在编码规则进行配置")]
    public class SaleOrderNoConfig : ModuleConfig<SaleOrderNoConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly SaleOrderNoConfigValue defaultValue = new SaleOrderNoConfigValue();

        /// <summary>
        /// 默认值
        /// </summary>
        public override SaleOrderNoConfigValue DefaultValue
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
                return "用于配置销售订单编码生成规则";
            }
        }
    }

}
