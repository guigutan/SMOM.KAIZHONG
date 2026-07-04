using DevExpress.Xpf.LayoutControl;
using DevExpress.XtraRichEdit.Import;
using SIE.Core.ApiModels;
using SIE.Domain.Validation;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Dispatchs.Datas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SIE.Wpf.MES.TaskManagement.KZReports.Controls
{
    /// <summary>
    /// FeedingAreaScanControl.xaml 的交互逻辑
    /// </summary>
    public partial class FeedingAreaScanControl : UserControl
    {
        /// <summary>
        /// 操作类型(0:扫描供料区编码,1:扫描批次标签)
        /// </summary>
        public int OperateType;

        /// <summary>
        /// 供料区信息
        /// </summary>
        public virtual BaseDataInfo baseDataInfo { get; set; }

        /// <summary>
        /// 扫描的标签信息
        /// </summary>
        public virtual ObservableCollection<Pda_AssemblyGetItemLabelInfo> itemLabelInfos { get; }

        /// <summary>
        /// 已扫描标签
        /// </summary>
        public List<string> Labels;

        public FeedingAreaScanControl()
        {
            InitializeComponent();
            //初始要扫描供料区编码
            OperateType = 0;
            itemLabelInfos = new ObservableCollection<Pda_AssemblyGetItemLabelInfo>();
            baseDataInfo = new BaseDataInfo();

            this.Loaded += Control_Loaded;
            this.Unloaded -= Control_Loaded;

        }

        private void Control_Loaded(object sender, RoutedEventArgs e)
        {
            reset();
            try
            {
                this.dataGrid.ItemsSource = itemLabelInfos;
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
                    if (OperateType == 0)
                    {
                        baseDataInfo = RT.Service.Resolve<DispatchController>().GetFeedingAreaInfo(barcode);
                        this.AreaCode.Content = barcode;
                        itemLabelInfos?.Clear();
                        this.txtBarcode.Text = "";
                        this.txtBarcode.Focus();
                        showTip("请扫描物料标签");
                        //改为扫描标签
                        OperateType = 1;
                    }
                    else
                    {
                        var result = RT.Service.Resolve<DispatchController>().GetFeedingAreaItemLabelInfos(baseDataInfo.Code, barcode);

                        if (itemLabelInfos.Any(a => result.Any(p => p.ItemLabelId == a.ItemLabelId)))
                        {
                            throw new ValidationException("已存在相同扫描信息,请勿重复扫描".L10N());
                        }
                        foreach (var r in result)
                        {
                            itemLabelInfos.Add(r);
                        }
                        showTip("请扫描物料标签");
                    }
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
        /// 提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RT.Service.Resolve<DispatchController>().SubmitFeedingAreaItemLabelInfo(baseDataInfo.Code, itemLabelInfos.ToList());
                reset();
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
            this.AreaCode.Content = string.Empty;
            itemLabelInfos.Clear();
            OperateType = 0;
            this.txtBarcode.Text = "";
            this.txtBarcode.Focus();
            showTip("请扫描供料区编码");
            //kZReportHelper.SwitchToEnglishMode();
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
        /// 显示错误
        /// </summary>
        /// <param name="msg"></param>
        void showError(string msg)
        {
            this.txtTip.Foreground = new SolidColorBrush(Colors.Red);
            this.txtTip.Text = msg;
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
