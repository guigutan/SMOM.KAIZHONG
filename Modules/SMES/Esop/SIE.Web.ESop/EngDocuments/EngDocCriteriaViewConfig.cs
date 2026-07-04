using SIE.ESop.EngDocuments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.ESop.EngDocuments
{
    /// <summary>
    /// 工程文件管理查询
    /// </summary>
    public class EngDocCriteriaViewConfig : WebViewConfig<EngDocCriteria>
    {
        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Type).Show();
            View.Property(p => p.WoNo).Show();
            View.Property(p => p.ProductCode).Show();
            View.Property(p => p.ProductName).Show();
            View.Property(p => p.DocCode).Show();
            View.Property(p => p.DocName).Show();
        }
    }
}
