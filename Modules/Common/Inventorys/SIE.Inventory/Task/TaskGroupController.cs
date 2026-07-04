using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Inventory.Task
{
    /// <summary>
    /// 任务组控制器
    /// </summary>
    public class TaskGroupController : DomainController
    {
        /// <summary>
        /// 查询任务组数据
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="action">查询条件委托</param>
        /// <param name="page">分页信息</param>
        /// <returns>任务组数据</returns>
        public virtual EntityList<TaskGroup> GetTaskGroupDatas(double? warehouseId, Action<IEntityQueryer<TaskGroup>> action = null, PagingInfo page = null)
        {
            var query = Query<TaskGroup>();
            if (warehouseId.HasValue)
                query.Where(p => p.WarehouseId == warehouseId.Value);

            action?.Invoke(query);
            return query.ToList(page, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取任务数据s
        /// </summary>
        /// <param name="warehouseId">仓库Id</param>
        /// <param name="taskGroupId">任务组Id</param>
        /// <param name="action">查询条件委托</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="order">排序信息</param>
        /// <returns>任务列表数据</returns>
        public virtual EntityList<TaskManagement> GetTaskDatas(double? warehouseId, double? taskGroupId, Action<IEntityQueryer<TaskManagement>> action = null, PagingInfo pagingInfo = null, IList<OrderInfo> order = null)
        {
            var query = Query<TaskManagement>();
            if (warehouseId.HasValue)
                query.Where(p => p.FromWarehouseId == warehouseId.Value);

            if (taskGroupId.HasValue)
                query.Where(p => p.TaskGroupId == taskGroupId.Value);

            action?.Invoke(query);
            EagerLoadOptions elo = new EagerLoadOptions();
            return query.OrderBy(order).ToList(pagingInfo, elo.LoadWithViewProperty());
        }

        /// <summary>
        /// 根据数量生产任务组号
        /// </summary>
        /// <param name="qty">数量</param>
        /// <returns>任务号集合</returns>
        public virtual List<string> GetTaskGroupNos(int qty)
        {
            var config = ConfigService.GetConfig(new NoConfig(), typeof(TaskGroup));
            if (config == null || config.BacodeRule == null)
                throw new ValidationException("未找到任务组号生成规则,请检查规则配置".L10N());
            return RT.Service.Resolve<NumberRuleController>()
                .GenerateSegment(config.BacodeRule.Id, qty).ToList();
        }
    }
}
