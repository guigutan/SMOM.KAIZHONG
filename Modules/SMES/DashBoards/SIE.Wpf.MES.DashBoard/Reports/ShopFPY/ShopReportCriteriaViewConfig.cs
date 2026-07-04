using SIE.MES.DashBoard.Reports.ShopFPY;
using SIE.Wpf.Resources;

namespace SIE.Wpf.MES.DashBoard.Reports.ShopFPY
{
    /// <summary>
    /// 车间报表查询实体视图配置
    /// </summary>
    internal class ShopReportCriteriaViewConfig : WPFViewConfig<ShopReportViewModelCriteria>
    {
        /// <summary>
        /// 默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("查询条件");
            View.UseDefaultCommands();

            using (View.OrderProperties())
            {
                View.Property(p => p.Shop).HasLabel("车间").UseResourceWorkShopEditor().Show();
                View.Property(p => p.CollectDate).HasLabel("日期").UseDateRangeEditor(p => { p.DateRangeType = ObjectModel.DateRangeType.Month; p.DateTimePart = ObjectModel.DateTimePart.Date; }).Show();
            }
        }

        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Shop).HasLabel("车间").Show();
                View.Property(p => p.CollectDate).HasLabel("日期").UseDateRangeEditor(p => { p.DateRangeType = ObjectModel.DateRangeType.Month; p.DateTimePart = ObjectModel.DateTimePart.Date; }).Show();
            }
        }
    }
}
