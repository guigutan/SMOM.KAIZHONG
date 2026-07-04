using SIE.Api;
using SIE.Defects;
using SIE.Domain;
using SIE.Items;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Reports;
using SIE.MES.TaskManagement.ShowBoards.ViewModels;
using SIE.MES.WorkOrders;
using SIE.Resources.Employees;
using SIE.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.TaskManagement.ShowBoards
{
    /// <summary>
    /// 看板控制器
    /// </summary>
    public partial class ShowBoardController : DomainController
    {
        /// <summary>
        /// 时间格式
        /// </summary>
        private readonly string _ymd = "yyyy-MM-dd";

        #region 任务计划看板
        /// <summary>
        /// 日任务计划（展示近3日车间任务计划（计划开始日期为当前日期及后两日的所有任务）)
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="resourceId">产线Id</param>
        /// <returns>日任务计划</returns>
        [ApiService("获取日任务计划")]
        [return: ApiReturn("日任务计划 DayPlanTaskInfo")]
        public virtual List<DayPlanTaskInfo> GetDayPlanTaskInfos([ApiParameter("车间Id")] double workShopId, [ApiParameter("产线Id")] double? resourceId)
        {
            var dayPlanTaskInfos = new List<DayPlanTaskInfo>();
            var allWorkGroupIds = new List<double>();
            var allEmployeeGroupIds = new List<double>();
            var dicWorkGroupListOfTask = new Dictionary<double, List<WorkGroup>>();
            var dicEmployeeGroupListOfTask = new Dictionary<double, List<EmployeeGroup>>();
            var tobeDealTasks = new List<DispatchTask>();
            var dicWorkOrderTypesOfTask = new Dictionary<double, Core.WorkOrders.WorkOrderType>();

            //获取近3日车间任务计划（计划开始日期为当前日期及后两日的所有任务）
            var dispatchCt = RT.Service.Resolve<DispatchController>();
            var employeeCt = RT.Service.Resolve<EmployeeController>();
            var dispatchTasks = dispatchCt.GetDispatchTasksByDate(workShopId, resourceId);
            var dispatchTaskIds = dispatchTasks.Select(p => p.Id).Distinct().ToList();
            var dicMergedDispatchTasks = dispatchCt.GetMergedTasks(dispatchTaskIds);

            //获取每个任务的工单类型字典和工单未关闭的任务列表
            GetValidTasks(tobeDealTasks, dicWorkOrderTypesOfTask, dispatchTasks, dicMergedDispatchTasks);

            var tobeDealTaskIds = tobeDealTasks.Select(p => p.Id).Distinct().ToList();
            var dispatchTaskDetails = dispatchCt.GetDispatchTaskDetails(tobeDealTaskIds);
            var dicTaskDetails = dispatchTaskDetails.GroupBy(p => p.DispatchTaskId).ToDictionary(p => p.Key, p => p.ToList());
            var employeeIds = dispatchTaskDetails.Where(p => p.AdoType == AdoType.Employee).Select(p => p.AdoId).Distinct().ToList();
            var employeeGroupIds = dispatchTaskDetails.Where(p => p.AdoType == AdoType.EmployeeGroup).Select(p => p.AdoId).Distinct().ToList();
            var workGroupIds = dispatchTaskDetails.Where(p => p.AdoType == AdoType.WorkGroup).Select(p => p.AdoId).Distinct().ToList();
            var employees = employeeCt.GetEmployeeList(employeeIds);
            var dicEmployees = employees.ToDictionary(p => p.Id);
            var employeeGroupIdsOfemployee = employees.Where(p => p.EmployeeGroupId.HasValue).Select(p => p.EmployeeGroupId).Distinct().Cast<double>().ToList();
            var workGroupIdsOfemployee = employees.Where(p => p.WorkGroupId.HasValue).Select(p => p.WorkGroupId).Distinct().ToList().Cast<double>().ToList();
            allEmployeeGroupIds.AddRange(employeeGroupIds);
            allEmployeeGroupIds.AddRange(employeeGroupIdsOfemployee);
            allWorkGroupIds.AddRange(workGroupIds);
            allWorkGroupIds.AddRange(workGroupIdsOfemployee);
            var allWorkGroupList = employeeCt.GetWorkGroupList(allWorkGroupIds);
            var dicAllWorkGroups = allWorkGroupList.ToDictionary(p => p.Id);
            var allEmployeeGroupList = employeeCt.GetEmployeeGroupList(allEmployeeGroupIds);
            var dicAllEmployeeGroups = allEmployeeGroupList.ToDictionary(p => p.Id);

            tobeDealTasks.ForEach(task =>
            {
                List<DispatchTaskDetail> taskDetails = null;
                if (dicTaskDetails.TryGetValue(task.Id, out taskDetails))
                {
                    if (!dicWorkGroupListOfTask.ContainsKey(task.Id))
                        dicWorkGroupListOfTask.Add(task.Id, new List<WorkGroup>());
                    if (!dicEmployeeGroupListOfTask.ContainsKey(task.Id))
                        dicEmployeeGroupListOfTask.Add(task.Id, new List<EmployeeGroup>());

                    //获取每个任务指派的班组和员工组列表
                    foreach (var taskDetail in taskDetails)
                    {
                        GetTaskDetailInfo(dicWorkGroupListOfTask, dicEmployeeGroupListOfTask, dicEmployees, dicAllWorkGroups, dicAllEmployeeGroups, taskDetail);
                    }
                }
            });

            tobeDealTasks.ForEach(task =>
            {
                Core.WorkOrders.WorkOrderType workOrderType;
                dicWorkOrderTypesOfTask.TryGetValue(task.Id, out workOrderType);
                //按班组创建日计划任务信息
                List<WorkGroup> workGroups = null;
                if (dicWorkGroupListOfTask.TryGetValue(task.Id, out workGroups))
                {
                    foreach (var workGroup in workGroups)
                    {
                        dayPlanTaskInfos.AddRange(CreateDayPlanTaskInfoByDate(workGroup.Name, workOrderType, task));
                    }
                }

                //按员工组创建日计划任务信息
                List<EmployeeGroup> employeeGroups = null;
                if (dicEmployeeGroupListOfTask.TryGetValue(task.Id, out employeeGroups))
                {
                    foreach (var employeeGroup in employeeGroups)
                    {
                        dayPlanTaskInfos.AddRange(CreateDayPlanTaskInfoByDate(employeeGroup.Name, workOrderType, task));
                    }
                }
            });

            //根据组名、工单类型和计划日期分组统计数据
            var dayPlanTaskInfoList = dayPlanTaskInfos.GroupBy(q => new { q.GroupName, q.Type, q.PlanDate }).Select(q => new DayPlanTaskInfo
            {
                GroupName = q.Key.GroupName,
                Type = q.Key.Type,
                PlanDate = q.Key.PlanDate,
                DispatchQty = q.Sum(i => i.DispatchQty),
                DispatchTotalQty = q.Sum(i => i.DispatchTotalQty),
                FinishedDispatchQty = q.Sum(i => i.FinishedDispatchQty),
                FinishedTotalQty = q.Sum(i => i.FinishedTotalQty),
                FinishedRate = "{0}".L10nFormat(Math.Round(q.Sum(i => i.FinishedDispatchQty) / q.Sum(i => i.DispatchQty), 2, MidpointRounding.AwayFromZero) * 100)
            }).OrderByDescending(p => p.PlanDate).ToList();

            return dayPlanTaskInfoList;
        }

        /// <summary>
        /// 获取任务指派的班组和员工组列表
        /// </summary>
        /// <param name="task"></param>
        /// <param name="dicWorkGroupListOfTask"></param>
        /// <param name="dicEmployeeGroupListOfTask"></param>
        /// <param name="dicEmployees"></param>
        /// <param name="dicAllWorkGroups"></param>
        /// <param name="dicAllEmployeeGroups"></param>
        /// <param name="taskDetail"></param>
        private static void GetTaskDetailInfo(Dictionary<double, List<WorkGroup>> dicWorkGroupListOfTask, Dictionary<double, List<EmployeeGroup>> dicEmployeeGroupListOfTask, Dictionary<double, Employee> dicEmployees, Dictionary<double, WorkGroup> dicAllWorkGroups, Dictionary<double, EmployeeGroup> dicAllEmployeeGroups, DispatchTaskDetail taskDetail)
        {
            if (taskDetail.AdoType == AdoType.Employee)
            {
                Employee employee = null;
                if (dicEmployees.TryGetValue(taskDetail.AdoId, out employee))
                {
                    if (taskDetail.AdoGroup == AdoGroup.WorkGroup)
                    {
                        WorkGroup workGroup = null;
                        if (employee.WorkGroupId.HasValue &&
                            dicAllWorkGroups.TryGetValue(employee.WorkGroupId.Value, out workGroup) &&
                            !dicWorkGroupListOfTask[taskDetail.DispatchTaskId].Contains(workGroup))
                        {
                            dicWorkGroupListOfTask[taskDetail.DispatchTaskId].Add(workGroup);
                        }
                    }
                    else if (taskDetail.AdoGroup == AdoGroup.EmployeeGroup && employee.EmployeeGroupId.HasValue)
                    {
                        EmployeeGroup employeeGroup = null;
                        if (dicAllEmployeeGroups.TryGetValue(employee.EmployeeGroupId.Value, out employeeGroup)
                            && !dicEmployeeGroupListOfTask[taskDetail.DispatchTaskId].Contains(employeeGroup))
                        {
                            dicEmployeeGroupListOfTask[taskDetail.DispatchTaskId].Add(employeeGroup);
                        }
                    }
                }
            }
            else if (taskDetail.AdoType == AdoType.EmployeeGroup)
            {
                EmployeeGroup employeeGroup = null;
                if (dicAllEmployeeGroups.TryGetValue(taskDetail.AdoId, out employeeGroup) &&
                    !dicEmployeeGroupListOfTask[taskDetail.DispatchTaskId].Contains(employeeGroup))
                {
                    dicEmployeeGroupListOfTask[taskDetail.DispatchTaskId].Add(employeeGroup);
                }
            }
            else
            {
                WorkGroup workGroup = null;
                if (dicAllWorkGroups.TryGetValue(taskDetail.AdoId, out workGroup) &&
                    !dicWorkGroupListOfTask[taskDetail.DispatchTaskId].Contains(workGroup))
                {
                    dicWorkGroupListOfTask[taskDetail.DispatchTaskId].Add(workGroup);
                }
            }
        }

        /// <summary>
        /// 获取每个任务的工单类型字典和有效的(工单未关闭和未完工)任务列表
        /// </summary>
        /// <param name="tobeDealTasks">有效的任务列表</param>
        /// <param name="dicWorkOrderTypesOfTask">工单类型字典</param>
        /// <param name="dispatchTasks">任务单列表</param>
        /// <param name="dicMergedDispatchTasks">已合并的任务单字典</param>
        private void GetValidTasks(List<DispatchTask> tobeDealTasks, Dictionary<double, Core.WorkOrders.WorkOrderType> dicWorkOrderTypesOfTask, EntityList<DispatchTask> dispatchTasks, Dictionary<double, List<DispatchTask>> dicMergedDispatchTasks)
        {
            dispatchTasks.ForEach(task =>
            {
                if (task.MergedStatus != MergedStatus.MergeRows)
                    GetValidNormalTasks(tobeDealTasks, dicWorkOrderTypesOfTask, task);
                else
                    GetValidMergeRowsTasks(tobeDealTasks, dicWorkOrderTypesOfTask, dicMergedDispatchTasks, task);
            });
        }

        /// <summary>
        /// 获取未合并任务的工单类型字典和有效的(工单未关闭和未完工)任务列表
        /// </summary>
        /// <param name="tobeDealTasks">有效的任务列表</param>
        /// <param name="dicWorkOrderTypesOfTask">工单类型字典</param>
        /// <param name="task">任务单</param>
        private void GetValidNormalTasks(List<DispatchTask> tobeDealTasks, Dictionary<double, Core.WorkOrders.WorkOrderType> dicWorkOrderTypesOfTask, DispatchTask task)
        {
            var workOrder = task.WorkOrder;
            if (workOrder.State != Core.WorkOrders.WorkOrderState.Finish && workOrder.State != Core.WorkOrders.WorkOrderState.Close)
            {
                tobeDealTasks.Add(task);
                dicWorkOrderTypesOfTask.Add(task.Id, task.WorkOrder.Type);
            }
        }

        /// <summary>
        /// 获取已合并任务的工单类型字典和有效的(工单未关闭和未完工)任务列表
        /// </summary>
        /// <param name="tobeDealTasks">有效的任务列表</param>
        /// <param name="dicWorkOrderTypesOfTask">工单类型字典</param>
        /// <param name="dicMergedDispatchTasks">已合并的任务单字典</param>
        /// <param name="task">任务单</param>
        private void GetValidMergeRowsTasks(List<DispatchTask> tobeDealTasks, Dictionary<double, Core.WorkOrders.WorkOrderType> dicWorkOrderTypesOfTask, Dictionary<double, List<DispatchTask>> dicMergedDispatchTasks, DispatchTask task)
        {
            List<DispatchTask> mergedTasks = null;
            if (dicMergedDispatchTasks.TryGetValue(task.Id, out mergedTasks))
            {
                if (mergedTasks.Any(p => p.WorkOrder.State != Core.WorkOrders.WorkOrderState.Close && p.WorkOrder.State != Core.WorkOrders.WorkOrderState.Finish))
                {
                    var mergedTask = mergedTasks.FirstOrDefault();
                    tobeDealTasks.Add(task);
                    dicWorkOrderTypesOfTask.Add(task.Id, mergedTask.WorkOrder.Type);
                }
            }
        }

        /// <summary>
        /// 日任务计划（展示近3日车间任务计划（计划开始日期为当前日期及后两日的所有任务）)
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="resourceId">产线Id</param>
        /// <returns>日任务计划</returns>
        [ApiService("获取日任务计划")]
        [return: ApiReturn("日任务计划 DayPlanTaskInfo")]
        public virtual List<DayPlanTaskInfo> GetDayPlanTaskInfosOf3Day([ApiParameter("车间Id")] double workShopId, [ApiParameter("产线Id")] double? resourceId)
        {
            var dayPlanTaskInfos = new List<DayPlanTaskInfo>();
            var dicWorkGroupListOfTask = new Dictionary<double, List<WorkGroup>>();
            var dicEmployeeGroupListOfTask = new Dictionary<double, List<EmployeeGroup>>();

            //获取近3日车间任务计划（计划开始日期为当前日期及后两日的所有任务）
            var dispatchCt = RT.Service.Resolve<DispatchController>();
            var planTaskInfoList = dispatchCt.GetDispatchTasksOf3Day(workShopId, resourceId);

            //加载任务相关信息
            var taskSimulation = new TaskSimulation();
            taskSimulation.LoadTaskRelateInfo(workShopId, resourceId, planTaskInfoList);

            //根据任务计划信息获取对应的班组和员工组字典
            GetWorkGroupEmployeeInfo(dicWorkGroupListOfTask, dicEmployeeGroupListOfTask, taskSimulation);

            //根据班组和员工组创建日计划任务信息列表
            dayPlanTaskInfos.AddRange(GetDayPlanTaskInfos(dicWorkGroupListOfTask, dicEmployeeGroupListOfTask, taskSimulation.DicTaskInfos));

            return GetTaskInfosByGroupName(dayPlanTaskInfos);
        }

        /// <summary>
        /// 根据组名、工单类型和计划日期分组统计数据
        /// </summary>
        /// <param name="dayPlanTaskInfos">日计划任务信息</param>
        /// <returns>统计后的日计划任务信息列表</returns>
        private List<DayPlanTaskInfo> GetTaskInfosByGroupName(List<DayPlanTaskInfo> dayPlanTaskInfos)
        {
            //根据组名、工单类型和计划日期分组统计数据
            return dayPlanTaskInfos.GroupBy(q => new { q.GroupName, q.Type, q.PlanDate }).Select(q => new DayPlanTaskInfo
            {
                GroupName = q.Key.GroupName,
                Type = q.Key.Type,
                PlanDate = q.Key.PlanDate,
                DispatchQty = q.Sum(i => i.DispatchQty),
                DispatchTotalQty = q.Sum(i => i.DispatchTotalQty),
                FinishedDispatchQty = q.Sum(i => i.FinishedDispatchQty),
                FinishedTotalQty = q.Sum(i => i.FinishedTotalQty),
                FinishedRate = "{0}".L10nFormat(Math.Round(q.Sum(i => i.FinishedDispatchQty) / q.Sum(i => i.DispatchQty), 2, MidpointRounding.AwayFromZero) * 100)
            }).OrderByDescending(p => p.PlanDate).ToList();
        }

        /// <summary>
        /// 获取日计划任务信息列表
        /// </summary>
        /// <param name="dicWorkGroupListOfTask">班组字典</param>
        /// <param name="dicEmployeeGroupListOfTask">员工组字典</param>
        /// <param name="dicTaskInfoList">任务计划信息字典</param>
        /// <returns>日计划任务信息列表</returns>
        private List<DayPlanTaskInfo> GetDayPlanTaskInfos(Dictionary<double, List<WorkGroup>> dicWorkGroupListOfTask, Dictionary<double, List<EmployeeGroup>> dicEmployeeGroupListOfTask, Dictionary<double, List<PlanTaskInfo>> dicTaskInfoList)
        {
            List<DayPlanTaskInfo> dayPlanTaskInfos = new List<DayPlanTaskInfo>();
            dicTaskInfoList.ForEach(task =>
            {
                var planTaskInfo = task.Value.FirstOrDefault();
                var workOrderType = task.Value.Select(p => p.WorkOrderType).FirstOrDefault();
                //按班组创建日计划任务信息
                List<WorkGroup> workGroups = null;
                if (dicWorkGroupListOfTask.TryGetValue(task.Key, out workGroups))
                {
                    foreach (var workGroup in workGroups)
                    {
                        dayPlanTaskInfos.AddRange(CreateDayPlanTaskInfoByDate(workGroup.Name, workOrderType, planTaskInfo));
                    }
                }

                //按员工组创建日计划任务信息
                List<EmployeeGroup> employeeGroups = null;
                if (dicEmployeeGroupListOfTask.TryGetValue(task.Key, out employeeGroups))
                {
                    foreach (var employeeGroup in employeeGroups)
                    {
                        dayPlanTaskInfos.AddRange(CreateDayPlanTaskInfoByDate(employeeGroup.Name, workOrderType, planTaskInfo));
                    }
                }
            });

            return dayPlanTaskInfos;
        }

        /// <summary>
        /// 根据任务计划信息获取对应的班组和员工组字典
        /// </summary>
        /// <param name="dicWorkGroupListOfTask">班组字典</param>
        /// <param name="dicEmployeeGroupListOfTask">员工组字典</param>
        /// <param name="taskSimulation">任务计划信息字典</param>
        private void GetWorkGroupEmployeeInfo(Dictionary<double, List<WorkGroup>> dicWorkGroupListOfTask, Dictionary<double, List<EmployeeGroup>> dicEmployeeGroupListOfTask, TaskSimulation taskSimulation)
        {
            var dicTaskInfoList = taskSimulation.DicTaskInfos;
            var dicTaskDetails = taskSimulation.DicTaskDetails;

            dicTaskInfoList.ForEach(task =>
            {
                List<DispatchTaskDetail> taskDetails = null;
                if (dicTaskDetails.TryGetValue(task.Key, out taskDetails))
                {
                    //初始化班组和员工组字典
                    Initialize(task, dicWorkGroupListOfTask, dicEmployeeGroupListOfTask);

                    //获取每个任务指派的班组和员工组列表
                    foreach (var taskDetail in taskDetails)
                    {
                        if (taskDetail.AdoType == AdoType.Employee)
                            //根据员工获取对应的班组或员工组字典
                            GetWorkEmployeeGroupInfoOfEmployee(task, taskDetail, dicWorkGroupListOfTask, dicEmployeeGroupListOfTask, taskSimulation);
                        else if (taskDetail.AdoType == AdoType.EmployeeGroup) //根据员工组获取对应的员工组字典
                            GetEmployeeGroupInfo(task, taskDetail.AdoId, dicEmployeeGroupListOfTask, taskSimulation.DicAllEmployeeGroups);
                        else  //根据班组获取对应的班组字典
                            GetWorkGroupInfo(task, taskDetail.AdoId, dicWorkGroupListOfTask, taskSimulation.DicAllWorkGroups);
                    }
                }
            });
        }

        /// <summary>
        /// 初始化班组和员工组字典
        /// </summary>
        /// <param name="task">某一任务单的所有任务计划信息列表</param>
        /// <param name="dicWorkGroupListOfTask">班组字典</param>
        /// <param name="dicEmployeeGroupListOfTask">员工组字典</param>        /// 
        private void Initialize(KeyValuePair<double, List<PlanTaskInfo>> task, Dictionary<double, List<WorkGroup>> dicWorkGroupListOfTask, Dictionary<double, List<EmployeeGroup>> dicEmployeeGroupListOfTask)
        {
            if (!dicWorkGroupListOfTask.ContainsKey(task.Key))
                dicWorkGroupListOfTask.Add(task.Key, new List<WorkGroup>());
            if (!dicEmployeeGroupListOfTask.ContainsKey(task.Key))
                dicEmployeeGroupListOfTask.Add(task.Key, new List<EmployeeGroup>());
        }

        /// <summary>
        /// 根据员工获取对应的班组或员工组字典
        /// </summary>
        /// <param name="task">某一任务单的所有任务计划信息列表</param>
        /// <param name="taskDetail">派工任务明细</param>
        /// <param name="dicWorkGroupListOfTask">班组字典</param>
        /// <param name="dicEmployeeGroupListOfTask">员工组字典</param>
        /// <param name="dicEmployees">所有员工字典</param>
        /// <param name="dicAllWorkGroups">所有班组字典</param>
        /// <param name="dicAllEmployeeGroups">所有员工组字典</param>        
        private void GetWorkEmployeeGroupInfoOfEmployee(KeyValuePair<double, List<PlanTaskInfo>> task, DispatchTaskDetail taskDetail, Dictionary<double, List<WorkGroup>> dicWorkGroupListOfTask, Dictionary<double, List<EmployeeGroup>> dicEmployeeGroupListOfTask, TaskSimulation taskSimulation)
        {
            var dicEmployees = taskSimulation.DicEmployees;
            var dicAllWorkGroups = taskSimulation.DicAllWorkGroups;
            var dicAllEmployeeGroups = taskSimulation.DicAllEmployeeGroups;

            Employee employee = null;
            if (dicEmployees.TryGetValue(taskDetail.AdoId, out employee))
            {
                if (taskDetail.AdoGroup == AdoGroup.WorkGroup)
                    if (employee.WorkGroupId.HasValue)
                        GetWorkGroupInfo(task, employee.WorkGroupId.Value, dicWorkGroupListOfTask, dicAllWorkGroups);
                    else if (taskDetail.AdoGroup == AdoGroup.EmployeeGroup)
                        if (employee.EmployeeGroupId.HasValue)
                            GetEmployeeGroupInfo(task, employee.EmployeeGroupId.Value, dicEmployeeGroupListOfTask, dicAllEmployeeGroups);
            }
        }

        /// <summary>
        /// 根据班组获取对应的班组字典
        /// </summary>
        /// <param name="task">某一任务单的所有任务计划信息列表</param>
        /// <param name="workGroupId">班组Id</param>
        /// <param name="dicWorkGroupListOfTask">班组字典</param>
        /// <param name="dicAllWorkGroups">所有班组字典</param>        
        private void GetWorkGroupInfo(KeyValuePair<double, List<PlanTaskInfo>> task, double workGroupId, Dictionary<double, List<WorkGroup>> dicWorkGroupListOfTask, Dictionary<double, WorkGroup> dicAllWorkGroups)
        {
            WorkGroup workGroup = null;
            if (dicAllWorkGroups.TryGetValue(workGroupId, out workGroup))
                if (!dicWorkGroupListOfTask[task.Key].Contains(workGroup))
                    dicWorkGroupListOfTask[task.Key].Add(workGroup);
        }

        /// <summary>
        /// 根据员工组获取对应的员工组字典
        /// </summary>
        /// <param name="task">某一任务单的所有任务计划信息列表</param>
        /// <param name="employeeGroupId">员工组Id</param>
        /// <param name="dicEmployeeGroupListOfTask">员工组字典</param>
        /// <param name="dicAllEmployeeGroups">所有员工组字典</param>        
        private void GetEmployeeGroupInfo(KeyValuePair<double, List<PlanTaskInfo>> task, double employeeGroupId, Dictionary<double, List<EmployeeGroup>> dicEmployeeGroupListOfTask, Dictionary<double, EmployeeGroup> dicAllEmployeeGroups)
        {
            EmployeeGroup employeeGroup = null;
            if (dicAllEmployeeGroups.TryGetValue(employeeGroupId, out employeeGroup))
                if (!dicEmployeeGroupListOfTask[task.Key].Contains(employeeGroup))
                    dicEmployeeGroupListOfTask[task.Key].Add(employeeGroup);
        }

        /// <summary>
        /// 根据日期创建日计划任务信息列表
        /// </summary>
        /// <param name="GroupName">组名</param>
        /// <param name="workOrderType">工单类型</param>
        /// <param name="task">任务单</param>
        /// <returns>日计划任务信息列表</returns>
        private List<DayPlanTaskInfo> CreateDayPlanTaskInfoByDate(string GroupName, Core.WorkOrders.WorkOrderType workOrderType, DispatchTask task)
        {
            var dayPlanTaskInfos = new List<DayPlanTaskInfo>();
            //当天(0时0分0秒和23时59分59秒)
            DateTime now = RF.Find<DispatchTask>().GetDbTime();
            DateTime sToday = new DateTime(now.Year, now.Month, now.Day);
            DateTime eToday = sToday.AddDays(1).AddSeconds(-1);

            //明天(0时0分0秒和23时59分59秒)
            var sTomorrow = sToday.AddDays(1);
            var eTomorrow = eToday.AddDays(1);

            //后天(0时0分0秒和23时59分59秒)
            var sAfterTomorrow = sTomorrow.AddDays(1);
            var eAfterTomorrow = sTomorrow.AddDays(1);

            if (task.PlanBeginTime <= eToday && task.PlanEndTime >= sToday)
            {
                DayPlanTaskInfo dayPlanTaskInfo = CreateDayPlanTaskInfo(GroupName, workOrderType, task, sToday);
                dayPlanTaskInfos.Add(dayPlanTaskInfo);
            }
            if (task.PlanBeginTime <= eTomorrow && task.PlanEndTime >= sTomorrow)
            {
                DayPlanTaskInfo dayPlanTaskInfo = CreateDayPlanTaskInfo(GroupName, workOrderType, task, sTomorrow);
                dayPlanTaskInfos.Add(dayPlanTaskInfo);
            }
            if (task.PlanBeginTime <= eAfterTomorrow && task.PlanEndTime >= sAfterTomorrow)
            {
                DayPlanTaskInfo dayPlanTaskInfo = CreateDayPlanTaskInfo(GroupName, workOrderType, task, sAfterTomorrow);
                dayPlanTaskInfos.Add(dayPlanTaskInfo);
            }

            return dayPlanTaskInfos;
        }

        /// <summary>
        /// 根据日期创建日计划任务信息列表
        /// </summary>
        /// <param name="GroupName">组名</param>
        /// <param name="workOrderType">工单类型</param>
        /// <param name="task">任务单信息</param>
        /// <returns>日计划任务信息列表</returns>
        private List<DayPlanTaskInfo> CreateDayPlanTaskInfoByDate(string GroupName, Core.WorkOrders.WorkOrderType workOrderType, PlanTaskInfo task)
        {
            var dayPlanTaskInfos = new List<DayPlanTaskInfo>();
            //当天(0时0分0秒和23时59分59秒)
            DateTime now = RF.Find<DispatchTask>().GetDbTime();
            DateTime sToday = new DateTime(now.Year, now.Month, now.Day);
            DateTime eToday = sToday.AddDays(1).AddSeconds(-1);

            //明天(0时0分0秒和23时59分59秒)
            var sTomorrow = sToday.AddDays(1);
            var eTomorrow = eToday.AddDays(1);

            //后天(0时0分0秒和23时59分59秒)
            var sAfterTomorrow = sTomorrow.AddDays(1);
            var eAfterTomorrow = sTomorrow.AddDays(1);

            if (task.PlanBeginTime <= eToday && task.PlanEndTime >= sToday)
            {
                DayPlanTaskInfo dayPlanTaskInfo = CreateDayPlanTaskInfo(GroupName, workOrderType, task, sToday);
                dayPlanTaskInfos.Add(dayPlanTaskInfo);
            }
            if (task.PlanBeginTime <= eTomorrow && task.PlanEndTime >= sTomorrow)
            {
                DayPlanTaskInfo dayPlanTaskInfo = CreateDayPlanTaskInfo(GroupName, workOrderType, task, sTomorrow);
                dayPlanTaskInfos.Add(dayPlanTaskInfo);
            }
            if (task.PlanBeginTime <= eAfterTomorrow && task.PlanEndTime >= sAfterTomorrow)
            {
                DayPlanTaskInfo dayPlanTaskInfo = CreateDayPlanTaskInfo(GroupName, workOrderType, task, sAfterTomorrow);
                dayPlanTaskInfos.Add(dayPlanTaskInfo);
            }

            return dayPlanTaskInfos;
        }

        /// <summary>
        /// 创建日计划任务信息
        /// </summary>
        /// <param name="GroupName">组名(员工组/班组)</param>
        /// <param name="workOrderType">工单类型</param>
        /// <param name="task">任务单</param>
        /// <param name="planDate">计划日期</param>
        /// <returns>日计划任务信息</returns>
        private DayPlanTaskInfo CreateDayPlanTaskInfo(string GroupName, Core.WorkOrders.WorkOrderType workOrderType, DispatchTask task, DateTime planDate)
        {
            var dayPlanTaskInfo = new DayPlanTaskInfo()
            {
                GroupName = GroupName,
                Type = EnumViewModel.EnumToLabel(workOrderType).L10N(),
                DispatchQty = 1,
                DispatchTotalQty = task.DispatchQty,
                FinishedTotalQty = task.ReportQty,
                PlanDate = planDate.ToString(_ymd)
            };

            if (task.TaskStatus == DispatchTaskStatus.Finished || task.TaskStatus == DispatchTaskStatus.Closed)
                dayPlanTaskInfo.FinishedDispatchQty = 1;

            return dayPlanTaskInfo;
        }

        /// <summary>
        /// 创建日计划任务信息
        /// </summary>
        /// <param name="GroupName">组名(员工组/班组)</param>
        /// <param name="workOrderType">工单类型</param>
        /// <param name="task">任务单信息</param>
        /// <param name="planDate">计划日期</param>
        /// <returns>日计划任务信息</returns>
        private DayPlanTaskInfo CreateDayPlanTaskInfo(string GroupName, Core.WorkOrders.WorkOrderType workOrderType, PlanTaskInfo task, DateTime planDate)
        {
            var dayPlanTaskInfo = new DayPlanTaskInfo()
            {
                GroupName = GroupName,
                Type = EnumViewModel.EnumToLabel(workOrderType).L10N(),
                DispatchQty = 1,
                DispatchTotalQty = task.DispatchQty,
                FinishedTotalQty = task.ReportQty,
                PlanDate = planDate.ToString(_ymd)
            };

            if (task.TaskStatus == DispatchTaskStatus.Finished || task.TaskStatus == DispatchTaskStatus.Closed)
                dayPlanTaskInfo.FinishedDispatchQty = 1;

            return dayPlanTaskInfo;
        }

        /// <summary>
        /// 获取计划任务统计信息
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="resourceId">产线Id</param>
        /// <returns>计划任务统计信息</returns>
        [ApiService("获取计划任务统计信息")]
        [return: ApiReturn("计划任务统计信息 PlanTaskTotalInfo")]
        public virtual PlanTaskTotalInfo GetPlanTaskTotalInfo([ApiParameter("车间Id")] double workShopId, [ApiParameter("产线Id")] double? resourceId)
        {
            var taskTotalInfo = new PlanTaskTotalInfo();
            var tobeDealTasks = new List<DispatchTask>();
            DateTime now = RF.Find<DispatchTask>().GetDbTime();
            //当天(0时0分0秒和23时59分59秒)
            DateTime sToday = new DateTime(now.Year, now.Month, now.Day);
            DateTime eToday = sToday.AddDays(1).AddSeconds(-1);

            //当月(1日0时0分0秒和当月最后一天23时59分59秒)
            var start = new DateTime(now.Year, now.Month, 1);
            var end = start.AddMonths(1).AddSeconds(-1);
            var taskCt = RT.Service.Resolve<DispatchController>();

            //获取当月的任务单列表
            var dispatchTasks = taskCt.GetDispatchTaskOfMonth(workShopId, resourceId, start, end);
            var dispatchTaskIds = dispatchTasks.Select(p => p.Id).Distinct().ToList();
            var dicMergedDispatchTasks = taskCt.GetMergedTasks(dispatchTaskIds);
            dispatchTasks.ForEach(task =>
            {
                if (task.MergedStatus != MergedStatus.MergeRows)
                {
                    tobeDealTasks.Add(task);
                }
                else
                {
                    List<DispatchTask> mergedTasks = null;
                    if (dicMergedDispatchTasks.TryGetValue(task.Id, out mergedTasks))
                    {
                        if (mergedTasks.Any(p => p.WorkOrder.State != Core.WorkOrders.WorkOrderState.Close))
                        {
                            var mergedTask = mergedTasks.FirstOrDefault();
                            tobeDealTasks.Add(mergedTask);
                        }
                    }
                }
            });

            //获取当天的任务单列表
            var tasksOfDay = tobeDealTasks.Where(p => p.PlanBeginTime <= eToday && p.PlanEndTime >= sToday);

            taskTotalInfo.DispatchTotalQtyOfDay = tasksOfDay.Sum(p => p.DispatchQty);
            taskTotalInfo.FinishedTotalQtyOfDay = tasksOfDay.Sum(p => p.ReportQty);
            taskTotalInfo.DispatchTotalQtyOfMonth = tobeDealTasks.Sum(p => p.DispatchQty);
            taskTotalInfo.FinishedTotalQtyOfMonth = tobeDealTasks.Sum(p => p.ReportQty);

            return taskTotalInfo;
        }

        /// <summary>
        /// 获取计划任务统计信息
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="resourceId">产线Id</param>
        /// <returns></returns>
        [ApiService("获取计划任务统计信息")]
        [return: ApiReturn("计划任务统计信息 PlanTaskTotalInfo")]
        public virtual PlanTaskTotalInfo GetTotalTaskInfos([ApiParameter("车间Id")] double workShopId, [ApiParameter("产线Id")] double? resourceId)
        {
            var taskTotalInfo = new PlanTaskTotalInfo();
            DateTime now = RF.Find<DispatchTask>().GetDbTime();
            //当天(0时0分0秒和23时59分59秒)
            DateTime sToday = new DateTime(now.Year, now.Month, now.Day);
            DateTime eToday = sToday.AddDays(1).AddSeconds(-1);

            //当月(1日0时0分0秒和当月最后一天23时59分59秒)
            var start = new DateTime(now.Year, now.Month, 1);
            var end = start.AddMonths(1).AddSeconds(-1);
            var taskCt = RT.Service.Resolve<DispatchController>();

            //获取当月的任务单列表
            var monthlyTaskInfos = taskCt.GetMonthlyTaskInfos(workShopId, resourceId, start, end);
            var dailyTaskInfos = monthlyTaskInfos.Where(p => p.PlanBeginTime <= eToday && p.PlanEndTime >= sToday);

            taskTotalInfo.DispatchTotalQtyOfDay = dailyTaskInfos.Sum(p => p.DispatchQty);
            taskTotalInfo.FinishedTotalQtyOfDay = dailyTaskInfos.Sum(p => p.ReportQty);
            taskTotalInfo.DispatchTotalQtyOfMonth = monthlyTaskInfos.Sum(p => p.DispatchQty);
            taskTotalInfo.FinishedTotalQtyOfMonth = monthlyTaskInfos.Sum(p => p.ReportQty);

            return taskTotalInfo;
        }


        /// <summary>
        /// 获取异常任务信息
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="resourceId">产线Id</param>
        /// <returns>异常任务信息</returns>
        [ApiService("获取异常任务信息")]
        [return: ApiReturn("异常任务信息 AbnormalTaskInfo")]
        public virtual List<AbnormalTaskInfo> GetAbnormalTaskInfos([ApiParameter("车间Id")] double workShopId, [ApiParameter("产线Id")] double? resourceId)
        {
            var dispatchCt = RT.Service.Resolve<DispatchController>();
            var employeeCt = RT.Service.Resolve<EmployeeController>();
            var dicEmployeeGroupListOfTask = new Dictionary<double, List<EmployeeGroup>>();

            var abnormalTasks = dispatchCt.GetAbnormalTasks(workShopId, resourceId);
            var workOrderIdsOfTask = abnormalTasks.Where(p => p.WorkOrderId.HasValue).Select(p => p.WorkOrderId).ToList().Cast<double>().ToList();

            var dispatchTaskIds = abnormalTasks.Select(p => p.Id).Distinct().ToList();
            var dicMergedDispatchTasks = dispatchCt.GetMergedTasks(dispatchTaskIds);
            var dicWorkOrderListOfTask = new Dictionary<double, List<WorkOrder>>();
            var dicWorkOrderTypesOfTask = new Dictionary<double, Core.WorkOrders.WorkOrderType>();
            //获取每个任务的工单字典和工单未关闭的任务列表
            List<DispatchTask> tobeDealTasks = GetTobeDealTasks(dicWorkOrderListOfTask, dicWorkOrderTypesOfTask, abnormalTasks, workOrderIdsOfTask, dicMergedDispatchTasks);

            //GetWorkGroupsOfTask
            Dictionary<double, List<WorkGroup>> dicWorkGroupListOfTask = GetWorkGroupsOfTask(dispatchCt, employeeCt, dicEmployeeGroupListOfTask, tobeDealTasks);

            var productIdsOfTask = abnormalTasks.Select(p => p.ProductId).ToList();
            var productIds = dicMergedDispatchTasks.SelectMany(p => p.Value).Select(p => p.ProductId).ToList();
            productIds.AddRange(productIdsOfTask);
            productIds = productIds.Distinct().ToList();
            var products = RT.Service.Resolve<ItemController>().GetItemList(productIds);
            var dicProducts = products.ToDictionary(p => p.Id);

            DateTime now = RF.Find<DispatchTask>().GetDbTime();
            var abnormalTaskInfos = new List<AbnormalTaskInfo>();
            tobeDealTasks.ForEach(task =>
            {
                Core.WorkOrders.WorkOrderType workOrderType;
                dicWorkOrderTypesOfTask.TryGetValue(task.Id, out workOrderType);

                //按班组和工单创建异常任务信息
                List<WorkGroup> workGroups = null;
                if (dicWorkGroupListOfTask.TryGetValue(task.Id, out workGroups))
                {
                    foreach (var workGroup in workGroups)
                    {
                        List<WorkOrder> wos = null;
                        if (!dicWorkOrderListOfTask.TryGetValue(task.Id, out wos))
                            continue;

                        foreach (var wo in wos)
                        {
                            Item item = null;
                            dicProducts.TryGetValue(wo.ProductId, out item);
                            var abnormalTaskInfo = new AbnormalTaskInfo()
                            {
                                WorkOrderNo = wo.No,
                                ProductName = item?.Name,
                                GroupName = workGroup.Name,
                                Type = EnumViewModel.EnumToLabel(workOrderType).L10N(),
                                DispatchQty = 1,
                                PlanDate = task.PlanEndTime.ToString(_ymd),
                                UnFinishedQty = 0,
                                ExtendedDays = 0
                            };

                            if (task.TaskStatus != DispatchTaskStatus.Finished && task.TaskStatus != DispatchTaskStatus.Closed)
                                abnormalTaskInfo.UnFinishedQty = 1;
                            if (now > task.PlanEndTime)
                                abnormalTaskInfo.ExtendedDays = Math.Ceiling((now - task.PlanEndTime).TotalDays);
                            abnormalTaskInfos.Add(abnormalTaskInfo);
                        }

                    }
                }

                //按员工组和工单创建异常任务信息
                List<EmployeeGroup> employeeGroups = null;
                if (!dicEmployeeGroupListOfTask.TryGetValue(task.Id, out employeeGroups))
                    return;

                foreach (var employeeGroup in employeeGroups)
                {
                    List<WorkOrder> wos = null;
                    if (!dicWorkOrderListOfTask.TryGetValue(task.Id, out wos))
                        continue;

                    foreach (var wo in wos)
                    {
                        Item item = null;
                        dicProducts.TryGetValue(wo.ProductId, out item);
                        var abnormalTaskInfo = new AbnormalTaskInfo()
                        {
                            WorkOrderNo = wo.No,
                            ProductName = item?.Name,
                            GroupName = employeeGroup.Name,
                            Type = EnumViewModel.EnumToLabel(workOrderType).L10N(),
                            DispatchQty = 1,
                            PlanDate = task.PlanEndTime.ToString(_ymd),
                            UnFinishedQty = 0
                        };

                        if (task.TaskStatus != DispatchTaskStatus.Finished && task.TaskStatus != DispatchTaskStatus.Closed)
                            abnormalTaskInfo.UnFinishedQty = 1;
                        if (now > task.PlanEndTime)
                            abnormalTaskInfo.ExtendedDays = Math.Ceiling((now - task.PlanEndTime).TotalDays);
                        abnormalTaskInfos.Add(abnormalTaskInfo);
                    }
                }
            });

            //根据组名、工单类型和计划日期分组统计数据
            var dayPlanTaskInfoList = abnormalTaskInfos.GroupBy(q => new { q.GroupName, q.WorkOrderNo }).Select(q => new AbnormalTaskInfo
            {
                GroupName = q.Key.GroupName,
                WorkOrderNo = q.Key.WorkOrderNo,
                ProductName = q.Select(p => p.ProductName).FirstOrDefault(),
                Type = q.Select(p => p.Type).FirstOrDefault(),
                PlanDate = q.Max(p => p.PlanDate),
                DispatchQty = q.Sum(i => i.DispatchQty),
                UnFinishedQty = q.Sum(i => i.UnFinishedQty),
                ExtendedDays = q.Max(i => i.ExtendedDays),
            }).ToList();

            return dayPlanTaskInfoList;
        }

        /// <summary>
        /// //获取每个任务指派的班组和员工组列表
        /// </summary>
        /// <param name="dispatchCt"></param>
        /// <param name="employeeCt"></param>
        /// <param name="dicEmployeeGroupListOfTask"></param>
        /// <param name="tobeDealTasks"></param>
        /// <returns></returns>
        private static Dictionary<double, List<WorkGroup>> GetWorkGroupsOfTask(DispatchController dispatchCt, EmployeeController employeeCt, Dictionary<double, List<EmployeeGroup>> dicEmployeeGroupListOfTask, List<DispatchTask> tobeDealTasks)
        {
            var tobeDealTaskIds = tobeDealTasks.Select(p => p.Id).Distinct().ToList();
            var dispatchTaskDetails = dispatchCt.GetDispatchTaskDetails(tobeDealTaskIds);
            var dicTaskDetails = dispatchTaskDetails.GroupBy(p => p.DispatchTaskId).ToDictionary(p => p.Key, p => p.ToList());
            var employeeIds = dispatchTaskDetails.Where(p => p.AdoType == AdoType.Employee).Select(p => p.AdoId).Distinct().ToList();
            var employeeGroupIds = dispatchTaskDetails.Where(p => p.AdoType == AdoType.EmployeeGroup).Select(p => p.AdoId).Distinct().ToList();
            var workGroupIds = dispatchTaskDetails.Where(p => p.AdoType == AdoType.WorkGroup).Select(p => p.AdoId).Distinct().ToList();
            var employees = employeeCt.GetEmployeeList(employeeIds);
            var dicEmployees = employees.ToDictionary(p => p.Id);
            var employeeGroupIdsOfemployee = employees.Where(p => p.EmployeeGroupId.HasValue).Select(p => p.EmployeeGroupId).Distinct().Cast<double>().ToList();
            var workGroupIdsOfemployee = employees.Where(p => p.WorkGroupId.HasValue).Select(p => p.WorkGroupId).Distinct().ToList().Cast<double>().ToList();
            var allEmployeeGroupIds = new List<double>();
            allEmployeeGroupIds.AddRange(employeeGroupIds);
            allEmployeeGroupIds.AddRange(employeeGroupIdsOfemployee);
            allEmployeeGroupIds = allEmployeeGroupIds.Distinct().ToList();
            var allWorkGroupIds = new List<double>();
            allWorkGroupIds.AddRange(workGroupIds);
            allWorkGroupIds.AddRange(workGroupIdsOfemployee);
            allWorkGroupIds = allWorkGroupIds.Distinct().ToList();
            var allWorkGroupList = employeeCt.GetWorkGroupList(allWorkGroupIds);
            var dicAllWorkGroups = allWorkGroupList.ToDictionary(p => p.Id);
            var allEmployeeGroupList = employeeCt.GetEmployeeGroupList(allEmployeeGroupIds);
            var dicAllEmployeeGroups = allEmployeeGroupList.ToDictionary(p => p.Id);

            var dicWorkGroupListOfTask = new Dictionary<double, List<WorkGroup>>();
            tobeDealTasks.ForEach(task =>
            {
                List<DispatchTaskDetail> taskDetails = null;
                if (!dicTaskDetails.TryGetValue(task.Id, out taskDetails))
                    return;

                if (!dicWorkGroupListOfTask.ContainsKey(task.Id))
                    dicWorkGroupListOfTask.Add(task.Id, new List<WorkGroup>());
                if (!dicEmployeeGroupListOfTask.ContainsKey(task.Id))
                    dicEmployeeGroupListOfTask.Add(task.Id, new List<EmployeeGroup>());

                //获取每个任务指派的班组和员工组列表
                foreach (var taskDetail in taskDetails)
                {
                    if (taskDetail.AdoType == AdoType.Employee)
                    {
                        Employee employee = null;
                        if (!dicEmployees.TryGetValue(taskDetail.AdoId, out employee))
                        {
                            continue;
                        }

                        if (taskDetail.AdoGroup == AdoGroup.WorkGroup)
                        {
                            WorkGroup workGroup = null;
                            if (employee.WorkGroupId.HasValue
                                && dicAllWorkGroups.TryGetValue(employee.WorkGroupId.Value, out workGroup)
                                && !dicWorkGroupListOfTask[task.Id].Contains(workGroup))
                            {
                                dicWorkGroupListOfTask[task.Id].Add(workGroup);
                            }
                        }
                        else if (taskDetail.AdoGroup == AdoGroup.EmployeeGroup)
                        {
                            EmployeeGroup employeeGroup = null;
                            if (employee.EmployeeGroupId.HasValue && dicAllEmployeeGroups.TryGetValue(employee.EmployeeGroupId.Value, out employeeGroup) &&
                                !dicEmployeeGroupListOfTask[task.Id].Contains(employeeGroup))
                            {
                                dicEmployeeGroupListOfTask[task.Id].Add(employeeGroup);
                            }
                        }
                        else
                        {
                            //
                        }

                    }
                    else if (taskDetail.AdoType == AdoType.EmployeeGroup)
                    {
                        EmployeeGroup employeeGroup = null;
                        if (dicAllEmployeeGroups.TryGetValue(taskDetail.AdoId, out employeeGroup) && !dicEmployeeGroupListOfTask[task.Id].Contains(employeeGroup))
                        {
                            dicEmployeeGroupListOfTask[task.Id].Add(employeeGroup);
                        }
                    }
                    else
                    {
                        WorkGroup workGroup = null;
                        if (dicAllWorkGroups.TryGetValue(taskDetail.AdoId, out workGroup) && !dicWorkGroupListOfTask[task.Id].Contains(workGroup))
                        {
                            dicWorkGroupListOfTask[task.Id].Add(workGroup);
                        }
                    }
                }

            });
            return dicWorkGroupListOfTask;
        }

        /// <summary>
        /// 获取每个任务的工单字典和工单未关闭的任务列表
        /// </summary>
        /// <param name="dicWorkOrderListOfTask"></param>
        /// <param name="dicWorkOrderTypesOfTask"></param>
        /// <param name="abnormalTasks"></param>
        /// <param name="workOrderIdsOfTask"></param>
        /// <param name="dicMergedDispatchTasks"></param>
        /// <returns></returns>
        private static List<DispatchTask> GetTobeDealTasks(Dictionary<double, List<WorkOrder>> dicWorkOrderListOfTask, Dictionary<double, Core.WorkOrders.WorkOrderType> dicWorkOrderTypesOfTask, EntityList<DispatchTask> abnormalTasks, List<double> workOrderIdsOfTask, Dictionary<double, List<DispatchTask>> dicMergedDispatchTasks)
        {
            var workOrderIds = dicMergedDispatchTasks.SelectMany(p => p.Value).Where(p => p.WorkOrderId.HasValue).Select(p => p.WorkOrderId).ToList().Cast<double>().ToList();
            workOrderIds.AddRange(workOrderIdsOfTask);
            workOrderIds = workOrderIds.Distinct().ToList();
            var workOrders = RT.Service.Resolve<WorkOrderController>().GetWorkOrdersByWoIds(workOrderIds);
            var dicWorkOrders = workOrders.ToDictionary(p => p.Id);
            var tobeDealTasks = new List<DispatchTask>();

            //获取每个任务的工单字典和工单未关闭的任务列表
            abnormalTasks.ForEach(task =>
            {
                if (!dicWorkOrderListOfTask.ContainsKey(task.Id))
                    dicWorkOrderListOfTask.Add(task.Id, new List<WorkOrder>());

                if (task.MergedStatus != MergedStatus.MergeRows)
                {
                    if (!task.WorkOrderId.HasValue)
                        return;

                    if (!dicWorkOrders.TryGetValue(task.WorkOrderId.Value, out WorkOrder wo))
                        return;

                    if (wo.State == Core.WorkOrders.WorkOrderState.Close || wo.State == Core.WorkOrders.WorkOrderState.Finish)
                        return;

                    if (!dicWorkOrderTypesOfTask.ContainsKey(task.Id))
                        dicWorkOrderTypesOfTask.Add(task.Id, wo.Type);
                    if (!dicWorkOrderListOfTask[task.Id].Contains(wo))
                        dicWorkOrderListOfTask[task.Id].Add(wo);
                    tobeDealTasks.Add(task);
                    return;
                }
                List<DispatchTask> mergedTasks = null;
                if (dicMergedDispatchTasks.TryGetValue(task.Id, out mergedTasks))
                    return;

                if (!mergedTasks.Any(p => p.WorkOrder.State != Core.WorkOrders.WorkOrderState.Close && p.WorkOrder.State != Core.WorkOrders.WorkOrderState.Finish))
                    return;

                mergedTasks.ForEach(m =>
                {
                    WorkOrder wo = null;
                    if (dicWorkOrders.TryGetValue(m.WorkOrderId.Value, out wo))
                    {
                        if (!dicWorkOrderTypesOfTask.ContainsKey(task.Id))
                            dicWorkOrderTypesOfTask.Add(task.Id, wo.Type);
                        if (!dicWorkOrderListOfTask[task.Id].Contains(wo))
                            dicWorkOrderListOfTask[task.Id].Add(wo);
                    }
                });

                tobeDealTasks.Add(task);
            });
            return tobeDealTasks;
        }

        /// <summary>
        /// 获取每个任务的工单字典和工单未关闭的任务列表
        /// </summary>
        /// <param name="woCt"></param>
        /// <param name="dicWorkOrderListOfTask"></param>
        /// <param name="dicWorkOrderTypesOfTask"></param>
        /// <param name="dayProduceTasks"></param>
        /// <param name="workOrderIdsOfTask"></param>
        /// <param name="dicMergedDispatchTasks"></param>
        /// <returns></returns>
        private static List<DispatchTask> GetTobeDealTasks(WorkOrderController woCt, Dictionary<double, List<WorkOrder>> dicWorkOrderListOfTask, Dictionary<double, Core.WorkOrders.WorkOrderType> dicWorkOrderTypesOfTask, EntityList<DispatchTask> dayProduceTasks, List<double> workOrderIdsOfTask, Dictionary<double, List<DispatchTask>> dicMergedDispatchTasks)
        {
            var workOrderIds = dicMergedDispatchTasks.SelectMany(p => p.Value).Where(p => p.WorkOrderId.HasValue).Select(p => p.WorkOrderId).ToList().Cast<double>().ToList();
            workOrderIds.AddRange(workOrderIdsOfTask);
            workOrderIds = workOrderIds.Distinct().ToList();
            var workOrders = woCt.GetWorkOrdersByWoIds(workOrderIds);
            var dicWorkOrders = workOrders.ToDictionary(p => p.Id);

            var tobeDealTasks = new List<DispatchTask>();
            //获取每个任务的工单字典和工单未关闭的任务列表
            dayProduceTasks.ForEach(task =>
            {
                if (!dicWorkOrderListOfTask.ContainsKey(task.Id))
                    dicWorkOrderListOfTask.Add(task.Id, new List<WorkOrder>());

                if (task.MergedStatus != MergedStatus.MergeRows)
                {
                    var workOrder = task.WorkOrder;
                    WorkOrder wo = null;
                    if (workOrder != null && dicWorkOrders.TryGetValue(workOrder.Id, out wo))
                    {
                        dicWorkOrderTypesOfTask.Add(task.Id, wo.Type);
                        if (!dicWorkOrderListOfTask[task.Id].Contains(wo))
                        {
                            dicWorkOrderListOfTask[task.Id].Add(wo);
                        }
                    }

                    tobeDealTasks.Add(task);
                }
                else
                {
                    List<DispatchTask> mergedTasks = null;
                    if (dicMergedDispatchTasks.TryGetValue(task.Id, out mergedTasks))
                    {
                        if (mergedTasks.Any(p => p.WorkOrder.State != Core.WorkOrders.WorkOrderState.Close))
                        {
                            var mergedTask = mergedTasks.FirstOrDefault();
                            dicWorkOrderTypesOfTask.Add(task.Id, mergedTask.WorkOrder.Type);
                            tobeDealTasks.Add(task);
                        }

                        mergedTasks.ForEach(m =>
                        {
                            WorkOrder wo = null;
                            if (dicWorkOrders.TryGetValue(m.WorkOrderId.Value, out wo) && !dicWorkOrderListOfTask[task.Id].Contains(wo))
                            {
                                dicWorkOrderListOfTask[task.Id].Add(wo);
                            }
                        });
                    }
                }
            });
            return tobeDealTasks;
        }


        /// <summary>
        /// 获取异常任务信息
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="resourceId">产线Id</param>
        /// <returns>异常任务信息</returns>
        [ApiService("获取异常任务信息")]
        [return: ApiReturn("异常任务信息 AbnormalTaskInfo")]
        public virtual List<AbnormalTaskInfo> GetAbnormalTaskInfos1([ApiParameter("车间Id")] double workShopId, [ApiParameter("产线Id")] double? resourceId)
        {
            var abnormalTaskInfos = new List<AbnormalTaskInfo>();
            var dicWorkGroupListOfTask = new Dictionary<double, List<WorkGroup>>();
            var dicEmployeeGroupListOfTask = new Dictionary<double, List<EmployeeGroup>>();

            DateTime now = RF.Find<DispatchTask>().GetDbTime();
            var dispatchCt = RT.Service.Resolve<DispatchController>();
            var planTaskInfoList = dispatchCt.GetAbnormalTasks1(workShopId, resourceId);

            //加载任务相关信息
            var taskSimulation = new TaskSimulation();
            taskSimulation.LoadTaskRelateInfo(workShopId, resourceId, planTaskInfoList);

            //根据任务计划信息获取对应的班组和员工组字典
            GetWorkGroupEmployeeInfo(dicWorkGroupListOfTask, dicEmployeeGroupListOfTask, taskSimulation);

            //根据班组、员工组和工单创建异常任务信息列表
            abnormalTaskInfos.AddRange(GetAbnormalTasks(dicWorkGroupListOfTask, dicEmployeeGroupListOfTask, now, taskSimulation.DicTaskInfos));

            //根据组名和工单分组统计数据
            return GetAbnormalTasksByGroupName(abnormalTaskInfos);
        }

        /// <summary>
        /// 根据组名和工单分组统计异常任务信息数据
        /// </summary>
        /// <param name="abnormalTaskInfos">异常任务信息列表</param>
        /// <returns>组名和工单分组统计异常任务信息数据</returns>
        private List<AbnormalTaskInfo> GetAbnormalTasksByGroupName(List<AbnormalTaskInfo> abnormalTaskInfos)
        {
            return abnormalTaskInfos.GroupBy(q => new { q.GroupName, q.WorkOrderNo }).Select(q => new AbnormalTaskInfo
            {
                GroupName = q.Key.GroupName,
                WorkOrderNo = q.Key.WorkOrderNo,
                ProductName = q.Select(p => p.ProductName).FirstOrDefault(),
                Type = q.Select(p => p.Type).FirstOrDefault(),
                PlanDate = q.Max(p => p.PlanDate),
                DispatchQty = q.Sum(i => i.DispatchQty),
                UnFinishedQty = q.Sum(i => i.UnFinishedQty),
                ExtendedDays = q.Max(i => i.ExtendedDays),
            }).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dicWorkGroupListOfTask"></param>
        /// <param name="dicEmployeeGroupListOfTask"></param>
        /// <param name="now"></param>
        /// <param name="dicTaskInfoList"></param>
        /// <returns></returns>
        private List<AbnormalTaskInfo> GetAbnormalTasks(Dictionary<double, List<WorkGroup>> dicWorkGroupListOfTask, Dictionary<double, List<EmployeeGroup>> dicEmployeeGroupListOfTask, DateTime now, Dictionary<double, List<PlanTaskInfo>> dicTaskInfoList)
        {
            List<AbnormalTaskInfo> abnormalTaskInfos = new List<AbnormalTaskInfo>();
            dicTaskInfoList.ForEach(task =>
            {
                //按班组和工单创建异常任务信息
                abnormalTaskInfos.AddRange(GetWorkGroupAbnormalTasks(task, dicWorkGroupListOfTask, now));

                //按员工组和工单创建异常任务信息
                abnormalTaskInfos.AddRange(GetEmployeeGroupAbnormalTasks(task, dicEmployeeGroupListOfTask, now));
            });

            return abnormalTaskInfos;
        }

        /// <summary>
        /// 获取按员工组分类的异常任务信息列表
        /// </summary>
        /// <param name="task">以工单号分组的任务信息</param>
        /// <param name="dicEmployeeGroupListOfTask">员工组字典</param>
        /// <param name="now">当前时间</param>
        /// <returns>按员工组分类的异常任务信息列表</returns>
        private List<AbnormalTaskInfo> GetEmployeeGroupAbnormalTasks(KeyValuePair<double, List<PlanTaskInfo>> task, Dictionary<double, List<EmployeeGroup>> dicEmployeeGroupListOfTask, DateTime now)
        {
            List<AbnormalTaskInfo> abnormalTaskInfos = new List<AbnormalTaskInfo>();
            List<EmployeeGroup> employeeGroups = null;
            if (dicEmployeeGroupListOfTask.TryGetValue(task.Key, out employeeGroups))
            {
                foreach (var employeeGroup in employeeGroups)
                {
                    var dicTaskInfosOfWo = task.Value.GroupBy(p => p.WorkOrderNo).ToDictionary(p => p.Key, p => p.ToList());
                    foreach (var dicTaskInfoOfWo in dicTaskInfosOfWo)
                    {
                        var taskInfo = dicTaskInfoOfWo.Value.FirstOrDefault();
                        var abnormalTaskInfo = CreateAbnormalTask(employeeGroup.Name, dicTaskInfoOfWo, taskInfo);
                        if (taskInfo.TaskStatus != DispatchTaskStatus.Finished && taskInfo.TaskStatus != DispatchTaskStatus.Closed)
                            abnormalTaskInfo.UnFinishedQty = 1;
                        if (now > taskInfo.PlanEndTime)
                            abnormalTaskInfo.ExtendedDays = Math.Ceiling((now - taskInfo.PlanEndTime).TotalDays);
                        abnormalTaskInfos.Add(abnormalTaskInfo);
                    }
                }
            }

            return abnormalTaskInfos;
        }

        /// <summary>
        /// 获取按班组分类的异常任务信息列表
        /// </summary>
        /// <param name="task">以工单号分组的任务信息</param>
        /// <param name="dicWorkGroupListOfTask">班组字典</param>
        /// <param name="now">当前时间</param>
        /// <returns>按班组分类的异常任务信息列表</returns>
        private List<AbnormalTaskInfo> GetWorkGroupAbnormalTasks(KeyValuePair<double, List<PlanTaskInfo>> task, Dictionary<double, List<WorkGroup>> dicWorkGroupListOfTask, DateTime now)
        {
            List<AbnormalTaskInfo> abnormalTaskInfos = new List<AbnormalTaskInfo>();
            List<WorkGroup> workGroups = null;
            if (dicWorkGroupListOfTask.TryGetValue(task.Key, out workGroups))
            {
                foreach (var workGroup in workGroups)
                {
                    var dicTaskInfosOfWo = task.Value.GroupBy(p => p.WorkOrderNo).ToDictionary(p => p.Key, p => p.ToList());
                    foreach (var dicTaskInfoOfWo in dicTaskInfosOfWo)
                    {
                        var taskInfo = dicTaskInfoOfWo.Value.FirstOrDefault();
                        var abnormalTaskInfo = CreateAbnormalTask(workGroup.Name, dicTaskInfoOfWo, taskInfo);
                        if (taskInfo.TaskStatus != DispatchTaskStatus.Finished && taskInfo.TaskStatus != DispatchTaskStatus.Closed)
                            abnormalTaskInfo.UnFinishedQty = 1;
                        if (now > taskInfo.PlanEndTime)
                            abnormalTaskInfo.ExtendedDays = Math.Ceiling((now - taskInfo.PlanEndTime).TotalDays);
                        abnormalTaskInfos.Add(abnormalTaskInfo);
                    }
                }
            }

            return abnormalTaskInfos;
        }

        /// <summary>
        /// 创建异常任务信息
        /// </summary>
        /// <param name="workGroupName">班组名称</param>
        /// <param name="dicTaskInfoOfWo">以工单号的任务单信息</param>
        /// <param name="taskInfo"></param>
        /// <returns>某任务单信息</returns>
        private AbnormalTaskInfo CreateAbnormalTask(string workGroupName, KeyValuePair<string, List<PlanTaskInfo>> dicTaskInfoOfWo, PlanTaskInfo taskInfo)
        {
            return new AbnormalTaskInfo()
            {
                WorkOrderNo = dicTaskInfoOfWo.Key,
                ProductName = taskInfo.ProductName,
                GroupName = workGroupName,
                Type = EnumViewModel.EnumToLabel(taskInfo.WorkOrderType).L10N(),
                DispatchQty = 1,
                PlanDate = taskInfo.PlanEndTime.ToString(_ymd),
                UnFinishedQty = 0,
                ExtendedDays = 0
            };
        }

        /// <summary>
        /// 七日产能工时统计
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="resourceId">产线Id</param>
        /// <returns></returns>
        [ApiService("获取七日产能工时信息")]
        [return: ApiReturn("七日产能工时信息 CapacityHourInfo")]
        public virtual List<CapacityHourInfo> GetCapacityHourStatistics([ApiParameter("车间")] double workShopId, [ApiParameter("产线")] double? resourceId)
        {
            var capacityHourInfos = new List<CapacityHourInfo>();
            var reportCt = RT.Service.Resolve<ReportController>();
            var reportRecords = reportCt.GetReportRecordList(workShopId, resourceId);

            //获取产能工时信息列表
            capacityHourInfos.AddRange(GetCapacityHourInfos(reportRecords));

            //根据日期分组获取产能工时统计信息列表
            return GetCapacityHourInfosByDate(capacityHourInfos);
        }

        /// <summary>
        /// 获取根据日期分组获取产能工时统计信息列表
        /// </summary>
        /// <param name="capacityHourInfos">产能工时信息列表</param>
        /// <returns>根据日期分组获取产能工时统计信息列表</returns>
        private List<CapacityHourInfo> GetCapacityHourInfosByDate(List<CapacityHourInfo> capacityHourInfos)
        {
            return capacityHourInfos.GroupBy(q => new { q.PlanDate }).Select(q => new CapacityHourInfo
            {
                PlanDate = q.Key.PlanDate,
                Hour = q.Sum(i => i.Hour),
                FinishedTotalQty = q.Sum(i => i.FinishedTotalQty),
                sHour = q.Min(i => i.Hour),
                eHour = q.Max(i => i.Hour),
                sFinishedTotalQty = q.Min(i => i.FinishedTotalQty),
                eFinishedTotalQty = q.Max(i => i.FinishedTotalQty)
            }).ToList();
        }

        /// <summary>
        /// 获取产能工时信息列表
        /// </summary>
        /// <param name="reportRecords">报工记录列表</param>
        /// <returns>产能工时信息列表</returns>
        private List<CapacityHourInfo> GetCapacityHourInfos(EntityList<ReportRecord> reportRecords)
        {
            List<CapacityHourInfo> capacityHourInfos = new List<CapacityHourInfo>();

            reportRecords.ForEach(p =>
            {
                var capacityHourInfo = new CapacityHourInfo()
                {
                    Hour = p.Hour,
                    FinishedTotalQty = p.ReportQty
                };

                if (p.ReportTime.HasValue)
                    capacityHourInfo.PlanDate = p.ReportTime.Value.ToString(_ymd);

                capacityHourInfos.Add(capacityHourInfo);
            });

            return capacityHourInfos;
        }
        #endregion

        #region 任务生产看板
        /// <summary>
        /// 获取日生产任务信息列表
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="resourceId">产线Id</param>
        /// <returns>日生产任务信息列表</returns>
        [ApiService("获取日生产任务信息")]
        [return: ApiReturn("日生产任务信息 DayProduceTaskInfo")]
        public virtual List<DayProduceTaskInfo> GetDayProduceTaskInfo([ApiParameter("车间")] double workShopId, [ApiParameter("产线")] double? resourceId)
        {
            var itemCt = RT.Service.Resolve<ItemController>();
            var dispatchCt = RT.Service.Resolve<DispatchController>();
            var employeeCt = RT.Service.Resolve<EmployeeController>();
            var woCt = RT.Service.Resolve<WorkOrderController>();
            var dayProduceTaskInfos = new List<DayProduceTaskInfo>();
            var dicWorkOrderListOfTask = new Dictionary<double, List<WorkOrder>>();
            var dicEmployeeGroupListOfTask = new Dictionary<double, List<EmployeeGroup>>();
            var dicWorkOrderTypesOfTask = new Dictionary<double, Core.WorkOrders.WorkOrderType>();

            var dayProduceTasks = dispatchCt.GetDayProduceTasks(workShopId, resourceId);
            var workOrderIdsOfTask = dayProduceTasks.Where(p => p.WorkOrderId.HasValue).Select(p => p.WorkOrderId).ToList().Cast<double>().ToList();
            var productIdsOfTask = dayProduceTasks.Select(p => p.ProductId).ToList();
            var dispatchTaskIds = dayProduceTasks.Select(p => p.Id).Distinct().ToList();
            var dicMergedDispatchTasks = dispatchCt.GetMergedTasks(dispatchTaskIds);
            // 获取每个任务的工单字典和工单未关闭的任务列表
            List<DispatchTask> tobeDealTasks = GetTobeDealTasks(woCt, dicWorkOrderListOfTask, dicWorkOrderTypesOfTask, dayProduceTasks, workOrderIdsOfTask, dicMergedDispatchTasks);
            // 获取每个任务指派的班组和员工组列表
            Dictionary<double, List<WorkGroup>> dicWorkGroupListOfTask = GetDayWorkGroupsOfTask(dispatchCt, employeeCt, dicEmployeeGroupListOfTask, tobeDealTasks);

            var productIds = dicMergedDispatchTasks.SelectMany(p => p.Value).Select(p => p.ProductId).ToList();
            productIds.AddRange(productIdsOfTask);
            productIds = productIds.Distinct().ToList();
            var products = itemCt.GetItemList(productIds);
            var dicProducts = products.ToDictionary(p => p.Id);
            DateTime now = RF.Find<DispatchTask>().GetDbTime();
            tobeDealTasks.ForEach(task =>
            {
                Core.WorkOrders.WorkOrderType workOrderType;
                dicWorkOrderTypesOfTask.TryGetValue(task.Id, out workOrderType);

                //按班组和工单创建异常任务信息
                List<WorkGroup> workGroups = null;
                if (dicWorkGroupListOfTask.TryGetValue(task.Id, out workGroups))
                {
                    foreach (var workGroup in workGroups)
                    {
                        List<WorkOrder> wos = null;
                        if (!dicWorkOrderListOfTask.TryGetValue(task.Id, out wos))
                            continue;

                        foreach (var wo in wos)
                        {
                            Item item = null;
                            dicProducts.TryGetValue(wo.ProductId, out item);
                            var dayProduceTaskInfo = new DayProduceTaskInfo()
                            {
                                WorkOrderNo = wo.No,
                                ProductName = item?.Name,
                                GroupName = workGroup.Name,
                                Type = EnumViewModel.EnumToLabel(workOrderType).L10N(),
                                DispatchQty = 1,
                                DispatchTotalQty = task.DispatchQty,
                                FinishedTotalQty = task.ReportQty,
                                FinishedDispatchQty = 0
                            };

                            if (task.TaskStatus == DispatchTaskStatus.Finished || task.TaskStatus == DispatchTaskStatus.Closed)
                                dayProduceTaskInfo.FinishedDispatchQty = 1;
                            if (now > task.PlanEndTime)
                                dayProduceTaskInfo.ExtendedDays = Math.Ceiling((now - task.PlanEndTime).TotalDays);
                            dayProduceTaskInfos.Add(dayProduceTaskInfo);
                        }

                    }
                }

                //按员工组和工单创建异常任务信息
                List<EmployeeGroup> employeeGroups = null;
                if (dicEmployeeGroupListOfTask.TryGetValue(task.Id, out employeeGroups))
                {
                    foreach (var employeeGroup in employeeGroups)
                    {
                        List<WorkOrder> wos = null;
                        if (!dicWorkOrderListOfTask.TryGetValue(task.Id, out wos))
                        {
                            continue;
                        }
                        foreach (var wo in wos)
                        {
                            Item item = null;
                            dicProducts.TryGetValue(wo.ProductId, out item);
                            var dayProduceTaskInfo = new DayProduceTaskInfo()
                            {
                                WorkOrderNo = wo.No,
                                ProductName = item?.Name,
                                GroupName = employeeGroup.Name,
                                Type = EnumViewModel.EnumToLabel(workOrderType).L10N(),
                                DispatchQty = 1,
                                DispatchTotalQty = task.DispatchQty,
                                FinishedTotalQty = task.ReportQty,
                                FinishedDispatchQty = 0
                            };

                            if (task.TaskStatus == DispatchTaskStatus.Finished || task.TaskStatus == DispatchTaskStatus.Closed)
                                dayProduceTaskInfo.FinishedDispatchQty = 1;
                            if (now > task.PlanEndTime)
                                dayProduceTaskInfo.ExtendedDays = Math.Ceiling((now - task.PlanEndTime).TotalDays);
                            dayProduceTaskInfos.Add(dayProduceTaskInfo);
                        }
                    }
                }
            });
            List<DayProduceTaskInfo> dayPlanTaskInfoList = GetDayProduceTaskInfo(dayProduceTaskInfos);

            return dayPlanTaskInfoList;
        }

        /// <summary>
        /// 获取日生产任务信息
        /// </summary>
        /// <param name="dayProduceTaskInfos"></param>
        /// <returns></returns>
        private static List<DayProduceTaskInfo> GetDayProduceTaskInfo(List<DayProduceTaskInfo> dayProduceTaskInfos)
        {
            return dayProduceTaskInfos.GroupBy(q => new { q.GroupName, q.WorkOrderNo }).Select(q => new DayProduceTaskInfo
            {
                GroupName = q.Key.GroupName,
                WorkOrderNo = q.Key.WorkOrderNo,
                ProductName = q.Select(p => p.ProductName).FirstOrDefault(),
                Type = q.Select(p => p.Type).FirstOrDefault(),
                ExtendedDays = q.Max(p => p.ExtendedDays),
                DispatchQty = q.Sum(i => i.DispatchQty),
                DispatchTotalQty = q.Sum(i => i.DispatchTotalQty),
                FinishedDispatchQty = q.Sum(i => i.FinishedDispatchQty),
                FinishedTotalQty = q.Sum(i => i.FinishedTotalQty),
                FinishedRate = "{0}".L10nFormat(Math.Round(q.Sum(i => i.FinishedDispatchQty) / q.Sum(i => i.DispatchQty), 2, MidpointRounding.AwayFromZero) * 100)
            }).Where(p => p.FinishedRate != "100").OrderByDescending(p => p.ExtendedDays).ToList();
        }

        /// <summary>
        /// 获取每个任务指派的班组和员工组列表
        /// </summary>
        /// <param name="dispatchCt"></param>
        /// <param name="employeeCt"></param>
        /// <param name="dicEmployeeGroupListOfTask"></param>
        /// <param name="tobeDealTasks"></param>
        /// <returns></returns>
        private static Dictionary<double, List<WorkGroup>> GetDayWorkGroupsOfTask(DispatchController dispatchCt, EmployeeController employeeCt, Dictionary<double, List<EmployeeGroup>> dicEmployeeGroupListOfTask, List<DispatchTask> tobeDealTasks)
        {
            var tobeDealTaskIds = tobeDealTasks.Select(p => p.Id).Distinct().ToList();
            var dispatchTaskDetails = dispatchCt.GetDispatchTaskDetails(tobeDealTaskIds);
            var dicTaskDetails = dispatchTaskDetails.GroupBy(p => p.DispatchTaskId).ToDictionary(p => p.Key, p => p.ToList());
            var employeeIds = dispatchTaskDetails.Where(p => p.AdoType == AdoType.Employee).Select(p => p.AdoId).Distinct().ToList();
            var employeeGroupIds = dispatchTaskDetails.Where(p => p.AdoType == AdoType.EmployeeGroup).Select(p => p.AdoId).Distinct().ToList();
            var workGroupIds = dispatchTaskDetails.Where(p => p.AdoType == AdoType.WorkGroup).Select(p => p.AdoId).Distinct().ToList();
            var employees = employeeCt.GetEmployeeList(employeeIds);
            var dicEmployees = employees.ToDictionary(p => p.Id);
            var employeeGroupIdsOfemployee = employees.Where(p => p.EmployeeGroupId.HasValue).Select(p => p.EmployeeGroupId).Distinct().Cast<double>().ToList();
            var workGroupIdsOfemployee = employees.Where(p => p.WorkGroupId.HasValue).Select(p => p.WorkGroupId).Distinct().ToList().Cast<double>().ToList();
            var allEmployeeGroupIds = new List<double>();
            allEmployeeGroupIds.AddRange(employeeGroupIds);
            allEmployeeGroupIds.AddRange(employeeGroupIdsOfemployee);
            var allWorkGroupIds = new List<double>();
            allWorkGroupIds.AddRange(workGroupIds);
            allWorkGroupIds.AddRange(workGroupIdsOfemployee);
            var allWorkGroupList = employeeCt.GetWorkGroupList(allWorkGroupIds);
            var dicAllWorkGroups = allWorkGroupList.ToDictionary(p => p.Id);
            var allEmployeeGroupList = employeeCt.GetEmployeeGroupList(allEmployeeGroupIds);
            var dicAllEmployeeGroups = allEmployeeGroupList.ToDictionary(p => p.Id);

            var dicWorkGroupListOfTask = new Dictionary<double, List<WorkGroup>>();
            tobeDealTasks.ForEach(p =>
            {
                if (!dicTaskDetails.TryGetValue(p.Id, out List<DispatchTaskDetail> taskDetails))
                {
                    return;
                }

                if (!dicWorkGroupListOfTask.ContainsKey(p.Id))
                {
                    dicWorkGroupListOfTask.Add(p.Id, new List<WorkGroup>());
                }

                if (!dicEmployeeGroupListOfTask.ContainsKey(p.Id))
                {
                    dicEmployeeGroupListOfTask.Add(p.Id, new List<EmployeeGroup>());
                }

                //获取每个任务指派的班组和员工组列表
                GetTaskGroupInfo(dicEmployeeGroupListOfTask, dicEmployees, dicAllWorkGroups, dicAllEmployeeGroups, dicWorkGroupListOfTask, taskDetails);

            });
            return dicWorkGroupListOfTask;
        }

        /// <summary>
        /// 获取每个任务指派的班组和员工组列表
        /// </summary>
        /// <param name="dicEmployeeGroupListOfTask"></param>
        /// <param name="dicEmployees"></param>
        /// <param name="dicAllWorkGroups"></param>
        /// <param name="dicAllEmployeeGroups"></param>
        /// <param name="dicWorkGroupListOfTask"></param>
        /// <param name="taskDetails"></param>
        private static void GetTaskGroupInfo(Dictionary<double, List<EmployeeGroup>> dicEmployeeGroupListOfTask, Dictionary<double, Employee> dicEmployees, Dictionary<double, WorkGroup> dicAllWorkGroups, Dictionary<double, EmployeeGroup> dicAllEmployeeGroups, Dictionary<double, List<WorkGroup>> dicWorkGroupListOfTask, List<DispatchTaskDetail> taskDetails)
        {
            foreach (var taskDetail in taskDetails)
            {
                if (taskDetail.AdoType == AdoType.Employee)
                {
                    if (!dicEmployees.TryGetValue(taskDetail.AdoId, out Employee employee))
                    {
                        continue;
                    }

                    if (taskDetail.AdoGroup == AdoGroup.WorkGroup)
                    {
                        if (!employee.WorkGroupId.HasValue)
                        {
                            continue;
                        }

                        if (dicAllWorkGroups.TryGetValue(employee.WorkGroupId.Value, out WorkGroup workGroup) &&
                            !dicWorkGroupListOfTask[taskDetail.DispatchTaskId].Contains(workGroup))
                        {
                            dicWorkGroupListOfTask[taskDetail.DispatchTaskId].Add(workGroup);
                        }
                        continue;
                    }
                    if (taskDetail.AdoGroup == AdoGroup.EmployeeGroup && employee.EmployeeGroupId.HasValue)
                    {
                        if (!dicAllEmployeeGroups.TryGetValue(employee.EmployeeGroupId.Value, out EmployeeGroup employeeGroup))
                        {
                            continue;
                        }

                        if (!dicEmployeeGroupListOfTask[taskDetail.DispatchTaskId].Contains(employeeGroup))
                        {
                            dicEmployeeGroupListOfTask[taskDetail.DispatchTaskId].Add(employeeGroup);
                        }
                    }
                }
                else if (taskDetail.AdoType == AdoType.EmployeeGroup)
                {
                    EmployeeGroup employeeGroup = null;
                    if (dicAllEmployeeGroups.TryGetValue(taskDetail.AdoId, out employeeGroup) &&
                        !dicEmployeeGroupListOfTask[taskDetail.DispatchTaskId].Contains(employeeGroup))
                    {
                        dicEmployeeGroupListOfTask[taskDetail.DispatchTaskId].Add(employeeGroup);
                    }
                }
                else
                {
                    WorkGroup workGroup = null;
                    if (dicAllWorkGroups.TryGetValue(taskDetail.AdoId, out workGroup) &&
                        !dicWorkGroupListOfTask[taskDetail.DispatchTaskId].Contains(workGroup))
                    {
                        dicWorkGroupListOfTask[taskDetail.DispatchTaskId].Add(workGroup);
                    }
                }
            }
        }

        /// <summary>
        /// 获取日生产任务信息列表
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="resourceId">产线Id</param>
        /// <returns>日生产任务信息列表</returns>
        [ApiService("获取日生产任务信息")]
        [return: ApiReturn("日生产任务信息 DayProduceTaskInfo")]
        public virtual List<DayProduceTaskInfo> GetDayProduceTaskInfo1([ApiParameter("车间")] double workShopId, [ApiParameter("产线")] double? resourceId)
        {
            var dayProduceTaskInfos = new List<DayProduceTaskInfo>();
            var dicWorkGroupListOfTask = new Dictionary<double, List<WorkGroup>>();
            var dicEmployeeGroupListOfTask = new Dictionary<double, List<EmployeeGroup>>();

            DateTime now = RF.Find<DispatchTask>().GetDbTime();
            var planTaskInfoList = RT.Service.Resolve<DispatchController>().GetDayProduceTaskInfos(workShopId, resourceId);

            //加载任务相关信息
            var taskSimulation = new TaskSimulation();
            taskSimulation.LoadTaskRelateInfo(workShopId, resourceId, planTaskInfoList);

            //根据任务计划信息获取对应的班组和员工组字典
            GetWorkGroupEmployeeInfo(dicWorkGroupListOfTask, dicEmployeeGroupListOfTask, taskSimulation);

            //按班组、员工组和工单创建异常任务信息
            dayProduceTaskInfos.AddRange(GetProduceTasks(dicWorkGroupListOfTask, dicEmployeeGroupListOfTask, now, taskSimulation.DicTaskInfos));

            //根据组名和工单分组获取异常任务统计信息
            return GetProduceTasksByGroup(dayProduceTaskInfos);
        }

        /// <summary>
        /// 获取根据组名和工单分组获取异常任务统计信息
        /// </summary>
        /// <param name="dayProduceTaskInfos">异常任务信息</param>
        /// <returns>根据组名和工单分组获取异常任务统计信息</returns>
        private List<DayProduceTaskInfo> GetProduceTasksByGroup(List<DayProduceTaskInfo> dayProduceTaskInfos)
        {
            return dayProduceTaskInfos.GroupBy(q => new { q.GroupName, q.WorkOrderNo }).Select(q => new DayProduceTaskInfo
            {
                GroupName = q.Key.GroupName,
                WorkOrderNo = q.Key.WorkOrderNo,
                ProductName = q.Select(p => p.ProductName).FirstOrDefault(),
                Type = q.Select(p => p.Type).FirstOrDefault(),
                ExtendedDays = q.Max(p => p.ExtendedDays),
                DispatchQty = q.Sum(i => i.DispatchQty),
                DispatchTotalQty = q.Sum(i => i.DispatchTotalQty),
                FinishedDispatchQty = q.Sum(i => i.FinishedDispatchQty),
                FinishedTotalQty = q.Sum(i => i.FinishedTotalQty),
                FinishedRate = "{0}".L10nFormat(Math.Round(q.Sum(i => i.FinishedDispatchQty) / q.Sum(i => i.DispatchQty), 2, MidpointRounding.AwayFromZero) * 100)
            }).Where(p => p.FinishedRate != "100").OrderByDescending(p => p.ExtendedDays).ToList();
        }

        /// <summary>
        /// 获取按班组、员工组和工单创建异常任务信息
        /// </summary>
        /// <param name="dicWorkGroupListOfTask">班组字典</param>
        /// <param name="dicEmployeeGroupListOfTask">员工组字典</param>
        /// <param name="now">当前时间</param>
        /// <param name="dicTaskInfoList">任务单信息列表</param>
        /// <returns>按班组、员工组和工单创建异常任务信息</returns>
        private List<DayProduceTaskInfo> GetProduceTasks(Dictionary<double, List<WorkGroup>> dicWorkGroupListOfTask, Dictionary<double, List<EmployeeGroup>> dicEmployeeGroupListOfTask, DateTime now, Dictionary<double, List<PlanTaskInfo>> dicTaskInfoList)
        {
            List<DayProduceTaskInfo> dayProduceTaskInfos = new List<DayProduceTaskInfo>();
            dicTaskInfoList.ForEach(task =>
            {
                //按班组和工单创建异常任务信息
                dayProduceTaskInfos.AddRange(GetWorkGroupProduceTasks(task, dicWorkGroupListOfTask, now));

                //按员工组和工单创建异常任务信息
                dayProduceTaskInfos.AddRange(GetEmployeeGroupProduceTasks(task, dicEmployeeGroupListOfTask, now));
            });

            return dayProduceTaskInfos;
        }

        /// <summary>
        /// 获取按员工组和工单创建异常任务信息
        /// </summary>
        /// <param name="task">以工单号分组的任务信息</param>
        /// <param name="dicEmployeeGroupListOfTask">员工组字典</param>
        /// <param name="now">当前时间</param>
        /// <returns>按员工组和工单创建异常任务信息</returns>
        private List<DayProduceTaskInfo> GetEmployeeGroupProduceTasks(KeyValuePair<double, List<PlanTaskInfo>> task, Dictionary<double, List<EmployeeGroup>> dicEmployeeGroupListOfTask, DateTime now)
        {
            List<DayProduceTaskInfo> dayProduceTaskInfos = new List<DayProduceTaskInfo>();
            List<EmployeeGroup> employeeGroups = null;
            if (dicEmployeeGroupListOfTask.TryGetValue(task.Key, out employeeGroups))
            {
                foreach (var employeeGroup in employeeGroups)
                {
                    var dicTaskInfosOfWo = task.Value.GroupBy(p => p.WorkOrderNo).ToDictionary(p => p.Key, p => p.ToList());
                    foreach (var dicTaskInfoOfWo in dicTaskInfosOfWo)
                    {
                        var taskInfo = dicTaskInfoOfWo.Value.FirstOrDefault();
                        var dayProduceTaskInfo = CreateDayProduceTaskInfo(employeeGroup.Name, taskInfo);
                        if (taskInfo.TaskStatus == DispatchTaskStatus.Finished || taskInfo.TaskStatus == DispatchTaskStatus.Closed)
                            dayProduceTaskInfo.FinishedDispatchQty = 1;
                        if (now > taskInfo.PlanEndTime)
                            dayProduceTaskInfo.ExtendedDays = Math.Ceiling((now - taskInfo.PlanEndTime).TotalDays);
                        dayProduceTaskInfos.Add(dayProduceTaskInfo);
                    }
                }
            }

            return dayProduceTaskInfos;
        }

        /// <summary>
        /// 获取按班组和工单创建异常任务信息
        /// </summary>
        /// <param name="task">以工单号分组的任务信息</param>
        /// <param name="dicWorkGroupListOfTask">班组字典</param>
        /// <param name="now">当前时间</param>
        /// <returns>按班组和工单创建异常任务信息</returns>
        private List<DayProduceTaskInfo> GetWorkGroupProduceTasks(KeyValuePair<double, List<PlanTaskInfo>> task, Dictionary<double, List<WorkGroup>> dicWorkGroupListOfTask, DateTime now)
        {
            List<DayProduceTaskInfo> dayProduceTaskInfos = new List<DayProduceTaskInfo>();
            List<WorkGroup> workGroups = null;
            if (dicWorkGroupListOfTask.TryGetValue(task.Key, out workGroups))
            {
                foreach (var workGroup in workGroups)
                {
                    var dicTaskInfosOfWo = task.Value.GroupBy(p => p.WorkOrderNo).ToDictionary(p => p.Key, p => p.ToList());
                    foreach (var dicTaskInfoOfWo in dicTaskInfosOfWo)
                    {
                        var taskInfo = dicTaskInfoOfWo.Value.FirstOrDefault();
                        var dayProduceTaskInfo = CreateDayProduceTaskInfo(workGroup.Name, taskInfo);
                        if (taskInfo.TaskStatus == DispatchTaskStatus.Finished || taskInfo.TaskStatus == DispatchTaskStatus.Closed)
                            dayProduceTaskInfo.FinishedDispatchQty = 1;
                        if (now > taskInfo.PlanEndTime)
                            dayProduceTaskInfo.ExtendedDays = Math.Ceiling((now - taskInfo.PlanEndTime).TotalDays);
                        dayProduceTaskInfos.Add(dayProduceTaskInfo);
                    }
                }
            }

            return dayProduceTaskInfos;
        }

        /// <summary>
        /// 创建日生产任务信息
        /// </summary>
        /// <param name="groupName">组名</param>
        /// <param name="taskInfo">任务单信息</param>
        /// <returns>日生产任务信息</returns>
        private DayProduceTaskInfo CreateDayProduceTaskInfo(string groupName, PlanTaskInfo taskInfo)
        {
            return new DayProduceTaskInfo()
            {
                WorkOrderNo = taskInfo.WorkOrderNo,
                ProductName = taskInfo.ProductName,
                GroupName = groupName,
                Type = EnumViewModel.EnumToLabel(taskInfo.WorkOrderType).L10N(),
                DispatchQty = 1,
                DispatchTotalQty = taskInfo.DispatchQty,
                FinishedTotalQty = taskInfo.ReportQty,
                FinishedDispatchQty = 0
            };
        }

        /// <summary>
        /// 获取生产缺陷信息列表
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="resourceId">产线Id</param>
        /// <returns>生产缺陷信息列表</returns>
        [ApiService("获取生产缺陷信息")]
        [return: ApiReturn("生产缺陷信息 DayDefectInfo")]
        public virtual List<DayDefectInfo> GetDayDefectInfo([ApiParameter("车间")] double workShopId, [ApiParameter("产线")] double? resourceId)
        {
            List<DayDefectInfo> dayDefectInfos = new List<DayDefectInfo>();
            var reportDefects = RT.Service.Resolve<ReportController>().GetReportDefects(workShopId, resourceId);
            var defectIds = reportDefects.Select(p => p.DefectId).Distinct().ToList();
            var defects = RT.Service.Resolve<DefectController>().GetDefectList(defectIds);
            var dicDefects = defects.ToDictionary(p => p.Id);
            foreach (var reportDefect in reportDefects)
            {
                Defect defect = null;
                var dayDefectInfo = new DayDefectInfo()
                {
                    DefectQty = 1,
                    DefectRate = 0
                };

                if (dicDefects.TryGetValue(reportDefect.DefectId, out defect))
                {
                    dayDefectInfo.DefectName = defect.Description;
                }
                dayDefectInfos.Add(dayDefectInfo);
            }

            var dayDefectInfoList = dayDefectInfos.GroupBy(q => new { q.DefectName }).Select(q => new DayDefectInfo
            {
                DefectName = q.Key.DefectName,
                DefectQty = q.Sum(p => p.DefectQty)
            });

            var top9DefectInfos = dayDefectInfoList.OrderByDescending(p => p.DefectQty).Take(9).ToList();
            var otherDefectInfos = dayDefectInfoList.OrderByDescending(p => p.DefectQty).Skip(9);
            if (otherDefectInfos.Any())
            {
                var dayDefectInfo = new DayDefectInfo()
                {
                    DefectName = "其他",
                    DefectQty = otherDefectInfos.Sum(p => p.DefectQty)
                };

                top9DefectInfos.Add(dayDefectInfo);
            }

            var defectSumQty = top9DefectInfos.Sum(p => p.DefectQty);
            decimal cumQty = 0m;
            foreach (var top9DefectInfo in top9DefectInfos)
            {
                cumQty += top9DefectInfo.DefectQty;
                var defectRate = Math.Round(cumQty / defectSumQty, 2, MidpointRounding.AwayFromZero) * 100;
                top9DefectInfo.DefectSumQty = cumQty;
                top9DefectInfo.DefectRate = defectRate;
            }

            return top9DefectInfos;
        }

        /// <summary>
        /// 获取缺陷良率信息列表
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="resourceId">产线Id</param>
        /// <returns>缺陷良率信息列表</returns>
        [ApiService("获取缺陷良率信息")]
        [return: ApiReturn("缺陷良率信息 DefectRateInfo")]
        public virtual List<DefectRateInfo> GetDefectRateInfo([ApiParameter("车间")] double workShopId, [ApiParameter("产线")] double? resourceId)
        {
            var defectRateInfos = new List<DefectRateInfo>();
            var reportCt = RT.Service.Resolve<ReportController>();
            var reportRecords = reportCt.GetReportRecordList(workShopId, resourceId);
            reportRecords.ForEach(p =>
            {
                var defectRateInfo = new DefectRateInfo()
                {
                    OkQty = p.OkQty,
                    NgQty = p.NgQty,
                    OkRate = 0
                };

                if (p.ReportTime.HasValue)
                    defectRateInfo.PlanDate = p.ReportTime.Value.ToString(_ymd);

                defectRateInfos.Add(defectRateInfo);
            });

            var defectRateInfoList = defectRateInfos.GroupBy(q => new { q.PlanDate }).Select(q => new DefectRateInfo
            {
                PlanDate = q.Key.PlanDate,
                OkQty = q.Sum(p => p.OkQty),
                NgQty = q.Sum(p => p.NgQty),
                OkRate = Math.Round(q.Sum(p => p.OkQty) / (q.Sum(p => p.OkQty) + q.Sum(p => p.NgQty)), 2, MidpointRounding.AwayFromZero) * 100
            }).OrderBy(p => p.PlanDate).ToList();

            return defectRateInfoList;
        }

        #endregion
    }
}
