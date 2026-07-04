using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Equipments.Configs
{
    /// <summary>
    /// 设备编码生成规则
    /// </summary>
    [System.ComponentModel.DisplayName("设备台账设备编码生成规则")]
    [System.ComponentModel.Description("用于设备编码生成的具体规则,具体规则详细请在配置项中进行配置")]
    public class EquipCodeConfig : ModuleConfig<EquipCodeConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly EquipCodeConfigValue defaultValue = new EquipCodeConfigValue { NumberRule = null };

        /// <summary>
        /// 默认值
        /// </summary>
        public override EquipCodeConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }
    }

    /// <summary>
    /// 设备编码配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("设备编码")]
    public class EquipCodeConfigValue : ConfigValue
    {
        #region 编码规则 NumberRule
        /// <summary>
        /// 编码规则ID
        /// </summary>
        [Label("编码规则")]
        public static readonly IRefIdProperty NumberRuleIdProperty =
              P<EquipCodeConfigValue>.RegisterRefId(e => e.NumberRuleId, ReferenceType.Normal);

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
            P<EquipCodeConfigValue>.RegisterRef(e => e.NumberRule, NumberRuleIdProperty);

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
            {
                return "NIL";
            }
            return NumberRule.Name;
        }
    }
}
