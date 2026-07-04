using SIE.Domain.Validation;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Dispatchs.Datas;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SIE.Wpf.MES.TaskManagement.KZReports.Controls
{
    /// <summary>
    /// PreparationEquip.xaml 的交互逻辑
    /// </summary>
    public partial class PreparationEquip : UserControl
    {

        KZTaskReportViewModelBase model;
        KZReportHelper kZReportHelper;
        /// <summary>
        /// 工装/检具列表
        /// </summary>
        protected virtual ObservableCollection<Pda_PreStartupSetupToolInfo> ToolInfoList { get; }

        /// <summary>
        /// 
        /// </summary>
        public PreparationEquip()
        {
            InitializeComponent();
            this.Loaded += Control_Loaded;
            this.Unloaded -= Control_Loaded;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_model"></param>
        public PreparationEquip(KZTaskReportViewModelBase _model)
        {
            InitializeComponent();
            this.model = _model;
            this.DataContext = _model;
            kZReportHelper = model.kZReportHelper;

            ToolInfoList = new ObservableCollection<Pda_PreStartupSetupToolInfo>();
            this.dataGrid.ItemsSource = ToolInfoList;

            this.Loaded += Control_Loaded;
            this.Unloaded -= Control_Loaded;
        }

        private void Control_Loaded(object sender, RoutedEventArgs e)
        {
            reset();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            close();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (ToolInfoList.Count == 0)
            {
                CRT.MessageService.ShowInstantMessage("没有要提交的数据");
                return;
            }
            if (CRT.MessageService.AskQuestion("确认要提交吗?".L10nFormat(), "确认"))
            {
                var list = ToolInfoList.ToList();
                RT.Service.Resolve<DispatchController>().PreStartupSetupToolSubmmit(model.DispatchTaskId ?? 0, list);
                CRT.MessageService.ShowMessage("提交成功".L10N(), "提示".L10N());
                System.Threading.Thread.Sleep(1000);
                close();
            }

            this.txtBarcode.Text = "";
            this.txtBarcode.Focus();

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            reset();
        }

        private void txtBarcode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    showTip("");
                    string barcode = this.txtBarcode.Text;
                    var toolInfo = RT.Service.Resolve<DispatchController>().GetPreStartupSetupToolInfos(barcode, model.DispatchTaskId ?? 0);
                    if (ToolInfoList.Any(p => p.Code == toolInfo.Code && p.Type == toolInfo.Type))
                        throw new ValidationException("[{0}][{1}]已在明细列表,请勿重复扫描".L10nFormat(toolInfo.Type, toolInfo.Code));
                    ToolInfoList.Add(toolInfo);

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
        /// 重置
        /// </summary>
        void reset()
        {
            ToolInfoList.Clear();
            this.txtBarcode.Text = "";
            this.txtBarcode.Focus();
            showTip("请扫描工装/检具编码");
            kZReportHelper.SwitchToEnglishMode();
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
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
