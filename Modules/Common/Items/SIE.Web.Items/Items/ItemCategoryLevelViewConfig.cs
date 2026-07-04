using SIE.Domain;
using SIE.Items;
using SIE.MetaModel.View;
using SIE.Web.Items.Items.Commands;

namespace SIE.Web.Items.Items
{
    /// <summary>
    /// 分类层级视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    class ItemCategoryLevelViewConfig : WebViewConfig<ItemCategoryLevel>
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

        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(ItemCategory));
        }
        protected override void ConfigListView()
        {
            View.DraggableForTree();
            View.InlineEdit();
            View.UseCommands(
                typeof(AddTreeCommand).FullName,
                WebCommandNames.Edit,
                WebCommandNames.Delete,
                WebCommandNames.TreeCopy,
                WebCommandNames.Save,
                typeof(InsertCommand).FullName,
                "SIE.Web.Items.Items.Commands.ItemCategoryLevelAddChildCommand",
                WebCommandNames.ExportXls);
            View.Property(p => p.Code).ShowInList(width: 300);
            View.Property(p => p.Name);
            View.Property(p => p.Type).Readonly(p => p.PersistenceStatus != PersistenceStatus.New || p.TreePId != null)
                .UseListSetting(e => { e.HelpInfo = "新增状态且树形父ID等于空可编辑"; });
        }
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Type);
        }
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Type).UseEnumEditor(p => p.AllowBlank = true);
        }
    }
}
