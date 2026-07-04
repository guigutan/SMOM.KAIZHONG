using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.SpareParts.Configs
{
    /// <summary>
    /// 备件入库单号
    /// </summary>
    [System.ComponentModel.DisplayName("备件入库单号生成规则")]
    [System.ComponentModel.Description("用于备件入库单号生成的具体规则,具体规则详细请在配置项中进行配置")]
    public class SparePartStoreNoConfig : ModuleConfig<SparePartStoreNoConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly SparePartStoreNoConfigValue defaultValue = new SparePartStoreNoConfigValue { CodeRule = null };

        /// <summary>
        /// 默认值
        /// </summary>
        public override SparePartStoreNoConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }
    }

    /// <summary>
    /// 备件入库单号配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("备件入库单号")]
    public class SparePartStoreNoConfigValue : ConfigValue
    {
        #region 编码规则 CodeRule
        /// <summary>
        /// 编码规则ID
        /// </summary>
        [Label("编码规则")]
        public static readonly IRefIdProperty NumberRuleIdProperty =
              P<SparePartStoreNoConfigValue>.RegisterRefId(e => e.NumberRuleId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<NumberRule> CodeRuleProperty =
            P<SparePartStoreNoConfigValue>.RegisterRef(e => e.CodeRule, NumberRuleIdProperty);

        /// <summary>
        /// 编码规则
        /// </summary>
        public NumberRule CodeRule
        {
            get { return this.GetRefEntity(CodeRuleProperty); }
            set { this.SetRefEntity(CodeRuleProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns>编码规则名称</returns>
        public override string Display()
        {
            if (CodeRule == null)
                return "NIL";
            return CodeRule.Name;
        }
    }
}
