using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Warehouses.Configs
{
    /// <summary>
    /// 仓库编码配置项内容
    /// </summary>
    [RootEntity, Serializable]
    [Label("仓库编码规则")]
    public class WarehousesCodeConfigValue : ConfigValue
    {
        #region 编码规则 WarehousesCodeRule
        /// <summary>
        /// 编码规则ID
        /// </summary>
        [Label("仓库编码")]
        public static readonly IRefIdProperty NumberRuleIdProperty =
              P<WarehousesCodeConfigValue>.RegisterRefId(e => e.WarehousesCodeRuleId, ReferenceType.Normal);

        /// <summary>
        /// 编码规则ID
        /// </summary>
        public double? WarehousesCodeRuleId
        {
            get { return (double?)this.GetRefNullableId(NumberRuleIdProperty); }
            set { this.SetRefNullableId(NumberRuleIdProperty, value); }
        }

        /// <summary>
        /// 编码规则
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> WarehousesCodeRuleProperty =
            P<WarehousesCodeConfigValue>.RegisterRef(e => e.WarehousesCodeRule, NumberRuleIdProperty);

        /// <summary>
        /// 编码规则
        /// </summary>
        public NumberRule WarehousesCodeRule
        {
            get { return this.GetRefEntity(WarehousesCodeRuleProperty); }
            set { this.SetRefEntity(WarehousesCodeRuleProperty, value); }
        }
        #endregion

        /// <summary>
        ///  显示值
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            if (WarehousesCodeRule == null)
                return "NIL";
            return WarehousesCodeRule.Name;
        }
    }
}
