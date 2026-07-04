using SIE.MES.BlueLable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.BlueLable
{
    /// <summary>
    /// 蓝标层级查询
    /// </summary>
    public class BlueLableLevelCriterialViewConfig : WebViewConfig<BlueLableLevelCriterial>
    {
        /// <summary>
        /// 视图查询
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Item).ShowInList(width: 150);
            }
        }
    }
}
