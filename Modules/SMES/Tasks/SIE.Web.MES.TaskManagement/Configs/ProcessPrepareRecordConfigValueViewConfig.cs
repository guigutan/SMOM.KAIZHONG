using SIE.Barcodes;
using SIE.MES.TaskManagement.Configs;
using SIE.MES.TaskManagement.Reports;
using System;

namespace SIE.Web.MES.TaskManagement.Configs
{
    /// <summary>
    /// 工序产前准备配置值视图配置
    /// </summary>
    internal class ProcessPrepareRecordConfigValueViewConfig : WebViewConfig<ProcessPrepareRecordConfigValue>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.IsValidateProcessPrepare).Show();

        }
    }
}
