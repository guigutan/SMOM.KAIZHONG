using SIE.Dock.YardMaintains;
using SIE.Dock.YardMaintains.Service;
using SIE.Dock.YardZones;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Dock.YardZones
{
    /// <summary>
    /// 园片区维护查询视图配置
    /// </summary>
    internal class YardZoneCriteriaViewConfig : WebViewConfig<YardZoneCriteria>
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
                View.Property(p => p.YardMaintainNameId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<YardMaintainService>().GetYardMaintainList(keyword, pagingInfo);
                }).Show();
            }
        }
    }
}
