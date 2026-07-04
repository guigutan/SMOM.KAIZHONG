using SIE.Common.Configs;

namespace SIE.Warehouses.Configs
{
    /// <summary>
    /// 库区编码配置
    /// </summary>
    [System.ComponentModel.DisplayName("库区编码规则")]
    [System.ComponentModel.Description("用于配置库区编码规则")]
    public class StorageAreaCodeConfig : ModuleConfig<StorageAreaCodeConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        private readonly StorageAreaCodeConfigValue defaultValue = new StorageAreaCodeConfigValue { };

        /// <summary>
        /// 默认值
        /// </summary>
        public override StorageAreaCodeConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
