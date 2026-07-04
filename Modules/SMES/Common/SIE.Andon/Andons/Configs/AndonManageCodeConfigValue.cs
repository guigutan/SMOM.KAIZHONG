using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.Andons.Configs
{
    /// <summary>
    /// 安灯管理事件编码生成规则
    /// </summary>
    [RootEntity, Serializable]
    [Label("安灯管理事件编码生成规则")]
    public class AndonManageCodeConfigValue : ConfigValue
    {
        #region 事件编码规则 AndonManageCodeRule
        /// <summary>
        /// 事件编码规则Id
        /// </summary>
        [Label("事件编码规则")]
        public static readonly IRefIdProperty AndonManageCodeRuleIdProperty =
            P<AndonManageCodeConfigValue>.RegisterRefId(e => e.AndonManageCodeRuleId, ReferenceType.Normal);

        /// <summary>
        /// 事件编码规则Id
        /// </summary>
        public double AndonManageCodeRuleId
        {
            get { return (double)this.GetRefId(AndonManageCodeRuleIdProperty); }
            set { this.SetRefId(AndonManageCodeRuleIdProperty, value); }
        }

        /// <summary>
        /// 事件编码规则
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> AndonManageCodeRuleProperty =
            P<AndonManageCodeConfigValue>.RegisterRef(e => e.AndonManageCodeRule, AndonManageCodeRuleIdProperty);

        /// <summary>
        /// 事件编码规则
        /// </summary>
        public NumberRule AndonManageCodeRule
        {
            get { return this.GetRefEntity(AndonManageCodeRuleProperty); }
            set { this.SetRefEntity(AndonManageCodeRuleProperty, value); }
        }
        #endregion
        /// <summary>
        /// 显示值
        /// </summary>
        /// <returns></returns>
        public override string Display()
        {
            if(this.AndonManageCodeRule == null)
            {
                return "NIL";
            }
            return AndonManageCodeRule.Name;
        }
    }
}
