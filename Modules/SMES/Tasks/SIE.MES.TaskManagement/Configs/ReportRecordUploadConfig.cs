using SIE.Common.Configs;
using System.ComponentModel;

namespace SIE.MES.TaskManagement.Configs
{
    /// <summary>
    /// 报工记录上传配置
    /// </summary>
    [DisplayName("报工记录上传配置")]
    [Description("用于配置报工记录上传控制参数")]
    public class ReportRecordUploadConfig : ModuleConfig<ReportRecordUploadConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        public override ReportRecordUploadConfigValue DefaultValue
        {
            get { return new ReportRecordUploadConfigValue(); }
        }
    }


}
