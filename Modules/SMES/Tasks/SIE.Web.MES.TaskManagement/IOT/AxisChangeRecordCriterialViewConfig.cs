using SIE.MES.TaskManagement.IOT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.IOT
{
    public class AxisChangeRecordCriterialViewConfig:WebViewConfig<AxisChangeRecordCriterial>
    {
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.IotEntity);
                View.Property(p => p.ChangeFlag);
                View.Property(p => p.TaskNo);
                View.Property(p => p.IsReport);
                View.Property(p => p.ResourceName);
                View.Property(p => p.ResourceCode);
                View.Property(p => p.CollectionTime).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All).ShowInList(width: 150); ;
            }
        }
    }
}
