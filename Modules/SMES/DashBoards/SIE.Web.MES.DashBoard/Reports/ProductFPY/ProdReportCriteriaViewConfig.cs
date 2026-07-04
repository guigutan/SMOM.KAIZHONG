using SIE.Domain;
using SIE.Items;
using SIE.MES.DashBoard.Reports.ProductFPY;
using SIE.MetaModel.View;

namespace SIE.Web.MES.DashBoard.Reports.ProductFPY
{
    /// <summary>
    /// 产品报表查询实体视图配置
    /// </summary>
    internal class ProdReportCriteriaViewConfig : WebViewConfig<ProductReportViewModelCriteria>
    {
        /// <summary>
        /// 默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            View.ReplaceCommands(WebCommandNames.ExecuteQuery, "SIE.Web.MES.DashBoard.Reports.ProductFPY.Commands.ProductReportCriteriaCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.ProductModel).UseProductModelEditor().Cascade(p => p.Product, null).HasLabel("产品机型").Show()
                    .UseListSetting(e => { e.HelpInfo = "更改产品机型清空产品"; });
                View.Property(p => p.Product).UseDataSource((e, p, k) =>
               {
                   var criteria = e as ProductReportViewModelCriteria;
                   if (criteria.ProductModel == null) return new EntityList<Item>();
                   return RT.Service.Resolve<Items.ItemController>().GetItems(ItemType.Material, p, k, (double)criteria.ProductModelId);
               }).UsePagingLookUpEditor((c, e) => c.ReloadDataOnPopping = true).HasLabel("产品").Show()
               .UseListSetting(e => { e.HelpInfo = "显示当前产品机型且物料类型为原材料的物料"; });
                View.Property(p => p.CollectDate).HasLabel("日期").UseDateRangeEditor(p => { p.DateRangeType = ObjectModel.DateRangeType.Month; p.DateFormat = "Y/m/d"; }).Show();
            }
        }
    }
}
