using SIE.Common.Configs;

namespace SIE.CSM.Suppliers.Configs
{
    /// <summary>
    /// 是否自动创建供应商用户配置
    /// </summary>
    [System.ComponentModel.DisplayName("是否自动创建供应商用户配置")]
    [System.ComponentModel.Description("是否自动创建供应商用户配置")]
    public class SupplierIsAutoCreateConfig : ModuleConfig<SupplierIsAutoCreateConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        private readonly SupplierIsAutoCreateConfigValue defaultValue = new SupplierIsAutoCreateConfigValue { IsAutoCreate = false };

        /// <summary>
        /// 默认值
        /// </summary>
        public override SupplierIsAutoCreateConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
