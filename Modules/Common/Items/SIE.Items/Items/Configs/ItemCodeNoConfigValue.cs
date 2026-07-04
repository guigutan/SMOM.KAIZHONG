using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Items.Items.Configs
{
    /// <summary>
    /// 物料编码配置项内容
    /// </summary>
    [RootEntity, Serializable]
    [Label("物料编码生成规则")]
    public class ItemCodeNoConfigValue : ConfigValue
    {
        #region 编码规则 BacodeRule
        /// <summary>
        /// 编码规则ID
        /// </summary>
        [Label("编码规则")]
        public static readonly IRefIdProperty ItemCodeRuleIdProperty =
              P<ItemCodeNoConfigValue>.RegisterRefId(e => e.ItemCodeRuleId, ReferenceType.Normal);

        /// <summary>
        /// 编码规则ID
        /// </summary>
        public double? ItemCodeRuleId
        {
            get { return (double?)this.GetRefNullableId(ItemCodeRuleIdProperty); }
            set { this.SetRefNullableId(ItemCodeRuleIdProperty, value); }
        }

        /// <summary>
        /// 编码规则
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> ItemCodeRuleProperty =
            P<ItemCodeNoConfigValue>.RegisterRef(e => e.ItemCodeRule, ItemCodeRuleIdProperty);

        /// <summary>
        /// 编码规则
        /// </summary>
        public NumberRule ItemCodeRule
        {
            get { return this.GetRefEntity(ItemCodeRuleProperty); }
            set { this.SetRefEntity(ItemCodeRuleProperty, value); }
        }

        #endregion

        /// <summary>
        ///  显示值
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            if (ItemCodeRule == null)
                return "NIL";
            return ItemCodeRule.Name;
        }
    }
}
