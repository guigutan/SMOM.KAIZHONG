using System;
using SIE.Wpf.Command;
using SIE.Resources.WipResources;
using System.Linq;

namespace SIE.Wpf.Resources.WipResources.Commands
{
    /// <summary>
    /// 停用生产资源命令
    /// </summary>
    [Command(ImageName = "Cancel", Label = "停用", ToolTip = "停用生产资源", GroupType = 100)]
    public class WipResourceStopCommand : ListViewCommand
    {
        /// <summary>
        /// 判断停用资源命令能否执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>能执行返回true，不能执行返回false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.SelectedEntities.OfType<WipResource>().Any(p => p.ResourceState == ResourceState.Actived);
        }

        /// <summary>
        /// 停用资源命令执行方法
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var data = view.SelectedEntities.OfType<WipResource>().Where(p => p.ResourceState == ResourceState.Actived).ToList();
            if (CRT.MessageService.AskQuestion("确认禁用选择的[{0}]条数据吗？".L10nFormat(data.Count)))
            {
                RT.Service.Resolve<WipResourceController>().DisableWipResource(data);
                CRT.MessageService.ShowMessage("禁用成功".L10N());
                view.QueryView?.TryExecuteQuery();
            }
        }
    }
}
