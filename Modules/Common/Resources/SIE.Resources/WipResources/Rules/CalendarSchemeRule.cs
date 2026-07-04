using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.Resources.CalendarSchemes;
using System;
using System.ComponentModel;

namespace SIE.Resources.WipResources.Rules
{
    /// <summary>
    /// 禁用日历方案时引用规则
    /// </summary>
    [DisplayName("日历方案被生产资源引用")]
    [Description("日历方案被生产资源引用不允许删除")]
    class CalendarSchemeRule : NoReferencedRule<CalendarScheme>
    {
        /// <summary>
        /// 日历方案关联计划资源
        /// </summary>
        public CalendarSchemeRule()
        {
            Properties.Add(WipResource.SchemeIdProperty);
            Scope = EntityStatusScopes.Delete;
            MessageBuilder = (o, e) =>
            {
                var entity = o as CalendarScheme;
                return "日历方案[{0}]被生产资源引用,不能删除.".L10nFormat(entity.Name);
            };
        }
    }
}
