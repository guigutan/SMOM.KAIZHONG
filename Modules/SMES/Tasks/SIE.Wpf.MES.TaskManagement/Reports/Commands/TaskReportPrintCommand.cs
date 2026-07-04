using DevExpress.Xpf.Editors;
using SIE.Common.Prints;
using SIE.MES.TaskManagement.Reports;
using SIE.Wpf.Command;
using SIE.Wpf.Common.Prints;
using SIE.Wpf.Controls.WaitProgress;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace SIE.Wpf.MES.TaskManagement.Reports.Commands
{
    /// <summary>
    /// 报工并打印 命令
    /// </summary>
    [Command(ImageName = "PrintData",
        Label = "报工并打印",
        ToolTip = "报工并打印",
        GroupType = CommandGroupType.Edit)]
    public class TaskReportPrintCommand : DetailViewCommand
    {
        /// <summary>
        /// 是否可执行的逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            var vm = view.Current as TaskReportViewModel;
            return base.CanExecute(view) && vm.DispatchTask != null && vm.DispatchTask.TaskStatus == SIE.MES.TaskManagement.Dispatchs.DispatchTaskStatus.Executing;
        }

        /// <summary>
        /// 执行具体的逻辑。
        /// </summary>
        /// <param name="view">视图对象</param>
        public override void Execute(DetailLogicalView view)
        {
            var vm = view.Current as TaskReportViewModel;
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
                            var reportRecord = vm.TaskReport();
                            if (reportRecord != null) 
                            {
                                var filePath = RT.Service.Resolve<PrintsController>().DownloadPrintTemplate(vm.TemplateId.Value);
                                PrintTaskReportLabel(ui.MainView.ViewId, filePath, vm,new List<ReportRecord>() { reportRecord });
                            }
                            else
                                CRT.MessageService.ShowError("报工失败".L10N());
                        }
                    }
                };
            });
        }

        /// <summary>
        /// 打印条码
        /// </summary>
        /// <param name="viewId">视图ID</param>
        /// <param name="filePath">模板路径</param>
        /// <param name="entity">条码打印视图模型</param>
        /// <param name="reportRecordList">报工记录</param>
        public void PrintTaskReportLabel(string viewId, string filePath, TaskReportViewModel entity, List<ReportRecord> reportRecordList)
        {
            Exception exception = null;
            var win = new WaitDialog();
            win.Width = 500;
            win.WindowStyle = WindowStyle.None;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Topmost = true;
            win.GetLogicalChild<ProgressBarEdit>().StyleSettings = new ProgressBarMarqueeStyleSettings();
            win.ShowInTaskbar = false;
            win.Text = "打印中".L10N();
            ThreadPool.QueueUserWorkItem(oo =>
            {
                try
                {
                    IPrintable printable = new ReportRecordPrintable();
                    var report = ReportFactory.Current.GetReportByExtension(entity.Template.Type);
                    report.Print(printable, filePath, entity.Printer, () =>
                    {
                        return reportRecordList;
                    }, () => { });
                }
                catch (Exception exc)
                {
                    exception = exc;
                }

                Action ac = () => win.DialogResult = true;
                win.Dispatcher.BeginInvoke(ac);
            });

            win.ShowDialog();
            if (exception != null)
                exception.Alert();
            else
                CRT.MessageService.ShowMessage("打印成功".L10N(), "操作提示".L10N());
        }
    }
}
