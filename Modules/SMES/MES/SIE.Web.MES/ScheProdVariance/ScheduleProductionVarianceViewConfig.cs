using DevExpress.DataProcessing;
using SIE.MES.ItemCPN;
using SIE.MES.ScheProdVariance;
using SIE.MetaModel.View;
using SIE.Web.MES.LineAndon.Commands;
using SIE.Web.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ScheProdVariance
{
    public class ScheduleProductionVarianceViewConfig : WebViewConfig<ScheduleProductionVariance>
    {
        protected override void ConfigView()
        {
            View.ClearCommands();
        }

        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.ExportXls);
            using (View.OrderProperties())
            {
                View.Property(p => p.ProcessCode).ShowInList(width: 150);
                View.Property(p => p.ItemCode).ShowInList(width: 150);
                View.Property(p => p.ItemName).ShowInList(width: 200);
                View.Property(p => p.OldItem).ShowInList(width: 150);
                View.Property(p => p.WoNo).ShowInList(width: 150);
                View.Property(p => p.No).ShowInList(width: 150);
                View.Property(p => p.ShiftType).ShowInList(width: 150);
                View.Property(p => p.TotalShiftValue).ShowInList(width: 150);
                View.Property(p => p.ReportQty).ShowInList(width: 150);
                View.Property(p => p.NowDay).ShowInList(width: 150);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            }
        }
    }
}