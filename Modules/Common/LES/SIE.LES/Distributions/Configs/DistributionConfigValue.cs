using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.LES.Distributions.Configs
{
    /// <summary>
    /// 配送单号单号配置项内容
    /// </summary>
    [RootEntity, Serializable]
    [DisplayMember(nameof(DistributionNoRule))]
    [Label("配送单管理生成规则配置")]
    public class DistributionConfigValue : ConfigValue
    {
        #region 编码规则 DistributionNoRule
        /// <summary>
        /// 编码规则ID
        /// </summary>
        [Label("单号规则")]
        public static readonly IRefIdProperty DistributionNoRuleIdProperty =
              P<DistributionConfigValue>.RegisterRefId(e => e.DistributionNoRuleId, ReferenceType.Normal);

        /// <summary>
        /// 编码规则ID
        /// </summary>
        public double? DistributionNoRuleId
        {
            get { return (double?)this.GetRefNullableId(DistributionNoRuleIdProperty); }
            set { this.SetRefNullableId(DistributionNoRuleIdProperty, value); }
        }

        /// <summary>
        /// 编码规则
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> DistributionNoRuleProperty =
            P<DistributionConfigValue>.RegisterRef(e => e.DistributionNoRule, DistributionNoRuleIdProperty);

        /// <summary>
        /// 编码规则
        /// </summary>
        public NumberRule DistributionNoRule
        {
            get { return this.GetRefEntity(DistributionNoRuleProperty); }
            set { this.SetRefEntity(DistributionNoRuleProperty, value); }
        }
        #endregion

        #region 是否跳过扫描配送/配送接收 IsNoDistribution
        /// <summary>
        /// 是否跳过扫描配送/配送接收
        /// </summary>
        [Label("是否跳过扫描配送/配送接收")]
        public static readonly Property<IsNoDistributionType?> IsNoDistributionProperty = P<DistributionConfigValue>.Register(e => e.IsNoDistribution);

        /// <summary>
        /// 是否跳过扫描配送/配送接收
        /// </summary>
        public IsNoDistributionType? IsNoDistribution
        {
            get { return this.GetProperty(IsNoDistributionProperty); }
            set { this.SetProperty(IsNoDistributionProperty, value); }
        }
        #endregion

        /// <summary>
        ///  显示值
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            if (DistributionNoRule == null)
                return "NIL";
            return DistributionNoRule?.Name + ";" + IsNoDistribution?.ToLabel().L10N();
        }
    }
}
