using SIE.Andon.Andons;
using SIE.Andon.Andons.ViewModels;
using SIE.Rbac.Users;
using SIE.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Andon.Andons
{
    public class UserReplaceViewModelViewConfig : WebViewConfig<UserReplaceViewModel>
    {
        protected override void ConfigDetailsView()
        {
            using (View.OrderProperties())
            {
                View.HasDetailColumnsCount(2);
                View.Property(p => p.UserId).Show().UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<AndonController>().GetUsers(k, p);
                });
                View.Property(p => p.ReplaceUserId).Show().UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<AndonController>().GetUsers(k, p);
                });
            }
        }
    }
}
