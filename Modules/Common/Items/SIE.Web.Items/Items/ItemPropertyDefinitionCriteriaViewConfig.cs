using SIE.Items;

namespace SIE.Web.Items
{
    /// <summary>
    /// 视图配置
    /// </summary>
    public class ItemPropertyDefinitionCriteriaViewConfig : WebViewConfig<ItemPropertyDefinitionCriteria>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Name).Show();
                View.Property(p => p.PropertyType).Show().UseEnumEditor(p => p.AllowBlank = true);
                View.Property(p => p.CatalogType).UseDataSource((e, c, r) =>
                {
                    string keyword = "%" + r + "%";
                    return RT.Service.Resolve<ItemController>().GetCatalogTypes(keyword, c);
                }).UsePagingLookUpEditor(p => p.Editable = true).Show();
            }
        }
    }
}