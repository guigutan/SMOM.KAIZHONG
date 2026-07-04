using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.FixedAssets.Accounts.Config
{
    /// <summary>
    /// 固定资产台账配置项
    /// </summary>
    [System.ComponentModel.DisplayName("固定资产台账配置项")]
    [System.ComponentModel.Description("固定资产台账配置项")]
    public class FixedAssetAccountConfig : ModuleConfig<FixedAssetAccountConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly FixedAssetAccountConfigValue defaultValue = new FixedAssetAccountConfigValue();

        /// <summary>
        /// 默认值
        /// </summary>
        public override FixedAssetAccountConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }
    }

    /// <summary>
    /// 固定资产台账配置项值
    /// </summary>
    [RootEntity, Serializable]
    [Label("固定资产台账配置项值")]
    public class FixedAssetAccountConfigValue : ConfigValue
    {
        #region 折旧方式 DepreciationWay
        /// <summary>
        /// 折旧方式
        /// </summary>
        [Label("折旧方式")]
        public static readonly Property<DepreciationWay> DepreciationWayProperty = P<FixedAssetAccountConfigValue>.Register(e => e.DepreciationWay);

        /// <summary>
        /// 折旧方式
        /// </summary>
        public DepreciationWay DepreciationWay
        {
            get { return this.GetProperty(DepreciationWayProperty); }
            set { this.SetProperty(DepreciationWayProperty, value); }
        }
        #endregion

        #region 编码规则 NumberRule
        /// <summary>
        /// 编码规则ID
        /// </summary>
        [Label("固定资产台账编码规则")]
        public static readonly IRefIdProperty NumberRuleIdProperty =
              P<FixedAssetAccountConfigValue>.RegisterRefId(e => e.NumberRuleId, ReferenceType.Normal);

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
            P<FixedAssetAccountConfigValue>.RegisterRef(e => e.Number, NumberRuleIdProperty);

        /// <summary>
        /// 编码规则
        /// </summary>
        public NumberRule Number
        {
            get { return this.GetRefEntity(NumberRuleProperty); }
            set { this.SetRefEntity(NumberRuleProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns></returns>
        public override string Display()
        {
            return "折旧方式:{0}".L10nFormat(DepreciationWay.ToLabel().L10N());
        }
    }
}
