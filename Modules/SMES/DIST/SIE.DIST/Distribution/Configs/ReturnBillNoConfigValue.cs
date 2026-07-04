using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.DIST.Distribution.Configs
{
    /// <summary>
    /// 配送退料单号配置项
    /// </summary>
    [RootEntity, Serializable]
    [Label("销售退货单单号生成规则")]
    public class ReturnBillNoConfigValue : ConfigValue
    {
        #region  条码规则 NumberRule
        /// <summary>
        /// 条码规则ID
        /// </summary>
        [Label("生成规则")]
        public static readonly IRefIdProperty NumberRuleIdProperty =
              P<ReturnBillNoConfigValue>.RegisterRefId(e => e.NumberRuleId, ReferenceType.Normal);

        /// <summary>
        /// 条码规则
        /// </summary>
        public double? NumberRuleId
        {
            get { return (double?)this.GetRefNullableId(NumberRuleIdProperty); }
            set { this.SetRefNullableId(NumberRuleIdProperty, value); }
        }

        /// <summary>
        /// 条码规则
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> NumberRuleProperty =
            P<ReturnBillNoConfigValue>.RegisterRef(e => e.NumberRule, NumberRuleIdProperty);

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
    class ReturnBillNoConfigValueConfig : EntityConfig<ReturnBillNoConfigValue>
    {
        /// <summary>
        /// 验证规则配置
        /// </summary>
        /// <param name="rules">验证规则声明器</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(ReturnBillNoConfigValue.NumberRuleIdProperty, new RequiredRule());
        }
    }
}
