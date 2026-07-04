using SIE.MES.BlueLable;
using SIE.MES.LineAndon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.BlueLable
{
    public class BlueLabelReplaceCriteriaViewConfig : WebViewConfig<BlueLabelReplaceCriteria>
    {
        /// <summary>
        /// 视图查询
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.OldBlueLabel).ShowInList(width: 150);
                View.Property(p => p.NewBlueLabel).ShowInList(width: 150);
            }
        }
    }
}


