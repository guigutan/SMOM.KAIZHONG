using SIE.Common;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Inventory.Transactions
{
    /// <summary>
    /// 事务与功能关系
    /// </summary>
    [RootEntity, Serializable]
    [Label("大类与小类关系")]
    public partial class FunctionTransaction : DataEntity
    {
        #region 单据大类 Function
        /// <summary>
        /// 单据大类
        /// </summary>
        [Label("单据大类")]
        public static readonly IRefIdProperty FunctionIdProperty = P<FunctionTransaction>.RegisterRefId(e => e.FunctionId, ReferenceType.Normal);

        /// <summary>
        /// 单据大类
        /// </summary>
        public double FunctionId
        {
            get { return (double)GetRefId(FunctionIdProperty); }
            set { SetRefId(FunctionIdProperty, value); }
        }

        /// <summary>
        /// 单据大类
        /// </summary>
        public static readonly RefEntityProperty<Function> FunctionProperty = P<FunctionTransaction>.RegisterRef(e => e.Function, FunctionIdProperty);

        /// <summary>
        /// 单据大类
        /// </summary>
        public Function Function
        {
            get { return GetRefEntity(FunctionProperty); }
            set { SetRefEntity(FunctionProperty, value); }
        }
        #endregion

        #region 单据小类 Transaction
        /// <summary>
        /// 单据小类
        /// </summary>
        [Label("单据小类")]
        public static readonly IRefIdProperty TransactionIdProperty = P<FunctionTransaction>.RegisterRefId(e => e.TransactionId, ReferenceType.Normal);

        /// <summary>
        /// 单据小类
        /// </summary>
        public double TransactionId
        {
            get { return (double)GetRefId(TransactionIdProperty); }
            set { SetRefId(TransactionIdProperty, value); }
        }

        /// <summary>
        /// 单据小类
        /// </summary>
        public static readonly RefEntityProperty<Transaction> TransactionProperty = P<FunctionTransaction>.RegisterRef(e => e.Transaction, TransactionIdProperty);

        /// <summary>
        /// 单据小类
        /// </summary>
        public Transaction Transaction
        {
            get { return GetRefEntity(TransactionProperty); }
            set { SetRefEntity(TransactionProperty, value); }
        }
        #endregion

        #region 单据小类编码 TransactionCode
        /// <summary>
        /// 单据小类编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> TransactionCodeProperty = P<FunctionTransaction>.RegisterView(e => e.TransactionCode, p => p.Transaction.Code);

        /// <summary>
        /// 单据小类编码
        /// </summary>
        public string TransactionCode
        {
            get { return this.GetProperty(TransactionCodeProperty); }
        }
        #endregion

        #region 单据小类名称 TransactionName
        /// <summary>
        /// 单据小类名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> TransactionNameProperty = P<FunctionTransaction>.RegisterView(e => e.TransactionName, p => p.Transaction.Name);

        /// <summary>
        /// 单据小类名称
        /// </summary>
        public string TransactionName
        {
            get { return this.GetProperty(TransactionNameProperty); }
        }
        #endregion

        #region 是否内部使用 TransactionIsInternalUse
        /// <summary>
        /// 是否内部使用
        /// </summary>
        [Label("内部使用")]
        public static readonly Property<bool> TransactionIsInternalUseProperty = P<FunctionTransaction>.RegisterView(e => e.TransactionIsInternalUse, p => p.Transaction.IsInternalUse);

        /// <summary>
        /// 是否内部使用
        /// </summary>
        public bool TransactionIsInternalUse
        {
            get { return this.GetProperty(TransactionIsInternalUseProperty); }
        }
        #endregion

        #region 单据小类状态 TransactionState
        /// <summary>
        /// 单据小类状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> TransactionStateProperty = P<FunctionTransaction>.RegisterView(e => e.TransactionState, p => p.Transaction.State);

        /// <summary>
        /// 单据小类状态
        /// </summary>
        public State TransactionState
        {
            get { return this.GetProperty(TransactionStateProperty); }
        }
        #endregion

        #region 单据小类来源 TransactionSourceType
        /// <summary>
        /// 单据小类来源
        /// </summary>
        [Label("来源")]
        public static readonly Property<SourceType> TransactionSourceTypeProperty = P<FunctionTransaction>.RegisterView(e => e.TransactionSourceType, p => p.Transaction.SourceType);

        /// <summary>
        /// 单据小类来源
        /// </summary>
        public SourceType TransactionSourceType
        {
            get { return this.GetProperty(TransactionSourceTypeProperty); }
        }
        #endregion

    }

    /// <summary>
    /// 实体配置
    /// </summary>
    internal class FunctionTransactionConfig : EntityConfig<FunctionTransaction>
    {
        /// <summary>
        /// 属性元数据配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("TRANS_FUNC_TRANS").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}