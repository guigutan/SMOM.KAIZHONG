using SIE.MES.Common.HomeMenusConfigs;
using SIE.MetaModel.View;
using SIE.Web.MES.Common.HomeMenusConfigs.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.MES.Common.HomeMenusConfigs
{
    /// <summary>
    /// 
    /// </summary>
    public class HomeMenusConfigViewConfig : WebViewConfig<HomeMenusConfig>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.ReplaceCommands(WebCommandNames.ExportXls, typeof(EditPermissionsCommand).FullName);
            View.RemoveCommands(WebCommandNames.ExportXlsAll, WebCommandNames.ExportXlsSelection);
            View.Property(p => p.Role).HasLabel("角色");
            View.Property(p => p.User).HasLabel("用户");
        }
    }
}
