using SIE.MES.PackingQC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.PackingQC
{
    public class PackingReportRecordViewConfig : WebViewConfig<PackingReportRecord>
    {
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.BlueLabel).Readonly().ShowInList(width: 300);
                View.Property(p => p.Report).Readonly().ShowInList(width: 300);
                View.Property(p => p.ReturnMessage).Readonly().ShowInList(width: 500);
                View.Property(p => p.BeginDate).Readonly().ShowInList(width: 300);
                View.Property(p => p.EndDate).Readonly().ShowInList(width: 300);
            }
        }
    }
}
