using SIE.Domain;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.Workbench.EmployeeManages
{
    /// <summary>
    /// 员工管理控制器
    /// </summary>
    public class EmployeeManageController : DomainController
    {
        /// <summary>
        /// 获取班组员工出勤列表
        /// </summary>
        /// <param name="groupId">班组ID</param>
        /// <param name="date">日期</param>
        /// <returns>员工出勤列表</returns>
        public virtual EntityList<EmployeeClockingIn> GetEmployeeClockingIns(double groupId, DateTime date)
        {
            return Query<EmployeeClockingIn>().Where(p => p.Employee.WorkGroupId == groupId && p.ClockingInDate == date).ToList();
        }

        /// <summary>
        /// 获取班组员工管理信息
        /// </summary>
        /// <param name="groupId">班组ID</param>
        /// <param name="date">日期</param>
        /// <returns>管理信息</returns>
        public virtual EmployeeManage GetEmployeeManage(double groupId, DateTime date)
        {
            var manage = new EmployeeManage();
            //var controller = RT.Service.Resolve<EmployeeController>();
            //List<string> types = new List<string>();
            //var clockingIns = GetEmployeeClockingIns(groupId, date).Select(p => p.EmployeeId);
            //var groupEmployees = controller.GetEmployeeByWorkGroupId(groupId);
            //var groupOnLoans = controller.GetGroupOnLoans(groupId, null).Select(p => p.Employee);
            //foreach (Employee emp in groupEmployees)
            //{
            //    EmployeeInfo info = CreateEmployeeInfo(clockingIns, emp, "新员工");
            //    manage.Employees.Add(info);
            //}
            //foreach (Employee emp in groupOnLoans)
            //{
            //    EmployeeInfo info = CreateEmployeeInfo(clockingIns, emp, "借调员工", true);
            //    manage.Employees.Add(info);
            //    manage.OnLoanEmployees.Add(info);
            //}
            //manage.DueQty = manage.Employees.Count;
            //manage.AbsenteeismQty = manage.Employees.Where(x => x.IsAbsenteeism == true).Count();
            //manage.ArrivedQty = manage.Employees.Where(x => x.IsAbsenteeism == false).Count();
            //manage.Employees.Add(new EmployeeInfo()
            //{
            //    AddEmployee = true
            //});
            //if (groupEmployees.Count > 0)
            //    types.Add("----新员工");
            //if (groupOnLoans.Count() > 0)
            //    types.Add("——借调员工");
            //manage.EmployeeType = string.Join("   ", types);
            return manage;
        }
    }
}