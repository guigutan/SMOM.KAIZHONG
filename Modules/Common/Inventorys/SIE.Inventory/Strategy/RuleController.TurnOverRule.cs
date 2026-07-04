using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.NumberRules;
using SIE.Core.Common;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Inventory.Strategy
{
    /// <summary>
    /// 周转规则控制器
    /// </summary>
    public partial class RuleController
    {
        /// <summary>
        /// 验证排序字段不能重复
        /// </summary>
        /// <param name="dicSortField">排序字段字典</param>
        /// <param name="sortField">排序字段</param>
        /// <exception cref="ValidationException"></exception>
        private void ValidateSortFileRepeat(Dictionary<int, SortField> dicSortField, SortField? sortField)
        {
            if (sortField.HasValue)
            {
                if (!dicSortField.ContainsKey((int)sortField.Value))
                    dicSortField.Add((int)sortField.Value, sortField.Value);
                else
                    throw new ValidationException("排序字段为:[{0}]不能重复".L10nFormat((sortField.Value).ToLabel()));
            }
        }

        /// <summary>
        /// 获取周转规则编号
        /// </summary>
        /// <returns>返回库存调整单号</returns>
        public virtual string GetTurnOverRuleCode()
        {
            var config = ConfigService.GetConfig(new NoConfig(), typeof(TurnOverRule));
            if (config == null || config.BacodeRule == null)
                throw new ValidationException("未找到编码生成规则,请检查规则配置".L10N());
            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.BacodeRule.Id, 1).FirstOrDefault();
        }

        /// <summary>
        /// 获取周转规则
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="isDefault">是否默认周转规则</param>
        /// <returns>周转规则</returns>
        public virtual TurnOverRule GetTurnOverRuleData(string code, bool? isDefault = false)
        {
            var query = Query<TurnOverRule>();
            if (isDefault.HasValue && isDefault.Value)
                query.Where(p => p.IsDefault == isDefault.Value);
            else
                query.Where(p => p.Code == code);
            query.Exists<TurnOverRuleDetail>((x, y) => y.Where(f => f.TurnOverRuleId == x.Id));
            return query.FirstOrDefault();
        }

        /// <summary>
        /// 获取非当前周转策略数据
        /// </summary>
        /// <returns>周转策略数据</returns>
        public virtual EntityList<TurnOverRule> GetNonCurrentTurnOverRules(double turnOverRuleId)
        {
            return Query<TurnOverRule>().Where(p => p.Id != turnOverRuleId).ToList();
        }

        /// <summary>
        /// 获取过滤条件分配规则
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="customerId">客户ID</param>
        /// <param name="orderType">订单类型</param>
        /// <returns>分配规则</returns>
        public virtual EntityList<TurnOverRule> GetFilterTurnOverRules(double? warehouseId, double? customerId, OrderType? orderType)
        {
            var query = Query<TurnOverRule>().Where(p => p.State == State.Enable);
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
            query.Exists<TurnOverRuleDetail>((x, y) => y.Where(f => f.TurnOverRuleId == x.Id));

            return query.OrderBy(p => p.CustomerId).OrderBy(p => p.WarehouseId).OrderBy(p => p.OrderType).OrderByDescending(p => p.IsDefault).ToList();
        }

        /// <summary>
        /// 初始化周转规则
        /// </summary>
        public virtual void InitTurnOverRule()
        {
            TurnOverRule turnOverRule = new TurnOverRule();
            turnOverRule.Code = TurnOverRule.Default;
            turnOverRule.Name = "默认标准周转规则".L10N();
            turnOverRule.State = State.Enable;
            turnOverRule.IsDefault = true;
            turnOverRule.Description = "当物料使用的周转、分配规则不能按需求分配库存时，会用此规则替换掉原规则重新执行分配。".L10N();
            TurnOverRuleDetail turnOverRuleDetail = new TurnOverRuleDetail()
            {
                LineNo = 1,
                SortField1 = SortField.LotAtt03,
                FieldType1 = Commom.DataType.Date,
                SortType1 = SortType.Asc
            };

            for (int i = 1; i <= 5; i++)
            {
                TurnOverDefinition turnOverDefinition = new TurnOverDefinition();
                turnOverDefinition.Sequence = i;
                if (i == 1)
                {
                    turnOverDefinition.SortField = SortField.LotAtt03;
                    turnOverDefinition.FieldType = Commom.DataType.Date;
                    turnOverDefinition.SortType = SortType.Asc;
                }

                turnOverRuleDetail.DefinitionList.Add(turnOverDefinition);
            }

            turnOverRule.DetailList.Add(turnOverRuleDetail);


            RF.Save(turnOverRule);
        }

        /// <summary>
        /// 验证周转规则明细
        /// </summary>
        /// <param name="turnOverRuleDtl">周转规则明细</param>
        public virtual void ValidValidTurnOverRuleDetail(TurnOverRuleDetail turnOverRuleDtl)
        {
            Dictionary<int, SortField> dicSortField = new Dictionary<int, SortField>();
            if (turnOverRuleDtl == null)
            {
                throw new ValidationException("周转规则明细不能为空".L10N());
            }
            if (!turnOverRuleDtl.SortField1.HasValue)
                throw new ValidationException("排序字段1不能空;".L10N());

            if (turnOverRuleDtl.SortField1.HasValue && !turnOverRuleDtl.SortType1.HasValue)
                throw new ValidationException("排序字段1不为空时,排序方式1不能为空;".L10N());

            if (turnOverRuleDtl.SortField2.HasValue && !turnOverRuleDtl.SortType2.HasValue)
                throw new ValidationException("排序字段2不为空时,排序方式2不能为空;".L10N());

            if (turnOverRuleDtl.SortField3.HasValue && !turnOverRuleDtl.SortType3.HasValue)
                throw new ValidationException("排序字段3不为空时,排序方式3不能为空;".L10N());

            if (turnOverRuleDtl.SortField4.HasValue && !turnOverRuleDtl.SortType4.HasValue)
                throw new ValidationException("排序字段4不为空时,排序方式4不能为空;".L10N());

            if (turnOverRuleDtl.SortField5.HasValue && !turnOverRuleDtl.SortType5.HasValue)
                throw new ValidationException("排序字段5不为空时,排序方式5不能为空;".L10N());

            if (!turnOverRuleDtl.SortField1.HasValue && (turnOverRuleDtl.SortType1.HasValue || turnOverRuleDtl.UpperLimit1.HasValue || !turnOverRuleDtl.EqualValue1.IsNullOrEmpty() ||
                turnOverRuleDtl.UpperLimitDay1.HasValue || turnOverRuleDtl.LowerLimit1.HasValue || turnOverRuleDtl.LowerLimitDay1.HasValue))
                throw new ValidationException("排序字段1为空时,其它字段不需要维护;".L10N());

            if (!turnOverRuleDtl.SortField2.HasValue && (turnOverRuleDtl.SortType2.HasValue || turnOverRuleDtl.UpperLimit2.HasValue || !turnOverRuleDtl.EqualValue2.IsNullOrEmpty() ||
                turnOverRuleDtl.UpperLimitDay2.HasValue || turnOverRuleDtl.LowerLimit2.HasValue || turnOverRuleDtl.LowerLimitDay2.HasValue))
                throw new ValidationException("排序字段2为空时,其它字段不需要维护;".L10N());

            if (!turnOverRuleDtl.SortField3.HasValue && (turnOverRuleDtl.SortType3.HasValue || turnOverRuleDtl.UpperLimit3.HasValue || !turnOverRuleDtl.EqualValue3.IsNullOrEmpty() ||
                turnOverRuleDtl.UpperLimitDay3.HasValue || turnOverRuleDtl.LowerLimit3.HasValue || turnOverRuleDtl.LowerLimitDay3.HasValue))
                throw new ValidationException("排序字段3为空时,其它字段不需要维护;".L10N());

            if (!turnOverRuleDtl.SortField4.HasValue && (turnOverRuleDtl.SortType4.HasValue || turnOverRuleDtl.UpperLimit4.HasValue || !turnOverRuleDtl.EqualValue4.IsNullOrEmpty() ||
                turnOverRuleDtl.UpperLimitDay4.HasValue || turnOverRuleDtl.LowerLimit4.HasValue || turnOverRuleDtl.LowerLimitDay4.HasValue))
                throw new ValidationException("排序字段4为空时,其它字段不需要维护;".L10N());

            if (!turnOverRuleDtl.SortField5.HasValue && (turnOverRuleDtl.SortType5.HasValue || turnOverRuleDtl.UpperLimit5.HasValue || !turnOverRuleDtl.EqualValue5.IsNullOrEmpty() ||
                turnOverRuleDtl.UpperLimitDay5.HasValue || turnOverRuleDtl.LowerLimit5.HasValue || turnOverRuleDtl.LowerLimitDay5.HasValue))
                throw new ValidationException("排序字段5为空时,其它字段不需要维护;".L10N());

            if (turnOverRuleDtl.LowerLimit1.HasValue && turnOverRuleDtl.UpperLimit1.HasValue && turnOverRuleDtl.LowerLimit1.Value > turnOverRuleDtl.UpperLimit1.Value)
                throw new ValidationException("下限值1:[{0}]不能大于上限值1:{1}".L10nFormat(turnOverRuleDtl.LowerLimit1.Value, turnOverRuleDtl.UpperLimit1.Value));

            if (turnOverRuleDtl.LowerLimitDay1.HasValue && turnOverRuleDtl.UpperLimitDay1.HasValue && turnOverRuleDtl.LowerLimitDay1.Value > turnOverRuleDtl.UpperLimitDay1.Value)
                throw new ValidationException("下限天数1:[{0}]不能大于上限天数1:{1}".L10nFormat(turnOverRuleDtl.LowerLimitDay1.Value, turnOverRuleDtl.UpperLimitDay1.Value));

            if (turnOverRuleDtl.LowerLimit2.HasValue && turnOverRuleDtl.UpperLimit2.HasValue && turnOverRuleDtl.LowerLimit2.Value > turnOverRuleDtl.UpperLimit2.Value)
                throw new ValidationException("下限值2:[{0}]不能大于上限值2:{1}".L10nFormat(turnOverRuleDtl.LowerLimit2.Value, turnOverRuleDtl.UpperLimit2.Value));

            if (turnOverRuleDtl.LowerLimitDay2.HasValue && turnOverRuleDtl.UpperLimitDay2.HasValue && turnOverRuleDtl.LowerLimitDay2.Value > turnOverRuleDtl.UpperLimitDay2.Value)
                throw new ValidationException("下限天数2:[{0}]不能大于上限天数2:{1}".L10nFormat(turnOverRuleDtl.LowerLimitDay2.Value, turnOverRuleDtl.UpperLimitDay2.Value));

            if (turnOverRuleDtl.LowerLimit3.HasValue && turnOverRuleDtl.UpperLimit3.HasValue && turnOverRuleDtl.LowerLimit3.Value > turnOverRuleDtl.UpperLimit3.Value)
                throw new ValidationException("下限值3:[{0}]不能大于上限值3:{1}".L10nFormat(turnOverRuleDtl.LowerLimit3.Value, turnOverRuleDtl.UpperLimit3.Value));

            if (turnOverRuleDtl.LowerLimitDay3.HasValue && turnOverRuleDtl.UpperLimitDay3.HasValue && turnOverRuleDtl.LowerLimitDay3.Value > turnOverRuleDtl.UpperLimitDay3.Value)
                throw new ValidationException("下限天数3:[{0}]不能大于上限天数3:{1}".L10nFormat(turnOverRuleDtl.LowerLimitDay3.Value, turnOverRuleDtl.UpperLimitDay3.Value));

            if (turnOverRuleDtl.LowerLimit4.HasValue && turnOverRuleDtl.UpperLimit4.HasValue && turnOverRuleDtl.LowerLimit4.Value > turnOverRuleDtl.UpperLimit4.Value)
                throw new ValidationException("下限值4:[{0}]不能大于上限值4:{1}".L10nFormat(turnOverRuleDtl.LowerLimit4.Value, turnOverRuleDtl.UpperLimit4.Value));

            if (turnOverRuleDtl.LowerLimitDay4.HasValue && turnOverRuleDtl.UpperLimitDay4.HasValue && turnOverRuleDtl.LowerLimitDay4.Value > turnOverRuleDtl.UpperLimitDay4.Value)
                throw new ValidationException("下限天数4:[{0}]不能大于上限天数4:{1}".L10nFormat(turnOverRuleDtl.LowerLimitDay4.Value, turnOverRuleDtl.UpperLimitDay4.Value));

            if (turnOverRuleDtl.LowerLimit5.HasValue && turnOverRuleDtl.UpperLimit5.HasValue && turnOverRuleDtl.LowerLimit5.Value > turnOverRuleDtl.UpperLimit5.Value)
                throw new ValidationException("下限值5:[{0}]不能大于上限值5:{1}".L10nFormat(turnOverRuleDtl.LowerLimit5.Value, turnOverRuleDtl.UpperLimit5.Value));

            if (turnOverRuleDtl.LowerLimitDay5.HasValue && turnOverRuleDtl.UpperLimitDay5.HasValue && turnOverRuleDtl.LowerLimitDay5.Value > turnOverRuleDtl.UpperLimitDay5.Value)
                throw new ValidationException("下限天数5:[{0}]不能大于上限天数5:{1}".L10nFormat(turnOverRuleDtl.LowerLimitDay5.Value, turnOverRuleDtl.UpperLimitDay5.Value));

            const string sortFieldMsg = "维护的排序字段必须连续!";
            if (!turnOverRuleDtl.SortField1.HasValue && (turnOverRuleDtl.SortField2.HasValue || turnOverRuleDtl.SortField3.HasValue ||
                turnOverRuleDtl.SortField4.HasValue || turnOverRuleDtl.SortField5.HasValue))
                throw new ValidationException(sortFieldMsg.L10N());

            if (!turnOverRuleDtl.SortField2.HasValue && (turnOverRuleDtl.SortField3.HasValue || turnOverRuleDtl.SortField4.HasValue ||
                turnOverRuleDtl.SortField5.HasValue))
                throw new ValidationException(sortFieldMsg.L10N());

            if (!turnOverRuleDtl.SortField3.HasValue && (turnOverRuleDtl.SortField4.HasValue || turnOverRuleDtl.SortField5.HasValue))
                throw new ValidationException(sortFieldMsg.L10N());

            if (!turnOverRuleDtl.SortField4.HasValue && turnOverRuleDtl.SortField5.HasValue)
                throw new ValidationException(sortFieldMsg.L10N());

            ////验证排序字段1不能重复
            ValidateSortFileRepeat(dicSortField, turnOverRuleDtl.SortField1);
            ////验证排序字段2不能重复
            ValidateSortFileRepeat(dicSortField, turnOverRuleDtl.SortField2);
            ////验证排序字段3不能重复
            ValidateSortFileRepeat(dicSortField, turnOverRuleDtl.SortField3);
            ////验证排序字段4不能重复
            ValidateSortFileRepeat(dicSortField, turnOverRuleDtl.SortField4);
            ////验证排序字段5不能重复
            ValidateSortFileRepeat(dicSortField, turnOverRuleDtl.SortField5);
        }

        /// <summary>
        /// 验证周转定义
        /// </summary>
        /// <param name="turnOverRuleDtl">周转规则明细</param>
        public virtual void ValidTurnOverDefinition(TurnOverRuleDetail turnOverRuleDtl)
        {
            Dictionary<int, SortField> dicSortField = new Dictionary<int, SortField>();
            SortField? tempSortField = null;
            turnOverRuleDtl.DefinitionList.ForEach(p =>
            {
                if (p.Sequence == 1 && !p.SortField.HasValue)
                    throw new ValidationException("顺序号为:[1]的排序字段不能空;".L10N());

                if (p.SortField.HasValue && !p.SortType.HasValue)
                    throw new ValidationException("排序字段不为空时,排序方式不能为空;".L10N());

                if (!p.SortField.HasValue && (p.SortType.HasValue || p.UpperLimit.HasValue || p.UpperLimitDay.HasValue ||
                    p.LowerLimit.HasValue || p.LowerLimitDay.HasValue))
                    throw new ValidationException("排序字段为空时,其它字段不需要维护;".L10N());

                if (p.SortField.HasValue)
                {
                    if (!dicSortField.ContainsKey((int)p.SortField.Value))
                        dicSortField.Add((int)p.SortField.Value, p.SortField.Value);
                    else
                        throw new ValidationException("排序字段为:[{0}]不能重复".L10nFormat((p.SortField.Value).ToLabel()));
                }

                if (p.LowerLimit.HasValue && p.UpperLimit.HasValue && p.LowerLimit.Value > p.UpperLimit.Value)
                    throw new ValidationException("下限值:[{0}]不能大于上限值:{1}".L10nFormat(p.LowerLimit.Value, p.UpperLimit.Value));

                if (p.LowerLimitDay.HasValue && p.UpperLimitDay.HasValue && p.LowerLimitDay.Value > p.UpperLimitDay.Value)
                    throw new ValidationException("下限天数:[{0}]不能大于上限天数:{1}".L10nFormat(p.LowerLimitDay.Value, p.UpperLimitDay.Value));

                if (p.SortField.HasValue && p.Sequence != 1 && !tempSortField.HasValue)
                    throw new ValidationException("维护的排序字段必须连续!".L10N());

                tempSortField = p.SortField;
            });
        }

        /// <summary>
        /// 获取可用的周转规则
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">查询关键字</param>
        /// <returns>周转规则信息</returns>
        public virtual EntityList<TurnOverRule> GetEnableTurnOverRule(PagingInfo pagingInfo, string keyword)
        {
            var query = Query<TurnOverRule>().Where(p => p.State == State.Enable);
            if (!keyword.IsNullOrEmpty())
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            query.Exists<TurnOverRuleDetail>((x, y) => y.Where(f => f.TurnOverRuleId == x.Id));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取周转规则
        /// </summary>
        /// <param name="turnOverRuleIds">周转规则ID集合</param>
        /// <returns>周转规则信息</returns>
        public virtual EntityList<TurnOverRule> GetTurnOverRules(List<double> turnOverRuleIds)
        {
            if (turnOverRuleIds == null || turnOverRuleIds.Count == 0)
            {
                return new EntityList<TurnOverRule>();
            }
            return turnOverRuleIds.SplitContains(sons =>
            {
                return Query<TurnOverRule>().Where(p => sons.Contains(p.Id)).ToList();
            });
        }

        /// <summary>
        /// 保存周转明细
        /// </summary>
        /// <param name="turnOverRuleDtl"></param>
        /// <returns></returns>
        public virtual TurnOverRuleDetail SaveTurnOverRuleDetail(TurnOverRuleDetail turnOverRuleDtl)
        {
            turnOverRuleDtl.DefinitionList.OrderBy(p => p.Sequence).ForEach(p =>
            {
                switch (p.Sequence)
                {
                    case 1:
                        turnOverRuleDtl.SortField1 = p.SortField;
                        turnOverRuleDtl.FieldType1 = p.FieldType;
                        turnOverRuleDtl.SortType1 = p.SortType;
                        turnOverRuleDtl.UpperLimit1 = p.UpperLimit;
                        turnOverRuleDtl.LowerLimit1 = p.LowerLimit;
                        turnOverRuleDtl.UpperLimitDay1 = p.UpperLimitDay;
                        turnOverRuleDtl.LowerLimitDay1 = p.LowerLimitDay;
                        turnOverRuleDtl.EqualValue1 = p.EqualValue;
                        break;
                    case 2:
                        turnOverRuleDtl.SortField2 = p.SortField;
                        turnOverRuleDtl.FieldType2 = p.FieldType;
                        turnOverRuleDtl.SortType2 = p.SortType;
                        turnOverRuleDtl.UpperLimit2 = p.UpperLimit;
                        turnOverRuleDtl.LowerLimit2 = p.LowerLimit;
                        turnOverRuleDtl.UpperLimitDay2 = p.UpperLimitDay;
                        turnOverRuleDtl.LowerLimitDay2 = p.LowerLimitDay;
                        turnOverRuleDtl.EqualValue2 = p.EqualValue;
                        break;
                    case 3:
                        turnOverRuleDtl.SortField3 = p.SortField;
                        turnOverRuleDtl.FieldType3 = p.FieldType;
                        turnOverRuleDtl.SortType3 = p.SortType;
                        turnOverRuleDtl.UpperLimit3 = p.UpperLimit;
                        turnOverRuleDtl.LowerLimit3 = p.LowerLimit;
                        turnOverRuleDtl.UpperLimitDay3 = p.UpperLimitDay;
                        turnOverRuleDtl.LowerLimitDay3 = p.LowerLimitDay;
                        turnOverRuleDtl.EqualValue3 = p.EqualValue;
                        break;
                    case 4:
                        turnOverRuleDtl.SortField4 = p.SortField;
                        turnOverRuleDtl.FieldType4 = p.FieldType;
                        turnOverRuleDtl.SortType4 = p.SortType;
                        turnOverRuleDtl.UpperLimit4 = p.UpperLimit;
                        turnOverRuleDtl.LowerLimit4 = p.LowerLimit;
                        turnOverRuleDtl.UpperLimitDay4 = p.UpperLimitDay;
                        turnOverRuleDtl.LowerLimitDay4 = p.LowerLimitDay;
                        turnOverRuleDtl.EqualValue4 = p.EqualValue;
                        break;
                    case 5:
                        turnOverRuleDtl.SortField5 = p.SortField;
                        turnOverRuleDtl.FieldType5 = p.FieldType;
                        turnOverRuleDtl.SortType5 = p.SortType;
                        turnOverRuleDtl.UpperLimit5 = p.UpperLimit;
                        turnOverRuleDtl.LowerLimit5 = p.LowerLimit;
                        turnOverRuleDtl.UpperLimitDay5 = p.UpperLimitDay;
                        turnOverRuleDtl.LowerLimitDay5 = p.LowerLimitDay;
                        turnOverRuleDtl.EqualValue5 = p.EqualValue;
                        break;
                }
            });

            return turnOverRuleDtl;
        }

        /// <summary>
        /// 周转规则设置默认命令
        /// </summary>
        /// <param name="turnOverRuleId">周转策略ID</param>
        public virtual void SetIsDefaultTurnOverRuleData(double turnOverRuleId)
        {
            using (var tran = DB.TransactionScope(InveEntityDataProvider.ConnectionStringName))
            {
                TurnOverRule turnOverRule = RF.GetById<TurnOverRule>(turnOverRuleId);
                if (turnOverRule == null)
                    throw new ValidationException("周转规则不存在".L10N());
                if (!turnOverRule.DetailList.Any())
                    throw new ValidationException("当前周转规则没有添加明细".L10N());
                turnOverRule.IsDefault = true;
                RF.Save(turnOverRule);

                EntityList<TurnOverRule> turnOverRules = GetNonCurrentTurnOverRules(turnOverRuleId);
                turnOverRules.ForEach(p => p.IsDefault = false);
                RF.Save(turnOverRules);

                tran.Complete();
            }
        }
    }
}