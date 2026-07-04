using SIE.Common.Configs;
using System.ComponentModel;

namespace SIE.MES.TaskManagement.Configs
{
    /// <summary>
    /// 开机准备配置
    /// </summary>
    [DisplayName("开机准备配置")]
    [Description("用于配置开机准备配置的具体规则")]
    public class PreStartupSetupRecordConfig : ModuleConfig<PreStartupSetupRecordConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        public override PreStartupSetupRecordConfigValue DefaultValue
        {
            get { return new PreStartupSetupRecordConfigValue(); }
        }
    }


}
