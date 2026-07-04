using SIE.Domain.Validation;
using SIE.EMS.SpecialEquipment.RegularInspections;
using System;
using System.ComponentModel;

namespace SIE.EMS.SpecialEquipment.SpecialEquipmentAcounts
{

    /// <summary>
    /// 被引用的特种设备台账，不允许删除
    /// </summary>
    [DisplayName("被引用的特种设备台账，不允许删除")]
    [Description("被引用的特种设备台账，不允许删除")]
    public class SpecialEquipmentAccountRule : NoReferencedRule<SpecialEquipmentAccount>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SpecialEquipmentAccountRule()
        {
            Properties.Add(RegularInspection.SpecialEquipmentAccountIdProperty);
            MessageBuilder = (o, e) =>
            {
                var ps = o as SpecialEquipmentAccount;
                return "特种设备台账[{0}]被特种设备定检引用，不能删除".L10nFormat(ps.Code);
            };
        }
    }
}
