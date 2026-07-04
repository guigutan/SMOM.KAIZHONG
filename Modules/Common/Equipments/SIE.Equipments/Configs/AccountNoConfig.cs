using SIE.Common.Configs;
using System.ComponentModel;

namespace SIE.Equipments.Configs
{
    /// <summary>
    /// 设备台账编码配置
    /// </summary>
    [DisplayName("设备台账编码配置")]
    [Description("用于配置设备台账编码的生成规则")]
    public class AccountNoConfig : ModuleConfig<AccountNoConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly AccountNoConfigValue defaultValue = new AccountNoConfigValue();

        /// <summary>
        /// 默认值
        /// </summary>
        public override AccountNoConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }
    }
}
