using SIE.AbnormalInfo.AbnormalMonitors;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.AbnormalInfo.AbnormalMonitors
{
    /// <summary>
    /// 
    /// </summary>
    public class AbnormalWarnDefineCriteriaViewConfig : WebViewConfig<AbnormalWarnDefineCriteria>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show(ShowInWhere.All);
                View.Property(p => p.Name).Show(ShowInWhere.All);
                View.Property(p => p.CreateDate).UseDateRangeEditor(p => { p.DateFormat = "Y/m/d"; p.DateRangeType = ObjectModel.DateRangeType.Month; }).Show(ShowInWhere.All).Readonly(false);
            }
        }
    }
}
