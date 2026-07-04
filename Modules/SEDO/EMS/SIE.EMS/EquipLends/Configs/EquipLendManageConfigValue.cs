using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipLends.Configs
{
    /// <summary>
    /// 设备借还配置项
    /// </summary>
    [RootEntity, Serializable]
    [Label("设备借还配置项")]
    public class EquipLendManageConfigValue : ConfigValue
    {
        #region 借还单号编码规则 NoRule
        /// <summary>
        /// 借还单号编码规则Id
        /// </summary>
        [Label("借还单号编码规则")]
        public static readonly IRefIdProperty NoRuleIdProperty =
            P<EquipLendManageConfigValue>.RegisterRefId(e => e.NoRuleId, ReferenceType.Normal);

        /// <summary>
        /// 借还单号编码规则Id
        /// </summary>
        public double? NoRuleId
        {
            get { return (double?)this.GetRefNullableId(NoRuleIdProperty); }
            set { this.SetRefNullableId(NoRuleIdProperty, value); }
        }

        /// <summary>
        /// 借还单号编码规则
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> NoRuleProperty =
            P<EquipLendManageConfigValue>.RegisterRef(e => e.NoRule, NoRuleIdProperty);

        /// <summary>
        /// 借还单号编码规则
        /// </summary>
        public NumberRule NoRule
        {
            get { return this.GetRefEntity(NoRuleProperty); }
            set { this.SetRefEntity(NoRuleProperty, value); }
        }
        #endregion

        #region 是否启用借出审核 LendExamine
        /// <summary>
        /// 是否启用借出审核
        /// </summary>
        [Label("是否启用借出审核")]
        public static readonly Property<bool> LendExamineProperty = P<EquipLendManageConfigValue>.Register(e => e.LendExamine);

        /// <summary>
        /// 是否启用借出审核
        /// </summary>
        public bool LendExamine
        {
            get { return this.GetProperty(LendExamineProperty); }
            set { this.SetProperty(LendExamineProperty, value); }
        }
        #endregion

        #region 是否启用归还审核 ReurnExamine
        /// <summary>
        /// 是否启用归还审核
        /// </summary>
        [Label("是否启用归还审核")]
        public static readonly Property<bool> ReturnExamineProperty = P<EquipLendManageConfigValue>.Register(e => e.ReturnExamine);

        /// <summary>
        /// 是否启用归还审核
        /// </summary>
        public bool ReturnExamine
        {
            get { return this.GetProperty(ReturnExamineProperty); }
            set { this.SetProperty(ReturnExamineProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示值
        /// </summary>
        /// <returns></returns>
        public override string Display()
        {
            string ruleDisplay = NoRuleId.HasValue ? NoRule.Name : string.Empty;
            string lendDisplay = LendExamine ? "是".L10N() : "否".L10N();
            string returnDisplay = ReturnExamine ? "是".L10N() : "否".L10N();
            return "借还单号编码规则:{0} 是否启用借出审核:{1}  是否启用归还审核:{2}".L10nFormat(ruleDisplay, lendDisplay, returnDisplay);
        }
    }
}
