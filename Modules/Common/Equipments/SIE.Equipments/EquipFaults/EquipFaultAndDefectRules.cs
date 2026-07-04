using SIE.Domain.Validation;
using System;
using System.ComponentModel;

namespace SIE.Equipments.EquipFaults
{
    /// <summary>
    /// 设备故障与系统缺陷对应关系非重复验证规则
    /// </summary>
    [DisplayName("设备故障与系统缺陷对应关系非重复验证规则")]
    [Description("设备型号和设备不良代码不能重复")]
    public class EquipFaultAndDefectNotDuplicateRule : NotDuplicateRule<EquipFaultAndDefect>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EquipFaultAndDefectNotDuplicateRule()
        {
            Properties.Add(EquipFaultAndDefect.EquipModelIdProperty);
            Properties.Add(EquipFaultAndDefect.EquipBadCodeProperty);
            MessageBuilder = e =>
            {
                var faultDefect = e as EquipFaultAndDefect;
                return "设备型号[{0}]和设备不良代码[{1}]不能重复".L10nFormat(faultDefect.EquipModel?.Code, faultDefect.EquipBadCode);
            };
        }
    }
}