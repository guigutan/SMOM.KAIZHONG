using SIE.Wpf.Command;
using System;
using System.Windows;

namespace SIE.Wpf.MES.WIP.SuspectReport.Commands
{
    /// <summary>
    /// 提交
    /// </summary>
    [Command(ImageName = "Check", Label = "提交", ToolTip = "提交", GroupType = CommandGroupType.Edit)]
    public class SubmitSuspectReportCommand : DetailViewCommand
    {
        /// <summary>
        /// 是否可执行的逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            return base.CanExecute(view) && (view.Current as SuspectReportViewModel) != null;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="view"></param>
        public override void Execute(DetailLogicalView view)
        {

            //采集信息
            var vm = view.Current as SuspectReportViewModel;

            vm.Submit();

        }
    }
}
