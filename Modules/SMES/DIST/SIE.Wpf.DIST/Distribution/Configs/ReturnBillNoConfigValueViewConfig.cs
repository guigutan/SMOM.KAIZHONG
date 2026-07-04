using SIE.Common.NumberRules;
using SIE.DIST.Distribution.Configs;

namespace SIE.Wpf.DIST.Distribution.Configs
{
    /// <summary>
    /// 退料单号值配置视图配置
    /// </summary>
    class RmaBillNoConfigValueViewConfig : WPFViewConfig<ReturnBillNoConfigValue>
    {
        /// <summary>
        /// 通用视图配置
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
