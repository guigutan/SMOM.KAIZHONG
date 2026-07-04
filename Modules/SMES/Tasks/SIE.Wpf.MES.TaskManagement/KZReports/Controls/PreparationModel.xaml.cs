using DevExpress.Xpf.CodeView;
using Microsoft.Scripting.Utils;
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
    /// PreparationModel.xaml 的交互逻辑
    /// </summary>
    public partial class PreparationModel : UserControl
    {

        KZTaskReportViewModelBase model;
        KZReportHelper kZReportHelper;
        /// <summary>
        /// 模具列表
        /// </summary>
        protected virtual ObservableCollection<Pda_PreStartupSetupScanEquipAccountInfo> ModelInfoList { get; }
        /// <summary>
        /// 已上资源列表
        /// </summary>
        protected virtual ObservableCollection<Pda_PreStartupSetupScanEquipAccountInfo> ModelInfoRecordList { get; }
        /// <summary>
        /// 
        /// </summary>
        public PreparationModel()
        {
            InitializeComponent();
            this.Loaded += Control_Loaded;
            this.Unloaded -= Control_Loaded;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_model"></param>
        public PreparationModel(KZTaskReportViewModelBase _model)
        {
            InitializeComponent();
            this.model = _model;
            this.DataContext = _model;
            kZReportHelper = model.kZReportHelper;

            ModelInfoList = new ObservableCollection<Pda_PreStartupSetupScanEquipAccountInfo>();
            ModelInfoRecordList = new ObservableCollection<Pda_PreStartupSetupScanEquipAccountInfo>();
            this.dataGrid.ItemsSource = ModelInfoList;
            this.dataGrid2.ItemsSource = ModelInfoRecordList;

            this.Loaded += Control_Loaded;
            this.Unloaded -= Control_Loaded;
        }

        private void Control_Loaded(object sender, RoutedEventArgs e)
        {
            reset();
            if (model.DispatchTask != null)
            {
                var infos = RT.Service.Resolve<DispatchController>().GetPreStartupSetupEquipAccountTaskInfos(model.DispatchTask.No);
                if (infos != null && infos.Count > 0)
                {
                    ModelInfoRecordList.Clear();
                    foreach (var model in infos[0].ScannedModel.OrderByDescending(p => p.ScanTime))
                    {
                        ModelInfoRecordList.Add(model);
                    }
                    //ModelInfoRecordList.AddRange(infos[0].ScannedModel.OrderByDescending(p => p.ScanTime));
                }
                this.tabItemModelDetail.IsSelected = true;
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            close();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (ModelInfoList.Count == 0)
            {
                CRT.MessageService.ShowInstantMessage("没有要提交的数据");
                return;
            }
            if (CRT.MessageService.AskQuestion("确认要提交吗?".L10nFormat(), "确认"))
            {
                var list = ModelInfoList.ToList();
                RT.Service.Resolve<DispatchController>().PreStartupSetupEquipAccountChangedSubmmit(model.DispatchTaskId ?? 0, list);
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
                    var modelInfo = RT.Service.Resolve<DispatchController>().PreStartupSetupScanEquipAccount(barcode, model.DispatchTaskId ?? 0);
                    if (ModelInfoList.Any(p => p.Code == modelInfo.Code && p.Type == modelInfo.Type))
                        throw new ValidationException("[{0}][{1}]已在明细列表,请勿重复扫描".L10nFormat(modelInfo.Type, modelInfo.Code));

                    ModelInfoList.Add(modelInfo);

                    if (!this.tabItemScanDetail.IsSelected)
                        this.tabItemScanDetail.IsSelected = true;

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
            ModelInfoList.Clear();
            this.txtBarcode.Text = "";
            this.txtBarcode.Focus();
            showTip("请扫描模具编码");
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
