using DevExpress.DataProcessing;
using SIE.MES.ItemCPN;
using SIE.MES.ScheProdVariance;
using SIE.MetaModel.View;
using SIE.Web.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ScheProdVariance
{
    public class ScheduleProductionVarianceCriteriaViewConfig : WebViewConfig<ScheduleProductionVarianceCriteria>
    {
        protected override void ConfigQueryView()
        {
            //View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Save, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll, typeof(LineAreaImportCommand).FullName, typeof(LineAreaDLTemplateCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.BeginDate).UseDateRangeEditor(p =>
                {
                    p.DateRangeType = ObjectModel.DateRangeType.Custom;
                    p.StartDate = DateTime.Today.AddDays(-1);
                    p.EndDate = DateTime.Today.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
                });
            }
        }
    }
}
