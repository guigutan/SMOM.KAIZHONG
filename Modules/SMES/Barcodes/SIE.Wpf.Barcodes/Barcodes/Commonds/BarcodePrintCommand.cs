using DevExpress.Xpf.Editors;
using SIE.Barcodes;
using SIE.Barcodes.Printables;
using SIE.Common.Prints;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Wpf.Barcodes.ViewModels;
using SIE.Wpf.Command;
using SIE.Wpf.Common.Prints;
using SIE.Wpf.Controls.WaitProgress;
using System;
using System.Linq;
using System.Threading;
using System.Windows;

namespace SIE.Wpf.Barcodes.Commonds
{
    /// <summary>
    /// 条码打印按钮
    /// </summary>
    [Command(ImageName = "Printer", Label = "单体打印", ToolTip = "单体打印", GroupType = CommandGroupType.Edit)]
    public class BarcodePrintCommand : ListAddCommand
    {
        /// <summary>
        /// 判断单体打印命令能否执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>工单已打印数量小于工单计划数量返回true，否则返回false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var workOrder = view.Current as PrintWorkOrder;
            ////如果工单是返工工单并设置了使用旧条码，不能打印
            if (workOrder != null && workOrder.UseOldSn)
                return false;
            return workOrder != null && workOrder.PrintedQty < (int)workOrder.PlanQty && workOrder.State != WorkOrderState.CancelRelease;
        }

        /// <summary>
        /// 单体打印命令执行方法
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            BarcodePrintViewModel entity = CreateEntity();
            if (entity.ResidualQty == 0)
            {
                CRT.MessageService.ShowMessage("剩余数量为0，不可打印!".L10N(), "操作提示".L10N());
                return;
            }

            ControlResult ui = new DetailsUITemplate(entity.GetType(), ViewConfig.DetailsView, view.ModuleKey).CreateUI();
            ui.MainView.Data = entity;
            if (entity.Template != null && entity.Template.EntityType != typeof(BarcodePrintable).GetQualifiedName())
            {
                entity.Template = null;
                CRT.MessageService.ShowWarning("工单[{0}]打印设置的默认标签模板不是条码类型的，请重新选择打印模板".L10nFormat((View.Current as PrintWorkOrder).No));
            }

            CRT.Workbench.ShowDialog(ui, w =>
            {
                w.Height = 480;
                w.Width = 1000;
                w.Title = "单体打印 条码打印";
                w.Commands.Clear();
            });
            ReloadData(view);
        }

        /// <summary>
        /// 重新加载数据
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        private void ReloadData(ListLogicalView view)
        {
            if (view.DataLoader.AnyLoaded)
                view.DataLoader.ReloadDataAsync();
            else
                view.DataLoader.LoadDataAsync();
        }

        /// <summary>
        /// 创建新实体-提供扩展
        /// </summary>
        /// <returns>新实体</returns>
        private BarcodePrintViewModel CreateEntity()
        {
            var controller = RT.Service.Resolve<BarcodeController>();
            var workOrder = View.Current as PrintWorkOrder;
            var printedQty = workOrder.PrintedQty;
            var scrapedQty = controller.GetScrapBarcodeCount(workOrder.Id);
            int residualQty = (int)workOrder.PlanQty - printedQty;
            var model = new BarcodePrintViewModel()
            {
                WorkOrder = workOrder,
                DumpingQty = scrapedQty,
                ResidualQty = residualQty,
                PrintQty = residualQty,
                PrintedQty = printedQty,
                NumberRule = workOrder.Template?.NumberRule,
                Template = workOrder.Template?.LabelTemplate,
            };

            return model;
        }
    }

    /// <summary>
    /// 打印命令
    /// </summary>
    [Command(ImageName = "Printer", Label = "打印", ToolTip = "打印", GroupType = 10)]
    public class PrintCommand : DetailViewCommand
    {
        /// <summary>
        /// 是否能执行
        /// </summary>
        /// <param name="view">当前明细逻辑视图</param>
        /// <returns>能执行返回true，否则返回false</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            var viewModel = view.Current as BarcodePrintViewModel;
            return viewModel != null && viewModel.ResidualQty > 0 && viewModel.PrintQty > 0 && viewModel.SingleQty > 0 && viewModel.SingleQty <= viewModel.PrintQty;
        }

        /// <summary>
        /// 执行内容
        /// </summary>
        /// <param name="view">当前明细逻辑视图</param>
        public override void Execute(DetailLogicalView view)
        {
            var entity = view.Current as BarcodePrintViewModel;
            if (!ValidatePrint(entity))
                return;
            var filePath = RT.Service.Resolve<PrintsController>().DownloadPrintTemplate(entity.TemplateId);
            var result = GenerateBarcodes(entity);
            if (result.Item1 != null)
            {
                CRT.MessageService.ShowException(result.Item1);
                return;
            }
            PrintBarcodes(view.ViewId, entity, filePath, result.Item2);
        }

        /// <summary>
        /// 打印条码
        /// </summary>
        /// <param name="viewId">视图ID</param>
        /// <param name="entity">条码打印视图模型</param>
        /// <param name="filePath">模板路径</param>
        /// <param name="barcodes">生成条码列表</param>
        private void PrintBarcodes(string viewId, BarcodePrintViewModel entity, string filePath, EntityList<Barcode> barcodes)
        {
            if (barcodes == null || barcodes.Count == 0)
                return;
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
                    var report = ReportFactory.Current.GetReportByExtension(entity.Template.Type);
                    report.Print(new BarcodePrintable(), filePath, entity.Printer, () =>
                    {
                        if (entity.PrintControl)
                            return barcodes.OrderByDescending(p => p.Id).ToList();
                        return barcodes;
                    }, () =>
                    {
                    }, (short)entity.PageCount);
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
            CRT.Workbench.Close(viewId);
        }

        /// <summary>
        /// 生成条码
        /// </summary>
        /// <param name="entity">条码打印视图模型</param>
        /// <returns>生成条码列表</returns>
        private Tuple<Exception, EntityList<Barcode>> GenerateBarcodes(BarcodePrintViewModel entity)
        {
            Exception exception = null;
            EntityList<Barcode> barcodes = null;
            var win = new WaitDialog();
            win.Width = 500;
            win.WindowStyle = WindowStyle.None;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Topmost = true;
            win.GetLogicalChild<ProgressBarEdit>().StyleSettings = new ProgressBarMarqueeStyleSettings();
            win.ShowInTaskbar = false;
            win.Text = "生成中".L10N();
            ThreadPool.QueueUserWorkItem(oo =>
            {
                try
                {
                    var info = new PrinterInfo(entity.WorkOrderId, entity.NumberRuleId, entity.TemplateId, entity.PrintQty, entity.SingleQty, entity.PrintedQty);
                    barcodes = RT.Service.Resolve<BarcodeController>().Print(info); // 先保存再调用打印机
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                Action ac = () => win.DialogResult = true;
                win.Dispatcher.BeginInvoke(ac);
            });

            win.ShowDialog();
            return new Tuple<Exception, EntityList<Barcode>>(exception, barcodes);
        }

        /// <summary>
        /// 验证打印信息
        /// </summary>
        /// <param name="vm">条码打印视图模型</param>
        /// <exception cref="ValidationException">验证失败异常</exception>
        /// <returns>验证通过返回true，否则返回false</returns>
        bool ValidatePrint(BarcodePrintViewModel vm)
        {
            if (vm == null)
                return false;
            var broken = vm.Validate();
            if (broken.Count > 0)
                throw new ValidationException(broken.ToString());
            return true;
        }
    }
}