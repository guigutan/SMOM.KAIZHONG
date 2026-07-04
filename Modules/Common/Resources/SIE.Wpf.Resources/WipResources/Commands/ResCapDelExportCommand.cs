using SIE.Wpf.Command;

namespace SIE.Wpf.Resources.WipResources.Commands
{
    /// <summary>
    /// 详细产能导出命令
    /// </summary>
    [Command(ImageName = "ExportData", Label = "导出", ToolTip = "导出数据", GroupType = 20)]
    public class ResCapDelExportCommand : ExportCommand
    {
        /// <summary>
        /// 判断导出命令能否执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>能执行返回true，否则返回false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.Data != null && view.Data.Count > 0;
        }
    }
}
