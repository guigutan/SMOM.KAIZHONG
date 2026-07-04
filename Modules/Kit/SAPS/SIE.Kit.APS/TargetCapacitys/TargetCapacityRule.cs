using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace SIE.Kit.APS.TargetCapacitys
{
    /// <summary>
    /// 目标产能验证规则
    /// </summary>
    [DisplayName("目标产能重复验证")]
    [Description("目标产能不能重复")]
    public class TargetCapacityRule : NotDuplicateRule<TargetCapacity>
    {
        /// <summary>
        /// 不重复规则
        /// </summary>
        public TargetCapacityRule()
        {
            Properties.Add(TargetCapacity.EnterpriseIdProperty);
            Properties.Add(TargetCapacity.YearProperty);
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            this.MessageBuilder = e =>
            {
                var r = e as TargetCapacity;
                return "目标产能已经存在[工厂]{0}与[年份]{1}的数据".L10nFormat(r.Enterprise.Name, r.Year);
            };
        }
    }


    /// <summary>
    /// 目标产能验证规则
    /// </summary>
    [DisplayName("目标产能年份验证")]
    [Description("目标产能年份验证")]
    public class TargetCapacityYearRule : EntityRule<TargetCapacity>
    {
        /// <summary>
        /// 实体作用范围
        /// </summary>
        public TargetCapacityYearRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var targetcapacity = entity as TargetCapacity;
            var reg = new Regex(@"^[1-9]\d{3}$");
            var result = reg.IsMatch(targetcapacity.Year);
            if (!result)
            {
                e.BrokenDescription = "年份不正确!".L10N();
            }
        }
    }
}
