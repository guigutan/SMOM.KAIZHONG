using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Checks.Configs
{
    /// <summary>
    /// 点检单号
    /// </summary>
    [System.ComponentModel.DisplayName("点检单号生成规则")]
    [System.ComponentModel.Description("用于点检单号生成的具体规则,具体规则详细请在配置项中进行配置")]
    public class CheckPlanNoConfig : ModuleConfig<CheckPlanNoConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly CheckPlanNoConfigValue defaultValue = new CheckPlanNoConfigValue { NumberRule = null };

        /// <summary>
        /// 默认值
        /// </summary>
        public override CheckPlanNoConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }
    }

    /// <summary>
    /// 点检单号配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("点检单号")]
    public class CheckPlanNoConfigValue : ConfigValue
    {
        #region 编码规则 NumberRule
        /// <summary>
        /// 编码规则ID
        /// </summary>
        [Label("编码规则")]
        public static readonly IRefIdProperty NumberRuleIdProperty =
              P<CheckPlanNoConfigValue>.RegisterRefId(e => e.NumberRuleId, ReferenceType.Normal);

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
            P<CheckPlanNoConfigValue>.RegisterRef(e => e.NumberRule, NumberRuleIdProperty);

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
