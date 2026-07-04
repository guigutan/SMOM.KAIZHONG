using DevExpress.Xpf.Grid;
using SIE.Wpf.Common.Editors;
using System;
using System.Drawing.Printing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SIE.Wpf.MES.TaskManagement.KZReports.Controls
{
    /// <summary>
    /// ReportContentControl.xaml 的交互逻辑
    /// </summary>
    public partial class ReportContentControl : UserControl
    {

        KZTaskReportViewModelBase model;
        KZReportHelper kZReportHelper;
        bool isManual;

        /// <summary>
        /// 
        /// </summary>
        public ReportContentControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_model"></param>
        /// <param name="_isManual"></param>
        public ReportContentControl(KZTaskReportViewModelBase _model, bool _isManual = false)
        {
            InitializeComponent();
            this.model = _model;
            this.DataContext = _model;
            kZReportHelper = model.kZReportHelper;
            isManual = _isManual;
            if (_isManual)
            {
                lbOkQty.Visibility = Visibility.Collapsed;
                txtOkQty.Visibility = Visibility.Visible;
                btnOkQty.Visibility = Visibility.Visible;
            }
            else
            {
                btnOkQty.Visibility = Visibility.Collapsed;
                txtOkQty.Visibility = Visibility.Collapsed;
                lbOkQty.Visibility = Visibility.Visible;
            }
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
            // 加载打印机列表            
            BindPrinterDatas();

            var rowHeight = 40;
            var screenWidth = (int)SystemParameters.PrimaryScreenWidth;
            var screenHeight = (int)SystemParameters.PrimaryScreenHeight;
            if (screenWidth <= 1024 || screenHeight <= 800)
            {
                rowHeight = 30;
                this.grid1.RowDefinitions.ForEach(row => { row.MinHeight = rowHeight; });
                this.grid2.RowDefinitions.ForEach(row => { row.MinHeight = rowHeight; });
                this.grid3.RowDefinitions.ForEach(row => { row.MinHeight = rowHeight; });
            }
            if (model.IotMode != IotMode.Extrusion || isManual)
            {
                row3.Height = new GridLength(rowHeight);
                row3.MinHeight = rowHeight;
                row4.Height = new GridLength(0);
                row4.MinHeight = 0;
            }
            if (model.IotMode == IotMode.Extrusion && !isManual)
            {
                row3.Height = new GridLength(0);
                row3.MinHeight = 0;
                row4.Height = new GridLength(rowHeight);
                row4.MinHeight = rowHeight;
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

        private void btnResource_Click(object sender, RoutedEventArgs e)
        {
            kZReportHelper.ShowSelectResourceList();
        }

        private void btnSuspectQty_Click(object sender, RoutedEventArgs e)
        {
            kZReportHelper.ShowCalculatorEditor(KZTaskReportViewModel.SuspectQtyProperty);

        }

        private void btnOkQty_Click(object sender, RoutedEventArgs e)
        {
            kZReportHelper.ShowCalculatorEditor(KZTaskReportViewModel.OkQtyProperty);
        }

        private void txtSuspectQty_TouchUp(object sender, TouchEventArgs e)
        {
            kZReportHelper.ShowCalculatorEditor(KZTaskReportViewModel.SuspectQtyProperty);
        }

        private void txtOkQty_TouchUp(object sender, TouchEventArgs e)
        {
            kZReportHelper.ShowCalculatorEditor(KZTaskReportViewModel.OkQtyProperty);
        }

        private void txtOkQty_MouseUp(object sender, MouseButtonEventArgs e)
        {
            kZReportHelper.ShowCalculatorEditor(KZTaskReportViewModel.OkQtyProperty);
        }

        private void txtSuspectQty_MouseUp(object sender, MouseButtonEventArgs e)
        {
            kZReportHelper.ShowCalculatorEditor(KZTaskReportViewModel.SuspectQtyProperty);
        }
    }
}
