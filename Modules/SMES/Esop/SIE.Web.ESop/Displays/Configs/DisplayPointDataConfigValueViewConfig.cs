using SIE.ESop.Displays.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.ESop.Displays.Configs
{
    /// <summary>
    /// ESOP文档来源配置项视图配置
    /// </summary>
    public class DisplayPointDataConfigValueViewConfig : WebViewConfig<DisplayPointDataConfigValue>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.DataFrom);
        }
    }
}
