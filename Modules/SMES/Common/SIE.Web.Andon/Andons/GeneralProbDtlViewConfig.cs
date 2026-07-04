using SIE.Andon.Andons;
using SIE.Web.Andon.Andons.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Andon.Andons
{
    public class GeneralProbDtlViewConfig : WebViewConfig<GeneralProbDtl>
    {
        protected override void ConfigListView()
        {
            View.UseDefaultCommands().UseCommands(typeof(GeneralProbDtlImportCommand).FullName, typeof(GeneralProbDtlDLTemplateCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.Desc).Show();
            }
        }

        protected override void ConfigImportView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Desc).Show();
            }
        }

        protected override void ConfigSelectionView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Desc).Show();
            }
        }
    }
}
