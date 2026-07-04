using SIE.MES.EmpWork;
using SIE.MES.Fixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.EmpWork
{
    /// <summary>
    /// 人员与工作中心
    /// </summary>
    public class EmpWorkCentrtCriterialViewConfig : WebViewConfig<EmpWorkCentrtCriterial>
    {
        /// <summary>
        /// 视图查询
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Employee).ShowInList(width: 150);
                View.Property(p => p.EmpNo).ShowInList(width: 150);
                View.Property(p => p.WorkCenter).ShowInList(width: 150);
                View.Property(p => p.WorkName).ShowInList(width: 150);
            }
        }
    }
}
