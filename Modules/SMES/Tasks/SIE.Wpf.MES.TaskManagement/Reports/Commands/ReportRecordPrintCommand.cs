using SIE.Common.Prints;
using SIE.MES.TaskManagement.Reports;
using SIE.Wpf.Command;
using System;
using System.Collections.Generic;

namespace SIE.Wpf.MES.TaskManagement.Reports.Commands
{
    /// <summary>
    /// 报工记录打印 命令
    /// </summary>
    [Command(ImageName = "PrintData", Label = "打印", ToolTip = "打印", GroupType = CommandGroupType.Edit)]
    public class ReportRecordPrintCommand : ListViewCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.Current != null;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var reportRecord = view.Current as ReportRecord;
            var vm = new TaskReportViewModel();
            var ui = new DetailsUITemplate(vm.GetType(), TaskReportViewModelViewConfig.taskReportPrintView, view.ModuleKey).CreateUI();
            ui.MainView.Data = vm;
            
             CRT.Workbench.ShowDialog(ui, w =>
            {
                w.Width = 400;
                w.Height = 150;
                w.Title = "报工打印";
                w.Closing += (s, e) =>
                {
                    if (w.Result == 0)
                    {
                        if (vm.Printer.IsNullOrEmpty() || vm.Template == null)
                            CRT.MessageService.ShowError("请选择打印机和打印模板".L10N());
                        else
                        {
                            var filePath = RT.Service.Resolve<PrintsController>().DownloadPrintTemplate(vm.TemplateId.Value);
                            new TaskReportPrintCommand().PrintTaskReportLabel(ui.MainView.ViewId, filePath, vm, new List<ReportRecord>() { reportRecord });
                        }
                    }
                };
            });
        }
    }

}
