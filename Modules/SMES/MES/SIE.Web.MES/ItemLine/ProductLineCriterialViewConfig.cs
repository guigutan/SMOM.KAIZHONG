using SIE.MES.ItemLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ItemLine
{
    /// <summary>
    /// 产品与产线的关系实体查询
    /// </summary>
    public class ProductLineCriterialViewConfig :WebViewConfig<ProductLineCriterial>
    {
        /// <summary>
        /// 视图查询
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Item).ShowInList(width: 150).HasLabel("产品编码");
                View.Property(p => p.ItemName).ShowInList(width: 150).HasLabel("产品名称");
                View.Property(p => p.WipResource).ShowInList(width: 150);
                View.Property(p => p.LineName).ShowInList(width: 150);
                View.Property(p => p.Process).ShowInList(width: 150);
                View.Property(p => p.ProcessCode).ShowInList(width: 150);
               // View.Property(p => p.State).ShowInList(width: 150);
            }
        }
    }
}
