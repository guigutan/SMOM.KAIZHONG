using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Tech.Processs;
using SIE.Wpf.Command;

namespace SIE.Wpf.Tech.Processs.Commands
{
    /// <summary>
    /// 工序对应包装选择命令
    /// </summary>
    [Command(ImageName = "PlaylistCheck",
        Label = "选择", ToolTip = "选择",
        GroupType = CommandGroupType.Edit,
        DisplayMode = CommandDisplayMode.LabelAndIcon)]
    class LookupPackingUnitCommand : LookupCommand
    {
        /// <summary>
        /// 能否执行弹出
        /// </summary>
        /// <param name="view">当前列表逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var process = (view.Parent?.Current as Process);
            return process != null && process.PersistenceStatus != PersistenceStatus.New && process.Type == ProcessType.Packing;
        }
    }
}
