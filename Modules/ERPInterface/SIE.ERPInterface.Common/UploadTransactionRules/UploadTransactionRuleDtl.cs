using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Inventory.Transactions;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ERPInterface.Common.UploadTransactionRules
{
    /// <summary>
    /// 交易上传规则明细
    /// </summary>
    [RootEntity, Serializable]
    public class UploadTransactionRuleDtl : DataEntity
    {
        #region 单据大类 OrderType
        /// <summary>
        /// 单据大类
        /// </summary>
        [Label("单据大类")]
        public static readonly Property<OrderType> OrderTypeProperty = P<UploadTransactionRuleDtl>.Register(e => e.OrderType);

        /// <summary>
        /// 单据大类
        /// </summary>
        public OrderType OrderType
        {
            get { return this.GetProperty(OrderTypeProperty); }
            set { this.SetProperty(OrderTypeProperty, value); }
        }
        #endregion

        #region 交易类型 TransactionType
        /// <summary>
        /// 交易类型
        /// </summary>
        [Label("交易类型")]
        public static readonly Property<TransactionType> TransactionTypeProperty = P<UploadTransactionRuleDtl>.Register(e => e.TransactionType);

        /// <summary>
        /// 交易类型
        /// </summary>
        public TransactionType TransactionType
        {
            get { return this.GetProperty(TransactionTypeProperty); }
            set { this.SetProperty(TransactionTypeProperty, value); }
        }
        #endregion

        #region 单据小类 Transaction
        /// <summary>
        /// 单据小类
        /// </summary>
        [Label("单据小类")]
        public static readonly IRefIdProperty TransactionIdProperty =
            P<UploadTransactionRuleDtl>.RegisterRefId(e => e.TransactionId, ReferenceType.Normal);

        /// <summary>
        /// 单据小类
        /// </summary>
        public double? TransactionId
        {
            get { return (double)this.GetRefId(TransactionIdProperty); }
            set { this.SetRefId(TransactionIdProperty, value); }
        }

        /// <summary>
        /// 单据小类
        /// </summary>
        public static readonly RefEntityProperty<Transaction> TransactionProperty =
            P<UploadTransactionRuleDtl>.RegisterRef(e => e.Transaction, TransactionIdProperty);

        /// <summary>
        /// 单据小类
        /// </summary>
        public Transaction Transaction
        {
            get { return this.GetRefEntity(TransactionProperty); }
            set { this.SetRefEntity(TransactionProperty, value); }
        }
        #endregion

        #region 交易上传规则 UploadTransactionRule
        /// <summary>
        /// 交易上传规则Id
        /// </summary>
        [Label("交易上传规则")]
        public static readonly IRefIdProperty UploadTransactionRuleIdProperty =
            P<UploadTransactionRuleDtl>.RegisterRefId(e => e.UploadTransactionRuleId, ReferenceType.Parent);

        /// <summary>
        /// 交易上传规则Id
        /// </summary>
        public double UploadTransactionRuleId
        {
            get { return (double)this.GetRefId(UploadTransactionRuleIdProperty); }
            set { this.SetRefId(UploadTransactionRuleIdProperty, value); }
        }

        /// <summary>
        /// 交易上传规则
        /// </summary>
        public static readonly RefEntityProperty<UploadTransactionRule> UploadTransactionRuleProperty =
            P<UploadTransactionRuleDtl>.RegisterRef(e => e.UploadTransactionRule, UploadTransactionRuleIdProperty);

        /// <summary>
        /// 交易上传规则
        /// </summary>
        public UploadTransactionRule UploadTransactionRule
        {
            get { return this.GetRefEntity(UploadTransactionRuleProperty); }
            set { this.SetRefEntity(UploadTransactionRuleProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 交易上传规则明细 实体配置
    /// </summary>
    internal class UploadTransactionRuleDtlConfig : EntityConfig<UploadTransactionRuleDtl>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(new NotDuplicateRule()
            {
                Properties = { UploadTransactionRuleDtl.OrderTypeProperty, UploadTransactionRuleDtl.TransactionTypeProperty },
                MessageBuilder = o =>
                {
                    var entity = o as UploadTransactionRuleDtl;
                    return "已经存在单据大类为[{0}]，交易类型为[{1}]的交易上传规则。".L10nFormat(entity.OrderType, entity.TransactionType);
                }
            });
        }

        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("UL_TRANS_RULE_DTL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
