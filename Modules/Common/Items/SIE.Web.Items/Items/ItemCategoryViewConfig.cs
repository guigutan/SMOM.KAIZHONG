using SIE.Domain;
using SIE.Items;
using SIE.MetaModel.View;
using SIE.Web.Items.Items.Commands;

namespace SIE.Web.Items
{
    /// <summary>
    /// 分类视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    class ItemCategoryViewConfig : WebViewConfig<ItemCategory>
    {
        /// <summary>
        /// 列表视图配置
        /// </summary>
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
                typeof(AddChildCommand).FullName,
                WebCommandNames.ExportXls,
                "SIE.Web.Items.Items.Commands.ItemCategoryLevelCommand");
            View.Property(p => p.Code).ShowInList(width: 300);
            View.Property(p => p.Name);
            View.Property(p => p.LevelId).UsePagingLookUpEditor(e => { e.MethodClassName = "SIE.Web.Common.NumberRules.ItemCategoryLevelMethod"; })
            .UseDataSource((e, c, r) =>
            {
                //经前台特殊处理，e为回传当前实体的父数据做为数据查询条件
                var parentCategory = e as ItemCategory;
                var list = RT.Service.Resolve<ItemController>().GetChildItemCategoryLevel(parentCategory?.LevelId);
                list.EachNode(p => 
                { 
                    p.SetTreePId(null); 
                    return false;
                });  //取消树型关系
                return list;
            }).UseListSetting(e => { e.HelpInfo = "显示物料分类层级下的子分类，分类层级为空显示根节点"; });
            View.Property(p => p.Type).Readonly();
            View.Property(p => p.ItemType).Readonly(p => p.PersistenceStatus != PersistenceStatus.New)
                .UseListSetting(e => { e.HelpInfo = "新增状态可编辑"; });
        }

        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Level);
            View.Property(p => p.Type).UseEnumEditor(p => p.AllowBlank = true);
            View.Property(p => p.ItemType).UseEnumEditor(p => p.AllowBlank = true);
        }

        /// <summary>
        /// 下拉视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.LevelName);
            View.Property(p => p.Type);
            View.Property(p => p.ItemType);
        }
    }
}