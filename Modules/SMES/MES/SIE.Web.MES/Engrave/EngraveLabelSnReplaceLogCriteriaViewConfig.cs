using SIE.MES.Engrave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Engrave
{
    public class EngraveLabelSnReplaceLogCriteriaViewConfig : WebViewConfig<EngraveLabelSnReplaceLogCriteria>
    {
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.OldSn).Show();
                View.Property(p => p.NewSn).Show();
                View.Property(p => p.ProductCode).Show();
                View.Property(p => p.ProductName).Show();
                View.Property(p => p.ShortDescription).Show();
                View.Property(p => p.BatchNo).Show();
                View.Property(p => p.BlueLabel).Show();
            }
        }
    }
}
