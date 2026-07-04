using SIE.Domain;
using SIE.Items;
using SIE.Wpf.Command;

namespace SIE.Wpf.Items
{
    /// <summary>
    /// 插入分类命令
    /// </summary>
    [Command(Label = "插入", Location = MetaModel.View.CommandLocation.Menu, GroupType = CommandGroupType.Edit)]
    class InsertCategoryCommand : TreeInsertCommand
    {
        /// <summary>
        /// 设置子分类类型与父分类类型一致
        /// </summary>
        /// <param name="entity">分类</param>
        protected override void OnItemCreated(Entity entity)
        {
            var current = View.Current as ItemCategory;
            var category = entity as ItemCategory;
            category.PropertyChanged += Level_PropertyChanged;
            if (category.TreePId != null)
            {
                category.Type = current.Type;

                var parentCategory = RT.Service.Resolve<ItemController>().GetItemCategory(category.TreePId);
                if (parentCategory != null)
                    category.ItemType = parentCategory.ItemType;
            }
        }

        /// <summary>
        /// 分类属性变更事件
        /// </summary>
        /// <param name="sender">分类</param>
        /// <param name="e">属性变更参数</param>
        private void Level_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var category = View.Current as ItemCategory;
            if (e.PropertyName == nameof(ItemCategory.Level))
            {
                if (category.Level != null)
                    category.Type = category.Level.Type;
            }
        }
    }
}
