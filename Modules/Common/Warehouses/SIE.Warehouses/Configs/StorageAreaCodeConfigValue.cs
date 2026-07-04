using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Warehouses.Configs
{
    /// <summary>
    /// 库区编码配置项内容
    /// </summary>
    [RootEntity, Serializable]
    [Label("库区编码规则")]
    public class StorageAreaCodeConfigValue : ConfigValue
    {
        #region 编码规则 StorageAreaCodeRule
        /// <summary>
        /// 编码规则ID
        /// </summary>
        [Label("库区编码规则")]
        public static readonly IRefIdProperty NumberRuleIdProperty =
              P<StorageAreaCodeConfigValue>.RegisterRefId(e => e.StorageAreaCodeRuleId, ReferenceType.Normal);

        /// <summary>
        /// 编码规则ID
        /// </summary>
        public double? StorageAreaCodeRuleId
        {
            get { return (double?)this.GetRefNullableId(NumberRuleIdProperty); }
            set { this.SetRefNullableId(NumberRuleIdProperty, value); }
        }

        /// <summary>
        /// 编码规则
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> StorageAreaCodeRuleProperty =
            P<StorageAreaCodeConfigValue>.RegisterRef(e => e.StorageAreaCodeRule, NumberRuleIdProperty);

        /// <summary>
        /// 编码规则
        /// </summary>
        public NumberRule StorageAreaCodeRule
        {
            get { return this.GetRefEntity(StorageAreaCodeRuleProperty); }
            set { this.SetRefEntity(StorageAreaCodeRuleProperty, value); }
        }
        #endregion

        /// <summary>
        ///  显示值
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            if (StorageAreaCodeRule == null)
                return "NIL";
            return StorageAreaCodeRule.Name;
        }
    }
}
