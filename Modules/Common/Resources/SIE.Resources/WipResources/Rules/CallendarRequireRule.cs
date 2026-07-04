using SIE.Domain.Validation;
using SIE.ManagedProperty;
using System.ComponentModel;

namespace SIE.Resources.WipResources
{
    /// <summary>
    /// 生产资源日历方案不能为空验证规则
    /// </summary>
    [DisplayName("生产资源日历方案验证规则")]
    [Description("日历方案不能为空")]
    public class WipResourceCalendarSchemeRequireRule : RequireRule<WipResource>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WipResourceCalendarSchemeRequireRule()
        {
            MessageBuilder = (e) => { return "日历方案不能为空"; };
        }

        /// <summary>
        /// 验证属性
        /// </summary>
        protected override IManagedProperty Property
        {
            get
            {
                return WipResource.SchemeIdProperty;
            }
        }
    }

}
