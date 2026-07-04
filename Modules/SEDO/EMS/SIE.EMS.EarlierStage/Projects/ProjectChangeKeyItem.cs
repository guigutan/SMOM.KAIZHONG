using SIE.Domain;
using SIE.EMS.EarlierStage.Budgets;
using SIE.EMS.EarlierStage.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.EarlierStage.Projects
{
    /// <summary>
    /// 项目变更关键事项
    /// </summary>
    [ChildEntity, Serializable]
    [Label("项目变更关键事项")]
    public partial class ProjectChangeKeyItem : DataEntity
    {
        #region 项目变更 ProjectChange
        /// <summary>
        /// 项目变更Id
        /// </summary>
        [Label("项目变更")]
        public static readonly IRefIdProperty ProjectChangeIdProperty =
            P<ProjectChangeKeyItem>.RegisterRefId(e => e.ProjectChangeId, ReferenceType.Parent);

        /// <summary>
        /// 项目变更Id
        /// </summary>
        public double ProjectChangeId
        {
            get { return (double)this.GetRefId(ProjectChangeIdProperty); }
            set { this.SetRefId(ProjectChangeIdProperty, value); }
        }

        /// <summary>
        /// 项目变更
        /// </summary>
        public static readonly RefEntityProperty<ProjectChange> ProjectChangeProperty =
            P<ProjectChangeKeyItem>.RegisterRef(e => e.ProjectChange, ProjectChangeIdProperty);

        /// <summary>
        /// 项目变更
        /// </summary>
        public ProjectChange ProjectChange
        {
            get { return this.GetRefEntity(ProjectChangeProperty); }
            set { this.SetRefEntity(ProjectChangeProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty = P<ProjectChangeKeyItem>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double FactoryId
        {
            get { return (double)GetRefId(FactoryIdProperty); }
            set { SetRefId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<ProjectChangeKeyItem>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return GetRefEntity(FactoryProperty); }
            set { SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 部门 Department
        /// <summary>
        /// 部门Id
        /// </summary>
        [Label("部门")]
        public static readonly IRefIdProperty DepartmentIdProperty = P<ProjectChangeKeyItem>.RegisterRefId(e => e.DepartmentId, ReferenceType.Normal);

        /// <summary>
        /// 部门Id
        /// </summary>
        public double? DepartmentId
        {
            get { return (double?)GetRefNullableId(DepartmentIdProperty); }
            set { SetRefNullableId(DepartmentIdProperty, value); }
        }

        /// <summary>
        /// 部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> DepartmentProperty = P<ProjectChangeKeyItem>.RegisterRef(e => e.Department, DepartmentIdProperty);

        /// <summary>
        /// 部门
        /// </summary>
        public Enterprise Department
        {
            get { return GetRefEntity(DepartmentProperty); }
            set { SetRefEntity(DepartmentProperty, value); }
        }
        #endregion

        #region 事项说明 Description
        /// <summary>
        /// 事项说明
        /// </summary>
        [MaxLength(80)]
        [Required]
        [Label("事项说明")]
        public static readonly Property<string> DescriptionProperty = P<ProjectChangeKeyItem>.Register(e => e.Description);

        /// <summary>
        /// 事项说明
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 预算 Budget
        /// <summary>
        /// 预算Id
        /// </summary>
        public static readonly IRefIdProperty BudgetIdProperty = P<ProjectChangeKeyItem>.RegisterRefId(e => e.BudgetId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Budget> BudgetProperty = P<ProjectChangeKeyItem>.RegisterRef(e => e.Budget, BudgetIdProperty);

        /// <summary>
        /// 预算
        /// </summary>
        public Budget Budget
        {
            get { return GetRefEntity(BudgetProperty); }
            set { SetRefEntity(BudgetProperty, value); }
        }
        #endregion

        #region 事项预算 BudgetAmount
        /// <summary>
        /// 事项预算
        /// </summary>
        [Label("事项预算")]
        [MinValue(0)]
        public static readonly Property<decimal> BudgetAmountProperty = P<ProjectChangeKeyItem>.Register(e => e.BudgetAmount);

        /// <summary>
        /// 事项预算
        /// </summary>
        public decimal BudgetAmount
        {
            get { return GetProperty(BudgetAmountProperty); }
            set { SetProperty(BudgetAmountProperty, value); }
        }
        #endregion

        #region 事项状态 WorkStatus
        /// <summary>
        /// 事项状态
        /// </summary>
        [Label("事项状态")]
        public static readonly Property<WorkStatus> WorkStatusProperty = P<ProjectChangeKeyItem>.Register(e => e.WorkStatus);

        /// <summary>
        /// 事项状态
        /// </summary>
        public WorkStatus WorkStatus
        {
            get { return GetProperty(WorkStatusProperty); }
            set { SetProperty(WorkStatusProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(1000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<ProjectChangeKeyItem>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 事项成本 ActualCost
        /// <summary>
        /// 事项成本
        /// </summary>
        [Label("事项成本")]
        [MinValue(0)]
        public static readonly Property<decimal?> ActualCostProperty = P<ProjectChangeKeyItem>.Register(e => e.ActualCost);

        /// <summary>
        /// 事项成本
        /// </summary>
        public decimal? ActualCost
        {
            get { return this.GetProperty(ActualCostProperty); }
            set { this.SetProperty(ActualCostProperty, value); }
        }
        #endregion

        #region 工时成本 LaborCost
        /// <summary>
        /// 工时成本
        /// </summary>
        [Label("工时成本")]
        [MinValue(0)]
        public static readonly Property<decimal?> LaborCostProperty = P<ProjectChangeKeyItem>.Register(e => e.LaborCost);

        /// <summary>
        /// 工时成本
        /// </summary>
        public decimal? LaborCost
        {
            get { return GetProperty(LaborCostProperty); }
            set { SetProperty(LaborCostProperty, value); }
        }
        #endregion

        #region 原关键事项id ProjectKeyItemId
        /// <summary>
        /// 原关键事项id
        /// </summary>
        [Label("原关键事项id")]
        public static readonly Property<double?> ProjectKeyItemIdProperty = P<ProjectChangeKeyItem>.Register(e => e.ProjectKeyItemId);

        /// <summary>
        /// 原关键事项id
        /// </summary>
        public double? ProjectKeyItemId
        {
            get { return this.GetProperty(ProjectKeyItemIdProperty); }
            set { this.SetProperty(ProjectKeyItemIdProperty, value); }
        }
        #endregion

        #region 项目变更计划 ProjectChangeWorkItem
        /// <summary>
        /// 项目变更计划Id
        /// </summary>
        [Label("项目计划")]
        public static readonly IRefIdProperty ProjectChangeWorkItemIdProperty = P<ProjectChangeKeyItem>.RegisterRefId(e => e.ProjectChangeWorkItemId, ReferenceType.Normal);

        /// <summary>
        /// 项目变更计划Id
        /// </summary>
        public double? ProjectChangeWorkItemId
        {
            get { return (double?)GetRefNullableId(ProjectChangeWorkItemIdProperty); }
            set { SetRefNullableId(ProjectChangeWorkItemIdProperty, value); }
        }

        /// <summary>
        /// 项目变更计划
        /// </summary>
        public static readonly RefEntityProperty<ProjectChangeWorkItem> ProjectChangeWorkItemProperty = P<ProjectChangeKeyItem>.RegisterRef(e => e.ProjectChangeWorkItem, ProjectChangeWorkItemIdProperty);

        /// <summary>
        /// 项目变更计划
        /// </summary>
        public ProjectChangeWorkItem ProjectChangeWorkItem
        {
            get { return GetRefEntity(ProjectChangeWorkItemProperty); }
            set { SetRefEntity(ProjectChangeWorkItemProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 项目变更计划 ProjectChangeWorkItem_Display
        /// <summary>
        /// 项目变更计划
        /// </summary>
        [Label("项目变更计划")]
        public static readonly Property<string> ProjectChangeWorkItem_DisplayProperty = P<ProjectChangeKeyItem>.Register(e => e.ProjectChangeWorkItem_Display);

        /// <summary>
        /// 项目变更计划
        /// </summary>
        public string ProjectChangeWorkItem_Display
        {
            get { return this.GetProperty(ProjectChangeWorkItem_DisplayProperty); }
            set { this.SetProperty(ProjectChangeWorkItem_DisplayProperty, value); }
        }
        #endregion

        #region 预算编码 BudgetNo
        /// <summary>
        /// 预算编码
        /// </summary>
        [Label("预算编码")]
        public static readonly Property<string> BudgetNoProperty = P<ProjectChangeKeyItem>.RegisterView(e => e.BudgetNo, p => p.Budget.BudgetNo);

        /// <summary>
        /// 预算编码
        /// </summary>
        public string BudgetNo
        {
            get { return this.GetProperty(BudgetNoProperty); }
            set { SetProperty(BudgetNoProperty, value); }
        }
        #endregion

        #region 预算名称 BudgetName
        /// <summary>
        /// 预算名称
        /// </summary>
        [Label("预算名称")]
        public static readonly Property<string> BudgetNameProperty = P<ProjectChangeKeyItem>.RegisterView(e => e.BudgetName, p => p.Budget.BudgetName);

        /// <summary>
        /// 预算名称
        /// </summary>
        public string BudgetName
        {
            get { return this.GetProperty(BudgetNameProperty); }
            set { SetProperty(BudgetNameProperty, value); }
        }
        #endregion

        #region 预算金额 BudgetQty
        /// <summary>
        /// 预算金额
        /// </summary>
        [Label("预算金额")]
        public static readonly Property<decimal> BudgetQtyProperty = P<ProjectChangeKeyItem>.RegisterView(e => e.BudgetQty, p => p.Budget.BudgetAmount);

        /// <summary>
        /// 预算金额
        /// </summary>
        public decimal BudgetQty
        {
            get { return this.GetProperty(BudgetQtyProperty); }
            set { SetProperty(BudgetQtyProperty, value); }
        }
        #endregion

        #region 预占金额 ReserveAmount
        /// <summary>
        /// 预占金额
        /// </summary>
        [Label("预占金额")]
        public static readonly Property<decimal> ReserveAmountProperty = P<ProjectChangeKeyItem>.RegisterView(e => e.ReserveAmount, p => p.Budget.ReserveAmount);

        /// <summary>
        /// 预占金额
        /// </summary>
        public decimal ReserveAmount
        {
            get { return this.GetProperty(ReserveAmountProperty); }
            set { SetProperty(ReserveAmountProperty, value); }
        }
        #endregion

        #region 已使用金额 UsedAmount
        /// <summary>
        /// 已使用金额
        /// </summary>
        [Label("已使用金额")]
        public static readonly Property<decimal> UsedAmountProperty = P<ProjectChangeKeyItem>.RegisterView(e => e.UsedAmount, p => p.Budget.UsedAmount);

        /// <summary>
        /// 已使用金额
        /// </summary>
        public decimal UsedAmount
        {
            get { return this.GetProperty(UsedAmountProperty); }
            set { SetProperty(UsedAmountProperty, value); }
        }
        #endregion

        #region 预算可使用金额 CanUseAmount
        /// <summary>
        /// 预算可使用金额
        /// </summary>
        [Label("预算可使用金额")]
        public static readonly Property<decimal?> CanUseAmountProperty = P<ProjectChangeKeyItem>.RegisterReadOnly(
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

        #region 计划类型（界面属性） PlanType
        /// <summary>
        /// 计划类型
        /// </summary>
        [Label("计划类型")]
        public static readonly Property<PlanType> PlanTypeProperty = P<ProjectChangeKeyItem>.Register(e => e.PlanType);

        /// <summary>
        /// 计划类型
        /// </summary>
        public PlanType PlanType
        {
            get { return this.GetProperty(PlanTypeProperty); }
            set { this.SetProperty(PlanTypeProperty, value); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 项目变更关键事项 实体配置
    /// </summary>
    internal class ProjectChangeKeyItemConfig : EntityConfig<ProjectChangeKeyItem>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EARL_PRO_CHA_KEY").MapAllProperties();
            Meta.Property(ProjectChangeKeyItem.PlanTypeProperty).DontMapColumn();
            Meta.Property(ProjectChangeKeyItem.ProjectChangeWorkItem_DisplayProperty).DontMapColumn();
            Meta.Property(ProjectChangeKeyItem.DescriptionProperty).ColumnMeta.HasLength(160);
            Meta.Property(ProjectChangeKeyItem.RemarkProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}
