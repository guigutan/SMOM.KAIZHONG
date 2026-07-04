using SIE.AbnormalInfo.AbnormalMonitors;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.AbnormalInfo.AbnormalMonitors
{
    /// <summary>
    /// 异常定义查询实体视图配置
    /// </summary>
    public class AbnormalDefineCriteriaViewConfig : WebViewConfig<AbnormalDefineCriteria>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show(ShowInWhere.All);
                View.Property(p => p.AbnomalSourceId).Show(ShowInWhere.All);
                View.Property(p => p.AbnormalRuleId).Show(ShowInWhere.All);
                View.Property(p => p.AbnormalWarnDefineId).Show(ShowInWhere.All);
                View.Property(p => p.State).Show(ShowInWhere.All);
                View.Property(p => p.CreateDate).UseDateRangeEditor(p => { p.DateFormat = "Y/m/d"; p.DateRangeType = ObjectModel.DateRangeType.Month; }).Show(ShowInWhere.All).Readonly(false).Show(ShowInWhere.All);
            }
        }
    }
}
