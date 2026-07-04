using SIE.MES.TaskManagement.StockDeducRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.StockDeducRecords
{
    public class StockDeducRecordViewConfig : WebViewConfig<StockDeducRecord>
    {
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(StockDeducRecord));
            base.ConfigView();
        }

        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkOrderNo).Show().Readonly();
                View.Property(p => p.TaskNo).Show().Readonly();
                View.Property(p => p.ResourceName).Show().Readonly();
                View.Property(p => p.ProductCode).Show().Readonly();
                View.Property(p => p.ProductName).Show().Readonly();
                View.Property(p => p.BatchNo).Show().Readonly();
                View.Property(p => p.BatchQty).Show().Readonly();
                View.Property(p => p.ProcessCode).Show().Readonly();
                View.Property(p => p.ProcessName).Show().Readonly();
                View.Property(p => p.ItemCode).Show().Readonly();
                View.Property(p => p.ItemName).Show().Readonly();
                View.Property(p => p.ItemShortDescription).Show().Readonly();
                View.Property(p => p.DeductedQty).Show().Readonly();
            }
        }
    }
}
