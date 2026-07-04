using SIE.Common.Configs;

namespace SIE.Warehouses.Configs
{
    /// <summary>
    /// 库位编码配置
    /// </summary>
    [System.ComponentModel.DisplayName("库位编码规则")]
    [System.ComponentModel.Description("用于生成库位编码的具体规则,具体规则详细请在编码规则进行配置")]
    public class StorageLocationCodeConfig : ModuleConfig<StorageLocationCodeConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly StorageLocationCodeConfigValue defaultValue = new StorageLocationCodeConfigValue { StorageLocationCodeRule = null, StorageLocationPrintRule = null };

        /// <summary>
        /// 默认值
        /// </summary>
        public override StorageLocationCodeConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
