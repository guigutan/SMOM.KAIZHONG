using DevExpress.Xpf.Editors;
using SIE.Barcodes.Printables;
using SIE.Barcodes.WipBatchs;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Wpf.Barcodes.WipBatchs.ViewModels;
using SIE.Wpf.Command;
using SIE.Wpf.Common.Prints;
using SIE.Wpf.Controls.WaitProgress;
using System;
using System.Linq;
using System.Threading;
using System.Windows;

namespace SIE.Wpf.Barcodes.WipBatchs.Commands
{
    /// <summary>
    /// 生成并打印命令
    /// </summary>
    [Command(ImageName = "PrintData", Label = "生成并打印", ToolTip = "生成并打印批次", GroupType = 20)]
    public class GenerateAndPrintCommand : DetailViewCommand
    {
        /// <summary>
        /// 条件执行判断
        /// </summary>
        /// <param name="view">逻辑视图</param>
        /// <returns>条件结果</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            var model = view.Data as BatchGeneratingViewModel;
            return base.CanExecute(view) && model.NotGenerateQty > 0 && model.GenerateingQty > 0 && model.BatchQty > 0;
        }

        /// <summary>
        /// 命令执行方法
        /// </summary>
        /// <param name="view">逻辑视图</param>
        public override void Execute(DetailLogicalView view)
        {
            var model = view.Data as BatchGeneratingViewModel;
            var brokenList = model.Validate();
            if (brokenList.Count > 0)
            {
                CRT.MessageService.ShowError(brokenList.ToString(";\r\n", MetaModel.RuleLevel.Error));
                return;
            }

            if (model.Template == null)
                throw new ValidationException("打印模板不能为空".L10N());
            BatchBarcodeInfo info = new BatchBarcodeInfo
            {
                WorkOrderId = (double)model.BatchWoId,
                NumberRuleId = (double)model.NumberRuleId,
                BatchQty = model.BatchQty,
                GeneratingQty = model.GenerateingQty,
                GenerateChild = model.GenerateChildren,
                ChildNumberRuleId = model.ChildNumberRuleId.HasValue ? (double)model.ChildNumberRuleId : 0,
                ChildBatchQty = model.ChildBatchQty
            };

            Tuple<BatchWorkOrder, EntityList<WipBatch>> batchWoAndBarcodes = null;

            var win = new WaitDialog();
            win.Width = 500;
            win.WindowStyle = WindowStyle.None;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Topmost = true;
            win.GetLogicalChild<ProgressBarEdit>().StyleSettings = new ProgressBarMarqueeStyleSettings();
            win.ShowInTaskbar = false;
            win.Text = "生成中".L10N();

            Exception exception = null;
            ThreadPool.QueueUserWorkItem(oo =>
            {
                try
                {
                    batchWoAndBarcodes = RT.Service.Resolve<WipBatchController>().BatchsGenerating(info);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                Action ac = () => win.DialogResult = true;
                win.Dispatcher.BeginInvoke(ac);
            });

            win.ShowDialog();

            if (exception == null && batchWoAndBarcodes != null)
            {
                var batchWo = batchWoAndBarcodes.Item1;
                var barcodes = batchWoAndBarcodes.Item2;

                var filePath = RT.Service.Resolve<PrintsController>().DownloadPrintTemplate(model.TemplateId.Value);
                IPrintable printable = new WipBatchPrintable();
                if (model.GenerateChildren)
                    printable = new WipBatchPrintable();
                var report = ReportFactory.Current.GetReportByExtension(model.Template.Type);

                try
                {
                    report.Print(printable, filePath, model.Printer,
                        () =>
                    {
                        if (model.PrintControl)
                            return barcodes.OrderByDescending(p => p.Id).ToList();
                        return barcodes;
                    },
                        () =>
                    {
                        batchWo = RT.Service.Resolve<WipBatchController>().SavePrintedBarcodes(batchWo, barcodes);
                        CRT.MessageService.ShowMessage("打印成功".L10N(), "操作提示".L10N());
                    }, (short)model.PageCount);
                }
                catch (Exception exc)
                {
                    batchWo = RT.Service.Resolve<WipBatchController>().RemoveBarcodesOnFailedPrint(batchWo, barcodes);
                    CRT.MessageService.ShowException(exc);
                }

                decimal generatedQty = RT.Service.Resolve<WipBatchController>().CountGenerateBarcode(batchWo.Id);
                model.BatchWo = batchWo;
                model.GeneratedQty = generatedQty;
                model.NotGenerateQty = batchWo.PlanQty - generatedQty;
                model.GenerateingQty = batchWo.PlanQty - generatedQty;
            }
            else
            {
                CRT.MessageService.ShowException(exception);
            }
        }
    }
}
