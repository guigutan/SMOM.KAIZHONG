using SIE.Api;
using SIE.Core;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Inventory.Task
{
    /// <summary>
    /// 任务管理器控制器
    /// </summary>
    public partial class TaskController
    {
        #region 私有方法
        /// <summary>
        /// 获取完工任务所有操作人个数
        /// </summary>
        /// <param name="fromWarehouseId">仓库Id</param>
        /// <param name="date">日期</param>
        /// <returns>当天完工任务所有操作人个数</returns>
        private int GetActualOperatorQty(double fromWarehouseId, DateTime date)
        {
            var query = Query<ActualOperator>().Select(p => p.EmployeeId);
            query.Exists<TaskManagement>((x, y) => y.Where(p => p.Id == x.TaskManagementId
            && p.FromWarehouseId == fromWarehouseId && p.State == TaskState.Finish
            && p.EndDate >= date && p.EndDate < date.AddDays(1)));
            return query.Distinct().ToList<double>().Count();
        }

        /// <summary>
        /// 获取某天的完工任务数
        /// </summary>
        /// <param name="fromWarehouseId">仓库ID</param>
        /// <param name="date">日期</param>
        /// <param name="isByUser">是否关联实际操作人</param>
        /// <returns>完工任务数</returns>
        private EntityList<TaskManagement> GetFinishTasks(double fromWarehouseId, DateTime date, bool isByUser)
        {
            //获取所有指定操作人的任务集合
            var query = Query<TaskManagement>();
            query.Where(p => p.State == TaskState.Finish);
            query.Where(p => p.EndDate >= date && p.EndDate < date.AddDays(1));
            if (fromWarehouseId > 0)
            {
                query.Where(p => p.FromWarehouseId == fromWarehouseId);
            }

            if (isByUser)
            {
                query.Exists<ActualOperator>((a, b) => b.Join<Employee>((c, d) => c.EmployeeId == d.Id && d.Id == RT.IdentityId).Where(p => p.TaskManagementId == a.Id && p.IsMaster));
            }

            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取当前登录用户当天任务
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="page">分页信息</param>
        /// <param name="action">查询条件委托</param>
        /// <returns>当前登录用户任务</returns>
        private EntityList<TaskManagement> GetTasks(double warehouseId, PagingInfo page, Action<IEntityQueryer<TaskManagement>> action = null)
        {
            //获取所有指定操作人的任务集合
            var query = Query<TaskManagement>();

            //调拨来源仓跟目标仓可能会不一样。如果操作类型不是调拨，则取来源仓；如果是调拨，则根据事务类型
            if (warehouseId > 0)
                query.Where(p => (p.OperationType != OperationType.Allot && p.FromWarehouseId == warehouseId) ||
                (p.OperationType == OperationType.Allot && p.TransactionType == Transactions.TransactionType.Allocate && (p.ToWarehouseId == warehouseId || p.FromWarehouseId == warehouseId)));

            //委托，可变查询条件
            action?.Invoke(query);
            return query.OrderByDescending(p => p.Level).OrderByDescending(p=>p.Id).ToList(page, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取任务数
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="action">委托，可变查询条件</param>
        /// <returns>任务数</returns>
        private int GetTaskCount(double warehouseId, Action<IEntityQueryer<TaskManagement>> action = null)
        {
            //获取所有指定操作人的任务集合
            var query = Query<TaskManagement>();

            //调拨来源仓跟目标仓可能会不一样。如果操作类型不是调拨，则取来源仓；如果是调拨，则根据事务类型
            if (warehouseId > 0)
                query.Where(p => (p.OperationType != OperationType.Allot && p.FromWarehouseId == warehouseId) ||
                (p.OperationType == OperationType.Allot && p.TransactionType == Transactions.TransactionType.Allocate && (p.ToWarehouseId == warehouseId || p.FromWarehouseId == warehouseId)));


            //委托，可变查询条件
            action?.Invoke(query);
            return query.Count();
        }

        /// <summary>
        /// 获取评分
        /// </summary>
        /// <param name="personalQty">个人数</param>
        /// <param name="avgQty">平均数</param>
        /// <returns>分数</returns>
        private int GetScore(decimal personalQty, decimal avgQty)
        {
            int score = 0;
            if (avgQty == 0)
                return 0;

            var qty = personalQty - avgQty;
            if (qty >= avgQty * (decimal)0.3)
            {
                score = 5;
            }
            else if (qty > 0)
            {
                score = 4;
            }
            else if (qty == 0)
            {
                score = 3;
            }
            else if (avgQty * (decimal)0.2 >= -qty)
            {
                score = 2;
            }
            else
            {
                score = 1;
            }

            return score;
        }

        /// <summary>
        /// 创建任务列表
        /// </summary>
        /// <param name="result">任务列表</param>
        /// <param name="tasks">任务信息</param>
        /// <param name="empName">执行人</param>
        private void CreateAllTaskDataList(AllTaskDataList result, EntityList<TaskManagement> tasks, string empName)
        {
            tasks.ForEach(e =>
            {
                var data = new AllTaskData();
                data.TaskId = e.Id;
                data.TaskNo = e.No;
                data.BillNo = e.BillNo;
                data.ReleaseDate = e.ReleaseDate.Value.ToString(DateTimeFormat.MMddHHmm);
                data.ItemCode = e.ItemCode;
                data.ItemName = e.ItemName;
                data.UnitCode = e.ItemUnitName;
                data.Qty = e.Qty;
                data.Lot = e.SuggestLotCode;
                data.FromLocCode = e.SuggestFromLocCode;
                data.FromLpn = e.SuggestFromLpn;
                data.ToLocCode = e.SuggestToLocCode;
                data.ToLpn = e.SuggestToLpn;
                data.Level = (int)e.Level;
                data.LevelDesc = e.Level.ToLabel().L10N();
                data.OperationType = e.OperationType.ToLabel().L10N();
                data.Operator = empName;
                result.TaskList.Add(data);
            });
        }
        #endregion

        /// <summary>
        /// 领取任务
        /// </summary>
        /// <param name="taskId">根据任务Id领取任务</param>
        /// <param name="IsCheckSecond">是否校验二级明细号能否为空</param>
        [ApiService("领取任务")]
        public virtual TaskManagement ExecutingTask([ApiParameter("任务Id")] double taskId, [ApiParameter("校验分配ID或LPN")] bool IsCheckSecond)
        {
            try
            {
                using (var tran = DB.TransactionScope(InveEntityDataProvider.ConnectionStringName))
                {
                    DB.Update<TaskManagement>().Where(t => t.Id == taskId).Execute();
                    var task = RF.GetById<TaskManagement>(taskId);
                    if (task != null)
                    {
                        if (task.State == TaskState.Finish || task.State == TaskState.AutoFinish)
                        {
                            throw new ValidationException("任务[{0}]状态[{1}]已完工或自动完工，无法再领取！".L10nFormat(task.No, task.State.ToLabel()));
                        }
                        if (IsCheckSecond)
                        {
                            if (task.SecondBillDtlNo.IsNotEmpty())
                            {
                                throw new ValidationException("二级明细行号不是空".L10N());
                            }
                        }
                        if (task.State != TaskState.Release && task.GetById.HasValue && task.GetById > 0)
                        {
                            if (task.GetById != RT.IdentityId)
                                throw new ValidationException("任务[{0}]已经被[{1}]领取,无法再领取！".L10nFormat(task.No, task.GetBy.Name));
                        }
                        else
                        {
                            ValidateTask(task);

                            UpdateTaskState(task, TaskState.Executing,
                                e =>
                                {
                                    e.BeginDate = DateTime.Now;
                                    e.GetById = RT.IdentityId;

                                    if (e.OperatorList.Any(p => p.EmployeeId == RT.IdentityId))
                                    {
                                        e.OperatorList.Where(p => p.EmployeeId == RT.IdentityId).ForEach(p => p.IsMaster = true);
                                    }
                                    else
                                    {
                                        e.OperatorList.ForEach(p => p.IsMaster = false);
                                        e.OperatorList.Add(new Operator() { EmployeeId = RT.IdentityId, IsMaster = true });
                                    }
                                });
                        }
                    }

                    tran.Complete();
                    return task;
                }
            }
            catch (Exception ex)
            {
                var baseEx = ex.GetBaseException();
                if (baseEx is ValidationException)
                    throw new ValidationException((baseEx as ValidationException).Message);
                else
                    throw new ValidationException("领取任务失败,请检查数据后再操作!".L10N());
            }
        }

        /// <summary>
        /// 领取任务
        /// </summary>
        /// <param name="taskIds">根据任务Id领取任务</param>
        [ApiService("领取多个任务")]
        public virtual List<TaskManagement> ExecutingTasks([ApiParameter("任务Id")] List<double> taskIds)
        {
            try
            {
                using (var tran = DB.TransactionScope(InveEntityDataProvider.ConnectionStringName))
                {
                    DB.Update<TaskManagement>().Where(t => taskIds.Contains(t.Id)).Execute();
                    var tasks = RT.Service.Resolve<TaskController>().GetTaskManagements(taskIds);
                    if (tasks.Count != taskIds.Count)
                        throw new ValidationException("任务不存在".L10N());

                    tasks.ForEach(task =>
                    {
                        if (task != null)
                        {
                            if (task.State != TaskState.Release && task.GetById.HasValue && task.GetById > 0)
                            {
                                if (task.GetById != RT.IdentityId)
                                    throw new ValidationException("任务[{0}]已经被[{1}]领取,无法再领取！".L10nFormat(task.No, task.GetBy.Name));
                            }
                            else
                            {
                                ValidateTask(task);

                                UpdateTaskState(task, TaskState.Executing,
                                    e =>
                                    {
                                        e.BeginDate = DateTime.Now;
                                        e.GetById = RT.IdentityId;

                                        if (e.OperatorList.Any(p => p.EmployeeId == RT.IdentityId))
                                        {
                                            e.OperatorList.Where(p => p.EmployeeId == RT.IdentityId).ForEach(p => p.IsMaster = true);
                                        }
                                        else
                                        {
                                            e.OperatorList.ForEach(p => p.IsMaster = false);
                                            e.OperatorList.Add(new Operator() { EmployeeId = RT.IdentityId, IsMaster = true });
                                        }

                                    });
                            }
                        }
                    });

                    tran.Complete();
                    return tasks.ToList();
                }
            }
            catch (Exception ex)
            {
                var baseEx = ex.GetBaseException();
                if (baseEx is ValidationException)
                    throw new ValidationException((baseEx as ValidationException).Message);
                else
                    throw new ValidationException("领取任务失败,请检查数据后再操作!".L10N());
            }
        }

        /// <summary>
        /// 释放任务
        /// </summary>
        /// <param name="taskGroupId">根据任务组ID释放任务</param>
        [ApiService("释放任务")]
        public virtual void ReleaseTaskDataByTaskGroup([ApiParameter("任务组ID")] double taskGroupId)
        {
            var tasks = RT.Service.Resolve<TaskController>().GetTaskManagements(taskGroupId, OperationType.Check, TaskState.Executing, RT.IdentityId);
            ReleaseTasks(tasks);
        }

        /// <summary>
        /// 领取任务
        /// </summary>
        /// <param name="taskGroupId">根据任务组ID领取任务</param>
        [ApiService("领取任务")]
        public virtual void ExecuteTaskDataByTaskGroup([ApiParameter("任务组ID")] double taskGroupId)
        {
            var tasks = RT.Service.Resolve<TaskController>().GetTaskManagements(taskGroupId, OperationType.Check, null, null);
            ExcuteTasks(tasks);
        }

        /// <summary>
        /// 释放任务
        /// </summary>
        /// <param name="taskIds">根据任务Id集合释放任务</param>
        [ApiService("释放任务")]
        public virtual void ReleaseTaskData([ApiParameter("任务Id集合")] List<double> taskIds)
        {
            ReleaseTasks(taskIds);
        }

        /// <summary>
        /// 挂起任务
        /// </summary>
        /// <param name="taskId">任务id</param>
        [ApiService("挂起任务")]
        public virtual void HangUpTask([ApiParameter("任务Id")] double taskId)
        {
            var task = RF.GetById<TaskManagement>(taskId);
            if (task != null)
            {
                ValidateTask(task);
                UpdateTaskState(task, TaskState.HangUp);
            }
        }

        /// <summary>
        /// 异常反馈任务
        /// </summary>
        /// <param name="taskId">任务Id</param>
        /// <param name="reason">异常原因</param>
        [ApiService("异常反馈")]
        public virtual void AbnormalTask([ApiParameter("任务Id")] double taskId, [ApiParameter("异常原因")] string reason)
        {
            var task = RF.GetById<TaskManagement>(taskId);
            if (task != null)
            {
                ValidateTask(task);
                UpdateTaskState(task, TaskState.Abnormal, e => { e.StateDesc = reason; });
            }
        }

        /// <summary>
        /// 获取当前登录用户当天待处理任务
        /// </summary>
        /// <param name="fromWarehouseId">仓库Id</param>
        /// <param name="pageNumber">页号</param>
        /// <param name="pageSize">页码</param>
        /// <returns>任务列表</returns>
        [ApiService("任务列表：获取当前登录用户当天待处理任务")]
        [return: ApiReturn("返回待处理任务：AllTaskDataList")]
        public virtual AllTaskDataList GetToDoTasks([ApiParameter("仓库Id")] double fromWarehouseId, [ApiParameter("页号")] int pageNumber, [ApiParameter("页码")] int pageSize)
        {
            var result = new AllTaskDataList();
            var tasks = GetTasks(fromWarehouseId, new PagingInfo(pageNumber, pageSize), e =>
            {
                e.Where(p => p.State == TaskState.Release || p.State == TaskState.Appoint || p.State == TaskState.Executing);
                e.Exists<Operator>((a, b) => b.Join<Employee>((c, d) => c.EmployeeId == d.Id && d.Id == RT.IdentityId).Where(p => p.TaskManagementId == a.Id && p.IsMaster));
            });

            result.TaskCount = GetTaskCount(fromWarehouseId, e =>
             {
                 e.Where(p => p.State == TaskState.Release || p.State == TaskState.Appoint || p.State == TaskState.Executing);
                 e.Exists<Operator>((a, b) => b.Join<Employee>((c, d) => c.EmployeeId == d.Id && d.Id == RT.IdentityId).Where(p => p.TaskManagementId == a.Id && p.IsMaster));
             });
            var empName = string.Empty;
            if (tasks.Count > 0)
            {
                empName = tasks.FirstOrDefault().OperatorList.FirstOrDefault(p => p.IsMaster)?.Employee.Name;
            }

            CreateAllTaskDataList(result, tasks, empName);
            return result;
        }

        /// <summary>
        /// 获取当前登录用户当天已完成任务
        /// </summary>
        /// <param name="fromWarehouseId">仓库Id</param>
        /// <param name="pageNumber">页号</param>
        /// <param name="pageSize">页码</param>
        /// <returns>任务列表</returns>
        [ApiService("任务列表：获取当前登录用户当天已完成任务")]
        [return: ApiReturn("返回已完成任务：AllTaskDataList")]
        public virtual AllTaskDataList GetFinishedTasks([ApiParameter("仓库Id")] double fromWarehouseId, [ApiParameter("页号")] int pageNumber, [ApiParameter("页码")] int pageSize)
        {
            var result = new AllTaskDataList();
            var tasks = GetTasks(fromWarehouseId, new PagingInfo(pageNumber, pageSize), e =>
            {
                e.Where(p => p.State == TaskState.Finish);
                e.Where(p => p.EndDate >= DateTime.Today);
                e.Exists<ActualOperator>((a, b) => b.Join<Employee>((c, d) => c.EmployeeId == d.Id && d.Id == RT.IdentityId).Where(p => p.TaskManagementId == a.Id && p.IsMaster));
            });

            result.TaskCount = GetTaskCount(fromWarehouseId, e =>
             {
                 e.Where(p => p.State == TaskState.Finish);
                 e.Where(p => p.EndDate >= DateTime.Today);
                 e.Exists<ActualOperator>((a, b) => b.Join<Employee>((c, d) => c.EmployeeId == d.Id && d.Id == RT.IdentityId).Where(p => p.TaskManagementId == a.Id && p.IsMaster));
             });

            var empName = string.Empty;
            if (tasks.Count > 0)
            {
                empName = tasks.FirstOrDefault().ActualOperatorList.FirstOrDefault(p => p.IsMaster)?.Employee.Name;
            }

            CreateAllTaskDataList(result, tasks, empName);

            return result;
        }

        /// <summary>
        /// 获取所有未指定操作人的可领取任务
        /// </summary>
        /// <param name="fromWarehouseId">仓库ID</param>
        /// <param name="pageNumber">页号</param>
        /// <param name="pageSize">页码</param>
        /// <returns>返回所有未指定操作人的可领取任务:AllTaskDataList</returns>
        [ApiService("任务列表：获取所有未指定操作人的可领取任务")]
        [return: ApiReturn("返回可领取任务：AllTaskDataList")]
        public virtual AllTaskDataList GetCanExcuteTasks([ApiParameter("仓库Id")] double fromWarehouseId, [ApiParameter("页号")] int pageNumber, [ApiParameter("页码")] int pageSize)
        {
            var result = new AllTaskDataList();
            var tasks = GetTasks(fromWarehouseId, new PagingInfo(pageNumber, pageSize), e =>
             {
                 e.Where(p => p.State == TaskState.Release);
                 e.NotExists<Operator>((a, b) => b.Where(t => t.TaskManagementId == a.Id));
             });

            result.TaskCount = GetTaskCount(fromWarehouseId, e =>
             {
                 e.Where(p => p.State == TaskState.Release);
                 e.NotExists<Operator>((a, b) => b.Where(t => t.TaskManagementId == a.Id));
             });

            var empName = string.Empty;
            if (tasks.Count > 0)
            {
                empName = tasks.FirstOrDefault().ActualOperatorList.FirstOrDefault(p => p.IsMaster)?.Employee.Name;
            }

            CreateAllTaskDataList(result, tasks, empName);

            return result;
        }

        /// <summary>
        /// 获取工作统计信息
        /// </summary>
        /// <param name="fromWarehouseId">仓库ID</param>
        /// <param name="date">日期</param>
        /// <returns>工作统计信息</returns>
        [ApiService("工作统计-获取工作统计信息")]
        [return: ApiReturn("返回获取工作统计信息：StatisticsTaskData")]
        public virtual StatisticsTaskData GetStatisticsTaskData([ApiParameter("仓库Id")] double fromWarehouseId, [ApiParameter("日期(yyyy-MM-dd)")] string date)
        {
            var result = new StatisticsTaskData();
            DateTime dataDate;
            if (!DateTime.TryParse(date, out dataDate))
            {
                throw new ValidationException("日期[{0}]格式不正确".L10nFormat(date));
            }

            //个人完成任务
            var personalTasks = GetFinishTasks(fromWarehouseId, dataDate, true);

            //所有完成任务
            var allTasks = GetFinishTasks(fromWarehouseId, dataDate, false);

            if (allTasks.Count == 0)
                return null;

            var empQty = GetActualOperatorQty(fromWarehouseId, dataDate);
            double score = 0;

            //工作数量统计
            var item1 = new StatisticsData();
            item1.ProjectType = 1;
            item1.Project = "工作数量".L10N();
            item1.PersonalQty = personalTasks.Count;
            item1.SumQty = allTasks.Count;
            item1.AvgQty = Math.Round(item1.SumQty / empQty, 1);
            result.StatisticsDataList.Add(item1);
            score += GetScore(item1.PersonalQty, item1.AvgQty) * 0.4;

            //工作时长统计
            var item2 = new StatisticsData();
            item2.ProjectType = 2;
            item2.Project = "任务时长".L10N();
            item2.PersonalQty = personalTasks.Sum(p => Convert.ToInt16((p.EndDate.Value - p.BeginDate.Value).TotalMinutes));
            item2.SumQty = allTasks.Sum(p => Convert.ToInt16((p.EndDate.Value - p.BeginDate.Value).TotalMinutes));
            item2.AvgQty = Math.Round(item2.SumQty / empQty, 1);
            result.StatisticsDataList.Add(item2);
            score += GetScore(item2.PersonalQty, item2.AvgQty) * 0.4;

            //操作数量统计
            var item3 = new StatisticsData();
            item3.ProjectType = 3;
            item3.Project = "操作数量".L10N();
            item3.PersonalQty = personalTasks.Sum(p => p.Qty);
            item3.SumQty = allTasks.Sum(p => p.Qty);
            item3.AvgQty = Math.Round(item3.SumQty / empQty, 1);
            result.StatisticsDataList.Add(item3);
            score += GetScore(item3.PersonalQty, item3.AvgQty) * 0.2;

            result.Score = (int)Math.Round(score, 0);
            return result;
        }
    }
}
