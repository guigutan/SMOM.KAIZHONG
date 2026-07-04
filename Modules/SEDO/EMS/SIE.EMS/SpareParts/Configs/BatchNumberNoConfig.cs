using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.SpareParts.Configs
{
    /// <summary>
    /// 批次号编码和序列号编码生成规则
    /// </summary>
    [System.ComponentModel.DisplayName("批次号编码和序列号编码生成规则")]
    [System.ComponentModel.Description("用于备件接收和入库中批次号和序列号生成的具体规则")]
    public class BatchNumberNoConfig : ModuleConfig<BatchNumberConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly BatchNumberConfigValue defaultValue = new BatchNumberConfigValue { BatchNumberRule = null, SnNumberRule = null };

        /// <summary>
        /// 默认值
        /// </summary>
        public override BatchNumberConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }
    }

    /// <summary>
    /// 批次号配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("批次号")]
    public class BatchNumberConfigValue : ConfigValue
    {
        #region 批次号编码规则 BatchNumberRule
        /// <summary>
        /// 批次号编码规则Id
        /// </summary>
        [Label("批次号编码规则")]
        public static readonly IRefIdProperty BatchNumberRuleIdProperty =
            P<BatchNumberConfigValue>.RegisterRefId(e => e.BatchNumberRuleId, ReferenceType.Normal);

        /// <summary>
        /// 批次号编码规则Id
        /// </summary>
        public double? BatchNumberRuleId
        {
            get { return (double?)this.GetRefNullableId(BatchNumberRuleIdProperty); }
            set { this.SetRefNullableId(BatchNumberRuleIdProperty, value); }
        }

        /// <summary>
        /// 批次号编码规则
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> BatchNumberRuleProperty =
            P<BatchNumberConfigValue>.RegisterRef(e => e.BatchNumberRule, BatchNumberRuleIdProperty);

        /// <summary>
        /// 批次号编码规则
        /// </summary>
        public NumberRule BatchNumberRule
        {
            get { return this.GetRefEntity(BatchNumberRuleProperty); }
            set { this.SetRefEntity(BatchNumberRuleProperty, value); }
        }
        #endregion

        #region 序列号编码规则 SnNumberRule
        /// <summary>
        /// 序列号编码规则Id
        /// </summary>
        [Label("序列号编码规则")]
        public static readonly IRefIdProperty SnNumberRuleIdProperty =
            P<BatchNumberConfigValue>.RegisterRefId(e => e.SnNumberRuleId, ReferenceType.Normal);

        /// <summary>
        /// 序列号编码规则Id
        /// </summary>
        public double? SnNumberRuleId
        {
            get { return (double?)this.GetRefNullableId(SnNumberRuleIdProperty); }
            set { this.SetRefNullableId(SnNumberRuleIdProperty, value); }
        }

        /// <summary>
        /// 序列号编码规则
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> SnNumberRuleProperty =
            P<BatchNumberConfigValue>.RegisterRef(e => e.SnNumberRule, SnNumberRuleIdProperty);

        /// <summary>
        /// 序列号编码规则
        /// </summary>
        public NumberRule SnNumberRule
        {
            get { return this.GetRefEntity(SnNumberRuleProperty); }
            set { this.SetRefEntity(SnNumberRuleProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns>编码规则名称</returns>
        public override string Display()
        {
            if (BatchNumberRule == null || SnNumberRule == null)
                return "NIL";
            return BatchNumberRule.Name + ";" + SnNumberRule.Name;
        }
    }
}
