using SIE.Wpf.Command;

namespace SIE.Wpf.MES.Storages.Commands
{
    /// <summary>
    /// 附加列表通用保存命令
    /// </summary>
    [Command(ImageName = "SaveEntity",
    Label = "保存",
    ToolTip = "保存数据",
    Gestures = "Ctrl+Shift+S",
    GroupType = CommandGroupType.Edit)]
    public class AttachChildSaveCommand : ListSaveCommand
    {
        /// <summary>
        /// 保存后事件
        /// </summary>
        /// <param name="view">逻辑视图</param>
        protected override void OnSaved(ListLogicalView view)
        {
            base.OnSaved(view);
            if (view.DataLoader.AnyLoaded)
                view.DataLoader.ReloadDataAsync();
        }
    }
}