using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MetaModel;
using System;
using System.ComponentModel;

namespace SIE.Resources.Skills
{
    /// <summary>
    /// 技能清单验证规则
    /// </summary>
    [DisplayName("技能清单验证规则")]
    [Description("技能清单有效期必须大于0")]
    public class SkillValidityRule : PropertyRule<Skill>
    {
        /// <summary>
        /// 验证属性
        /// </summary>
        protected override IManagedProperty Property
        {
            get
            {
                return Skill.ValidityProperty;
            }
        }

        /// <summary>
        /// 技能清单有效期验证规则
        /// </summary>
        /// <param name="entity">技能</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var skill = entity as Skill;
            if (skill != null && skill.Validity < 0)
                e.BrokenDescription = "有效期不能小于0".L10N();
        }
    }

    /// <summary>
    /// 技能分类删除验证规则
    /// </summary>
    [DisplayName("技能分类删除验证规则")]
    [Description("技能分类被技能引用不能删除")]
    public class SkillCategoryNoRefSkill : NoReferencedRule<SkillCategory>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SkillCategoryNoRefSkill()
        {
            Properties.Add(Skill.CategoryIdProperty);
            MessageBuilder = (e, i) =>
            {
                return "不能删除，技能分类[{0}]被技能引用[{1}]次".L10nFormat((e as SkillCategory).Name, i);
            };
        }
    }
}
