using SIE.Items.KzItemCategorys;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Items.KzItemCategorys
{
    public class KzCategoryViewConfig : WebViewConfig<KzCategory>
    {
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(KzCategory));
            base.ConfigView();
        }

        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, WebCommandNames.Save, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll).UseImportCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show();
                View.Property(p => p.Name).Show();
            }
        }

        protected override void ConfigImportView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show();
                View.Property(p => p.Name).Show();
            }
        }

        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show();
                View.Property(p => p.Name).Show();
            }
        }

        protected override void ConfigSelectionView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show().Readonly();
                View.Property(p => p.Name).Show().Readonly();
            }
        }
    }
}
