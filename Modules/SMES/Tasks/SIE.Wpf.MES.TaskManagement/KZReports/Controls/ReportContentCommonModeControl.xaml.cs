using DevExpress.Xpf.Grid;
using DocumentFormat.OpenXml.EMMA;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Reports.Datas;
using SIE.Wpf.Common.Editors;
using Stimulsoft.Data.Expressions.Antlr.Runtime.Misc;
using Svg;
using System;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SIE.Wpf.MES.TaskManagement.KZReports.Controls
{
    /// <summary>
    /// ReportContentControl.xaml 的交互逻辑
    /// </summary>
    public partial class ReportContentCommonModeControl : UserControl
    {

        KZTaskReportViewModelBase model;
        KZReportHelper kZReportHelper;
        bool isManual;

        /// <summary>
        /// 
        /// </summary>
        public ReportContentCommonModeControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_model"></param>
        /// <param name="_isManual"></param>
        public ReportContentCommonModeControl(KZTaskReportViewModelBase _model, bool _isManual = false)
        {
            InitializeComponent();
            this.model = _model;
            this.DataContext = _model;
            kZReportHelper = model.kZReportHelper;
            isManual = _isManual;
            this.Loaded += ReportContentControl_Loaded;
            this.Unloaded -= ReportContentControl_Loaded;

            cbbPrinter.DropDownOpened -= CbbPrinter_DropDownOpened;
            cbbPrinter.DropDownOpened += CbbPrinter_DropDownOpened;
        }

        private void CbbPrinter_DropDownOpened(object sender, EventArgs e)
        {
            BindPrinterDatas();
        }

        private void ReportContentControl_Loaded(object sender, RoutedEventArgs e)
        {
            var contentWidth = borderMain.ActualWidth;
            listQueue.MaxHeight = 390;
            // 加载打印机列表            
            BindPrinterDatas();

            var rowHeight = 40;
            var screenWidth = (int)SystemParameters.PrimaryScreenWidth;
            var screenHeight = (int)SystemParameters.PrimaryScreenHeight;
            if (screenWidth <= 1024 || screenHeight <= 800)
            {
                rowHeight = 30;

                listQueue.MaxHeight = 210;
            }
            if (model.IotMode == IotMode.Normal || isManual)
            {
                //row3.Height = new GridLength(rowHeight);
                //row3.MinHeight = rowHeight;
                ////row5.Height = new GridLength(rowHeight);
                ////row5.MinHeight = rowHeight;
                //row4.Height = new GridLength(0);
                //row4.MinHeight = 0;
            }
            else
            {
                //row3.Height = new GridLength(0);
                //row3.MinHeight = 0;
                ////row5.Height = new GridLength(0);
                ////row5.MinHeight = 0;
                //row4.Height = new GridLength(rowHeight);
                //row4.MinHeight = rowHeight;
            }

        }

        /// <summary>
        /// 绑定打印机列表数据
        /// </summary>
        void BindPrinterDatas()
        {
            try
            {
                var printer = model.Printer;
                //var selectValue = cbx.SelectedValue;
                cbbPrinter.Items.Clear();
                foreach (string installedPrinter in PrinterSettings.InstalledPrinters)
                {
                    cbbPrinter.Items.Add(installedPrinter);
                }
                //cbx.SelectedValue = selectValue;
                model.Printer = printer;
            }
            catch (PlatformNotSupportedException)
            {
                Console.WriteLine("此平台不支持访问已安装的打印机。");
            }
        }


        private void btnSuspectQty_Click(object sender, RoutedEventArgs e)
        {
            showSuspectQtyInput(sender);

        }

        private void btnOkQty_Click(object sender, RoutedEventArgs e)
        {
            showGoodQtyInput(sender);
        }

        /// <summary>
        /// 编辑良品数 
        /// </summary>
        /// <param name="sender"></param>
        void showGoodQtyInput(object sender)
        {

        }

        /// <summary>
        /// 编辑可疑品数
        /// </summary>
        /// <param name="sender"></param>
        void showSuspectQtyInput(object sender)
        {
            if (model is KZTaskReportCommonModeViewModel modelCommonMode)
            {
                Task.Run(() =>
                {
                    CRT.MainThread.InvokeIfRequired(() =>
                    {
                        var btn = sender as Button;
                        var id = btn.Tag.ToString();

                        var queue = this.listQueue.Items.Cast<DispatchTaskQueue>().FirstOrDefault(p => p.Id.ToString() == id);
                        decimal value = queue.IotSuspectQty;

                        kZReportHelper.ShowCalculatorEditor((value) =>
                        {
                            modelCommonMode.SetIotSuspectQty(queue.Id, value);
                            queue.IotSuspectQty = value;
                            return value;
                        }, value);
                    });
                });

            }
        }


        private void listQueue_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var data = (sender as ListBox).SelectedItem;
            if (data == null) return;
            model.DispatchTask = model.LoadTask((data as DispatchTaskQueue).DispatchTaskId);
        }
    }
}
