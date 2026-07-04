using SIE.EMS.Report.MttrAndMtbfReports;
using SIE.Web.EMS.Report.MttrAndMtbfReports.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.Report.MttrAndMtbfReports
{
    /// <summary>
    /// MTTR/MTBF统计报表明细
    /// </summary>
    public class MttrAndMtbfReportViewModelViewConfig : WebViewConfig<MttrAndMtbfReportViewModel>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(MttrAndMtbfReportViewModel));
            View.UseCommand(typeof(MttrAndMtbfReportExportCommand).FullName);
        }
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.WithoutPaging();
            View.UseClientOrder();
            View.DisableEditing();
            View.Property(p => p.StatiItem).ShowInList(width: 150).HasLabel("");
            View.Property(p => p.One).ShowInList(width: 70);
            View.Property(p => p.Two).ShowInList(width: 70);
            View.Property(p => p.Three).ShowInList(width: 70);
            View.Property(p => p.Four).ShowInList(width: 70);
            View.Property(p => p.Five).ShowInList(width: 70);
            View.Property(p => p.Six).ShowInList(width: 70);
            View.Property(p => p.Seven).ShowInList(width: 70);
            View.Property(p => p.Eight).ShowInList(width: 70);
            View.Property(p => p.Nine).ShowInList(width: 70);
            View.Property(p => p.Ten).ShowInList(width: 70);
            View.Property(p => p.Eleven).ShowInList(width: 70);
            View.Property(p => p.Twelve).ShowInList(width: 70);
        }
    }
}
