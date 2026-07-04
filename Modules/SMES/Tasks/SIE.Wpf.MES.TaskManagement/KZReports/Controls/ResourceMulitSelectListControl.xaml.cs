using SIE.Threading;
using SIE.View;
using System;
using System.Collections.Generic;
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
    /// ResourceListControl.xaml 的交互逻辑
    /// </summary>
    public partial class ResourceMulitSelectListControl : UserControl
    {
        KZTaskReportViewModelBase model;
        KZReportHelper kZReportHelper;

        public double ResouceId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ResourceMulitSelectListControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_model"></param>
        public ResourceMulitSelectListControl(KZTaskReportViewModelBase _model)
        {
            InitializeComponent();
            this.model = _model;
            this.DataContext = _model;
            this.Loaded += ResourceListControl_Loaded;
            this.Unloaded -= ResourceListControl_Loaded;
        }

        private void ResourceListControl_Loaded(object sender, RoutedEventArgs e)
        {
            loadData();
            this.scanTxt.Focus();
        }

        void loadData()
        {
            var loadAll = cbShowAll.IsChecked == true;
            var text = scanTxt.Text;
            model.ResourceList.Clear();
            Task.Run(new Action(() =>
            {
                model.LoadResourceList(loadAll, text);
            }).WithCurrentThreadContext());
        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as System.Windows.Controls.Button;
            var tagString = btn.Tag.ToString();
            if (tagString.IsNullOrEmpty())
            {
                return;
            }
            ResouceId = double.Parse(tagString);
            if (model is KZTaskReportMultiStationViewModel vm)
            {
                if (vm.ReportViewModelList.Any(p => p.ResourceId == ResouceId))
                {
                    CRT.MessageService.ShowWarningFormatted("当前工位已在生产工位");
                    return;
                }
            }
            close();
        }

        void close()
        {
            var parent = Window.GetWindow(this);
            if (parent != null && parent is Window)
            {
                (parent as Window)?.Close();
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            loadData();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            loadData();
        }

        private void scanTxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;
            loadData();
        }
    }
}
