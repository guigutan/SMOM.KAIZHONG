using SIE.MES.TaskManagement.PreStartupSetupRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.PreStartupSetupRecords
{
    public class PreStartupSetupRecordCriteriaViewConfig : WebViewConfig<PreStartupSetupRecordCriteria>
    {
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkOrderNo).Show();
                View.Property(p => p.TaskNo).Show();
                View.Property(p => p.Process).Show();
                View.Property(p => p.ResourceCode).Show();
                View.Property(p => p.ResourceName).Show();
                View.Property(p => p.ItemCode).Show();
                View.Property(p => p.ItemName).Show();
                View.Property(p => p.ShortDescription).Show();
                View.Property(p => p.ToolCode).Show();
                View.Property(p => p.ToolName).Show();
                View.Property(p => p.DrawingNo).Show();
                View.Property(p => p.CheckerFixtureType).Show();
                View.Property(p => p.CreateDate).Show().UseDateRangeEditor(p =>
                {
                    p.DateRangeType = ObjectModel.DateRangeType.LastMonth;
                });
            }
        }
    }
}
