using SIE.Dock.YardMaintains;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Dock.YardMaintains
{
    /// <summary>
    /// 园区维护查询视图配置
    /// </summary>
    internal class YardMaintainCriteriaViewConfig : WebViewConfig<YardMaintainCriteria>
    {
        /// <summary>
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
