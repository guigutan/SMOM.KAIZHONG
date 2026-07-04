using SIE.Items.KzItemCategorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Items.KzItemCategorys
{
    public class KzItemCategorySelectViewConfig : WebViewConfig<KzItemCategorySelect>
    {
        protected override void ConfigListView()
        {
            View.Property(p => p.Code).Show().Readonly();
        }

        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code).Show().Readonly();
        }

        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code).Show().Readonly();
        }
    }
}
