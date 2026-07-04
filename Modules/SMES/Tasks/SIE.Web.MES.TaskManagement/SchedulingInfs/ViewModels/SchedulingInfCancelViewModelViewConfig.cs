using SIE.MES.TaskManagement.SchedulingInfs.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.SchedulingInfs.ViewModels
{
    public class SchedulingInfCancelViewModelViewConfig : WebViewConfig<SchedulingInfCancelViewModel>
    {
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.Reason).Show().UseMemoEditor();
        }
    }
}
