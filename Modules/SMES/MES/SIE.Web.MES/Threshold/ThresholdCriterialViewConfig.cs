using SIE.MES.ItemLine;
using SIE.MES.Threshold;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Threshold
{
    /// <summary>
    /// 阈值实体查询
    /// </summary>
    public class ThresholdCriterialViewConfig : WebViewConfig<ThresholdCriterial>
    {
        /// <summary>
        /// 视图查询
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Item).ShowInList(width: 150);
                View.Property(p => p.ItemName).ShowInList(width: 150);
                View.Property(p => p.Process).ShowInList(width: 150);
                View.Property(p => p.ThresholdValue).ShowInList(width: 150);
            }
        }
    }
}
