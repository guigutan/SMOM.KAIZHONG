using SIE.Common.Configs;
using SIE.Common.Configs.Module;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ProductIntfc.InspLogs.Configs
{
    /// <summary>
    /// 成品报检是否传QMS配置
    /// </summary>
    [DisplayName("成品报检是否传QMS配置")]
    [Description("成品报检是否传QMS配置")]
    public class InspLogCallConfig : ModuleConfig<InspLogCallConfigValue>
    {
        readonly InspLogCallConfigValue defaultValue = new InspLogCallConfigValue { IsCall = true };

        /// <summary>
        /// 默认值
        /// </summary>
        public override InspLogCallConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
