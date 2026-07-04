using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Barcodes.Panels.Configs
{
    /// <summary>
    /// 拼板码打印规则配置
    /// </summary>
    [RootEntity, Serializable]
    [Label("拼板码打印规则配置")]
    public class PanelPrintConfigValue : ConfigValue
    {
        #region 编码规则 BacodeRule
        /// <summary>
        /// 编码规则ID
        /// </summary>
        [Label("编码规则")]
        public static readonly IRefIdProperty NumberRuleIdProperty =
              P<PanelPrintConfigValue>.RegisterRefId(e => e.NumberRuleId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<NumberRule> BacodeRuleProperty =
            P<PanelPrintConfigValue>.RegisterRef(e => e.BacodeRule, NumberRuleIdProperty);

        /// <summary>
        /// 编码规则
        /// </summary>
        public NumberRule BacodeRule
        {
            get { return this.GetRefEntity(BacodeRuleProperty); }
            set { this.SetRefEntity(BacodeRuleProperty, value); }
        }
        #endregion

        #region 条码模板 LabelTemplate
        /// <summary>
        /// 条码模板Id
        /// </summary>
        [Label("条码模板")]
        public static readonly IRefIdProperty LabelTemplateIdProperty =
            P<PanelPrintConfigValue>.RegisterRefId(e => e.LabelTemplateId, ReferenceType.Normal);

        /// <summary>
        /// 条码模板Id
        /// </summary>
        public double? LabelTemplateId
        {
            get { return (double?)this.GetRefNullableId(LabelTemplateIdProperty); }
            set { this.SetRefNullableId(LabelTemplateIdProperty, value); }
        }

        /// <summary>
        /// 条码模板
        /// </summary>
        public static readonly RefEntityProperty<PrintTemplate> LabelTemplateProperty =
            P<PanelPrintConfigValue>.RegisterRef(e => e.LabelTemplate, LabelTemplateIdProperty);

        /// <summary>
        /// 条码模板
        /// </summary>
        public PrintTemplate LabelTemplate
        {
            get { return this.GetRefEntity(LabelTemplateProperty); }
            set { this.SetRefEntity(LabelTemplateProperty, value); }
        }
        #endregion
    }
}
