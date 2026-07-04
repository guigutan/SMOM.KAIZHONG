using SIE.EMS.Tpms.Configs;

namespace SIE.Web.EMS.Configs
{
    /// <summary>
    /// TPM评分单号视图
    /// </summary>
    public class TpmScoreNoConfigValueViewConfig : WebViewConfig<TpmScoreNoConfigValue>
    {
        /// <summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.NumberRule);
        }
    }
}
