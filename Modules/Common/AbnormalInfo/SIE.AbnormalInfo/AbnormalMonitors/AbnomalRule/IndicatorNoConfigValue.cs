using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.AbnormalInfo.AbnormalMonitors
{
    /// <summary>
    /// 指标编码
    /// </summary>
    [RootEntity, Serializable]
    [Label("指标编码生成规则")]
    public class IndicatorNoConfigValue : ConfigValue
    {
        #region 条码规则 NumberRule
        /// <summary>
        /// 条码规则ID
        /// </summary>
        [Label("生成规则")]
        public static readonly IRefIdProperty NumberRuleIdProperty =
              P<IndicatorNoConfigValue>.RegisterRefId(e => e.NumberRuleId, ReferenceType.Normal);

        /// <summary>
        /// 条码规则ID
        /// </summary>
        public double NumberRuleId
        {
            get { return (double)this.GetRefId(NumberRuleIdProperty); }
            set { this.SetRefId(NumberRuleIdProperty, value); }
        }

        /// <summary>
        /// 条码规则
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> NumberRuleProperty =
            P<IndicatorNoConfigValue>.RegisterRef(e => e.NumberRule, NumberRuleIdProperty);

        /// <summary>
        /// 条码规则
        /// </summary>
        public NumberRule NumberRule
        {
            get { return this.GetRefEntity(NumberRuleProperty); }
            set { this.SetRefEntity(NumberRuleProperty, value); }
        }
        #endregion

        /// <summary>
        /// 把配置值显示出来
        /// </summary>
        /// <returns>配置值</returns>
        public override string Display()
        {
            if (NumberRule == null)
            {
                return "NIL";
            }
            else
            {
                return NumberRule.Name;
            }
        }
    }

    /// <summary>
    /// 实体配置
    /// </summary>
    class IndicatorNoConfigValueConfig : EntityConfig<IndicatorNoConfigValue>
    {
        /// <summary>
        /// 添加验证规则
        /// </summary>
        /// <param name="rules">验证规则声明器</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(IndicatorNoConfigValue.NumberRuleIdProperty, new RequiredRule());
        }
    }
}