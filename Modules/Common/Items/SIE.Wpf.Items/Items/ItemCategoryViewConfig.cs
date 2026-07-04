using SIE.Domain;
using SIE.Items;

namespace SIE.Wpf.Items
{
    /// <summary>
    /// 分类视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    class ItemCategoryViewConfig : WPFViewConfig<ItemCategory>
    {
        #region 类型只读 TypeReadOnly
        /// <summary>
        /// 类型只读
        /// </summary>
        public static readonly Property<bool> TypeReadOnlyProperty = P<ItemCategory>.RegisterExtensionReadOnly("TypeReadOnly", typeof(ItemCategoryViewConfig),
            GetTypeReadOnly, ItemCategory.IdProperty);

        /// <summary>
        /// 类型只读
        /// </summary>
        /// <param name="me">分类层级</param>
        /// <returns>编辑状态或者存在父层级返回true，否则返回false</returns>
        public static bool GetTypeReadOnly(ItemCategory me)
        {
            ////编辑状态或者存在父层级只读
            if (me.TreePId != null)
            {
                var parentCategory = RT.Service.Resolve<ItemController>().GetItemCategory(me.TreePId);
                return parentCategory.ItemType.HasValue;
            }

            return false;
        }
        #endregion

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior(typeof(CategoryBehavior));
            View.UseDefaultBehaviors().InlineEdit().UseDefaultCommands().RemoveCommands(WPFCommandNames.Undo, WPFCommandNames.Redo);
            View.ReplaceCommands(WPFCommandNames.ListAdd, typeof(AddCategoryCommand));
            View.ReplaceCommands(WPFCommandNames.ListCopy, typeof(CopyCategoryCommand));
            View.ReplaceCommands(WPFCommandNames.ListDelete, typeof(CategoryDeleteCommand));
            View.UseCommands(typeof(AddCategoryChildCommand), typeof(InsertCategoryCommand));

            View.Property(p => p.Code).Readonly(DataEntityStatus.IsEditStatusProperty);
            View.Property(p => p.Name);
            View.Property(p => p.Level).UseDataSource((e, p, s) =>
            {
                var category = e as ItemCategory;
                return RT.Service.Resolve<ItemController>().GetChildLevel(category.TreePId);
            }).UsePagingLookUpEditor().Readonly(DataEntityStatus.IsEditStatusProperty);
            View.Property(p => p.Type).Readonly(true);
            View.Property(p => p.ItemType).Readonly(TypeReadOnlyProperty);
        }

        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Level);
            View.Property(p => p.Type).UseEnumEditor(e => e.AllowNullInput = true);
            View.Property(p => p.ItemType);
        }

        /// <summary>
        /// 下拉视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.AddBehavior(typeof(CategoryBehavior));
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Level);
            View.Property(p => p.Type);
            View.Property(p => p.ItemType);
        }
    }
}