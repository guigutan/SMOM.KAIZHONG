using SIE.MES.ItemChecker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ItemChecker
{
    /// <summary>
    /// 检具与产品的关系实体查询
    /// </summary>
    public class CheckerItemCriterialViewConfig : WebViewConfig<CheckerItemCriterial>
    {
        /// <summary>
        /// 视图查询
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.CheckerUphold).ShowInList(width: 150);
                View.Property(p => p.CheckerName).ShowInList(width: 150);
                View.Property(p => p.Item).ShowInList(width: 150);
                View.Property(p => p.ItemName).ShowInList(width: 150);
                //View.Property(p => p.).ShowInList(width: 150);
                View.Property(p => p.Process).ShowInList(width: 150);
                View.Property(p => p.ProcessCode).ShowInList(width: 150);
            }
        }
    }
}
