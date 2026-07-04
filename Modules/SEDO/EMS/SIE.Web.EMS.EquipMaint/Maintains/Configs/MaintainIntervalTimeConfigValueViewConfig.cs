using SIE.EMS.Maintains.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.EquipMaint.Maintains.Configs
{
    /// <summary>
    /// 配置值视图
    /// </summary>
    public class MaintainIntervalTimeConfigValueViewConfig : WebViewConfig<MaintainIntervalTimeConfigValue>
    {
        /// <summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.IsIntervalTime).Cascade(p => p.IntervalTime, null);
            View.Property(p => p.IntervalTime).UseSpinEditor(p => { p.MinValue = 1; p.MaxValue = 6; }).Readonly(p => !p.IsIntervalTime);
        }
    }
}
