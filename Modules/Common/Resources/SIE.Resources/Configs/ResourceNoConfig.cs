using SIE.Common.Configs;

namespace SIE.Resources.Configs

{
    /// <summary>
    /// 资源编号配置
    /// </summary>
    public class ResourceNoConfig : ModuleConfig<ResourceNoConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        public override ResourceNoConfigValue DefaultValue
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public override string Description
        {
            get
            {
                return "用于配置资源编号";
            }
        }
    }
}
