using SIE.Domain;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using SIE.Wpf.MES.WIP;
using SIE.Wpf.Windows;
using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.MES.Workbench.Workstations
{
    /// <summary>
    /// StationLockableLookUpEditor.xaml 的交互逻辑
    /// </summary>
    public partial class StationLockableLookUpEditor : UserControl
    {
        public StationLockableLookUpEditor()
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
            if (workstation == null || !workstation.ResourceId.HasValue || !workstation.ProcessId.HasValue)
                this.editor.ItemsSource = new EntityList<Station>();
            else
                this.editor.ItemsSource = RT.Service.Resolve<ProcessController>().GetStationsByResourceId(workstation.ResourceId.Value, workstation.ProcessId.Value, null);
        }
    }
}
