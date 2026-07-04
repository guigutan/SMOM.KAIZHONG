using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.ComponentModel;

namespace SIE.EMS.Purchases.FixtureReceives.Configs
{
    /// <summary>
    /// 序列号编码规则配置
    /// </summary>
    [DisplayName("序列号编码规则配置")]
    [Description("用于配置序列号编码规则")]
    public class ReceiveSnNoConfig : ModuleConfig<ReceiveSnNoConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly ReceiveSnNoConfigValue defaultValue = new ReceiveSnNoConfigValue();

        /// <summary>
        /// 默认值
        /// </summary>
        public override ReceiveSnNoConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }
    }

    /// <summary>
    /// 序列号编码规则配置
    /// </summary>
    [RootEntity, Serializable]
    public class ReceiveSnNoConfigValue : ConfigValue
    {
        #region 编码规则 NumberRule
        /// <summary>
        /// 编码规则Id
        /// </summary>
        [Label("编码规则")]
        public static readonly IRefIdProperty NumberRuleIdProperty =
            P<ReceiveSnNoConfigValue>.RegisterRefId(e => e.NumberRuleId, ReferenceType.Normal);

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
            P<ReceiveSnNoConfigValue>.RegisterRef(e => e.NumberRule, NumberRuleIdProperty);

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
        /// 把配置值显示出来
        /// </summary>
        /// <returns>配置值</returns>
        public override string Display()
        {
            return "序列号编码规则：{0};".L10nFormat(NumberRule?.Name);
        }
    }
}
