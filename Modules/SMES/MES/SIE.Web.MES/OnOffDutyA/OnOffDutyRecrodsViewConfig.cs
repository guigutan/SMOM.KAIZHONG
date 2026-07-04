using DocumentFormat.OpenXml.Wordprocessing;
using SIE.Domain;
using SIE.MES.OnOffDutyA;
using SIE.MetaModel.View;
using System;

namespace SIE.Web.MES.OnOffDutyA
{
    public class OnOffDutyRecrodsViewConfig : WebViewConfig<OnOffDutyRecrodsA>
    {
        protected override void ConfigListView()
        {
            //View.UseDefaultCommands();
            View.UseCommands(WebCommandNames.Edit, WebCommandNames.Save);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);

          


            View.Property(p => p.EmployeeCode).Readonly();
            View.Property(p => p.EmployeeName).Readonly();

            View.Property(p => p.IsAdditionalRecording).Readonly();
            View.Property(p => p.OnOffDutyType).Readonly();
            View.Property(p => p.OnDutyTime).ShowInList(width: 200);
            View.Property(p => p.OffDutyTime).ShowInList(width: 200);


            //View.Property(p => p.OnDutyDuration).UseSpinEditor(p => p.DecimalPrecision = 2).Readonly();
           
            View.Property(p => p.OnDutyDuration).UseSpinEditor(p => p.DecimalPrecision = 2).Readonly();



        }
    }
}
