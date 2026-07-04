using SIE.Core.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Core.Configs
{
    /// <summary>
    /// 拣货时拆标，是否自动扫描标签
    /// </summary>
    public class AfterSpliteAutoScanConfigViewConfig : WebViewConfig<AfterSpliteAutoScanConfigValue>
    {
        ///<summary>
        /// 拣货时拆标，是否自动扫描标签
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.AfterSpliteAutoScanType).Show();
        }
    }
}
