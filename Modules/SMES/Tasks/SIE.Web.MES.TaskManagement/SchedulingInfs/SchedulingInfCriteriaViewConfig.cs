using SIE.MES.TaskManagement.SchedulingInfs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.SchedulingInfs
{
    public class SchedulingInfCriteriaViewConfig : WebViewConfig<SchedulingInfCriteria>
    {
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Factory).Show();
                View.Property(p => p.WorkOrderId).Show();
                View.Property(p => p.ProcessId).Show();
                View.Property(p => p.Mrb).Show();
                View.Property(p => p.AndonLineId).Show();
                View.Property(p => p.ItemId).Show();
                View.Property(p => p.ShortDescription).Show();
                View.Property(p => p.WorkOrderUpdate).Show().UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.LastMonth);
                View.Property(p => p.IsCheck).Show();
                View.Property(p => p.InStorageDate).Show().UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.Today);
                View.Property(p => p.BeginDate).Show().UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.Today);
                View.Property(p => p.EndDate).Show().UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.Today);
                View.Property(p => p.CancelTime).Show().UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All);
                View.Property(p => p.IsSchedulingInfReturn).Show();
                View.Property(p => p.IsCancel).Show().DefaultValue(YesNo.No);
                View.Property(p => p.IsGenerateTask).Show();
            }
        }
    }
}
