using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Resources.WipResources;
using SIE.Wpf.Command;

namespace SIE.Wpf.Resources.WipResources.Commands
{
    /// <summary>
    /// 复制生产资源
    /// </summary>
    [Command(ImageName = "ContentCopy", Label = "复制添加", ToolTip = "复制并添加数据", Gestures = "Ctrl+Shift+C", Location = CommandLocation.All, GroupType = 10)]
    public class WipResourceCopyCommand : ListCopyCommand
    {
        /// <summary>
        /// 创建编辑项
        /// </summary>
        /// <param name="entity">生产资源</param>
        protected override void OnItemCreated(Entity entity)
        {
            base.OnItemCreated(entity);
            var item = entity as WipResource;
            item.Code = RT.Service.Resolve<WipResourceController>().GetResourceNo();
            item.Name = string.Format("{0}-副本", item.Name);
            item.ResourceState = ResourceState.Unused;
        }

        /// <summary>
        /// 复制新增是否可用
        /// </summary>
        /// <param name="view">实体</param>
        /// <returns></returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var item = view.Current as WipResource;
            return (item != null) /*&& (item.SourceType == SyncSourceType.Custom)*/;
        }
    }
}
