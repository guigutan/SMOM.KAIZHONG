using SIE.Resources.Employees;
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
    /// ReportMainControl.xaml 的交互逻辑
    /// </summary>
    public partial class UserLoginControl : UserControl
    {

        KZTaskReportViewModelBase model;
        /// <summary>
        /// 员工
        /// </summary>
        public Employee Employee;
        /// <summary>
        /// 
        /// </summary>
        public UserLoginControl()
        {
            InitializeComponent();
            this.Loaded += UserLoginControl_Loaded;
            this.Unloaded -= UserLoginControl_Loaded;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_model"></param>
        public UserLoginControl(KZTaskReportViewModelBase _model)
        {
            InitializeComponent();
            this.model = _model;
            this.DataContext = _model;
            this.Loaded += UserLoginControl_Loaded;
            this.Unloaded -= UserLoginControl_Loaded;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserLoginControl_Loaded(object sender, RoutedEventArgs e)
        {
            //this.txtUserName.Focusable = true;
            this.txtUserName.Focus();
        }

        /// <summary>
        /// 员工上号
        /// </summary>
        /// <returns></returns>
        private void EmpLogin(string code)
        {
            Employee = null;
            if (string.IsNullOrEmpty(code))
            {
                //MessageBox.Show("请输入工号".L10N());
                return;
            }
            try
            {
                //校验员工号是否存在
                Employee = RT.Service.Resolve<EmployeeController>().GetEmployeeByCode(code);
                if (Employee == null)
                {
                    MessageBox.Show( "输入的工号不存在，请检查".L10N());
                    this.btnConfirmOk.Focus();
                    this.txtUserName.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
                        
            var parent = Window.GetWindow(this);
            if (parent != null && parent is Window)
            {
                (parent as Window)?.Close();
            }
        }

        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private void btnConfirmOk_Click(object sender, RoutedEventArgs e)
        {
            EmpLogin(this.txtUserName.Text);
        }

        private void txtUserName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                EmpLogin(this.txtUserName.Text);
        }

        private void txtUserName_GotFocus(object sender, RoutedEventArgs e)
        {
            txtUserName.Text = "";
        }

        private void txtUserName_LostFocus(object sender, RoutedEventArgs e)
        {
            //txtUserName.Text = "请输入员工号";
        }
    }
}
