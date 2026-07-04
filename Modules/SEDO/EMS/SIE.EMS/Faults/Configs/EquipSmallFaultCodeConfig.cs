using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Faults.Configs
{
    /// <summary>
    /// 设备故障小类编码
    /// </summary>
    [System.ComponentModel.DisplayName("设备故障小类编码生成规则")]
    [System.ComponentModel.Description("用于设备故障小类编码生成的具体规则,具体规则详细请在配置项中进行配置")]
    public class EquipSmallFaultCodeConfig : ModuleConfig<EquipSmallFaultCodeConfigValue>
    {
        /// <summary> 
        /// 默认值
        /// </summary>
        readonly EquipSmallFaultCodeConfigValue defaultValue = new EquipSmallFaultCodeConfigValue { CodeRule = null };

        /// <summary>
        /// 默认值
        /// </summary>
        public override EquipSmallFaultCodeConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }
    }

    /// <summary>
    /// 设备故障小类编码配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("设备故障小类编码")]
    public class EquipSmallFaultCodeConfigValue : ConfigValue
    {
        #region 编码规则 CodeRule
        /// <summary>
        /// 编码规则ID
        /// </summary>
        [Label("编码规则")]
        public static readonly IRefIdProperty NumberRuleIdProperty =
              P<EquipSmallFaultCodeConfigValue>.RegisterRefId(e => e.NumberRuleId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<NumberRule> CodeRuleProperty =
            P<EquipSmallFaultCodeConfigValue>.RegisterRef(e => e.CodeRule, NumberRuleIdProperty);

        /// <summary>
        /// 编码规则
        /// </summary>
        public NumberRule CodeRule
        {
            get { return this.GetRefEntity(CodeRuleProperty); }
            set { this.SetRefEntity(CodeRuleProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns>编码规则名称</returns>
        public override string Display()
        {
            if (CodeRule == null)
                return "NIL";
            return CodeRule.Name;
        }
    }
}
