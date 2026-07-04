using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.ComponentModel;

namespace SIE.Resources.CalendarSchemes
{
    /// <summary>
    /// 名称验证
    /// </summary>
    [DisplayName("周方案验证规则")]
    [Description("周方案验证规则,验证名称不重复")]
    class CalendarWeekNameNotDuplicateRule : NotDuplicateRule<CalendarSchemeWeek>
    {
        /// <summary>
        /// 不重复规则
        /// </summary>
        public CalendarWeekNameNotDuplicateRule()
        {
            Properties.Add(CalendarSchemeWeek.SchemeIdProperty);
            Properties.Add(CalendarSchemeWeek.NameProperty);
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            this.MessageBuilder = e =>
            {
                var r = e as CalendarSchemeWeek;
                return "已经存在[名称]是{0}的周方案".L10nFormat(r.Name);
            };
        }
    }

    /// <summary>
    /// 启用日期验证
    /// </summary>
    [DisplayName("周方案验证规则")]
    [Description("周方案验证规则,验证启用日期不重复")]
    class CalendarWeekNotDuplicateRule : NotDuplicateRule<CalendarSchemeWeek>
    {
        /// <summary>
        /// 不重复规则
        /// </summary>
        public CalendarWeekNotDuplicateRule()
        {
            Properties.Add(CalendarSchemeWeek.SchemeIdProperty);
            Properties.Add(CalendarSchemeWeek.ActiveDateProperty);
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            this.MessageBuilder = e =>
            {
                var r = e as CalendarSchemeWeek;
                return "已经存在[预设启用日期]是{0}的周方案".L10nFormat(r.ActiveDate.ToShortDateString());
            };
        }
    }

    /// <summary>
    /// 启用日期验证
    /// </summary>
    [DisplayName("周方案验证规则")]
    [Description("周方案验证规则,验证启用日期大于今天")]
    class CalendarWeekValidationRule : EntityRule<CalendarSchemeWeek>
    {
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var calendarWeek = entity as CalendarSchemeWeek;
            if (calendarWeek.ActiveDate.Date < DateTime.Now.Date)
            {
                throw new ValidationException("周方案启用日期必须大于等于今天.".L10N());
            }
        }
    }
}
