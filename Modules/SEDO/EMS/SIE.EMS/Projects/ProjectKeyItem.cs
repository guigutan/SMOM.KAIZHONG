using SIE.Domain;
using SIE.EMS.Budgets;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Projects
{
    /// <summary>
    /// 项目关键事项
    /// </summary>
    [ChildEntity, Serializable]
    [DisplayMember(nameof(Description))]
    [Label("项目关键事项")]
    public partial class ProjectKeyItem : DataEntity
    {
        #region 项目 Project
        /// <summary>
        /// 项目Id
        /// </summary>
        public static readonly IRefIdProperty ProjectIdProperty = P<ProjectKeyItem>.RegisterRefId(e => e.ProjectId, ReferenceType.Parent);

        /// <summary>
        /// 项目Id
        /// </summary>
        public double ProjectId
        {
            get { return (double)GetRefId(ProjectIdProperty); }
            set { SetRefId(ProjectIdProperty, value); }
        }

        /// <summary>
        /// 项目
        /// </summary>
        public static readonly RefEntityProperty<Project> ProjectProperty = P<ProjectKeyItem>.RegisterRef(e => e.Project, ProjectIdProperty);

        /// <summary>
        /// 项目
        /// </summary>
        public Project Project
        {
            get { return GetRefEntity(ProjectProperty); }
            set { SetRefEntity(ProjectProperty, value); }
        }
        #endregion

        #region 事项说明 Description
        /// <summary>
        /// 事项说明
        /// </summary>
        [MaxLength(80)]
        [Required]
        [Label("事项说明")]
        public static readonly Property<string> DescriptionProperty = P<ProjectKeyItem>.Register(e => e.Description);

        /// <summary>
        /// 事项说明
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 事项预算 BudgetAmount
        /// <summary>
        /// 事项预算
        /// </summary>
        [Label("事项预算")]
        [MinValue(0)]
        [Required]
        public static readonly Property<decimal> BudgetAmountProperty = P<ProjectKeyItem>.Register(e => e.BudgetAmount);

        /// <summary>
        /// 事项预算
        /// </summary>
        public decimal BudgetAmount
        {
            get { return GetProperty(BudgetAmountProperty); }
            set { SetProperty(BudgetAmountProperty, value); }
        }
        #endregion

        #region 事项成本 ActualCost
        /// <summary>
        /// 事项成本
        /// </summary>
        [Label("事项成本")]
        [MinValue(0)]
        public static readonly Property<decimal?> ActualCostProperty = P<ProjectKeyItem>.Register(e => e.ActualCost);

        /// <summary>
        /// 事项成本
        /// </summary>
        public decimal? ActualCost
        {
            get { return this.GetProperty(ActualCostProperty); }
            set { this.SetProperty(ActualCostProperty, value); }
        }
        #endregion

        #region 预算 Budget
        /// <summary>
        /// 预算Id
        /// </summary>
        public static readonly IRefIdProperty BudgetIdProperty = P<ProjectKeyItem>.RegisterRefId(e => e.BudgetId, ReferenceType.Normal);

        /// <summary>
        /// 预算Id
        /// </summary>
        public double? BudgetId
        {
            get { return (double?)GetRefNullableId(BudgetIdProperty); }
            set { SetRefNullableId(BudgetIdProperty, value); }
        }

        /// <summary>
        /// 预算
        /// </summary>
        public static readonly RefEntityProperty<Budget> BudgetProperty = P<ProjectKeyItem>.RegisterRef(e => e.Budget, BudgetIdProperty);

        /// <summary>
        /// 预算
        /// </summary>
        public Budget Budget
        {
            get { return GetRefEntity(BudgetProperty); }
            set { SetRefEntity(BudgetProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 预算编码 BudgetNo
        /// <summary>
        /// 预算编码
        /// </summary>
        [Label("预算编码")]
        public static readonly Property<string> BudgetNoProperty = P<ProjectKeyItem>.RegisterView(e => e.BudgetNo, p => p.Budget.BudgetNo);

        /// <summary>
        /// 预算编码
        /// </summary>
        public string BudgetNo
        {
            get { return this.GetProperty(BudgetNoProperty); }
        }
        #endregion

        #region 预算名称 BudgetName
        /// <summary>
        /// 预算名称
        /// </summary>
        [Label("预算名称")]
        public static readonly Property<string> BudgetNameProperty = P<ProjectKeyItem>.RegisterView(e => e.BudgetName, p => p.Budget.BudgetName);

        /// <summary>
        /// 预算名称
        /// </summary>
        public string BudgetName
        {
            get { return this.GetProperty(BudgetNameProperty); }
        }
        #endregion

        #region 预算金额 BudgetQty
        /// <summary>
        /// 预算金额
        /// </summary>
        [Label("预算金额")]
        public static readonly Property<decimal> BudgetQtyProperty = P<ProjectKeyItem>.RegisterView(e => e.BudgetQty, p => p.Budget.BudgetAmount);

        /// <summary>
        /// 预算金额
        /// </summary>
        public decimal BudgetQty
        {
            get { return this.GetProperty(BudgetQtyProperty); }
        }
        #endregion

        #region 预占金额 ReserveAmount
        /// <summary>
        /// 预占金额
        /// </summary>
        [Label("预占金额")]
        public static readonly Property<decimal> ReserveAmountProperty = P<ProjectKeyItem>.RegisterView(e => e.ReserveAmount, p => p.Budget.ReserveAmount);

        /// <summary>
        /// 预占金额
        /// </summary>
        public decimal ReserveAmount
        {
            get { return this.GetProperty(ReserveAmountProperty); }
        }
        #endregion

        #region 已使用金额 UsedAmount
        /// <summary>
        /// 已使用金额
        /// </summary>
        [Label("已使用金额")]
        public static readonly Property<decimal> UsedAmountProperty = P<ProjectKeyItem>.RegisterView(e => e.UsedAmount, p => p.Budget.UsedAmount);

        /// <summary>
        /// 已使用金额
        /// </summary>
        public decimal UsedAmount
        {
            get { return this.GetProperty(UsedAmountProperty); }
        }
        #endregion

        #region 预算可使用金额 CanUseAmount
        /// <summary>
        /// 预算可使用金额
        /// </summary>
        [Label("预算可使用金额")]
        public static readonly Property<decimal?> CanUseAmountProperty = P<ProjectKeyItem>.RegisterReadOnly(
            e => e.CanUseAmount, e => e.GetCanUseAmount(), BudgetAmountProperty);
        /// <summary>
        /// 预算可使用金额
        /// </summary>

        public decimal? CanUseAmount
        {
            get { return this.GetProperty(CanUseAmountProperty); }
        }
        private decimal? GetCanUseAmount()
        {
            if (Budget == null)
            {
                return null;
            }
            return BudgetQty - ReserveAmount - UsedAmount;
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 项目关键事项 实体配置
    /// </summary>
    internal class ProjectKeyItemConfig : EntityConfig<ProjectKeyItem>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EARL_PROJ_KEY").MapAllProperties();
            Meta.Property(ProjectKeyItem.DescriptionProperty).ColumnMeta.HasLength(160);
            Meta.EnablePhantoms();
        }
    }
}
