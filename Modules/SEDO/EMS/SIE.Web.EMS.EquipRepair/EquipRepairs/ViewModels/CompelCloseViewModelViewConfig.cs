using SIE.EMS.EquipRepair.EquipRepairs.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.EquipRepair.EquipRepairs.ViewModels
{
    /// <summary>
    /// 取消视图配置
    /// </summary>
    public class CompelCloseViewModelViewConfig : WebViewConfig<CompelCloseViewModel>
    {
        /// <summary>
        /// 明细视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.CloseReason).UseMemoEditor().HasLabel("关单原因".L10N() + "*");
        }
    }
}
