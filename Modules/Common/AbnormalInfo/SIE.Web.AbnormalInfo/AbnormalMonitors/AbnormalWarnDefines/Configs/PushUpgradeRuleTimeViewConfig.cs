

using SIE.AbnormalInfo.AbnormalMonitors.Configs;

namespace SIE.Web.AbnormalInfo.AbnormalMonitors.Configs

{
    /// <summary>
    /// 推送升级机制循环次数配置值视图配置
    /// </summary>
    class PushUpgradeRuleTimeViewConfig : WebViewConfig<PushUpgradeRuleTimeConfigValue>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.CyclicTimes).UseSpinEditor(p => { p.MinValue = 0; p.AllowNegative = false;p.AllowDecimals = false;}).Show(ShowInWhere.All);
            }
        }
    }
}
