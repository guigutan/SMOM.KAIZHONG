using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.MES.PrepareProducts.Configs
{
    /// <summary>
    /// 产前准备项目维护实体编码配置项
    /// </summary>
    [RootEntity, Serializable]
    [Label("产前准备项目维护实体编码配置项")]
    public class PrepareProjectCodeConfigValue : ConfigValue
    {
        #region 项目编码规则 ProCodeRule
        /// <summary>
        /// 项目编码规则Id
        /// </summary>
        [Label("项目编码规则")]
        public static readonly IRefIdProperty ProCodeRuleIdProperty =
            P<PrepareProjectCodeConfigValue>.RegisterRefId(e => e.ProCodeRuleId, ReferenceType.Normal);

        /// <summary>
        /// 项目编码规则Id
        /// </summary>
        public double? ProCodeRuleId
        {
            get { return (double?)this.GetRefNullableId(ProCodeRuleIdProperty); }
            set { this.SetRefNullableId(ProCodeRuleIdProperty, value); }
        }

        /// <summary>
        /// 项目编码规则
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> ProCodeRuleProperty =
            P<PrepareProjectCodeConfigValue>.RegisterRef(e => e.ProCodeRule, ProCodeRuleIdProperty);

        /// <summary>
        /// 项目编码规则
        /// </summary>
        public NumberRule ProCodeRule
        {
            get { return this.GetRefEntity(ProCodeRuleProperty); }
            set { this.SetRefEntity(ProCodeRuleProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示值
        /// </summary>
        /// <returns></returns>
        public override string Display()
        {
            if (this.ProCodeRule == null)
            {
                return "NULL";
            }
            return this.ProCodeRule.Name;
        }
    }
}
