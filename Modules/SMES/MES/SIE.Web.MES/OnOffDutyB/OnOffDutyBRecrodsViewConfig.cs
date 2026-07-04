using SIE.MES.OnOffDutyB;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.OnOffDutyB
{
    /// <summary>
    /// B在岗信息
    /// </summary>
    public class OnOffDutyBRecrodsViewConfig : WebViewConfig<OnOffDutyBRecrods>
    {
        /// <summary>
        /// B在岗信息
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);

            View.Property(p => p.EmployeeCode).Readonly();
            View.Property(p => p.EmployeeName).Readonly();

            View.Property(p => p.Resource).Readonly().HasLabel("资源".L10N());
            View.Property(p => p.ResourceId).Readonly();
            View.Property(p => p.ResourceName).Readonly();
            //View.Property(p => p.Process).Readonly().HasLabel("工序".L10N());
            //View.Property(p => p.Station).Readonly().HasLabel("工位".L10N());
            View.Property(p => p.IsAdditionalRecording).Readonly();
            View.Property(p => p.OnOffDutyType).Readonly();
            View.Property(p => p.OnDutyTime).Readonly();
            View.Property(p => p.OffDutyTime).Readonly();
            View.Property(p => p.OnDutyDuration).UseSpinEditor(p => p.DecimalPrecision = 2).Readonly();
        }
    }
}
