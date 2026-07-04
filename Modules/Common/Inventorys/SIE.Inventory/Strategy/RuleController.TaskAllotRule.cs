using SIE.Common.Configs.CommonConfigs;
using SIE.Common.Configs;
using SIE.Common.Employees;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.Items;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using SIE.Domain.Validation;
using SIE.Inventory.Task;
using SIE.Items.Items;

namespace SIE.Inventory.Strategy
{
    /// <summary>
    /// 任务分配规则控制器
    /// </summary>
    public partial class RuleController
    {
        /// <summary>
        /// 获取任务分配规则数据
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>任务分配规则数据</returns>
        public virtual EntityList<TaskAllotRule> GetTaskAllotRules(TaskAllotRuleCriteria criteria)
        {
            var query = Query<TaskAllotRule>();
            if (criteria.Code.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(criteria.Code));
            }

            if (criteria.Name.IsNotEmpty())
            {
                query.Where(p => p.Name.Contains(criteria.Name));
            }

            if (criteria.OperationType.HasValue)
            {
                query.Where(p => p.OperationType == criteria.OperationType.Value);
            }

            if (criteria.WarehouseId.HasValue)
            {
                query.Where(p => p.WarehouseId == criteria.WarehouseId.Value);
            }

            if (criteria.LogicArea.IsNotEmpty())
            {
                query.Exists<LogicArea>((x, l) => l.Where(p => p.Id == x.LogicAreaId && (p.Code.Contains(criteria.LogicArea) || p.Name.Contains(criteria.LogicArea))));
            }

            if (criteria.ItemCategory.IsNotEmpty())
            {
                query.Exists<ItemCategory>((x, i) => i.Where(p => p.Id == x.ItemCategoryId && (p.Code.Contains(criteria.ItemCategory) || p.Name.Contains(criteria.ItemCategory))));
            }

            if (criteria.Employee.IsNotEmpty())
            {
                query.Join<TaskAllotRuleEmployee>((x, d) => x.Id == d.TaskAllotRuleId)
                     .Join<TaskAllotRuleEmployee, Employee>((d, e) => d.EmployeeId == e.Id)
                     .Where<TaskAllotRuleEmployee, Employee>((x, d, e) => e.Code.Contains(criteria.Employee) || e.Name.Contains(criteria.Name));
            }

            if (criteria.CreateDate.BeginValue.HasValue)
            {
                query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue.Value);
            }

            if (criteria.CreateDate.EndValue.HasValue)
            {
                query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue.Value);
            }

            query.OrderBy(criteria.OrderInfoList);
            return query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取任务分配规则编号
        /// </summary>
        /// <returns>返回任务分配规则编号</returns>
        public virtual string GetTaskAllotRuleCode()
        {
            var config = ConfigService.GetConfig(new NoConfig(), typeof(TaskAllotRule));
            if (config == null || config.BacodeRule == null)
                throw new ValidationException("未找到编码生成规则,请检查规则配置".L10N());
            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.BacodeRule.Id, 1).FirstOrDefault();
        }

        /// <summary>
        /// 删除员工信息
        /// </summary>
        /// <param name="empIds">员工ID集合</param>
        public virtual void DeleteTaskAllotRuleEmployees(List<double> empIds)
        {
            using (var tran = DB.TransactionScope(InveEntityDataProvider.ConnectionStringName))
            {
                empIds.SplitDataExecute(tmpIds =>
                {
                    DB.Delete<TaskAllotRuleEmployee>().Where(p => tmpIds.Contains(p.Id)).Execute();
                });

                tran.Complete();
            }
        }

        /// <summary>
        /// 获取任务分配规则
        /// </summary>
        /// <param name="types">任务操作类型</param>
        /// <param name="storerCodes">货主集合</param>
        /// <param name="whIds">仓库ID集合</param>
        ///// <param name="itemCategoryIds">库存分类ID集合</param>
        /// <param name="locIds">库位ID集合</param>
        /// <returns></returns>
        public virtual List<TaskAllotRule> GetTaskAllotRuleData(List<OperationType> types, List<string> storerCodes, List<double> whIds, List<double> locIds, List<ItemCategory> itemCategorys)
        {
            List<TaskAllotRule> taskAllotRules = new List<TaskAllotRule>();
            var query = Query<TaskAllotRule>().Where(p => p.State == State.Enable);
            if (types.Any())
            {
                query.Where(p => types.Contains((OperationType)p.OperationType) || p.OperationType == null);
            }

            if (storerCodes.Any())
            {
                query.Where(p => storerCodes.Contains(p.StorerCode) || p.StorerCode == null);
            }

            if (whIds.Any())
            {
                query.Where(p => whIds.Contains((double)p.WarehouseId) || p.WarehouseId == null);
            }

            var rules = query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            List<double> logicAreaIds = rules.Where(p => p.LogicAreaId != null).Select(p => p.LogicAreaId.Value).Distinct().ToList();
            if (locIds.Any() && logicAreaIds.Any())
            {
                var logicAreas = RT.Service.Resolve<WarehouseController>().GetLogicAreaByIds(logicAreaIds, locIds);
                List<double> tmpLogicAreaIds = logicAreas.Select(t => t.Id).Distinct().ToList();
                var tmpAllotRules = rules.Where(p => p.LogicAreaId == null || tmpLogicAreaIds.Contains(p.LogicAreaId ?? 0)).ToList();
                if (tmpAllotRules.Any())
                {
                    taskAllotRules.AddRange(tmpAllotRules);
                }
            }
            else
            {
                taskAllotRules.AddRange(rules);
            }

            List<TaskAllotRule> allotRules = new List<TaskAllotRule>();

            //过滤库存分类
            foreach (var item in taskAllotRules)
            {
                if (!item.ItemCategoryId.HasValue)
                {
                    allotRules.Add(item);
                    continue;
                }

                var itemCategory = itemCategorys.FirstOrDefault(t => t.Id == item.ItemCategoryId);
                if (itemCategory != null)
                {
                    EntityList<ItemCategory> itemCategories = new EntityList<ItemCategory>();
                    if (itemCategory.TreePId == null)
                    {
                        itemCategories.Add(itemCategory);
                    }
                    else
                    {
                        RT.Service.Resolve<TaskController>().GetItemCateGoryFuns(itemCategory.TreePId, itemCategories);
                    }

                    if (itemCategories.Any(t => t.Id == item.ItemCategoryId))
                    {
                        allotRules.Add(item);
                    }
                }
            }

            return allotRules;
        }
    }
}