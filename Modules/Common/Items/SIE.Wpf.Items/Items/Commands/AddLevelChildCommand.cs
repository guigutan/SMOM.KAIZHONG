using SIE.Domain;
using SIE.Items;
using SIE.Wpf.Command;

namespace SIE.Wpf.Items
{
    /// <summary>
    /// 添加分类层级子命令
    /// </summary>
    [Command(Label = "添加子", Location = MetaModel.View.CommandLocation.Menu, GroupType = CommandGroupType.Edit)]
    class AddLevelChildCommand : TreeAddChildCommand
    {
        /// <summary>
        /// 设置子层级类型与父层级类型一致
        /// </summary>
        /// <param name="entity">分类层级</param>
        protected override void OnItemCreated(Entity entity)
        {
            var parent = View.Control.CurrentItem as ItemCategoryLevel;
            var level = entity as ItemCategoryLevel;
            level.Type = parent.Type;
        }
    }
}
