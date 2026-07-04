using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Warehouses.Configs
{
    /// <summary>
    /// 事务原因No配置项内容
    /// </summary>
    [RootEntity, Serializable]
    [DisplayMember(nameof(ReasonNoRule))]
    [Label("事务原因编码生成规则")]
    public class ReasonNoConfigValue : ConfigValue
    {
        #region 编码规则 ReasonNoRule
        /// <summary>
        /// 编码规则ID
        /// </summary>
        [Label("编码规则")]
        public static readonly IRefIdProperty ReasonNoRuleIdProperty =
              P<ReasonNoConfigValue>.RegisterRefId(e => e.ReasonNoRuleId, ReferenceType.Normal);

        /// <summary>
        /// 编码规则ID
        /// </summary>
        public double? ReasonNoRuleId
        {
            get { return (double?)this.GetRefNullableId(ReasonNoRuleIdProperty); }
            set { this.SetRefNullableId(ReasonNoRuleIdProperty, value); }
        }

        /// <summary>
        /// 编码规则
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> ReasonNoRuleProperty =
            P<ReasonNoConfigValue>.RegisterRef(e => e.ReasonNoRule, ReasonNoRuleIdProperty);

        /// <summary>
        /// 编码规则
        /// </summary>
        public NumberRule ReasonNoRule
        {
            get { return this.GetRefEntity(ReasonNoRuleProperty); }
            set { this.SetRefEntity(ReasonNoRuleProperty, value); }
        }
        #endregion

        /// <summary>
        ///  显示值
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            if (ReasonNoRule == null)
                return "NIL";
            return ReasonNoRule.Name;
        }
    }
}
