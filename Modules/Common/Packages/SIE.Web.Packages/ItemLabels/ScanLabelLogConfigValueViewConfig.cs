using SIE.Packages.ItemLabels.Configs;

namespace SIE.Web.Packages.ItemLabels
{
    /// <summary>
    /// 视图配置
    /// </summary>
    public class ScanLabelLogConfigValueViewConfig : WebViewConfig<ScanLabelLogConfigValue>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.IsLogScanLabel).ShowInDetail();
        }
    }
}
