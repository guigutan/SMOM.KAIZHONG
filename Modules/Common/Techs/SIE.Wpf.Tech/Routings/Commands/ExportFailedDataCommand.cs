using SIE.Domain;
using SIE.Wpf.Command;
using SIE.Wpf.Tech.Routings.ViewModels;

namespace SIE.Wpf.Tech.Routings.Commands
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
            var current = view.Data as EntityList<RoutingCheckDataViewModel>;
            if (base.CanExecute(view) && current != null && current.Count > 0)
            {
                return true;
            }

            return false;
        }
    }

    /// <summary>
    /// 清除导入结果
    /// </summary>
    [Command(ImageName = "Clear", Label = "清除结果", ToolTip = "清除导入结果", GroupType = CommandGroupType.View)]
    public class ClearImportDateCommand : ListViewCommand
    {
        /// <summary>
        /// 清除导入结果命令能否执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>能执行返回true，不能执行返回false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.Data.Count > 0;
        }

        /// <summary>
        /// 清除导入结果命令执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            view.Data.Clear();
        }
    }
}