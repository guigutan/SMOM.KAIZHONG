using SIE.MES.PrepareProducts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.MES.PrepareProducts
{
    /// <summary>
    /// 产前项目准备查询实体视图配置
    /// </summary>
    public class PrepareProjectCriteriaViewConfig : WebViewConfig<PrepareProjectCriteria>
    {
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.ProCode).Show();
                View.Property(p => p.ProName).Show();
                View.Property(p => p.ProType).Show();
            }
        }
    }
}
