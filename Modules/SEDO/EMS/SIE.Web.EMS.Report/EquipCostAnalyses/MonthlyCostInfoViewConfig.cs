using SIE.EMS.Report.EquipCostAnalyses;

namespace SIE.Web.EMS.Report.EquipCostAnalyses
{
    /// <summary>
    /// 月度成本分析-界面
    /// </summary>
    internal class MonthlyCostInfoViewConfig : WebViewConfig<MonthlyCostInfo>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.WithoutPaging();
            View.UseClientOrder();
            View.Property(p => p.CostItem).ShowInList(150);
            View.Property(p => p.January).ShowInList(70);
            View.Property(p => p.February).ShowInList(70);
            View.Property(p => p.March).ShowInList(70);
            View.Property(p => p.April).ShowInList(70);
            View.Property(p => p.May).ShowInList(70);
            View.Property(p => p.June).ShowInList(70);
            View.Property(p => p.July).ShowInList(70);
            View.Property(p => p.August).ShowInList(70);
            View.Property(p => p.September).ShowInList(70);
            View.Property(p => p.October).ShowInList(70);
            View.Property(p => p.November).ShowInList(70);
            View.Property(p => p.December).ShowInList(70);
        }
    }
}
