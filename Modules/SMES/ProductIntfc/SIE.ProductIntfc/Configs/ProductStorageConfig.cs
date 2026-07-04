using SIE.Common.Configs;
using System.ComponentModel;

namespace SIE.ProductIntfc.Configs
{
    /// <summary>
    /// 报检单号配置
    /// </summary>
    [DisplayName("成品入库配置")]
    [Description("用于配置成品入库单号、默认仓库")]
    public class ProductStorageConfig : ModuleConfig<ProductStorageConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly ProductStorageConfigValue defaultValue = new ProductStorageConfigValue();

        /// <summary>
        /// 默认值
        /// </summary>
        public override ProductStorageConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }
    }
}