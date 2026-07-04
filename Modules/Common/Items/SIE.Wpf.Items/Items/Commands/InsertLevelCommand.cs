using SIE.Domain;
using SIE.Items;
using SIE.Wpf.Command;

namespace SIE.Wpf.Items
{
    /// <summary>
    /// 插入分类层级命令
    /// </summary>
    [Command(Label = "插入", Location = MetaModel.View.CommandLocation.Menu, GroupType = CommandGroupType.Edit)]
    class InsertLevelCommand : TreeInsertCommand
    {
        /// <summary>
        /// 设置子层级类型与父层级类型一致
        /// </summary>
        /// <param name="entity">分类层级</param>
        protected override void OnItemCreated(Entity entity)
        {
            var current = View.Current as ItemCategoryLevel;
            var level = entity as ItemCategoryLevel;
            if (level.TreePId != null)
            {
                level.Type = current.Type;
            }
        }
    }
}
