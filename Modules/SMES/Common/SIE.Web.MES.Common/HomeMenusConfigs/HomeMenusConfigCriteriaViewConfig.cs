using SIE.MES.Common.HomeMenusConfigs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.MES.Common.HomeMenusConfigs
{
    /// <summary>
    /// 
    /// </summary>
    public class HomeMenusConfigCriteriaViewConfig : WebViewConfig<HomeMenusConfigCriteria>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.UseName).HasLabel("用户");
            View.Property(p => p.RoleName).HasLabel("角色");
        }
    }
}