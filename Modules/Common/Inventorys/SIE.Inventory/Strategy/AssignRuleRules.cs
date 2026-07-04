using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;

namespace SIE.Inventory.Strategy
{
    /// <summary>
    /// 库存调整验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("分配规则验证规则")]
    [System.ComponentModel.Description("分配规则验证规则")]
    public class AssignRuleValidateRule : EntityRule<AssignRule>
    {
        /// <summary>
        /// 初始化需要验证的属性、影响范围、规则
        /// </summary>
        public AssignRuleValidateRule()
        {
            ConnectToDataSource = true;
            Property = AssignRule.IdProperty;
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var assignRule = entity as AssignRule;
            RT.Service.Resolve<RuleController>().ValidAssignRule(assignRule);
        }
    }
}
