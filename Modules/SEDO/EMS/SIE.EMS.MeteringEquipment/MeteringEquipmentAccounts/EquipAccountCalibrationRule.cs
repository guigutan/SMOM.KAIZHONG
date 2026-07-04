using SIE.Domain.Validation;
using SIE.EMS.InspectionRules;
using System;
using System.ComponentModel;

namespace SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts
{
    /// <summary>
    /// 被引用的检验规程，不允许删除
    /// </summary>
    [DisplayName("被引用的检验规程，不允许删除")]
    [Description("被引用的检验规程，不允许删除")]
    public class EquipAccountCalibrationRule : NoReferencedRule<InspectionRule>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EquipAccountCalibrationRule()
        {
            Properties.Add(EquipAccountCalibration.InspectionRuleIdProperty);
            MessageBuilder = (o, e) =>
            {
                var ps = o as InspectionRule;
                return "检验规程[{0}]已被计量设备台账下的计量校验规程引用，不能删除".L10nFormat(ps.Code);
            };
        }
    }
 
}
