using SIE.MES.OnOffDuty;
using SIE.MetaModel.View;
using System;

namespace SIE.Web.MES.OnOffDuty
{
    public class OnOffDutyRecrodsViewConfig : WebViewConfig<OnOffDutyRecrods>
    {
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);

            View.Property(p => p.EmployeeCode).Readonly();
            View.Property(p => p.EmployeeName).Readonly();

            View.Property(p => p.Resource).Readonly().HasLabel("资源".L10N());
            View.Property(p => p.Process).Readonly().HasLabel("工序".L10N());
            View.Property(p => p.Station).Readonly().HasLabel("工位".L10N());
            View.Property(p => p.IsAdditionalRecording).Readonly();
            View.Property(p => p.OnOffDutyType).Readonly();
            View.Property(p => p.OnDutyTime).Readonly();
            View.Property(p => p.OffDutyTime).Readonly();
            View.Property(p => p.OnDutyDuration).UseSpinEditor(p => p.DecimalPrecision = 2).Readonly();
        }
    }
}
