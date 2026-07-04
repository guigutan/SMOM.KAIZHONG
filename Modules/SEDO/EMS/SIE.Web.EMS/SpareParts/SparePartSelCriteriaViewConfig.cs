using SIE.EMS.SpareParts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.SpareParts
{
    /// <summary>
    /// 备件基础数据选择 视图
    /// </summary>
    public class SparePartSelCriteriaViewConfig : WebViewConfig<SparePartSelCriteria>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("备件基础数据选择");
            using (View.OrderProperties())
            {
                View.Property(p => p.SparePartCode).Show();
                View.Property(p => p.SparePartName).Show();
                View.Property(p => p.ModelCode).Show().Readonly(p => p.IsReadOnly);
            }
        }
    }
}
