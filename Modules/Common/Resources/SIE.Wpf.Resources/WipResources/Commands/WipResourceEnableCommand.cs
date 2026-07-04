using System;
using SIE.Wpf.Command;
using SIE.Resources.WipResources;
using SIE.Domain;
using System.Linq;

namespace SIE.Wpf.Resources.WipResources.Commands
{
    /// <summary>
    /// 启用生产资源
    /// </summary>
    [Command(ImageName = "Play", Label = "启用", ToolTip = "启用生产资源", GroupType = 100)]
    public class WipResourceEnableCommand : ListViewCommand
    {
        /// <summary>
        /// 是否执行启用操作
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>true/false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.SelectedEntities.OfType<WipResource>().Any(p => p.PersistenceStatus != PersistenceStatus.New && p.ResourceState != ResourceState.Actived);
        }

        /// <summary>
        /// 生产资源启用
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var toActive = view.SelectedEntities.OfType<WipResource>().Where(p => p.PersistenceStatus != PersistenceStatus.New && p.ResourceState != ResourceState.Actived).ToList();

            RT.Service.Resolve<WipResourceController>().EnableWipResource(toActive);
            CRT.MessageService.ShowMessage("启用成功".L10N());

            view.QueryView?.TryExecuteQuery();
        }
    }
}
