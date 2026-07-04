using SIE.EMS.AssetScraps.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.AssetScraps
{
    /// <summary>
    ///  视图配置
    /// </summary>
    public class EnableAssetDisposalConfigValueViewConfig : WebViewConfig<EnableAssetDisposalConfigValue>
    {
        /// <summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.IsEnableAssetDisposal).Cascade(p => p.IsEnableFixedAssetScrap, null);
            View.Property(p => p.IsEnableFixedAssetScrap).Readonly(p => !p.IsEnableAssetDisposal);
        }
    }
}
