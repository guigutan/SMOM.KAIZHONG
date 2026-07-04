using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Fixtures.Models.Config
{
    /// <summary>
    /// 工治具型号配置项
    /// </summary>
    [System.ComponentModel.DisplayName("工治具型号配置项")]
    [System.ComponentModel.Description("工治具型号配置项")]
    public class FixturesModelsConfig : ModuleConfig<FixturesModelsConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly FixturesModelsConfigValue defaultValue = new FixturesModelsConfigValue();

        /// <summary>
        /// 默认值
        /// </summary>
        public override FixturesModelsConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }
    }

    /// <summary>
    /// 固定资产台账配置项值
    /// </summary>
    [RootEntity, Serializable]
    [Label("工治具型号配置项值")]
    public class FixturesModelsConfigValue : ConfigValue
    {
        #region 编码规则 NumberRule
        /// <summary>
        /// 编码规则ID
        /// </summary>
        [Label("工治具型号编码规则")]
        public static readonly IRefIdProperty NumberRuleIdProperty =
              P<FixturesModelsConfigValue>.RegisterRefId(e => e.NumberRuleId, ReferenceType.Normal);

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
            P<FixturesModelsConfigValue>.RegisterRef(e => e.Number, NumberRuleIdProperty);

        /// <summary>
        /// 编码规则
        /// </summary>
        public NumberRule Number
        {
            get { return this.GetRefEntity(NumberRuleProperty); }
            set { this.SetRefEntity(NumberRuleProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns></returns>
        public override string Display()
        {
            return "";
        }
    }
}
