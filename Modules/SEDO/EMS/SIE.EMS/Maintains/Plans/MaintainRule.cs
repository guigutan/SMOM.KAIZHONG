using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SIE.EMS.Maintains.Plans
{
    /// <summary>
    /// 保养计划实体验证规则
    /// </summary>
    [DisplayName("保养计划实体验证规则")]
    [Description("保养计划清单实体验证规则")]
    public class MaintainRule : EntityRule<MaintainPlan>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MaintainRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">MI实体</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var maintainPlan = entity as MaintainPlan;

            if (maintainPlan == null)
            {
                return;
            }
            if (maintainPlan.MaintainTime < 0)
            {
                e.BrokenDescription = "保养计划时长不允许为负数".L10N();
                return;
            }


        }
    }
}
