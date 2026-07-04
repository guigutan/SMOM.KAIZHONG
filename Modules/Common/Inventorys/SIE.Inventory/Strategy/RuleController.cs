using SIE.Domain;
using System.Collections.Generic;

namespace SIE.Inventory.Strategy
{
    /// <summary>
    /// 规则控制器
    /// </summary>
    public partial class RuleController : DomainController
    {
        /// <summary>
        /// 根据周转编码获取周转规则
        /// </summary>
        /// <param name="code">周转编码</param>
        /// <returns>返回周转规则</returns>
        public virtual TurnOverRule GetTurnOverRule(string code)
        {
            return Query<TurnOverRule>().Where(p => p.Code == code).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据周转编码获取周转规则
        /// </summary>
        /// <param name="turnoverIds">周转规则</param>
        /// <returns>返回周转规则</returns>
        public virtual EntityList<TurnOverRuleDetail> GetTurnOverRuleDtls(List<double> turnoverIds)
        {
            return Query<TurnOverRuleDetail>().Where(p => turnoverIds.Contains(p.TurnOverRuleId) && p.TurnOverRule.State == State.Enable).ToList();
        }

        /// <summary>
        /// 根据分配编码获取分配规则
        /// </summary>
        /// <param name="code">分配编码</param>
        /// <returns>返回分配规则</returns>
        public virtual AssignRule GetAssignRule(string code)
        {
            return Query<AssignRule>().Where(p => p.Code == code).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据分配编码获取分配规则
        /// </summary>
        /// <param name="assignRuleIds">分配规则</param>
        /// <returns>返回周转规则</returns>
        public virtual EntityList<AssignRuleDetail> GetAssignRuleDtls(List<double> assignRuleIds)
        {
            return Query<AssignRuleDetail>().Where(p => assignRuleIds.Contains(p.AssignRuleId) && p.AssignRule.State == State.Enable).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 分配规则行号是否已经存在
        /// </summary>
        /// <param name="assignId">分配规则</param>
        /// <param name="lineNo">行号</param>
        /// <returns>bool</returns>
        public virtual bool AssignRuleDetailIsExistLineNo(double assignId, int lineNo)
        {
            return Query<AssignRuleDetail>().Where(p => p.AssignRuleId == assignId && p.LineNo == lineNo).Count() > 0;
        }

        /// <summary>
        /// 周转规则行号是否存在
        /// </summary>
        /// <param name="turnOverId">周转规则Id</param>
        /// <param name="lineNo">行号</param>
        /// <returns>bool</returns>
        public virtual bool TurnOverRuleDetailIsExistLineNo(double turnOverId, int lineNo)
        {
            return Query<TurnOverRuleDetail>().Where(p => p.TurnOverRuleId == turnOverId && p.LineNo == lineNo).Count() > 0;
        }

        /// <summary>
        /// 上架规则行号是否存在
        /// </summary>
        /// <param name="onshelvesId">上架规则Id</param>
        /// <param name="lineNo">行号</param>
        /// <returns>bool</returns>
        public virtual bool OnshelvesRuleDetailIsExistLineNo(double onshelvesId, int lineNo)
        {
            return Query<OnShelvesRuleDetail>().Where(p => p.OnShelvesRuleId == onshelvesId && p.LineNo == lineNo).Count() > 0;
        }
    }
}
