using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.LoadItems.Configs
{
    /// <summary>
    /// 工单耗用量耗用单号编码规则
    /// </summary>
    [RootEntity, Serializable]
    [Label("工单耗用量耗用单号编码规则")]
    public class WoCostItemNoConfigValue : ConfigValue
    {
        #region 耗用单号编码规则 CostNoRule
        /// <summary>
        /// 耗用单号编码规则Id
        /// </summary>
        [Label("耗用单号编码规则")]
        public static readonly IRefIdProperty CostNoRuleIdProperty =
            P<WoCostItemNoConfigValue>.RegisterRefId(e => e.CostNoRuleId, ReferenceType.Normal);

        /// <summary>
        /// 耗用单号编码规则Id
        /// </summary>
        public double CostNoRuleId
        {
            get { return (double)this.GetRefId(CostNoRuleIdProperty); }
            set { this.SetRefId(CostNoRuleIdProperty, value); }
        }

        /// <summary>
        /// 耗用单号编码规则
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> CostNoRuleProperty =
            P<WoCostItemNoConfigValue>.RegisterRef(e => e.CostNoRule, CostNoRuleIdProperty);

        /// <summary>
        /// 耗用单号编码规则
        /// </summary>
        public NumberRule CostNoRule
        {
            get { return this.GetRefEntity(CostNoRuleProperty); }
            set { this.SetRefEntity(CostNoRuleProperty, value); }
        }
        #endregion


        /// <summary>
        /// 默认显示值
        /// </summary>
        /// <returns></returns>
        public override string Display()
        {
            if (this.CostNoRule == null)
            {
                return "NULL";
            }
            return CostNoRule.Name;
        }
    }
}
