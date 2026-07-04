using SIE.MES.ListAtts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ListAtts
{
    internal class ListAttCriteriaViewConfig : WebViewConfig<ListAttCriteria>
    {
        protected override void ConfigView()
        {
            View.UseClientOrder();
        }
        protected override void ConfigQueryView()
        {
            View.Property(p => p.DeptName).HasLabel("部门名称");
            View.Property(p => p.AreaName).HasLabel("区域名称");
            View.Property(p => p.Pin).HasLabel("人员编号");
            View.Property(p => p.Name).HasLabel("人员姓名");
            View.Property(p => p.EventTimeS).HasLabel("记录设备触发时间-开始");
            View.Property(p => p.EventTimeE).HasLabel("记录设备触发时间-结束");
        }
    }
}
