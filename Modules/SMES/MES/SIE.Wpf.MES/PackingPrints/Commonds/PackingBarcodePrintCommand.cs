using DevExpress.Xpf.Editors;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.PackingPrints;
using SIE.Wpf.Command;
using SIE.Wpf.Common.Prints;
using SIE.Wpf.Controls.WaitProgress;
using SIE.Wpf.MES.PackingPrints.ViewModels;
using System;
using System.Linq;
using System.Threading;
using System.Windows;

namespace SIE.Wpf.MES.PackingPrints.Commonds
{
    /// <summary>
    /// 包装号打印按钮
    /// </summary>
    [Command(ImageName = "Printer", Label = "打印", ToolTip = "打印", GroupType = CommandGroupType.Edit)]
    public class PackingBarcodePrintCommand : ListAddCommand
    {
        /// <summary>
        /// 判断包装号打印命令能否执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>选中返回true，否则返回false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var workOrder = view.Current as PackingWorkOrder;
            return workOrder != null;
        }

        /// <summary>
        /// 单体打印命令执行方法
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            PackingBarcodePrintViewModel entity = CreateEntity();
            ControlResult ui = new DetailsUITemplate(entity.GetType(), ViewConfig.DetailsView, view.ModuleKey).CreateUI();
            ui.MainView.Data = entity;
            CRT.Workbench.ShowDialog(ui, w =>
            {
                w.Height = 480;
                w.Width = 1000;
                w.Title = "包装号打印".L10N();
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
        private PackingBarcodePrintViewModel CreateEntity()
        {
            var workOrder = View.Current as PackingWorkOrder;
            var model = new PackingBarcodePrintViewModel() 
            {
                WorkOrder = workOrder,
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
            var viewModel = view.Current as PackingBarcodePrintViewModel;
            var f = viewModel != null && viewModel.PrintQty > 0;
            if (!f)
                return false;
            if (viewModel.Template == null)
                return false;
            return true;
        }

        /// <summary>
        /// 执行内容
        /// </summary>
        /// <param name="view">当前明细逻辑视图</param>
        public override void Execute(DetailLogicalView view)
        {
            var entity = view.Current as PackingBarcodePrintViewModel;
            if (!ValidatePrint(entity))
                return;
            var filePath = RT.Service.Resolve<PrintsController>().DownloadPrintTemplate(entity.TemplateId.Value);
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
        private void PrintBarcodes(string viewId, PackingBarcodePrintViewModel entity, string filePath, EntityList<PackingBarcode> barcodes)
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
                    report.Print(new PackingPrintable(), filePath, entity.Printer, () =>
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
        private Tuple<Exception, EntityList<PackingBarcode>> GenerateBarcodes(PackingBarcodePrintViewModel entity)
        {
            Exception exception = null;
            EntityList<PackingBarcode> barcodes = null;
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
                    var info = new PackingPrinterInfo(entity.WorkOrderId, entity.NumberRuleId.Value, Convert.ToDouble(entity.PackageRuleDetailId), entity.TemplateId.Value, entity.PrintQty, entity.PageCount);
                    barcodes = RT.Service.Resolve<PackingBarcodeController>().Print(info); // 先保存再调用打印机
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                Action ac = () => win.DialogResult = true;
                win.Dispatcher.BeginInvoke(ac);
            });

            win.ShowDialog();
            return new Tuple<Exception, EntityList<PackingBarcode>>(exception, barcodes);
        }

        /// <summary>
        /// 验证打印信息
        /// </summary>
        /// <param name="vm">包装号打印视图模型</param>
        /// <exception cref="ValidationException">验证失败异常</exception>
        /// <returns>验证通过返回true，否则返回false</returns>
        bool ValidatePrint(PackingBarcodePrintViewModel vm)
        {
            if (vm == null)
                return false;
            var curTemplate = RF.GetById<PrintTemplate>(vm.TemplateId);
            if (curTemplate == null)
                throw new EntityNotFoundException(typeof(PrintTemplate), vm.TemplateId);
            if (curTemplate.EntityType != typeof(PackingPrintable).GetQualifiedName())
                throw new ValidationException("打印模板错误，请配置【包装号】类型的模板！".L10N());
            var broken = vm.Validate();
            if (broken.Count > 0)
                throw new ValidationException(broken.ToString());
            return true;
        }
    }

}
