using SIE.KZ.Base.SmomControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.KZ.Base.SmomControl
{
    public class GuidFactoryRelastionViewConfig : WebViewConfig<GuidFactoryRelastion>
    {
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.InfType).Show().Readonly();
                View.Property(p => p.Guid).Show().Readonly();
                View.Property(p => p.SourceFactory).Show().Readonly();
            }
        }
    }
}
