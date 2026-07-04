using SIE.EMS.Maintains.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.EquipMaint.Maintains.Configs
{
    /// <summary>
    /// 配置值视图
    /// </summary>
    public class MaintainWorkTimeConfigValueViewConfig : WebViewConfig<MaintainWorkTimeConfigValue>
    {
        /// <summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.IsMaintainForWorkTime);
        }
    }
}
