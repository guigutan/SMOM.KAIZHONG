using SIE.Common.Configs;

namespace SIE.CSM.Suppliers.Configs
{
    /// <summary>
    /// 委外业务参数配置项
    /// </summary>
    [System.ComponentModel.DisplayName("委外业务参数配置项")]
    [System.ComponentModel.Description("委外业务参数配置项")]
    public class SupplierOutsourcingConfig : ModuleConfig<SupplierOutsourcingConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        private readonly SupplierOutsourcingConfigValue defaultValue = new SupplierOutsourcingConfigValue { IsHasStorer = false,OutsourcingReceive = OutsourcingReceiveType.UseAllHand,OutsourcingUseTime = OutsourcingTimeType.Receipt};

        /// <summary>
        /// 默认值
        /// </summary>
        public override SupplierOutsourcingConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
