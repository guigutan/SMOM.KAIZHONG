using SIE.MES.TaskManagement.Reports;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.Reports
{
    public class WeightOfSamplingReportViewConfig : WebViewConfig<WeightOfSamplingReport>
    {
        protected override void ConfigView()
        {
            base.ConfigView();
            View.AssignAuthorize(typeof(WeightOfSamplingReport));
        }

        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkOrderNo).ShowInList(width:150).Readonly();
                View.Property(p => p.ProductCode).ShowInList(width:150).Readonly();
                View.Property(p => p.ProductName).ShowInList(width:300).Readonly();
                View.Property(p => p.PlanQty).Show().Readonly();
                View.Property(p => p.Weight).Show().Readonly();
                View.Property(p => p.OldWeight).Show().Readonly();
                View.Property(p => p.Unit).Show().Readonly();
            }
        }
    }
}
