using SIE.EMS.SpecialEquipment.RegularInspections.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.SpecialEquipment.RegularInspections.Configs
{
    /// <summary>
    /// 数据列视图
    /// </summary>
    internal class ValueConfigValueViewConfig : WebViewConfig<ValueConfigValue>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.FormEdit();
        }

        /// <summary>
        /// 表单视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.Qty);
        }
    }
}
