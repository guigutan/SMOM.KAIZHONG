using AngleSharp.Css.Dom;
using SIE.Andon.Andons;
using SIE.MetaModel.View;
using SIE.Web.Andon.Andons.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Andon.Andons
{
    public class AndonGroupViewConfig : WebViewConfig<AndonGroup>
    {
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(AndonGroup));
        }

        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, WebCommandNames.Save, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll, typeof(AndonGroupImportCommand).FullName, typeof(AndonGroupDLTemplateCommand).FullName, typeof(UserReplaceCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show();
                View.Property(p => p.Name).Show();
                View.ChildrenProperty(p => p.AndonGroupDetailList).Show(ChildShowInWhere.All);
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
                View.Property(p => p.Code).Show();
                View.Property(p => p.Name).Show();
            }
        }
    }
}
