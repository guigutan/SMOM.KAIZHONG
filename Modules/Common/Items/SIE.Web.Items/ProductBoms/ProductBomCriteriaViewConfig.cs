using SIE.Items;
using SIE.Web.ClientMetaModel;

namespace SIE.Web.Items.ProductBoms
{
    /// <summary>
    /// 产品BOM查询实体视图配置
    /// </summary>
    internal class ProductBomCriteriaViewConfig : WebViewConfig<ProductBomCriteria>
    {
        /// <summary>
        /// 默认查询视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();

            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show(ShowInWhere.All);
                View.Property(p => p.Name).Show(ShowInWhere.All);
                View.Property(p => p.Product).UsePagingLookUpPopupEditor(p => 
                {
                    p.Editable = true; 
                    p.MultiOrSelect = MultiSelect.Multi;
                }).Show(ShowInWhere.All);
                View.Property(p => p.ProductName).Show(ShowInWhere.All);
                View.Property(p => p.SpecificationModel).Show(ShowInWhere.All);
            }
        }
    }
}
