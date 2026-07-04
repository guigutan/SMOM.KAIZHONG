using SIE.MES.TaskManagement.StockDeducRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.StockDeducRecords
{
    public class StockDeducRecordDetailViewConfig : WebViewConfig<StockDeducRecordDetail>
    {
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.ItemLabel).Show().Readonly();
                View.Property(p => p.DeductedQty).Show().Readonly();
                View.Property(p => p.ItemCode).Show().Readonly();
                View.Property(p => p.ItemName).Show().Readonly();
            }
        }
    }
}
