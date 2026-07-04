using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Inventory.Commom;
using SIE.Inventory.Transactions;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.ERPInterface.Common.UploadTransactionRules
{
    /// <summary>
    /// 交易上传规则
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    public class UploadTransactionRule : DataEntity
    {
        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<UploadTransactionRule>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion

        #region 启用时间 ActivationDate
        /// <summary>
        /// 启用时间
        /// </summary>
        [Label("启用时间")]
        public static readonly Property<DateTime?> ActivationDateProperty = P<UploadTransactionRule>.Register(e => e.ActivationDate);

        /// <summary>
        /// 启用时间
        /// </summary>
        public DateTime? ActivationDate
        {
            get { return this.GetProperty(ActivationDateProperty); }
            set { this.SetProperty(ActivationDateProperty, value); }
        }
        #endregion

        #region 交易上传规则明细 UploadTransactionRuleDtlList
        /// <summary>
        /// 交易上传规则明细
        /// </summary>
        [Label("交易上传规则明细")]
        public static readonly ListProperty<EntityList<UploadTransactionRuleDtl>> UploadTransactionRuleDtlListProperty = P<UploadTransactionRule>.RegisterList(e => e.UploadTransactionRuleDtlList);

        /// <summary>
        /// 交易上传规则明细
        /// </summary>
        public EntityList<UploadTransactionRuleDtl> UploadTransactionRuleDtlList
        {
            get { return this.GetLazyList(UploadTransactionRuleDtlListProperty); }
        }
        #endregion

        #region 排除仓库 UploadTransctionExclWhList
        /// <summary>
        /// 排除仓库
        /// </summary>
        [Label("排除仓库")]
        public static readonly ListProperty<EntityList<UploadTransactionExclWh>> UploadTransctionExclWhListProperty = P<UploadTransactionRule>.RegisterList(e => e.UploadTransctionExclWhList);

        /// <summary>
        /// 排除仓库
        /// </summary>
        public EntityList<UploadTransactionExclWh> UploadTransctionExclWhList
        {
            get { return this.GetLazyList(UploadTransctionExclWhListProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 交易上传规则 实体配置
    /// </summary>
    internal class UploadTransactionRuleConfig : EntityConfig<UploadTransactionRule>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(UploadTransactionRule.NameProperty, new StringLengthRangeRule() { Max = 80 });
            rules.AddRule(UploadTransactionRule.NameProperty, new RequiredRule());
            rules.AddRule(UploadTransactionRule.NameProperty, new NotDuplicateRule());
        }

        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("UL_TRANS_RULE").MapAllProperties();
            Meta.Property(UploadTransactionRule.NameProperty).ColumnMeta.HasLength("80");
            Meta.EnablePhantoms();
        }
    }
}
