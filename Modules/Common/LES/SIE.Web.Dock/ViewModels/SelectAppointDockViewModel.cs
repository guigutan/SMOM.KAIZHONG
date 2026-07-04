using SIE.Dock.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Dock.ViewModels
{
    /// <summary>
    /// 选择预约月台ViewModel视图配置
    /// </summary>
    public class SelectAppointDockViewModelViewConfig : WebViewConfig<SelectAppointDockViewModel>
    {
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.Property(p => p.AppointTimeDisplay).ShowInList(width: 240);
        }

        /// <summary>
        /// 下拉视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.AppointTimeDisplay).ShowInList(width: 240);
        }
    }
}