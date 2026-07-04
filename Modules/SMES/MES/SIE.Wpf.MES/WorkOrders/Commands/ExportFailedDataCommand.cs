using SIE.Domain;
using SIE.Wpf.Command;
using SIE.Wpf.MES.WorkOrders.ViewModels;

namespace SIE.Wpf.MES.WorkOrders.Commands
{
    /// <summary>
    /// 导出工单导入失败的数据
    /// </summary>
    [Command(ImageName = "ExportData", Label = "导出Excel", ToolTip = "导出数据", GroupType = CommandGroupType.View)]
    public class ExportFailedDataCommand : ExportCommand
    {
        /// <summary>
        /// 是否可执行导出Excel
        /// </summary>
        /// <param name="view">当前视图对象</param>
        /// <returns>返回是否可执行导出Excel</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var current = view.Data as EntityList<WorkOrderCheckDataViewModel>;
            if (base.CanExecute(view) && current != null && current.Count > 0)
            {
                return true;
            }

            return false;
        }
    }
}