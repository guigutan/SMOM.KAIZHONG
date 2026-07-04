using Newtonsoft.Json;
using SIE.Core.Logs;
using SIE.Domain;
using SIE.EventMessages.Release;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Reports;
using SIE.MES.WorkOrders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.TaskManagement.Interfaces
{
    /// <summary>
    /// APS任务拆分MES任务单控制器
    /// </summary>
    public class ApsTaskController : DomainController, IWorkOrderTask
    {
        /// <summary>
        /// APS拆分前获取MES.TaskManagement工单生成的任务单列表
        /// </summary>
        /// <param name="workOrderInfos">工单信息列表</param>
        /// <returns>任务单列表</returns>
        public virtual IReadOnlyList<DispatchTaskInfo> WorkOrderTask(IReadOnlyList<WorkOrderInfo> workOrderInfos)
        {
            SaveWorkOrderTaskLog(workOrderInfos);
            var woIds = workOrderInfos.Select(p => p.WorkOrderId).Distinct().ToList();

            return CreateDispatchTaskInfos(woIds);
        }

        /// <summary>
        /// 保存APS拆分前获取MES.TaskManagement工单生成的任务单列表日志
        /// </summary>
        /// <param name="workOrderInfos">工单信息列表</param>
        private void SaveWorkOrderTaskLog(IReadOnlyList<WorkOrderInfo> workOrderInfos)
        {
            using (var tran = DB.AutonomousTransactionScope(TaskManagementEntityDataProvider.ConnectionStringName))
            {
                var strValue = JsonConvert.SerializeObject(workOrderInfos);
                var inputValue = "工单信息列表:{0}".L10nFormat(strValue);
                var log = new InterfaceLog()
                {
                    Name = "IWorkOrderTask",
                    Method = "WorkOrderTask",
                    ControllerName = "ApsTaskController",
                    InputValue = inputValue,
                };

                RF.Save(log);
                tran.Complete();
            }
        }

        /// <summary>
        /// 创建任务单信息列表
        /// </summary>
        /// <param name="woIds">工单Id列表</param>
        /// <returns>任务单信息列表</returns>
        private List<DispatchTaskInfo> CreateDispatchTaskInfos(List<double> woIds)
        {
            List<DispatchTaskInfo> taskInfoList = new List<DispatchTaskInfo>();
            woIds.SplitDataExecute(tempIds =>
            {
                var normalTasks = Query<DispatchTask>()
                         .Join<WorkOrder>((dispatchTask, wo) => dispatchTask.WorkOrderId != null
                            && tempIds.Contains(wo.Id)
                            && dispatchTask.WorkOrderId == wo.Id
                            && wo.State != Core.WorkOrders.WorkOrderState.Close
                            && wo.State != Core.WorkOrders.WorkOrderState.Finish
                            && dispatchTask.MergedStatus != MergedStatus.MergeRows
                            && dispatchTask.IsMainTask)
                          .Select<WorkOrder>((dispatchTask, wo) => new
                          {
                              WorkOrderId = wo.Id,
                              DispatchTaskId = dispatchTask.Id,
                              No = dispatchTask.No,
                              TaskStatus = (int)dispatchTask.TaskStatus,
                              MergedStatus = (int)dispatchTask.MergedStatus
                          }).ToList<DispatchTaskInfo>();

                if (normalTasks.Any())
                {
                    taskInfoList.AddRange(normalTasks);
                }


                var mergeRowTasks = Query<DispatchTask>().As("dispatchTask")
                    .Join<AssociatedTask>((dispatchTask, associatedTask) => dispatchTask.Id == associatedTask.DispatchTaskId
                        && dispatchTask.MergedStatus == MergedStatus.MergeRows
                        && dispatchTask.IsMainTask)
                      .Join<AssociatedTask, DispatchTask>("task", (associatedTask, task) => associatedTask.TaskId == task.Id)
                      .Join<DispatchTask, WorkOrder>((task, wo) => task.WorkOrderId == wo.Id
                        && tempIds.Contains(wo.Id)
                        && task.WorkOrderId != null && wo.State != Core.WorkOrders.WorkOrderState.Close
                        && wo.State != Core.WorkOrders.WorkOrderState.Finish)
                      .Select<AssociatedTask, DispatchTask, WorkOrder>(
                      (dispatchTask, associatedTask, task, wo) => new
                      {
                          WorkOrderId = wo.Id,
                          DispatchTaskId = dispatchTask.Id,
                          No = dispatchTask.No,
                          TaskStatus = (int)dispatchTask.TaskStatus,
                          MergedStatus = (int)dispatchTask.MergedStatus
                      }).ToList<DispatchTaskInfo>();

                if (mergeRowTasks.Any())
                {
                    var taskIds = mergeRowTasks.Select(p => p.DispatchTaskId).Distinct().ToList();
                    var dicMergedDispatchTasks = RT.Service.Resolve<DispatchController>().GetMergedTasks(taskIds);
                    foreach (var mergeRowTask in mergeRowTasks)
                    {
                        List<DispatchTask> dispatchTasks = null;
                        if (dicMergedDispatchTasks.TryGetValue(mergeRowTask.DispatchTaskId, out dispatchTasks) && dispatchTasks.Select(p => p.WorkOrderId).Distinct().Count() > 1)
                        {
                            taskInfoList.Add(mergeRowTask);
                        }
                    }
                }
            });

            return taskInfoList;
        }

        /// <summary>
        /// 获取工单任务单信息集合
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        public List<DispatchTaskInfo> GetWorkOrderDispatchTasks(double workOrderId)
        {
            var list = new List<DispatchTaskInfo>();
            var result = Query<DispatchTask>().Where(m => m.WorkOrderId == workOrderId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            foreach (var res in result)
            {
                list.Add(new DispatchTaskInfo()
                {
                    WorkOrderId = res.WorkOrderId.Value,
                    DispatchTaskId = res.Id,
                    No = res.No,
                    MergedStatus = (int)res.MergedStatus,
                    TaskStatus = (int)res.TaskStatus,
                });
            }
            return list;
        }
    }
}
