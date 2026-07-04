using SIE.Common.Configs;
using SIE.ObjectModel;
using SIE.ProductIntfc.OutputProducts;
using System.ComponentModel;

namespace SIE.ProductIntfc.Configs
{
    /// <summary>
    /// 联/副产品入库配置
    /// </summary>
    [DisplayName("联/副产品入库配置")]
    [Description("用于配置联/副产品入库单号、默认仓库")]
    [ConfigForEntity(typeof(OutputProduct))]
    public class OutputProductsConfig : ModuleConfig<OutputProductsConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly OutputProductsConfigValue defaultValue = new OutputProductsConfigValue();

        /// <summary>
        /// 默认值
        /// </summary>
        public override OutputProductsConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }
    }
}