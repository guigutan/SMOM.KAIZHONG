using SIE.Domain;
using SIE.Items;
using SIE.Wpf.Command;
using System.ComponentModel;

namespace SIE.Wpf.Items
{
    /// <summary>
    /// 分类添加命令
    /// </summary>
    [Command(ImageName = "AddEntity", Label = "添加", ToolTip = "添加数据", Gestures = "Ctrl+Shift+N", GroupType = 10)]
    class AddCategoryCommand : ListAddCommand
    {
        /// <summary>
        /// 实体创建后事件
        /// </summary>
        /// <param name="entity">分类</param>
        protected override void OnItemCreated(Entity entity)
        {
            base.OnItemCreated(entity);
            var category = entity as ItemCategory;
            category.PropertyChanged += Category_PropertyChanged;
        }

        /// <summary>
        /// 分类属性变更事件
        /// </summary>
        /// <param name="sender">分类</param>
        /// <param name="e">属性变更事件参数</param>
        private void Category_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var category = sender as ItemCategory;
            if (e.PropertyName == nameof(ItemCategory.Level))
            {
                if (category.Level != null)
                    category.Type = category.Level.Type;
            }
        }
    }

    /// <summary>
    /// 分类添加命令
    /// </summary>
    [Command(ImageName = "CopyEntity", Label = "复制添加", ToolTip = "复制添加数据", Gestures = "Ctrl+Shift+N", GroupType = 10)]
    class CopyCategoryCommand : ListCopyCommand
    {
        /// <summary>
        /// 实体创建后事件
        /// </summary>
        /// <param name="entity">分类</param>
        protected override void OnItemCreated(Entity entity)
        {
            base.OnItemCreated(entity);
            var category = entity as ItemCategory;
            category.PropertyChanged += Category_PropertyChanged;
        }

        /// <summary>
        /// 分类属性变更事件
        /// </summary>
        /// <param name="sender">分类</param>
        /// <param name="e">属性变更事件参数</param>
        private void Category_PropertyChanged(object sender, PropertyChangedEventArgs e)
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
