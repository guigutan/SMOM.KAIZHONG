using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.ComponentModel;

namespace SIE.EMS.Faults
{
    /// <summary>
    /// 故障中类存在引用不能删除
    /// </summary>
    [DisplayName("NoReferencedRule验证规则")]
    [Description("故障中类存在引用不能删除")]
    public class EquipMiddleFaultReferencedRule : NoReferencedRule<EquipMiddleFault>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EquipMiddleFaultReferencedRule()
        {
            Properties.Add(EquipSmallFault.MiddleFaultIdProperty);
            MessageBuilder = (o, e) =>
            {
                var equipMiddleFault = o as EquipMiddleFault;
                return "设备故障中类[{0}]已经被[{1}]引用，不能删除".L10nFormat(equipMiddleFault.Code, "故障小类".L10N());
            };
        }
    }

    /// <summary>
    /// 故障大类存在引用不能删除
    /// </summary>
    [DisplayName("NoReferencedRule验证规则")]
    [Description("故障大类存在引用不能删除")]
    public class EquipLargeFaultReferencedRule : NoReferencedRule<EquipLargeFault>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EquipLargeFaultReferencedRule()
        {
            Properties.Add(EquipMiddleFault.LargeFaultIdProperty);
            MessageBuilder = (o, e) =>
            {
                var equipLargeFault = o as EquipLargeFault;
                return "设备故障大类[{0}]已经被[{1}]引用，不能删除".L10nFormat(equipLargeFault.Code, "故障中类".L10N());
            };
        }
    }

    #region 故障中类名称非重复验证规则
    /// <summary>
    /// 故障中类名称非重复验证规则
    /// </summary>
    [DisplayName("故障中类名称非重复验证规则")]
    [Description("故障中类名称不能重复")]
    class EquipMiddleFaultNameNotDuplicateRule : NotDuplicateRule<EquipMiddleFault>
    {
        /// <summary>
        /// 不重复规则
        /// </summary>
        public EquipMiddleFaultNameNotDuplicateRule()
        {
            Properties.Add(EquipMiddleFault.LargeFaultIdProperty);
            Properties.Add(EquipMiddleFault.NameProperty);
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            MessageBuilder = e =>
            {
                var entity = e as EquipMiddleFault;
                return string.Format("故障中类名称[{0}]在故障大类[{1}]已维护，不允许重复维护！".L10N(), entity.Name, entity.LargeFault.Name);
            };
        }
    }
    #endregion

    #region 故障小类名称非重复验证规则
    /// <summary>
    /// 故障小类名称非重复验证规则
    /// </summary>
    [DisplayName("故障小类名称非重复验证规则")]
    [Description("故障小类名称不能重复")]
    class EquipSmallFaultNameNotDuplicateRule : NotDuplicateRule<EquipSmallFault>
    {
        /// <summary>
        /// 不重复规则
        /// </summary>
        public EquipSmallFaultNameNotDuplicateRule()
        {
            Properties.Add(EquipSmallFault.MiddleFaultIdProperty);
            Properties.Add(EquipSmallFault.NameProperty);
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            MessageBuilder = e =>
            {
                var entity = e as EquipSmallFault;
                return string.Format("故障小类名称[{0}]在故障中类[{1}]已维护，不允许重复维护！".L10N(), entity.Name, entity.MiddleFault.Name);
            };
        }
    }
    #endregion
}
