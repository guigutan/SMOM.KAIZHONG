using SIE.Common.NumberRules;
using SIE.DIST.Distribution.Configs;
using SIE.Domain;

namespace SIE.Wpf.DIST.Distribution.Configs
{
    /// <summary>
    /// 单据条码视图配置
    /// </summary>
    class BillLabelConfigValueViewConfig : WPFViewConfig<BillLabelConfigValue>
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
                View.Property(p => p.PrintTemplate).Show(ShowInWhere.All).UseDataSource((o, c, e) =>
                {
                    var edit = o as BillLabelConfigValue;
                    var results = new EntityList<SIE.Common.Prints.PrintTemplate>();
                    if (edit.NumberRule == null) return results;

                    foreach (var template in edit.NumberRule?.TemplateList)
                    {
                        results.Add(template.Template);
                    }

                    return results;
                });
            }
        }
    }
}
