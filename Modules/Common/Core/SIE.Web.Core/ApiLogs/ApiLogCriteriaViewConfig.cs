using SIE.Core.ApiLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Core.ApiLogs
{
    /// <summary>
    ///  视图配置
    /// </summary>
    internal class ApiLogCriteriaViewConfig : WebViewConfig<ApiLogCriteria>
    {
        /// <summary>
        /// 查询
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.ApiName).Show(ShowInWhere.All);
                View.Property(p => p.CreateByName).Show(ShowInWhere.All);
                View.Property(p => p.KeyValue).Show(ShowInWhere.All).UseListSetting(f => f.HelpInfo = "匹配所有的关键字内容".L10N());
                View.Property(p => p.StartTime).Show(ShowInWhere.All);
                View.Property(p => p.EndTime).Show(ShowInWhere.All);                              
                View.Property(p => p.Controller).Show(ShowInWhere.All);
                View.Property(p => p.Method).Show(ShowInWhere.All);
            }
        }
    }
}
