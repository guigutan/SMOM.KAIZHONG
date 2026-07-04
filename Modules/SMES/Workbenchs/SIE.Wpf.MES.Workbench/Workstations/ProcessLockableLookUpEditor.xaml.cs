using SIE.Domain;
using SIE.Tech.Processs;
using SIE.Wpf.MES.WIP;
using SIE.Wpf.Windows;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.MES.Workbench.Workstations
{
    /// <summary>
    /// ProcessLockableLookUpEditor.xaml 的交互逻辑
    /// </summary>
    public partial class ProcessLockableLookUpEditor : UserControl
    {
        public ProcessLockableLookUpEditor()
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
            var workstation = this.DataContext as Workstation;
            if (workstation == null || !workstation.EmployeeId.HasValue)
                this.editor.ItemsSource = new EntityList<Process>();
            else
                this.editor.ItemsSource = RT.Service.Resolve<ProcessController>().GetProcesssByUserId(workstation.EmployeeId.Value, string.Empty, new List<ProcessType>() { ProcessType.Pqc });
        }
    }
}
