using DocumentFormat.OpenXml.EMMA;
using DotLiquid.Util;
using IronPython.Runtime.Operations;
using Microsoft.Scripting.Utils;
using NPOI.OpenXmlFormats.Vml;
using SIE.Andon.Andons;
using SIE.Andon.Andons.Enum;
using SIE.Api;
using SIE.Barcodes;
using SIE.Barcodes.WipBatchs;
using SIE.Common;
using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Core.ApiLogs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.LES;
using SIE.Inventory.Task;
using SIE.Items;
using SIE.Items.KzItemCategorys;
using SIE.MES.Outsourcing;
using SIE.MES.Outsourcing.Configs;
using SIE.MES.ProcessProperty;
using SIE.MES.TaskManagement.Configs;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.FeedingRecords;
using SIE.MES.TaskManagement.Models;
using SIE.MES.TaskManagement.Reports.Datas;
using SIE.MES.TaskManagement.Reports.Enums;
using SIE.MES.TaskManagement.SuspectProductLabels;
using SIE.MES.Threshold;
using SIE.MES.WIP.Pressure;
using SIE.MES.WIP.Pressure.Configs;
using SIE.MES.WorkOrders;
using SIE.MES.WorkReportPlans;
using SIE.Packages.ItemLabels;
using SIE.Rbac.InvOrgs;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Process = SIE.Tech.Processs.Process;
using WorkOrder = SIE.MES.WorkOrders.WorkOrder;
using WorkOrderController = SIE.MES.WorkOrders.WorkOrderController;


namespace SIE.MES.TaskManagement.Reports
{
    /// <summary>
    /// 报工API控制器
    /// </summary>
    public partial class ReportController : DomainController
    {
        /// <summary>
        /// 任务单查询
        /// </summary>
        /// <param name="info">报工任务查询信息</param>
        /// <returns>派工任务信息列表</returns>
        [ApiService("任务单查询")]
        [return: ApiReturn("派工任务信息列表. 参数类型: List<DispatchTaskInfo>")]
        public virtual WorkOrderTaskInfo QueryDispatchTaskInfo([ApiParameter("报工任务查询信息")] TaskQueryInfo info)
        {
            if (info == null)
            {
                throw new ValidationException("参数异常".L10N());
            }

            WorkOrderTaskInfo workOrderTaskInfo = new WorkOrderTaskInfo();
            ValidateDispatchTaskQueryInfo(info);
            Dictionary<DispatchTaskStatus, List<DispatchTask>> dicTasks = GetDispatchTaskDic(info, 0);
            Dictionary<DispatchTaskStatus, List<DispatchTask>> dicTasksAll = GetAllDispatchTaskDic(info, 0);
            List<DispatchTask> tasks = new List<DispatchTask>();
            List<DispatchTask> dispatchingtasks = null;
            List<DispatchTask> toDispatchtasks = null;
            switch (info.TaskType)
            {   //0待处理任务
                case 0:
                    dicTasks.TryGetValue(DispatchTaskStatus.Dispatched, out tasks);
                    if (info.Visiable)//未派工 派工中可显示
                    {
                        dicTasks.TryGetValue(DispatchTaskStatus.Dispatching, out dispatchingtasks);
                        dicTasks.TryGetValue(DispatchTaskStatus.ToDispatch, out toDispatchtasks);
                        tasks = GetTasks(tasks, dispatchingtasks, toDispatchtasks);
                    }
                    break;
                case 1:
                    dicTasks.TryGetValue(DispatchTaskStatus.Executing, out tasks);
                    dicTasks.TryGetValue(DispatchTaskStatus.Dispatched, out dispatchingtasks);
                    tasks = GetTasks(tasks, dispatchingtasks, null);
                    break;
                case 2:
                    dicTasks.TryGetValue(DispatchTaskStatus.Finished, out tasks);
                    if (tasks != null && tasks.Count > 0)
                        tasks = tasks.OrderByDescending(p => p.UpdateDate).ToList();
                    break;
                //case -1:
                //    dicTasks.TryGetValue(DispatchTaskStatus.Dispatched, out tasks);
                //    dicTasks.TryGetValue(DispatchTaskStatus.Dispatching, out dispatchingtasks);
                //    tasks = GetTasks(tasks, dispatchingtasks, null);
                //    break;
                default:
                    break;
            }


            SetTaskTotalQty(info, workOrderTaskInfo, dicTasks);
            SetTaskTotalQty(info, workOrderTaskInfo, dicTasksAll);
            DateTime now = RF.Find<DispatchTask>().GetDbTime();
            var config = RT.Service.Resolve<DispatchController>().GetDispatchTaskConfig();
            if (tasks != null)
            {
                var taskIds = tasks.Select(p => p.Id).ToList();

                //获取工序属性
                var processIds = tasks.Where(p => p.ProcessId != null).Select(p => p.ProcessId.Value).Distinct().ToList();

                var saveRecord = GetReportRecordIds(taskIds, false);
                tasks.OrderBy(p => p.PlanBeginTime).ForEach(task =>
                {

                    EntityList<ProcessPty> processPtys = RT.Service.Resolve<ProcessPtyController>().GetProcessPtysByProcessIds(new List<double>() { task.ProcessId.Value }, task.ProductId);
                    var kzItemCategory = RT.Service.Resolve<KzItemCategorysController>().GetKzItemCategorieByItemId(task.ProductId);
                    var pps = new List<ProcessPty>();
                    if (kzItemCategory != null)
                    {
                        pps = processPtys.Where(p => p.KzCategoryId == kzItemCategory.KzCategoryId).ToList();
                    }
                    ////当找得到分类得时候，优先找到分类的，然后再找工序的
                    if (pps.Count == 0)
                        pps = processPtys.Where(p => p.KzCategoryId == null).ToList();

                    var layoutInfo = task.WorkOrder.LayoutInfoList.FirstOrDefault(p => p.ProcessCode == task.Process.Code);

                    var tupe = RT.Service.Resolve<DispatchController>().MaxReportQtyAndMaxRemainQty(task);
                    var maxReportQty = tupe.Item1;
                    var MaxRemainQty = tupe.Item2;
                    var ProcessMaxRemainQty = RT.Service.Resolve<DispatchController>().GetProcessMaxRemainQty(task);

                    DispatchTaskInfo taskInfo = new DispatchTaskInfo()
                    {
                        TaskId = task.Id,
                        TaskNo = task.No,
                        TaskStatus = task.TaskStatus.ToLabel().L10N(),
                        TaskStatusValue = task.TaskStatus,
                        ItemCode = task.ProductCode,
                        ItemName = task.ProductName,
                        ShortDescription = task.ShortDescription,
                        TaskQty = task.DispatchQty,
                        ReportQty = task.RemainQty,
                        RemainQty = task.RemainQty,
                        MaxRemainQty = MaxRemainQty,//task.MaxRemainQty,
                        ProcessMaxRemainQty = ProcessMaxRemainQty,//maxReportQty
                        Priority = task.Priority.ToLabel(),
                        PriorityValue = task.Priority,
                        ResourceId = task.ResourceId,
                        ResourceName = task.ResourceName,
                        ProcessId = task.ProcessId,
                        ProcessName = task.ProcessName,
                        PlanBeginDate = task.PlanBeginTime,
                        PlanEndDate = task.PlanEndTime,
                        ActualBeginDate = task.BeginTime,
                        ActualEndDate = task.EndTime,
                        CanStart = ValidateIsCanStartTask(config, task.PlanBeginTime, task.Priority, false),
                        IsSyntype = task.IsSyntype,
                        IsCanQuickReport = saveRecord.Any(p => p == task.Id),
                        WorkOrderId = task.WorkOrder.Id,
                        WorkOrderNo = task.WorkOrder.No,
                        PlanQty = task.WorkOrder.PlanQty,
                        Zcode = layoutInfo?.Zcode ?? 0,
                        IsReportValid = pps.FirstOrDefault(p => p.ProcessId == task.ProcessId)?.IsReportValid ?? false
                    };
                    //var processPty = processPtys.FirstOrDefault(p => p.ProcessId == task.ProcessId);
                    ////已完成的要重新计算已报工数
                    //if (info.TaskType == 2 && processPty != null && processPty.IsPbcd == true)
                    //    taskInfo.ReportQty = task.WorkOrder.ProcessBomList.Where(p => p.ProcessId == task.ProcessId).Sum(p => p.SingleQty) * task.WorkOrder.PlanQty - task.ReportQty - task.SuspectQty;
                    //完成的单据，已报工数计算不一样
                    if (info.TaskType == 2)
                        taskInfo.ReportQty = task.ReportQty;
                    DateTime date = task.EndTime.HasValue ? task.EndTime.Value : now;
                    taskInfo.Progress = date <= task.PlanEndTime ? 0 : 1;
                    workOrderTaskInfo.TaskInfos.Add(taskInfo);
                });
            }
            return workOrderTaskInfo;
        }

        /// <summary>
        /// 获取派工中和待派工任务
        /// </summary>
        /// <param name="tasks"></param>
        /// <param name="dispatchingtasks"></param>
        /// <param name="toDispatchtasks"></param>
        /// <returns></returns>
        private List<DispatchTask> GetTasks(List<DispatchTask> tasks, List<DispatchTask> dispatchingtasks, List<DispatchTask> toDispatchtasks)
        {
            if (dispatchingtasks != null && dispatchingtasks.Any())
            {
                if (tasks == null)
                { tasks = new List<DispatchTask>(); }
                tasks.AddRange(dispatchingtasks);
            }
            if (toDispatchtasks != null && toDispatchtasks.Any())
            {
                if (tasks == null)
                { tasks = new List<DispatchTask>(); }
                tasks.AddRange(toDispatchtasks);
            }

            return tasks;
        }

        /// <summary>
        /// 任务单查询
        /// </summary>
        /// <param name="info">报工任务查询信息</param>
        /// <returns>派工任务信息列表</returns>
        [ApiService("任务单查询")]
        [return: ApiReturn("派工任务信息列表. 参数类型: List<DispatchTaskInfo>")]
        public virtual WorkOrderTaskInfo QueryDispatchTaskNum([ApiParameter("报工任务查询信息")] TaskQueryInfo info)
        {
            if (info == null)
            {
                throw new ValidationException("查询参数不能为空，请检查".L10N());
            }
            WorkOrderTaskInfo workOrderTaskInfo = new WorkOrderTaskInfo();
            ValidateDispatchTaskQueryInfo(info);
            Dictionary<DispatchTaskStatus, List<DispatchTask>> dicTasks = GetDispatchTaskDic(info);
            Dictionary<DispatchTaskStatus, List<DispatchTask>> dicTasksAll = GetAllDispatchTaskDic(info);//不分页
            List<DispatchTask> tasks = null;
            switch (info.TaskType)
            {
                case 0:
                    dicTasks.TryGetValue(DispatchTaskStatus.Dispatched, out tasks);
                    break;
                case 1:
                    dicTasks.TryGetValue(DispatchTaskStatus.Executing, out tasks);
                    break;
                case 2:
                    {
                        dicTasks.TryGetValue(DispatchTaskStatus.Finished, out tasks);
                        if (tasks != null && tasks.Count > 0)
                            tasks = tasks.OrderByDescending(p => p.UpdateDate).ToList();
                    }
                    break;
                default:
                    throw new ValidationException("查询任务类型错误，请检查".L10N());
            }
            SetTaskTotalQty(info, workOrderTaskInfo, dicTasks);
            SetTaskTotalQty(info, workOrderTaskInfo, dicTasksAll);//不分页显示所有待处理，处理中的数据给PDA
            DateTime now = RF.Find<DispatchTask>().GetDbTime();
            var config = RT.Service.Resolve<DispatchController>().GetDispatchTaskConfig();
            if (tasks != null)
            {
                var taskIds = tasks.Select(p => p.Id).ToList();

                //获取工序属性
                var processIds = tasks.Where(p => p.ProcessId != null).Select(p => p.ProcessId.Value).Distinct().ToList();

                var saveRecord = GetReportRecordIds(taskIds, false);
                tasks.ForEach(task =>
                {

                    EntityList<ProcessPty> processPtys = RT.Service.Resolve<ProcessPtyController>().GetProcessPtysByProcessIds(new List<double>() { task.ProcessId.Value }, task.ProductId);
                    var kzItemCategory = RT.Service.Resolve<KzItemCategorysController>().GetKzItemCategorieByItemId(task.ProductId);
                    var pps = new List<ProcessPty>();
                    if (kzItemCategory != null)
                    {
                        pps = processPtys.Where(p => p.KzCategoryId == kzItemCategory.KzCategoryId).ToList();
                    }
                    ////当找得到分类得时候，优先找到分类的，然后再找工序的
                    if (pps.Count == 0)
                        pps = processPtys.Where(p => p.KzCategoryId == null).ToList();

                    var layoutInfo = task.WorkOrder.LayoutInfoList.FirstOrDefault(p => p.ProcessCode == task.Process.Code);
                    var MaxRemainQty = RT.Service.Resolve<DispatchController>().MaxReportQtyAndMaxRemainQty(task).Item2;
                    var ProcessMaxRemainQty = RT.Service.Resolve<DispatchController>().GetProcessMaxRemainQty(task);

                    DispatchTaskInfo taskInfo = new DispatchTaskInfo()
                    {
                        TaskId = task.Id,
                        TaskNo = task.No,
                        TaskStatus = task.TaskStatus.ToLabel(),
                        TaskStatusValue = task.TaskStatus,
                        ItemCode = task.ProductCode,
                        ItemName = task.ProductName,
                        TaskQty = task.DispatchQty,
                        ReportQty = task.RemainQty,
                        RemainQty = task.RemainQty,
                        MaxRemainQty = MaxRemainQty,//task.MaxRemainQty,
                        ProcessMaxRemainQty = ProcessMaxRemainQty,
                        Priority = task.Priority.ToLabel(),
                        PriorityValue = task.Priority,
                        ResourceId = task.ResourceId,
                        ResourceName = task.ResourceName,
                        ProcessId = task.ProcessId,
                        ProcessName = task.ProcessName,
                        PlanBeginDate = task.PlanBeginTime,
                        PlanEndDate = task.PlanEndTime,
                        ActualBeginDate = task.BeginTime,
                        ActualEndDate = task.EndTime,
                        CanStart = ValidateIsCanStartTask(config, task.PlanBeginTime, task.Priority, false),
                        IsSyntype = task.IsSyntype,
                        IsCanQuickReport = saveRecord.Any(p => p == task.Id),
                        WorkOrderId = task.WorkOrder.Id,
                        WorkOrderNo = task.WorkOrder.No,
                        Zcode = layoutInfo?.Zcode ?? 0,
                        IsReportValid = pps.FirstOrDefault(p => p.ProcessId == task.ProcessId)?.IsReportValid ?? false
                    };
                    //var processPty = processPtys.FirstOrDefault(p => p.ProcessId == task.ProcessId);
                    //if (info.TaskType == 2 && processPty != null && processPty.IsPbcd == true)
                    //    taskInfo.ReportQty = task.WorkOrder.ProcessBomList.Where(p => p.ProcessId == task.ProcessId).Sum(p => p.SingleQty) * task.WorkOrder.PlanQty - task.ReportQty - task.SuspectQty;
                    //完成的单据，已报工数计算不一样
                    if (info.TaskType == 2)
                        taskInfo.ReportQty = task.ReportQty;
                    DateTime date = task.EndTime.HasValue ? task.EndTime.Value : now;
                    taskInfo.Progress = date <= task.PlanEndTime ? 0 : 1;
                    workOrderTaskInfo.TaskInfos.Add(taskInfo);
                });
            }
            return workOrderTaskInfo;
        }

        /// <summary>
        /// 根据不同状态任务设置总数
        /// </summary>
        /// <param name="info">报工任务查询信息</param>
        /// <param name="workOrderTaskInfo">工单任务信息</param>
        /// <param name="dicTasks">任务单字典</param>
        private void SetTaskTotalQty(TaskQueryInfo info, WorkOrderTaskInfo workOrderTaskInfo, Dictionary<DispatchTaskStatus, List<DispatchTask>> dicTasks)
        {
            workOrderTaskInfo.TaskType = info.TaskType;
            if (info.Visiable)
            {
                workOrderTaskInfo.PendingQty = !dicTasks.ContainsKey(DispatchTaskStatus.Dispatched) ? 0 : dicTasks[DispatchTaskStatus.Dispatched].Count;
                workOrderTaskInfo.PendingQty += !dicTasks.ContainsKey(DispatchTaskStatus.Dispatching) ? 0 : dicTasks[DispatchTaskStatus.Dispatching].Count;
                workOrderTaskInfo.PendingQty += !dicTasks.ContainsKey(DispatchTaskStatus.ToDispatch) ? 0 : dicTasks[DispatchTaskStatus.ToDispatch].Count;
            }
            else
            {
                workOrderTaskInfo.PendingQty = !dicTasks.ContainsKey(DispatchTaskStatus.Dispatched) ? 0 : dicTasks[DispatchTaskStatus.Dispatched].Count;
            }

            workOrderTaskInfo.ProcessingQty = !dicTasks.ContainsKey(DispatchTaskStatus.Executing) ? 0 : dicTasks[DispatchTaskStatus.Executing].Count;
            workOrderTaskInfo.ProcessingQty += !dicTasks.ContainsKey(DispatchTaskStatus.Dispatched) ? 0 : dicTasks[DispatchTaskStatus.Dispatched].Count;
            var finishQty = !dicTasks.ContainsKey(DispatchTaskStatus.Finished) ? 0 : dicTasks[DispatchTaskStatus.Finished].Count;
            var closeQty = !dicTasks.ContainsKey(DispatchTaskStatus.Closed) ? 0 : dicTasks[DispatchTaskStatus.Closed].Count;
            workOrderTaskInfo.CompletedQty = finishQty + closeQty;
        }

        /// <summary>
        /// 获取报工任务字典
        /// </summary>
        /// <param name="info">报工任务查询信息</param>
        /// <param name="type">类型(0:PDA手动报工)</param>
        /// <returns>任务字典</returns>
        private Dictionary<DispatchTaskStatus, List<DispatchTask>> GetDispatchTaskDic(TaskQueryInfo info, int? type = null)
        {
            var status = new List<DispatchTaskStatus>() { DispatchTaskStatus.Dispatched, DispatchTaskStatus.Dispatching, DispatchTaskStatus.ToDispatch, DispatchTaskStatus.Executing, DispatchTaskStatus.Finished, DispatchTaskStatus.Closed };
            PagingInfo pagingInfo = new PagingInfo(info.PageNumber ?? 1, info.PageSize ?? int.MaxValue - 1, true);
            var firstProcess = false;
            //var config = RT.Service.Resolve<DispatchController>().GetDispatchConfig();
            //if (config != null && config.IsFirstProcess == true)
            //{
            //    firstProcess = true;
            //}
            var taskList = RT.Service.Resolve<DispatchController>().GetDispatchTasksByEmployee(info, status, pagingInfo, !firstProcess, firstProcess, type);
            var dicTasks = taskList.GroupBy(p => p.TaskStatus).ToDictionary(p => p.Key, p => p.ToList());
            return dicTasks;
        }

        /// <summary>
        /// 获取报工任务字典
        /// </summary>
        /// <param name="info">报工任务查询信息</param>
        /// <param name="type">类型(0:PDA手动报工)</param>
        /// <returns>任务字典</returns>
        private Dictionary<DispatchTaskStatus, List<DispatchTask>> GetAllDispatchTaskDic(TaskQueryInfo info, int? type = null)
        {
            var status = new List<DispatchTaskStatus>() { DispatchTaskStatus.Dispatched, DispatchTaskStatus.Dispatching, DispatchTaskStatus.ToDispatch, DispatchTaskStatus.Executing, DispatchTaskStatus.Finished, DispatchTaskStatus.Closed };
            var firstProcess = false;
            //var config = RT.Service.Resolve<DispatchController>().GetDispatchConfig();
            //if (config != null && config.IsFirstProcess == true)
            //{
            //    firstProcess = true;
            //}
            var taskList = RT.Service.Resolve<DispatchController>().GetDispatchTasksByEmployee(info, status, null, !firstProcess, firstProcess, type);
            var dicTasks = taskList.GroupBy(p => p.TaskStatus).ToDictionary(p => p.Key, p => p.ToList());
            return dicTasks;
        }

        /// <summary>
        /// 验证报工任务查询条件
        /// </summary>
        /// <param name="info">报工任务查询信息</param>
        void ValidateDispatchTaskQueryInfo(TaskQueryInfo info)
        {
            if (info == null)
                throw new ValidationException("任务单查询条件不能为空".L10N());
            if (info.EmployeeId == 0)
                throw new ValidationException("员工不能为空".L10N());
            //if (info.ResourceId == 0)
            //    throw new ValidationException("资源不能为空".L10N());
        }

        /// <summary>
        /// 任务开工
        /// </summary>
        /// <param name="dispatchTaskId">派工任务ID</param>
        [ApiService("任务开工")]
        public virtual void TaskStart([ApiParameter("派工任务ID")] double dispatchTaskId)
        {
            var employee = RT.Service.Resolve<EmployeeController>().GetEmployeeById(RT.IdentityId);
            StartWork(employee, RF.GetById<DispatchTask>(dispatchTaskId));
        }

        /// <summary>
        /// 获取报工信息
        /// </summary>
        /// <param name="dispatchTaskId">派工任务ID</param>
        [ApiService("获取报工信息")]
        [return: ApiReturn("报工任务信息. 参数类型: ReportTaskInfo")]
        public virtual ReportTaskInfo GetReportTaskInfo([ApiParameter("派工任务ID")] double dispatchTaskId)
        {
            return GetReportTaskRecordInfo(dispatchTaskId);
        }

        /// <summary>
        /// 任务单首件报检
        /// </summary>
        /// <param name="dispatchTaskId">派工任务ID</param>、
        /// <param name="inspQty">报检数量</param>
        /// <returns>首件报检成功返回true，失败返回false</returns>
        [ApiService("任务单首件报检")]
        public virtual bool TaskReportFirstInsp([ApiParameter("派工任务ID")] double dispatchTaskId, [ApiParameter("报检数量")] int inspQty)
        {
            var task = RF.GetById<DispatchTask>(dispatchTaskId, new EagerLoadOptions().LoadWithViewProperty());
            if (task == null)
                throw new ValidationException("未找到派工任务".L10N());
            if (task.ReportMode != ReportMode.Manual)
                throw new ValidationException("不允许首件报检，派工任务报工方式非手动方式".L10N());
            if (task.TaskStatus != DispatchTaskStatus.Executing)
                throw new ValidationException("不允许首件报检，任务单非执行状态".L10N());
            if (task.IsVirtualPart)
                throw new ValidationException("虚拟件任务单不允许首件报检".L10N());
            if (task.SpecificationCode.IsNotEmpty())
                throw new ValidationException("规格件任务单不允许首件报检".L10N());
            task.InspQty = inspQty;
            ReportFirstInsp(task);
            return true;
        }

        /// <summary>
        /// 快速任务报工
        /// </summary>
        /// <param name="dispatchTaskId">派工任务ID</param>
        /// <param name="okQty"></param>
        /// <returns>快速报工成功返回true，失败返回false</returns>
        [ApiService("快速任务报工")]
        public virtual bool TaskReportQuick([ApiParameter("派工任务ID")] double dispatchTaskId, [ApiParameter("用户数量")] decimal okQty)
        {
            var task = RF.GetById<DispatchTask>(dispatchTaskId);
            if (task == null)
                throw new ValidationException("未找到派工任务".L10N());
            if (okQty <= 0)
            {
                throw new ValidationException("报工数量必须大于0".L10N());
            }
            if (task.ReportMode != ReportMode.Manual)
                throw new ValidationException("不允许报工，派工任务报工方式非手动方式".L10N());
            ReportTaskInfo info = GetReportTaskRecordInfo(dispatchTaskId);
            var record = GetReportRecord(dispatchTaskId);
            if (record != null)
            {
                info.OkQty = okQty;//record.OkQty;
                info.NgQty = record.NgQty;
                //已保存报工数据直接提交  
                TaskReport(info, true);
                return true;
            }
            ////B：按预设报工数量（合格品数），快速提交  
            //var config = GetReportRuleConfigByProduct(task.ProductId);
            //decimal reportQty = 0;
            //if (config != null)
            //{
            //    reportQty = config.IsModReport ?  : config.ReportQty;
            //}
            if (okQty > task.DispatchQty - task.ReportQty)
            {
                throw new ValidationException("报工数量必须小于等于剩余报工数".L10N());
            }
            info.ReportOkQty = okQty;
            info.OkQty = okQty;
            TaskReport(info, true);
            return true;
        }

        /// <summary>
        /// 任务报工
        /// </summary>
        /// <param name="taskInfo">报工任务信息</param>
        [ApiService("任务报工")]
        public virtual void TaskReport([ApiParameter("报工任务信息")] ReportTaskInfo taskInfo)
        {
            taskInfo.OkQty = taskInfo.ReportOkQty;
            taskInfo.NgQty = taskInfo.ReportNgQty;
            TaskReport(taskInfo, true);
        }

        /// <summary>
        /// 报工保存
        /// </summary>
        /// <param name="taskInfo">报工信息</param>           
        [ApiService("报工保存草稿")]
        public virtual void ReportSave([ApiParameter("报工任务信息")] ReportTaskInfo taskInfo)
        {
            taskInfo.OkQty = taskInfo.ReportOkQty;
            taskInfo.NgQty = taskInfo.ReportNgQty;
            TaskReport(taskInfo, false);
        }

        /// <summary>
        /// 获取快速报工数量
        /// </summary>
        /// <param name="taskId">派工任务ID</param>
        [ApiService("获取快速报工数量")]
        public virtual decimal GetQuickReportQty([ApiParameter("派工任务ID")] double taskId)
        {
            var task = RF.GetById<DispatchTask>(taskId);
            ReportRecord record = GetReportRecord(taskId);
            if (record != null)
                return record.OkQty;
            return GetTaskToReportQty(task);
        }

        /// <summary>
        /// 报工记录查询
        /// </summary>
        /// <param name="dispatchTaskId">派工任务ID</param>
        /// <returns>报工记录列表</returns>
        [ApiService("报工记录查询")]
        [return: ApiReturn("报工记录列表. 参数类型: List<ReportRecordInfo>")]
        public virtual List<ReportRecordInfo> QueryReportRecordInfo([ApiParameter("派工任务ID")] double dispatchTaskId)
        {
            List<ReportRecordInfo> infos = new List<ReportRecordInfo>();
            var records = GetReportRecords(dispatchTaskId, true);
            records.ForEach(record =>
            {
                infos.Add(new ReportRecordInfo()
                {
                    Principal = record.Principal.Name,
                    Hour = record.Hour,
                    NgQty = record.NgQty,
                    OkQty = record.OkQty,
                    Qty = record.ReportQty,
                    ReportDate = record.ReportTime.Value
                });
            });
            return infos;
        }

        /// <summary>
        /// 获取合并任务列表
        /// </summary>
        /// <param name="dispatchTaskId">派工任务ID</param>
        /// <returns>关联任务信息列表</returns>
        [ApiService("获取合并任务列表")]
        [return: ApiReturn("关联任务信息列表. 参数类型: List<AssociatedTaskInfo>")]
        public virtual List<AssociatedTaskInfo> GetAssociatedTaskInfos([ApiParameter("派工任务ID")] double dispatchTaskId)
        {
            List<AssociatedTaskInfo> infos = new List<AssociatedTaskInfo>();
            var associatedTasks = RT.Service.Resolve<ReportController>().GetIsSyntypeTasks(dispatchTaskId);
            foreach (var associatedTask in associatedTasks)
            {
                var task = RF.GetById<DispatchTask>(associatedTask.TaskId);
                if (task == null)
                    continue;
                infos.Add(new AssociatedTaskInfo()
                {
                    WorkOrder = task.WorkOrder.No,
                    TaskNo = task.No,
                    Qty = task.DispatchQty,
                    NgQty = task.NgQty,
                    OkQty = task.ReportQty - task.NgQty,
                    CreateDate = task.CreateDate.ToString("yyyy.MM.dd HH:mm")
                });
            }
            return infos;
        }


        /// <summary>
        /// 获取当前用户生产资源列表
        /// </summary>
        ///  <param name="key">生产资源编码或名称</param>
        /// <returns>获取生产资源列表</returns>
        [ApiService("获取生产资源列表")]
        [return: ApiReturn("关联生产资源列表. 参数类型: List<PdaResourceInfo>")]
        public virtual List<PdaResourceInfo> GetPdaResourceInfo(string key)
        {
            List<PdaResourceInfo> infos = new List<PdaResourceInfo>();
            var employeeResources = RT.Service.Resolve<EmployeeController>().GetEmployeeResourceByEmpId(RT.IdentityId, key);


            foreach (var employeeResource in employeeResources)
            {
                infos.Add(new PdaResourceInfo()
                {
                    ResourceId = employeeResource.ResourceId,
                    Code = employeeResource.ResourceCode,
                    Name = employeeResource.ResourceName
                });
            }
            return infos;
        }


        /// <summary>
        /// 根据生产资源获取可报工数据
        /// </summary>
        /// <param name="resourceId">生产资源Id</param>
        /// <param name="keyword">查询字符串</param>
        /// <param name="pageNumber">页数</param>
        /// <param name="pageSize">页数据数量</param>
        /// <returns>可报工列表</returns>
        [ApiService("获取可报工列表")]
        [return: ApiReturn("关联生产资源列表. 参数类型: PagingReportInfo")]
        public virtual PagingReportInfo GetPdaReportInfoByResourceId([ApiParameter("生产资源Id")] double resourceId, [ApiParameter("查询字符串")] string keyword, [ApiParameter("页数，为空查第一页")] int? pageNumber, [ApiParameter("页数据数量，为空查所有")] int? pageSize)
        {
            var resource = RF.GetById<WipResource>(resourceId);
            if (resource == null)
                throw new ValidationException("生产资源不正确，请检查！");
            var pagingInfo = new PagingInfo()
            {
                PageNumber = pageNumber.HasValue ? pageNumber.Value : 1,
                PageSize = pageSize.HasValue ? pageSize.Value : int.MaxValue - 1,
                IsNeedCount = true
            };

            var dispatchTasks = RT.Service.Resolve<DispatchController>().GetDispatchTaskByResourceId(resourceId, pagingInfo, keyword);
            PagingReportInfo pagingReportInfo = new PagingReportInfo()
            {
                PageNumber = pagingInfo.PageNumber,
                PageSize = pagingInfo.PageSize,
                TotalCount = dispatchTasks.TotalCount,
            };
            foreach (var task in dispatchTasks)
            {
                PdaReportInfo pdaReportInfo = new PdaReportInfo()
                {
                    DispatchTaskId = task.Id,
                    DispatchTaskNo = task.No,
                    ItemCode = task.ProductCode,
                    ItemName = task.ProductName,
                    ResourceId = resourceId,
                    ResourceCode = resource.Code,
                    ResourceName = resource.Name,
                    ProcessCode = task.ProcessCode,
                    ProcessName = task.ProcessName,
                    ReportQty = task.ReportQty,
                    State = task.TaskStatus.ToLabel(),
                    PlanBeginTime = task.PlanBeginTime,
                };
                pagingReportInfo.ReportInfos.Add(pdaReportInfo);
            }
            return pagingReportInfo;
        }

        /// <summary>
        /// 报工前验证是否直接完成任务单
        /// </summary>
        /// <param name="submitPdaReportInfo"></param>
        /// <returns></returns>
        [ApiService("报工前验证是否直接完成任务单")]
        public virtual string ReportValid(SubmitPdaReportInfo submitPdaReportInfo)
        {
            if (submitPdaReportInfo.DispatchTaskId == 0)
                throw new ValidationException("请先选择任务单");

            var dispatchTask = RF.GetById<DispatchTask>(submitPdaReportInfo.DispatchTaskId, new EagerLoadOptions().LoadWithViewProperty());
            if (dispatchTask == null)
                throw new ValidationException("找不到任务单");
            //var processPty = RT.Service.Resolve<ProcessPtyController>().GetProcessPtysByProcessIds(new List<double>() { dispatchTask.ProcessId.Value }, dispatchTask.ProductId).FirstOrDefault();
            var processPty = RT.Service.Resolve<ProcessPtyController>().GetProcessPtysByProcessId(dispatchTask.ProcessId.Value, dispatchTask.ProductId);
            var qty = dispatchTask.ReportQty + dispatchTask.SuspectQty + submitPdaReportInfo.SuspectQty + submitPdaReportInfo.GoodQty;

            var maxReportQty = RT.Service.Resolve<DispatchController>().MaxReportQty(dispatchTask);

            if (qty >= dispatchTask.DispatchQty && qty < maxReportQty/*dispatchTask.MaxReportQty*/)
            {
                if (processPty != null && processPty.IsReportValid == true)
                {
                    var tasks = RT.Service.Resolve<DispatchController>().GetDispatchTasks(dispatchTask.WorkOrderId.Value);
                    var dic = tasks.GroupBy(p => p.Seq).ToDictionary(p => p.Key, p => p.ToList());
                    decimal lastReportQty = 0;
                    decimal lastSuspectQty = 0;
                    if (dic != null && dic.Count > 0)
                    {
                        lastReportQty = dic.OrderBy(p => p.Key).FirstOrDefault().Value.Sum(p => p.ReportQty);
                        lastSuspectQty = dic.OrderBy(p => p.Key).FirstOrDefault().Value.Sum(p => p.SuspectQty);
                    }
                    return "首工序已报工数量{0}，可疑品数量{1}，当前任务数量{2},已报工数量{3}，可疑品数量{4}，是否将任务单已完成".L10nFormat(lastReportQty, lastSuspectQty, dispatchTask?.DispatchQty ?? 0, dispatchTask?.ReportQty ?? 0, dispatchTask?.SuspectQty ?? 0);
                }
            }
            return null;
        }

        /// <summary>
        /// 提交报工数量
        /// </summary>
        /// <param name="submitPdaReportInfo">生产资源Id</param>

        [ApiService("提交报工列表")]
        [return: ApiReturn("")]
        public virtual List<PdaPrintInfo> SubmitPdaReportInfo(SubmitPdaReportInfo submitPdaReportInfo)
        {
            if (submitPdaReportInfo.SourceType == null)
                submitPdaReportInfo.SourceType = Enums.SourceType.Report_Manual;

            var dispatchTask = ValidationSubmitReportData(submitPdaReportInfo);

            var dispatchConfig = ValidationConfigData(submitPdaReportInfo);
            //提交接口
            var wipDatas = SubmitReportInfo(submitPdaReportInfo, dispatchTask, dispatchConfig, 0, -1);

            var printDatas = CreatePrintDatas(true, PrintLabelType.Good, dispatchTask, dispatchConfig.GoodLabel, dispatchConfig.SuspectLabel, wipDatas);

            return printDatas;
        }

        /// <summary>
        /// 根据任务单ID获取标签打印信息
        /// </summary>
        /// <returns></returns>
        [ApiService("根据任务单ID获取标签打印信息")]
        public virtual List<PdaPrintInfo> GetPdaPrintInfoByTaskId(string workOrderNo)
        {
            var wo = Query<WorkOrder>().Where(p => p.No == workOrderNo).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            var wipDatas = Query<WipBatch>().Where(p => p.WorkOrder.No == workOrderNo).OrderByDescending(p => p.UpdateDate).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var printDatas = CreatePrintDatas(true, PrintLabelType.Good, wo, "", "", wipDatas);
            return printDatas;
        }

        /// <summary>
        /// 根据工单获取工单及标签信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [ApiService("根据工单获取工单及标签信息")]
        public virtual List<PdaWoAndLabelInfo> GetPdaWoAndLabelInfo(string key)
        {
            List<PdaWoAndLabelInfo> infos = new List<PdaWoAndLabelInfo>();
            var wos = Query<WorkOrder>().Where(p => p.No.Contains(key)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            foreach (var wo in wos)
            {
                PdaWoAndLabelInfo info = new PdaWoAndLabelInfo();
                info.WorkOrderNo = wo.No;
                info.ProductCode = wo.ProductCode;
                info.ProductName = wo.ProductName;
                info.PlanQty = wo.PlanQty;
                var sortInfo = new List<OrderInfo>();
                sortInfo.Add(new OrderInfo() { Property = "CreateDate", SortIndex = 0, SortOrder = ListSortDirection.Ascending });
                sortInfo.Add(new OrderInfo() { Property = "BatchNo", SortIndex = 0, SortOrder = ListSortDirection.Ascending });
                var wipDatas = Query<WipBatch>().Where(p => p.WorkOrderId == wo.Id).OrderBy(sortInfo).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                var printDatas = CreatePrintDatas(true, PrintLabelType.Good, wo, "", "", wipDatas);
                info.pdaPrintInfos = printDatas;

                infos.Add(info);
            }
            return infos;
        }

        /// <summary>
        /// 获取打印标签数据
        /// </summary>
        /// <param name="IsSuspectProduct">是否生成的数据,存在可疑品</param>
        /// <param name="labelType">打印类型</param>
        /// <param name="dispatchTask"></param>
        /// <param name="printTemplate"></param>
        /// <param name="wipBatchs"></param>
        /// <returns></returns>
        public virtual List<PdaPrintInfo> CreatePrintDatas(bool IsSuspectProduct, PrintLabelType labelType, WorkOrder workOrder, string printTemplate, string suspectTemplate, EntityList<WipBatch> wipBatchs)
        {
            List<PdaPrintInfo> printDatas = new List<PdaPrintInfo>();
            //var layoutInfos = workOrder.LayoutInfoList;//RT.Service.Resolve<SIE.MES.WorkOrders.WorkOrderController>().GetLayoutInfosByWorkOrderId(woIds);
            //string layoutCodes = string.Join("、", layoutInfos.OrderBy(p => p.Vornr).Select(p => p.ProcessCode));
            var processCodes = wipBatchs.Select(p => p.ProcessCode).Distinct().ToList();
            var processList = RT.Service.Resolve<ProcessController>().GetProcessesList(processCodes);
            int index = 1;
            foreach (var item in wipBatchs)
            {
                var labelPrintInfo = new PdaPrintInfo()
                {
                    BatchNo = item.BatchNo,
                    IsSuspectProduct = item.IsSuspectProduct == YesNo.Yes,
                    ProductCode = workOrder.ProductCode,
                    ProductName = workOrder.ProductName,
                    Qty = item.Qty,
                    DateTime = item.UpdateDate.ToString("yyyy/MM/dd HH:mm:ss"),
                    PrintTemplateId = processList.FirstOrDefault(p => p.Code == item.ProcessCode)?.PrintTemplateId,
                    SerialNumber = index.ToString()
                };
                index += 1;
                if (labelPrintInfo.PrintTemplateId == null || labelPrintInfo.PrintTemplateId == 0)
                {
                    //根据产品的物料类型，找到配置项
                    TypeConfigValue configValue = Query<TypeConfigValue>().Where(p => p.Type == workOrder.Product.Mtart).FirstOrDefault();
                    var config = ConfigService.GetConfig(new ItemTypeConfig(), typeof(DispatchTask), configValue);
                    if (config != null && config.ProcessPrintTemplateId != null)
                    {
                        labelPrintInfo.PrintTemplateId = config.ProcessPrintTemplateId;
                    }
                }

                if (IsSuspectProduct)
                {
                    labelType = item.IsSuspectProduct == YesNo.Yes ? PrintLabelType.Suspect : PrintLabelType.Good;
                }
                string template = printTemplate;
                if (labelType == PrintLabelType.Suspect)
                    template = suspectTemplate;
                //计算标签类型
                var P_Type = RT.Service.Resolve<WipBatchController>().GetWipBatchType(item);
                labelPrintInfo.Type = P_Type;

                printDatas.Add(labelPrintInfo);
            }
            return printDatas;
        }

        /// <summary>
        /// 报工提交接口
        /// </summary>
        /// <param name="submitPdaReportInfo"></param>
        /// <param name="dispatchTask"></param>
        /// <param name="dispatchConfig"></param>
        /// <param name="type">报工类型,0:手动报工,1:扫码报工</param>
        /// <returns></returns>
        private EntityList<WipBatch> SubmitReportInfo(SubmitPdaReportInfo submitPdaReportInfo, DispatchTask dispatchTask, DispatchConfigValue dispatchConfig, int type, double wipId)
        {
            EntityList<WipBatch> goodWipBatchs = new EntityList<WipBatch>();    //良品标签
            EntityList<WipBatch> printWipBatchs = new EntityList<WipBatch>();   //返回需要打印的标签
            EntityList<SuspectProductLabel> suspectLabels = new EntityList<SuspectProductLabel>();    //可疑品标签
            ReportTaskInfo info = GetReportTaskRecordInfo(submitPdaReportInfo.DispatchTaskId);

            info.IsAutoFeeding = submitPdaReportInfo.IsAutoFeeding;
            info.ReportEmployeeId = submitPdaReportInfo.ReportEmployeeId;
            info.IsSkipValidatePreQty = submitPdaReportInfo.IsSkipValidatePreQty;
            info.IsValidatePrepare = submitPdaReportInfo.IsValidatePrepare;
            info.IsTaskFinish = submitPdaReportInfo.IsTaskFinish;
            info.IsCommonMode = submitPdaReportInfo.IsCommonMode;

            info.SourceType = submitPdaReportInfo.SourceType;
            if (info.SourceType == null)
            {
                info.SourceType = type == 0 ? Enums.SourceType.Report_Manual : Enums.SourceType.Report_Scan;
            }

            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                //处理批次标签
                SubmitReportWipBatchs(submitPdaReportInfo, dispatchTask, dispatchConfig, goodWipBatchs, printWipBatchs, suspectLabels);

                //校验是否报工
                var batchNos = goodWipBatchs.Select(p => p.BatchNo).ToList();
                ValidateProcessHasReport(batchNos, dispatchTask.ProcessCode);

                //校验标签工单与派工任务工单是否一致
                if (goodWipBatchs.Any(p => p.WorkOrderId != info.WorkOrderId))
                    throw new ValidationException("标签对应的工单与提交的派工任务工单不一致,请检查");

                //调用报工接口
                //合格数和不合格数都为0的时候，就不给报工，否则没有合格也没有不合格，报个什么工

                info.OkQty = submitPdaReportInfo.GoodQty;
                info.NgQty = submitPdaReportInfo.ScrapQty;
                info.ReworkQty = submitPdaReportInfo.ReworkQty;
                info.SuspectQty = 0;
                info.IsSuspect = false;
                if (info.OkQty > 0 || info.NgQty > 0 || info.ReworkQty > 0)
                {
                    var record = TaskReport(info, true);
                    //良品标签关联报工记录
                    CreateReportWipBatchs(goodWipBatchs, record);

                    //自动上料扣料(适用于成品工单包装报工)
                    if (info.IsAutoFeeding)
                        AutoFeedingDeductionItems(record, dispatchTask, goodWipBatchs);
                }
                //可疑品需要在报工时也进行扣料 2025.11.26
                if (suspectLabels.Count > 0)
                {
                    info.OkQty = 0;
                    info.NgQty = 0;
                    info.ReworkQty = 0;
                    info.SuspectQty = submitPdaReportInfo.SuspectQty;
                    info.BatchNo = GetReportBatchNo();
                    info.IsSuspect = true;
                    var record = TaskReport(info, true);
                    suspectLabels.ForEach(label => { label.ReportRecordId = record.Id; });
                    RF.Save(suspectLabels);
                }

                //末工序,良品与返工批次,生成物料标签
                if (dispatchTask.EndProcess == true)
                {
                    GenerateItemLabels(info.TaskId, goodWipBatchs);
                }

                //可疑品时也需要更新任务单状态
                if (submitPdaReportInfo.SuspectQty > 0)
                {
                    ValidateReportSingleTask(dispatchTask);
                    //更新任务单可疑品数
                    UpdateSuspectQty(submitPdaReportInfo.DispatchTaskId);
                    DateTime dbTime = RF.Find<ReportRecord>().GetDbTime();
                    UpdateDispatchTaskState(info.TaskId, dbTime, submitPdaReportInfo.IsTaskFinish);
                }

                //记录非IOT报工数量
                if (submitPdaReportInfo.SourceType != Enums.SourceType.Report_IOT)
                {
                    var qty = submitPdaReportInfo.GoodQty + submitPdaReportInfo.ScrapQty + submitPdaReportInfo.ReworkQty + submitPdaReportInfo.SuspectQty;
                    UpdateDispatchTaskManualReportQty(dispatchTask.Id, qty);
                }

                tran.Complete();

                //可疑品安灯事件 (尽量不要影响报工业务)
                if (submitPdaReportInfo.SuspectQty > 0)
                    CreateAndonManages(dispatchTask, submitPdaReportInfo, wipId);

                //委外任务单报工要回传
                if (dispatchTask.IsOutsourcing == true)
                {
                    var invOrg = RT.Service.Resolve<InvOrgController>().GetByCode(RT.InvOrg.Value);
                    //操作委外需求单(包含接口)
                    UpdateOutsourcingRequest(dispatchTask, invOrg, goodWipBatchs);
                }
            }
            return printWipBatchs;
        }

        /// <summary>
        /// 操作委外需求单
        /// </summary>
        public virtual void UpdateOutsourcingRequest(DispatchTask dispatchTask, InvOrg invOrg, EntityList<WipBatch> goodWipBatchs)
        {
            //根据工单+工序+委外工厂获取委外需求单
            OutsourcingRequest request = RT.Service.Resolve<OutsourcingRequestController>().GetOutsourcingRequestsByWoIdProcessCode(dispatchTask.WorkOrderId.Value, dispatchTask.Process.Code).FirstOrDefault(p => p.OutFactory == invOrg.ExternalId);
            if (request != null)
            {
                var sns = goodWipBatchs.Select(p => p.BatchNo).ToList();
                //var list = request.OutsourcingReportLogList.Where(p => sns.Contains(p.SN)).ToList();
                //if(list.Count>0)
                //    throw new ValidationException("委外单【{0}】,批次【{1}】已经报工，不能重复报工！".L10nFormat(request.NO, string.Join(',',list.Select(p => p.SN).ToList())));
                EntityList<OutsourcingReportLog> logs = new EntityList<OutsourcingReportLog>();
                //良品标签创建报工记录
                foreach (var goodWipBatch in goodWipBatchs)
                {
                    OutsourcingReportLog log = new OutsourcingReportLog();
                    log.OutsourcingRequestId = request.Id;
                    log.SN = goodWipBatch.BatchNo;
                    log.LotNo = goodWipBatch.BatchNo;
                    log.Qty = goodWipBatch.Qty;
                    log.PassQty = 0;
                    log.NgQty = 0;
                    log.State = OutsourcingDetailState.Submitted;
                    log.PersistenceStatus = PersistenceStatus.New;
                    log.ProcessingType = ProcessingType.Good;
                    log.ReportFactory = invOrg.ExternalId;
                    RF.Save(log);
                    logs.Add(log);
                }

                var reportState = ReportState.PartReport;
                if (request.OutsourcingReportLogList.Sum(p => p.Qty) + logs.Sum(p => p.Qty) >= request.RequestQty)
                {
                    reportState = ReportState.Finish;
                }
                EntityList<ProcessingInStock> processingInStocks = new EntityList<ProcessingInStock>();
                EntityList<ProcessingOutbound> processingOutbounds = new EntityList<ProcessingOutbound>();
                //根据创建的记录，判断是否需要拆分发料明细和收货明细
                foreach (var p in logs)
                {
                    //可能会拆标签,发料明细数据要重新校对
                    var outbound = request.ProcessingOutsourcingOutboundList.FirstOrDefault(item => item.SN == p.SN);
                    if (outbound != null)
                    {
                        outbound.Qty = p.Qty;
                    }
                    else
                    {
                        outbound = new ProcessingOutbound();
                        outbound.GenerateId();
                        outbound.SourceId = 0;
                        outbound.PersistenceStatus = PersistenceStatus.New;
                        outbound.Qty = p.Qty;
                        outbound.SN = p.SN;
                        outbound.LotNo = p.LotNo;
                        outbound.State = OutsourcingDetailState.Submitted;
                        outbound.OutsourcingRequestId = request.Id;
                    }
                    processingOutbounds.Add(outbound);
                    //可能会拆标签,收货明细的数据要重新校对
                    //var inStock = request.ProcessingOutsourcingInStockList.FirstOrDefault(item => item.SN == p.SN);
                    //if (inStock != null)
                    //{
                    //    inStock.Qty = p.Qty;
                    //    inStock.ProcessingType = p.ProcessingType;
                    //}
                    //else
                    //{
                    //    inStock = new ProcessingInStock();
                    //    inStock.GenerateId();
                    //    inStock.Qty = p.Qty;
                    //    inStock.ProcessingType = p.ProcessingType;
                    //    inStock.SN = p.SN;
                    //    inStock.LotNo = p.LotNo;
                    //    inStock.State = OutsourcingDetailState.Submitted;
                    //    inStock.OutsourcingRequestId = request.Id;
                    //}
                    //processingInStocks.Add(inStock);
                }

                using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
                {
                    if (processingInStocks.Count > 0)
                        RF.Save(processingInStocks);
                    if (processingOutbounds.Count > 0)
                        RF.Save(processingOutbounds);
                    DB.Update<OutsourcingRequest>().Set(p => p.ReportState, reportState).Where(p => p.Id == request.Id).Execute();
                    request.ReportState = reportState;
                    tran.Complete();
                }
                /********************切记这里的request需求单是为了做为接口参数，不要保存*************************/

                var req = new OutsourcingRequest();
                req.Clone(request, new CloneOptions(CloneActions.NormalProperties));
                //调用接口回传报工记录
                req.OutsourcingReportLogList.AddRange(logs);
                if (processingInStocks.Count > 0)
                {
                    req.ProcessingOutsourcingInStockList.AddRange(processingInStocks);
                }
                if (processingOutbounds.Count > 0)
                {
                    req.ProcessingOutsourcingOutboundList.AddRange(processingOutbounds);
                }
                //调用接口
                RT.Service.Resolve<OutsourcingApiController>().SyncOutsourcingRequestToOtherFactory(req, 3, req.InitiatorFactory);
            }
        }

        /// <summary>
        /// 提交可疑品标签
        /// </summary>
        /// <param name="info"></param>
        /// <param name="IsSn">是否耐压SN标签</param>
        public virtual SuspectProductLabel SubmitSuspectReportInfo(SubmitPdaReportInfo info, bool IsSn)
        {
            var dispatchTask = RF.GetById<DispatchTask>(info.DispatchTaskId, new EagerLoadOptions().LoadWithViewProperty());
            if (dispatchTask == null)
                throw new ValidationException("任务单不存在".L10N());
            if (info.SnInfos.Count == 0)
                throw new ValidationException("标签数据不能为空".L10N());
            var snInfo = info.SnInfos.FirstOrDefault();
            SuspectProductLabel suspectLabel;
            if (IsSn)
            {
                //suspectLabel = NewSuspectProductLabel(snInfo.Sn, snInfo.SuspectQty, dispatchTask, info.ResourceId, LabelType.WipSn);
                //RF.Save(suspectLabel);

                //RT.Service.Resolve<WipPressureController>().SetSnSuspectState(snInfo.Sn, true);

                //return suspectLabel;
                //耐压SN可疑品提交报工
                suspectLabel = SuspectProductLabelReport(snInfo, dispatchTask, info.ResourceId);
                return suspectLabel;
            }

            var wipBatch = RT.Service.Resolve<WipBatchController>().GetWipBatch(snInfo.Sn);
            if (dispatchTask == null)
                throw new ValidationException("工序标签[{0}]不存在".L10nFormat(snInfo.Sn));

            if (snInfo.SuspectQty < wipBatch.Qty)
            {
                var splitWipBatch = RT.Service.Resolve<WipBatchController>().CreateSplitWipBatch(wipBatch, snInfo.SuspectQty, dispatchTask.ProcessCode);
                splitWipBatch.GenerateProcessCode = dispatchTask.ProcessCode;
                splitWipBatch.IsSuspectProduct = YesNo.Yes;
                RF.Save(splitWipBatch);
                //如果修改数量就要记录在哪个工序进行修改的
                if (snInfo.SuspectQty > 0)
                {
                    wipBatch.EditQtyProcessCode = dispatchTask.ProcessCode;
                }
                //更新原标签数量
                wipBatch.Qty -= snInfo.SuspectQty;
                RF.Save(wipBatch);

                suspectLabel = NewSuspectProductLabel(splitWipBatch.BatchNo, snInfo.SuspectQty, dispatchTask, info.ResourceId, LabelType.WipBatch, snInfo.Sn);
                RF.Save(suspectLabel);
            }
            else
            {
                wipBatch.IsSuspectProduct = YesNo.Yes;
                RF.Save(wipBatch);

                suspectLabel = NewSuspectProductLabel(wipBatch.BatchNo, snInfo.SuspectQty, dispatchTask, info.ResourceId, LabelType.WipBatch, snInfo.Sn);
                RF.Save(suspectLabel);
            }

            ////更新任务单可疑品数
            //UpdateSuspectQty(info.DispatchTaskId);
            ////更新任务单状态
            //DateTime dbTime = RF.Find<ReportRecord>().GetDbTime();
            //UpdateDispatchTaskState(info.DispatchTaskId, dbTime, info.IsTaskFinish);

            return suspectLabel;
        }

        /// <summary>
        /// 耐压SN可疑品提交报工
        /// </summary>
        /// <param name="snInfo"></param>
        /// <param name="dispatchTask"></param>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        private SuspectProductLabel SuspectProductLabelReport(ReportSnInfo snInfo, DispatchTask dispatchTask, double resourceId)
        {
            SuspectProductLabel suspectLabel;
            var wipPressureSn = RT.Service.Resolve<WipPressureController>().GetWipPressureSn(snInfo.Sn);

            if (wipPressureSn.WipPressure.BatchNo.IsNullOrEmpty())
                throw new ValidationException("未找到耐压SN标签【{0}】对应的工序标签".L10nFormat(snInfo.Sn));
            var wipBatch = RT.Service.Resolve<WipBatchController>().GetWipBatch(wipPressureSn.WipPressure.BatchNo);
            if (dispatchTask == null)
                throw new ValidationException("工序标签[{0}]不存在".L10nFormat(snInfo.Sn));

            if (snInfo.SuspectQty < wipBatch.Qty)
            {
                var splitWipBatch = RT.Service.Resolve<WipBatchController>().CreateSplitWipBatch(wipBatch, snInfo.SuspectQty, dispatchTask.ProcessCode);
                splitWipBatch.GenerateProcessCode = dispatchTask.ProcessCode;
                splitWipBatch.IsSuspectProduct = YesNo.Yes;
                RF.Save(splitWipBatch);
                //如果修改数量就要记录在哪个工序进行修改的
                if (snInfo.SuspectQty > 0)
                {
                    wipBatch.EditQtyProcessCode = dispatchTask.ProcessCode;
                }
                //更新原标签数量
                wipBatch.Qty -= snInfo.SuspectQty;
                RF.Save(wipBatch);

                suspectLabel = NewSuspectProductLabel(snInfo.Sn, snInfo.SuspectQty, dispatchTask, resourceId, LabelType.WipSn, splitWipBatch.BatchNo);
                RF.Save(suspectLabel);
            }
            else
            {
                wipBatch.IsSuspectProduct = YesNo.Yes;
                RF.Save(wipBatch);

                suspectLabel = NewSuspectProductLabel(snInfo.Sn, snInfo.SuspectQty, dispatchTask, resourceId, LabelType.WipSn, wipBatch.BatchNo);
                RF.Save(suspectLabel);
            }
            wipPressureSn.IsSuspectProduct = true;
            RF.Save(wipPressureSn);
            return suspectLabel;
        }

        /// <summary>
        /// 处理批次标签
        /// </summary>
        /// <param name="submitPdaReportInfo"></param>
        /// <param name="dispatchTask"></param>
        /// <param name="dispatchConfig"></param>
        /// <param name="goodWipBatchs"></param>
        /// <param name="retWipBatchs"></param>
        /// <param name="suspectLabels"></param>
        void SubmitReportWipBatchs(SubmitPdaReportInfo submitPdaReportInfo, DispatchTask dispatchTask, DispatchConfigValue dispatchConfig, EntityList<WipBatch> goodWipBatchs, EntityList<WipBatch> retWipBatchs, EntityList<SuspectProductLabel> suspectLabels)
        {
            if (submitPdaReportInfo.SnInfos.Count > 0)
            {
                //扫码报工,标签拆分处理
                foreach (var snInfo in submitPdaReportInfo.SnInfos)
                {
                    var batchNo = snInfo.Sn;
                    var batch = RT.Service.Resolve<WipBatchController>().GetWipBatch(batchNo);

                    if (submitPdaReportInfo.IsAutoFeeding)
                    {
                        if (batch.Qty != snInfo.GoodQty)
                        {
                            batch.EditQtyProcessCode = dispatchTask.ProcessCode;
                        }
                        //包装报工自动上料扣料场景,不做标签拆分
                        batch.Qty = snInfo.GoodQty;
                        RF.Save(batch);
                        goodWipBatchs.Add(batch);
                        retWipBatchs.Add(batch);
                    }
                    else
                    {
                        CreateSplitWipBatch(batch, snInfo, dispatchTask, goodWipBatchs, retWipBatchs);
                    }

                    //创建批次标签(可疑品)
                    if (snInfo.SuspectQty > 0)
                    {
                        EntityList<WipBatch> wipBatchs = new EntityList<WipBatch>();

                        var tuple = CreateWipBatchs(dispatchTask, dispatchConfig, YesNo.Yes, snInfo.SuspectQty);
                        tuple.Item1.ForEach(p => { p.ReportRecordIds = batch?.ReportRecordIds; p.SourceNo = snInfo.Sn; });
                        RF.Save(tuple.Item1);
                        wipBatchs.AddRange(tuple.Item1);
                        retWipBatchs.AddRange(tuple.Item1);

                        var suspectProductLabels = CreateSuspectProductLabel(dispatchTask, dispatchConfig, snInfo.SuspectQty, submitPdaReportInfo.ResourceId, wipBatchs);
                        RF.Save(suspectProductLabels);
                        suspectLabels.AddRange(suspectProductLabels);

                    }
                }
            }
            else
            {
                //bool isGrenOne = false;
                //手动报工(首工序)才创建批次标签(良品)
                if (submitPdaReportInfo.GoodQty > 0)
                {
                    var tuple = CreateWipBatchs(dispatchTask, dispatchConfig, YesNo.No, submitPdaReportInfo.GoodQty);
                    RF.Save(tuple.Item1);
                    goodWipBatchs.AddRange(tuple.Item1);
                    retWipBatchs.AddRange(tuple.Item1);
                }
                //创建批次标签(可疑品)
                if (submitPdaReportInfo.SuspectQty > 0)
                {
                    EntityList<WipBatch> wipBatchs = new EntityList<WipBatch>();

                    var tuple = CreateWipBatchs(dispatchTask, dispatchConfig, YesNo.Yes, submitPdaReportInfo.SuspectQty);
                    //tuple.Item1.ForEach(p => p.ReportRecordIds = batch?.ReportRecordIds);
                    RF.Save(tuple.Item1);
                    wipBatchs.AddRange(tuple.Item1);
                    retWipBatchs.AddRange(tuple.Item1);

                    var suspectProductLabels = CreateSuspectProductLabel(dispatchTask, dispatchConfig, submitPdaReportInfo.SuspectQty, submitPdaReportInfo.ResourceId, wipBatchs);
                    RF.Save(suspectProductLabels);
                    suspectLabels.AddRange(suspectProductLabels);

                }
            }
        }

        /// <summary>
        /// 扫码报工,标签拆分处理
        /// </summary>
        /// <param name="wipBatch"></param>
        /// <param name="snInfo"></param>
        /// <param name="dispatchTask"></param>
        /// <param name="goodWipBatchs"></param>
        /// <param name="retWipBatchs"></param>
        void CreateSplitWipBatch(WipBatch wipBatch, ReportSnInfo snInfo, DispatchTask dispatchTask, EntityList<WipBatch> goodWipBatchs, EntityList<WipBatch> retWipBatchs)
        {
            //扫码报工,沿用旧标签
            //var wipBatch = RT.Service.Resolve<WipBatchController>().GetWipBatch(batchNo);
            var batchQty = wipBatch.Qty;
            var submitQty = snInfo.GoodQty + snInfo.SuspectQty;
            if (submitQty == 0)
                return;
            //校验分单数量
            var layoutInfo = dispatchTask.WorkOrder.LayoutInfoList.Where(p => p.ProcessCode == dispatchTask.ProcessCode).FirstOrDefault();
            decimal zcode = layoutInfo?.Zcode ?? batchQty;
            if (dispatchTask.IsReportByZcode == true)
            {
                zcode = layoutInfo?.Zcode ?? 0;
                if (zcode > 0 && snInfo.GoodQty > 0 && batchQty > zcode && snInfo.GoodQty != zcode)
                    throw new ValidationException("任务单工序[{0}]配置了只允许按分单数量报工,报工良品数量不允许超过分单数量[{1}]".L10nFormat(dispatchTask.ProcessCode, zcode));

                zcode = (layoutInfo == null || layoutInfo.Zcode == 0) ? batchQty : layoutInfo.Zcode;
            }

            if (zcode == 0)
                zcode = submitQty;

            if (batchQty > submitQty)
            {
                //提交数据小于标签数量, 需要进行拆标签报工
                if (snInfo.GoodQty > 0)
                {
                    if (snInfo.GoodQty > zcode)
                    {
                        var qty = snInfo.GoodQty;
                        //良品数>分单数,需要拆分标签
                        while (qty > 0)
                        {
                            var splitQty = qty > zcode ? zcode : qty;
                            var splitWipBatch = RT.Service.Resolve<WipBatchController>().CreateSplitWipBatch(wipBatch, splitQty, dispatchTask.ProcessCode);
                            splitWipBatch.GenerateProcessCode = dispatchTask.ProcessCode;
                            RF.Save(splitWipBatch);
                            goodWipBatchs.Add(splitWipBatch);
                            retWipBatchs.Add(splitWipBatch);
                            qty -= splitQty;
                        }
                    }
                    else
                    {
                        var splitWipBatch = RT.Service.Resolve<WipBatchController>().CreateSplitWipBatch(wipBatch, snInfo.GoodQty, dispatchTask.ProcessCode);
                        splitWipBatch.GenerateProcessCode = dispatchTask.ProcessCode;
                        RF.Save(splitWipBatch);
                        goodWipBatchs.Add(splitWipBatch);
                        retWipBatchs.Add(splitWipBatch);
                    }
                }

                //更新原标签数量
                var wipBatchQty = batchQty - submitQty;
                if (wipBatch.Qty != wipBatchQty)
                {
                    wipBatch.EditQtyProcessCode = dispatchTask.ProcessCode;
                }
                wipBatch.Qty = wipBatchQty;
                RF.Save(wipBatch);
            }
            else
            {
                //良品数>分单数,需要拆分标签
                if (snInfo.GoodQty > zcode)
                {
                    var qty = snInfo.GoodQty;
                    while (qty > zcode)
                    {
                        var splitWipBatch = RT.Service.Resolve<WipBatchController>().CreateSplitWipBatch(wipBatch, zcode, dispatchTask.ProcessCode);
                        splitWipBatch.GenerateProcessCode = dispatchTask.ProcessCode;
                        RF.Save(splitWipBatch);
                        goodWipBatchs.Add(splitWipBatch);
                        retWipBatchs.Add(splitWipBatch);
                        qty -= zcode;
                    }
                    //更新原标签数量
                    if (wipBatch.Qty != qty)
                    {
                        wipBatch.EditQtyProcessCode = dispatchTask.ProcessCode;
                    }
                    wipBatch.Qty = qty;
                }
                else
                {
                    if (wipBatch.Qty != snInfo.GoodQty)
                    {
                        wipBatch.EditQtyProcessCode = dispatchTask.ProcessCode;
                    }
                    wipBatch.Qty = snInfo.GoodQty;
                }
                RF.Save(wipBatch);
                goodWipBatchs.Add(wipBatch);
            }
            //用户提出需求, 原标签也需要重新打印. 2025 09.02
            retWipBatchs.Add(wipBatch);
        }

        /// <summary>
        /// 标签关联报工记录
        /// </summary>
        /// <param name="goodWipBatchs"></param>
        /// <param name="record"></param>
        void CreateReportWipBatchs(EntityList<WipBatch> goodWipBatchs, ReportRecord record)
        {

            //良品标签关联报工记录
            foreach (var w in goodWipBatchs)
            {
                var reportWipBatch = new ReportWipBatch()
                {
                    ReportRecord = record,
                    WipBatch = w,
                    BatchNo = w.BatchNo,
                    SourceNo = w.SourceNo,
                    Qty = w.Qty,
                };
                RF.Save(reportWipBatch);

                //关联报工记录ID
                var ids = w.ReportRecordIds.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();
                ids.Add(record.Id.ToString());
                w.ReportRecordIds = ids.Distinct().Concat(",");

                DB.Update<WipBatch>().Set(p => p.ReportRecordIds, w.ReportRecordIds).Where(p => p.Id == w.Id).Execute();
            }
        }

        /// <summary>
        /// 验证任务单数据
        /// </summary>
        /// <param name="submitPdaReportInfo"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        private DispatchTask ValidationSubmitReportData(SubmitPdaReportInfo submitPdaReportInfo)
        {
            if (submitPdaReportInfo == null)
                throw new ValidationException("参数不能为空.".L10N());
            var dispatchTask = RF.GetById<DispatchTask>(submitPdaReportInfo.DispatchTaskId, new EagerLoadOptions().LoadWithViewProperty());
            if (dispatchTask == null)
                throw new ValidationException("任务单的资源不能为空，手动报工失败".L10N());
            if (!dispatchTask.ResourceId.HasValue)
                throw new ValidationException("任务单生产资源不存在.".L10N());
            if (dispatchTask.TaskStatus != DispatchTaskStatus.Dispatched && dispatchTask.TaskStatus != DispatchTaskStatus.Executing)
                throw new ValidationException("任务单[{0}]状态[{1}]不正确.".L10nFormat(dispatchTask.No, dispatchTask.TaskStatus.ToLabel()));
            //校验可疑品和良品数
            if (submitPdaReportInfo.GoodQty < 0)
                throw new ValidationException("良品数小于0不允许报工".L10N());
            if (submitPdaReportInfo.SuspectQty < 0)
                throw new ValidationException("可疑品数小于0不允许报工".L10N());

            decimal reportQty = submitPdaReportInfo.GoodQty + submitPdaReportInfo.SuspectQty;
            if (reportQty <= 0)
                throw new ValidationException("报工数量小于0不允许报工".L10N());

            var processPty = RT.Service.Resolve<ProcessPtyController>().GetProcessPtysByProcessId(dispatchTask.ProcessId ?? 0, dispatchTask.ProductId);
            if (processPty?.IsReportValid == null || processPty?.IsReportValid == false)
            {
                var MaxRemainQty = RT.Service.Resolve<DispatchController>().MaxReportQtyAndMaxRemainQty(dispatchTask).Item2;

                if (reportQty > MaxRemainQty/*dispatchTask.MaxRemainQty*/)
                {
                    throw new ValidationException("良品报工数+可疑品报工数超过剩余可报工数[{0}]，不允许报工".L10nFormat(MaxRemainQty/*dispatchTask.MaxRemainQty*/));
                }
            }
            else
            {
                var qty = dispatchTask.ReportQty + dispatchTask.SuspectQty + submitPdaReportInfo.SuspectQty + submitPdaReportInfo.GoodQty;

                var maxReportQty = RT.Service.Resolve<DispatchController>().MaxReportQty(dispatchTask);

                if (qty > maxReportQty/*dispatchTask.MaxReportQty*/)
                {
                    throw new ValidationException("已报工数{0}已达到容差数量{1}".L10nFormat(qty, maxReportQty/*dispatchTask.MaxReportQty*/));
                }
            }
            //if (dispatchTask.WorkOrder.PlanQty - (dispatchTask.ReportQty + dispatchTask.NgQty) < reportQty)
            //    throw new ValidationException("当前报工数不能超过剩余可报工数".L10N());
            var processBoms = dispatchTask.WorkOrder.ProcessBomList.Where(p => p.ProcessId == dispatchTask.ProcessId).AsEntityList();
            var recordList = RT.Service.Resolve<FeedingRecordController>().GetFeedingRecordsByResourceId(dispatchTask.ResourceId.Value, new EagerLoadOptions().LoadWithViewProperty(), true);
            StringBuilder errMsg = new StringBuilder();
            foreach (var bom in processBoms)
            {
                decimal remainingQty = recordList.Where(p => p.ItemId == bom.ItemId && p.RemainingQty.HasValue).Sum(p => p.RemainingQty.Value);
                decimal totalQty = reportQty * (bom.Weight ?? 0);//bom.SingleQty;
                if (totalQty > remainingQty)
                    errMsg.AppendLine("物料{0}-{3}上料数量{1}小于报工数{2}，请先上料后报工".L10nFormat(bom.Item?.Code, remainingQty, totalQty, bom.Item?.Name));
            }
            if (errMsg.ToString().IsNotEmpty())
            {
                throw new ValidationException(errMsg.ToString());
            }

            //var firstProcess = false;
            //var config = RT.Service.Resolve<DispatchController>().GetDispatchConfig();
            //if (config != null && config.IsFirstProcess == true)
            //{
            //    firstProcess = true;
            //}
            ////非手动报工，或手动报工未勾选配置项，就校验当前是否首工序
            //if (submitPdaReportInfo.IsReportManual == false || (submitPdaReportInfo.IsReportManual == true && firstProcess == false))
            //{
            //    //var processId = dispatchTask.WorkOrder.RoutingProcessList.OrderBy(p => p.Index).FirstOrDefault()?.ProcessId;
            //    //if (dispatchTask.ProcessId != processId)
            //    if (dispatchTask.StartProcess == null || dispatchTask.StartProcess == false)
            //        throw new ValidationException("当前工序不是首工序");
            //}
            //else
            //{
            //    //当手动报工，且勾选配置项的时候，就判断是否当前工单已生成的第一个工序的任务单
            //    var firstTask = Query<DispatchTask>().Where(p => p.WorkOrderId == dispatchTask.WorkOrderId).OrderBy(p => p.Seq).FirstOrDefault();
            //    if (firstTask == null || firstTask.Seq != dispatchTask.Seq)
            //    {
            //        throw new ValidationException("当前工序，不是当前工单已生成的第一个工序的任务单");
            //    }
            //}

            return dispatchTask;
        }

        /// <summary>
        /// 验证配置项
        /// </summary>
        /// <param name="submitPdaReportInfo"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        private DispatchConfigValue ValidationConfigData(SubmitPdaReportInfo submitPdaReportInfo)
        {
            var invOrg = RT.Service.Resolve<InvOrgController>().GetByCode(RT.InvOrg.Value);
            var dispatchConfig = RT.Service.Resolve<DispatchController>().GetDispatchConfig();
            if (dispatchConfig == null)
                throw new ValidationException("派工管理配置项不存在.".L10N());
            //if (!dispatchConfig.NumberRuleId.HasValue)
            //    throw new ValidationException("派工管理编码规则配置项未配置.".L10N());
            //2810用的是绕包线编码规则以及非绕包线编码规则
            if (invOrg.ExternalId != "2810")
            {

                if (!dispatchConfig.NumberRuleId.HasValue)
                    throw new ValidationException("派工管理编码规则配置项未配置.".L10N());
                //if (submitPdaReportInfo.GoodQty > 0 && dispatchConfig.GoodLabel.IsNullOrEmpty())
                //    throw new ValidationException("派工管理良品标签配置项未配置.".L10N());
                //if (submitPdaReportInfo.SuspectQty > 0 && dispatchConfig.SuspectLabel.IsNullOrEmpty())
                //    throw new ValidationException("派工管理可疑品标签配置项未配置.".L10N());
            }
            else
            {
                if (!dispatchConfig.EntangleNumberRuleId.HasValue)
                    throw new ValidationException("派工管理绕包线编码规则配置项未配置.".L10N());
                if (!dispatchConfig.UnEntangleNumberRuleId.HasValue)
                    throw new ValidationException("派工管理非绕包线编码规则配置项未配置.".L10N());
            }


            return dispatchConfig;
        }

        /// <summary>
        /// 生成可疑标签
        /// </summary>
        /// <param name="dispatchTask"></param>
        /// <param name="dispatchConfig"></param>
        /// <param name="suspectQty"></param>
        /// <param name="resourceId"></param>
        /// <param name="wipBatchs"></param>
        /// <param name="sourceNo">来源标签</param>
        /// <returns></returns>
        private EntityList<SuspectProductLabel> CreateSuspectProductLabel(DispatchTask dispatchTask, DispatchConfigValue dispatchConfig, decimal suspectQty, double resourceId, EntityList<WipBatch> wipBatchs)
        {
            EntityList<SuspectProductLabel> suspectProductLabels = new EntityList<SuspectProductLabel>();
            if (wipBatchs.Count() == 0)
            {
                var invOrg = RT.Service.Resolve<InvOrgController>().GetByCode(RT.InvOrg.Value);

                string batchNo = "";
                double numberRuleId = 0;
                //如果工序配置了编码规则,优先使用该规则
                if (dispatchTask.Process?.NumberRuleId > 0)
                    numberRuleId = dispatchTask.Process.NumberRuleId.Value;

                if (numberRuleId == 0)
                {
                    //根据产品的物料类型，找到配置项
                    TypeConfigValue configValue = Query<TypeConfigValue>().Where(p => p.Type == dispatchTask.Product.Mtart).FirstOrDefault();
                    var config = ConfigService.GetConfig(new ItemTypeConfig(), typeof(DispatchTask), configValue);
                    if (config != null && config.ProcessNumberRuleId != null)
                    {
                        numberRuleId = config.ProcessNumberRuleId.Value;
                    }
                }

                if (numberRuleId == 0)
                {
                    //2810用的是其他绕包和非绕包，以后上新系统需要改
                    if (invOrg.ExternalId != "2810")
                    {
                        numberRuleId = dispatchConfig.NumberRuleId.GetValueOrDefault();
                    }
                    else
                    {
                        if (dispatchTask.Product.Zmc.Contains("绕包"))
                        {
                            numberRuleId = dispatchConfig.EntangleNumberRuleId.GetValueOrDefault();
                        }
                        else
                        {
                            numberRuleId = dispatchConfig.UnEntangleNumberRuleId.GetValueOrDefault();
                        }
                    }
                }

                batchNo = RT.Service.Resolve<NumberRuleController>().GenerateSegment(numberRuleId, 1, dispatchTask).FirstOrDefault();

                //手动报工、扫码报工创建可疑品标签时的良品数量默认为0 
                SuspectProductLabel suspectProductLabel = NewSuspectProductLabel(batchNo, suspectQty, dispatchTask, resourceId, LabelType.WipBatch);

                suspectProductLabels.Add(suspectProductLabel);
            }
            else
            {
                foreach (var item in wipBatchs)
                {

                    SuspectProductLabel suspectProductLabel = NewSuspectProductLabel(item.BatchNo, item.Qty, dispatchTask, resourceId, LabelType.WipBatch, item.SourceNo);

                    suspectProductLabels.Add(suspectProductLabel);
                }
            }
            return suspectProductLabels;
        }

        /// <summary>
        /// 创建可疑品标签实体
        /// </summary>
        /// <param name="batchNo"></param>
        /// <param name="qty"></param>
        /// <param name="dispatchTask"></param>
        /// <param name="resourceId"></param>
        /// <param name="labelType"></param>
        /// <param name="sourceNo"></param>
        /// <returns></returns>
        SuspectProductLabel NewSuspectProductLabel(string batchNo, decimal qty, DispatchTask dispatchTask, double? resourceId, LabelType labelType = LabelType.WipBatch, string sourceNo = null)
        {
            SuspectProductLabel suspectProductLabel = new SuspectProductLabel()
            {
                BatchNo = batchNo,
                GoodQty = 0,
                WorkOrderId = dispatchTask.WorkOrderId.GetValueOrDefault(),
                Qty = qty,
                ProcessId = dispatchTask.ProcessId.GetValueOrDefault(),
                HandleState = SuspectHandleState.Pending,
                LabelType = labelType,
                NeedMrbReport = false,
                DispatchTaskId = dispatchTask.Id,
                WipResourceId = resourceId > 0 ? resourceId : dispatchTask.ResourceId,
                ProcessBatchNo = sourceNo
            };
            return suspectProductLabel;
        }


        /// <summary>
        /// 生成批次标签
        /// </summary>
        /// <param name="dispatchTask"></param>
        /// <param name="dispatchConfig"></param>
        /// <param name="isSuspectProduct"></param>
        /// <param name="printQty"></param>
        /// <param name="zcode"></param>
        /// <returns></returns>
        public virtual Tuple<EntityList<WipBatch>, bool> CreateWipBatchs(DispatchTask dispatchTask, DispatchConfigValue dispatchConfig, YesNo isSuspectProduct, decimal printQty, decimal zcode = 0)
        {
            //是否只生成一个标签
            bool isGrenOne = false;
            if (zcode == 0)
            {
                var layoutInfo = dispatchTask.WorkOrder.LayoutInfoList.Where(p => p.ProcessCode == dispatchTask.ProcessCode).FirstOrDefault();
                //标签按该工序任务单分单数量生成（分单数量在工单工艺路线里），标签个数=向上取整（良品数量/分单数量），
                //常规标签数量为分单数量，尾箱标签数量=良品数量-【向上取整（良品数量/分单数量）-1】*分单数

                //分子
                //decimal zcode = 0;
                if (layoutInfo != null && layoutInfo.Zcode != 0)
                {
                    zcode = layoutInfo.Zcode;
                }
                else
                {
                    zcode = printQty;

                    isGrenOne = true;
                }
            }
            //生成标签数量
            var invOrg = RT.Service.Resolve<InvOrgController>().GetByCode(RT.InvOrg.Value);

            int count = Convert.ToInt32(Math.Ceiling(printQty / zcode));

            List<string> batchNos = new List<string>();
            double numberRuleId = 0;

            //如果工序配置了编码规则,优先使用该规则
            if (dispatchTask.Process?.NumberRuleId > 0)
                numberRuleId = dispatchTask.Process.NumberRuleId.Value;

            if (numberRuleId == 0)
            {
                //根据产品的物料类型，找到配置项
                TypeConfigValue configValue = Query<TypeConfigValue>().Where(p => p.Type == dispatchTask.Product.Mtart).FirstOrDefault();
                var config = ConfigService.GetConfig(new ItemTypeConfig(), typeof(DispatchTask), configValue);
                if (config != null && config.ProcessNumberRuleId != null)
                    numberRuleId = config.ProcessNumberRuleId.Value;
            }
            if (numberRuleId == 0)
            {
                //2810用的是绕包线编码规则以及非绕包线编码规则
                if (invOrg.ExternalId != "2810")
                {

                    numberRuleId = dispatchConfig.NumberRuleId.GetValueOrDefault();
                }
                else
                {
                    if (dispatchTask.Product.Zmc.Contains("绕包"))
                    {
                        numberRuleId = dispatchConfig.EntangleNumberRuleId.GetValueOrDefault();
                    }
                    else
                    {
                        numberRuleId = dispatchConfig.UnEntangleNumberRuleId.GetValueOrDefault();
                    }
                }
            }

            batchNos = RT.Service.Resolve<NumberRuleController>().GenerateSegment(numberRuleId, count, dispatchTask).ToList();

            int qty = Convert.ToInt32(printQty);
            var range = new BarcodeRange()
            {
                PrintQty = qty,
                StartSn = batchNos[0],
                EndSn = batchNos[count - 1],
                State = ReceiveState.NoReceive,
                WorkOrderId = dispatchTask.WorkOrderId.GetValueOrDefault(),
            };

            RF.Save(range);
            EntityList<WipBatch> wipBatchs = new EntityList<WipBatch>();
            //生成标签
            decimal printedCount = printQty;
            foreach (var sn in batchNos)
            {
                bool isMantissa = false;
                decimal batchQty = zcode;
                if (printedCount < zcode)
                {
                    batchQty = printedCount;
                    isMantissa = true;
                }

                var barcode = new WipBatch()
                {
                    BatchNo = sn,
                    IsScraped = false,
                    Qty = batchQty,
                    BoxesQty = batchQty,
                    IsMantissa = isMantissa,
                    WorkOrderId = dispatchTask.WorkOrderId.GetValueOrDefault(),
                    PrintDate = DateTime.Now,
                    BatchState = BatchState.Generated,
                    Range = range,
                    IsChild = false,
                    IsGenerateChild = false,
                    IsGenerate = true,
                    IsSuspectProduct = isSuspectProduct,
                    DispatchTaskId = dispatchTask?.Id,
                    ResourceCode = dispatchTask?.ResourceCode,
                    ProcessCode = dispatchTask?.ProcessCode,
                    GenerateProcessCode = dispatchTask?.ProcessCode,
                    IsOutsourcing = dispatchTask?.IsOutsourcing,
                    EditQtyProcessCode = dispatchTask.ProcessCode
                };
                wipBatchs.Add(barcode);
                printedCount -= zcode;
            }
            isGrenOne = wipBatchs.Count == 1;

            return new Tuple<EntityList<WipBatch>, bool>(wipBatchs, isGrenOne);
        }

        /// <summary>
        /// 创建安灯记录
        /// </summary>
        /// <param name="dispatchTask"></param>
        /// <param name="submitPdaReportInfo"></param>
        private void CreateAndonManages(DispatchTask dispatchTask, SubmitPdaReportInfo submitPdaReportInfo, double wipId)
        {
            //查询判断是否调用安灯接口
            var andonList = RT.Service.Resolve<AndonController>().GetAndonBySuspect();
            if (andonList.Count() == 0)
            {
                return;
            }
            var thresholds = RT.Service.Resolve<ThresholdController>().GetThresholdByItem(dispatchTask.ProductId, dispatchTask.ProcessId.GetValueOrDefault());
            if (thresholds.Count() == 0)
            {
                return;
            }
            //按工单数量和比值计算安灯触发阈值，累计可疑品达到阈值才触发安灯
            var task = RF.GetById<DispatchTask>(dispatchTask.Id);
            var alertQty = (task.WorkOrder.PlanQty * thresholds[0].AlertValue / 100);
            if (task.SuspectQty < alertQty)
                return;

            //decimal thresholdValue = 0;
            //decimal.TryParse(thresholds[0].ThresholdValue, out thresholdValue);
            //可疑品数>报工数量(良品+可疑品数)*（当前产品+工序维护的阈值百分比）
            //if (submitPdaReportInfo.SuspectQty <= (submitPdaReportInfo.GoodQty + submitPdaReportInfo.SuspectQty) * (thresholds[0].AlertValue / 100))
            //{
            //    return;
            //}
            var andonManagect = RT.Service.Resolve<AndonManageController>();
            EntityList<AndonManage> andonManages = new EntityList<AndonManage>();
            if (wipId == -1)
            {
                wipId = dispatchTask.ResourceId.GetValueOrDefault();
            }
            foreach (var item in andonList)
            {
                if (wipId == -1)
                {
                    wipId = dispatchTask.ResourceId.GetValueOrDefault();
                }
                var manage = andonManagect.CreateAndonManage(item.Id, 0, dispatchTask.ProcessId.GetValueOrDefault(), wipId, dispatchTask.FactoryId, dispatchTask.WorkShopId, dispatchTask.WorkOrderId, isValidAndonEquipAccount: false);
                manage.ProblemDesc = "可疑品达到阈值";
                andonManages.Add(manage);
            }
            RF.Save(andonManages);

            var dbDateTime = RF.Find<AndonManage>().GetDbTime();
            foreach (var andonManage in andonManages)
            {
                //创建触发操作记录
                var andonManageOperateLog = new AndonManageOperateLog
                {
                    AndonManageId = andonManage.Id,
                    OperateTime = dbDateTime,
                    OperateType = AndonManageOperateType.Add,
                    OperaterId = RT.IdentityId,
                    LastOperate = 0,
                    PersistenceStatus = PersistenceStatus.New
                };
                RF.Save(andonManageOperateLog);

                try
                {
                    //触发企微
                    RT.Service.Resolve<AndonManageController>().SendMarkdownMessage(andonManage);
                }
                catch (Exception ex)
                {
                    RT.Logger.Error(ex.Message);
                }
                try
                {
                    //触发IOT安灯
                    RT.Service.Resolve<AndonManageController>().SendIotMessage(andonManage);
                }
                catch (Exception ex)
                {
                    RT.Logger.Error(ex.Message);
                }
            }
        }

        /// <summary>
        /// 获取打印标签数据
        /// </summary>
        /// <param name="IsSuspectProduct">是否生成的数据,存在可疑品</param>
        /// <param name="labelType">打印类型</param>
        /// <param name="dispatchTask"></param>
        /// <param name="printTemplate"></param>
        /// <param name="wipBatchs"></param>
        /// <returns></returns>
        public virtual List<PdaPrintInfo> CreatePrintDatas(bool IsSuspectProduct, PrintLabelType labelType, DispatchTask dispatchTask, string printTemplate, string suspectTemplate, EntityList<WipBatch> wipBatchs, WipResource wipResource = null)
        {
            List<PdaPrintInfo> printDatas = new List<PdaPrintInfo>();
            var woIds = new List<double>() { dispatchTask.WorkOrderId.GetValueOrDefault() };
            var layoutInfos = RT.Service.Resolve<SIE.MES.WorkOrders.WorkOrderController>().GetLayoutInfosByWorkOrderId(woIds);
            string layoutCodes = string.Join("、", layoutInfos.OrderBy(p => p.Vornr).Select(p => p.ProcessCode));
            var list = dispatchTask.WorkOrder;
            foreach (var item in wipBatchs)
            {
                var labelPrintInfo = new PdaPrintInfo()
                {
                    BatchNo = item.BatchNo,
                    IsSuspectProduct = item.IsSuspectProduct == YesNo.Yes,
                    Qty = item.Qty,
                    ResourceCode = wipResource != null ? wipResource.Code : dispatchTask.Resource?.Code,
                    ResourceName = wipResource != null ? wipResource.Name : dispatchTask.Resource?.Name,
                    ProductName = dispatchTask.WorkOrder?.Product?.Name,
                    ProductCode = dispatchTask.WorkOrder?.Product?.Code,
                    ProcessCode = dispatchTask.Process?.Code,
                    DateTime = item.UpdateDate.ToString("yyyy/MM/dd HH:mm:ss"),
                    PrintTemplateId = dispatchTask.Process?.PrintTemplateId //优先使用工序配置的打印模板
                };

                if (labelPrintInfo.PrintTemplateId == null || labelPrintInfo.PrintTemplateId == 0)
                {
                    //根据产品的物料类型，找到配置项
                    TypeConfigValue configValue = Query<TypeConfigValue>().Where(p => p.Type == dispatchTask.Product.Mtart).FirstOrDefault();
                    var config = ConfigService.GetConfig(new ItemTypeConfig(), typeof(DispatchTask), configValue);
                    if (config != null && config.ProcessPrintTemplateId != null)
                    {
                        labelPrintInfo.PrintTemplateId = config.ProcessPrintTemplateId;
                    }
                }

                if (IsSuspectProduct)
                {
                    labelType = item.IsSuspectProduct == YesNo.Yes ? PrintLabelType.Suspect : PrintLabelType.Good;
                }
                string template = printTemplate;
                if (labelType == PrintLabelType.Suspect)
                    template = suspectTemplate;
                string printData = printTemplate
                     .Replace("@WorkOrderNo", dispatchTask.WorkOrder?.No)   //工单号
                     .Replace("@WorkOrderState", dispatchTask.WorkOrderState.ToLabel())  //工单状态
                     .Replace("@DispatchTaskNo", dispatchTask.No)     //派单号
                     .Replace("@DispatchTaskStatus", dispatchTask.TaskStatus.ToLabel())     //派工单状态
                     .Replace("@ProductCode", dispatchTask.Product?.Code)     //物料编码
                     .Replace("@ProductName", dispatchTask.Product?.Name)     //物料名称
                     .Replace("@ProductShortDesc", dispatchTask.Product?.ShortDescription)     //旧料号
                     .Replace("@ResourceCode", dispatchTask.Resource?.Code)     //资源编码
                     .Replace("@ResourceName", dispatchTask.Resource?.Name)     //资源名称
                     .Replace("@ProcessCode", dispatchTask.Process?.Code)     //工序编码
                     .Replace("@ProcessName", dispatchTask.Process?.Name)     //工序名称
                     .Replace("@ReportType", labelType.ToLabel())     //报工类型
                     .Replace("@ReportQty", item.Qty.ToString())     //报工数量：根据报工类型取值报工数量
                     .Replace("@LabelNo", item.BatchNo)     //标签号
                     .Replace("@CreateDate", item.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"))     //标签创建时间
                     .Replace("@ReportType", layoutCodes)     //工单工艺路线
                     ;
                labelPrintInfo.PrintCmd = printData;
                printDatas.Add(labelPrintInfo);
            }
            return printDatas;
        }

        #region 扫码报工


        /// <summary>
        /// 获取工单信息
        /// </summary>
        ///  <param name="key">SN或工单号</param>
        ///  <param name="processId">工序ID</param>
        /// <returns>获取生产资源列表</returns>
        [ApiService("获取工单信息")]
        [return: ApiReturn("工单信息. 参数类型: List<PdaWorkOrderInfo>")]
        [ApiLog]
        public virtual PdaScanReturnInfo GetWorkOrderInfo(string key, double processId)
        {
            if (key.IsNullOrEmpty())
                throw new ValidationException("输入的信息不正确".L10N());
            WorkOrder workOrder = RT.Service.Resolve<WorkOrderController>().GetWorkOrder(key);
            if (workOrder == null)
            {
                var barcode = RT.Service.Resolve<BarcodeController>().GetBarcode(key);
                if (barcode == null)
                    throw new ValidationException("找不到工单号信息.".L10N());
                workOrder = RF.GetById<WorkOrder>(barcode.WorkOrderId, new EagerLoadOptions().LoadWithViewProperty());
            }
            if (workOrder == null)
                throw new ValidationException("找不到工单号信息.".L10N());
            //根据工序+工单号获取任务单
            var dispatchTask = RT.Service.Resolve<DispatchController>().GetDispatchTaskByWoProcess(workOrder.Id, processId);
            if (dispatchTask == null)
                throw new ValidationException("找不到任务单信息.".L10N());
            PdaScanReturnInfo workOrderInfo = new PdaScanReturnInfo()
            {
                WorkOrderId = workOrder.Id,
                WorkOrderNo = workOrder.No,
                ProductCode = workOrder.ProductCode,
                ProductName = workOrder.ProductName,
                ProductId = workOrder.ProductId,
                DispatchTaskId = dispatchTask.Id,
                DispatchTaskNo = dispatchTask.No,
                PlanQty = workOrder.PlanQty
            };
            return workOrderInfo;
        }

        /// <summary>
        /// 验证扫码信息
        /// </summary>
        ///  <param name="pdaScanInfo">扫描内容</param>
        /// <returns>获取生产资源列表</returns>
        [ApiService("验证扫码信息")]
        [return: ApiReturn("工单信息. 参数类型: List<PdaResourceInfo>")]
        [ApiLog]
        public virtual PdaProcessScanReturnInfo CheckProcessScanInfo(PdaScanInfo pdaScanInfo)
        {
            var result = ValidationProcessScanInfo(pdaScanInfo);
            return result;
        }

        /// <summary>
        /// 验证扫码信息
        /// </summary>
        ///  <param name="pdaScanInfo">扫描内容</param>
        /// <returns>获取生产资源列表</returns>
        [ApiService("验证扫码信息")]
        [return: ApiReturn("工单信息. 参数类型: List<PdaResourceInfo>")]
        [ApiLog]
        public virtual PdaScanReturnInfo CheckScanInfo(PdaScanInfo pdaScanInfo)
        {
            PdaScanReturnInfo workOrderInfo = ValidationScanInfo(pdaScanInfo);

            return workOrderInfo;
        }

        /// <summary>
        /// 验证扫码信息
        /// </summary>
        ///  <param name="pdaScanInfo">扫描内容</param>
        /// <returns>获取生产资源列表</returns>
        [ApiService("获取派工单")]
        [return: ApiReturn("工单信息. 参数类型: List<PdaResourceInfo>")]
        [ApiLog]
        public virtual List<PdaPgReturnInfo> ShowPgInfo(PdaPgInfo pdaPgInfo)
        {
            List<PdaPgReturnInfo> pdaPgReturnInfo = CombinationPgInfo(pdaPgInfo);
            return pdaPgReturnInfo;
        }

        public virtual PdaProcessScanReturnInfo ValidationProcessScanInfo(PdaScanInfo pdaScanInfo)
        {
            var process = RF.GetById<Process>(pdaScanInfo.ProcessId, new EagerLoadOptions().LoadWithViewProperty());

            PdaProcessScanReturnInfo workOrderInfo = new PdaProcessScanReturnInfo();
            if (pdaScanInfo.Sn.IsNullOrEmpty())
                throw new ValidationException("标签不能为空.".L10N());
            if (pdaScanInfo.ProcessId == 0)
                throw new ValidationException("工序不正确.".L10N());

            var wipBatch = RT.Service.Resolve<WipBatchController>().GetWipBatch(pdaScanInfo.Sn);
            if (wipBatch == null)
                throw new ValidationException("标签[{0}]不存在.".L10nFormat(pdaScanInfo.Sn));
            if (wipBatch.IsScraped)
                throw new ValidationException("标签[{0}]已报废.".L10nFormat(pdaScanInfo.Sn));
            if (wipBatch.IsRework)
            {
                workOrderInfo.Tip = "标签[{0}]为返工标签, 不允许报工.".L10nFormat(pdaScanInfo.Sn);
            }

            bool isExists = RT.Service.Resolve<ProcessPtyController>().GetIsTransferProcessPty(pdaScanInfo.ProcessId, wipBatch.WorkOrder.ProductId);
            //转入模式判断当前工序是否启用
            if (pdaScanInfo.ScanType == 2 && isExists == false)
            {
                workOrderInfo.Tip = "当前工序[{0}]未启用转入，禁止使用转入类型".L10nFormat(process?.Name);
            }

            StringBuilder errMsg = new StringBuilder();
            try
            {
                //扫描首个标签 校验生产资源信息
                if (pdaScanInfo.IsFirstSn)
                {
                    //获取工单信息
                    var workOrder = RF.GetById<WorkOrder>(wipBatch.WorkOrderId, new EagerLoadOptions().LoadWithViewProperty());
                    workOrderInfo.WorkOrderId = wipBatch.WorkOrderId;
                    workOrderInfo.WorkOrderNo = workOrder?.No;
                    workOrderInfo.ProductCode = workOrder?.ProductCode;
                    workOrderInfo.ProductName = workOrder?.ProductName;
                    workOrderInfo.PlanQty = workOrder == null ? 0 : workOrder.PlanQty;
                    workOrderInfo.ProductId = (workOrder?.ProductId).GetValueOrDefault();
                    //获取派工任务单信息
                    //派工单号（根据标签对应的工单+当前资源+工序找到状态为“已派工”/“执行中”最早的任务）
                    //如果委外的标签就只能操作委外的任务单
                    Expression<Func<DispatchTask, bool>> exp = null;
                    exp = p => p.WorkOrderId == wipBatch.WorkOrderId && p.ProcessId == pdaScanInfo.ProcessId;
                    var tasks = _dispatchController.GetDispatchTaskByExpression(exp, p => p.CreateDate);
                    if (tasks.Count < 1)
                        throw new ValidationException("找不到对应的任务单号.".L10N());
                    //把任务单和状态也显示出来
                    if (tasks.All(p => p.TaskStatus != DispatchTaskStatus.Executing && p.TaskStatus != DispatchTaskStatus.Dispatched))
                    {
                        var nos = tasks.Select(p => p.No).ToList();
                        var status = tasks.Select(p => p.TaskStatus.ToLabel()).ToList();
                        throw new ValidationException("任务单[{0}]状态为[{1}]".L10nFormat(string.Join('、', nos), string.Join("、", status)));
                    }

                    var firstTask = tasks.FirstOrDefault(p => p.TaskStatus == DispatchTaskStatus.Executing || p.TaskStatus == DispatchTaskStatus.Dispatched);

                    if (firstTask.StartProcess == true)
                        throw new ValidationException("首工序不允许扫码报工.".L10N());
                    if (firstTask.TaskStatus != DispatchTaskStatus.Executing && firstTask.TaskStatus != DispatchTaskStatus.Dispatched)
                        throw new ValidationException("只有状态为[执行中、已派工]的任务单可报工".L10N());

                    var layoutInfo = firstTask.WorkOrder.LayoutInfoList.FirstOrDefault(p => p.ProcessCode == firstTask.Process.Code);

                    pdaScanInfo.DispatchTaskId = firstTask.Id;
                    workOrderInfo.DispatchTaskId = firstTask.Id;
                    workOrderInfo.DispatchTaskNo = firstTask.No;
                    workOrderInfo.DispatchQty = firstTask.DispatchQty;
                    workOrderInfo.ProcessCode = firstTask.ProcessCode;

                    var maxReportQty = RT.Service.Resolve<DispatchController>().MaxReportQty(firstTask);

                    workOrderInfo.MaxReportQty = maxReportQty;//firstTask.MaxReportQty;
                    workOrderInfo.RemainQty = firstTask.RemainQty;
                    workOrderInfo.Zcode = layoutInfo.Zcode;
                    workOrderInfo.MaxRemainQty = RT.Service.Resolve<DispatchController>().MaxReportQtyAndMaxRemainQty(firstTask, Enums.SourceType.Report_Scan).Item2;
                    workOrderInfo.ProcessMaxRemainQty = RT.Service.Resolve<DispatchController>().GetProcessMaxRemainQty(firstTask);


                    var processPty = RT.Service.Resolve<ProcessPtyController>().GetProcessPtysByProcessId(firstTask.ProcessId ?? 0, firstTask.ProductId);
                    workOrderInfo.IsReportValid = processPty?.IsReportValid ?? false;
                }
                else
                {
                    if (pdaScanInfo.WorkOrderId != wipBatch.WorkOrderId)
                        throw new ValidationException("标签工单与页面不一致.".L10N());
                }
            }
            catch (Exception ex)
            {
                workOrderInfo.Tip = ex.GetBaseException()?.Message;
            }

            //验证前置工序报工
            if (pdaScanInfo.DispatchTaskId > 0)
            {
                try
                {
                    var currTask = RF.GetById<DispatchTask>(pdaScanInfo.DispatchTaskId, new EagerLoadOptions().LoadWithViewProperty());
                    ValidatePrepareProcessHasReport(wipBatch, currTask, pdaScanInfo.Type == 1 ? false : true);
                }
                catch (Exception ex)
                {
                    workOrderInfo.Tip = ex.GetBaseException()?.Message;
                }
            }

            if (wipBatch.IsSuspectProduct == YesNo.Yes)
                errMsg.AppendLine("标签为可疑品，请进行可疑品处理.".L10N());

            var reportWibatch = Query<ReportWipBatch>().Where(p => p.BatchNo == wipBatch.BatchNo && p.ReportRecord.DispatchTaskId == pdaScanInfo.DispatchTaskId).FirstOrDefault();
            //标记条码状态和前端要提示的信息
            if (reportWibatch != null)
            {
                if (pdaScanInfo.Type != null && pdaScanInfo.Type == 1)
                {
                    workOrderInfo.State = "OK";
                }
            }
            else
            {
                workOrderInfo.State = "NG";
                workOrderInfo.Msg = "标签号{0},工序{1}未完成".L10nFormat(pdaScanInfo.Sn, process?.Code);
            }

            //校验工单当前工序是否为工作单元选择的工序，不是则报错提示‘当前工序应该为XXX’后续验证

            //校验当前标签 是否已转入
            if (pdaScanInfo.ScanType == 1)
            {
                if (isExists)
                {
                    bool transIsExists = IsExistsTransferLabels(pdaScanInfo.DispatchTaskId.GetValueOrDefault(), pdaScanInfo.Sn);
                    if (!transIsExists)
                        workOrderInfo.Tip = "标签不存在转入记录,请先转入.".L10N();
                }
                List<string> strLabels = new List<string>() { pdaScanInfo.Sn };
            }


            if (errMsg.ToString().IsNotEmpty())
                throw new ValidationException(errMsg.ToString());

            workOrderInfo.LabelNo = pdaScanInfo.Sn;
            workOrderInfo.LabelQty = wipBatch.Qty;
            workOrderInfo.IsSuspectProduct = wipBatch.IsSuspectProduct == YesNo.Yes;
            return workOrderInfo;
        }

        /// <summary>
        /// 验证扫描数据
        /// </summary>
        /// <param name="pdaScanInfo"></param>
        /// <exception cref="ValidationException"></exception>
        private PdaScanReturnInfo ValidationScanInfo(PdaScanInfo pdaScanInfo)
        {
            PdaScanReturnInfo workOrderInfo = new PdaScanReturnInfo();
            //扫描验证
            var wipBatch = ScanValidation(pdaScanInfo);
            bool isExists = RT.Service.Resolve<ProcessPtyController>().GetIsTransferProcessPty(pdaScanInfo.ProcessId, wipBatch.WorkOrder.ProductId);
            //转入模式判断当前工序是否启用
            if (pdaScanInfo.ScanType == 2 && isExists == false)
            {
                var process = RF.GetById<Process>(pdaScanInfo.ProcessId);
                throw new ValidationException("当前工序[{0}]未启用转入，禁止使用转入类型".L10nFormat(process?.Name));
            }

            StringBuilder errMsg = new StringBuilder();
            //扫描首个标签 校验生产资源信息
            if (pdaScanInfo.IsFirstSn)
            {
                //获取工单信息
                var workOrder = RF.GetById<WorkOrder>(wipBatch.WorkOrderId, new EagerLoadOptions().LoadWithViewProperty());
                workOrderInfo.WorkOrderId = wipBatch.WorkOrderId;
                workOrderInfo.WorkOrderNo = workOrder?.No;
                workOrderInfo.ProductCode = workOrder?.ProductCode;
                workOrderInfo.ProductName = workOrder?.ProductName;
                workOrderInfo.PlanQty = workOrder == null ? 0 : workOrder.PlanQty;
                workOrderInfo.ProductId = (workOrder?.ProductId).GetValueOrDefault();
                //获取派工任务单信息
                //派工单号（根据标签对应的工单+当前资源+工序找到状态为“已派工”/“执行中”最早的任务）
                //如果委外的标签就只能操作委外的任务单
                Expression<Func<DispatchTask, bool>> exp = null;
                //if (wipBatch.IsOutsourcing == true)
                //    exp = p => p.WorkOrderId == wipBatch.WorkOrderId && p.ProcessId == pdaScanInfo.ProcessId && /*p.ResourceId == pdaScanInfo.ResourceId &&*/
                ///*(p.TaskStatus == DispatchTaskStatus.Executing || p.TaskStatus == DispatchTaskStatus.Dispatched) &&*/ p.IsOutsourcing == true;
                //else
                //    exp = p => p.WorkOrderId == wipBatch.WorkOrderId && p.ProcessId == pdaScanInfo.ProcessId && /*p.ResourceId == pdaScanInfo.ResourceId &&*/
                //    /*(p.TaskStatus == DispatchTaskStatus.Executing || p.TaskStatus == DispatchTaskStatus.Dispatched) &&*/ (p.IsOutsourcing == false || p.IsOutsourcing == null);

                var tasks = new EntityList<DispatchTask>();
                var resource = RF.GetById<WipResource>(pdaScanInfo.ResourceId, new EagerLoadOptions().LoadWithViewProperty());

                var dispatchConfig = RT.Service.Resolve<DispatchController>().GetDispatchConfig();
                if (dispatchConfig.IsCheckProductionLineTaskList)//派工管理配置是否强制校验产线对应任务单True只能选择任务单执行对象
                {
                    var station = RF.GetById<Station>(pdaScanInfo.StationId);
                    exp = p => p.WorkOrderId == wipBatch.WorkOrderId && p.ProcessId == pdaScanInfo.ProcessId && p.TaskPerformer.Contains($"%{station.Name}%");
                }
                else
                    exp = p => p.WorkOrderId == wipBatch.WorkOrderId && p.ProcessId == pdaScanInfo.ProcessId;
                tasks = _dispatchController.GetDispatchTaskByExpression(exp, p => p.CreateDate);
                if (tasks.Count < 1)
                    throw new ValidationException("找不到对应的任务单号.".L10N());
                //把任务单和状态也显示出来
                if (tasks.All(p => p.TaskStatus != DispatchTaskStatus.Executing && p.TaskStatus != DispatchTaskStatus.Dispatched))
                {
                    var nos = tasks.Select(p => p.No).ToList();
                    var status = tasks.Select(p => p.TaskStatus.ToLabel()).ToList();
                    throw new ValidationException("任务单[{0}]状态为[{1}]".L10nFormat(string.Join('、', nos), string.Join("、", status)));
                }

                var firstTask = tasks.FirstOrDefault(p => p.TaskStatus == DispatchTaskStatus.Executing || p.TaskStatus == DispatchTaskStatus.Dispatched);
                //if (firstTask == null)
                //    throw new ValidationException("找不到对应的任务单号.".L10N());
                //if (firstTask.TaskStatus != DispatchTaskStatus.Executing && firstTask.TaskStatus != DispatchTaskStatus.Dispatched)
                //    throw new ValidationException("任务单[{0}]状态为[{1}]".L10nFormat(firstTask.No, firstTask.TaskStatus.ToLabel()));

                if (firstTask.StartProcess == true)
                    throw new ValidationException("首工序不允许扫码报工.".L10N());
                if (firstTask.TaskStatus != DispatchTaskStatus.Executing && firstTask.TaskStatus != DispatchTaskStatus.Dispatched)
                    errMsg.AppendLine("只有状态为[执行中、已派工]的任务单可报工".L10N());

                var layoutInfo = firstTask.WorkOrder.LayoutInfoList.FirstOrDefault(p => p.ProcessCode == firstTask.Process.Code);

                pdaScanInfo.DispatchTaskId = firstTask.Id;
                workOrderInfo.DispatchTaskId = firstTask.Id;
                workOrderInfo.DispatchTaskNo = firstTask.No;
                workOrderInfo.DispatchQty = firstTask.DispatchQty;
                workOrderInfo.ProcessCode = firstTask.ProcessCode;
                //当委外任务单进行报工的时候，要之前的工序是否存在委外
                var config = ConfigService.GetConfig(new OutsourcingReportConfig(), typeof(OutsourcingRequest));
                if (config != null && config.IsValidOutsourcingReportLog == true)
                {
                    var layoutInfos = Query<LayoutInfo>().Join<Process>((x, y) => x.ProcessCode == y.Code).Join<Process, WorkOrderRoutingProcess>((x, y) => x.Id == y.ProcessId && y.WorkOrderId == wipBatch.WorkOrderId && y.Index < firstTask.Seq).Join<WorkOrderRoutingProcess, OutsourcingRequest>((x, y) => x.ProcessId == y.BeginProcess.ProcessId && y.WorkOrderId == wipBatch.WorkOrderId).Where(p => p.WorkOrderId == wipBatch.WorkOrderId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

                    if (layoutInfos.Count > 0)
                    {
                        var lastLayoutInfo = layoutInfos.OrderByDescending(p => Convert.ToDecimal(p.Vornr)).FirstOrDefault();
                        OutsourcingRequest outsourcingRequest = Query<OutsourcingRequest>().Where(p => p.BeginProcess.Process.Code == lastLayoutInfo.ProcessCode && p.WorkOrderId == wipBatch.WorkOrderId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());

                        if (outsourcingRequest != null && outsourcingRequest.OutsourcingReportLogList.All(p => p.SN != pdaScanInfo.Sn))
                        {
                            throw new ValidationException("委外工序没有报工，不允许扫描".L10N());
                        }
                    }
                }

                var tuple = RT.Service.Resolve<DispatchController>().MaxReportQtyAndMaxRemainQty(firstTask, Enums.SourceType.Report_Scan);
                var maxReportQty = tuple.Item1;
                var remainQty = tuple.Item2;
                workOrderInfo.MaxReportQty = maxReportQty;//firstTask.MaxReportQty;
                workOrderInfo.RemainQty = firstTask.RemainQty;
                workOrderInfo.Zcode = layoutInfo.Zcode;
                workOrderInfo.MaxRemainQty = remainQty;//RT.Service.Resolve<DispatchController>().MaxReportQtyAndMaxRemainQty(firstTask).Item2;
                workOrderInfo.ProcessMaxRemainQty = RT.Service.Resolve<DispatchController>().GetProcessMaxRemainQty(firstTask);//remainQty


                var processPty = RT.Service.Resolve<ProcessPtyController>().GetProcessPtysByProcessId(firstTask.ProcessId ?? 0, firstTask.ProductId);
                workOrderInfo.IsReportValid = processPty?.IsReportValid ?? false;
            }
            else
            {
                if (pdaScanInfo.WorkOrderId != wipBatch.WorkOrderId)
                    errMsg.AppendLine("标签工单与页面不一致.".L10N());
            }
            //验证前置工序报工
            if (pdaScanInfo.DispatchTaskId > 0)
            {
                var currTask = RF.GetById<DispatchTask>(pdaScanInfo.DispatchTaskId, new EagerLoadOptions().LoadWithViewProperty());
                ValidatePrepareProcessHasReport(wipBatch, currTask, pdaScanInfo.Type == 1 ? false : true);
            }

            if (wipBatch.IsSuspectProduct == YesNo.Yes)
                errMsg.AppendLine("标签为可疑品，请进行可疑品处理.".L10N());

            var reportWibatch = Query<ReportWipBatch>().Where(p => p.BatchNo == wipBatch.BatchNo && p.ReportRecord.DispatchTaskId == pdaScanInfo.DispatchTaskId).FirstOrDefault();
            //标记条码状态和前端要提示的信息
            if (reportWibatch != null)
            {
                if (pdaScanInfo.Type != null && pdaScanInfo.Type == 1)
                {
                    workOrderInfo.State = "OK";
                }
                else
                {
                    errMsg.AppendLine("标签已提交.".L10N());
                }
            }
            else
            {
                workOrderInfo.State = "NG";
                workOrderInfo.Msg = "标签号{0},工序{1}未完成".L10nFormat(pdaScanInfo.Sn, workOrderInfo.ProcessCode);
            }

            //校验工单当前工序是否为工作单元选择的工序，不是则报错提示‘当前工序应该为XXX’后续验证

            //校验当前标签 是否已转入
            if (pdaScanInfo.ScanType == 1)
            {
                if (isExists)
                {
                    bool transIsExists = IsExistsTransferLabels(pdaScanInfo.DispatchTaskId.GetValueOrDefault(), pdaScanInfo.Sn);
                    if (!transIsExists)
                        errMsg.AppendLine("标签不存在转入记录,请先转入.".L10N());
                }
                List<string> strLabels = new List<string>() { pdaScanInfo.Sn };
                //验证条码标签是否已提交
                //var labels = GetReportScanLabelLogsByLabel(strLabels);
                //if (labels.Count() > 0)
                //    errMsg.AppendLine("标签已提交.".L10N());
            }


            if (errMsg.ToString().IsNotEmpty())
                throw new ValidationException(errMsg.ToString());

            workOrderInfo.LabelNo = pdaScanInfo.Sn;
            workOrderInfo.LabelQty = wipBatch.Qty;
            workOrderInfo.IsSuspectProduct = wipBatch.IsSuspectProduct == YesNo.Yes;
            return workOrderInfo;
        }

        /// <summary>
        /// 扫描验证
        /// </summary>
        /// <param name="pdaScanInfo"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        private WipBatch ScanValidation(PdaScanInfo pdaScanInfo)
        {
            if (pdaScanInfo.Sn.IsNullOrEmpty())
                throw new ValidationException("标签不能为空.".L10N());
            if (pdaScanInfo.ProcessId == 0)
                throw new ValidationException("工序不正确.".L10N());

            var wipBatch = RT.Service.Resolve<WipBatchController>().GetWipBatch(pdaScanInfo.Sn);
            if (wipBatch == null)
                throw new ValidationException("标签[{0}]不存在.".L10nFormat(pdaScanInfo.Sn));
            if (wipBatch.IsScraped)
                throw new ValidationException("标签[{0}]已报废.".L10nFormat(pdaScanInfo.Sn));
            if (wipBatch.IsRework)
                throw new ValidationException("标签[{0}]为返工标签, 不允许报工.".L10nFormat(pdaScanInfo.Sn));
            return wipBatch;
        }

        /// <summary>
        /// 组合派工信息
        /// </summary>
        /// <param name="pdaPgInfo"></param>
        /// <returns></returns>
        /// <exception cref="CombinationPgInfo"></exception>
        private List<PdaPgReturnInfo> CombinationPgInfo(PdaPgInfo pdaPgInfo)
        {
            List<PdaPgReturnInfo> info = new List<PdaPgReturnInfo>();
            if (pdaPgInfo.ProcessId <= 0)
                throw new ValidationException("工序不正确.".L10N());
            if (pdaPgInfo.ResourceId <= 0)
                throw new ValidationException("资源不正确.".L10N());
            if (pdaPgInfo.WorkOrderId <= 0)
                throw new ValidationException("工单不正确.".L10N());
            var dispatchTaskDataList = Query<DispatchTask>().Where(p => p.WorkOrderId == pdaPgInfo.WorkOrderId && p.No.Contains(pdaPgInfo.PgName) && p.ResourceId == pdaPgInfo.ResourceId && p.ProcessId == pdaPgInfo.ProcessId && (p.TaskStatus == DispatchTaskStatus.Dispatched || p.TaskStatus == DispatchTaskStatus.Executing)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var processIds = dispatchTaskDataList.Where(p => p.ProcessId != null).Select(p => p.ProcessId.Value).Distinct().ToList();
            foreach (var item in dispatchTaskDataList)
            {
                var processPtys = RT.Service.Resolve<ProcessPtyController>().GetProcessPtysByProcessIds(new List<double>() { item.ProcessId.Value }, item.ProductId);
                var kzItemCategory = RT.Service.Resolve<KzItemCategorysController>().GetKzItemCategorieByItemId(item.ProductId);
                var pps = new List<ProcessPty>();
                if (kzItemCategory != null)
                {
                    pps = processPtys.Where(p => p.KzCategoryId == kzItemCategory.KzCategoryId).ToList();
                }
                ////当找得到分类得时候，优先找到分类的，然后再找工序的
                if (pps.Count == 0)
                    pps = processPtys.Where(p => p.KzCategoryId == null).ToList();

                var layoutInfo = item.WorkOrder.LayoutInfoList.FirstOrDefault(p => p.ProcessCode == item.Process.Code);

                PdaPgReturnInfo pdaPgReturnInfo = new PdaPgReturnInfo();
                pdaPgReturnInfo.WorkOrderId = (double)item.WorkOrderId;
                pdaPgReturnInfo.WorkOrderNo = item.WorkOrderNo;
                pdaPgReturnInfo.DispatchTaskId = item.Id;
                pdaPgReturnInfo.DispatchTaskNo = item.No;
                pdaPgReturnInfo.ProductId = (double)item.ProductId;
                pdaPgReturnInfo.ProductCode = item.ProductCode;
                pdaPgReturnInfo.ProductName = item.ProductName;
                pdaPgReturnInfo.LabelQty = item.DispatchQty;
                pdaPgReturnInfo.PlanQty = item.WorkOrder.PlanQty;
                pdaPgReturnInfo.Zcode = layoutInfo?.Zcode ?? 0;
                pdaPgReturnInfo.IsReportValid = pps.FirstOrDefault(p => p.ProcessId == (item.ProcessId ?? 0))?.IsReportValid ?? false;
                info.Add(pdaPgReturnInfo);
            }
            return info;
        }

        /// <summary>
        /// 校验标签是否已报工
        /// </summary>
        /// <param name="batchNo"></param>
        /// <param name="process"></param>
        /// <param name="throwEx"></param>
        /// <returns></returns>
        public virtual bool ValidateProcessHasReport(string batchNo, string process, bool throwEx = true)
        {
            var processList = process.Split(',');
            var reportWibatchs = DB.Query<ReportWipBatch>().Where(p => p.BatchNo == batchNo && (processList.Contains(p.ReportRecord.Process.Code) || processList.Contains(p.ReportRecord.Process.Name))).Select(p => p.BatchNo).ToList<string>();
            if (reportWibatchs.Any())
            {
                if (throwEx)
                    throw new ValidationException("标签[{0}]已存在工序[{1}]任务的报工记录,请确认".L10nFormat(reportWibatchs.Distinct().Concat(","), process));
                return true;
            }
            return false;
        }

        /// <summary>
        /// 校验标签是否已报工
        /// </summary>
        /// <param name="batchNos"></param>
        /// <param name="process"></param>
        public virtual void ValidateProcessHasReport(List<string> batchNos, string process)
        {
            //当遇到包装的时候就跳过这个校验
            if (process.Contains("包装"))
                return;
            var processList = process.Split(',');
            var reportWibatchs = DB.Query<ReportWipBatch>().Where(p => batchNos.Contains(p.BatchNo) && (processList.Contains(p.ReportRecord.Process.Code) || processList.Contains(p.ReportRecord.Process.Name))).Select(p => p.BatchNo).ToList<string>();
            if (reportWibatchs.Any())
                throw new ValidationException("标签[{0}]已存在工序[{1}]任务的报工记录,请确认".L10nFormat(reportWibatchs.Distinct().Concat(","), process));
        }

        /// <summary>
        /// 校对前置任务是否已报工
        /// </summary>
        /// <param name="wipBatch"></param>
        /// <param name="currTask"></param>
        /// <exception cref="ValidationException"></exception>
        public virtual void ValidatePrepareProcessHasReport(WipBatch wipBatch, DispatchTask currTask, bool isValidateCurrTaskReport = true)
        {
            //查询工单对应的所有任务单
            var tasks = RT.Service.Resolve<DispatchController>().GetDispatchTasksByWorkOrderIds(new List<double>() { wipBatch.WorkOrderId });
            if (tasks.Count == 0)
                return;

            var taskPres = tasks.Where(p => p.Seq < currTask.Seq).ToList();  //前置工序

            //校验前置任务是否都有该标签的报工记录(如果是CS包装打印生成的直接跳过)
            if ((wipBatch.IsPressureSnPrint == null || wipBatch.IsPressureSnPrint == false) && wipBatch.ReportRecordIds.IsNullOrEmpty() && (wipBatch.IsOutsourcing == null || wipBatch.IsOutsourcing == false)/*(currTask.IsOutsourcing == null || currTask.IsOutsourcing == false)*/)
            {
                var list = GetReportWipBatchsByWipBatchId(wipBatch.Id);
                if (list.Count == 0)
                    throw new ValidationException("对应工序标签[{0}]还未进行报工,请确认".L10nFormat(wipBatch.BatchNo));
                //补全异常数据
                wipBatch.ReportRecordIds = list.OrderBy(p => p.CreateDate).Select(p => p.ReportRecordId.ToString()).Concat(",");
                RF.Save(wipBatch);
            }

            //校验当前工序是否已报工
            if (isValidateCurrTaskReport)
            {
                ValidateProcessHasReport(wipBatch.BatchNo, currTask.ProcessCode);
            }

            var recordIds = wipBatch.ReportRecordIds.Split(",", StringSplitOptions.RemoveEmptyEntries);
            var records = RT.Service.Resolve<ReportController>().GetReportRecordsByIds(recordIds.Select(p => double.Parse(p)).ToList(), true);

            //拆分标签不允许进行前置工序报工
            if (wipBatch.SourceNo.IsNotEmpty() && records.Any(p => p.DispatchTaskId == currTask.Id))
            {
                throw new ValidationException("任务单[{0}]已存在标签[{1}]的报工数据,请确认".L10nFormat(currTask.No, wipBatch.BatchNo));
            }

            var config = ConfigService.GetConfig(new WipPressureVerifyCodeConfig(), typeof(WipPressure));
            var isNotValidatePressureReport = config?.IsNotValidatePressureReport;
            var pressureProcess = new List<string>() { "电性能测试", "耐压测试" };

            List<string> processList = new List<string>();
            foreach (var task in taskPres)
            {
                if (isNotValidatePressureReport == true)
                {
                    if (pressureProcess.Contains(task.ProcessCode) || pressureProcess.Contains(task.ProcessName))
                        continue;
                }

                if (!records.Any(p => p.ProcessCode == task.ProcessCode))
                {
                    if (!processList.Contains(task.ProcessCode))
                        processList.Add(task.ProcessCode);
                }
            }
            if (processList.Count > 0)
                throw new ValidationException("对应工序标签[{0}]还未完成前工序[{1}]任务报工,请确认".L10nFormat(wipBatch.BatchNo, processList.Concat("、")));

        }

        /// <summary>
        /// 提交工单扫码验证
        /// </summary>
        ///  <param name="submitInfo">提交数据</param>
        [ApiService("提交工单扫码验证")]
        [ApiLog]
        public virtual string SubmitScanValid(PdaScanSubmitInfo submitInfo)
        {
            if (submitInfo.DispatchTaskId == 0)
                throw new ValidationException("请先选择任务单");

            var dispatchTask = RF.GetById<DispatchTask>(submitInfo.DispatchTaskId, new EagerLoadOptions().LoadWithViewProperty());

            var processPty = RT.Service.Resolve<ProcessPtyController>().GetProcessPtysByProcessId(dispatchTask.ProcessId.Value, dispatchTask.ProductId);

            var qty = dispatchTask.ReportQty + dispatchTask.SuspectQty + submitInfo.DetailInfos.Sum(p => p.SuspectQty) + submitInfo.DetailInfos.Sum(p => p.GoodQty);

            var maxReportQty = RT.Service.Resolve<DispatchController>().MaxReportQty(dispatchTask);

            if (qty >= dispatchTask.DispatchQty && qty < maxReportQty/*dispatchTask.MaxReportQty*/)
            {
                if (processPty.IsReportValid == true)
                {
                    var tasks = RT.Service.Resolve<DispatchController>().GetDispatchTasks(dispatchTask.WorkOrderId.Value);
                    var dic = tasks.GroupBy(p => p.Seq).ToDictionary(p => p.Key, p => p.ToList());
                    decimal lastReportQty = 0;
                    decimal lastSuspectQty = 0;
                    if (dic != null && dic.Count > 0)
                    {
                        lastReportQty = dic.OrderBy(p => p.Key).FirstOrDefault().Value.Sum(p => p.ReportQty);
                        lastSuspectQty = dic.OrderBy(p => p.Key).FirstOrDefault().Value.Sum(p => p.SuspectQty);
                    }
                    return "首工序已报工数量{0}，可疑品数量{1}，当前任务数量{2},已报工数量{3}，可疑品数量{4}，是否将任务单已完成".L10nFormat(lastReportQty, lastSuspectQty, dispatchTask?.DispatchQty ?? 0, dispatchTask?.ReportQty ?? 0, dispatchTask?.SuspectQty ?? 0);
                    //return "已报工数{0}已达到任务数量{1}是否将任务更新为已完成".L10nFormat(qty, dispatchTask.DispatchQty);
                }
            }
            return null;
        }

        /// <summary>
        /// 提交工单扫码数据
        /// </summary>
        ///  <param name="submitInfo">提交数据</param>
        [ApiService("提交工单扫码数据")]
        [ApiLog]
        public virtual List<PdaPrintInfo> SubmitScanInfo(PdaScanSubmitInfo submitInfo)
        {
            var datas = ValidationSubmitInfo(submitInfo);
            var task = datas.Item2;
            //转入模式验证标签是否已提交
            if (submitInfo.ScanType == 2)
            {
                SaveTransferData(task, submitInfo);
                return new List<PdaPrintInfo>();
            }

            var submitPdaReportInfo = GetSubmitPdaReportInfoData(submitInfo);
            submitPdaReportInfo.IsTaskFinish = submitInfo.IsTaskFinish;
            var dispatchConfig = ValidationConfigData(submitPdaReportInfo);
            EntityList<WipBatch> wipBatchs = new EntityList<WipBatch>();
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                //提交扫描记录到日志表中
                SaveReportScanLabelLog(submitInfo, task);
                //提交接口
                var wipBatchList = SubmitReportInfo(submitPdaReportInfo, task, dispatchConfig, 1, submitInfo.ResourceId);
                wipBatchs.AddRange(wipBatchList);

                tran.Complete();

            }
            var printDatas = CreatePrintDatas(true, PrintLabelType.Good, task, dispatchConfig.GoodLabel, dispatchConfig.SuspectLabel, wipBatchs);

            return printDatas;
        }

        /// <summary>
        /// 生产提交实体
        /// </summary>
        /// <param name="submitInfo"></param>
        /// <returns></returns>
        private SubmitPdaReportInfo GetSubmitPdaReportInfoData(PdaScanSubmitInfo submitInfo)
        {
            SubmitPdaReportInfo pdaReportInfo = new SubmitPdaReportInfo()
            {
                SourceType = Enums.SourceType.Report_Scan,
                IsTaskFinish = true,
                IsValidatePrepare = true,
                IsSkipValidatePreQty = submitInfo.IsSkipValidatePreQty,
                ResourceId = submitInfo.ResourceId,
                ReportEmployeeId = submitInfo.ReportEmployeeId,
                IsAutoFeeding = submitInfo.IsAutoFeeding,
                DispatchTaskId = submitInfo.DispatchTaskId,
                ReportQty = submitInfo.DetailInfos.Sum(p => p.Qty),
                GoodQty = submitInfo.DetailInfos.Sum(p => p.GoodQty),
                SuspectQty = submitInfo.DetailInfos.Sum(p => p.SuspectQty),
                ScrapQty = 0,
                ReworkQty = 0,
                SnInfos = submitInfo.DetailInfos.Select(p =>
                    new ReportSnInfo()
                    {
                        Sn = p.Sn,
                        GoodQty = p.GoodQty,
                        SuspectQty = p.SuspectQty,
                        ReworkQty = 0,
                        ScrapQty = 0,
                    }
                ).ToList()
            };
            return pdaReportInfo;
        }


        /// <summary>
        /// 验证扫描提交的信息
        /// </summary>
        /// <param name="submitInfo"></param>
        /// <exception cref="ValidationException"></exception>
        private Tuple<WorkOrder, DispatchTask> ValidationSubmitInfo(PdaScanSubmitInfo submitInfo)
        {
            if (submitInfo.DetailInfos.Count() == 0)
                throw new ValidationException("扫描列表不能为空,提交失败.".L10N());
            //获取工单信息
            var workOrder = RF.GetById<WorkOrder>(submitInfo.WorkOrderId, new EagerLoadOptions().LoadWithViewProperty());
            if (workOrder == null)
                throw new ValidationException("工单不存在,提交失败.".L10N());
            var sns = submitInfo.DetailInfos.Select(p => p.Sn).Distinct().ToList();
            if (sns.Count() != submitInfo.DetailInfos.Count())
                throw new ValidationException("提交的标签存在重复,提交失败.".L10N());
            var infos = submitInfo.DetailInfos.Where(p => p.Qty < 0).ToList();
            if (infos.Count() > 0)
                throw new ValidationException("标签[{0}]数量不能小于0,提交失败.".L10nFormat(string.Join(",", infos.Select(p => p.Sn))));
            infos = submitInfo.DetailInfos.Where(p => p.Qty > p.SuspectQty + p.GoodQty).ToList();
            //if (infos.Count > 0)
            //    throw new ValidationException("标签[{0}]的良品数+可疑品数不能小于标签数!".L10nFormat(string.Join(",", infos.Select(p => p.Sn))));
            infos = submitInfo.DetailInfos.Where(p => p.SuspectQty < 0).ToList();
            if (infos.Count() > 0)
                throw new ValidationException("标签[{0}]可疑品数量不能小于0,提交失败.".L10nFormat(string.Join(",", infos.Select(p => p.Sn))));
            infos = submitInfo.DetailInfos.Where(p => (p.SuspectQty + p.Qty) <= 0).ToList();
            if (infos.Count() > 0)
                throw new ValidationException("标签[{0}]数量+可疑品数量不能小于等于0,提交失败.".L10nFormat(string.Join(",", infos.Select(p => p.Sn))));
            //获取派工任务单信息
            var dispatchTask = RF.GetById<DispatchTask>(submitInfo.DispatchTaskId, new EagerLoadOptions().LoadWithViewProperty());
            if (dispatchTask == null)
                throw new ValidationException("派工单不存在,提交失败.".L10N());
            if (dispatchTask.TaskStatus != DispatchTaskStatus.Executing && dispatchTask.TaskStatus != DispatchTaskStatus.Dispatched)
                throw new ValidationException("只有状态为[执行中、已派工]的任务单可报工，提交失败.".L10N());
            if (dispatchTask.WorkOrderId != submitInfo.WorkOrderId)
                throw new ValidationException("任务单工单与页面工单不一致，提交失败.".L10N());
            var wipBatchs = RT.Service.Resolve<WipBatchController>().GetWipBatchs(sns);
            if (wipBatchs.Count() != submitInfo.DetailInfos.Count())
            {
                var batchNos = wipBatchs.Select(p => p.BatchNo).ToList();
                var detailInfos = submitInfo.DetailInfos.Where(p => batchNos.Contains(p.Sn) == false).ToList();
                throw new ValidationException("标签[{0}]不存在.".L10nFormat(string.Join(",", detailInfos.Select(p => p.Sn))));
            }
            var dispatchConfig = RT.Service.Resolve<DispatchController>().GetDispatchConfig();
            var processCodes = dispatchConfig?.IsAllowOverProcessCodes?.Split(",", StringSplitOptions.RemoveEmptyEntries);
            if (!processCodes.Contains(dispatchTask.Process.Code))
            {
                var detailInfoList = from a in wipBatchs
                                     join b in submitInfo.DetailInfos on a.BatchNo equals b.Sn
                                     where a.Qty < (b.GoodQty + b.SuspectQty)
                                     select b;
                if (detailInfoList.Count() > 0)
                {
                    throw new ValidationException("标签[{0}]的良品数量+可疑品数量必须等于源标签数量，提交失败.".L10nFormat(string.Join(",", detailInfoList.Select(p => p.Sn))));
                }
            }
            //校验报工方案配置, 报工数量允许大于前工序报工数量
            WorkReportPlan workReportPlan = GetWorkReportPlan(submitInfo.ProcessId);
            if (workReportPlan != null && workReportPlan.IsReportQuantity)
            {
                int seq = dispatchTask.Seq;
                int preSeq = GetTaskSeq(p => p.WorkOrderId == dispatchTask.WorkOrderId && p.ProcessId != null && p.Seq < seq && p.Seq > 0);
                if (preSeq > 0)  //找到前置工序
                {

                }
            }
            //转入模式验证标签是否已提交
            if (submitInfo.ScanType == 2)
            {
                var list = GetTransferLabelsByLabel(sns);
                if (list.Count() > 0)
                    throw new ValidationException("标签[{0}]已转入,提交失败.".L10nFormat(string.Join(",", list.Select(p => p.LabelNo))));
            }
            else
            {
                bool isExists = RT.Service.Resolve<ProcessPtyController>().GetIsTransferProcessPty(submitInfo.ProcessId, dispatchTask.ProductId);
                if (isExists)//转入工序
                {
                    var list = GetTransferLabelsByLabel(sns);
                    if (list.Count() != sns.Count())//存在未转入的标签
                    {
                        var labels = list.Select(p => p.LabelNo).ToList();
                        string label = string.Join(",", sns.Where(p => !labels.Contains(p)));
                        throw new ValidationException("标签[{0}]未转入，提交失败".L10nFormat(label));
                    }

                }
                ////验证条码标签是否已提交
                //var scanlabels = GetReportScanLabelLogsByLabel(sns);
                //if (scanlabels.Count() > 0)
                //    throw new ValidationException("标签[{0}]已提交，提交失败".L10nFormat(string.Join(",", scanlabels.Select(p => p.LabelNo))));
            }

            return new Tuple<WorkOrder, DispatchTask>(workOrder, dispatchTask);
        }

        /// <summary>
        /// 保存转入的数据
        /// </summary>
        /// <param name="dispatchTask"></param>
        /// <param name="submitInfo"></param>
        private void SaveTransferData(DispatchTask dispatchTask, PdaScanSubmitInfo submitInfo)
        {
            EntityList<ReportTransferLabel> transferLabels = new EntityList<ReportTransferLabel>();
            foreach (var item in submitInfo.DetailInfos)
            {
                ReportTransferLabel transferLabel = new ReportTransferLabel()
                {
                    DispatchTaskId = dispatchTask.Id,
                    LabelNo = item.Sn,
                    Qty = item.Qty,

                };
                transferLabels.Add(transferLabel);
            }
            RF.BatchInsert(transferLabels);
        }

        /// <summary>
        /// 保存扫描记录
        /// </summary>
        /// <param name="submitInfo"></param>
        /// <param name="dispatchTask"></param>
        private void SaveReportScanLabelLog(PdaScanSubmitInfo submitInfo, DispatchTask dispatchTask)
        {

            EntityList<ReportScanLabelLog> reportScanLabelLogs = new EntityList<ReportScanLabelLog>();
            foreach (var item in submitInfo.DetailInfos)
            {
                ReportScanLabelLog reportScanLabelLog = new ReportScanLabelLog()
                {
                    DispatchTaskId = dispatchTask.Id,
                    LabelNo = item.Sn,
                    LabelQty = item.Qty,
                    GoodQty = item.GoodQty,//item.Qty - item.SuspectQty,
                    SuspectQty = item.SuspectQty,
                    WorkOrderId = submitInfo.WorkOrderId,
                    ProcessId = submitInfo.ProcessId,

                };
                reportScanLabelLogs.Add(reportScanLabelLog);
            }
            RF.BatchInsert(reportScanLabelLogs);
        }
        #endregion


        /// <summary>
        /// 创建物料标签
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="wipBatchs"></param>
        public virtual EntityList<ItemLabel> GenerateItemLabels(double taskId, EntityList<WipBatch> wipBatchs)
        {
            var task = RF.GetById<DispatchTask>(taskId,
                new EagerLoadOptions()
                .LoadWith(DispatchTask.WorkOrderProperty)
                .LoadWith(DispatchTask.ProductProperty)
                .LoadWith(Item.UnitProperty)
                );
            var labelNos = wipBatchs.Select(p => p.BatchNo).ToList();
            var itemLabels = RT.Service.Resolve<ItemLabelController>().GetItemLabels(labelNos);
            var newLabels = new EntityList<ItemLabel>();
            foreach (var wipBatch in wipBatchs)
            {
                if (wipBatch.IsScraped || wipBatch.IsSuspectProduct == YesNo.Yes)
                    continue;
                var itemLabel = itemLabels.FirstOrDefault(p => p.Label == wipBatch.BatchNo);


                //验证条码是否存在，存在则更新，不存在则新增
                if (itemLabel == null)
                {
                    itemLabel = new ItemLabel();
                }

                itemLabel.Label = wipBatch.BatchNo;
                itemLabel.Qty = wipBatch.Qty;
                if (itemLabel.InitialQty == null || itemLabel.InitialQty == 0)
                    itemLabel.InitialQty = itemLabel.Qty;
                itemLabel.ItemId = task.ProductId;
                itemLabel.UnitId = task.Product?.UnitId;
                itemLabel.SourceType = LabelSource.BatchWip;
                itemLabel.ItemLabelState = ItemLabelState.Receive;
                itemLabel.WorkOrderId = task.WorkOrderId;
                itemLabel.FactoryId = task.FactoryId;
                itemLabel.Lot = task.WorkOrder?.BatchNo;
                itemLabel.ProductionDate = wipBatch.CreateDate;
                itemLabel.Lgort = task.WorkOrder?.Lgort;
                //itemLabel.ItemExtProp = itemExtProp;
                //itemLabel.ItemExtPropName = itemExtPropName;
                //itemLabel.ProjectNo = projectNo.IsNullOrEmpty() ? "*" : projectNo;
                //itemLabel.IsSerialNumber = RT.Service.Resolve<ItemStockBaseController>().CheckItemIsSer(item.Id);

                newLabels.Add(itemLabel);
            }
            //获取库位
            //var storageLocationCodes = newLabels.Select(p => p.Lgort).Distinct().ToList();
            //var storageLocations = RT.Service.Resolve<WarehouseController>().GetStorageLocations(storageLocationCodes, new EagerLoadOptions().LoadWithViewProperty());
            //foreach (var label in newLabels)
            //{
            //    label.StorageLocation = storageLocations.FirstOrDefault(p => p.Code == label.Lgort);
            //    label.WarehouseId = label.StorageLocation?.WarehouseId;
            //}

            RF.Save(newLabels);

            return newLabels;
        }

        /// <summary>
        /// 根据报工记录获取报工标签列表
        /// </summary>
        /// <param name="reportRecordId"></param>
        /// <returns></returns>
        public virtual EntityList<ReportWipBatch> GetReportWipBatchsByReportId(double reportRecordId)
        {
            var q = Query<ReportWipBatch>().Where(p => p.ReportRecordId == reportRecordId);
            var list = q.ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            //标注颜色
            if (list.Count > 0)
            {
                var woId = list.FirstOrDefault().ReportRecord.WorkOrder;

                var curTask = list.FirstOrDefault().ReportRecord.DispatchTask;
                var lastTasks = RT.Service.Resolve<DispatchController>().GetDispatchTasks(list.FirstOrDefault().WorkOrderId).Where(p => p.Seq > curTask.Seq).OrderBy(p => p.Seq).ToList();
                //当存在下工序任务单的时候
                if (lastTasks.Count > 0)
                {
                    //获取上工序的任务单
                    var lastDic = lastTasks.GroupBy(p => p.Seq).ToDictionary(p => p.Key, p => p.ToList());
                    var tasks = lastDic.OrderBy(p => p.Key).FirstOrDefault().Value;
                    var BatchNos = list.Select(p => p.BatchNo).Distinct().ToList();
                    var taskIds = tasks.Select(p => p.Id).Distinct().ToList();
                    //获取下工序的批次且批次号和当前批次号相同的数据
                    var lastProcessWipbatchs = Query<ReportWipBatch>().Where(p => taskIds.Contains(p.ReportRecord.DispatchTaskId) && BatchNos.Contains(p.BatchNo)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

                    if (lastProcessWipbatchs.Count > 0)
                    {
                        foreach (var l in list)
                        {
                            if (lastProcessWipbatchs.Any(p => p.BatchNo == l.BatchNo))
                                l.Color = "1";
                        }
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// 根据报工记录获取报工标签列表
        /// </summary>
        /// <param name="reportRecordIds"></param>
        /// <returns></returns>
        public virtual EntityList<ReportWipBatch> GetReportWipBatchsByReportIds(List<double> reportRecordIds)
        {
            var reportWipBatches = reportRecordIds.SplitContains(ids =>
             {
                 var q = Query<ReportWipBatch>().Where(p => ids.Contains(p.ReportRecordId));
                 var list = q.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                 return list;
             });
            return reportWipBatches;
        }

        /// <summary>
        /// 根据报工标签获取报工标签列表
        /// </summary>
        /// <param name="wipBatchId"></param>
        /// <returns></returns>
        public virtual EntityList<ReportWipBatch> GetReportWipBatchsByWipBatchId(double wipBatchId)
        {
            var q = Query<ReportWipBatch>().Where(p => p.WipBatchId == wipBatchId);
            var list = q.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        /// 根据标签号获取报工标签
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        public virtual EntityList<ReportWipBatch> GetReportWipBatchesBySn(string sn)
        {
            var list = Query<ReportWipBatch>().Where(p => p.BatchNo == sn).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        /// 更新任务单可疑品数
        /// </summary>
        /// <param name="taskId"></param>
        public virtual void UpdateSuspectQty(double taskId)
        {
            using (var db = DB.Create(TaskManagementEntityDataProvider.ConnectionStringName))
            {
                //可疑品数 = SUM(标签数量) - SUM(良品) - SUM(废品) - SUM(返工数)
                var sql = $@"UPDATE TM_DISP_TASK A
   SET A.SUSPECT_QTY =
       (SELECT NVL(SUM(T.QTY) - SUM(T.GOOD_QTY) - SUM(T.SCRAP_QTY) - SUM(T.REPAIR_QTY), 0) QTY
          FROM WIP_SUSPECT_PROD_LABEL T
         WHERE T.DISPATCH_TASK_ID = {taskId})
 WHERE A.ID = {taskId}";

                var ret = db.ExecuteNonQuery(sql, System.Data.CommandType.Text);
            }
        }

        /// <summary>
        /// 导出报工记录审核数据为Excel（高性能版本）
        /// 按月导出和大数据量导出专用接口，避免LoadWithViewProperty的性能问题
        /// </summary>
        /// <param name="criteria">查询条件</param>
        /// <returns>报工记录审核列表</returns>
        [ApiService("导出报工记录审核")]
        [return: ApiReturn("报工记录审核列表")]
        public virtual EntityList<ReportRecordExamine> ExportReportRecordExamine([ApiParameter("查询条件")] ReportRecordExamineCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ValidationException("查询条件不能为空".L10N());
            }

            // 导出场景必须有时间范围限制
            if (!criteria.ReportTime.BeginValue.HasValue || !criteria.ReportTime.EndValue.HasValue)
            {
                throw new ValidationException("导出必须选择时间范围".L10N());
            }

            // 计算时间跨度，限制单次导出范围（最多3个月）
            var timeSpan = criteria.ReportTime.EndValue.Value - criteria.ReportTime.BeginValue.Value;
            if (timeSpan.TotalDays > 90)
            {
                throw new ValidationException("导出时间范围不能超过3个月".L10N());
            }

            // 使用优化的查询方法
            return QueryReportRecordExamineForExport(criteria);
        }
    }
}