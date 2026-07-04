using SIE.Items.ProductFamilys;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Tech.Processs
{
    public class ProcessCriteriaViewConfig : WebViewConfig<ProcessCriteria>
    {
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Type);
            View.Property(p => p.ProductFamilyId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<ProductFamilyController>().GetProductFamily(keyword, pagingInfo);
            });
        }
    }
}
