using SIE.MES.ItemEquipAccount.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ItemEquipAccount.Configs
{
    public class EquipAccountItemConfigValueViewConfig : WebViewConfig<EquipAccountItemConfigValue>
    {
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.IsValidAcupoint).Show();
            }
        }

        protected override void ConfigDetailsView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.IsValidAcupoint).Show();
            }
        }
    }
}
