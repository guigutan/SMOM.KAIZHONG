using SIE.Core.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Core.Configs
{
    /// <summary>
    ///  电子签名源配置项视图
    /// </summary>
    public class ElecSignConfigViewConfig:WebViewConfig<ElecSignConfigValue>
    {
        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.ElecSignType).Show();
        }
    }
}
