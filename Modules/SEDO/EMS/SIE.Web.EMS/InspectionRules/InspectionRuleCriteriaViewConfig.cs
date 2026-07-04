using SIE.EMS.InspectionRules;

namespace SIE.Web.EMS.InspectionRules
{
    /// <summary>
    /// 检验规程查询视图
    /// </summary>
    public class InspectionRuleCriteriaViewConfig : WebViewConfig<InspectionRuleCriteria>
    {
        /// <summary>
        /// 主体
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).HasLabel("编码").Show(ShowInWhere.All);
                View.Property(p => p.Name).HasLabel("名称").Show(ShowInWhere.All);
                View.Property(p => p.InspectionRuleType).Show(ShowInWhere.All);
                View.Property(p => p.CheckCategory).Show(ShowInWhere.All);
                View.Property(p => p.CreateDate).UseDateRangeEditor(p =>
                {
                    p.DateRangeType = ObjectModel.DateRangeType.All;
                }).Show(ShowInWhere.All);
            }
        }
    }
}
