using SIE.EMS.Maintains.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.EquipMaint.Maintains.Configs
{
    /// <summary>
    /// 配置值视图
    /// </summary>
    public class MaintainAlertTimeConfigValueViewConfig : WebViewConfig<MaintainAlertTimeConfigValue>
    {
        /// <summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.AlertTime).UseSpinEditor(p => { p.MinValue = 1; });
            View.Property(p => p.ExpiredTime).UseSpinEditor(p => { p.MinValue = 1; });
            View.Property(p => p.MaintainConfirmExpiredTime).Show(ShowInWhere.Hide);
        }
    }
}
