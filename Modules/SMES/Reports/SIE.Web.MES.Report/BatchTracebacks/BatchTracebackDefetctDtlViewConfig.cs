using SIE.MES.Report.BatchTracebacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Report.BatchTracebacks
{
    public class BatchTracebackDefetctDtlViewConfig : WebViewConfig<BatchTracebackDefetctDtl>
    {
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.DefectCode).Show().Readonly();
                View.Property(p => p.DefectDesc).Show().Readonly();
            }
        }
    }
}
