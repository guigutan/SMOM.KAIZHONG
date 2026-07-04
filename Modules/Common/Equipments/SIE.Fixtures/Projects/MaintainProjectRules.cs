using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.ComponentModel;

namespace SIE.Fixtures.Projects
{
    /// <summary>
    /// 保养项目验证规则
    /// </summary>
    [DisplayName("保养项目验证规则")]
    [Description("保养项目验证规则-检验最小值不能大于等于检验最大值")]
    public class MaintainProjectRules : EntityRule<MaintainProject>
    {
        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var model = entity as MaintainProject;
            if (model.MaxValue.HasValue && model.MinValue.HasValue&& model.MinValue > model.MaxValue)
            { e.BrokenDescription = "检测合格最小值{0}不能大于检测合格最大值{1}".L10nFormat(model.MinValue, model.MaxValue); }
            if (model.ConsumableQty < 0)
            {
                e.BrokenDescription = "耗材用量必须大于等于0".L10nFormat(model.MinValue, model.MaxValue);
            }
            if (model.CheckTag == Defects.InspectionItems.CheckTag.Quantitative&& !model.MaxValue.HasValue&& !model.MinValue.HasValue)
            {
                e.BrokenDescription = "检测标记为定量时，最大值和最小值至少一个有值".L10N();
            }
        }
    }
}
