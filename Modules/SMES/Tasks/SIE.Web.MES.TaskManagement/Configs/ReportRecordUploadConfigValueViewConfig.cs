using SIE.Barcodes;
using SIE.MES.TaskManagement.Configs;
using SIE.MES.TaskManagement.Reports;
using System;

namespace SIE.Web.MES.TaskManagement.Configs
{
    /// <summary>
    /// 报工记录配置值视图配置
    /// </summary>
    internal class ReportRecordUploadConfigValueViewConfig : WebViewConfig<ReportRecordUploadConfigValue>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.EnableUpload).Show();
            View.Property(p => p.UploadSteus).Show().UseListSetting(p => p.HelpInfo = "多个工序控制码使用英文逗号分隔");

        }
    }
}
