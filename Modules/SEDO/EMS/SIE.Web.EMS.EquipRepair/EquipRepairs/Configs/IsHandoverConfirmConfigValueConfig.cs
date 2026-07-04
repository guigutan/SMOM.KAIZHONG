using SIE.EMS.EquipRepair.EquipRepairs.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.EquipRepair.EquipRepairs.Configs
{
    /// <summary>
    /// 是否交机确认配置界面
    /// </summary>
    public class IsHandoverConfirmConfigValueConfig : WebViewConfig<IsHandoverConfirmConfigValue>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.IsHandoverConfirm).DefaultValue(true);
        }
    }
}
