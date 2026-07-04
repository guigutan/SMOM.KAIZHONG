using SIE.MES.Report.BatchTracebacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Report.BatchTracebacks
{
    public class BatchTracebackPreSetupViewConfig : WebViewConfig<BatchTracebackPreSetup>
    {
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.ToolCode).Show().Readonly();
                View.Property(p => p.ToolName).Show().Readonly();
                View.Property(p => p.DrawingNo).Show().Readonly();
                View.Property(p => p.ToolState).Show().Readonly();
                View.Property(p => p.UniqueCode).Show().Readonly();
                View.Property(p => p.CheckerFixtureType).Show().Readonly();
            }
        }
    }
}
