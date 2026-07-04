using SIE.Domain;
using SIE.Items;

namespace SIE.Wpf.Items
{
    /// <summary>
    /// 分类层级视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    class ItemCategoryLevelViewConfig : WPFViewConfig<ItemCategoryLevel>
    {
        #region 类型只读 TypeReadOnly
        /// <summary>
        /// 类型只读
        /// </summary>
        public static readonly Property<bool> TypeReadOnlyProperty = P<ItemCategoryLevel>.RegisterExtensionReadOnly("TypeReadOnly", typeof(ItemCategoryLevelViewConfig),
            GetTypeReadOnly, ItemCategoryLevel.IdProperty);

        /// <summary>
        /// 类型只读
        /// </summary>
        /// <param name="me">分类层级</param>
        /// <returns>编辑状态或者存在父层级返回true，否则返回false</returns>
        public static bool GetTypeReadOnly(ItemCategoryLevel me)
        {
            ////编辑状态或者存在父层级只读
            return me.PersistenceStatus != PersistenceStatus.New || me.TreePId.HasValue;
        }
        #endregion

        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            // 默认视图配置
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior(typeof(CategoryLevelBehavior));
            View.UseDefaultBehaviors().InlineEdit().UseDefaultCommands().RemoveCommands(WPFCommandNames.Undo, WPFCommandNames.Redo);
            View.ReplaceCommands(WPFCommandNames.ListDelete, typeof(CategoryLevelDeleteCommand));
            View.UseCommands(typeof(AddLevelChildCommand), typeof(InsertLevelCommand));
            View.Property(p => p.Code).Readonly(DataEntityStatus.IsEditStatusProperty);
            View.Property(p => p.Name);
            View.Property(p => p.Type).Readonly(TypeReadOnlyProperty);
        }

        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Type).UseEnumEditor(e => e.AllowNullInput = true);
        }

        /// <summary>
        /// 下拉视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Type);
        }
    }
}