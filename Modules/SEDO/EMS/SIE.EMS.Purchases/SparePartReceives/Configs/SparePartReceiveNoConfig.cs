using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.ComponentModel;

namespace SIE.EMS.Purchases.SparePartReceives.Configs
{
    /// <summary>
    /// 批次序列号编码规则配置
    /// </summary>
    [DisplayName("批次序列号编码规则配置")]
    [Description("用于配置批次序列号编码规则")]
    public class SparePartReceiveNoConfig : ModuleConfig<SparePartReceiveNoConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly SparePartReceiveNoConfigValue defaultValue = new SparePartReceiveNoConfigValue();

        /// <summary>
        /// 默认值
        /// </summary>
        public override SparePartReceiveNoConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }
    }

    /// <summary>
    /// 批次序列号编码规则配置
    /// </summary>
    [RootEntity, Serializable]
    public class SparePartReceiveNoConfigValue : ConfigValue
    {
        #region 批次编码规则 LotNumberRule
        /// <summary>
        /// 批次编码规则Id
        /// </summary>
        [Label("批次编码规则")]
        public static readonly IRefIdProperty LotNumberRuleIdProperty =
            P<SparePartReceiveNoConfigValue>.RegisterRefId(e => e.LotNumberRuleId, ReferenceType.Normal);

        /// <summary>
        /// 批次编码规则Id
        /// </summary>
        public double? LotNumberRuleId
        {
            get { return (double?)this.GetRefNullableId(LotNumberRuleIdProperty); }
            set { this.SetRefNullableId(LotNumberRuleIdProperty, value); }
        }

        /// <summary>
        /// 批次编码规则
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> LotNumberRuleProperty =
            P<SparePartReceiveNoConfigValue>.RegisterRef(e => e.LotNumberRule, LotNumberRuleIdProperty);

        /// <summary>
        /// 批次编码规则
        /// </summary>
        public NumberRule LotNumberRule
        {
            get { return this.GetRefEntity(LotNumberRuleProperty); }
            set { this.SetRefEntity(LotNumberRuleProperty, value); }
        }
        #endregion

        #region 序列号编码规则 SnNumberRule
        /// <summary>
        /// 序列号编码规则Id
        /// </summary>
        [Label("序列号编码规则")]
        public static readonly IRefIdProperty SnNumberRuleIdProperty =
            P<SparePartReceiveNoConfigValue>.RegisterRefId(e => e.SnNumberRuleId, ReferenceType.Normal);

        /// <summary>
        /// 序列号编码规则Id
        /// </summary>
        public double? SnNumberRuleId
        {
            get { return (double?)this.GetRefNullableId(SnNumberRuleIdProperty); }
            set { this.SetRefNullableId(SnNumberRuleIdProperty, value); }
        }

        /// <summary>
        /// 序列号编码规则
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> SnNumberRuleProperty =
            P<SparePartReceiveNoConfigValue>.RegisterRef(e => e.SnNumberRule, SnNumberRuleIdProperty);

        /// <summary>
        /// 序列号编码规则
        /// </summary>
        public NumberRule SnNumberRule
        {
            get { return this.GetRefEntity(SnNumberRuleProperty); }
            set { this.SetRefEntity(SnNumberRuleProperty, value); }
        }
        #endregion
    }
}
