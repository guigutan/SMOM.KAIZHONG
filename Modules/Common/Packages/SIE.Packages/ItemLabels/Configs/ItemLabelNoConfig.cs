using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Core.WorkOrders;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Packages.ItemLabels.Configs
{
    /// <summary>
    /// 物料标签号配置
    /// </summary>
    [System.ComponentModel.DisplayName("物料标签号生成规则")]
    [System.ComponentModel.Description("用于选择物料标签号生成的具体规则,具体规则详细请在条码规则进行配置")]
    public class ItemLabelNoConfig : ModuleConfig<ItemLabelNoConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly ItemLabelNoConfigValue defaultValue = new ItemLabelNoConfigValue { BacodeRule = null };

        /// <summary>
        /// 默认值 
        /// </summary>
        public override ItemLabelNoConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// 物料标签号配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("物料标签号配置值")]
    public class ItemLabelNoConfigValue : ConfigValue
    {
        #region 编码规则 BacodeRule
        /// <summary>
        /// 编码规则ID
        /// </summary>
        [Label("编码规则")]
        public static readonly IRefIdProperty NumberRuleIdProperty =
              P<ItemLabelNoConfigValue>.RegisterRefId(e => e.NumberRuleId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<NumberRule> BacodeRuleProperty =
            P<ItemLabelNoConfigValue>.RegisterRef(e => e.BacodeRule, NumberRuleIdProperty);

        /// <summary>
        /// 编码规则
        /// </summary>
        public NumberRule BacodeRule
        {
            get { return this.GetRefEntity(BacodeRuleProperty); }
            set { this.SetRefEntity(BacodeRuleProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示 
        /// </summary>
        /// <returns>编码规则名称</returns>
        public override string Display()
        {
            if (BacodeRule == null)
                return "NIL";
            return BacodeRule.Name;
        }
    }
}
