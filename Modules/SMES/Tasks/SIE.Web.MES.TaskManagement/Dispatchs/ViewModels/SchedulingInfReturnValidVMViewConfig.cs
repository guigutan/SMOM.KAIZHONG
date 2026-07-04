using SIE.MES.TaskManagement.Dispatchs.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.Dispatchs.ViewModels
{
    public class SchedulingInfReturnValidVMViewConfig : WebViewConfig<SchedulingInfReturnValidViewModel>
    {
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.ReturnReason).Show().UseMemoEditor();
        }
    }
}
