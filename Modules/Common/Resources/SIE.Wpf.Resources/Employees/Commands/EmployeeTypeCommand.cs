using SIE.Domain;
using SIE.Resources.Employees;
using SIE.Wpf.Command;
using System;
using System.Linq;

namespace SIE.Wpf.Resources.Employees.Commands
{
    /// <summary>
    /// 设为组长
    /// </summary>
    [Command(Label = "设为组长")]
    public class ChargehandCommand : ListViewCommand
    {
        /// <summary>
        /// 是否可执行命令
        /// </summary>
        /// <param name="view">ListLogicalView</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.SelectedEntities.Any() && view.SelectedEntities.OfType<Employee>().All(p => p.EmployeeType != EmployeeType.Chargehand);
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="view">ListLogicalView</param>
        public override void Execute(ListLogicalView view)
        {
            try
            { 
                var selected = view.SelectedEntities.OfType<Employee>();
                foreach (Employee employee in selected)
                {
                    employee.EmployeeType = EmployeeType.Chargehand;
                    RF.Save(employee);
                }  
            }
            catch (Exception exc)
            {
                exc.Alert();
            }
        }
    }

    /// <summary>
    /// 设为班长
    /// </summary>
    [Command(Label = "设为班长")]
    public class MonitorCommand : ListViewCommand
    {
        /// <summary>
        /// 是否可执行命令
        /// </summary>
        /// <param name="view">ListLogicalView</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.SelectedEntities.Any() && view.SelectedEntities.OfType<Employee>().All(p => p.EmployeeType != EmployeeType.Monitor);
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="view">ListLogicalView</param>
        public override void Execute(ListLogicalView view)
        {
            try
            {
                var selected = view.SelectedEntities.OfType<Employee>();
                foreach (Employee employee in selected)
                {
                    employee.EmployeeType = EmployeeType.Monitor;
                    RF.Save(employee);
                }
            }
            catch (Exception exc)
            {
                exc.Alert();
            }
        }
    }

    /// <summary>
    /// 设为班组长
    /// </summary>
    [Command(Label = "设为班组长")]
    public class ForemanCommand : ListViewCommand
    {
        /// <summary>
        /// 是否可执行命令
        /// </summary>
        /// <param name="view">ListLogicalView</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.SelectedEntities.Any() && view.SelectedEntities.OfType<Employee>().All(p => p.EmployeeType != EmployeeType.Foreman);
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="view">ListLogicalView</param>
        public override void Execute(ListLogicalView view)
        {
            try 
            {
                var selected = view.SelectedEntities.OfType<Employee>();
                foreach (Employee employee in selected)
                {
                    employee.EmployeeType = EmployeeType.Foreman;
                    RF.Save(employee);
                }
            }
            catch (Exception exc)
            {
                exc.Alert();
            }
        }
    }

    /// <summary>
    /// 清空员工类型
    /// </summary>
    [Command(Label = "清空员工类型")]
    public class ClearTypeCommand : ListViewCommand
    {
        /// <summary>
        /// 是否可执行命令
        /// </summary>
        /// <param name="view">ListLogicalView</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.SelectedEntities.Any() && view.SelectedEntities.OfType<Employee>().All(p => p.EmployeeType != null);
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="view">ListLogicalView</param>
        public override void Execute(ListLogicalView view)
        {
                var selected = view.SelectedEntities.OfType<Employee>();
                foreach (Employee employee in selected)
                {
                    employee.EmployeeType = null;
                    RF.Save(employee);
                }
        }
    }
}
