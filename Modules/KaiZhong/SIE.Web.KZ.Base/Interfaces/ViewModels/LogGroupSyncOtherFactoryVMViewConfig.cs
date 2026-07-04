using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.KZ.Base.Interfaces.ViewModels
{
    public class LogGroupSyncOtherFactoryVMViewConfig : WebViewConfig<LogGroupSyncOtherFactoryViewModel>
    {
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(InfNcDataLogGroup));
        }

        protected override void ConfigDetailsView()
        {
            View.Property(p => p.Factory).Show();
        }
    }
}
