using SIE.Domain.Validation;
using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.MES.TaskManagement.Reports.Commands
{
    /// <summary>
    /// 首件报检 命令
    /// </summary>
    [Command(ImageName = "TextRelease",
        Label = "首件报检",
        ToolTip = "首件报检",
        GroupType = CommandGroupType.Edit)]
    public class FirstInspReportCommand : DetailViewCommand
    {
        /// <summary>
        /// 是否可执行的逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            var vm = view.Current as TaskReportViewModel;
            return base.CanExecute(view) && vm.DispatchTask != null && vm.DispatchTask.TaskStatus == SIE.MES.TaskManagement.Dispatchs.DispatchTaskStatus.Executing 
                && !vm.DispatchTask.IsVirtualPart && vm.DispatchTask.SpecificationCode.IsNullOrEmpty();
        }

        /// <summary>
        /// 执行具体的逻辑。
        /// </summary>
        /// <param name="view">视图对象</param>
        public override void Execute(DetailLogicalView view)
        {
            var vm = view.Current as TaskReportViewModel;
            vm.InspQty = 0;

            var ui = new DetailsUITemplate(vm.GetType(), TaskReportViewModelViewConfig.dispatchTaskInspView, view.ModuleKey).CreateUI();
            ui.MainView.Data = vm;

            CRT.Workbench.ShowDialog(ui, w =>
            {
                w.Width = 400;
                w.Height = 200;
                w.Title = "首件报检";
                w.Closing += (s, e) =>
                {
                    if (w.Result == 0)
                    {
                        if (vm.InspQty <= 0)
                            CRT.MessageService.ShowError("报检数量必须为正整数！".L10N());
                        else 
                        {
                            vm.DispatchTask.InspQty = vm.InspQty;
                            vm.ReportFirstInsp();
                        } 
                    }
                };
            });
        }
    }
}
