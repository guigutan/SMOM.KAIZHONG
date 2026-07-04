using SIE.Common.Configs;

namespace SIE.MES.WorkOrders.Configs
{
    /// <summary>
    /// 工序 BOM 参考工单 BOM 配置
    /// </summary>
    [System.ComponentModel.DisplayName("工序 BOM 参考工单 BOM 配置")]
    [System.ComponentModel.Description("工序 BOM 参考工单 BOM 配置")]
    public class ReferenceWoBomConfig : ModuleConfig<ReferenceWoBomConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly ReferenceWoBomConfigValue defaultValue = new ReferenceWoBomConfigValue
        {
            ReferenceWoBom = true
        };

        /// <summary>
        /// 默认值 
        /// </summary>
        public override ReferenceWoBomConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
