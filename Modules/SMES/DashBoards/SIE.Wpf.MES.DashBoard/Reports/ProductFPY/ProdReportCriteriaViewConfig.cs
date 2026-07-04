using SIE.Domain;
using SIE.Items;
using SIE.MES.DashBoard.Reports.ProductFPY;

namespace SIE.Wpf.MES.DashBoard.Reports.ProductFPY
{
    /// <summary>
    /// 产品报表查询实体视图配置
    /// </summary>
    internal class ProdReportCriteriaViewConfig : WPFViewConfig<ProductReportViewModelCriteria>
    {
        /// <summary>
        /// 默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();

            using (View.OrderProperties())
            {
                View.Property(p => p.ProductModel).HasLabel("产品机型").Show();
                View.Property(p => p.Product).UsePagingLookUpEditor(p => { p.ReloadDataOnPopping = true; p.DisplayMember = nameof(Item.Name); }).UseDataSource((e, p, k) =>
                {
                    var criteria = e as ProductReportViewModelCriteria;
                    if (criteria.ProductModel == null) return new EntityList<Item>();
                    return RT.Service.Resolve<Items.ItemController>().GetItems(ItemType.Material, p, k, (double)criteria.ProductModelId);
                }).HasLabel("产品").Show();
                View.Property(p => p.CollectDate).HasLabel("日期").UseDateRangeEditor(p => { p.DateRangeType = ObjectModel.DateRangeType.Month; p.DateTimePart = ObjectModel.DateTimePart.Date; }).Show();
            }
        }
    }
}
