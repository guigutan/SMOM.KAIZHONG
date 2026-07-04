using SIE.EMS.EquipRepair.EquipRepairs.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.EquipRepair.EquipRepairs.ViewModels
{
    /// <summary>
    /// 维修开始视图配置
    /// </summary>
    public class RepairStartViewModelViewConfig:WebViewConfig<RepairStartViewModel>
    {
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p=>p.IsStopMachineRepair).DefaultValue(false);
        }
    }
}
