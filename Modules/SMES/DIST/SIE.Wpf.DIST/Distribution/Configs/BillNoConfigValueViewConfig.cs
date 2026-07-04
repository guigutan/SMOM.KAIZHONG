using SIE.Common.NumberRules;
using SIE.DIST.Distribution.Configs;

namespace SIE.Wpf.DIST.Distribution.Configs
{
    /// <summary>
    /// 配送单单号配置值视图配置
    /// </summary>
    public class BillNoConfigValueViewConfig : WPFViewConfig<BillNoConfigValue>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.NumberRule).Show(ShowInWhere.All).UsePagingLookUpEditor().UseDataSource((o, pagingInfo, e) =>
                {
                    return RT.Service.Resolve<NumberRuleController>().GetNumberRule(RuleType.Other);
                });
            }
        }
    }
}