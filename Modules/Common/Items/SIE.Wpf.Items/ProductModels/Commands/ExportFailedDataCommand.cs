using SIE.Domain;
using SIE.Wpf.Command;
using SIE.Wpf.Items.ProductModels.ViewModels;

namespace SIE.Wpf.Items.ProductModels.Commands
{
    /// <summary>
    /// 导出导入失败数据
    /// </summary>
    [Command(ImageName = "ExportData", Label = "导出Excel", ToolTip = "导出数据", GroupType = CommandGroupType.View)]
    public class ExportFailedDataCommand : ExportCommand
    {
        /// <summary>
        /// 是否可执行导出命令
        /// </summary>
        /// <param name="view">列表视图</param>
        /// <returns>true/false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var current = view.Data as EntityList<ProductModelCheckDataViewModel>;
            if (base.CanExecute(view) && current != null && current.Count > 0)
            {
                return true;
            }

            return false;
        }
    }
}
