using SIE.Domain.Validation;
using SIE.EMS.InspectionRules;
using System;
using System.ComponentModel;

namespace SIE.EMS.SpecialEquipment.Models
{
    /// <summary>
    /// 被引用的检验规程，不允许删除
    /// </summary>
    [DisplayName("被引用的检验规程，不允许删除")]
    [Description("被引用的检验规程，不允许删除")]
    public class EquipModelRegularInspectionRule : NoReferencedRule<InspectionRule>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EquipModelRegularInspectionRule()
        {
            Properties.Add(EquipModelRegularInspection.InspectionRuleIdProperty);
            MessageBuilder = (o, e) =>
            {
                var ps = o as InspectionRule;
                return "检验规程[{0}]已被设备型号下的设备定检规程引用，不能删除".L10nFormat(ps.Code);
            };
        }
    }
}
