using SIE.MetaModel.View;
using SIE.Resources.WipResources;
using SIE.Wpf.Command;
using System.Linq;

namespace SIE.Wpf.Resources.WipResources.Commands
{
    /// <summary>
    /// 删除生产资源
    /// </summary>
    [Command(ImageName = "DeleteEntity", Label = "删除", ToolTip = "删除数据", Gestures = "Delete", Location = CommandLocation.All, GroupType = 10)]
    public class WipResourceDeleteCommand : ListDeleteCommand
    {
        /// <summary>
        /// 删除按钮是否可用
        /// </summary>
        /// <param name="view">生产资源视图实体</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.SelectedEntities.OfType<WipResource>().All(item => item.ResourceState != ResourceState.Actived /*&& item.SourceType == SyncSourceType.Custom*/);
        }
    }
}
