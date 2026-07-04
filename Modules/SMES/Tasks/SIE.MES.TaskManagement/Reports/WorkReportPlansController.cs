using Microsoft.Scripting.Utils;
using SIE.Domain;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.WorkOrders;
using SIE.Resources.Employees;
using SIE.Resources.Skills;
using SIE.Tech.Processs;
using SIE.Tech.Routings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Reports
{
    /// <summary>
    /// 工单报工方案逻辑控制器
    /// </summary>
    public partial class WorkReportPlansController : DomainController
    {
        /// <summary>
        /// 校验员工技能
        /// </summary>
        /// <param name="reportStaffId">报工人员</param>
        /// <param name="dispatchTasks"></param>
        /// <param name="erroMsg"></param>
        /// <returns></returns>
        public virtual bool CheckEmployeeSkills(double reportStaffId, EntityList<DispatchTask> dispatchTasks,out  string erroMsg)
        {
            var skillCt = RT.Service.Resolve<SkillController>();
            var employeeSkills = skillCt.GetEmployeeSkills(new List<double>() { reportStaffId });

            var dicSkillList = GetSkillListOfTask(dispatchTasks);
            var message = "";
            var employee = RF.GetById<Employee>(reportStaffId);
            foreach (var dispatchTask in dispatchTasks)
            {
                dicSkillList.TryGetValue(dispatchTask.Id, out List<Skill> skillList);
                if (skillList.Any())
                {
                    message += ValidateEmployeeSkill(dispatchTask, employeeSkills, new List<Employee>() { employee }, skillList);
                }
            }
            erroMsg = message;
            return message.IsNullOrEmpty();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dispatchTasks"></param>
        /// <returns></returns>
        private Dictionary<double, List<Skill>> GetSkillListOfTask(EntityList<DispatchTask> dispatchTasks)
        {
            var dicSkillList = new Dictionary<double, List<Skill>>();
            var dispatchTaskOfProcess = dispatchTasks.Where(p => p.Process != null);
            var processIds = dispatchTaskOfProcess.Select(p => p.ProcessId.Value).Distinct().ToList();
            var dispatchTaskOfNullProcess = dispatchTasks.Where(p => p.Process == null);
            var processSkills = RT.Service.Resolve<ProcessController>().GetProcessSkills(processIds);

            foreach (var dispatchTask in dispatchTaskOfNullProcess)
            {
                var workOrderIds = new List<double>();
                if (!dicSkillList.ContainsKey(dispatchTask.Id))
                    dicSkillList.Add(dispatchTask.Id, new List<Skill>());
                workOrderIds.Add(dispatchTask.WorkOrderId.Value);

                var skills = Query<Skill>().Exists<ProcessSkill>(
                    (x, y) => y.Join<Process>((c, d) => c.ProcessId == d.Id && c.IsCheck)
                        .Join<Process, RoutingProcess>((c, d) => c.Id == d.ProcessId)
                         .Join<RoutingProcess, RoutingVersion>((c, d) => c.VersionId == d.Id)
                          .Join<RoutingVersion, WorkOrder>((c, d) => c.Id == d.VersionId && workOrderIds.Contains(d.Id))
                        .Where(p => p.SkillId == x.Id)).ToList();
                dicSkillList[dispatchTask.Id].AddRange(skills);
            }
            var dicProcessSkills = processSkills.GroupBy(p => p.ProcessId).ToDictionary(p => p.Key, p => p.ToList());
            foreach (var dispatchTask in dispatchTaskOfProcess)
            {
                if (!dicSkillList.ContainsKey(dispatchTask.Id))
                    dicSkillList.Add(dispatchTask.Id, new List<Skill>());
                List<ProcessSkill> processSkill = null;
                if (dicProcessSkills.TryGetValue(dispatchTask.ProcessId.Value, out processSkill))
                {
                    var skills = processSkill.Select(p => p.Skill);
                    if (skills.Any())
                    {
                        dicSkillList[dispatchTask.Id].AddRange(skills);
                    }
                }
            }

            return dicSkillList;
        }
        private string ValidateEmployeeSkill(DispatchTask dispatchTask, EntityList<EmployeeSkill> employeeSkills, List<Employee> employeesOfTaskDetail, List<Skill> skillList)
        {
            string errMsg = string.Empty;
            var dicEmployeeSkills = employeeSkills.GroupBy(p => p.EmployeeId).ToDictionary(p => p.Key, p => p.ToList());
            foreach (var employee in employeesOfTaskDetail)
            {
                foreach (var skill in skillList)
                {
                    List<EmployeeSkill> employeeSkillList = null;
                    if (dicEmployeeSkills.TryGetValue(employee.Id, out employeeSkillList))
                    {
                        var skillIds = employeeSkillList.Select(p => p.SkillId).Distinct();
                        if (!skillIds.Contains(skill.Id))
                        {
                            return "任务单[{0}]的已选员工[{1}]没有[{2}]工序技能要求,操作失败！".L10nFormat(dispatchTask.No, employee.Name, skill.Name) + "\n";
                        }
                    }
                    else
                    {
                        return "任务单[{0}]的已选员工[{1}]没有[{2}]工序技能要求,操作失败！".L10nFormat(dispatchTask.No, employee.Name, skill.Name) + "\n";
                    }
                }
            }

            return errMsg;
        }
    }
}
