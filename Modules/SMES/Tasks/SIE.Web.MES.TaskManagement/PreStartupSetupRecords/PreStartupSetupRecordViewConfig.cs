using SIE.MES.TaskManagement.PreStartupSetupRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.PreStartupSetupRecords
{
    public class PreStartupSetupRecordViewConfig : WebViewConfig<PreStartupSetupRecord>
    {
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkOrderNo).Show().Readonly();
                View.Property(p => p.TaskNo).Show().Readonly();
                View.Property(p => p.ProcessCode).Show().Readonly();
                View.Property(p => p.ProcessName).Show().Readonly();
                View.Property(p => p.ResourceCode).Show().Readonly();
                View.Property(p => p.ResourceName).Show().Readonly();
                View.Property(p => p.ItemCode).Show().Readonly();
                View.Property(p => p.ItemName).Show().Readonly();
                View.Property(p => p.ShortDescription).Show().Readonly();
                View.Property(p => p.ToolState).Show().Readonly();
                View.Property(p => p.ToolCode).Show().Readonly();
                View.Property(p => p.ToolName).Show().Readonly();
                View.Property(p => p.DrawingNo).Show().Readonly();
                View.Property(p => p.CheckerFixtureType).Show().Readonly();
                View.Property(p => p.Qty).Show().Readonly();
                View.Property(p => p.UniqueCode).Show().Readonly();
            }
        }
    }
}
