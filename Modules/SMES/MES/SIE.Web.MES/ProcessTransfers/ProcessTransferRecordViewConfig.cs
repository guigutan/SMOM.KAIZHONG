using SIE.MES.ProcessTransfers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.MES.ProcessTransfers
{
    internal class ProcessTransferRecordViewConfig : WebViewConfig<ProcessTransferRecord>
    {
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkOrderNo);
                View.Property(p => p.ProCode);
                View.Property(p => p.ProName);
                View.Property(p => p.Barcode);
                View.Property(p => p.BarcodeType);
                View.Property(p => p.Resource);
                View.Property(p => p.Process);
                View.Property(p => p.Type);
                View.Property(p => p.Qty);
                View.Property(p => p.OperateBy);
                View.Property(p => p.OperateTime);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            }
        }
        protected override void ConfigQueryView()
        {
            View.Property(p => p.WorkOrder);
            View.Property(p => p.Barcode);
            View.Property(p => p.Resource);
            View.Property(p => p.Process);
            View.Property(p => p.Type);
            View.Property(p => p.OperateBy);
            View.Property(p => p.OperateTime).UseDateRangeEditor();
        }
    }
}
