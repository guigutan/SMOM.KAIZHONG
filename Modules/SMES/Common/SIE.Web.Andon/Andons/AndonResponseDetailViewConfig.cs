using DevExpress.Web.Rendering;
using SIE.Andon.Andons;
using SIE.MetaModel.View;
using SIE.Web.Andon.Andons.Commands;
using SIE.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Andon.Andons
{
    public class AndonResponseDetailViewConfig : WebViewConfig<AndonResponseDetail>
    {
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, WebCommandNames.Save, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll, typeof(AndonResponseDtlImportCommand).FullName, typeof(AndonResponseDetailDLTemplateCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.AndonUpholdId).Show();
                View.Property(p => p.AndonGroupId).Show();
                View.Property(p => p.AndonseepLevel).Show().UseCatalogEditor(p => p.CatalogType = "ANDONSESP_LEVEL");
            }
        }

        protected override void ConfigImportView()
        {
            using (View.OrderProperties())
            {
                View.PropertyRef(p => p.AndonUphold.AndonDesc).Show().HasLabel("区域描述");
                View.PropertyRef(p => p.AndonGroup.Code).Show().HasLabel("安灯责任组编码");
                View.Property(p => p.AndonseepLevel).Show();
            }        
        }
    }
}
