using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SIE.AbnormalInfo.AbnormalMonitors.AbnormalWarnDefines
{

    /// <summary>
    /// 异常预警定义推送方式验证规则
    /// </summary>
    [DisplayName("异常预警定义推送方式验证规则")]
    [Description("异常预警定义推送方式不能为空")]
    public class AbnormalWarnDefinePushModuleRules : EntityRule<AbnormalWarnDefine>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AbnormalWarnDefinePushModuleRules()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 验证方法
        /// </summary>
        /// <param name="entity">数据源配置</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var warnDefine = entity as AbnormalWarnDefine;
            if (!warnDefine.PushModuleId.HasValue)
                e.BrokenDescription = "推送模块不能为空".L10N();
        }
    }


    /// <summary>
    /// 异常预警定义推送升级机制重复验证规则
    /// </summary>
    [DisplayName("异常预警定义推送升级机制重复验证规则")]
    [Description("异常预警定义推送升级机制重复验证规则")]
    public class PushUpgradeRuleNotDuplicateRule : EntityRule<AbnormalWarnDefine>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PushUpgradeRuleNotDuplicateRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 验证方法
        /// </summary>
        /// <param name="entity">数据源配置</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var warnDefine = entity as AbnormalWarnDefine;
            if (RT.Service.Resolve<AbnormalWarnDefineService>().PushUpgradeRuleDuplicate(warnDefine))
                e.BrokenDescription = "推送升级机制不能重复".L10N();
        }
    }

    /// <summary>
    /// 异常预警定义推送升级机制的推送对象重复验证规则
    /// </summary>
    [DisplayName("异常预警定义推送升级机制的推送对象重复验证规则")]
    [Description("异常预警定义推送升级机制的推送对象重复验证规则")]
    public class PushTargetNotDuplicateRule : EntityRule<PushUpgradeRule>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PushTargetNotDuplicateRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 验证方法
        /// </summary>
        /// <param name="entity">数据源配置</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var upRule = entity as PushUpgradeRule;
            if (RT.Service.Resolve<AbnormalWarnDefineService>().PushUpgradeRuleDuplicate(upRule))
                e.BrokenDescription = "推送升级机制中推送对象不能重复".L10N();
        }
    }

}
