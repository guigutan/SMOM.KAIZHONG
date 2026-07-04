using SIE.Packages.ItemLabels.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Packages.ItemLabels
{
    /// <summary>
    /// 视图配置
    /// </summary>
    public class NotSerLabelNumControlConfigViewConfig:WebViewConfig<NotSerLabelNumControlConfigValue>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.IsControlCount).ShowInDetail();
        }
    }
}
