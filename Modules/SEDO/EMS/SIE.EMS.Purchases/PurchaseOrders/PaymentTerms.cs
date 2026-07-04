using SIE.Domain;
using SIE.EMS.Purchases.Enums;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Purchases.PurchaseOrders
{
    /// <summary>
    /// 付款条件
    /// </summary>
    [ChildEntity, Serializable]
    [Label("付款条件")]
    [DisplayMember(nameof(Phase))]
    public partial class PaymentTerms : DataEntity
    {
        /// <summary>
        /// 快码类型：付款阶段
        /// </summary>
        public static string PhaseCatalog { get { return "PHASE_CATALOG"; } }

        /// <summary>
        /// 快码类型：付款方式
        /// </summary>
        public static string PaymentMethodCatalog { get { return "PAYMENT_METHOD_CATALOG"; } }

        #region 付款阶段 Phase
        /// <summary>
        /// 付款阶段
        /// </summary>
        [Label("付款阶段")]
        [Required]
        public static readonly Property<string> PhaseProperty = P<PaymentTerms>.Register(e => e.Phase);

        /// <summary>
        /// 付款阶段
        /// </summary>
        public string Phase
        {
            get { return GetProperty(PhaseProperty); }
            set { SetProperty(PhaseProperty, value); }
        }
        #endregion

        #region 付款比例(%) Percent
        /// <summary>
        /// 付款比例(%)
        /// </summary>
        [Label("付款比例(%)")]
        [MinValue(0.01)]
        [MaxValue(100)]
        public static readonly Property<decimal> PercentProperty = P<PaymentTerms>.Register(e => e.Percent);

        /// <summary>
        /// 付款比例(%)
        /// </summary>
        public decimal Percent
        {
            get { return GetProperty(PercentProperty); }
            set { SetProperty(PercentProperty, value); }
        }
        #endregion

        #region 付款金额 Amount
        /// <summary>
        /// 付款金额
        /// </summary>
        [Label("付款金额")]
        [MinValue(0)]
        public static readonly Property<decimal> AmountProperty = P<PaymentTerms>.Register(e => e.Amount);

        /// <summary>
        /// 付款金额
        /// </summary>
        public decimal Amount
        {
            get { return GetProperty(AmountProperty); }
            set { SetProperty(AmountProperty, value); }
        }
        #endregion

        #region 付款条件 Condition
        /// <summary>
        /// 付款条件
        /// </summary>
        [Label("付款条件")]
        public static readonly Property<PaymentCondition> ConditionProperty = P<PaymentTerms>.Register(e => e.Condition);

        /// <summary>
        /// 付款条件
        /// </summary>
        public PaymentCondition Condition
        {
            get { return GetProperty(ConditionProperty); }
            set { SetProperty(ConditionProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<PaymentTermsState> StateProperty = P<PaymentTerms>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public PaymentTermsState State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 付款方式 PaymentMethod
        /// <summary>
        /// 付款方式
        /// </summary>
        [Label("付款方式")]
        [Required]
        public static readonly Property<string> PaymentMethodProperty = P<PaymentTerms>.Register(e => e.PaymentMethod);

        /// <summary>
        /// 付款方式
        /// </summary>
        public string PaymentMethod
        {
            get { return GetProperty(PaymentMethodProperty); }
            set { SetProperty(PaymentMethodProperty, value); }
        }
        #endregion

        #region 付款日期 PaymentDate
        /// <summary>
        /// 付款日期
        /// </summary>
        [Label("付款日期")]
        [Required]
        public static readonly Property<DateTime?> PaymentDateProperty = P<PaymentTerms>.Register(e => e.PaymentDate);

        /// <summary>
        /// 付款日期
        /// </summary>
        public DateTime? PaymentDate
        {
            get { return GetProperty(PaymentDateProperty); }
            set { SetProperty(PaymentDateProperty, value); }
        }
        #endregion

        #region 采购订单 PurchaseOrder
        /// <summary>
        /// 采购订单Id
        /// </summary>
        public static readonly IRefIdProperty PurchaseOrderIdProperty = P<PaymentTerms>.RegisterRefId(e => e.PurchaseOrderId, ReferenceType.Parent);

        /// <summary>
        /// 采购订单Id
        /// </summary>
        public double PurchaseOrderId
        {
            get { return (double)GetRefId(PurchaseOrderIdProperty); }
            set { SetRefId(PurchaseOrderIdProperty, value); }
        }

        /// <summary>
        /// 采购订单
        /// </summary>
        public static readonly RefEntityProperty<PurchaseOrder> PurchaseOrderProperty = P<PaymentTerms>.RegisterRef(e => e.PurchaseOrder, PurchaseOrderIdProperty);

        /// <summary>
        /// 采购订单
        /// </summary>
        public PurchaseOrder PurchaseOrder
        {
            get { return GetRefEntity(PurchaseOrderProperty); }
            set { SetRefEntity(PurchaseOrderProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 累计金额(界面属性) CumulativeAmount
        /// <summary>
        /// 累计金额
        /// </summary>
        [Label("累计金额")]
        public static readonly Property<decimal> CumulativeAmountProperty = P<PaymentTerms>.Register(e => e.CumulativeAmount);

        /// <summary>
        /// 累计金额
        /// </summary>
        public decimal CumulativeAmount
        {
            get { return this.GetProperty(CumulativeAmountProperty); }
            set { this.SetProperty(CumulativeAmountProperty, value); }
        }
        #endregion

        #region 累计比例(%) CumulativePercent
        /// <summary>
        /// 累计比例(%)
        /// </summary>
        [Label("累计比例(%)")]
        public static readonly Property<decimal> CumulativePercentProperty = P<PaymentTerms>.Register(e => e.CumulativePercent);

        /// <summary>
        /// 累计比例(%)
        /// </summary>
        public decimal CumulativePercent
        {
            get { return this.GetProperty(CumulativePercentProperty); }
            set { this.SetProperty(CumulativePercentProperty, value); }
        }
        #endregion

        #region 币种 Currency
        /// <summary>
        /// 币种
        /// </summary>
        [Label("币种")]
        public static readonly Property<Currency> CurrencyProperty = P<PaymentTerms>.RegisterView(e => e.Currency, p => p.PurchaseOrder.Currency);

        /// <summary>
        /// 币种
        /// </summary>
        public Currency Currency
        {
            get { return this.GetProperty(CurrencyProperty); }
        }
        #endregion

        #region 金额单位 AmountUnit
        /// <summary>
        /// 金额单位
        /// </summary>
        [Label("金额单位")]
        public static readonly Property<AmountUnit> AmountUnitProperty = P<PaymentTerms>.RegisterReadOnly(
            e => e.AmountUnit, e => e.GetAmountUnit(), CurrencyProperty);
        /// <summary>
        /// 金额单位
        /// </summary>
        public AmountUnit AmountUnit
        {
            get { return this.GetProperty(AmountUnitProperty); }
        }
        private AmountUnit GetAmountUnit()
        {
            return (AmountUnit)Currency;
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 付款条件 实体配置
    /// </summary>
    internal class PaymentTermsConfig : EntityConfig<PaymentTerms>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_PO_PAYMENT").MapAllProperties();
            Meta.Property(PaymentTerms.CumulativeAmountProperty).DontMapColumn();
            Meta.Property(PaymentTerms.CumulativePercentProperty).DontMapColumn();
            Meta.EnableSort();
            Meta.EnablePhantoms();
        }
    }
}