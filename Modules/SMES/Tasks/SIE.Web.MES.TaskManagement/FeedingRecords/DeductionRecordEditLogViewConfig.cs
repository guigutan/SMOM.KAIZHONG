using SIE.MES.TaskManagement.FeedingRecords;
using SIE.MES.TaskManagement.Reports;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.FeedingRecords
{
    public class DeductionRecordEditLogViewConfig : WebViewConfig<DeductionRecordEditLog>
    {
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(ReportRecordExamine));
            base.ConfigView();
        }

        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.FeedingItemLabel).Show().Readonly();
                View.Property(p => p.ItemLabelLot).Show().Readonly();
                View.Property(p => p.OldEditQty).Show().Readonly();
                View.Property(p => p.NewEditQty).Show().Readonly();
            }
        }
    }
}
