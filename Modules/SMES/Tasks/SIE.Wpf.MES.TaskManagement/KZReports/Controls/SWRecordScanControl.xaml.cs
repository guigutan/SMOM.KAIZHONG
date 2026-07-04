using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Dispatchs.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SIE.Wpf.MES.TaskManagement.KZReports.Controls
{
    /// <summary>
    /// SWRecordScanControl.xaml 的交互逻辑
    /// </summary>
    public partial class SWRecordScanControl : UserControl
    {
        KZTaskReportViewModelBase model;

        KZReportHelper kZReportHelper;

        public SWRecordScanControl()
        {
            InitializeComponent();
        }

        public SWRecordScanControl(KZTaskReportViewModelBase _model)
        {
            InitializeComponent();
            this.model = _model;
            kZReportHelper = model.kZReportHelper;
            this.templateBorder.DataContext = this.model;
            this.Loaded += SWRecordScanControl_Loaded;
            this.Unloaded -= SWRecordScanControl_Loaded;
        }

        private void SWRecordScanControl_Loaded(object sender, RoutedEventArgs e)
        {
            reset();
        }

        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="msg"></param>
        void showTip(string msg)
        {
            this.txtTip.Foreground = new SolidColorBrush(Colors.Green);
            this.txtTip.Text = msg;
        }

        private void btnActualQty_Click(object sender, RoutedEventArgs e)
        {
            if (this.model.Sn.IsNullOrEmpty())
            {
                CRT.MessageService.ShowError("请先扫描标签".L10N());
                return;
            }
            kZReportHelper.ShowCalculatorEditor(KZTaskReportViewModel.ActualQtyProperty);
        }

        /// <summary>
        /// 显示错误
        /// </summary>
        /// <param name="msg"></param>
        void showError(string msg)
        {
            this.txtTip.Foreground = new SolidColorBrush(Colors.Red);
            this.txtTip.Text = msg;
        }

        /// <summary>
        /// 重置
        /// </summary>
        void reset()
        {
            this.model.Sn = string.Empty;
            this.model.ItemLabelId = 0;
            this.model.FeedingRecordId = 0;
            this.model.Lot = string.Empty;
            this.model.ItemCode = string.Empty;
            this.model.ItemName = string.Empty;
            this.model.ItemUnit = string.Empty;
            this.model.ItemLabelState = string.Empty;
            this.model.FeedingQty = 0;
            this.model.BlankingQty = 0;
            this.model.RemainingQty = 0;
            this.model.ActualQty = 0;

            this.txtBarcode.Text = "";
            this.txtBarcode.Focus();
            showTip("请扫描标签");
            kZReportHelper.SwitchToEnglishMode();
        }

        /// <summary>
        /// 重新开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReStart_Click(object sender, RoutedEventArgs e)
        {
            reset();
        }

        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            close();
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.model.Sn.IsNullOrEmpty())
                {
                    CRT.MessageService.ShowError("请先扫描标签".L10N());
                    return;
                }

                PdaSWRecordScanInfo info = new PdaSWRecordScanInfo();
                info.Sn = this.model.Sn;
                info.ItemLabelId = this.model.ItemLabelId;
                info.FeedingRecordId = this.model.FeedingRecordId;
                info.Lot = this.model.Lot;
                info.ItemCode = this.model.ItemCode;
                info.ItemName = this.model.ItemName;
                info.ItemUnit = this.model.ItemUnit;
                info.ItemLabelState = this.model.ItemLabelState;
                info.FeedingQty = this.model.FeedingQty;
                info.BlankingQty = this.model.BlankingQty;
                info.RemainingQty = this.model.RemainingQty;
                info.ActualQty = this.model.ActualQty;
                RT.Service.Resolve<DispatchController>().SubmitPdaSWRecordScanInfo(new List<PdaSWRecordScanInfo>() { info });
                CRT.MessageService.ShowMessage("提交成功");
                showTip("提交成功");
                reset();
                //this.close();
            }
            catch (Exception ex)
            {
                showError(ex.GetBaseException().Message);
            }
            finally
            {
                this.txtBarcode.Text = "";
                this.txtBarcode.Focus();
            }
        }

        // 处理输入框的按键事件
        private void txtBarcode_KeyUp(object sender, KeyEventArgs e)
        {
            // 判断是否按下了回车键
            if (e.Key == Key.Enter)
            {
                string barcode = this.txtBarcode.Text;
                try
                {
                    var info = RT.Service.Resolve<DispatchController>().GetPdaSWRecordScanInfo(barcode);
                    this.model.Sn = info.Sn;
                    this.model.ItemLabelId = info.ItemLabelId;
                    this.model.FeedingRecordId = info.FeedingRecordId;
                    this.model.Lot = info.Lot;
                    this.model.ItemCode = info.ItemCode;
                    this.model.ItemName = info.ItemName;
                    this.model.ItemUnit = info.ItemUnit;
                    this.model.ItemLabelState = info.ItemLabelState;
                    this.model.FeedingQty = info.FeedingQty;
                    this.model.BlankingQty = info.BlankingQty;
                    this.model.RemainingQty = info.RemainingQty;
                    this.model.ActualQty = info.ActualQty ?? 0;

                    showTip("[{0}]扫描成功".L10nFormat(barcode));
                }
                catch (Exception ex)
                {
                    showError(ex.GetBaseException().Message);
                }
                finally
                {
                    this.txtBarcode.Text = "";
                    this.txtBarcode.Focus();
                }
            }
        }


        void close()
        {
            var parent = Window.GetWindow(this);
            if (parent != null && parent is Window)
            {
                (parent as Window)?.Close();
            }
        }
    }
}
