using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;

namespace SIE.Packages.Packages
{
    /// <summary>
    /// 复核包装规则控制器
    /// </summary>
    public partial class RePackageController : DomainController
    {
        /// <summary>
        /// 根据客户ID获取不是当前规则的其他规则数
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="currentRuleId"></param>
        /// <returns></returns>
        public virtual int GetCustomerRuleCount(double? customerId, double currentRuleId)
        {
            var query = Query<RePackageRule>().Where(p => p.Id != currentRuleId);
            if (customerId.HasValue)
                query.Where(p => p.CustomerId == customerId);
            return query.Count();
        }

        /// <summary>
        /// 根据空客户规则
        /// </summary>
        /// <param name="currentRuleId"></param>
        /// <returns></returns>
        public virtual int GetNullCustomerCount(double currentRuleId)
        {
            var query = Query<RePackageRule>().Where(p => p.Id != currentRuleId);
            query.Where(p => p.CustomerId == null);
            return query.Count();
        }

        /// <summary>
        /// 获取装箱规则
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="boxType"></param>
        /// <returns></returns>
        public virtual RePackageRuleDetail GetRePackageRuleDetail(double? customerId, BoxType boxType)
        {
            var query = Query<RePackageRuleDetail>()
                .Join<RePackageRule>((p, r) => p.RePackageRuleId == r.Id && r.State == State.Enable);
            if (customerId.HasValue)
            {
                query.Where<RePackageRule>((p, r) => r.CustomerId == customerId.Value || r.CustomerId == null);
            }
            else
            {
                query.Where<RePackageRule>((p, r) => r.CustomerId == null);
            }
            query.Where(p => p.BoxType == boxType);
            query.OrderBy<RePackageRule>((p, r) => r.CustomerId);
            return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取复核包装规则
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual RePackageRule GetPackageRuleById(double id)
        {
            return Query<RePackageRule>().Where(p => p.Id == id).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 启用规则
        /// </summary>
        /// <param name="ruleIds"></param>
        public virtual void EnablePackageRules(List<double> ruleIds)
        {
            ruleIds.ForEach(id =>
            {
                var rule = GetPackageRuleById(id);
                var count = GetCustomerRuleCount(rule.CustomerId, id);
                if (count > 0)
                {
                    throw new ValidationException("同一客户[{0}]只能有一个规则启用".L10nFormat(rule.CustomerCode));
                }
                rule.State = State.Enable;
                RF.Save(rule);
            });
        }
    }
}
