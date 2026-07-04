using SIE.KZ.Base.SmomControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.KZ.Base.SmomControl
{
    public class SmomControlSettingViewConfig : WebViewConfig<SmomControlSetting>
    {
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.FactoryCode).Show();
                View.Property(p => p.FactoryName).Show();
                View.Property(p => p.FactoryUrl).Show();
                View.Property(p => p.IsMain).UseEnumEditor().Show();
                View.Property(p => p.IsQrqc).UseEnumEditor().Show();
                View.ChildrenProperty(p => p.FactoryDetail).Show(ChildShowInWhere.All).HasLabel("工厂");
                View.ChildrenProperty(p => p.TypeParamDetail).Show(ChildShowInWhere.All);
            }
        }

        protected override void ConfigQueryView()
        {
            View.Property(p => p.FactoryCode).Show();
            View.Property(p => p.FactoryName).Show();
        }
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.FactoryCode).Show();
            View.Property(p => p.FactoryName).Show();
        }
    }
}
