using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;

namespace SIE.Warehouses.Configs
{
    /// <summary>
    /// 库位配置项内容
    /// </summary>
    [RootEntity, Serializable]
    [Label("库位编码规则")]
    public class StorageLocationCodeConfigValue : ConfigValue
    {
        #region 编码规则 StorageLocationCode
        /// <summary>
        /// 编码规则ID
        /// </summary>
        [Label("库位编码规则")]
        public static readonly IRefIdProperty StorageLocationCodeRuleIdProperty =
              P<StorageLocationCodeConfigValue>.RegisterRefId(e => e.StorageLocationCodeRuleId, ReferenceType.Normal);

        /// <summary>
        /// 编码规则ID
        /// </summary>
        public double? StorageLocationCodeRuleId
        {
            get { return (double?)this.GetRefNullableId(StorageLocationCodeRuleIdProperty); }
            set { this.SetRefNullableId(StorageLocationCodeRuleIdProperty, value); }
        }

        /// <summary>
        /// 编码规则
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> StorageLocationCodeRuleProperty =
            P<StorageLocationCodeConfigValue>.RegisterRef(e => e.StorageLocationCodeRule, StorageLocationCodeRuleIdProperty);

        /// <summary>
        /// 编码规则
        /// </summary>
        public NumberRule StorageLocationCodeRule
        {
            get { return this.GetRefEntity(StorageLocationCodeRuleProperty); }
            set { this.SetRefEntity(StorageLocationCodeRuleProperty, value); }
        }
        #endregion

        #region 打印模板 StorageLocationPrintRule
        /// <summary>
        /// 打印模板
        /// </summary>
        [Label("打印模板")]
        public static readonly IRefIdProperty StorageLocationPrintRuleIdProperty =
              P<StorageLocationCodeConfigValue>.RegisterRefId(e => e.StorageLocationPrintRuleId, ReferenceType.Normal);

        /// <summary>
        /// 打印模板
        /// </summary>
        public double? StorageLocationPrintRuleId
        {
            get { return (double?)this.GetRefNullableId(StorageLocationPrintRuleIdProperty); }
            set { this.SetRefNullableId(StorageLocationPrintRuleIdProperty, value); }
        }

        /// <summary>
        /// 打印模板
        /// </summary>
        public static readonly RefEntityProperty<PrintTemplate> StorageLocationPrintRuleProperty =
            P<StorageLocationCodeConfigValue>.RegisterRef(e => e.StorageLocationPrintRule, StorageLocationPrintRuleIdProperty);

        /// <summary>
        /// 打印模板
        /// </summary>
        public PrintTemplate StorageLocationPrintRule
        {
            get { return this.GetRefEntity(StorageLocationPrintRuleProperty); }
            set { this.SetRefEntity(StorageLocationPrintRuleProperty, value); }
        }
        #endregion

        /// <summary>
        ///  显示值
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            List<string> displayList = new List<string>();
            if (StorageLocationCodeRule != null)
                displayList.Add(string.Format("编码规则：{0}".L10nFormat(StorageLocationCodeRule.Name)));

            if (StorageLocationPrintRule != null)
                displayList.Add(string.Format("打印模板：{0}".L10nFormat(StorageLocationPrintRule.FileName)));

            if (displayList.Count == 0)
            {
                return "NIL";
            }

            return string.Join(",", displayList);
        }
    }
}
