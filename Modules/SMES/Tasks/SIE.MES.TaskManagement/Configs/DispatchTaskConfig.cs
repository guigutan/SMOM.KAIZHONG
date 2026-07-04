using SIE.Common.Configs;
using System.ComponentModel;

namespace SIE.MES.TaskManagement.Configs
{
    /// <summary>
    /// 派工任务单生成配置
    /// </summary>
    [DisplayName("派工任务单生成配置")]
    [Description("用于派工任务单生成配置具体规则")]
    public class DispatchTaskConfig : GlobalConfig<DispatchTaskConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        public override DispatchTaskConfigValue DefaultValue
        {
            get { return new DispatchTaskConfigValue(); }
        }
    }
}