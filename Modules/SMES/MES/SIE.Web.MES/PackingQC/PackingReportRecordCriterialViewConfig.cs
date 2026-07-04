using SIE.MES.PackingQC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.PackingQC
{
    public class PackingReportRecordCriterialViewConfig:WebViewConfig<PackingReportRecordCriterial>
    {
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.BlueLabel).ShowInList(width: 150);
                View.Property(p => p.Report).ShowInList(width: 150);
            }
        }
    }
}
