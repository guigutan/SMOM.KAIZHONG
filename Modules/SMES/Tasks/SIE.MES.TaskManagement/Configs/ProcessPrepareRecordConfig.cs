using SIE.Common.Configs;
using System.ComponentModel;

namespace SIE.MES.TaskManagement.Configs
{
    /// <summary>
    /// 工序产前准备配置
    /// </summary>
    [DisplayName("工序产前准备配置")]
    [Description("用于配置工序产前准备的具体规则")]
    public class ProcessPrepareRecordConfig : ModuleConfig<ProcessPrepareRecordConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        public override ProcessPrepareRecordConfigValue DefaultValue
        {
            get { return new ProcessPrepareRecordConfigValue(); }
        }
    }


}
