using SIE.Wpf.Command;
using System;
using System.Windows;

namespace SIE.Wpf.MES.WIP.SuspectReport.Commands
{
    /// <summary>
    /// 提交
    /// </summary>
    [Command(ImageName = "Printer", Label = "打印", ToolTip = "打印", GroupType = CommandGroupType.Edit)]
    public class PrintSuspectLabelCommand : DetailViewCommand
    {
        /// <summary>
        /// 是否可执行的逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            return base.CanExecute(view) && (view.Current as SuspectReportViewModel) != null && (view.Current as SuspectReportViewModel).SuspectLabel != null;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="view"></param>
        public override void Execute(DetailLogicalView view)
        {

            //采集信息
            var vm = view.Current as SuspectReportViewModel;

            vm.Print();

        }
    }
}
