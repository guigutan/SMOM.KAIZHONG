using SIE.Wpf.Command;

namespace SIE.Wpf.Resources.WipResources.Commands
{
    /// <summary>
    /// 导出生产资源
    /// </summary>
    [Command(ImageName = "ExportData", Label = "导出", ToolTip = "导出数据", GroupType = 110)]
    public class WipResourceExportCommand : ExportCommand
    {
        /// <summary>
        /// 生产资源导出命令是否可使用
        /// </summary>
        /// <param name="view">生产资源视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.Data != null && view.Data.Count > 0;
        }
    }
}
