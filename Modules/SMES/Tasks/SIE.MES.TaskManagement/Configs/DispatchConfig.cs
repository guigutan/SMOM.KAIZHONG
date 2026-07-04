using SIE.Common.Configs;
using System.ComponentModel;

namespace SIE.MES.TaskManagement.Configs
{
    /// <summary>
    /// 派工任务单配置
    /// </summary>
    [DisplayName("派工任务单配置")]
    [Description("用于派工任务单的具体规则")]
    public class DispatchConfig : ModuleConfig<DispatchConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        public override DispatchConfigValue DefaultValue
        {
            get { return new DispatchConfigValue(); }
        }
    }
}
