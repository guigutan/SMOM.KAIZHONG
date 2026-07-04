using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.ComponentModel;

namespace SIE.Resources.CalendarSchemes
{
    /// <summary>
    /// 日历方案
    /// </summary>
    [DisplayName("日历方案验证规则")]
    [Description("日历方案验证规则,验证缺省状态下不允许停用")]
    public class CalendarSchemeEnableRule : EntityRule<CalendarScheme>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CalendarSchemeEnableRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 子类重此方法来添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var calendarScheme = entity as CalendarScheme;
            if (calendarScheme.IsDefault == YesNo.Yes && calendarScheme.IsEnable == YesNo.No)
            {
                e.BrokenDescription = "日历方案[{0}]缺省的情况下，不允许停用".L10nFormat(calendarScheme.Name);
            }
        }
    }

    /// <summary>
    /// 日历方案
    /// </summary>
    [DisplayName("日历方案验证规则")]
    [Description("日历方案验证规则,验证缺省状态下不允许停用")]
    public class CalendarSchemeStateRule : EntityRule<CalendarScheme>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CalendarSchemeStateRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 子类重此方法来添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var calendarScheme = entity as CalendarScheme;
            if (calendarScheme.IsDefault == YesNo.Yes && RT.Service.Resolve<CalendarSchemeController>().ExistsDefault(calendarScheme.Id))
            {
                e.BrokenDescription = "不允许多个日历方案同时是缺省的".L10N();
            }
        }
    }
}