using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;

namespace SIE.Wpf.Items.Items
{
    /// <summary>
    /// 物料查询实体视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class ItemCriteriaViewConfig : WPFViewConfig<ItemCriteria>
    {
        #region 清除物料分类 ClearItemCategory
        /// <summary>
        /// 清除物料分类
        /// </summary> 
        public static readonly Property<bool> ClearItemCategoryProperty = P<ItemCriteria>.RegisterExtensionReadOnly("ClearItemCategory", typeof(ItemCriteriaViewConfig),
            GetClearItemCategory, ItemCriteria.CategoryTypeProperty);

        /// <summary>
        /// 清除物料分类
        /// </summary>
        public static bool GetClearItemCategory(ItemCriteria me)
        {
            me.ItemCategory = null;
            return false;
        }
        #endregion

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
                View.Property(p => p.UpdateDate).UseDateRangeEditor(p => { p.DateTimePart = DateTimePart.Date; p.DateRangeType = ObjectModel.DateRangeType.All; }).ShowInDetail();
                View.Property(p => p.UpdateBy).ShowInDetail();
                View.Property(p => p.PurchasingAgent).ShowInDetail();
                View.Property(p => p.CategoryType).ShowInDetail();
                View.Property(p => p.ItemCategoryId).UseDataSource((e, p, s) =>
                {
                    var criteria = e as ItemCriteria;
                    if (criteria == null || !criteria.CategoryType.HasValue) return new EntityList<ItemCategory>();
                    return RT.Service.Resolve<ItemController>().GetItemSmallCategory(criteria.CategoryType, criteria.Type, s, p);
                }).ShowInDetail().Readonly(ClearItemCategoryProperty).UsePagingLookUpEditor(e => e.ReloadDataOnPopping = true);
            }
        }
    }
}