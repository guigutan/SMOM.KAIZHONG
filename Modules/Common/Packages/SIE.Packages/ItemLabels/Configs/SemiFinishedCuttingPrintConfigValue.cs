using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.MetaModel;
using System;

namespace SIE.Packages.ItemLabels.Configs
{
    /// <summary>
    /// 半成品条码配置值
    /// </summary>
    [RootEntity, Serializable]
    public class SemiFinishedCuttingPrintConfigValue : ConfigValue
    {
        /// <summary>
        /// id
        /// </summary>
        public static readonly IRefIdProperty BarcodeRuleIdProperty =
            P<SemiFinishedCuttingPrintConfigValue>.RegisterRefId(e => e.BarcodeRuleId, ReferenceType.Normal);

        /// <summary>
        /// id
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
            P<SemiFinishedCuttingPrintConfigValue>.RegisterRef(e => e.BarcodeRule, BarcodeRuleIdProperty);

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
    /// 半成品条码配置值配置
    /// </summary>
    class SemiFinishedCuttingPrintConfigValueConfig : EntityConfig<SemiFinishedCuttingPrintConfigValue>
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
