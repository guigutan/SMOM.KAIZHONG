using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Dock.DockAppoints.Configs
{
    /// <summary>
    /// 月台预约配置项内容
    /// </summary>
    [RootEntity, Serializable]
    [DisplayMember(nameof(NumberRule))]
    [Label("月台预约配置项内容")]
    public class DockAppointNoConfigValue : ConfigValue
    {
        #region 编码规则 NumberRule
        /// <summary>
        /// 编码规则ID
        /// </summary>
        [Label("生成规则")]
        public static readonly IRefIdProperty NumberRuleIdProperty =
              P<DockAppointNoConfigValue>.RegisterRefId(e => e.NumberRuleId, ReferenceType.Normal);

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
            P<DockAppointNoConfigValue>.RegisterRef(e => e.NumberRule, NumberRuleIdProperty);

        /// <summary>
        /// 编码规则
        /// </summary>
        public NumberRule NumberRule
        {
            get { return this.GetRefEntity(NumberRuleProperty); }
            set { this.SetRefEntity(NumberRuleProperty, value); }
        }
        #endregion

        #region 打印模板 PrintTemplate
        /// <summary>
        /// 打印模板ID
        /// </summary>
        [Label("打印模板")]
        public static readonly IRefIdProperty PrintTemplateIdProperty =
              P<DockAppointNoConfigValue>.RegisterRefId(e => e.PrintTemplateId, ReferenceType.Normal);

        /// <summary>
        /// 打印模板ID
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
            P<DockAppointNoConfigValue>.RegisterRef(e => e.PrintTemplate, PrintTemplateIdProperty);

        /// <summary>
        /// 打印模板
        /// </summary>
        public PrintTemplate PrintTemplate
        {
            get { return this.GetRefEntity(PrintTemplateProperty); }
            set { this.SetRefEntity(PrintTemplateProperty, value); }
        }
        #endregion

        #region 最大预约时间(H) MaxAppointTime
        /// <summary>
        /// 最大预约时间(H)
        /// </summary>
        [Label("最大预约时间(H)")]
        public static readonly Property<decimal?> MaxAppointTimeProperty = P<DockAppointNoConfigValue>.Register(e => e.MaxAppointTime);

        /// <summary>
        /// 最大预约时间(H)
        /// </summary>
        public decimal? MaxAppointTime
        {
            get { return this.GetProperty(MaxAppointTimeProperty); }
            set { this.SetProperty(MaxAppointTimeProperty, value); }
        }
        #endregion

        /// <summary>
        ///  显示值
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            string ruleName = "NIL";
            string printLabelRuleName = "NIL";

            if (NumberRule != null)
            {
                ruleName = NumberRule.Name;
            }

            if (PrintTemplate != null)
            {
                printLabelRuleName = PrintTemplate.FileName;
            }
            return "编码规则:{0},单据模板:{1},最大预约时间(H):{2}".L10nFormat(ruleName, printLabelRuleName, MaxAppointTime);
        }
    }
}
