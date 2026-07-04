using SIE.Common;
using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.Domain;
using SIE.Common.NumberRules;
using SIE.Core.Common;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Inventory.Strategy;
using SIE.Inventory.Task.Configs;
using SIE.Inventory.Transactions;
using SIE.Items;
using SIE.Items.Items;
using SIE.Resources.Employees;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Inventory.Task
{
    /// <summary>
    /// 任务管理器控制器
    /// </summary>
    /// <seealso cref="SIE.DomainController" />
    public partial class TaskController : DomainController
    {
        /// <summary>
        /// 获取任务号
        /// </summary>
        /// <returns>任务号</returns>
        /// <exception cref="ValidationException">未找到任务号生成规则,请检查规则配置</exception>
        public virtual string GetTaskNo()
        {
            var config = ConfigService.GetConfig(new NoConfig(), typeof(TaskManagement));
            if (config == null || config.BacodeRule == null)
                throw new ValidationException("未找到任务号生成规则,请检查规则配置".L10N());
            return RT.Service.Resolve<NumberRuleController>()
                .GenerateSegment(config.BacodeRule.Id, 1)
                .FirstOrDefault();
        }

        /// <summary>
        /// 根据数量生产任务号
        /// </summary>
        /// <param name="qty">数量</param>
        /// <returns>任务号集合</returns>
        public virtual List<string> GetTaskNos(int qty)
        {
            var config = ConfigService.GetConfig(new NoConfig(), typeof(TaskManagement));
            if (config == null || config.BacodeRule == null)
                throw new ValidationException("未找到任务号生成规则,请检查规则配置".L10N());
            return RT.Service.Resolve<NumberRuleController>()
                .GenerateSegment(config.BacodeRule.Id, qty).ToList();
        }

        /// <summary>
        /// 获取任务仓库最大加急数
        /// </summary>
        /// <returns>仓库最大加急数</returns>
        /// <exception cref="ValidationException">未找到任务仓库最大加急数配置,请检查参数配置</exception>
        public virtual int GetTaskUrgentMaxCount()
        {
            var config = ConfigService.GetConfig(new TaskParameterConfig(), typeof(TaskManagement));
            if (config == null)
                throw new ValidationException("未找到任务仓库最大加急数配置,请检查参数配置".L10N());
            return config.UrgentMaxCount;
        }

        /// <summary>
        /// 根据任务ID集合获取任务集合
        /// </summary>
        /// <param name="taskIdList">任务ID集合</param>
        /// <param name="action">查询条件委托</param>
        /// <returns>任务集合</returns>
        public virtual EntityList<TaskManagement> GetTaskManagements(List<double> taskIdList, Action<IEntityQueryer<TaskManagement>> action = null)
        {
            EntityList<TaskManagement> results = new EntityList<TaskManagement>();
            taskIdList = taskIdList.Distinct().ToList();
            DataProcessEx.SplitDataExecute(taskIdList, sons =>
            {
                var query = Query<TaskManagement>();
                action?.Invoke(query);
                query.Where(p => sons.Contains(p.Id)).ToList();
                var list = query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                results.AddRange(list);
            });
            return results;
        }

        /// <summary>
        /// 根据任务ID集合获取任务集合
        /// </summary>
        /// <param name="taskIdList">任务ID集合</param>
        /// <param name="action">查询条件委托</param>
        /// <returns>任务集合</returns>
        public virtual EntityList<TaskManagement> GetTaskManagementsNoEager(List<double> taskIdList, Action<IEntityQueryer<TaskManagement>> action = null)
        {
            EntityList<TaskManagement> results = new EntityList<TaskManagement>();
            taskIdList = taskIdList.Distinct().ToList();
            DataProcessEx.SplitDataExecute(taskIdList, sons =>
            {
                var query = Query<TaskManagement>();
                action?.Invoke(query);
                query.Where(p => sons.Contains(p.Id)).ToList();
                var list = query.ToList();
                results.AddRange(list);
            });
            return results;
        }


        /// <summary>
        /// 根据单据类型、任务来源ID集合获取任务集合信息
        /// </summary>
        /// <param name="orderType">订单类型</param>
        /// <param name="taskSourceListId">任务来源ID（最终产生任务的来源ID）</param>
        /// <param name="action">查询条件委托</param>
        /// <returns>任务集合列表</returns>
        public virtual EntityList<TaskManagement> GetTaskManagements(OrderType orderType, List<double> taskSourceListId, Action<IEntityQueryer<TaskManagement>> action = null)
        {
            EntityList<TaskManagement> results = new EntityList<TaskManagement>();
            taskSourceListId = taskSourceListId.Distinct().ToList();
            DataProcessEx.SplitDataExecute(taskSourceListId, sons =>
            {
                var query = Query<TaskManagement>();
                query.Where(p => p.OrderType == orderType);
                action?.Invoke(query);
                var list = query.Where(p => sons.Contains(p.TaskSourceId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                results.AddRange(list);
            });
            return results;
        }

        /// <summary>
        /// 根据单据类型、任务来源ID集合获取任务集合信息
        /// </summary>
        /// <param name="orderType">订单类型</param>
        /// <param name="billDtlIdList">单据明细ID集合</param>
        /// <param name="taskSourceListId">任务来源ID（最终产生任务的来源ID）</param>
        /// <param name="action">查询条件委托</param>
        /// <returns>任务集合列表</returns>
        public virtual EntityList<TaskManagement> GetTaskManagementsNoEager(OrderType orderType, List<double> billDtlIdList, List<double> taskSourceListId, Action<IEntityQueryer<TaskManagement>> action = null)
        {
            EntityList<TaskManagement> results = new EntityList<TaskManagement>();
            taskSourceListId = taskSourceListId.Distinct().ToList();
            DataProcessEx.SplitDataExecute(taskSourceListId, sons =>
            {
                var query = Query<TaskManagement>();
                query.Where(p => billDtlIdList.Contains(p.BillDtlId));
                query.Where(p => p.OrderType == orderType);
                action?.Invoke(query);
                var list = query.Where(p => sons.Contains(p.TaskSourceId)).ToList();
                results.AddRange(list);
            });
            return results;
        }

        /// <summary>
        /// 根据单据类型、任务来源ID集合获取任务集合信息
        /// </summary>
        /// <param name="orderType">订单类型</param>
        /// <param name="billDtlIdList">单据明细ID集合</param>
        /// <param name="taskSourceListId">任务来源ID（最终产生任务的来源ID）</param>        
        /// <returns>任务集合列表</returns>
        public virtual EntityList<TaskManagement> GetTaskManagementsNoEager(OrderType orderType, List<double> billDtlIdList, List<double> taskSourceListId)
        {
            EntityList<TaskManagement> results = new EntityList<TaskManagement>();
            taskSourceListId = taskSourceListId.Distinct().ToList();
            DataProcessEx.SplitDataExecute(taskSourceListId, sons =>
            {
                var query = Query<TaskManagement>();
                query.Where(p => billDtlIdList.Contains(p.BillDtlId));
                query.Where(p => p.OrderType == orderType);
                var list = query.Where(p => sons.Contains(p.TaskSourceId)).ToList(null, null);
                results.AddRange(list);
            });
            return results;
        }

        /// <summary>
        /// 获取任务
        /// </summary>
        /// <param name="taskGroupId">任务组</param>
        /// <param name="taskState">任务状态</param>
        /// <param name="operationType">操作类型</param>
        /// <param name="getById">任务领取人</param>
        /// <returns>任务</returns>
        public virtual EntityList<TaskManagement> GetTaskManagements(double taskGroupId, OperationType? operationType, TaskState? taskState = null, double? getById = null)
        {
            var query = Query<TaskManagement>().Where(p => p.TaskGroupId == taskGroupId);
            if (operationType.HasValue)
                query.Where(p => p.OperationType == operationType);
            if (taskState.HasValue)
                query.Where(p => p.State == taskState);
            if (getById.HasValue)
                query.Where(p => p.GetById == getById);
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据操作类型、来源仓库获取释放/执行中状态的任务
        /// </summary>
        /// <param name="operationType">操作类型</param>
        /// <param name="fromWarehouseId">来源仓库</param>
        /// <param name="action">查询条件委托</param>
        /// <param name="page">分页信息</param>
        /// <param name="isAutoState">包含状态</param>
        /// <returns>释放任务集合</returns>
        public virtual EntityList<TaskManagement> GetTaskManagements(OperationType? operationType, double? fromWarehouseId, Action<IEntityQueryer<TaskManagement>> action = null, PagingInfo page = null, bool isAutoState = true)
        {
            List<OperationType> ops = new List<OperationType>();
            if (operationType.HasValue)
                ops.Add(operationType.Value);
            return GetTaskManagements(ops, fromWarehouseId, action, page, isAutoState);
        }

        /// <summary>
        /// 根据操作类型、来源仓库获取释放/执行中状态的任务
        /// </summary>
        /// <param name="operationTypes">操作类型</param>
        /// <param name="fromWarehouseId">来源仓库</param>
        /// <param name="action">查询条件委托</param>
        /// <param name="page">分页信息</param>
        /// <param name="isAutoState">包含状态</param>
        /// <returns>释放任务集合</returns>
        public virtual EntityList<TaskManagement> GetTaskManagements(List<OperationType> operationTypes, double? fromWarehouseId, Action<IEntityQueryer<TaskManagement>> action = null, PagingInfo page = null, bool isAutoState = true)
        {
            EagerLoadOptions elo = new EagerLoadOptions();

            EntityList<TaskManagement> tasks = new EntityList<TaskManagement>();
            //获取所有指定操作人的任务集合
            var query = Query<TaskManagement>();
            if (operationTypes.Count > 0)
                query.Where(p => operationTypes.Contains(p.OperationType));
            if (isAutoState)
                query.Where(p => p.State == TaskState.Release || p.State == TaskState.Executing || p.State == TaskState.Appoint);

            if (fromWarehouseId.HasValue)
                query.Where(p => p.FromWarehouseId == fromWarehouseId.Value);

            query.LeftJoin<Operator>((a, b) => a.Id == b.TaskManagementId && b.IsMaster);
            action?.Invoke(query);

            elo.LoadWithViewProperty();
            var taskDatas = query.OrderBy(p => p.Level).OrderByDescending(p => p.State).OrderByDescending(x => x.No).ToList(page, elo);
            foreach (var task in taskDatas)
            {
                if (task.State == TaskState.Appoint && task.OperatorList.Count > 0 && !task.OperatorList.Any(c => c.EmployeeId == RT.IdentityId))
                {
                    continue;
                }

                tasks.Add(task);
                if (page != null && tasks.Count >= page.PageSize)
                    break;
            }

            return tasks;
        }

        /// <summary>
        /// 默认查询
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>任务列表</returns> 
        public virtual EntityList<TaskManagement> GetTaskManagements(TaskManagementCriteria criteria)
        {
            var q = Query<TaskManagement>();

            //增加仓库权限关联查询
            q.Exists<WarehouseEmployee>((x, y) => y.Where(p => (p.WarehouseId == x.FromWarehouseId || p.WarehouseId == x.ToWarehouseId) && p.EmployeeId == RT.IdentityId));

            if (criteria.TaskGroupNo.IsNotEmpty())
                q.Join<TaskGroup>((m, g) => m.TaskGroupId == g.Id && g.No == criteria.TaskGroupNo);
            if (criteria.Item != null)
                q.Where(p => p.ItemId == criteria.ItemId);
            if (criteria.Item != null)
                q.Where(p => p.ItemId == criteria.ItemId);
            if (criteria.FromWarehouse != null)
                q.Where(p => p.FromWarehouseId == criteria.FromWarehouseId);
            if (criteria.ToWarehouse != null)
                q.Where(p => p.ToWarehouseId == criteria.ToWarehouseId);
            if (criteria.No.IsNotEmpty())
                q.Where(p => p.No.Contains(criteria.No));
            if (criteria.BillNo.IsNotEmpty())
                q.Where(p => p.BillNo.Contains(criteria.BillNo));
            if (!string.IsNullOrEmpty(criteria.State))
            {
                var criteriaState = new List<int>();
                criteria.State.Split(',').ForEach(state =>
                {
                    criteriaState.Add(int.Parse(state));
                });
                q.Where(p => criteriaState.Contains((int)p.State));
            }
            if (criteria.Level.HasValue)
                q.Where(p => p.Level == criteria.Level);
            if (criteria.OperationType.HasValue)
                q.Where(p => p.OperationType == criteria.OperationType);
            if (criteria.TransactionType.HasValue)
                q.Where(p => p.TransactionType == criteria.TransactionType);
            if (criteria.SuggestToLocId.HasValue)
                q.Where(p => p.SuggestToLocId == criteria.SuggestToLocId);
            if (!criteria.Lot.IsNullOrEmpty())
                q.Where(p => p.SuggestLot.Code.Contains(criteria.Lot));
            if (!criteria.LPN.IsNullOrEmpty())
                q.Where(p => p.SuggestFromLpn.Contains(criteria.LPN));
            if (!criteria.ActualToLpn.IsNullOrEmpty())
                q.Where(p => p.ActualToLpn.Contains(criteria.ActualToLpn));
            if (!criteria.SuggestToLpn.IsNullOrEmpty())
                q.Where(p => p.SuggestToLpn.Contains(criteria.SuggestToLpn));
            if (!criteria.ActualFromLpn.IsNullOrEmpty())
                q.Where(p => p.ActualFromLpn.Contains(criteria.ActualFromLpn));
            if (criteria.ReleaseDate.BeginValue.HasValue)
                q.Where(p => p.ReleaseDate >= criteria.ReleaseDate.BeginValue);
            if (criteria.ReleaseDate.EndValue.HasValue)
                q.Where(p => p.ReleaseDate <= criteria.ReleaseDate.EndValue);
            if (criteria.ActualOperator != null)
                q.Exists<ActualOperator>((a, b) => b.Where(p => p.EmployeeId == criteria.ActualOperatorId && p.TaskManagementId == a.Id));
            if (criteria.EndDateTime.BeginValue.HasValue)
                q.Where(p => p.EndDate >= criteria.EndDateTime.BeginValue);
            if (criteria.EndDateTime.EndValue.HasValue)
                q.Where(p => p.EndDate <= criteria.EndDateTime.EndValue);
            if (criteria.CreateDate.BeginValue.HasValue)
                q.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue.Value);
            if (criteria.CreateDate.EndValue.HasValue)
                q.Where(p => p.CreateDate < criteria.CreateDate.EndValue.Value.AddDays(1));
            if (criteria.CreateById.HasValue)
                q.Where(p => p.CreateBy == criteria.CreateById.Value);

            q.OrderBy(criteria.OrderInfoList);
            return q.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据库位获取同排层列库位（不同深度，入库锁已锁）任务
        /// </summary>
        /// <param name="locId">库位</param>
        /// <param name="depth">深度</param>
        /// <returns>任务</returns>
        public virtual EntityList<TaskManagement> GetTaskManagements(double locId, int depth)
        {
            var query = Query<TaskManagement>()
                .Join<StorageLocation>("l", (p, l) => p.SuggestToLocId == l.Id && l.IsInLock)
                .Join<StorageLocation, StorageLocation>("ll", (l, ll) => ll.AreaId == l.AreaId && ll.Id == locId &&
                ll.RowNo == l.RowNo && ll.ColumnNo == l.ColumnNo && ll.LayerNo == l.LayerNo && l.Depth == depth);
            return query.ToList();
        }

        /// <summary>
        /// 获取任务(释放、执行中)
        /// </summary>
        /// <param name="suggestLpn">建议目标LPN</param>
        /// <param name="fromWhCode">来源仓库编码</param>
        /// <param name="elo">贪懒加载</param>
        /// <returns>任务列表</returns>
        public virtual EntityList<TaskManagement> GetTaskManagements(string suggestLpn, string fromWhCode, EagerLoadOptions elo)
        {
            var query = Query<TaskManagement>().Where(p => p.SuggestToLpn == suggestLpn);
            if (fromWhCode.IsNotEmpty())
            {
                query.Join<Warehouse>((p, w) => p.FromWarehouseId == w.Id && w.Code == fromWhCode);
            }
            query.Where(p => p.State == TaskState.Release || p.State == TaskState.Executing);
            return query.ToList(null, elo);
        }

        /// <summary>
        /// 获取任务(释放、执行中)
        /// </summary>
        /// <param name="suggestLpn">建议目标LPN</param>
        /// <param name="toWarehouseId">目标仓库仓库ID</param>
        /// <param name="elo">贪懒加载</param>
        /// <returns>任务列表</returns>
        public virtual EntityList<TaskManagement> GetTaskManagements(string suggestLpn, double? toWarehouseId, EagerLoadOptions elo)
        {
            var query = Query<TaskManagement>().Where(p => p.SuggestToLpn == suggestLpn);
            if (toWarehouseId.HasValue)
                query.Where(p => p.FromWarehouseId == toWarehouseId);
            query.Where(p => p.State == TaskState.Release || p.State == TaskState.Executing);
            return query.ToList(null, elo);
        }

        /// <summary>
        /// 获取任务
        /// </summary>
        /// <param name="asnNo">ASN单号</param>
        /// <param name="lineNo">行号</param>
        /// <returns>任务</returns>
        public virtual EntityList<TaskManagement> GetTaskManagements(string asnNo, string lineNo)
        {
            var query = Query<TaskManagement>().Where(p => p.BillNo == asnNo && p.BillDtlNo == lineNo);
            query.Where(p => (p.State == TaskState.Release || p.State == TaskState.Appoint || p.State == TaskState.Executing));
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 根据单据明细ID集合获取任务信息
        /// </summary>
        /// <param name="lpns">LPN集合</param>
        /// <param name="action">查询条件委托</param>
        /// <returns>任务列表</returns>
        public virtual EntityList<TaskManagement> GetTaskManagementByLpns(List<string> lpns, Action<IEntityQueryer<TaskManagement>> action = null)
        {
            //var exp = lpns.CreateContainsExpression<TaskManagement>("x", nameof(TaskManagement.SuggestFromLpn));
            //if (exp == null)
            //{
            //    return new EntityList<TaskManagement>();
            //}

            //var query = Query<TaskManagement>();
            //action?.Invoke(query);

            //return query.Where(exp).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            EntityList<TaskManagement> results = new EntityList<TaskManagement>();
            lpns = lpns.Distinct().ToList();
            DataProcessEx.SplitDataExecute(lpns, sons =>
            {
                var query = Query<TaskManagement>();
                action?.Invoke(query);
                var list = query.Where(p => sons.Contains(p.SuggestFromLpn)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                results.AddRange(list);
            });
            return results;
        }

        /// <summary>
        /// 获取任务行数
        /// </summary>
        /// <param name="queryAction">查询条件委托</param>
        /// <returns>任务行数</returns>
        public virtual int GetTaskManagementCount(Action<IEntityQueryer<TaskManagement>> queryAction)
        {
            var query = Query<TaskManagement>();
            queryAction?.Invoke(query);
            return query.Count();
        }

        /// <summary>
        /// 根据单据明细ID集合获取任务信息
        /// </summary>
        /// <param name="orderType">单据类型</param>
        /// <param name="billDtlIdlist">单据明细ID集合</param>
        /// <param name="action">查询条件委托</param>
        /// <returns>任务列表</returns>
        public virtual EntityList<TaskManagement> GetBillDtlTaskManagements(OrderType orderType, List<double> billDtlIdlist, Action<IEntityQueryer<TaskManagement>> action = null)
        {
            if (billDtlIdlist == null || billDtlIdlist.Count == 0)
            {
                return new EntityList<TaskManagement>();
            }
            return billDtlIdlist.SplitContains(sons =>
            {
                var query = Query<TaskManagement>();
                query.Where(p => p.OrderType == orderType);
                action?.Invoke(query);
                return query.Where(p => sons.Contains(p.BillDtlId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 根据单据ID获取任务信息
        /// </summary>
        /// <param name="orderType">单据类型</param>
        /// <param name="billId">单据ID.</param>
        /// <param name="action">查询条件委托</param>
        /// <returns>任务列表</returns>
        public virtual EntityList<TaskManagement> GetBillTaskManagements(OrderType orderType, double billId, Action<IEntityQueryer<TaskManagement>> action = null)
        {
            var query = Query<TaskManagement>();
            query.Where(p => p.OrderType == orderType && p.BillId == billId);
            action?.Invoke(query);
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据推荐下架仓库获取任务信息
        /// </summary>
        /// <param name="fromWarehouseId">来源仓库</param>
        /// <returns>任务列表</returns>
        public virtual EntityList<TaskManagement> GetTaskManagement(double fromWarehouseId)
        {
            var query = Query<TaskManagement>();
            query.Where(p => p.FromWarehouseId == fromWarehouseId && (p.State == TaskState.Release || p.State == TaskState.Appoint || p.State == TaskState.Executing));
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取任务信息
        /// </summary>        
        /// <param name="taskIds">任务</param>
        /// <returns>任务列表</returns>
        public virtual EntityList<TaskManagement> GetTaskManagement(List<double> taskIds)
        {
            return taskIds.SplitContains(ids =>
            {
                return Query<TaskManagement>().Where(f => ids.Contains(f.Id)).ToList();
            });
        }

        /// <summary>
        /// 获取任务
        /// </summary>
        /// <param name="asnNo">ASN单号</param>
        /// <param name="lineNo">行号</param>
        /// <returns>任务</returns>
        public virtual TaskManagement GetTaskManagement(string asnNo, string lineNo)
        {
            var query = Query<TaskManagement>().Where(p => p.BillNo == asnNo && p.BillDtlNo == lineNo);
            query.Where(p => (p.State == TaskState.Release || p.State == TaskState.Appoint || p.State == TaskState.Executing));
            return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取任务
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="billDtlId">单据行ID</param>
        /// <param name="orderType">单据类型</param>
        /// <returns>任务</returns>
        public virtual TaskManagement GetTaskManagement(string billNo, double billDtlId, OrderType orderType)
        {
            return Query<TaskManagement>().Where(p => p.BillNo == billNo
                && p.BillDtlId == billDtlId && p.OrderType == orderType).FirstOrDefault();
        }

        /// <summary>
        /// 根据仓库、任务组ID获取任务信息
        /// </summary>
        /// <param name="warehouseId">仓库</param>
        /// <param name="taskGroupIds">任务组ID</param>
        /// <returns>任务信息</returns>
        public virtual EntityList<TaskManagement> GetTaskByGroupIds(double warehouseId, List<double> taskGroupIds)
        {
            var alltaskGroupIds = taskGroupIds.Cast<double?>().ToList();

            var result = new EntityList<TaskManagement>();

            for (int i = 0; i < Math.Ceiling((double)alltaskGroupIds.Count / 1000); i++)
            {
                var query = Query<TaskManagement>().Where(p => p.FromWarehouseId == warehouseId && p.TaskGroupId != null);
                query.Where(x => alltaskGroupIds.Skip(i * 1000).Take(1000).Contains(x.TaskGroupId));

                result.AddRange(query.ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
            }

            return result;
        }

        /// <summary>
        /// 获取任务
        /// </summary>
        /// <param name="taskGroupId">任务组Id</param>
        /// <param name="itemId">物料Id</param>
        /// <param name="itemExtPropName">物料扩展属性</param>
        /// <param name="lotId">批次</param>
        /// <param name="storageLocationId">库位</param>
        /// <param name="lpn">lpn</param>
        /// <param name="operationType">操作类型</param>
        /// <returns>任务</returns>
        public virtual EntityList<TaskManagement> GetTaskManagementsForScan(double taskGroupId, double itemId, string itemExtPropName, double? lotId, double? storageLocationId, string lpn, OperationType? operationType)
        {
            var query = Query<TaskManagement>().Where(p => p.TaskGroupId == taskGroupId && p.ItemId == itemId && p.ItemExtPropName == itemExtPropName
            && p.SuggestFromLpn == lpn);

            query.Where(p => p.SuggestLotId == lotId);

            if (storageLocationId.HasValue)
            {
                query.Where(p => p.SuggestFromLocId == storageLocationId);
            }
            if (operationType.HasValue)
                query.Where(p => p.OperationType == operationType);

            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取任务数据
        /// </summary>
        /// <param name="orderType">单据类型</param>
        /// <param name="billNo">单据号</param>
        /// <param name="warehouseId">来源仓库</param>
        /// <param name="page">分页信息</param>
        /// <param name="sortInfo">排序信息</param>
        /// <returns>任务数据</returns>
        public virtual EntityList<TaskManagement> GetTaskManagementForBillNo(OrderType orderType, string billNo, double? warehouseId = null, PagingInfo page = null, IList<OrderInfo> sortInfo = null)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            var query = Query<TaskManagement>().Where(p => p.OrderType == orderType);
            if (billNo.IsNotEmpty())
            {
                query.Where(p => p.BillNo.Contains(billNo));
            }

            if (warehouseId.HasValue)
            {
                query.Where(p => p.FromWarehouseId == warehouseId.Value);
            }

            return query.OrderBy(sortInfo).ToList(page, elo.LoadWithViewProperty());
        }

        /// <summary>
        /// 获取任务By类型
        /// </summary>
        /// <param name="dtlIds">明细Id</param>
        /// <param name="operationType">类型</param>
        /// <returns>类型</returns>
        public virtual EntityList<TaskManagement> GetTaskManagementsFromDtlIds(List<double> dtlIds, OperationType operationType)
        {
            return Query<TaskManagement>().Where(p => dtlIds.Contains(p.BillDtlId) && (p.State == TaskState.Create || p.State == TaskState.Release || p.State == TaskState.Appoint || p.State == TaskState.Executing) && p.OperationType == operationType).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取任务By类型
        /// </summary>
        /// <param name="dtlId">明细Id</param>
        /// <param name="operationType">类型</param>
        /// <returns>类型</returns>
        public virtual TaskManagement GetTaskManagementsFromDtlId(double dtlId, OperationType operationType)
        {
            return Query<TaskManagement>().Where(p => dtlId == p.BillDtlId && (p.State == TaskState.Create || p.State == TaskState.Executing) && p.OperationType == operationType).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取任务By类型
        /// </summary>
        /// <param name="dtlId">明细Id</param>
        /// <param name="operationType">类型</param>
        /// <param name="elo">婪加载</param>
        /// <returns>类型</returns>
        public virtual TaskManagement GetReleaseTaskManagementsFromDtlId(double dtlId, OperationType operationType, EagerLoadOptions elo = null)
        {
            return Query<TaskManagement>().Where(p => dtlId == p.BillDtlId && p.State == TaskState.Release
            && p.OperationType == operationType).FirstOrDefault(elo);
        }


        /// <summary>
        /// 验证任务主操作人
        /// </summary>
        /// <param name="taskManagement">任务信息</param>
        /// <exception cref="ValidationException">
        /// "实际主操作人只能有一个".L10N()
        /// </exception>
        public virtual void ValidateTaskManagement(TaskManagement taskManagement)
        {
            if (taskManagement.OperatorList.Count(p => p.IsMaster) > 1)
                throw new ValidationException("实际主操作人只能有一个".L10N());
        }

        /// <summary>
        /// 验证任务是否能加急
        /// </summary>
        /// <param name="taskManagement">任务管理</param>
        /// <exception cref="ValidationException">
        /// "只能加急有来源仓库的任务".L10N()
        /// or
        /// "来源仓库[{0}]的任务达到加急最大值".L10nFormat(taskManagement.FromWarehouse.Code)
        /// </exception>
        public virtual void ValidateTaskUrgentMaxCount(TaskManagement taskManagement)
        {
            if (!taskManagement.FromWarehouseId.HasValue)
                throw new ValidationException("只能加急有来源仓库的任务".L10N());
            var urgentTaskCount = GetTaskManagement(taskManagement.FromWarehouseId.Value).Count(p => p.Level == TaskLevel.Urgent);
            int maxCount = GetTaskUrgentMaxCount();
            if (urgentTaskCount >= maxCount)
                throw new ValidationException("来源仓库[{0}]的任务达到加急最大值".L10nFormat(taskManagement.FromWarehouse.Code));
        }

        /// <summary>
        /// 获取任务状态
        /// </summary>
        /// <param name="task">任务信息</param>
        /// <returns>任务状态</returns>
        public virtual TaskState GetTaskState(TaskManagement task)
        {
            TaskState state = TaskState.Create;
            DateTime dt = DateTime.Now;
            task.ReleaseDate = task.ReleaseDate.HasValue ? task.ReleaseDate.Value : dt;

            if (task.ReleaseDate > dt)
            {
                state = TaskState.Create;
                return state;
            }

            if (task.OperatorList.Any() && task.ReleaseDate <= dt)
            {
                state = TaskState.Appoint;
            }

            if (!task.OperatorList.Any() && task.ReleaseDate <= dt)
            {
                state = TaskState.Release;
            }

            return state;
        }

        /// <summary>
        /// 保存实际操作人
        /// </summary>
        /// <param name="task">任务</param>
        /// <param name="employeeIdList">员工ID集合</param>
        public virtual void SaveTaskActOperator(TaskManagement task, List<double> employeeIdList)
        {
            if (employeeIdList.Count > 0)
            {
                ActualOperator op;
                employeeIdList.ForEach(e =>
                {
                    op = new ActualOperator()
                    {
                        EmployeeId = e,
                        IsMaster = e == RT.IdentityId
                    };

                    task.ActualOperatorList.Add(op);
                });
            }
        }

        /// 保存实际操作人
        /// </summary>
        /// <param name="task">任务</param>
        /// <param name="employeeIdList">员工ID集合</param>
        public virtual EntityList<ActualOperator> InsertTaskActOperator(TaskManagement task, List<double> employeeIdList)
        {
            EntityList<ActualOperator> operators = new EntityList<ActualOperator>();
            if (employeeIdList.Count > 0)
            {
                employeeIdList.ForEach(e =>
                {
                    var op = new ActualOperator()
                    {
                        EmployeeId = e,
                        IsMaster = e == RT.IdentityId,
                        TaskManagementId = task.Id,
                    };
                    operators.Add(op);
                });
            }
            return operators;
        }

        /// <summary>
        /// 保存指定操作人
        /// </summary>
        /// <param name="task">任务</param>
        /// <param name="employeeIdList">员工ID集合</param>
        public virtual void SaveTaskOperator(TaskManagement task, List<double> employeeIdList)
        {
            if (employeeIdList.Count > 0)
            {
                Operator op;
                employeeIdList.ForEach(e =>
                {
                    op = new Operator()
                    {
                        EmployeeId = e,
                        IsMaster = employeeIdList.Count == 1
                    };
                    task.OperatorList.Add(op);
                });
            }
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="data">任务采集数据</param>
        /// <exception cref="ValidationException">
        /// 物料不能为空".L10N()
        /// or
        /// 任务来源ID不能为空".L10N()
        /// or
        /// 批次不能为空".L10N()
        /// </exception>
        public virtual void Validate(TaskCollectData data)
        {
            if (data.operationType == OperationType.Check)
                return;

            if (data.Item == null)
                throw new ValidationException("物料不能为空".L10N());

            if (data.StockTransList.Count <= 0)
            {
                throw new ValidationException("交易双方参数不能为空".L10N());
            }

            if (data.Lot == null && !data.IsAllowEmptyLot)
                throw new ValidationException("批次不能为空".L10N());
        }

        /// <summary>
        /// 保存任务
        /// </summary>
        /// <param name="task">需要保存的任务</param>
        /// <param name="taskSourceId">任务来源ID（最终产生任务的来源ID）</param>
        /// <param name="locList">库位列表</param>
        /// <param name="stockTrans">交易双方参数</param>
        /// <param name="taskCollectData">交易记录相关数据</param>
        public virtual void SaveTask(TaskManagement task, double taskSourceId, List<StorageLocation> locList, StockTrans stockTrans, TaskCollectData taskCollectData)
        {
            var fromLoc = locList.FirstOrDefault(p => p.Id == stockTrans.FromLocationId);
            var toLoc = locList.FirstOrDefault(p => p.Id == stockTrans.ToLocationId);
            task.Item = taskCollectData.Item;
            task.SuggestLot = taskCollectData.Lot;
            task.FromWarehouseId = taskCollectData.FromWarehouseId == null ? fromLoc?.WarehouseId : taskCollectData.FromWarehouseId;
            task.FromAreaId = fromLoc?.AreaId;
            task.ToWarehouseId = taskCollectData.ToWarehouseId == null ? toLoc?.WarehouseId : taskCollectData.ToWarehouseId;
            task.SuggestFromLocId = fromLoc?.Id;
            task.SuggestFromLpn = stockTrans.FromLpn;
            task.SuggestToLocId = toLoc?.Id;
            task.SuggestToLpn = stockTrans.ToLpn;
            task.ReleaseDate = taskCollectData.ReleaseDate.HasValue ? taskCollectData.ReleaseDate.Value : DateTime.Now;
            task.State = GetTaskState(task);
            task.Qty = stockTrans.Qty;
            task.BillNo = taskCollectData.BaseTransactionData.BillNo;
            task.BillId = taskCollectData.BaseTransactionData.BillId;
            task.BillDtlId = taskCollectData.BaseTransactionData.BillDetailId;
            task.BillDtlNo = taskCollectData.BaseTransactionData.BillDetailNo;
            task.RelationOrderNo = taskCollectData.BaseTransactionData.RelationOrderNo;
            task.StorerCode = taskCollectData.BaseTransactionData.StorerCode;
            task.ProjectNo = taskCollectData.BaseTransactionData.ProjectNo;
            task.TaskNo = taskCollectData.BaseTransactionData.TaskNo;
            task.OnhandState = taskCollectData.BaseTransactionData.OnhandState;
            task.OrderType = taskCollectData.BaseTransactionData.OrderType;
            task.TransactionId = taskCollectData.BaseTransactionData.TransactionId;
            task.SowType = taskCollectData.BaseTransactionData.SowType;
            task.TransactionType = taskCollectData.BaseTransactionData.TransactionType;
            task.SecondBillDtlNo = taskCollectData.SecondLineNo;
            task.OperationType = taskCollectData.operationType;
            if (taskCollectData.TaskState.HasValue)
            {
                task.State = taskCollectData.TaskState.Value;
            }
            task.GetById = taskCollectData.GetById;
            task.Level = taskCollectData.BaseTransactionData.TaskLevel;
            task.TaskSourceId = taskSourceId;
            task.ItemExtProp = taskCollectData.ItemExtProp;
            task.ItemExtPropName = taskCollectData.ItemExtPropName;
            task.SuggestFromStation = taskCollectData.SuggestFromStation;
            task.SuggestToStation = taskCollectData.SuggestToStation;
            task.ActualFromStation = taskCollectData.ActualFromStation;
            task.ActualToStaion = taskCollectData.ActualToStaion;
        }

        /// <summary>
        /// 保存任务，其他单据等需要额外保存交易数据的，可以重写此方法
        /// </summary>
        /// <param name="task">需要保存的任务</param>
        /// <param name="taskCollectData">交易记录相关数据</param>
        public virtual void SaveTask(TaskManagement task, TaskCollectData taskCollectData)
        { }

        /// <summary>
        /// 创建任务
        /// </summary>
        /// <param name="taskCollectData">交易记录相关数据</param>
        /// <returns>任务列表</returns>
        public virtual EntityList<TaskManagement> CreateTask(TaskCollectData taskCollectData)
        {
            EntityList<TaskManagement> taskList = new EntityList<TaskManagement>();
            var fromLocIdList = taskCollectData.StockTransList.Values.Select(p => p.FromLocationId).Distinct().ToList();
            var toLocIdList = taskCollectData.StockTransList.Values.Select(p => p.ToLocationId).Distinct().ToList();
            var locIdList = fromLocIdList.Union(toLocIdList).ToList();

            EagerLoadOptions elo = new EagerLoadOptions();
            var locList = RT.Service.Resolve<WarehouseController>().GetStorageLocations(locIdList, elo).ToList();

            //批量生产任务单号
            var taskNoList = GetTaskNos(taskCollectData.StockTransList.Count);
            int index = 0;
            foreach (var stock in taskCollectData.StockTransList)
            {
                TaskManagement task = new TaskManagement();
                task.No = taskNoList[index];
                Validate(taskCollectData);

                //保存指定操作人
                SaveTaskOperator(task, taskCollectData.EmployeeIdList);

                //保存任务信息
                SaveTask(task, stock.Key, locList, stock.Value, taskCollectData);
                SaveTask(task, taskCollectData);

                taskList.Add(task);
                index++;
            }

            RF.Save(taskList);
            return taskList;
        }

        /// <summary>
        /// 创建任务
        /// </summary>
        /// <param name="taskCollectDataList">交易记录相关数据集合</param>
        /// <returns>任务列表</returns>
        public virtual EntityList<TaskManagement> CreateTask(List<TaskCollectData> taskCollectDataList)
        {
            if (taskCollectDataList == null || taskCollectDataList.Count == 0)
            {
                return new EntityList<TaskManagement>();
            }
            EntityList<TaskManagement> taskList = new EntityList<TaskManagement>();

            var fromLocIdList = taskCollectDataList.SelectMany(p => p.StockTransList.Values).Select(p => p.FromLocationId).Distinct().ToList();
            var toLocIdList = taskCollectDataList.SelectMany(p => p.StockTransList.Values).Select(p => p.ToLocationId).Distinct().ToList();
            var locIdList = fromLocIdList.Union(toLocIdList).Distinct().ToList();

            EagerLoadOptions elo = new EagerLoadOptions();
            var locList = RT.Service.Resolve<WarehouseController>().GetStorageLocations(locIdList, elo).ToList();

            //批量生产任务单号
            var count = taskCollectDataList.SelectMany(p => p.StockTransList).Count();
            List<string> taskNoList = GetTaskNos(count);
            int index = 0;
            foreach (var taskCollectData in taskCollectDataList)
            {
                foreach (var stock in taskCollectData.StockTransList)
                {
                    TaskManagement task = new TaskManagement();
                    task.No = taskNoList[index];
                    Validate(taskCollectData);

                    //保存指定操作人
                    SaveTaskOperator(task, taskCollectData.EmployeeIdList);

                    //保存任务信息
                    SaveTask(task, stock.Key, locList, stock.Value, taskCollectData);
                    SaveTask(task, taskCollectData);

                    taskList.Add(task);
                    index++;
                }
            }

            ////批量保存
            RT.Service.Resolve<SIE.Core.Common.Controllers.CommonController>().BatchInsertSave(taskList);
            taskList.MarkSaved();
            return taskList;
        }

        /// <summary>
        /// 库存盘点创建任务
        /// </summary>
        /// <param name="taskCollectDataList">交易记录相关数据集合</param>
        /// <remarks>原来的方法CreateTask执行需要23秒，优化后4.6秒</remarks>
        public virtual EntityList<TaskManagement> CreateTaskStockCount(List<TaskCollectData> taskCollectDataList)
        {
            EntityList<TaskManagement> taskList = new EntityList<TaskManagement>();
            var stockList = taskCollectDataList.Select(p => p.StockTransList).ToList();
            var taskNoList = GetTaskNos(stockList.Count);
            List<double> locIdList = new List<double>();
            stockList.Select(p => p.Values).ForEach(p =>
            {
                locIdList.AddRange(p.Select(f => f.FromLocationId).Distinct().ToList());
                locIdList.AddRange(p.Select(f => f.ToLocationId).Distinct().ToList());
            });
            var locList = RT.Service.Resolve<WarehouseController>().GetStorageLocations(locIdList.Distinct().ToList(), new EagerLoadOptions()).ToList();
            int index = 0;
            taskCollectDataList.ForEach(taskCollectData =>
            {
                //批量生产任务单号
                if (taskCollectData.StockTransList.Count > 0)
                {
                    var stock = taskCollectData.StockTransList.First();
                    TaskManagement task = new TaskManagement();
                    task.No = taskNoList[index];
                    Validate(taskCollectData);

                    //保存指定操作人
                    SaveTaskOperator(task, taskCollectData.EmployeeIdList);
                    //保存任务信息
                    SaveTask(task, stock.Key, locList, stock.Value, taskCollectData);
                    SaveTask(task, taskCollectData);

                    taskList.Add(task);
                    index++;
                }
            });
            BulkSaver.Save(taskList);
            taskList.MarkSaved();
            return taskList;
        }

        /// <summary>
        /// 创建自动完工任务
        /// </summary>
        /// <param name="taskCollectDataList">交易记录相关数据集合</param>
        /// <returns>任务列表</returns>
        public virtual EntityList<TaskManagement> CreateAutoFinishTask(List<TaskCollectData> taskCollectDataList)
        {
            EntityList<TaskManagement> taskList = new EntityList<TaskManagement>();
            var allStockTrans = taskCollectDataList.SelectMany(p => p.StockTransList.Values);
            var fromLocIdList = allStockTrans.Select(p => p.FromLocationId).Distinct().ToList();
            var toLocIdList = allStockTrans.Select(p => p.ToLocationId).Distinct().ToList();
            var locIdList = fromLocIdList.Union(toLocIdList).ToList();
            EagerLoadOptions elo = new EagerLoadOptions();
            var locList = RT.Service.Resolve<WarehouseController>().GetStorageLocations(locIdList, elo).ToList();
            //批量生产任务单号
            var taskNoList = GetTaskNos(taskCollectDataList.SelectMany(a => a.StockTransList).Count());
            int index = 0;
            foreach (var taskCollectData in taskCollectDataList)
            {
                foreach (var stock in taskCollectData.StockTransList)
                {
                    TaskManagement task = new TaskManagement();
                    task.No = taskNoList[index];
                    Validate(taskCollectData);

                    //保存指定操作人
                    SaveTaskActOperator(task, taskCollectData.EmployeeIdList);

                    //保存任务信息
                    SaveFinishTask(task, stock.Key, locList, stock.Value, taskCollectData);
                    SaveTask(task, taskCollectData);

                    taskList.Add(task);
                    index++;
                }
            }

            RT.Service.Resolve<SIE.Core.Common.Controllers.CommonController>().BatchInsertSave(taskList);
            return taskList;
        }

        /// <summary>
        /// 创建自动完工状态任务
        /// </summary>
        /// <param name="task">任务</param>
        /// <param name="taskSourceId">来源Id</param>
        /// <param name="locList">库位列表</param>
        /// <param name="stockTrans">交易双方参数</param>
        /// <param name="taskCollectData">交易记录相关数据</param>
        private void SaveFinishTask(TaskManagement task, double taskSourceId, List<StorageLocation> locList, StockTrans stockTrans, TaskCollectData taskCollectData)
        {
            var fromLoc = locList.FirstOrDefault(p => p.Id == stockTrans.FromLocationId);
            var toLoc = locList.FirstOrDefault(p => p.Id == stockTrans.ToLocationId);
            task.Item = taskCollectData.Item;
            task.SuggestLot = taskCollectData.Lot;
            task.FromWarehouseId = fromLoc?.WarehouseId;
            task.FromAreaId = fromLoc?.AreaId;
            task.ToWarehouseId = taskCollectData.ToWarehouseId == null ? toLoc?.WarehouseId : taskCollectData.ToWarehouseId;
            task.SuggestFromLocId = fromLoc?.Id;
            task.SuggestFromLpn = stockTrans.FromLpn;
            task.SuggestToLocId = toLoc?.Id;
            task.SuggestToLpn = stockTrans.ToLpn;
            task.ActualFromLocId = fromLoc?.Id;
            task.ActualFromLpn = stockTrans.FromLpn;
            task.ActualToLocId = toLoc?.Id;
            task.ActualToLpn = stockTrans.ToLpn;
            task.ReleaseDate = taskCollectData.ReleaseDate.HasValue ? taskCollectData.ReleaseDate.Value : DateTime.Now;
            task.State = TaskState.AutoFinish;
            task.Qty = stockTrans.Qty;
            task.ActualQty = task.Qty;
            task.ActualLotId = task.SuggestLotId;
            task.BillNo = taskCollectData.BaseTransactionData.BillNo;
            task.BillId = taskCollectData.BaseTransactionData.BillId;
            task.BillDtlId = taskCollectData.BaseTransactionData.BillDetailId;
            task.BillDtlNo = taskCollectData.BaseTransactionData.BillDetailNo;
            task.StorerCode = taskCollectData.BaseTransactionData.StorerCode;
            task.ProjectNo = taskCollectData.BaseTransactionData.ProjectNo;
            task.TaskNo = taskCollectData.BaseTransactionData.TaskNo;
            task.OnhandState = taskCollectData.BaseTransactionData.OnhandState;
            task.OrderType = taskCollectData.BaseTransactionData.OrderType;
            task.TransactionId = taskCollectData.BaseTransactionData.TransactionId;
            task.TransactionType = taskCollectData.BaseTransactionData.TransactionType;
            task.OperationType = taskCollectData.operationType;
            task.Level = taskCollectData.BaseTransactionData.TaskLevel;
            task.TaskSourceId = taskSourceId;
            task.SecondBillDtlNo = taskCollectData.SecondLineNo;
            task.BeginDate = DateTime.Now;
            task.EndDate = DateTime.Now;
            task.LengthTime = Convert.ToDecimal(Math.Round((task.EndDate.Value - task.BeginDate.Value).TotalMinutes, 1));
        }

        /// <summary>
        /// 设置任务完工信息
        /// </summary>
        /// <param name="tasks">任务</param>
        /// <param name="dtlDatas">单据大类任务管理数据</param>
        private void SetFinishTaskDatas(EntityList<TaskManagement> tasks, List<TaskFinishData> dtlDatas)
        {
            var whCtl = RT.Service.Resolve<WarehouseController>();
            var toLocIds = dtlDatas.Select(p => p.StockTrans).Where(p => p.ToWarehouseId == null).Select(p => p.ToLocationId).Distinct().ToList();
            if (toLocIds.Any())
            {
                var dicLoc = whCtl.GetStorageLocations(toLocIds, null).ToDictionary(p => p.Id);
                foreach (var dtl in dtlDatas.Where(f => f.StockTrans.ToWarehouseId == null))
                {
                    dicLoc.TryGetValue(dtl.StockTrans.ToLocationId, out StorageLocation toLocation);
                    dtl.StockTrans.ToWarehouseId = toLocation?.WarehouseId;
                }
            }
            List<double> updateTaskIds = new List<double>();
            var operators = new EntityList<ActualOperator>();
            foreach (var dtl in dtlDatas)
            {
                var task = tasks.FirstOrDefault(p => p.BillDtlId == dtl.BillDtlId && p.TaskSourceId == dtl.TaskSourceId &&
                                       p.State != TaskState.Finish && p.State != TaskState.Close && p.State != TaskState.AutoFinish);
                var stockTrans = dtl.StockTrans;

                if (task != null)
                {
                    task.BeginDate = task.BeginDate ?? DateTime.Now;
                    task.EndDate = DateTime.Now;
                    task.LengthTime = Convert.ToDecimal(Math.Round((task.EndDate.Value - task.BeginDate.Value).TotalMinutes, 1));
                    task.ActualLotId = dtl.LotId;
                    task.ActualQty = stockTrans.Qty;
                    task.ActualFromLocId = stockTrans.FromLocationId;
                    task.ActualFromLpn = stockTrans.FromLpn;
                    task.ActualToLocId = stockTrans.ToLocationId;
                    task.ToWarehouseId = stockTrans.ToWarehouseId;
                    task.ActualToLpn = stockTrans.ToLpn;
                    task.State = TaskState.Finish;
                    operators.AddRange(InsertTaskActOperator(task, dtl.EmployeeIdList));
                    updateTaskIds.Add(task.Id);
                }
            }
            if (updateTaskIds.Any())
            {
                tasks.Where(f => updateTaskIds.Contains(f.Id)).ForEach(a =>
                 {
                     DB.Update<TaskManagement>().Set(p => p.BeginDate, a.BeginDate)
                     .Set(p => p.EndDate, a.EndDate)
                     .Set(p => p.LengthTime, a.LengthTime)
                     .Set(p => p.ActualLotId, a.ActualLotId)
                     .Set(p => p.ActualQty, a.ActualQty)
                     .Set(p => p.ActualFromLocId, a.ActualFromLocId)
                     .Set(p => p.ActualFromLpn, a.ActualFromLpn)
                     .Set(p => p.ActualToLocId, a.ActualToLocId)
                     .Set(p => p.ToWarehouseId, a.ToWarehouseId)
                     .Set(p => p.ActualToLpn, a.ActualToLpn)
                     .Set(p => p.State, TaskState.Finish)
                     .Where(p => p.Id == a.Id).Execute();
                 });
                tasks.MarkSaved();
                if (operators.Any())
                    RT.Service.Resolve<SIE.Core.Common.Controllers.CommonController>().BatchInsertSave(operators);
            }
        }

        /// <summary>
        /// 完工任务：根据任务的单据明细ID和来源ID
        /// </summary>
        /// <param name="datas">任务完工提交数据</param>
        public virtual void FinishTask(List<TaskFinishData> datas)
        {
            if (!datas.Any())
                return;
            var orderTypeList = datas.Select(p => p.OrderType).Distinct().ToList();
            foreach (var orderType in orderTypeList)
            {
                var dtlDatas = datas.Where(p => p.OrderType == orderType).ToList();
                var taskBillDtlIdList = dtlDatas.Select(p => p.BillDtlId).Distinct().ToList();
                var taskSourceIdList = dtlDatas.Select(p => p.TaskSourceId).Distinct().ToList();
                var tasks = GetTaskManagementsNoEager(orderType, taskBillDtlIdList, taskSourceIdList);
                SetFinishTaskDatas(tasks, dtlDatas);
            }
        }

        /// <summary>
        /// 完工任务：根据任务的单据明细ID和来源ID
        /// </summary>
        /// <param name="tasks">任务</param>
        /// <param name="datas">任务完工提交数据</param>
        public virtual void FinishTask(EntityList<TaskManagement> tasks, List<TaskFinishData> datas)
        {
            if (!datas.Any())
                return;
            var orderTypeList = datas.Select(p => p.OrderType).Distinct().ToList();
            foreach (var orderType in orderTypeList)
            {
                var dtlDatas = datas.Where(p => p.OrderType == orderType).ToList();
                SetFinishTaskDatas(tasks, dtlDatas);
            }
        }

        /// <summary>
        /// 完工任务：根据任务的单据明细ID和来源ID
        /// </summary>
        /// <param name="datas">任务完工提交数据</param>
        public virtual void FinishTask(List<TaskFinishData> datas, EntityList<TaskManagement> tasksFrom)
        {
            if (!datas.Any())
                return;
            var orderTypeList = datas.Select(p => p.OrderType).Distinct().ToList();
            foreach (var orderType in orderTypeList)
            {
                var dtlDatas = datas.Where(p => p.OrderType == orderType).ToList();
                var tasks = tasksFrom.Where(f => f.OrderType == orderType).AsEntityList();
                SetFinishTaskDatas(tasks, dtlDatas);
            }
        }

        /// <summary>
        /// 取消任务完工
        /// </summary>
        /// <param name="orderType">任务状态</param>
        /// <param name="taskIdList">最终产生任务的来源ID</param>
        public virtual void UnFinishTask(OrderType orderType, List<double> taskIdList)
        {
            UpdateTaskState(orderType, taskIdList, TaskState.Close,
                e => { e.CloseDate = DateTime.Now; e.CloseById = RT.IdentityId; },
                query => { query.Where(p => p.State == TaskState.Finish || p.State == TaskState.AutoFinish); });
        }

        /// <summary>
        /// 冻结任务：根据任务来源ID集合
        /// </summary>
        /// <param name="orderType">订单类型</param>
        /// <param name="taskSourceIdlist">任务来源Id列表</param>
        /// <param name="queryAction">委托，用于查询</param>
        public virtual void FrozenTask(OrderType orderType, List<double> taskSourceIdlist, Action<IEntityQueryer<TaskManagement>> queryAction = null)
        {
            UpdateTaskState(orderType, taskSourceIdlist, TaskState.Frozen,
                e => { e.FrozenDate = DateTime.Now; e.FrozenById = RT.IdentityId; }, queryAction);
        }

        /// <summary>
        /// 关闭任务
        /// </summary>
        /// <param name="orderType">类型</param>
        /// <param name="taskSourceIdlist">任务ID集合</param>
        public virtual void CloseTask(OrderType orderType, List<double> taskSourceIdlist)
        {
            UpdateTaskState(orderType, taskSourceIdlist, TaskState.Close,
                e => { e.CloseDate = DateTime.Now; e.CloseById = RT.IdentityId; },
                query => { query.Where(p => p.State != TaskState.Finish && p.State != TaskState.Close && p.State != TaskState.AutoFinish); });
        }

        /// <summary>
        /// 关闭任务：根据单据明细ID集合
        /// </summary>
        /// <param name="orderType">订单类型</param>
        /// <param name="billDtlIdlist">单据明细ID集合.</param>
        public virtual void CloseBillDtlTasks(OrderType orderType, List<double> billDtlIdlist)
        {
            UpdateBillDtlTaskState(orderType, billDtlIdlist, TaskState.Close,
                e => { e.CloseDate = DateTime.Now; e.CloseById = RT.IdentityId; },
                query => { query.Where(p => p.State != TaskState.Finish && p.State != TaskState.Close && p.State != TaskState.AutoFinish); });
        }

        /// <summary>
        /// 关闭任务：根据单据ID
        /// </summary>
        /// <param name="orderType">订单类型</param>
        /// <param name="billId">单据ID</param>
        public virtual void CloseBillTasks(OrderType orderType, double billId)
        {
            UpdateBillTaskState(orderType, billId, TaskState.Close,
                e => { e.CloseDate = DateTime.Now; e.CloseById = RT.IdentityId; },
                query => { query.Where(p => p.State != TaskState.Finish); });
        }

        /// <summary>
        /// 释放任务（先根据ID查询最新对象，再更新）
        /// </summary>
        /// <param name="taskIdList">任务ID集合</param>
        public virtual void ReleaseTasks(List<double> taskIdList)
        {
            var taskList = GetTaskManagements(taskIdList, query => { query.Where(p => p.State != TaskState.Finish); });
            ReleaseTasks(taskList);
        }

        /// <summary>
        /// 释放任务
        /// </summary>
        /// <param name="taskList">任务集合</param>
        public virtual void ReleaseTasks(EntityList<TaskManagement> taskList)
        {
            if (taskList.Any(p => p.State != TaskState.Create && p.State != TaskState.Appoint && p.State != TaskState.Executing
                && p.State != TaskState.Frozen && p.State != TaskState.Abnormal))
            {
                throw new ValidationException("存在不是可释放状态任务，无法释放".L10N());
            }

            taskList.ForEach(p =>
            {
                p.State = TaskState.Release;
                p.ReleaseDate = DateTime.Now;
                p.GetBy = null;
                p.BeginDate = null;
                p.EndDate = null;
                p.FrozenById = null;
                p.FrozenDate = null;
                p.OperatorList.Clear();
            });

            RF.Save(taskList);
        }

        /// <summary>
        /// 领取任务
        /// </summary>
        /// <param name="taskList">任务集合</param>
        /// <param name="isWcs">立库任务</param>
        /// <param name="targetLpn">建议目标LPN</param>
        /// <param name="isFrozen">是否立库任务</param>
        public virtual void ExcuteTasks(EntityList<TaskManagement> taskList, bool isWcs = false, string targetLpn = "", bool isFrozen = false)
        {
            var emp = new Employee();
            if (isWcs)
            {
                emp = RT.Service.Resolve<EmployeeController>().GetWcsEmployee();
            }
            //领取操作人
            else
                emp = RT.Service.Resolve<EmployeeController>().GetEmployeeByUserId(RT.IdentityId);
            foreach (var p in taskList.Where(p => p.State == TaskState.Release || p.State == TaskState.Appoint || isFrozen && p.State == TaskState.Frozen))
            {
                if (p.State == TaskState.Appoint && emp != null && !p.OperatorList.Any(t => t.EmployeeId == emp.Id))
                    continue;

                p.State = TaskState.Executing;
                p.BeginDate = DateTime.Now;
                p.GetById = isWcs ? emp.Id : RT.IdentityId;
                if (targetLpn.IsNotEmpty())
                    p.SuggestToLpn = targetLpn;
                if (emp != null)
                {
                    if (p.OperatorList.Any(t => t.EmployeeId == emp.Id))
                    {
                        p.OperatorList.Where(t => t.EmployeeId == emp.Id).ForEach(t => t.IsMaster = true);
                    }
                    else
                    {
                        p.OperatorList.ForEach(t => t.IsMaster = false);
                        p.OperatorList.Add(new Operator() { Employee = emp, IsMaster = true });
                    }
                }
            }

            RF.Save(taskList);
        }

        /// <summary>
        /// 冻结任务任务（先根据ID查询最新对象，再更新）
        /// </summary>
        /// <param name="taskIdList">任务ID集合</param>
        public virtual void FrozenTasks(List<double> taskIdList)
        {
            var taskList = GetTaskManagements(taskIdList,
                query => { query.Where(p => p.State != TaskState.Finish && p.State != TaskState.Close); });
            FrozenTasks(taskList);
        }

        /// <summary>
        /// 冻结任务任务
        /// </summary>
        /// <param name="taskList">任务集合</param>
        public virtual void FrozenTasks(EntityList<TaskManagement> taskList)
        {
            if (taskList.Any(p => p.State != TaskState.Create && p.State != TaskState.Appoint
                && p.State != TaskState.Release && p.State != TaskState.Close && p.State != TaskState.Finish))
            {
                throw new ValidationException("存在不是可冻结状态任务，无法冻结".L10N());
            }

            taskList.ForEach(p =>
            {
                p.State = TaskState.Frozen;
                p.FrozenById = RT.IdentityId;
                p.FrozenDate = DateTime.Now;
            });

            RF.Save(taskList);
        }

        /// <summary>
        /// 更新任务状态：根据任务来源ID集合
        /// </summary>
        /// <param name="orderType">订单类型</param>
        /// <param name="taskSourceIdlist">任务来源Id列表</param>
        /// <param name="state">任务状态</param>
        /// <param name="action">委托,用于更新任务实体其他字段</param>
        /// <param name="queryAction">委托，用于查询</param>
        public virtual void UpdateTaskState(OrderType orderType, List<double> taskSourceIdlist, TaskState state, Action<TaskManagement> action = null, Action<IEntityQueryer<TaskManagement>> queryAction = null)
        {
            var tasks = GetTaskManagements(orderType, taskSourceIdlist, queryAction);
            if (tasks != null && tasks.Count > 0)
            {
                tasks.ForEach(e =>
                {
                    e.State = state;
                    action?.Invoke(e);
                });

                RF.Save(tasks);
            }
        }

        /// <summary>
        /// 更新任务状态：根据任务对象
        /// </summary>
        /// <param name="task">任务对象</param>
        /// <param name="state">更新状态</param>
        /// <param name="action">委托,用于更新任务实体其他字段</param>
        public virtual void UpdateTaskState(TaskManagement task, TaskState state, Action<TaskManagement> action = null)
        {
            if (task != null)
            {
                task.State = state;
                action?.Invoke(task);
                RF.Save(task);
            }
        }

        /// <summary>
        /// 更新任务状态：根据单据明细ID集合
        /// </summary>
        /// <param name="orderType">订单类型</param>
        /// <param name="billDtlIdlist">单据明细ID集合.</param>
        /// <param name="state">任务状态</param>
        /// <param name="action">委托,用于更新任务实体其他字段</param>
        /// <param name="queryAction">委托，用于查询</param>
        public virtual void UpdateBillDtlTaskState(OrderType orderType, List<double> billDtlIdlist, TaskState state, Action<TaskManagement> action = null, Action<IEntityQueryer<TaskManagement>> queryAction = null)
        {
            var tasks = GetBillDtlTaskManagements(orderType, billDtlIdlist, queryAction);
            if (tasks != null && tasks.Count > 0)
            {
                tasks.ForEach(e =>
                {
                    e.State = state;
                    action?.Invoke(e);
                });

                RF.Save(tasks);
            }
        }

        /// <summary>
        /// 更新任务状态：根据单据ID
        /// </summary>
        /// <param name="orderType">订单类型</param>
        /// <param name="billId">单据ID</param>
        /// <param name="state">任务状态</param>
        /// <param name="action">委托,用于更新任务实体其他字段</param>
        /// <param name="queryAction">委托，用于查询</param>
        public virtual void UpdateBillTaskState(OrderType orderType, double billId, TaskState state, Action<TaskManagement> action = null, Action<IEntityQueryer<TaskManagement>> queryAction = null)
        {
            var tasks = GetBillTaskManagements(orderType, billId, queryAction);
            if (tasks != null && tasks.Count > 0)
            {
                tasks.ForEach(e =>
                {
                    e.State = state;
                    action?.Invoke(e);
                });

                RF.Save(tasks);
            }
        }

        /// <summary>
        /// 更新任务等级
        /// </summary>
        /// <param name="taskId">任务ID</param>
        /// <param name="taskLevel">任务等级</param>
        public virtual void UpdateTaskLevel(double taskId, TaskLevel taskLevel)
        {
            var query = DB.Update<TaskManagement>();
            query.Set(f => f.Level, taskLevel)
                .Where(f => f.Id == taskId).Execute();
        }

        /// <summary>
        /// 根据任务ID验证任务是否可执行
        /// </summary>
        /// <param name="task">任务ID</param>
        /// <returns>任务</returns>
        public virtual TaskManagement ValidateTask(TaskManagement task)
        {
            if (task == null)
            {
                throw new ValidationException("任务不存在".L10N());
            }
            else
            {
                if (task.State != TaskState.Release && task.State != TaskState.Executing)
                {
                    if (task.State == TaskState.Appoint)
                    {
                        if (!task.OperatorList.Any(p => p.EmployeeId == RT.IdentityId))
                            throw new ValidationException("任务[{0}]是指定状态,该用户不存在指定操作人列表中!".L10nFormat(task.No));
                    }
                    else
                        throw new ValidationException("任务[{0}]不是释放或者执行中状态".L10nFormat(task.No));
                }
            }

            return task;
        }

        /// <summary>
        /// 任务状态改变
        /// </summary>
        /// <param name="action">委托：更新查询条件</param>
        /// <returns>更新行数</returns>
        public virtual int UpdateTasks(Action<IEntityUpdate<TaskManagement>> action = null)
        {
            var query = DB.Update<TaskManagement>();
            action?.Invoke(query);
            var count = query.Execute();
            return count;
        }

        /// <summary>
        /// 任务状态改变
        /// </summary>
        /// <param name="taskIds">任务ID</param>
        /// <param name="action">委托：更新查询条件</param>
        /// <returns>更新行数</returns>
        public virtual int UpdateTasks(List<double> taskIds, Action<IEntityUpdate<TaskManagement>> action = null)
        {
            //var exp = taskIds.CreateContainsExpression<TaskManagement>("x", nameof(TaskManagement.Id));
            //if (exp == null)
            //{
            //    return 0;
            //}
            //var query = DB.Update<TaskManagement>();
            //action?.Invoke(query);
            //query.Where(exp);
            //var count = query.Execute();
            //return count;
            if (taskIds == null || taskIds.Count == 0)
            {
                return 0;
            }
            int count = 0;
            taskIds.SplitDataExecute(sons =>
            {
                var query = DB.Update<TaskManagement>();
                action?.Invoke(query);
                query.Where(p => sons.Contains(p.Id));
                count = query.Execute();
            });
            return count;
        }

        /// <summary>
        /// 更新Wcs的任务（任务领取人和指定操作人写入为WCS）
        /// </summary>
        /// <param name="taskIds">任务集合</param>
        /// <param name="action">委托：更新查询条件</param>
        /// <returns>更新行数</returns>
        public virtual int UpdateWcsTasks(List<double> taskIds, Action<IEntityUpdate<TaskManagement>> action = null)
        {
            //var exp = taskIds.CreateContainsExpression<TaskManagement>("x", nameof(TaskManagement.Id));
            //if (exp == null)
            //{
            //    return 0;
            //}
            //var emp = RT.Service.Resolve<EmployeeController>().GetWcsEmployee();
            //var query = DB.Update<TaskManagement>();
            //query.Set(p => p.GetById, emp.Id);
            //action?.Invoke(query);

            //query.Where(exp);
            //var count = query.Execute();
            //if (count <= 0)
            //{
            //    throw new ValidationException("无法更新任务，没有找到创建、释放或者执行中任务".L10N());
            //}

            ////更新指定操作人 20220330 永学取消指令写指定操作人
            ////UpdateTaskOp(taskIds, emp.Id);

            if (taskIds == null || taskIds.Count == 0)
            {
                return 0;
            }
            var emp = RT.Service.Resolve<EmployeeController>().GetWcsEmployee();
            int count = 0;
            taskIds.SplitDataExecute(sons =>
            {
                var query = DB.Update<TaskManagement>();
                query.Set(p => p.GetById, emp.Id);
                action?.Invoke(query);

                query.Where(p => sons.Contains(p.Id));
                count = query.Execute();
            });
            if (count <= 0)
            {
                throw new ValidationException("无法更新任务，没有找到创建、释放或者执行中任务".L10N());
            }
            return count;
        }

        /// <summary>
        /// 更新指定操作人
        /// </summary>
        /// <param name="taskIds">任务Id</param>
        /// <param name="empId">WCS操作人</param>
        public virtual void UpdateTaskOp(List<double> taskIds, double empId)
        {
            var ops = RT.Service.Resolve<TaskController>().GetOperators(taskIds, empId);
            EntityList<Operator> newOps = new EntityList<Operator>();
            taskIds.Where(p => !ops.Select(f => f.TaskManagementId).ToList().Contains(p)).ForEach(p =>
            {
                Operator op = new Operator() { EmployeeId = empId, TaskManagementId = p, IsMaster = true };
                newOps.Add(op);
            });
            RF.Save(newOps);
        }

        /// <summary>
        /// 获取任务集合
        /// </summary>
        /// <param name="action">委托：查询条件</param>
        /// <param name="elo">贪婪加载项</param>
        /// <returns>任务集合</returns>
        public virtual EntityList<TaskManagement> GetTasks(Action<IEntityQueryer<TaskManagement>> action = null, EagerLoadOptions elo = null)
        {
            var query = Query<TaskManagement>();
            action?.Invoke(query);
            return query.ToList(null, elo);
        }



        /// <summary>
        /// 任务状态改变
        /// </summary>
        public virtual void StateChange()
        {
            var config = ConfigService.GetConfig(new TaskParameterConfig(), typeof(TaskManagement));
            if (config != null && config.UntreatedTimeout > 0)
            {
                //超时未处理
                var taskList = GetTasks(query =>
                {
                    query.Where(p => p.State == TaskState.Executing);

                    //超时未处理任务，自动释放
                    query.Where(p => p.BeginDate < DateTime.Now.AddMinutes(-(double)config.UntreatedTimeout / 24 / 60));
                });

                ReleaseTasks(taskList);

            }
        }

        /// <summary>
        /// 获取盘点任务
        /// </summary>
        /// <param name="operationType">操作类型</param>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="taskGroupId">任务组ID</param>
        /// <param name="taskId">任务ID</param>
        /// <param name="reCount">是否复盘</param>
        /// <param name="action">action</param>
        /// <returns>盘点任务</returns>
        public virtual List<TaskManagement> GetTaskManagementData(OperationType? operationType, double? warehouseId, double? taskGroupId, double? taskId, bool reCount, Action<IEntityQueryer<TaskManagement>> action = null)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            var query = Query<TaskManagement>();
            if (reCount)
            {
                query.Where(p => p.State == TaskState.Finish);
            }
            else
            {
                query.Where(p => p.State == TaskState.Release || p.State == TaskState.Executing || p.State == TaskState.Appoint);
            }

            if (operationType.HasValue)
                query.Where(p => p.OperationType == operationType.Value);

            if (warehouseId.HasValue)
                query.Where(p => p.FromWarehouseId == warehouseId.Value);

            if (taskGroupId.HasValue)
                query.Where(p => p.TaskGroupId == taskGroupId.Value);

            query.LeftJoin<Operator>((a, b) => a.Id == b.TaskManagementId && b.IsMaster);
            action?.Invoke(query);

            EntityList<TaskManagement> tasks = new EntityList<TaskManagement>();

            elo.LoadWithViewProperty();

            var taskDatas = query.OrderBy(x => x.Id).ToList(null, elo);

            EntityList<TaskManagement> tmpTasks = new EntityList<TaskManagement>();
            foreach (var task in taskDatas)
            {
                if (task.State == TaskState.Appoint && task.OperatorList.Count > 0 && !task.OperatorList.Any(c => c.EmployeeId == RT.IdentityId))
                {
                    continue;
                }

                tmpTasks.Add(task);
            }

            if (taskId.HasValue)
            {
                foreach (var task in tmpTasks)
                {
                    if (task.Id > taskId)
                    {
                        tasks.Add(task);
                        break;
                    }
                }
            }
            else
            {
                tasks.AddRange(tmpTasks);
            }


            return tasks.ToList();
        }

        /// <summary>
        /// 获取创建状态的任务
        /// </summary>
        /// <returns>任务数据</returns>
        public virtual EntityList<TaskManagement> GetTaskManagementsForJob()
        {
            return Query<TaskManagement>().Where(p => p.State == TaskState.Create).OrderBy(p => p.ReleaseDate).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取建议目标站台创建、释放、指定、执行中状态的任务
        /// </summary>
        /// <param name="stationCode">站台编码</param>
        /// <returns>任务数</returns>
        public virtual int GetTaskManagementsForSuggestToStation(string stationCode)
        {
            var query = Query<TaskManagement>().Where(p => p.State == TaskState.Create || p.State == TaskState.Release || p.State == TaskState.Appoint || p.State == TaskState.Executing);

            if (stationCode.IsNotEmpty())
            {
                query.Where(p => p.SuggestToStation.Contains(stationCode));
            }

            return query.Count();
        }

        /// <summary>
        /// 获取指定操作人
        /// </summary>
        /// <param name="taskIds">任务Id</param>
        /// <param name="employeeId">员工列表</param>
        /// <returns>指定操作人列表</returns>
        public virtual EntityList<Operator> GetOperators(List<double> taskIds, double employeeId)
        {
            var query = Query<Operator>().Where(p => p.EmployeeId == employeeId);
            return taskIds.SplitContains(sons =>
            {
                query.Where(p => sons.Contains(p.TaskManagementId));
                return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取指定操作人
        /// </summary>
        /// <param name="taskId">任务Id</param>
        /// <returns>指定操作人列表</returns>
        public virtual EntityList<Operator> GetOperators(double taskId)
        {
            return Query<Operator>().Where(p => p.TaskManagementId == taskId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取指定操作人数据
        /// </summary>
        /// <param name="idList">Id列表</param>
        /// <returns>指定操作人数据</returns>
        public virtual EntityList<Operator> GetOperatorList(List<double> idList)
        {
            return Query<Operator>().Where(p => idList.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 立库拣货-返回符合条件的任务数据
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="tasks">任务数据</param>
        /// <returns>返回符合条件的任务数据</returns>
        public virtual List<TaskManagement> GetPickUpTaskData(double warehouseId, EntityList<TaskManagement> tasks)
        {
            List<TaskManagement> effectTasks = new List<TaskManagement>();
            effectTasks.AddRange(tasks.Where(p => p.State != TaskState.Appoint).ToList());
            //指定状态的任务，只要指定列表中包含了当前操作人的
            var appointTaskIds = tasks.Where(p => p.State == TaskState.Appoint).Select(p => p.Id).ToList();
            if (appointTaskIds.Count > 0)
            {
                var ops = RT.Service.Resolve<TaskController>().GetOperators(appointTaskIds, RT.IdentityId);
                if (ops.Count > 0)
                {
                    var effectTaskIds = ops.Select(p => p.TaskManagementId).ToList();
                    effectTasks.AddRange(tasks.Where(p => effectTaskIds.Contains(p.Id)).ToList());
                }
            }
            return effectTasks;
        }

        /// <summary>
        /// 执行任务分配规则
        /// </summary>
        /// <remarks>原来这个方法都是异步执行，但是异步执行有个问题，就是在事务内，新的数据还没保存，异步执行找不到该数据更新报错</remarks>
        /// <param name="tasks">任务数据</param>
        public virtual void ExecuteTaskAllotRule(EntityList<TaskManagement> tasks)
        {
            if (!tasks.Any())
            {
                return;
            }

            List<OperationType> types = tasks.Select(p => p.OperationType).Distinct().ToList();
            List<string> storerCodes = tasks.Select(p => p.StorerCode).Distinct().ToList();
            var fromTasks = tasks.Where(p => (p.OperationType == OperationType.PutOn || p.OperationType == OperationType.PullOff || p.OperationType == OperationType.Move || p.OperationType == OperationType.Check) || ((p.OperationType == OperationType.Allot && p.OrderType != OrderType.AllocateIn) || (p.OperationType == OperationType.Replenish && p.OrderType != OrderType.ReplenishIn))).ToList();
            var toTasks = tasks.Where(p => (p.OperationType == OperationType.Allot && p.OrderType == OrderType.AllocateIn) || (p.OperationType == OperationType.Replenish && p.OrderType == OrderType.ReplenishIn)).ToList();
            List<double> whIds = fromTasks.Where(p => p.FromWarehouseId != null).Select(p => p.FromWarehouseId.Value).Distinct().ToList();
            List<double> toWhId = toTasks.Where(p => p.ToWarehouseId != null).Select(p => p.ToWarehouseId.Value).Distinct().ToList();
            whIds.AddRange(toWhId);
            List<double> locIds = fromTasks.Where(p => p.SuggestFromLocId != null).Select(p => p.SuggestFromLocId.Value).Distinct().ToList();
            List<double> toLocIds = toTasks.Where(p => p.SuggestToLocId != null).Select(p => p.SuggestToLocId.Value).Distinct().ToList();
            locIds.AddRange(toLocIds);

            whIds = whIds.Distinct().ToList();
            locIds = locIds.Distinct().ToList();

            List<double> itemIds = tasks.Where(p => p.ItemId != null).Select(p => p.ItemId.Value).Distinct().ToList();
            var itemCategoryRelations = RT.Service.Resolve<SIE.Items.ItemController>().GetItemCategoryRelationByCategoryTypes(itemIds, CategoryType.Item);
            var itemCategorys = itemCategoryRelations.Select(p => p.ItemCategory).ToList();

            var taskAllotRules = RT.Service.Resolve<RuleController>().GetTaskAllotRuleData(types, storerCodes, whIds, locIds, itemCategorys);

            var logicLocs = RT.Service.Resolve<WarehouseController>().GetLogicAreaLocByLocIds(locIds);

            foreach (var task in tasks)
            {
                double warehouseId = 0;
                double? logicId = null;
                if ((task.OperationType == OperationType.PutOn || task.OperationType == OperationType.PullOff || task.OperationType == OperationType.Move || task.OperationType == OperationType.Check) && ((task.OperationType == OperationType.Allot && task.OrderType != OrderType.AllocateIn) || (task.OperationType == OperationType.Replenish && task.OrderType != OrderType.ReplenishIn)))
                {
                    warehouseId = whIds.FirstOrDefault(t => t == task.FromWarehouseId.Value);
                    if (task.SuggestFromLocId.HasValue)
                    {
                        logicId = logicLocs.FirstOrDefault(t => t.StorageLocationId == task.SuggestFromLocId.Value)?.LogicAreaId;
                    }
                }

                if (task.ToWarehouseId != null && ((task.OperationType == OperationType.Allot && task.OrderType == OrderType.AllocateIn) || (task.OperationType == OperationType.Replenish && task.OrderType == OrderType.ReplenishIn)))
                {
                    warehouseId = whIds.FirstOrDefault(t => t == task.ToWarehouseId.Value);
                    if (task.SuggestToLocId.HasValue)
                    {
                        logicId = logicLocs.FirstOrDefault(t => t.StorageLocationId == task.SuggestToLocId.Value)?.LogicAreaId;
                    }
                }

                EntityList<ItemCategory> itemCategories = new EntityList<ItemCategory>();
                var itemCategory = itemCategoryRelations.FirstOrDefault(t => t.ItemId == task.ItemId.Value)?.ItemCategory;
                if (itemCategory != null)
                {
                    if (itemCategory.TreePId == null)
                    {
                        itemCategories.Add(itemCategory);
                    }
                    else
                    {
                        GetItemCateGoryFuns(itemCategory.TreePId, itemCategories);
                    }
                }

                List<double> itemCategorieIds = itemCategories.Select(p => p.Id).Distinct().ToList();
                var allotRule = taskAllotRules.Where(p => (p.OperationType == task.OperationType || p.OperationType == null) && (p.StorerCode == task.StorerCode || p.StorerCode.IsNullOrEmpty()) || (p.WarehouseId == warehouseId || p.WarehouseId == null) && (p.LogicAreaId == logicId || p.LogicAreaId == null) && (itemCategorieIds.Contains((double)p.ItemCategoryId) || p.ItemCategoryId == null)).OrderBy(t => t.Priority).ThenBy(p => p.OperationType).ThenBy(p => p.StorerCode).ThenBy(p => p.LogicAreaCode).ThenBy(p => p.WarehouseId).ThenByDescending(p => p.ItemCategoryLevelId).FirstOrDefault();

                if (allotRule == null || !allotRule.EmployeeList.Any(t => t.EmployeeStatus == EmployeeStatus.Job))
                {
                    continue;
                }

                List<double> empList = allotRule.EmployeeList.Select(p => p.EmployeeId).Distinct().ToList();

                //保存指定操作人
                SaveTaskOperator(task, empList);

                task.State = GetTaskState(task);

                RF.Save(task);
            }
        }

        /// <summary>
        /// 递归获取物料库存分类
        /// </summary>
        /// <param name="parentId">父ID</param>
        /// <param name="itemCategories">物料库存分类</param>
        public virtual void GetItemCateGoryFuns(double? parentId, EntityList<ItemCategory> itemCategories)
        {
            if (parentId == null)
                return;
            var tempItemCategory = RT.Service.Resolve<SIE.Items.ItemController>().GetItemCategory(parentId);
            if (tempItemCategory != null)
            {
                itemCategories.Add(tempItemCategory);
                GetItemCateGoryFuns(tempItemCategory.TreePId, itemCategories);
            }
        }


    }
}
