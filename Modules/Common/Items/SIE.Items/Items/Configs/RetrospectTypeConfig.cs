using SIE.Common.Configs;
using SIE.Core.Items;

namespace SIE.Items.Items.Configs
{
    /// <summary>
    /// 异常提示信息配置
    /// </summary>  
    [System.ComponentModel.DisplayName("产品追溯方式")]
    [System.ComponentModel.Description("产品追溯方式")]
    public class RetrospectTypeConfig : GlobalConfig<RetrospectTypeConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly RetrospectTypeConfigValue defaultvalue = new RetrospectTypeConfigValue() { RetrospectType = RetrospectType.Single };

        /// <summary>
        /// 获取默认值
        /// </summary>
        public override RetrospectTypeConfigValue DefaultValue
        {
            get
            {
                return defaultvalue;
            }
        }
    }
}
