using SIE.EMS.Report.SparePartMitReports;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.Report.SparePartMitReports
{
    /// <summary>
    /// 备件库综合统计报表
    /// </summary>
    public class SparePartMitReportViewModelViewConfig : WebViewConfig<SparePartMixtReportViewModel>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(SparePartMixtReportViewModel));
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.WithoutPaging();
        }
    }
}
