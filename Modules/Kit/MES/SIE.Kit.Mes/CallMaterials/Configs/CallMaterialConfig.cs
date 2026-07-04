using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.ComponentModel;

namespace SIE.Kit.MES.CallMaterials.Configs
{
    /// <summary>
    /// 报检单号配置
    /// </summary>
    [DisplayName("叫料单配置")]
    [Description("用于配置叫料单单号")]
    public class CallMaterialConfig : ModuleConfig<CallMaterialConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly CallMaterialConfigValue defaultValue = new CallMaterialConfigValue();

        /// <summary>
        /// 默认值
        /// </summary>
        public override CallMaterialConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }
    }

    /// <summary>
    /// 叫料单配置值
    /// </summary>
    [RootEntity, Serializable]
    public class CallMaterialConfigValue : ConfigValue
    {
        #region 叫料单单号 NumberRule
        /// <summary>
        /// 叫料单Id
        /// </summary>
        [Label("叫料单号")]
        public static readonly IRefIdProperty NumberRuleIdProperty =
            P<CallMaterialConfigValue>.RegisterRefId(e => e.NumberRuleId, ReferenceType.Normal);

        /// <summary>
        /// 叫料单单号Id
        /// </summary>
        public double? NumberRuleId
        {
            get { return (double?)this.GetRefNullableId(NumberRuleIdProperty); }
            set { this.SetRefNullableId(NumberRuleIdProperty, value); }
        }

        /// <summary>
        /// 叫料单单号
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> NumberRuleProperty =
            P<CallMaterialConfigValue>.RegisterRef(e => e.NumberRule, NumberRuleIdProperty);

        /// <summary>
        /// 叫料单单号
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
            return NumberRule?.Name;
        }
    }
}