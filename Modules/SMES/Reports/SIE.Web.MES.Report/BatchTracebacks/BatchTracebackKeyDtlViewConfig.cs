using SIE.MES.Report.BatchTracebacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Report.BatchTracebacks
{
    public class BatchTracebackKeyDtlViewConfig : WebViewConfig<BatchTracebackKeyDtl>
    {
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.ProcessCode).Show().Readonly();
                View.Property(p => p.SourceSn).Show().Readonly();
                View.Property(p => p.ItemLabelLot).Show().Readonly();
                View.Property(p => p.DeductedQty).Show().Readonly();
                View.Property(p => p.ShortDescription).Show().Readonly();
                View.Property(p => p.ItemCode).Show().Readonly();
                View.Property(p => p.ItemName).Show().Readonly();
                View.Property(p => p.Unit).Show().Readonly();
            }
        }
    }
}
