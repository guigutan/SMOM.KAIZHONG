using SIE.Barcodes;
using SIE.MES.TaskManagement.Configs;
using SIE.MES.TaskManagement.Reports;
using System;

namespace SIE.Web.MES.TaskManagement.Configs
{
    /// <summary>
    /// 开机准备配置值视图配置
    /// </summary>
    internal class PreStartupSetupRecordConfigValueViewConfig : WebViewConfig<PreStartupSetupRecordConfigValue>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            //View.Property(p => p.IsValidateStartupSetupPrepare).Show();
            using (View.OrderProperties())
            {
                View.Property(p => p.IsValidCheckerItem).Show();
                View.Property(p => p.IsValidEquipAccountItem).Show();
                View.Property(p => p.IsValidFixtureItem).Show();
            }
        }
    }
}
