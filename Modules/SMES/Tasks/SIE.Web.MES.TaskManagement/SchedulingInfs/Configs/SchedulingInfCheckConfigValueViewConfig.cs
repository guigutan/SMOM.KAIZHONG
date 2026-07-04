using SIE.MES.TaskManagement.SchedulingInfs.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.SchedulingInfs.Configs
{
    public class SchedulingInfCheckConfigValueViewConfig : WebViewConfig<SchedulingInfCheckConfigValue>
    {
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.PersonnelSkills).Show();
                View.Property(p => p.LineState).Show();
                View.Property(p => p.MoldState).Show();
                View.Property(p => p.ToolingState).Show();
                View.Property(p => p.InspEquipState).Show();
                View.Property(p => p.ItemComplete).Show();
                View.Property(p => p.DispatchNumber).Show();
            }
        }
    }
}
