using SIE.WorkBenchCommon.Workbench.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.WorkBenchCommon.Workbench.Base
{
    /// <summary>
    /// 发布显视模式ViewConfig
    /// </summary>
    public class PublishViewModelViewConfig : WebViewConfig<PublishViewModel>
    {
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.DisplayMode).HasLabel("显视模式").Show();
        }
    }
}
