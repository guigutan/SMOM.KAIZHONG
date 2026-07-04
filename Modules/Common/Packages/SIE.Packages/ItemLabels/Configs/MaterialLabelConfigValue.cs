using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.MetaModel;
using System;

namespace SIE.Packages.ItemLabels.Configs
{
    /// <summary>
    /// 物料标签打印配置项
    /// </summary>
    [RootEntity, Serializable]
    public class MaterialLabelConfigValue : ConfigValue
    {
        /// <summary>
        /// id
        /// </summary>
        public static readonly IRefIdProperty BarcodeRuleIdProperty =
           P<MaterialLabelConfigValue>.RegisterRefId(e => e.BarcodeRuleId, ReferenceType.Normal);

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
            P<MaterialLabelConfigValue>.RegisterRef(e => e.BarcodeRule, BarcodeRuleIdProperty);

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
    /// 物料标签打印配置值配置
    /// </summary>
    class MaterialLabelConfigValueConfig : EntityConfig<MaterialLabelConfigValue>
    {
    }
}
