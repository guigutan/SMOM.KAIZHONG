using SIE.MES.Engrave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Engrave
{
    internal class EngraveSnViewConfig : WebViewConfig<EngraveSn>
    {
        protected override void ConfigListView()
        {
            View.Property(p => p.Sn).ShowInList(300).Readonly();
        }
    }
}
