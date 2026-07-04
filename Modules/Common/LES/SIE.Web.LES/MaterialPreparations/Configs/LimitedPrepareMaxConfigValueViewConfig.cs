using SIE.LES.MaterialPreparations.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.LES.MaterialPreparations.Configs
{
    /// <summary>
    /// 推式备料是否限制最高存量配置视图
    /// </summary>
    public class LimitedPrepareMaxConfigValueViewConfig : WebViewConfig<LimitedPrepareMaxConfigValue>
    {
        /// <summary>
        /// 配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.IsLimited).Show(ShowInWhere.All);
            }
        }
    }
}
