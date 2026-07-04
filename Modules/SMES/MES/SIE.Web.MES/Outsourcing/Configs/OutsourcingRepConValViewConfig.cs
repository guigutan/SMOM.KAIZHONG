using DevExpress.XtraReports.Web.WebDocumentViewer;
using SIE.MES.Outsourcing.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Outsourcing.Configs
{
    public class OutsourcingRepConValViewConfig:WebViewConfig<OutsourcingReportConfigValue>
    {
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.IsInAutoReport).Show();
                View.Property(p => p.IsOutsourcingInsVaildReportLog).Show();
                View.Property(p => p.IsValidOutsourcingReportLog).Show();
                View.Property(p => p.ValidStartProcess).Show();
            }
        }
    }
}
