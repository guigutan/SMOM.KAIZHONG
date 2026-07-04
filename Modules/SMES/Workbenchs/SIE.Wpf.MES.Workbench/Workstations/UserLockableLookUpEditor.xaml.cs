using SIE.Resources.Employees;
using SIE.Wpf.Windows;
using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.MES.Workbench.Workstations
{
    /// <summary>
    /// UserLockableLookUpEditor.xaml 的交互逻辑
    /// </summary>
    public partial class UserLockableLookUpEditor : UserControl
    {
        public UserLockableLookUpEditor()
        {
            InitializeComponent();
        }

        private void Lock_Click(object sender, RoutedEventArgs e)
        {
            if (btnLock.IsChecked == true)
            {
                btnLock.Content = IconManager.GetPackIcon("Lock", 16, 16);
                editor.IsEnabled = false;
            }
            else
            {
                btnLock.Content = IconManager.GetPackIcon("Unlock", 16, 16);
                editor.IsEnabled = true;
            }
        }

        private void PopupOpening(object sender, DevExpress.Xpf.Editors.OpenPopupEventArgs e)
        {
            LoadLoginUserList();
        }

        void LoadLoginUserList()
        {
            this.editor.ItemsSource = RT.Service.Resolve<EmployeeController>().GetLinkedEmployees(string.Empty, string.Empty, null);
        }
    }
}
