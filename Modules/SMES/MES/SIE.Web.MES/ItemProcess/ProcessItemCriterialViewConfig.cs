using SIE.MES.ItemLine;
using SIE.MES.ItemProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ItemProcess
{
    /// <summary>
    /// 物料与工序的关系实体查询
    /// </summary>
    public class ProcessItemCriterialViewConfig : WebViewConfig<ProcessItemCriterial>
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
                //View.Property(p => p.).ShowInList(width: 150);
                View.Property(p => p.Process).ShowInList(width: 150);
                View.Property(p => p.State).ShowInList(width: 150);
            }
        }
    }
}
