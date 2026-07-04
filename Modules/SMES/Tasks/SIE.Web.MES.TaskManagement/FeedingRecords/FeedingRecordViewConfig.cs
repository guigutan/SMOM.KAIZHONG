using SIE.MES.TaskManagement.FeedingRecords;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.FeedingRecords
{
    public class FeedingRecordViewConfig : WebViewConfig<FeedingRecord>
    {
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.ExportXls);
            using (View.OrderProperties())
            {
                //View.Property(p => p.WorkOrderNo).Show().Readonly();
                //View.Property(p => p.TaskNo).Show().Readonly();
                //View.Property(p => p.ProcessCode).Show().Readonly();
                View.Property(p => p.ResourceCode).ShowInList(120).Readonly();
                View.Property(p => p.ResourceName).Show().Readonly();
                View.Property(p => p.FeedingAreaCode).ShowInList(120).Readonly();
                View.Property(p => p.FeedingAreaName).ShowInList(120).Readonly();

                View.Property(p => p.ItemCode).ShowInList(150).Readonly();
                View.Property(p => p.ItemName).ShowInList(150).Readonly();
                View.Property(p => p.ShortDescription).ShowInList(150).Readonly();

                //View.Property(p => p.Label).Show().Readonly();
                View.Property(p => p.FeedingItemLabel).ShowInList(150).Readonly();
                View.Property(p => p.ItemLabelLot).Show().Readonly();
                View.Property(p => p.FeedingQty).Show().Readonly();
                View.Property(p => p.DeductedQty).Show().Readonly();
                View.Property(p => p.BlankingQty).Show().Readonly();
                View.Property(p => p.RemainingQty).Show().Readonly();
            }
        }
    }
}
