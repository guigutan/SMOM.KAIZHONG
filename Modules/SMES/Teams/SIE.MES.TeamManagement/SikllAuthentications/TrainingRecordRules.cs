using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.Resources.Employees;
using System;
using System.ComponentModel;

namespace SIE.MES.TeamManagement.SikllAuthentications
{
    /// <summary>
    /// 培训记录开始时间验证规则
    /// </summary> 
    [DisplayName("培训记录开始时间验证规则")]
    [Description("开始时间不能为空，且必须小于当前时间")]
    public class TrainBeginDateRule : PropertyRule<TrainingRecord>
    {
        /// <summary>
        /// 验证属性
        /// </summary>
        protected override IManagedProperty Property => TrainingRecord.BeginDateProperty;

        /// <summary>
        /// 验证方法
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var record = entity as TrainingRecord;
            if (record == null)
                return;
            if (record.BeginDate.HasValue && record.BeginDate > DateTime.Now)
                e.BrokenDescription = "培训开始时间不能大于当前时间".L10N();
        }
    }

    /// <summary>
    /// 培训记录开始时间验证规则
    /// </summary> 
    [DisplayName("培训记录结束时间验证规则")]
    [Description("结束时间不能为空，且必须小于当前时间")]
    public class TraiEndDateRule : PropertyRule<TrainingRecord>
    {
        /// <summary>
        /// 验证属性
        /// </summary>
        protected override IManagedProperty Property => TrainingRecord.EndDateProperty;

        /// <summary>
        /// 验证方法
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var record = entity as TrainingRecord;
            if (record == null)
                return;
            if (record.EndDate.HasValue && record.EndDate > DateTime.Now)
                e.BrokenDescription = "培训结束时间不能大于当前时间".L10N();
        }
    }

    /// <summary>
    /// 培训记录新增修改 校验
    /// </summary>
    [DisplayName("培训记录新增修改时检查必填项")]
    [Description("培训记录新增修改时检查必填项")]
    public class TrainingRecordRules : EntityRule<TrainingRecord>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TrainingRecordRules()
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
            var trainingRecord = entity as TrainingRecord;
            if (trainingRecord.EndDate <= trainingRecord.BeginDate)
            {
                e.BrokenDescription = "培训结束时间必须大于培训开始时间".L10N();
            }

            if (trainingRecord.Duration < 0)
            {
                e.BrokenDescription = "请输入时长（非负数）".L10N();
            }

            var ctl = RT.Service.Resolve<SkillAuthController>();
            if (ctl.IsExsitTraningRecord(trainingRecord))
                e.BrokenDescription = "培训时间重叠".L10N();
        }
    }

    /// <summary>
    /// 员工删除验证规则
    /// </summary>
    [DisplayName("员工删除验证规则")]
    [Description("员工被培训记录引用不能删除")]
    public class EmployeeNoRefRecordRule : NoReferencedRule<Employee>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EmployeeNoRefRecordRule()
        {
            Properties.Add(TrainingRecord.EmployeeIdProperty);
            MessageBuilder = (e, i) =>
            {
                return "不能删除，员工[{0}]被培训记录引用[{1}]次".L10nFormat((e as Employee).Name, i);
            };
        }
    }

    /// <summary>
    /// 技能认证删除验证规则
    /// </summary>
    [DisplayName("技能认证删除验证规则")]
    [Description("技能认证被培训记录引用不能删除")]
    public class SkillAuthNoRefRecordRule : NoReferencedRule<SkillAuthentication>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SkillAuthNoRefRecordRule()
        {
            Properties.Add(TrainingRecord.SkillAuthIdProperty);
            MessageBuilder = (e, i) =>
            {
                return "不能删除，技能认证[{0}]被培训记录引用[{1}]次".L10nFormat((e as SkillAuthentication)?.Skill?.Name, i);
            };
        }
    }
}