using SIE.MES.TaskManagement.Specifications;

namespace SIE.Web.MES.TaskManagement.Specifications
{
    /// <summary>
    /// 叫料工单查询实体视图配置
    /// </summary>
    internal class ProductSpecificationCriteriaViewConfig : WebViewConfig<ProductSpecificationCriteria>
    {
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show(ShowInWhere.All);
                View.Property(p => p.Name).Show(ShowInWhere.All);
                View.Property(p => p.SpecificationModel).Show(ShowInWhere.All);
                View.Property(p => p.Type).UseEnumEditor().Show(ShowInWhere.All);
                View.Property(p => p.ItemSourceType).UseEnumEditor().Show(ShowInWhere.All);
                View.Property(p => p.State).UseEnumEditor().Show(ShowInWhere.All);
                View.Property(p => p.SourceType).UseEnumEditor().Show(ShowInWhere.All);
            }
        }
    }
}
