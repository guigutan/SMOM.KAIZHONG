using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.Resources.Employees;
using System;
using System.ComponentModel;

namespace SIE.MES.TeamManagement.SikllAuthentications
{
    /// <summary>
    /// 考试结果新增修改 校验
    /// </summary>
    [DisplayName("考试结果新增修改时检查必填项")]
    [Description("考试结果新增修改时检查必填项")]
    public class ExamResultRules : EntityRule<ExamResult>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ExamResultRules()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 重写实体规则验证方法
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var examResult = entity as ExamResult;
            if (examResult.Score < 0)
                e.BrokenDescription = "考试得分不能为负数".L10N();
            else if (examResult.ExamTime > DateTime.Now)
                e.BrokenDescription = "考试时间大于当前时间".L10N();
            var ctl = RT.Service.Resolve<SkillAuthController>();
            if (ctl.IsExsitExamResult(examResult))
                e.BrokenDescription = "考试时间重复".L10N();
        }
    }

    /// <summary>
    /// 员工删除验证规则
    /// </summary>
    [DisplayName("员工删除验证规则")]
    [Description("员工被考试结果引用不能删除")]
    public class EmployeeNoRefExamRule : NoReferencedRule<Employee>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EmployeeNoRefExamRule()
        {
            Properties.Add(ExamResult.EmployeeIdProperty);
            MessageBuilder = (e, i) =>
            {
                return "不能删除，员工[{0}]被考试结果引用[{1}]次".L10nFormat((e as Employee).Name, i);
            };
        }
    }

    /// <summary>
    /// 技能认证删除验证规则
    /// </summary>
    [DisplayName("技能认证删除验证规则")]
    [Description("技能认证被考试结果引用不能删除")]
    public class SkillAuthNoRefExamRule : NoReferencedRule<SkillAuthentication>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SkillAuthNoRefExamRule()
        {
            Properties.Add(ExamResult.SkillAuthIdProperty);
            MessageBuilder = (e, i) =>
            {
                return "不能删除，技能认证[{0}]被考试结果引用[{1}]次".L10nFormat((e as SkillAuthentication)?.Skill?.Name, i);
            };
        }
    }
}