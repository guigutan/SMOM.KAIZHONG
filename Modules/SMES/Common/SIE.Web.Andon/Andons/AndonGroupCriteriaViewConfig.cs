using SIE.Andon.Andons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Andon.Andons
{
    public class AndonGroupCriteriaViewConfig : WebViewConfig<AndonGroupCriteria>
    {
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show();
                View.Property(p => p.Name).Show();
                View.Property(p => p.UserCode).Show();
                View.Property(p => p.UserName).Show();
                View.Property(p => p.UserState).Show();
            }
        }
    }
}
