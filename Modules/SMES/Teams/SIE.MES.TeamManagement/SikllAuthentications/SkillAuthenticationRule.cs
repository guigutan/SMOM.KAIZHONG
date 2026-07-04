using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.Resources.Skills;
using System;
using System.ComponentModel;

namespace SIE.MES.TeamManagement.SikllAuthentications
{
    /// <summary>
    /// 技能删除验证规则
    /// </summary>
    [DisplayName("技能删除验证规则")]
    [Description("技能被技能认证引用不能删除")]
    public class SkillNoRefAuthRule : NoReferencedRule<Skill>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SkillNoRefAuthRule()
        {
            Properties.Add(SkillAuthentication.SkillIdProperty);
            MessageBuilder = (e, i) =>
            {
                return "不能删除，技能[{0}]被技能认证引用[{1}]次".L10nFormat((e as Skill)?.Name, i);
            };
        }
    }

    /// <summary>
    /// 技能分类删除验证规则
    /// </summary>
    [DisplayName("技能分类删除验证规则")]
    [Description("技能分类被技能认证引用不能删除")]
    public class SkillCategoryNoRefAuthRule : NoReferencedRule<SkillCategory>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SkillCategoryNoRefAuthRule()
        {
            Properties.Add(SkillAuthentication.SkillCategoryIdProperty);
            MessageBuilder = (e, i) =>
            {
                return "不能删除，技能分类[{0}]被技能认证引用[{1}]次".L10nFormat((e as SkillCategory)?.Name, i);
            };
        }
    }

    /// <summary>
    /// 技能重复验证规则
    /// </summary>
    [DisplayName("技能重复验证规则")]
    [Description("技能认证管理不能存在相同的技能")]
    public class SkillNotDuplicateRule : NotDuplicateRule<SkillAuthentication>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SkillNotDuplicateRule()
        {
            Properties.Add(SkillAuthentication.SkillIdProperty);
            MessageBuilder = (e) =>
            {
                return "已经存在技能[{0}]的员工技能认证管理".L10nFormat((e as SkillAuthentication).Skill?.Name);
            };
        }
    }

    /// <summary>
    /// 技能认证实体规则
    /// </summary>
    [DisplayName("技能认证实体规则")]
    [Description("技能要求不能都为无")]
    public class SkillAuthRule : EntityRule<SkillAuthentication>
    {
        /// <summary>
        /// 验证技能认证
        /// </summary>
        /// <param name="entity">技能认证</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var auth = entity as SkillAuthentication;
            if (auth.ExamRequired == ExamRequired.NoMatter && auth.OperationRequired == OperationRequired.NoMatter && auth.TrainingRequired == TrainingRequired.NoMatter)
                e.BrokenDescription = "技能认证要求不能都为无".L10N();
        }
    }
}