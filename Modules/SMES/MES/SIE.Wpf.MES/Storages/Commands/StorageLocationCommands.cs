using SIE.Domain;
using SIE.MES.Storages;
using SIE.Wpf.Command;

namespace SIE.Wpf.MES.Storages.Commands
{
    /// <summary>
    /// 货位的添加命令
    /// </summary>
    [Command(ImageName = "AddEntity",
    Label = "添加",
    ToolTip = "添加数据",
    Gestures = "Ctrl+Shift+N",
    GroupType = CommandGroupType.Edit)]
    public class AddStorageLocationCommand : ListAddCommand
    {
        /// <summary>
        /// 新实体创建后-提供扩展（给予关系表绑定引用值）
        /// </summary>
        /// <param name="entity">新增的实体</param>
        protected override void OnItemCreated(Entity entity)
        {
            base.OnItemCreated(entity);
            (entity as StorageLocation).StorageArea = View.Parent?.Current as StorageArea;
        }
    }
}