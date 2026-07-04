using SIE.ProductIntfc.InspLogs.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.ProductIntfc.InspLogs
{
    /// <summary>
    /// 成品报检传QMS配置项视图配置
    /// </summary>
    public class InspLogCallConfigValueViewConfig : WebViewConfig<InspLogCallConfigValue>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.IsCall).Show();
        }
    }
}
