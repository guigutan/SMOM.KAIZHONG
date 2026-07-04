using SIE.KZ.Base.SmomControl;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.KZ.Base.SmomControl
{
    public class TypeParamDetailViewConfig:WebViewConfig<TypeParamDetail>
    {
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, WebCommandNames.Save);
            View.Property(p => p.ParamType).Show();
        }
    }
}
