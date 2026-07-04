using SIE.Domain;
using SIE.MES.Storages;
using SIE.Wpf.Command;

namespace SIE.Wpf.MES.Storages.Commands
{
    /// <summary>
    /// 工位货区复制新增按钮
    /// </summary>
    [Command(ImageName = "ContentCopy", Label = "复制添加", ToolTip = "复制并添加数据", GroupType = CommandGroupType.Edit)]
    public class StorageAreaCopyCommand : ListCopyCommand
    {
        /// <summary>
        /// 是否可以执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.Current != null && base.CanExecute(view);
        }

        /// <summary>
        /// 实体创建后执行复制操作
        /// </summary>
        /// <param name="entity">实体</param>
        protected override void OnItemCreated(Entity entity)
        {
            var sourceStorageArea = View.Current as StorageArea;
            var descStorageArea = entity as StorageArea;
            descStorageArea.Code = sourceStorageArea.Code + "-复制";
            descStorageArea.Name = sourceStorageArea.Name + "-复制";
            descStorageArea.Type = sourceStorageArea.Type;
            descStorageArea.Warehouse = sourceStorageArea.Warehouse;
        }
    }
}