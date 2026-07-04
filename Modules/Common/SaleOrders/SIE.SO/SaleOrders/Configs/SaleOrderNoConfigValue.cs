using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.SO.SaleOrders.Configs
{
    /// <summary>
    /// 销售订单配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("销售订单编码生成规则")]
    public class SaleOrderNoConfigValue : ConfigValue
    {
        #region 编码规则 NumberRule
        /// <summary>
        /// 编码规则Id
        /// </summary>
        [Label("编码规则")]
        public static readonly IRefIdProperty NumberRuleIdProperty =
            P<SaleOrderNoConfigValue>.RegisterRefId(e => e.NumberRuleId, ReferenceType.Normal);

        /// <summary>
        /// 编码规则Id
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
            P<SaleOrderNoConfigValue>.RegisterRef(e => e.NumberRule, NumberRuleIdProperty);

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
        /// 显示名称
        /// </summary>
        /// <returns>编码规则名称</returns>
        public override string Display()
        {
            return NumberRule?.Name;
        }
    }
}
