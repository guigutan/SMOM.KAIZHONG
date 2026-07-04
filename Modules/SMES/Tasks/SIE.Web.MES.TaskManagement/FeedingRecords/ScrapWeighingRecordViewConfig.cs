using DevExpress.Web.Rendering;
using SIE.MES.TaskManagement.FeedingRecords;
using SIE.MetaModel.View;
using SIE.Web.MES.TaskManagement.FeedingRecords.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.FeedingRecords
{
    public class ScrapWeighingRecordViewConfig : WebViewConfig<ScrapWeighingRecord>
    {
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(ScrapWeighingRecord));
            if (ViewGroup == "EditView")
                ConfigEditView();
            base.ConfigView();
        }

        public void ConfigEditView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.ActualQty).Show();
                View.Property(p => p.Sn).Show().Readonly();
                View.Property(p => p.Lot).Show().Readonly();
                View.Property(p => p.ItemCode).Show().Readonly();
                View.Property(p => p.ItemName).Show().Readonly();
                View.Property(p => p.ItemLabelState).Show().Readonly();
                View.Property(p => p.RemainingQty).Show().Readonly();
                View.Property(p => p.DiffQty).Show().Readonly();
                View.Property(p => p.EditQty).Show().Readonly();
            }
        }

        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.UseCommands(typeof(ScrapWeighingRecordEditCommand).FullName, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
                View.Property(p => p.Sn).Show().Readonly();
                View.Property(p => p.Lot).Show().Readonly();
                View.Property(p => p.ItemCode).Show().Readonly();
                View.Property(p => p.ItemName).Show().Readonly();
                View.Property(p => p.ItemLabelState).Show().Readonly();
                View.Property(p => p.RemainingQty).Show().Readonly();
                View.Property(p => p.ActualQty).Show().Readonly();
                View.Property(p => p.DiffQty).Show().Readonly();
                View.Property(p => p.EditQty).Show().Readonly();
            }
        }
    }
}
