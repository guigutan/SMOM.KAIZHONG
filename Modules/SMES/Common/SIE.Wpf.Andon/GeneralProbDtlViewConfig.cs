using SIE.Andon.Andons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.Andon
{
    public class GeneralProbDtlViewConfig:WPFViewConfig<GeneralProbDtl>
    {
        protected override void ConfigSelectionView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Desc).Show();
            }
        }
    }
}
