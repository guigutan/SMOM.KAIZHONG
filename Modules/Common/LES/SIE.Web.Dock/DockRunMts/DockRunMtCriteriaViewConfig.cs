using SIE.Dock.DockRunMts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Dock.DockRunMts
{
    /// <summary>
    /// 月台运行维护查询视图配置
    /// </summary>
    internal class DockRunMtCriteriaViewConfig : WebViewConfig<DockRunMtCriteria>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show();
                View.Property(p => p.Name).Show();
            }
        }
    }
}
