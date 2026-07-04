using SIE.MES.TaskManagement.Dispatchs;
using SIE.Resources.Employees;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.TaskManagement.ShowBoards.ViewModels
{
    /// <summary>
    /// 任务仿真器
    /// </summary>
    public class TaskSimulation
    {
        /// <summary>
        /// 所有派工任务明细(员工ID,任务单信息列表)
        /// </summary>
        public Dictionary<double, List<PlanTaskInfo>> DicTaskInfos { get; set; }

        /// <summary>
        /// 所有派工任务明细(员工ID,派工任务明细列表)
        /// </summary>
        public Dictionary<double, List<DispatchTaskDetail>> DicTaskDetails { get; set; }

        /// <summary>
        /// 所有员工(员工ID,员工)
        /// </summary>
        public Dictionary<double, Employee> DicEmployees { get; set; }

        /// <summary>
        /// 所有班组(班组ID，班组)
        /// </summary>
        public Dictionary<double, WorkGroup> DicAllWorkGroups { get; set; }

        /// <summary>
        /// 所有员工组(员工组ID,员工组)
        /// </summary>
        public Dictionary<double, EmployeeGroup> DicAllEmployeeGroups { get; set; }

        /// <summary>
        /// 加载任务相关信息
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="resourceId">产线Id</param>
        /// <param name="planTaskInfoList">任务单信息列表</param>
        public void LoadTaskRelateInfo(double workShopId, double? resourceId, List<PlanTaskInfo> planTaskInfoList)
        {
            var allWorkGroupIds = new List<double>();
            var allEmployeeGroupIds = new List<double>();

            var dispatchCt = RT.Service.Resolve<DispatchController>();
            var employeeCt = RT.Service.Resolve<EmployeeController>();

            //缓存任务单信息                     
            this.DicTaskInfos = planTaskInfoList.GroupBy(p => p.DispatchTaskId).ToDictionary(p => p.Key, p => p.ToList());

            //缓存任务单明细信息
            var dispatchTaskIds = planTaskInfoList.Select(p => p.DispatchTaskId).Distinct().ToList();
            var dispatchTaskDetails = dispatchCt.GetDispatchTaskDetails(dispatchTaskIds);
            this.DicTaskDetails = dispatchTaskDetails.GroupBy(p => p.DispatchTaskId).ToDictionary(p => p.Key, p => p.ToList());

            //缓存员工信息
            var employeeIds = dispatchTaskDetails.Where(p => p.AdoType == AdoType.Employee).Select(p => p.AdoId).Distinct().ToList();
            var employees = employeeCt.GetEmployeeList(employeeIds);
            this.DicEmployees = employees.ToDictionary(p => p.Id);

            //缓存班组信息
            var workGroupIds = dispatchTaskDetails.Where(p => p.AdoType == AdoType.WorkGroup).Select(p => p.AdoId).Distinct().ToList();
            allWorkGroupIds.AddRange(workGroupIds);
            var workGroupIdsOfemployee = employees.Where(p => p.WorkGroupId != null).Select(p => p.WorkGroupId).Distinct().ToList().Cast<double>().ToList();
            allWorkGroupIds.AddRange(workGroupIdsOfemployee);
            var allWorkGroupList = employeeCt.GetWorkGroupList(allWorkGroupIds);
            this.DicAllWorkGroups = allWorkGroupList.ToDictionary(p => p.Id);

            //缓存员工组信息
            var employeeGroupIds = dispatchTaskDetails.Where(p => p.AdoType == AdoType.EmployeeGroup).Select(p => p.AdoId).Distinct().ToList();
            allEmployeeGroupIds.AddRange(employeeGroupIds);
            var employeeGroupIdsOfemployee = employees.Where(p => p.EmployeeGroupId != null).Select(p => p.EmployeeGroupId).Distinct().Cast<double>().ToList();
            allEmployeeGroupIds.AddRange(employeeGroupIdsOfemployee);
            var allEmployeeGroupList = employeeCt.GetEmployeeGroupList(allEmployeeGroupIds);
            this.DicAllEmployeeGroups = allEmployeeGroupList.ToDictionary(p => p.Id);
        }
    }
}
