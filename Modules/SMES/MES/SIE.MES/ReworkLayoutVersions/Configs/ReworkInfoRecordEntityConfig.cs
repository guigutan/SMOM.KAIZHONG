using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.MES.WorkOrders.Configs;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ReworkLayoutVersions.Configs
{
    /// <summary>
    /// 返工信息配置值
    /// </summary>
    [System.ComponentModel.DisplayName("返工信息配置值")]
    [System.ComponentModel.Description("返工信息配置值")]
    public class ReworkInfoRecordEntityConfig: ModuleConfig<ReworkInfoRecordConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly ReworkInfoRecordConfigValue defaultValue = new ReworkInfoRecordConfigValue { BacodeRule = null };

        /// <summary>
        /// 默认值 
        /// </summary>
        public override ReworkInfoRecordConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// 返工信息配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("返工信息配置值")]
    public class ReworkInfoRecordConfigValue : ConfigValue
    {
        #region 编码规则 BacodeRule
        /// <summary>
        /// 编码规则ID
        /// </summary>
        [Label("编码规则")]
        public static readonly IRefIdProperty NumberRuleIdProperty =
              P<ReworkInfoRecordConfigValue>.RegisterRefId(e => e.NumberRuleId, ReferenceType.Normal);

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
            P<ReworkInfoRecordConfigValue>.RegisterRef(e => e.BacodeRule, NumberRuleIdProperty);

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
