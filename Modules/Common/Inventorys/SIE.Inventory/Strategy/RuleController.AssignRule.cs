using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.NumberRules;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Inventory.Strategy
{
    /// <summary>
    /// 分配规则控制器
    /// </summary>
    public partial class RuleController
    {
        /// <summary>
        /// 获取分配规则
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>上架规则集合</returns>
        public virtual EntityList<AssignRule> GetAssignRule(AssignRuleCriteria criteria)
        {
            var query = Query<AssignRule>();
            if (!criteria.Code.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(criteria.Code));
            if (!criteria.Name.IsNullOrEmpty())
                query.Where(p => p.Name.Contains(criteria.Name));
            if (criteria.State.HasValue)
                query.Where(p => p.State == criteria.State);

            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();
            query.OrderBy(criteria.OrderInfoList);
            return query.ToList(criteria.PagingInfo, elo);
        }

        /// <summary>
        /// 获取分配规则
        /// </summary>
        /// <param name="ruleIds">分配规则</param>
        /// <returns></returns>
        public virtual EntityList<AssignRule> GetAssignRules(List<double> ruleIds)
        {
           return ruleIds.SplitContains(ids =>
            {
                return Query<AssignRule>().Where(p => ruleIds.Contains(p.Id)).ToList();
            });
        }

        /// <summary>
        /// 获取分配规则编号
        /// </summary>
        /// <returns>返回库存调整单号</returns>
        public virtual string GetAssignRuleCode()
        {
            var config = ConfigService.GetConfig(new NoConfig(), typeof(AssignRule));
            if (config == null || config.BacodeRule == null)
                throw new ValidationException("未找到编码生成规则,请检查规则配置".L10N());
            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.BacodeRule.Id, 1).FirstOrDefault();
        }

        /// <summary>
        /// 获取非当前分配策略数据
        /// </summary>
        /// <returns>分配策略数据</returns>
        public virtual EntityList<AssignRule> GetNonCurrentAssignRules(double assignRuleId)
        {
            return Query<AssignRule>().Where(p => p.Id != assignRuleId).ToList();
        }

        /// <summary>
        /// 获取分配规则
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="isDefault">是否获取默认分配规则</param>
        /// <returns>分配规则</returns>
        public virtual AssignRule GetAssignRuleData(string code, bool? isDefault = false)
        {
            var query = Query<AssignRule>();
            if (isDefault == true)
                query.Where(p => p.IsDefault == isDefault.Value);
            else
                query.Where(p => p.Code == code);

            return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取过滤条件分配规则
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="customerId">客户ID</param>
        /// <param name="orderType">订单类型</param>
        /// <returns>分配规则</returns>
        public virtual EntityList<AssignRule> GetFilterAssignRules(double? warehouseId, double? customerId, OrderType? orderType)
        {
            var query = Query<AssignRule>().Where(p => p.State == State.Enable);
            if (warehouseId.HasValue)
                query.Where(p => p.WarehouseId == warehouseId.Value || p.WarehouseId == null);
            else
                query.Where(p => p.WarehouseId == null);
            if (customerId.HasValue)
                query.Where(p => p.CustomerId == customerId.Value || p.CustomerId == null);
            else
                query.Where(p => p.CustomerId == null);
            if (orderType.HasValue)
                query.Where(p => p.OrderType == orderType.Value || p.OrderType == null);
            else
                query.Where(p => p.OrderType == null);

            return query.OrderBy(p => p.CustomerId).OrderBy(p => p.WarehouseId).OrderBy(p => p.OrderType).OrderByDescending(p => p.IsDefault).ToList();
        }

        /// <summary>
        /// 初始化分配规则
        /// </summary>
        public virtual void InitAssignRule()
        {
            AssignRule assignRule = new AssignRule();
            assignRule.Code = AssignRule.Default;
            assignRule.Name = "默认标准分配规则".L10N();
            assignRule.State = State.Enable;
            assignRule.IsDefault = true;
            assignRule.Description = "当物料使用的周转、分配规则不能按需求分配库存时，会用此规则替换掉原规则重新执行分配".L10N();
            assignRule.AssignRuleDetailList.Add(new AssignRuleDetail()
            {
                LineNo = 1,
                Sort1 = AssignSortType.TurnOver,
                State = Onhands.OnhandState.Ok,
            });

            RF.Save(assignRule);
        }

        /// <summary>
        /// 验证分配规则
        /// </summary>
        /// <param name="assignRule">分配规则</param>
        public virtual void ValidAssignRule(AssignRule assignRule)
        {
            if (assignRule.AssignRuleDetailList.Count == 0)
                throw new ValidationException("明细至少需要维护一条数据".L10N());

            assignRule.AssignRuleDetailList.ForEach(p =>
            {
                Dictionary<int, AssignSortType> dicAssignSortType = new Dictionary<int, AssignSortType>();
                if (p.Sort1.HasValue)
                    ValidatAssignSortType(dicAssignSortType, p.Sort1.Value);

                if (p.Sort2.HasValue)
                    ValidatAssignSortType(dicAssignSortType, p.Sort2.Value);

                if (p.Sort3.HasValue)
                    ValidatAssignSortType(dicAssignSortType, p.Sort3.Value);

                if (!p.Sort1.HasValue && (p.Sort2.HasValue || p.Sort3.HasValue))
                    throw new ValidationException("不能跳过前面排序直接维护后面排序".L10N());

                if (!p.Sort2.HasValue && p.Sort3.HasValue)
                    throw new ValidationException("不能跳过前面排序直接维护后面排序".L10N());

                if (p.Sort1.HasValue && p.Sort1.Value == AssignSortType.LpnQty && !p.LpnQtyMatchType.HasValue)
                    throw new ValidationException("排序1维护了可用数量,可用数量匹配规则的就不能为空".L10N());

                if (p.Sort2.HasValue && p.Sort2.Value == AssignSortType.LpnQty && !p.LpnQtyMatchType.HasValue)
                    throw new ValidationException("排序2维护了可用数量,可用数量匹配规则的就不能为空".L10N());

                if (p.Sort3.HasValue && p.Sort3.Value == AssignSortType.LpnQty && !p.LpnQtyMatchType.HasValue)
                    throw new ValidationException("排序3维护了可用数量,可用数量匹配规则的就不能为空".L10N());

                //if (p.AssignBase == AssignBase.PackageFirst && !p.PackageLevel.HasValue)
                //    throw new ValidationException("明细的分配原则为“整包优先”时，分配包装层级必须有值".L10N());

                //8）立库优先和立库靠后不能在同一行中同时存在，LPN优先和LPN靠后不能在同一行中同时存在 
                ValidatAssignDetail(p);
            });
        }

        /// <summary>
        /// 验证分配规则
        /// </summary>
        /// <param name="dicAssignSortType">排序字典</param>
        /// <param name="sort">排序字段</param>
        private void ValidatAssignSortType(Dictionary<int, AssignSortType> dicAssignSortType, AssignSortType sort)
        {
            if (dicAssignSortType == null)
                dicAssignSortType = new Dictionary<int, AssignSortType>();

            if (!dicAssignSortType.ContainsKey((int)sort))
                dicAssignSortType.Add((int)sort, sort);
            else
                throw new ValidationException("同一个排序:{0}只能维护一次".L10nFormat(sort.ToLabel()));
        }

        /// <summary>
        /// 验证分配规则明细
        /// </summary>
        /// <param name="ruleDetail">分配明细</param>
        private void ValidatAssignDetail(AssignRuleDetail ruleDetail)
        {
            Dictionary<int, AssignSortType> dicAutomated = new Dictionary<int, AssignSortType>();

            if ((ruleDetail.Sort1 == AssignSortType.AutomatedFirst || ruleDetail.Sort1 == AssignSortType.AutomatedLast) && !dicAutomated.ContainsKey((int)ruleDetail.Sort1))
            {
                dicAutomated.Add((int)AssignSortType.AutomatedFirst, AssignSortType.AutomatedFirst);
                dicAutomated.Add((int)AssignSortType.AutomatedLast, AssignSortType.AutomatedLast);
            }

            if ((ruleDetail.Sort2 == AssignSortType.AutomatedFirst || ruleDetail.Sort2 == AssignSortType.AutomatedLast))
            {
                if (!dicAutomated.ContainsKey((int)ruleDetail.Sort2))
                {
                    dicAutomated.Add((int)AssignSortType.AutomatedFirst, AssignSortType.AutomatedFirst);
                    dicAutomated.Add((int)AssignSortType.AutomatedLast, AssignSortType.AutomatedLast);
                }
                else
                    throw new ValidationException("立库优先和立库靠后不能在同一行中同时存在".L10N());
            }

            if ((ruleDetail.Sort3 == AssignSortType.AutomatedFirst || ruleDetail.Sort3 == AssignSortType.AutomatedLast))
            {
                if (!dicAutomated.ContainsKey((int)ruleDetail.Sort3))
                {
                    dicAutomated.Add((int)AssignSortType.AutomatedFirst, AssignSortType.AutomatedFirst);
                    dicAutomated.Add((int)AssignSortType.AutomatedLast, AssignSortType.AutomatedLast);
                }
                else
                    throw new ValidationException("立库优先和立库靠后不能在同一行明细中同时存在".L10N());
            }

            Dictionary<int, AssignSortType> dicLpnSort = new Dictionary<int, AssignSortType>();
            if ((ruleDetail.Sort1 == AssignSortType.LpnFirst || ruleDetail.Sort1 == AssignSortType.LpnLast) && !dicLpnSort.ContainsKey((int)ruleDetail.Sort1))
            {
                dicLpnSort.Add((int)AssignSortType.LpnFirst, AssignSortType.LpnFirst);
                dicLpnSort.Add((int)AssignSortType.LpnLast, AssignSortType.LpnLast);
            }

            if ((ruleDetail.Sort2 == AssignSortType.LpnFirst || ruleDetail.Sort2 == AssignSortType.LpnLast))
            {
                if (!dicLpnSort.ContainsKey((int)ruleDetail.Sort2))
                {
                    dicLpnSort.Add((int)AssignSortType.LpnFirst, AssignSortType.LpnFirst);
                    dicLpnSort.Add((int)AssignSortType.LpnLast, AssignSortType.LpnLast);
                }
                else
                    throw new ValidationException("LPN优先和LPN靠后不能在同一行明细中同时存在".L10N());
            }

            if ((ruleDetail.Sort3 == AssignSortType.LpnFirst || ruleDetail.Sort3 == AssignSortType.LpnLast))
            {
                if (!dicLpnSort.ContainsKey((int)ruleDetail.Sort3))
                {
                    dicLpnSort.Add((int)AssignSortType.LpnFirst, AssignSortType.LpnFirst);
                    dicLpnSort.Add((int)AssignSortType.LpnLast, AssignSortType.LpnLast);
                }
                else
                    throw new ValidationException("LPN优先和LPN靠后不能在同一行明细中同时存在".L10N());
            }
        }

        /// <summary>
        /// 获取可用的分配规则
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">查询关键字</param>
        /// <returns>上架规则信息</returns>
        public virtual EntityList<AssignRule> GetEnableAssignRule(PagingInfo pagingInfo, string keyword)
        {
            var query = Query<AssignRule>().Where(p => p.State == State.Enable);
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));

            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取分配规则明细
        /// </summary>
        /// <param name="assignRuleId">分配规则Id</param>
        /// <returns>分配规则明细</returns>
        public virtual EntityList<AssignRuleDetail> GetAssignRuleDetails(double assignRuleId)
        {
            return Query<AssignRuleDetail>().Where(p => p.AssignRuleId == assignRuleId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 分配规则设置默认命令
        /// </summary>
        /// <param name="assignRuleId">分配策略ID</param>
        public virtual void SetIsDefaultAssignRuleData(double assignRuleId)
        {
            using (var tran = DB.TransactionScope(InveEntityDataProvider.ConnectionStringName))
            {
                AssignRule assignRule = RF.GetById<AssignRule>(assignRuleId);
                if (assignRule == null)
                    throw new ValidationException("分配规则不存在".L10N());

                assignRule.IsDefault = true;
                RF.Save(assignRule);

                EntityList<AssignRule> assignRules = GetNonCurrentAssignRules(assignRuleId);
                assignRules.ForEach(p => p.IsDefault = false);
                RF.Save(assignRules);

                tran.Complete();
            }
        }
    }
}
