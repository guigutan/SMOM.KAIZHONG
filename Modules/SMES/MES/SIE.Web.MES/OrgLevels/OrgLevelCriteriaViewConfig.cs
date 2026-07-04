using SIE.MES.OrgLevels;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.OrgLevels
{
    internal class OrgLevelCriteriaViewConfig : WebViewConfig<OrgLevelCriteria>
    {
        protected override void ConfigView()
        {
            View.UseClientOrder();

        }
        protected override void ConfigQueryView()
        {
            View.Property(p => p.OrgCode).HasLabel("组织架构ID");             
            View.Property(p => p.OrgName).HasLabel("组织架构名称");             
            View.Property(p => p.ParentLevel).HasLabel("父级组织");             
            View.Property(p => p.TheLevel).HasLabel("组织层级");             
        }


    }
}
