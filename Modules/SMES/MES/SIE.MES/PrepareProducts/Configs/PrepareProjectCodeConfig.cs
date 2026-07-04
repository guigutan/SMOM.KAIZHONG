using SIE.Common.Configs;
using System.ComponentModel;

namespace SIE.MES.PrepareProducts.Configs
{
    /// <summary>
    /// 产前准备项目维护实体编码配置项
    /// </summary>
    [DisplayName("产前准备项目维护实体编码配置项")]
    [Description("产前准备项目维护实体编码配置项")]
    public class PrepareProjectCodeConfig : ModuleConfig<PrepareProjectCodeConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly PrepareProjectCodeConfigValue defaultValue = new PrepareProjectCodeConfigValue() { ProCodeRule = null };

        /// <summary>
        /// 默认值
        /// </summary>
        public override PrepareProjectCodeConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
