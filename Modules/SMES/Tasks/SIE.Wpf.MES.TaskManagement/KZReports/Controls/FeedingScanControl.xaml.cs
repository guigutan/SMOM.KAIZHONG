using DevExpress.Xpf.LayoutControl;
using DocumentFormat.OpenXml.Wordprocessing;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.FeedingIntervals;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Dispatchs.Datas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SIE.Wpf.MES.TaskManagement.KZReports.Controls
{
    /// <summary>
    /// FeedingScanControl.xaml 的交互逻辑
    /// </summary>
    public partial class FeedingScanControl : UserControl
    {
        KZTaskReportViewModelBase model;
        KZReportHelper kZReportHelper;
        //FeedingTaskListControl feedingTaskListControl;
        public double TaskId;
        /// <summary>
        /// 标签列表
        /// </summary>
        protected virtual ObservableCollection<CsFeedingScanInfos> ScanInfos { get; }
        protected virtual ObservableCollection<CsFeedingItemInfos> ItemInfos { get; }


        public FeedingScanControl()
        {
            InitializeComponent();
        }

        void close()
        {
            var parent = Window.GetWindow(this);
            if (parent != null && parent is Window)
            {
                (parent as Window)?.Close();
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
                    if (ScanInfos.Any(p => p.Sn == barcode))
                        throw new ValidationException("[{0}]已在明细列表,请勿重复扫描".L10nFormat(barcode));

                    var infos = RT.Service.Resolve<DispatchController>().AssemblyGetItemLabels(barcode, TaskId);
                    var ScanInfo = ScanInfos.Where(p => infos.Any(a => a.ItemLabelId == p.ItemLabelId)).FirstOrDefault();
                    if (ScanInfo!= null)
                    {
                        throw new ValidationException("[{0}]已在明细列表,请勿重复扫描".L10nFormat(ScanInfo.Sn));
                    }

                    foreach (var info in infos)
                    {
                        ScanInfos.Add(new CsFeedingScanInfos()
                        {
                            Sn = info.ItemLabel,
                            Batch = info.BatchNo,
                            ItemCode = info.ItemCode,
                            ItemName = info.ItemName,
                            FeedingQty = info.AssemblyQty,
                            FeedingTime=System.DateTime.Now,
                            ItemLabelId = info.ItemLabelId,
                            Unit = info.Unit,
                            ItemLabelState = info.ItemLabelState
                        });
                    }
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


        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="msg"></param>
        void showTip(string msg)
        {
            this.txtTip.Foreground = new SolidColorBrush(Colors.Green);
            this.txtTip.Text = msg;
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
        /// 提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                var msg = RT.Service.Resolve<DispatchController>().AssemblyValidItemRemainingQty(TaskId, ItemInfos.Select(p => p.ProcessBomId).Distinct().ToList());
                if (msg != null && msg != "")
                {
                    if (!CRT.MessageService.AskQuestion(msg))
                    {
                        //RT.Service.Resolve<DispatchController>().AssemblyValidRemainingQtySubmmit(TaskId);
                        reset();
                        this.close();
                        return;
                    }
                }

                if (ScanInfos == null || ScanInfos.Count < 1)
                    throw new ValidationException("未扫描标签，无法提交!".L10N());
                List<Pda_AssemblyGetItemLabelInfo> infos = new List<Pda_AssemblyGetItemLabelInfo>();
                foreach (var ScanInfo in ScanInfos)
                {
                    Pda_AssemblyGetItemLabelInfo info = new Pda_AssemblyGetItemLabelInfo();
                    info.ItemLabelId = ScanInfo.ItemLabelId;
                    info.ItemLabel = ScanInfo.Sn;
                    info.BatchNo = ScanInfo.Batch;
                    info.ItemCode = ScanInfo.ItemCode;
                    info.ItemName = ScanInfo.ItemName;
                    info.AssemblyQty = ScanInfo.FeedingQty;
                    info.Unit = ScanInfo.Unit;
                    info.ItemLabelState = ScanInfo.ItemLabelState;
                    infos.Add(info);
                }
                //上料
                RT.Service.Resolve<DispatchController>().AssemblySubmit(TaskId, infos);
                CRT.MessageService.ShowMessage("提交成功");
                showTip("提交成功");
                reset();
                GetProcessBomInfos();//刷新bom物料数据
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
        /// 重置
        /// </summary>
        void reset()
        {
            ScanInfos?.Clear();
            this.txtBarcode.Text = "";
            this.txtBarcode.Focus();
            showTip("请扫描标签");
            kZReportHelper.SwitchToEnglishMode();
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

        public FeedingScanControl(KZTaskReportViewModelBase _model, double taskId)
        {
            InitializeComponent();
            this.model = _model;
            this.DataContext = _model;
            kZReportHelper = model.kZReportHelper;
            TaskId = taskId;
            ScanInfos = new ObservableCollection<CsFeedingScanInfos>();
            ItemInfos = new ObservableCollection<CsFeedingItemInfos>();

            this.Loaded += FeedingScanControl_Loaded;
            this.Unloaded -= FeedingScanControl_Loaded;
        }

        private void FeedingScanControl_Loaded(object sender, RoutedEventArgs e)
        {
            reset();
            try
            {
                GetProcessBomInfos();

                this.dataGrid.ItemsSource = ItemInfos;

                //扫描信息指定数据源
                this.dataGrid1.ItemsSource = ScanInfos;
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

        void GetProcessBomInfos()
        {
            ItemInfos.Clear();
            //加载物料信息
            var infos = RT.Service.Resolve<DispatchController>().AssemblyGetProcessBomInfos(TaskId);

            foreach (var info in infos)
            {
                CsFeedingItemInfos itemInfos = new CsFeedingItemInfos();
                itemInfos.ProcessBomId = info.ProcessBomId;
                itemInfos.ItemCode = info.ProductCode;
                itemInfos.ItemName = info.ProductName;
                itemInfos.RequireQty = info.Qty;
                itemInfos.RemainQty = info.RemainingQty;
                ItemInfos.Add(itemInfos);
            }
        }
    }
}
