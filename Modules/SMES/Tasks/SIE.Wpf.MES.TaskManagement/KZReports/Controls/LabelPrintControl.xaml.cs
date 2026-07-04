using DevExpress.Utils.Extensions;
using DevExpress.Xpf.CodeView;
using DevExpress.Xpf.Editors;
using SIE.Barcodes.Printables;
using SIE.Barcodes.WipBatchs;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.KZ.Print;
using SIE.KZ.Print.Common;
using SIE.Threading;
using SIE.Wpf.Common.Prints;
using SIE.Wpf.Controls.WaitProgress;
using System;
using System.Collections.ObjectModel;
using System.Drawing.Printing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using PrintInfo = SIE.KZ.Print.Common.PrintInfo;

namespace SIE.Wpf.MES.TaskManagement.KZReports.Controls
{
    /// <summary>
    /// LabelPrintControl.xaml 的交互逻辑
    /// </summary>
    public partial class LabelPrintControl : UserControl
    {
        KZTaskReportViewModelBase model;
        KZReportHelper kZReportHelper;
        bool AutoPrint = false;
        bool IsPrinting = false;
        /// <summary>
        /// 
        /// </summary>
        protected virtual ObservableCollection<PrintInfo> PrintInfoList { get; }

        /// <summary>
        /// 
        /// </summary>
        public LabelPrintControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_model"></param>
        /// <param name="_autoPrint"></param>
        public LabelPrintControl(KZTaskReportViewModelBase _model, bool _autoPrint = false)
        {
            InitializeComponent();
            this.model = _model;
            this.DataContext = _model;
            kZReportHelper = model.kZReportHelper;
            AutoPrint = _autoPrint;

            PrintInfoList = new ObservableCollection<PrintInfo>();
            this.dataGrid.ItemsSource = PrintInfoList;

            this.Loaded += Control_Loaded;
            this.Unloaded -= Control_Loaded;
        }


        private void Control_Loaded(object sender, RoutedEventArgs e)
        {
            // 加载打印机列表
            cbbPrinter.Items.Clear();
            try
            {
                foreach (string installedPrinter in PrinterSettings.InstalledPrinters)
                {
                    cbbPrinter.Items.Add(installedPrinter);
                }
                cbbPrinter.SelectedValue = model.Printer;
            }
            catch (PlatformNotSupportedException)
            {
                Console.WriteLine("此平台不支持访问已安装的打印机。");
            }

            //加载标签数据
            PrintInfoList.Clear();
            if (model.PrintData != null && model.PrintData.Data.Any())
            {
                PrintInfoList.AddRange(model.PrintData.Data);
            }
            this.dataGrid.SelectAll();

            //自动打印
            if (AutoPrint)
            {
                IsPrinting = true;
                btnSubmit.IsEnabled = false;
                Task.Run(new Action(async () =>
                {
                    int sec = 6;
                    while (sec > 0 && IsPrinting)
                    {
                        sec--;
                        var btnName = $"打印({sec})";
                        CRT.MainThread.InvokeIfRequired(() => { this.btnSubmit.Content = btnName; });
                        await Task.Delay(1000);
                    }
                    if (!IsPrinting)
                        return;
                    CRT.MainThread.InvokeIfRequired(() =>
                    {
                        var printData = model.PrintData;
                        if (cbbPrinter.SelectedValue != null)
                            printData.PrinterName = cbbPrinter.SelectedValue.ToString();
                        //printData.Data = dataGrid.SelectedItems.Cast<PrintInfo>().ToList();
                        PrintLabels(printData, model.IsLocalPrint);
                    });
                }).WithCurrentThreadContext());
            }
        }


        /// <summary>
        /// 重置
        /// </summary>
        void reset()
        {

        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        void close()
        {
            IsPrinting = false;
            var parent = Window.GetWindow(this);
            if (parent != null && parent is Window)
            {
                (parent as Window)?.Close();
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            close();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (model.PrintData == null)
            {
                CRT.MessageService.ShowWarning("打印数据异常!");
                return;
            }
            var printData = model.PrintData;
            printData.Data = dataGrid.SelectedItems.Cast<PrintInfo>().ToList();
            if (cbbPrinter.SelectedValue != null)
                printData.PrinterName = cbbPrinter.SelectedValue.ToString();
            PrintLabels(printData, model.IsLocalPrint);

        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="printData">打印数据</param>
        /// <param name="isLocalPrint">本地打印</param>
        /// <exception cref="ValidationException"></exception>
        public virtual void PrintLabels(WipBatchData printData, bool isLocalPrint = false)
        {
            IsPrinting = false;
            Exception exception = null;
            var win = new WaitDialog();
            win.Width = 300;
            win.WindowStyle = WindowStyle.None;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Topmost = true;
            win.GetLogicalChild<ProgressBarEdit>().StyleSettings = new ProgressBarMarqueeStyleSettings();
            win.ShowInTaskbar = false;
            win.Text = "正在打印...".L10N();
            ThreadPool.QueueUserWorkItem(oo =>
            {
                try
                {
                    model.PrintLabels(printData, isLocalPrint);

                    //if (isLocalPrint)
                    //{
                    //    //本地打印
                    //    PrintTemplate template = RT.Service.Resolve<PrinterController>().GetConfigPrintTemplate(printData);
                    //    if (template == null)
                    //        throw new ValidationException("打印模板不存在，请检查相关配置".FormatArgs());

                    //    if (printData.PrinterName.IsNullOrEmpty())
                    //        throw new ValidationException("打印机不能为空".L10nFormat(printData.ResourceCode));

                    //    //打印数据
                    //    var labelNos = printData.Data.Select(p => p.BatchNo).ToList();
                    //    var labels = RT.Service.Resolve<WipBatchController>().GetWipBatches(labelNos);

                    //    // 2.根据类型获取报表类型
                    //    var report = ReportFactory.Current.GetReportByExtension(template.Type);
                    //    // 3.要打印的数据对象实例
                    //    var printable = new WipBatchPrintable();

                    //    var filePath = RT.Service.Resolve<PrintsController>().DownloadPrintTemplate(template.Id);

                    //    foreach (var item in labels)
                    //    {
                    //        item.PrintProcessCode = printData.Data?.FirstOrDefault(p => p.BatchNo == item.BatchNo)?.ProcessCode;
                    //    }

                    //    report.Print(printable, filePath, printData.PrinterName, () =>
                    //    {
                    //        return labels;
                    //    }, () =>
                    //    {

                    //    });
                    //}
                    //else
                    //{
                    //    //API打印
                    //    printData.PrinterName = "";
                    //    RT.Service.Resolve<SIE.KZ.Print.PrinterController>().PrintLabels(printData);
                    //}
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
            {
                exception.Alert();
                btnSubmit.IsEnabled = true;
            }
            else
            {
                CRT.MessageService.ShowInstantMessage("打印成功", "提示", 5);

                if (AutoPrint)
                {
                    Task.Run(new Action(() =>
                    {
                        Thread.Sleep(1000);
                        CRT.MainThread.InvokeIfRequired(() =>
                        {
                            close();
                        });
                    }).WithCurrentThreadContext());
                }
            }
        }

    }
}
