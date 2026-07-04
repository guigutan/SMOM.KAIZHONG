using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WorkOrders.Configs
{
    /// <summary>
    /// 工单号配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("工单号配置值")]
    public class WorkOrderNoConfigValue : ConfigValue
    {
        #region 编码规则 BacodeRule
        /// <summary>
        /// 编码规则ID
        /// </summary>
        [Label("编码规则")]
        public static readonly IRefIdProperty NumberRuleIdProperty =
              P<WorkOrderNoConfigValue>.RegisterRefId(e => e.NumberRuleId, ReferenceType.Normal);

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
            P<WorkOrderNoConfigValue>.RegisterRef(e => e.BacodeRule, NumberRuleIdProperty);

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
