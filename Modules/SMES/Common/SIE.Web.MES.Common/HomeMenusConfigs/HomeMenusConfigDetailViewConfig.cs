using SIE.MES.Common.HomeMenusConfigs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.MES.Common.HomeMenusConfigs
{
    /// <summary>
    ///菜单明细页面
    /// </summary>
    public class HomeMenusConfigDetailViewConfig : WebViewConfig<HomeMenusConfigDetail>
    {

        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.MenuName).HasLabel("菜单名称");
            View.Property(p => p.MoudleKey).HasLabel("模块");
            View.Property(p => p.Platform).HasLabel("平台");
        }
    }
}
