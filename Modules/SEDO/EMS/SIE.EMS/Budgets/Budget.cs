using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Budgets
{
    /// <summary>
    /// 预算
    /// </summary>
    [RootEntity, Serializable]
    [Label("预算")]
    [DisplayMember(nameof(BudgetNo))]
    public partial class Budget: DataEntity
    {
        #region 预算编号 BudgetNo
        /// <summary>
        /// 预算编号
        /// </summary>
        [Label("预算编号")]
        [Required]
        [NotDuplicate]
        public static readonly Property<string> BudgetNoProperty = P<Budget>.Register(e => e.BudgetNo);

        /// <summary>
        /// 预算编号
        /// </summary>
        public string BudgetNo
        {
            get { return GetProperty(BudgetNoProperty); }
            set { SetProperty(BudgetNoProperty, value); }
        }
        #endregion

        #region 预算名称 BudgetName
        /// <summary>
        /// 预算名称
        /// </summary>
        [Label("预算名称")]
        [Required]
        [NotDuplicate]
        public static readonly Property<string> BudgetNameProperty = P<Budget>.Register(e => e.BudgetName);

        /// <summary>
        /// 预算名称
        /// </summary>
        public string BudgetName
        {
            get { return GetProperty(BudgetNameProperty); }
            set { SetProperty(BudgetNameProperty, value); }
        }
        #endregion

        #region 预算单价 Price
        /// <summary>
        /// 预算单价
        /// </summary>
        [Label("预算单价")]
        [Required]
        public static readonly Property<decimal> PriceProperty = P<Budget>.Register(e => e.Price);

        /// <summary>
        /// 预算单价
        /// </summary>
        public decimal Price
        {
            get { return GetProperty(PriceProperty); }
            set { SetProperty(PriceProperty, value); }
        }
        #endregion

        #region 单价依据 PriceBasis
        /// <summary>
        /// 单价依据
        /// </summary>
        [MaxLength(1000)]
        [Label("单价依据")]
        public static readonly Property<string> PriceBasisProperty = P<Budget>.Register(e => e.PriceBasis);

        /// <summary>
        /// 单价依据
        /// </summary>
        public string PriceBasis
        {
            get { return GetProperty(PriceBasisProperty); }
            set { SetProperty(PriceBasisProperty, value); }
        }
        #endregion

        #region 预算金额 BudgetAmount
        /// <summary>
        /// 预算金额
        /// </summary>
        [Label("预算金额")]
        [Required]
        public static readonly Property<decimal> BudgetAmountProperty = P<Budget>.Register(e => e.BudgetAmount);

        /// <summary>
        /// 预算金额
        /// </summary>
        public decimal BudgetAmount
        {
            get { return GetProperty(BudgetAmountProperty); }
            set { SetProperty(BudgetAmountProperty, value); }
        }
        #endregion

        #region 预占金额 ReserveAmount
        /// <summary>
        /// 预占金额
        /// </summary>
        [Label("预占金额")]
        [MinValue(0)]
        public static readonly Property<decimal> ReserveAmountProperty = P<Budget>.Register(e => e.ReserveAmount);

        /// <summary>
        /// 预占金额
        /// </summary>
        public decimal ReserveAmount
        {
            get { return GetProperty(ReserveAmountProperty); }
            set { SetProperty(ReserveAmountProperty, value); }
        }
        #endregion

        #region 已使用金额 UsedAmount
        /// <summary>
        /// 已使用金额
        /// </summary>
        [Label("已使用金额")]
        [MinValue(0)]
        public static readonly Property<decimal> UsedAmountProperty = P<Budget>.Register(e => e.UsedAmount);

        /// <summary>
        /// 已使用金额
        /// </summary>
        public decimal UsedAmount
        {
            get { return GetProperty(UsedAmountProperty); }
            set { SetProperty(UsedAmountProperty, value); }
        }
        #endregion

        #region 预算等级 BudgeGrade
        /// <summary>
        /// 预算等级
        /// </summary>
        [Label("预算等级")]
        public static readonly Property<BudgeGrade> BudgeGradeProperty = P<Budget>.Register(e => e.BudgeGrade);

        /// <summary>
        /// 预算等级
        /// </summary>
        public BudgeGrade BudgeGrade
        {
            get { return GetProperty(BudgeGradeProperty); }
            set { SetProperty(BudgeGradeProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 可使用金额 CanUseAmount
        /// <summary>
        /// 可使用金额
        /// </summary>
        [Label("可使用金额")]
        public static readonly Property<decimal> CanUseAmountProperty = P<Budget>.RegisterReadOnly(
            e => e.CanUseAmount, e => e.GetCanUseAmount(), BudgetAmountProperty);
        /// <summary>
        /// 可使用金额
        /// </summary>

        public decimal CanUseAmount
        {
            get { return this.GetProperty(CanUseAmountProperty); }
        }
        private decimal GetCanUseAmount()
        {
            return BudgetAmount - ReserveAmount - UsedAmount;
        }
        #endregion

        #endregion
    }
    /// <summary>
    /// 预算 实体配置
    /// </summary>
    internal class BudgetConfig : EntityConfig<Budget>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EARL_BUDGET").MapAllProperties();
            Meta.Property(Budget.PriceBasisProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}
