using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.LES.MaterialReceptions.Configs
{
    /// <summary>
    /// 生产退料配置项
    /// </summary>
    [RootEntity, Serializable]
    [Label("生产退料配置项")]
    public class ReturnMaterialConfigValue : ConfigValue
    {
        #region 编码规则 ReturnMaterialCodeRule
        /// <summary>
        /// 编码规则Id
        /// </summary>
        [Label("编码规则")]
        public static readonly IRefIdProperty ReturnMaterialCodeRuleIdProperty =
            P<ReturnMaterialConfigValue>.RegisterRefId(e => e.ReturnMaterialCodeRuleId, ReferenceType.Normal);

        /// <summary>
        /// 编码规则Id
        /// </summary>
        public double ReturnMaterialCodeRuleId
        {
            get { return (double)this.GetRefId(ReturnMaterialCodeRuleIdProperty); }
            set { this.SetRefId(ReturnMaterialCodeRuleIdProperty, value); }
        }

        /// <summary>
        /// 编码规则
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> ReturnMaterialCodeRuleProperty =
            P<ReturnMaterialConfigValue>.RegisterRef(e => e.ReturnMaterialCodeRule, ReturnMaterialCodeRuleIdProperty);

        /// <summary>
        /// 编码规则
        /// </summary>
        public NumberRule ReturnMaterialCodeRule
        {
            get { return this.GetRefEntity(ReturnMaterialCodeRuleProperty); }
            set { this.SetRefEntity(ReturnMaterialCodeRuleProperty, value); }
        }
        #endregion

        #region 退料原因必填 ReasonRequired
        /// <summary>
        /// 退料原因必填
        /// </summary>
        [Label("退料原因必填")]
        public static readonly Property<bool> ReasonRequiredProperty = P<ReturnMaterialConfigValue>.Register(e => e.ReasonRequired);

        /// <summary>
        /// 退料原因必填
        /// </summary>
        public bool ReasonRequired
        {
            get { return this.GetProperty(ReasonRequiredProperty); }
            set { this.SetProperty(ReasonRequiredProperty, value); }
        }
        #endregion

        #region 退料原因默认值 ReasonDefault
        /// <summary>
        /// 退料原因默认值
        /// </summary>
        [Label("退料原因默认值")]
        public static readonly Property<string> ReasonDefaultProperty = P<ReturnMaterialConfigValue>.Register(e => e.ReasonDefault);

        /// <summary>
        /// 退料原因默认值
        /// </summary>
        public string ReasonDefault
        {
            get { return this.GetProperty(ReasonDefaultProperty); }
            set { this.SetProperty(ReasonDefaultProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示值
        /// </summary>
        /// <returns></returns>
        public override string Display()
        {
            var display = "";
            if (this.ReturnMaterialCodeRule != null)
            {
                display = ReturnMaterialCodeRule.Name;
            }
            if (ReasonDefault.IsNotEmpty())
            {
                display = "退料原因默认值".L10N() + ReasonDefault;
            }

            return "编码规则:{0},退料原因必填:".L10nFormat(display, ReasonRequired ? "yes" : "no");
        }
    }
}
