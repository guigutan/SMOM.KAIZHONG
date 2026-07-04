using DevExpress.Xpf.Editors;
using SIE.Domain;
using SIE.MES.Workbench.ProductingChecks;
using SIE.MES.Workbench.StationChecks;
using SIE.Resources.Employees;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace SIE.Wpf.MES.Workbench.ProductingChecks
{
    /// <summary>
    /// ChangeOnDutyControl.xaml 的交互逻辑
    /// </summary>
    public partial class ChangeOnDutyControl : UserControl
    {
        ObservableCollection<GroupEmployee> GroupEmployeeList;
        ProductingCheck _check;
        ProductingCheckControl _control;
        public ChangeOnDutyControl(ProductingCheckControl control, ProductingCheck check)
        {
            InitializeComponent();
            this.DataContext = check.ActualOnDuty;
            _control = control;
            _check = check;
            GroupEmployeeList = new ObservableCollection<GroupEmployee>();
            ctlEmployee.ItemsSource = GroupEmployeeList;
            InitEmployee(check);
        }

        private void InitEmployee(ProductingCheck check)
        {
            var controller = RT.Service.Resolve<EmployeeController>();
            var groupEmployees = controller.GetEmployeeByWorkGroupId(check.GroupId);
            //var groupOnLoans = controller.GetGroupOnLoans(check.GroupId, null).Select(p => p.Employee);
            groupEmployees.ForEach(e =>
            {
                GroupEmployeeList.Add(new GroupEmployee() { Id = e.Id, Employee = e, Display = "{0}  {1}".FormatArgs(e.Code, e.Name), StationId = check.StationId });
            });
            //groupOnLoans.ForEach(e =>
            //{
            //    GroupEmployeeList.Add(new GroupEmployee() { Id = e.Id, Employee = e, Display = "{0}  {1}".FormatArgs(e.Code, e.Name), StationId = check.StationId });
            //});
        }

        private void CtlEmployee_SelectedIndexChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            var cbx = sender as ComboBoxEdit;
            var employee = cbx.EditValue as Employee;
            var date = RF.Find<Employee>().GetDbTime();
            var onduty = new StationOnDuty() { OnDutyDate = date.Date, StationId = _check.StationId };
            if (_check.OnDuty == null)
            {
                onduty.OnDutyId = onduty.ActualOnDutyId = employee.Id;
            }
            else
            {
                if (_check.ActualOnDuty.Id == employee.Id)  //实际出勤人不变
                    return;
                onduty.OnDutyId = _check.OnDuty.Id;
                onduty.ActualOnDutyId = employee.Id;
            }
            RT.Service.Resolve<StationCheckController>().SaveStationOnDuty(onduty);
            _control.RefeshCheck();
        }

        private void BtnOnDuty_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _control.CloseOnDutyWindow();
        }
    }

    public class GroupEmployee
    {
        public double Id { get; set; }
        public string Display { get; set; }
        public Employee Employee { get; set; }

        public double StationId { get; set; }
    }
}
