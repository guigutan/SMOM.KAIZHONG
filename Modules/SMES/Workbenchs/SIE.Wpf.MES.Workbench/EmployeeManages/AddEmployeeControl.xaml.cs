using SIE.Domain;
using SIE.Resources.Employees;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SIE.Wpf.MES.Workbench.EmployeeManages
{
    /// <summary>
    /// AddEmployeeControl.xaml 的交互逻辑
    /// </summary>
    public partial class AddEmployeeControl : UserControl
    {
        ////EntityList<Employee> _selectedEmployeeList;

        /// <summary>
        /// 
        /// </summary>
        public EntityList<Employee> SelectedEmployeeList { get; set; } = new EntityList<Employee>();
       readonly EntityList<Employee> EmployeeList = new EntityList<Employee>();
        double[] Filter;
        ListLogicalView _employeeView;
        ListLogicalView _selectedView;

        /// <summary>
        /// 构造函数
        /// </summary>
        public AddEmployeeControl(double[] filter)
        {
            InitializeComponent();
            Filter = filter;
            CreateContorl();
            SeachEmployee();
        }

        private void CreateContorl()
        {
            _employeeView = AutoUI.ViewFactory.CreateListView(typeof(Employee), EmployeeExtViewConfig.OnLoanView);
            _employeeView.Data = EmployeeList;
            ctlEmp.Content = _employeeView.Control;
            _selectedView = AutoUI.ViewFactory.CreateListView(typeof(Employee), EmployeeExtViewConfig.OnLoanView);
            _selectedView.Data = SelectedEmployeeList;
            ctlSelectedEmp.Content = _selectedView.Control;
        }

        private void ButtonEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SeachEmployee();
        }

        private void SearchEdit_ButtonClick(object sender, RoutedEventArgs e)
        {
            SeachEmployee();
        }

        private void SeachEmployee()
        {
            EmployeeList.Clear();
            SelectedEmployeeList.Clear();
            var result = RT.Service.Resolve<EmployeeController>().GetEmployees(searchEdit.Text, Filter);
            if (result.Count > 0)
                EmployeeList.AddRange(result);
        }

        private void Cancel_Select(object sender, RoutedEventArgs e)
        {
            var employees = _selectedView.SelectedEntities.OfType<Employee>();
            if (employees.Any())
            {
                employees.ForEach(emp =>
                {
                    var item = SelectedEmployeeList.FirstOrDefault(p => p.Id == emp.Id);
                    if (item != null)
                    {
                        SelectedEmployeeList.Remove(item);
                    }
                    EmployeeList.Add(emp);
                });
            }
        }

        private void Add_Select(object sender, RoutedEventArgs e)
        {
            var employees = _employeeView.SelectedEntities.OfType<Employee>();
            if (employees.Any())
            {
                employees.ForEach(emp =>
                {
                    var item = EmployeeList.FirstOrDefault(p => p.Id == emp.Id);
                    if (item != null)
                    {
                        EmployeeList.Remove(item);
                    }
                    SelectedEmployeeList.Add(emp);
                });
            }
        }
    }
}