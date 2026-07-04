using SIE.Common.Configs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SIE.Andon.Andons.Configs
{
    /// <summary>
    /// 推送模块默认值
    /// </summary>
    [DisplayName("推送模块默认值")]
    [Description("推送模块默认值")]
    public class AndonPushPlugConfig : ModuleConfig<AndonPushPlugConfigValue>
    {
        readonly AndonPushPlugConfigValue defaultValue = new AndonPushPlugConfigValue { PushPugDefault = null };

        /// <summary>
        /// 推送模块默认值
        /// </summary>
        public override AndonPushPlugConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
