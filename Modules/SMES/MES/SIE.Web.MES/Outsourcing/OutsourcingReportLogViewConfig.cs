using SIE.MES.Outsourcing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Outsourcing
{
    public class OutsourcingReportLogViewConfig : WebViewConfig<OutsourcingReportLog>
    {

        const int SingleCharWidth = 10;

        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.SN).ShowInList(width: SingleCharWidth * 15).Readonly();
                View.Property(p => p.LotNo).ShowInList(width: SingleCharWidth * 15).Readonly();
                View.Property(p => p.Qty).UseSpinEditor(m => m.MinValue = 0).ShowInList(width: SingleCharWidth * 8)
                    .Readonly(p => p.State == OutsourcingDetailState.Submitted);
                View.Property(p => p.ProcessingType).Show().Readonly();
                //View.Property(p => p.PassQty).UseSpinEditor(m => m.MinValue = 0).ShowInList(width: SingleCharWidth * 8)
                //    .Readonly(p => p.State == OutsourcingDetailState.Submitted);
                //View.Property(p => p.NgQty).UseSpinEditor(m => m.MinValue = 0).ShowInList(width: SingleCharWidth * 8)
                //    .Readonly(p => p.State == OutsourcingDetailState.Submitted);
                View.Property(p => p.State).ShowInList(width: SingleCharWidth * 8).Readonly();
                View.Property(p => p.ReportFactory).Show().Readonly();
            }
        }
    }
}
