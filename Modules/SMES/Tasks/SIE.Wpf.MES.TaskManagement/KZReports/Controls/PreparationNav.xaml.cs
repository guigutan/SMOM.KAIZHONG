using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.MES.TaskManagement.KZReports.Controls
{
    /// <summary>
    /// PreparationNav.xaml 的交互逻辑
    /// </summary>
    public partial class PreparationNav : UserControl
    {

        KZTaskReportViewModelBase model;
        KZReportHelper kZReportHelper;

        /// <summary>
        /// 
        /// </summary>
        public PreparationNav()
        {
            InitializeComponent();
            this.Loaded += PreparationNav_Loaded;
            this.Unloaded -= PreparationNav_Loaded;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_model"></param>
        public PreparationNav(KZTaskReportViewModelBase _model)
        {
            InitializeComponent();
            this.model = _model;
            this.DataContext = _model;
            kZReportHelper = _model.kZReportHelper;
            this.Loaded += PreparationNav_Loaded;
            this.Unloaded -= PreparationNav_Loaded;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreparationNav_Loaded(object sender, RoutedEventArgs e)
        {
            //this.txtUserName.Focusable = true;
            //this.txtUserName.Focus();
        }

        /// <summary>
        /// 工装/检具
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEquip_Click(object sender, RoutedEventArgs e)
        {
            close();
            kZReportHelper.ShowPreparationEquip();
        }

        /// <summary>
        /// 上模
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModel_Click(object sender, RoutedEventArgs e)
        {
            close();
            kZReportHelper.ShowPreparationModel();
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
