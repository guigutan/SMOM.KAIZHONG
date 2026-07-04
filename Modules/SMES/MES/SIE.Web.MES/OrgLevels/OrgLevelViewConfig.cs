using SIE.MES.OrgLevels;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.OrgLevels
{
    internal class OrgLevelViewConfig : WebViewConfig<OrgLevel>
    {
        protected override void ConfigListView()
        {           
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            View.Property(p => p.OrgCode).Readonly();
            View.Property(p => p.OrgName).Readonly();
            View.Property(p => p.ParentLevel).Readonly();           
        }
    }
}
