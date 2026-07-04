using SIE.Common.Configs;
using SIE.ESop.Displays;
using System.ComponentModel;

namespace SIE.ESop.Configs
{
    /// <summary>
    /// 文档服务器配置
    /// </summary>
    [DisplayName("显示点配置")]
    [Description("显示点配置")]
    public partial class DisplayConfig : ModuleConfig<DisplayPointConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly DisplayPointConfigValue defaultValue = new DisplayPointConfigValue { Interval = 30, ReTryCount = 3, ModifierKeys = ModifierKeys.Control | ModifierKeys.Alt, Key = Key.F2 };

        /// <summary>
        /// 默认值
        /// </summary>
        public override DisplayPointConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }
    }
}