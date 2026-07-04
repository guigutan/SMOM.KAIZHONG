using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.EquipRepair.EquipRepairs.Configs
{
    /// <summary>
    /// 设备维修单号
    /// </summary>
    [System.ComponentModel.DisplayName("设备维修单号生成规则")]
    [System.ComponentModel.Description("用于设备维修单号生成的具体规则,具体规则详细请在配置项中进行配置")]
    public class EquipRepairNoConfig : ModuleConfig<EquipRepairNoConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly EquipRepairNoConfigValue defaultValue = new EquipRepairNoConfigValue { NumberRule = null };

        /// <summary>
        /// 默认值
        /// </summary>
        public override EquipRepairNoConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }
    }

    /// <summary>
    /// 设备维修单号配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("设备维修单号")]
    public class EquipRepairNoConfigValue : ConfigValue
    {
        #region 编码规则 NumberRule
        /// <summary>
        /// 编码规则ID
        /// </summary>
        [Label("编码规则")]
        public static readonly IRefIdProperty NumberRuleIdProperty =
              P<EquipRepairNoConfigValue>.RegisterRefId(e => e.NumberRuleId, ReferenceType.Normal);

        /// <summary>
        /// 编码规则ID
        /// </summary>
        public double? NumberRuleId
        {
            get { return (double?)this.GetRefNullableId(NumberRuleIdProperty); }
            set { this.SetRefNullableId(NumberRuleIdProperty, value); }
        }

        /// <summary>
        /// 编码规则
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> NumberRuleProperty =
            P<EquipRepairNoConfigValue>.RegisterRef(e => e.NumberRule, NumberRuleIdProperty);

        /// <summary>
        /// 编码规则
        /// </summary>
        public NumberRule NumberRule
        {
            get { return this.GetRefEntity(NumberRuleProperty); }
            set { this.SetRefEntity(NumberRuleProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns>编码规则名称</returns>
        public override string Display()
        {
            if (NumberRule == null)
                return "NIL";
            return NumberRule.Name;
        }
    }
}
