using SIE.EMS.Report.SparePartMitReports;
using SIE.MetaModel.View;
using SIE.Web.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.Report.SparePartMitReports
{
    /// <summary>
    /// 备件库综合统计查询视图
    /// </summary>
    public class SparePartMitReportViewModelCriteriaViewConfig : WebViewConfig<SparePartMixReportViewModelCriteria>
    {
        /// <summary>
        /// 配置查询列表
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.UseDefaultCommands();
            View.ReplaceCommands(WebCommandNames.ExecuteQuery, "SIE.Web.EMS.Report.MttrAndMtbfReports.Commands.MttrAndMtbfReportQuery");
            View.RemoveCommands(WebCommandNames.ClearQuery);
            View.Property(p => p.Warehouse).HasLabel("仓库".L10N()+"*");
            View.Property(p => p.BeginMonth).UseYearMonthEditor().DefaultValue(DateTime.Now).HasLabel("开始月份");
            View.Property(p => p.EndMonth).UseYearMonthEditor().DefaultValue(DateTime.Now).HasLabel("结束月份");
        }
    }
}
