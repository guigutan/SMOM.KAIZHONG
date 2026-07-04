using SIE.AbnormalInfo.AbnormalMonitors;
using SIE.Common.NumberRules;


namespace SIE.Web.AbnormalInfo.AnomalyMonitors
{
	/// <summary>
	/// 配送单单号配置值视图配置
	/// </summary>
	public class IndicatorNoConfigValueViewConfig : WebViewConfig<IndicatorNoConfigValue>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.NumberRule).Show(ShowInWhere.All).UsePagingLookUpEditor();
            }
        }
    }
}