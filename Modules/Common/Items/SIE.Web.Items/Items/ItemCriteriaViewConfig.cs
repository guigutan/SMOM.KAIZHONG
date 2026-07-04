using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using System.Linq;

namespace SIE.Web.Items.Items
{

    /// <summary>
    /// 物料查询实体视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class ItemCriteriaViewConfig : WebViewConfig<ItemCriteria>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).ShowInDetail();
                View.Property(p => p.Name).ShowInDetail();
                View.Property(p => p.SpecificationModel).ShowInDetail();
                View.Property(p => p.Type).ShowInDetail();
                View.Property(p => p.ItemSourceType).ShowInDetail();
                View.Property(p => p.State).ShowInDetail();
                View.Property(p => p.SourceType).ShowInDetail();
                View.Property(p => p.UpdateDate).UseDateRangeEditor(p => p.DateRangeType = DateRangeType.All).ShowInDetail();
                View.Property(p => p.UpdateBy).ShowInDetail();
                View.Property(p => p.PurchasingAgent).ShowInDetail();
                View.Property(p => p.CategoryType).ShowInDetail();
                View.Property(p => p.ItemCategory).UseDataSource((e, p, s) =>
                {
                    var criteria = e as ItemCriteria;
                    if (criteria == null || !criteria.CategoryType.HasValue)
                    {
                        return new EntityList<ItemCategory>();  
                    }
                    var results = RT.Service.Resolve<ItemController>().GetItemSmallCategory(criteria.CategoryType, criteria.Type, s, p);
                    results.ForEach(c => c.TreePId = null);
                    return results;
                }).UsePagingLookUpEditor().ShowInDetail()
                .UseListSetting(e => { e.HelpInfo = "显示当前物料分类类型下的物料小类"; });
                View.Property(p => p.ShortDescription).ShowInDetail();
                View.Property(p => p.Bismt).ShowInDetail();
            }
        }
    }
}