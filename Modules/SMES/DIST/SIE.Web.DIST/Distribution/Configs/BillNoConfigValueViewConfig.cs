using SIE.Common.NumberRules;
using SIE.DIST.Distribution.Configs;

namespace SIE.Web.DIST.Distribution.Configs
{
    /// <summary>
    /// 配送单单号配置值视图配置
    /// </summary>
    public class BillNoConfigValueViewConfig : WebViewConfig<BillNoConfigValue>
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
                }).UseListSetting(e => { e.HelpInfo = "显示规则类型为其它的编码规则"; });
            }
        }
    }
}