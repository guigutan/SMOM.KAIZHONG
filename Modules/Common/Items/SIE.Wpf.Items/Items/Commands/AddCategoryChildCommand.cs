using SIE.Domain;
using SIE.Items;
using SIE.Wpf.Command;
using System.ComponentModel;

namespace SIE.Wpf.Items
{
    /// <summary>
    /// 添加分类子命令
    /// </summary>
    [Command(Label = "添加子", Location = MetaModel.View.CommandLocation.Menu, GroupType = CommandGroupType.Edit)]
    class AddCategoryChildCommand : TreeAddChildCommand
    {
        /// <summary>
        /// 设置子层级类型与父层级类型一致
        /// </summary>
        /// <param name="entity">分类层级</param>
        protected override void OnItemCreated(Entity entity)
        {
            var category = entity as ItemCategory;
            category.PropertyChanged += Level_PropertyChanged;
            if (category.TreePId != null)
            {
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
        private void Level_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var category = sender as ItemCategory;
            if (e.PropertyName == nameof(ItemCategory.Level))
            {
                if (category.Level != null)
                    category.Type = category.Level.Type;
            }
        }
    }
}
