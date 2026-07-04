using SIE.Common.Configs;
using System.ComponentModel;

namespace SIE.MES.TaskManagement.Configs
{
    /// <summary>
    /// 报工记录配置
    /// </summary>
    [DisplayName("报工记录配置")]
    [Description("用于配置报工批次号和打印模板的具体规则")]
    public class ReportRecordDetailConfig : ModuleConfig<ReportRecordConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        public override ReportRecordConfigValue DefaultValue
        {
            get { return new ReportRecordConfigValue(); }
        }
    }


}
