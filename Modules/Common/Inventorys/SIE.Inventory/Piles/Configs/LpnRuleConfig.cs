using SIE.Common.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Inventory.Piles.Configs
{
    /// <summary>
    /// LPN编码规则配置
    /// </summary>
    [System.ComponentModel.DisplayName("移动端码盘创建LPN格式验证")]
    [System.ComponentModel.Description("用于移动端码盘创建LPN格式进行验证")]
    public class LpnRuleConfig : ModuleConfig<LpnRuleConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        private readonly LpnRuleConfigValue defaultValue = new LpnRuleConfigValue {};

        /// <summary>
        /// 默认值
        /// </summary>
        public override LpnRuleConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
