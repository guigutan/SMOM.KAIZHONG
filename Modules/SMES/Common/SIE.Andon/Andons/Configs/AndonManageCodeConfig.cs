using SIE.Common.Configs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SIE.Andon.Andons.Configs
{
    /// <summary>
    /// 安灯管理事件编码配置项
    /// </summary>
    [DisplayName("安灯管理事件编码默认值")]
    [Description("安灯管理事件编码默认值")]
    public class AndonManageCodeConfig : ModuleConfig<AndonManageCodeConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly AndonManageCodeConfigValue defaultValue = new AndonManageCodeConfigValue() { AndonManageCodeRule = null};

        /// <summary>
        /// 默认值
        /// </summary>
        public override AndonManageCodeConfigValue DefaultValue {
            get { return defaultValue; }
        }
    }
}
