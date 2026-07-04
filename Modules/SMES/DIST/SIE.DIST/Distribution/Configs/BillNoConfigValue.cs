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
    /// 配送单单号配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("配送单单号生成规则")]
    public class BillNoConfigValue : ConfigValue
    {
        #region 条码规则 NumberRule
        /// <summary>
        /// 条码规则ID
        /// </summary>
        [Label("生成规则")]
        public static readonly IRefIdProperty NumberRuleIdProperty =
              P<BillNoConfigValue>.RegisterRefId(e => e.NumberRuleId, ReferenceType.Normal);

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
            P<BillNoConfigValue>.RegisterRef(e => e.NumberRule, NumberRuleIdProperty);

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
    class BillNoConfigValueConfig : EntityConfig<BillNoConfigValue>
    {
        /// <summary>
        /// 添加验证规则
        /// </summary>
        /// <param name="rules">验证规则声明器</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(BillNoConfigValue.NumberRuleIdProperty, new RequiredRule());
        }
    }
}