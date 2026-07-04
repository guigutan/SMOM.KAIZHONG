using SIE.MES.QTimes.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.QTimes.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class QTPushObjectViewModelViewConfig : WebViewConfig<QTPushObjectViewModel>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.ObjectCode).Readonly().ShowInList(width: 120);
                View.Property(p => p.ObjectName).Readonly().ShowInList(width: 120);
            }
        }
    }
}
