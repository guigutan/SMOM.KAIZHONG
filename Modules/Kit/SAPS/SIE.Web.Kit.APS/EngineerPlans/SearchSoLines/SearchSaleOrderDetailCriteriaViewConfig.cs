using SIE.Kit.APS.EngineerPlans.SearchSoLines;

namespace SIE.Web.Kit.APS.EngineerPlans.Configs
{
    public class SearchSaleOrderDetailCriteriaViewConfig : WebViewConfig<SearchSaleOrderDetailCriteria>
    {
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show(ShowInWhere.All);
                View.Property(p => p.ItemId).HasLabel("生产型号").Show(ShowInWhere.All);
                View.Property(p => p.ItemCode).HasLabel("物料编码").Show(ShowInWhere.All);
                View.Property(p => p.ItemName).HasLabel("物料名称").Show(ShowInWhere.All);
                View.Property(p => p.CustomerId).HasLabel("客户").Show(ShowInWhere.All);
                View.Property(p => p.CustomerName).Show(ShowInWhere.All);

                View.Property(p => p.RegisterDateTime).UseDateRangeEditor(p =>
                {
                    p.DateRangeType = ObjectModel.DateRangeType.All;
                }).Show(ShowInWhere.All);

                View.Property(p => p.RequireDelivery).UseDateRangeEditor(p =>
                {
                    p.DateRangeType = ObjectModel.DateRangeType.All;
                }).Show(ShowInWhere.All);

            }
        }
    }
}
