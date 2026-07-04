using SIE.MetaModel.View;
using SIE.Wpf.Command;

namespace SIE.Wpf.Resources.ProcessTechs.Commands
{
    /// <summary>
    /// 导出制程工艺
    /// </summary>
    [Command(ImageName = "ExportData", Label = "导出", ToolTip = "导出数据", Location = CommandLocation.All, GroupType = CommandGroupType.Edit)]
    public class ExportProcessTechCommand : ExportCommand
    {
        /// <summary>
        /// 制程工艺导出命令是否可使用
        /// </summary>
        /// <param name="view">制程工艺视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.Data != null && view.Data.Count > 0;
        }
    }
}
