using SIE.MES.Andon;
using SIE.MES.Fixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Andon
{
    /// <summary>
    /// 安灯区域实体查询
    /// </summary>
    public class AndonUpholdCriterialViewConfig : WebViewConfig<AndonUpholdCriterial>
    {
        /// <summary>
        /// 视图查询
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.AndonDesc).ShowInList(width: 150);
                View.Property(p => p.AndonCode).ShowInList(width: 150);
                View.Property(p => p.PlantCode).ShowInList(width: 150);
                View.Property(p => p.PlantName).ShowInList(width: 150);
            }
        }
    }
}
