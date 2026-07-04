using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.Resources.Employees;
using System;
using System.ComponentModel;

namespace SIE.MES.TeamManagement.SikllAuthentications
{
    /// <summary>
    /// 实操记录新增修改 校验
    /// </summary>
    [DisplayName("实操记录新增修改时检查必填项")]
    [Description("实操记录新增修改时检查必填项")]
    public class OperationRecordRules : EntityRule<OperationRecord>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public OperationRecordRules()
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
            var operationRecord = entity as OperationRecord;
            if (operationRecord.AuditTime > DateTime.Now)
                e.BrokenDescription = "考核时间不能晚于当前时间".L10N();
            var ctl = RT.Service.Resolve<SkillAuthController>();
            if (ctl.IsExsitOperationRecord(operationRecord))
                e.BrokenDescription = "实操考核时间重复".L10N();
        }
    }

    /// <summary>
    /// 员工删除验证规则
    /// </summary>
    [DisplayName("员工删除验证规则")]
    [Description("员工被实操记录引用不能删除")]
    public class EmployeeNoRefOperationRule : NoReferencedRule<Employee>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EmployeeNoRefOperationRule()
        {
            Properties.Add(OperationRecord.EmployeeIdProperty);
            MessageBuilder = (e, i) =>
            {
                return "不能删除，员工[{0}]被实操记录引用[{1}]次".L10nFormat((e as Employee).Name, i);
            };
        }
    }

    /// <summary>
    /// 技能认证删除验证规则
    /// </summary>
    [DisplayName("技能认证删除验证规则")]
    [Description("技能认证被实操记录引用不能删除")]
    public class SkillAuthNoRefOperationRule : NoReferencedRule<SkillAuthentication>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SkillAuthNoRefOperationRule()
        {
            Properties.Add(OperationRecord.SkillAuthIdProperty);
            MessageBuilder = (e, i) =>
            {
                return "不能删除，技能认证[{0}]被实操记录引用[{1}]次".L10nFormat((e as SkillAuthentication)?.Skill?.Name, i);
            };
        }
    }
}