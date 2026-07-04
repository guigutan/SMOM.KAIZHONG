using SIE.Common.Configs;

namespace SIE.Warehouses.Configs
{
    /// <summary>
    /// 仓库编码配置
    /// </summary>
    [System.ComponentModel.DisplayName("仓库编码规则")]
    [System.ComponentModel.Description("用于配置仓库编码规则")]
    public class WarehousesCodeConfig : ModuleConfig<WarehousesCodeConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly WarehousesCodeConfigValue defaultValue = new WarehousesCodeConfigValue { };

        /// <summary>
        /// 默认值
        /// </summary>
        public override WarehousesCodeConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
