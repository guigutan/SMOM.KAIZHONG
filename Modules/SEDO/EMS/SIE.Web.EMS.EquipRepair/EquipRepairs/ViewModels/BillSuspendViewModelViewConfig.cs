using SIE.EMS.EquipRepair.EquipRepairs.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.EquipRepair.EquipRepairs.ViewModels
{
    /// <summary>
    /// 维修暂停视图
    /// </summary>
   public class BillSuspendViewModelViewConfig : WebViewConfig<BillSuspendViewModel>
    {
        /// <summary>
        /// 明细视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p=>p.SuspendReason).UseMemoEditor().HasLabel("暂停原因".L10N() + "*");
        }
    }
}
