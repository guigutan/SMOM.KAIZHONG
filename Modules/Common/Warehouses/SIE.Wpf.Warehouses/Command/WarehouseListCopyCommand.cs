using SIE.Domain;
using SIE.Warehouses;
using SIE.Wpf.Command;

namespace SIE.Wpf.Warehouses.Command
{
    /// <summary>
    /// 仓库复制新增命令
    /// </summary>
    [Command(ImageName = "CopyEntity", Label = "复制添加", ToolTip = "复制添加", GroupType = CommandGroupType.Edit)]
    public class WarehouseListCopyCommand : ListCopyCommand
    {
        /// <summary>
        /// 子类重写此方法来返回是否可执行的逻辑
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>是否可执行</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            if (view.Current != null && view.SelectedEntities.Count == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 复制新增时清空简码
        /// </summary>
        /// <param name="entity">仓库实体</param>
        protected override void OnItemCreated(Entity entity)
        {
            base.OnItemCreated(entity);
            var warehouse = entity as Warehouse;
            warehouse.SimpleCode = string.Empty;
        }
    }
}
