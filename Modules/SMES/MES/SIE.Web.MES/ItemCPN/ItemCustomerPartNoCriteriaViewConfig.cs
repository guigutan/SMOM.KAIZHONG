using DevExpress.DataProcessing;
using SIE.MES.ItemCPN;
using SIE.MetaModel.View;
using SIE.Web.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ItemCPN
{
    public class ItemCustomerPartNoCriteriaViewConfig : WebViewConfig<ItemCustomerPartNoCriteria>
    {
        protected override void ConfigQueryView()
        {
            //View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Save, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll, typeof(LineAreaImportCommand).FullName, typeof(LineAreaDLTemplateCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.ItemCode);
                View.Property(p => p.ItemName);
                View.Property(p => p.Customer);
                View.Property(p => p.CodeAlias);
            }
        }
    }
}
