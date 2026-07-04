using SIE.EMS.SpareParts.Configs;

namespace SIE.Web.EMS.Fixtures.Configs
{
    /// <summary>
    ///  批次号视图配置
    /// </summary>
    public class BatchNumberNoConfigValueViewConfig : WebViewConfig<BatchNumberConfigValue>
    {
        /// <summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.BatchNumberRule);
            View.Property(p => p.SnNumberRule);
        }
    }
}
