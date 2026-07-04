using SIE.EMS.EquipRepair.EquipRepairs.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.EquipRepair.EquipRepairs.Configs
{

    /// <summary>
    /// 维修单号配置界面
    /// </summary>
    public class EquipRepairNoConfigValueConfig : WebViewConfig<EquipRepairNoConfigValue>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.NumberRule);
        }
    }
}
