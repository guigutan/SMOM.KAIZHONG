using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.DIST.Distribution.Configs
{
    /// <summary>
    /// 单据条码配置项
    /// </summary>
    [RootEntity, Serializable]
    [Label("销售退货单标签生成规则")]
    public class BillLabelConfigValue : ConfigValue
    {
        #region NumberRule

        /// <summary>
        /// 条码规则ID
        /// </summary>
        [Label("条码规则")]
        public static readonly IRefIdProperty NumberRuleIdProperty =
              P<BillLabelConfigValue>.RegisterRefId(e => e.NumberRuleId, ReferenceType.Normal);

        /// <summary>
        /// 条码规则
        /// </summary>
        public double? NumberRuleId
        {
            get { return (double?)this.GetRefNullableId(NumberRuleIdProperty); }
            set { this.SetRefNullableId(NumberRuleIdProperty, value); }
        }

        /// <summary>
        /// 条码规则
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> NumberRuleProperty =
            P<BillLabelConfigValue>.RegisterRef(e => e.NumberRule, NumberRuleIdProperty);

        /// <summary>
        /// 条码规则
        /// </summary>
        public NumberRule NumberRule
        {
            get { return this.GetRefEntity(NumberRuleProperty); }
            set { this.SetRefEntity(NumberRuleProperty, value); }
        }

        #endregion

        #region 打印模板 RmaPrintTemplate

        /// <summary>
        /// 打印模板ID
        /// </summary>
        [Label("标签模板")]
        public static readonly IRefIdProperty PrintTemplateIdProperty =
            P<BillLabelConfigValue>.RegisterRefId(e => e.PrintTemplateId, ReferenceType.Normal);

        /// <summary>
        /// 打印模板
        /// </summary>
        public double? PrintTemplateId
        {
            get { return (double?)this.GetRefNullableId(PrintTemplateIdProperty); }
            set { this.SetRefNullableId(PrintTemplateIdProperty, value); }
        }

        /// <summary>
        /// 打印模板
        /// </summary>
        public static readonly RefEntityProperty<PrintTemplate> PrintTemplateProperty =
            P<BillLabelConfigValue>.RegisterRef(e => e.PrintTemplate, PrintTemplateIdProperty);

        /// <summary>
        /// 打印模板
        /// </summary>
        public PrintTemplate PrintTemplate
        {
            get { return this.GetRefEntity(PrintTemplateProperty); }
            set { this.SetRefEntity(PrintTemplateProperty, value); }
        }

        #endregion

        #region 编辑时，编码是否允许修改 IsNumberRuleSelected
        /// <summary>
        /// 编辑时，编码是否允许修改，扩展只读属性
        /// </summary>
        public static readonly Property<bool> IsNumberRuleSelectedRealOnlyProperty = P<BillLabelConfigValue>.RegisterReadOnly(
            e => e.IsNumberRuleSelectedRealOnly, e => e.GetIsNumberRuleSelectedRealOnly(), NumberRuleProperty);

        /// <summary>
        /// 编辑时，编码是否允许修改
        /// </summary>
        public bool IsNumberRuleSelectedRealOnly
        {
            get { return this.GetProperty(IsNumberRuleSelectedRealOnlyProperty); }
        }

        /// <summary>
        /// 获取编辑时，编码是否允许修改
        /// </summary>
        /// <returns>True:允许修改 False:不允许</returns>
        private bool GetIsNumberRuleSelectedRealOnly()
        {
            return NumberRule == null;
        }

        #endregion
        /// <summary>
        /// 把值显示出来
        /// </summary>
        /// <returns>配置值</returns>
        public override string Display()
        {
            return "{0},{1}".FormatArgs(
                DisplayHelper.Display(NumberRule != null ? NumberRule.Name : "NIL"),
                DisplayHelper.Display(PrintTemplate != null ? PrintTemplate.FileName : "NIL"));
        }
    }

    /// <summary>
    /// 页面配置
    /// </summary>
    class RmaBillLabelConfigValueConfig : EntityConfig<BillLabelConfigValue>
    {
        /// <summary>
        /// 对实体验证规则的配置
        /// </summary>
        /// <param name="rules">验证规则声明器</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(BillLabelConfigValue.NumberRuleIdProperty, new RequiredRule());
        }
    }
}
