using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.MetaModel;
using System;

namespace SIE.Packages.ItemLabels.Configs
{
    /// <summary>
    /// 成品条码值配置
    /// </summary>
    [RootEntity, Serializable]
    public class FinishedProductPrintConfigValue : ConfigValue
    {
        /// <summary>
        /// 条码规则
        /// </summary>
        public static readonly IRefIdProperty BarcodeRuleIdProperty =
            P<FinishedProductPrintConfigValue>.RegisterRefId(e => e.BarcodeRuleId, ReferenceType.Normal);

        /// <summary>
        /// ID
        /// </summary>
        public double? BarcodeRuleId
        {
            get { return (double?)this.GetRefNullableId(BarcodeRuleIdProperty); }
            set { this.SetRefNullableId(BarcodeRuleIdProperty, value); }
        }

        /// <summary>
        /// 条码规则
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> BarcodeRuleProperty =
            P<FinishedProductPrintConfigValue>.RegisterRef(e => e.BarcodeRule, BarcodeRuleIdProperty);

        /// <summary>
        /// NumberRuleAttachment  BarcodeAttachment
        /// </summary>
        public NumberRule BarcodeRule
        {
            get { return this.GetRefEntity(BarcodeRuleProperty); }
            set { this.SetRefEntity(BarcodeRuleProperty, value); }
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            if (BarcodeRule == null)
                return "NIL";
            return BarcodeRule.Name;
        }
    }

    /// <summary>
    /// 成品条码值配置
    /// </summary>
    class FinishedProductPrintConfigValueConfig : EntityConfig<FinishedProductPrintConfigValue>
    {
        /////// <summary>
        /////// 验证
        /////// </summary>
        /////// <param name="rules">规则</param>
        ////protected override void AddValidations(IValidationDeclarer rules)
        ////{
        ////}
    }
}
