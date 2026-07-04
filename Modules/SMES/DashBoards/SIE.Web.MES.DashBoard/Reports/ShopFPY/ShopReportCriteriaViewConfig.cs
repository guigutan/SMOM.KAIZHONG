using SIE.MES.DashBoard.Reports.ShopFPY;
using SIE.MetaModel.View;

namespace SIE.Web.MES.DashBoard.WorkOrderReachs
{
    /// <summary>
    /// 产品报表查询实体视图配置
    /// </summary>
    internal class ShopReportCriteriaViewConfig : WebViewConfig<ShopReportViewModelCriteria>
    {
        /// <summary>
        /// 默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            View.ReplaceCommands(WebCommandNames.ExecuteQuery, "SIE.Web.MES.DashBoard.Reports.ProductFPY.Commands.ShopReportCriteriaCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.Shop).HasLabel("车间").UseResourceWorkShopEditor().Show();
                View.Property(p => p.CollectDate).HasLabel("日期").UseDateRangeEditor(p =>
                {
                    p.DateRangeType = ObjectModel.DateRangeType.Month;
                    p.DateFormat = "Y/m/d";
                }).Show(ShowInWhere.Detail);
            }
        }
    }
}
