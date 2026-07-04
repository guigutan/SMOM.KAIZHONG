using SIE.Common.Configs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SIE.Andon.Andons.Configs
{
    /// <summary>
    /// 安灯类型推送模块配置项
    /// </summary>
    [DisplayName("推送模块默认值")]
    [Description("推送模块默认值")]
    public class AndonTypePushPlugConfig: ModuleConfig<AndonTypePushPlugConfigValue>
    {
        readonly AndonTypePushPlugConfigValue defaultValue = new AndonTypePushPlugConfigValue { PushPugDefault = null};

        /// <summary>
        /// 
        /// </summary>
        public override AndonTypePushPlugConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
