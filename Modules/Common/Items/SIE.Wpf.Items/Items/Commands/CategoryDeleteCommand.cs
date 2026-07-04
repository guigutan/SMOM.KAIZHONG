using SIE.Items;
using SIE.MetaModel.View;
using SIE.Wpf.Command;

namespace SIE.Wpf.Items
{
    /// <summary>
    /// 分类删除命令
    /// </summary>
    [Command(ImageName = "DeleteEntity",
        Label = "删除",
        ToolTip = "删除数据",
        Gestures = "Delete",
        Location = CommandLocation.All,
        GroupType = CommandGroupType.Edit)]
    class CategoryDeleteCommand : ListDeleteCommand
    {
        /// <summary>
        /// 是否可删除
        /// </summary>
        public bool? CanDelete;

        /// <summary>
        /// 是否可删除命令
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>true可删除，false不可删除</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            if (CanDelete.HasValue)
            {
                return CanDelete.Value;
            }

            var current = view.Current as ItemCategory;
            if (view.Current != null && current != null)
            {
                var resutl = !RT.Service.Resolve<ItemController>().HasChildCategory(current.Id);
                CanDelete = resutl;
                return CanDelete.Value;
            }

            return false;
        }
    }
}
