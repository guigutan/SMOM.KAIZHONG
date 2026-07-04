using SIE.Common.Configs;
using SIE.DataAuth;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Budgets;
using SIE.EMS.DataAuth;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.EarlierStage.Budgets
{
    /// <summary>
    /// 预算变更
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(BudgetChangeCriteria))]
    [Label("预算变更")]
    [EntityWithConfig(typeof(ApprovalConfig))]
    [EntityDataAuth(typeof(Resources.Employees.EmployeeEnterprise), nameof(FactoryId), true)]
    [BudgetDepartmentAuth(nameof(DepartmentId), true)]
    public partial class BudgetChange : DataEntity
    {
        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty = P<BudgetChange>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<BudgetChange>.RegisterRef(e => e.Factory, FactoryIdProperty);

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
        public static readonly IRefIdProperty DepartmentIdProperty = P<BudgetChange>.RegisterRefId(e => e.DepartmentId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> DepartmentProperty = P<BudgetChange>.RegisterRef(e => e.Department, DepartmentIdProperty);

        /// <summary>
        /// 部门
        /// </summary>
        public Enterprise Department
        {
            get { return GetRefEntity(DepartmentProperty); }
            set { SetRefEntity(DepartmentProperty, value); }
        }
        #endregion

        #region 变更内容 ChangeContent
        /// <summary>
        /// 变更内容
        /// </summary>
        [Label("变更内容")]
        public static readonly Property<string> ChangeContentProperty = P<BudgetChange>.Register(e => e.ChangeContent);

        /// <summary>
        /// 变更内容
        /// </summary>
        public string ChangeContent
        {
            get { return this.GetProperty(ChangeContentProperty); }
            set { this.SetProperty(ChangeContentProperty, value); }
        }
        #endregion

        #region 变更前预算 OriginalAmount
        /// <summary>
        /// 变更前预算
        /// </summary>
        [Label("变更前预算")]
        public static readonly Property<decimal> OriginalAmountProperty = P<BudgetChange>.Register(e => e.OriginalAmount);

        /// <summary>
        /// 变更前预算
        /// </summary>
        public decimal OriginalAmount
        {
            get { return GetProperty(OriginalAmountProperty); }
            set { SetProperty(OriginalAmountProperty, value); }
        }
        #endregion

        #region 变更后预算 NewAmount
        /// <summary>
        /// 变更后预算
        /// </summary>
        [Label("变更后预算")]
        public static readonly Property<decimal> NewAmountProperty = P<BudgetChange>.Register(e => e.NewAmount);

        /// <summary>
        /// 变更后预算
        /// </summary>
        public decimal NewAmount
        {
            get { return GetProperty(NewAmountProperty); }
            set { SetProperty(NewAmountProperty, value); }
        }
        #endregion

        #region 变更说明 Description
        /// <summary>
        /// 变更说明
        /// </summary>
        [Label("变更说明")]
        [MaxLength(1000)]
        [Required]
        public static readonly Property<string> DescriptionProperty = P<BudgetChange>.Register(e => e.Description);

        /// <summary>
        /// 变更说明
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
        public static readonly IRefIdProperty BudgetIdProperty = P<BudgetChange>.RegisterRefId(e => e.BudgetId, ReferenceType.Normal);

        /// <summary>
        /// 预算Id
        /// </summary>
        public double BudgetId
        {
            get { return (double)GetRefId(BudgetIdProperty); }
            set { SetRefId(BudgetIdProperty, value); }
        }

        /// <summary>
        /// 预算
        /// </summary>
        public static readonly RefEntityProperty<Budget> BudgetProperty = P<BudgetChange>.RegisterRef(e => e.Budget, BudgetIdProperty);

        /// <summary>
        /// 预算
        /// </summary>
        public Budget Budget
        {
            get { return GetRefEntity(BudgetProperty); }
            set { SetRefEntity(BudgetProperty, value); }
        }
        #endregion

        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<BudgetChange>.Register(e => e.ApprovalStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus ApprovalStatus
        {
            get { return GetProperty(ApprovalStatusProperty); }
            set { SetProperty(ApprovalStatusProperty, value); }
        }
        #endregion

        #region 审核时间 ApprovalTime
        /// <summary>
        /// 审核时间
        /// </summary>
        [Label("审核时间")]
        public static readonly Property<DateTime?> ApprovalTimeProperty = P<BudgetChange>.Register(e => e.ApprovalTime);

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? ApprovalTime
        {
            get { return this.GetProperty(ApprovalTimeProperty); }
            set { this.SetProperty(ApprovalTimeProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 工厂 FactoryName
        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        public static readonly Property<string> FactoryNameProperty = P<BudgetChange>.RegisterView(e => e.FactoryName, p => p.Factory.Name);

        /// <summary>
        /// 工厂
        /// </summary>
        public string FactoryName
        {
            get { return this.GetProperty(FactoryNameProperty); }
        }
        #endregion

        #region 部门 DepartmentName
        /// <summary>
        /// 部门
        /// </summary>
        [Label("部门")]
        public static readonly Property<string> DepartmentNameProperty = P<BudgetChange>.RegisterView(e => e.DepartmentName, p => p.Department.Name);

        /// <summary>
        /// 部门
        /// </summary>
        public string DepartmentName
        {
            get { return this.GetProperty(DepartmentNameProperty); }
        }
        #endregion

        #region 预算编号 BudgetNo
        /// <summary>
        /// 预算编号
        /// </summary>
        [Label("预算编号")]
        public static readonly Property<string> BudgetNoProperty = P<BudgetChange>.RegisterView(e => e.BudgetNo, p => p.Budget.BudgetNo);

        /// <summary>
        /// 预算编号
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
        public static readonly Property<string> BudgetNameProperty = P<BudgetChange>.RegisterView(e => e.BudgetName, p => p.Budget.BudgetName);

        /// <summary>
        /// 预算名称
        /// </summary>
        public string BudgetName
        {
            get { return this.GetProperty(BudgetNameProperty); }
        }
        #endregion

        #region 预算等级 BudgeGrade
        /// <summary>
        /// 预算等级
        /// </summary>
        [Label("预算等级")]
        public static readonly Property<BudgeGrade> BudgeGradeProperty = P<BudgetChange>.RegisterView(e => e.BudgeGrade, p => p.Budget.BudgeGrade);

        /// <summary>
        /// 预算等级
        /// </summary>
        public BudgeGrade BudgeGrade
        {
            get { return this.GetProperty(BudgeGradeProperty); }
        }
        #endregion

        #region 预算内容 BudgetContent
        /// <summary>
        /// 预算内容
        /// </summary>
        [Label("预算内容")]
        public static readonly Property<string> BudgetContentProperty = P<BudgetChange>.RegisterView(e => e.BudgetContent, p => p.Budget.BudgetContent);

        /// <summary>
        /// 预算内容
        /// </summary>
        public string BudgetContent
        {
            get { return this.GetProperty(BudgetContentProperty); }
        }
        #endregion

        #region 预算分类 BudgetClass
        /// <summary>
        /// 预算分类
        /// </summary>
        [Label("预算分类")]
        public static readonly Property<string> BudgetClassProperty = P<BudgetChange>.RegisterView(e => e.BudgetClass, p => p.Budget.BudgetClass);

        /// <summary>
        /// 预算分类
        /// </summary>
        public string BudgetClass
        {
            get { return this.GetProperty(BudgetClassProperty); }
        }
        #endregion

        #region 预算金额 BudgetAmount
        /// <summary>
        /// 预算金额
        /// </summary>
        [Label("预算金额")]
        public static readonly Property<decimal> BudgetAmountProperty = P<BudgetChange>.RegisterView(e => e.BudgetAmount, p => p.Budget.BudgetAmount);

        /// <summary>
        /// 预算金额
        /// </summary>
        public decimal BudgetAmount
        {
            get { return this.GetProperty(BudgetAmountProperty); }
        }
        #endregion

        #region 预占金额 ReserveAmount
        /// <summary>
        /// 预占金额
        /// </summary>
        [Label("预占金额")]
        public static readonly Property<decimal> ReserveAmountProperty = P<BudgetChange>.RegisterView(e => e.ReserveAmount, p => p.Budget.ReserveAmount);

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
        public static readonly Property<decimal> UsedAmountProperty = P<BudgetChange>.RegisterView(e => e.UsedAmount, p => p.Budget.UsedAmount);

        /// <summary>
        /// 已使用金额
        /// </summary>
        public decimal UsedAmount
        {
            get { return this.GetProperty(UsedAmountProperty); }
        }
        #endregion

        #region 可使用金额 CanUseAmount
        /// <summary>
        /// 可使用金额
        /// </summary>
        [Label("可使用金额")]
        public static readonly Property<decimal> CanUseAmountProperty = P<BudgetChange>.RegisterReadOnly(
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

        #region 年度 Year
        /// <summary>
        /// 年度
        /// </summary>
        [Label("年度")]
        public static readonly Property<DateTime?> YearProperty = P<BudgetChange>.RegisterView(e => e.Year, p => p.Budget.Year);

        /// <summary>
        /// 年度
        /// </summary>
        public DateTime? Year
        {
            get { return this.GetProperty(YearProperty); }
        }
        #endregion

        #region 投资分类 InvestClass
        /// <summary>
        /// 投资分类
        /// </summary>
        [Label("投资分类")]
        public static readonly Property<string> InvestClassProperty = P<BudgetChange>.RegisterView(e => e.InvestClass, p => p.Budget.InvestClass);

        /// <summary>
        /// 投资分类
        /// </summary>
        public string InvestClass
        {
            get { return this.GetProperty(InvestClassProperty); }
        }
        #endregion

        #region 编制人 BudgetCreateByName
        /// <summary>
        /// 编制人
        /// </summary>
        [Label("编制人")]
        public static readonly Property<string> BudgetCreateByNameProperty = P<BudgetChange>.RegisterView(e => e.BudgetCreateByName, "Budget.CreateUser.Name");

        /// <summary>
        /// 编制人
        /// </summary>
        public string BudgetCreateByName
        {
            get { return this.GetProperty(BudgetCreateByNameProperty); }
        }
        #endregion

        #region 编制时间 BudgetCreateDate
        /// <summary>
        /// 编制时间
        /// </summary>
        [Label("编制时间")]
        public static readonly Property<DateTime?> BudgetCreateDateProperty = P<BudgetChange>.RegisterView(e => e.BudgetCreateDate, p => p.Budget.CreateDate);

        /// <summary>
        /// 编制时间
        /// </summary>
        public DateTime? BudgetCreateDate
        {
            get { return this.GetProperty(BudgetCreateDateProperty); }
        }
        #endregion

        #region 币种 Currency
        /// <summary>
        /// 币种
        /// </summary>
        [Label("币种")]
        public static readonly Property<Currency?> CurrencyProperty = P<BudgetChange>.RegisterView(e => e.Currency, p => p.Budget.Currency);

        /// <summary>
        /// 币种
        /// </summary>
        public Currency? Currency
        {
            get { return this.GetProperty(CurrencyProperty); }
        }
        #endregion

        #region 金额单位 AmountUnit
        /// <summary>
        /// 金额单位
        /// </summary>
        [Label("金额单位")]
        public static readonly Property<AmountUnit?> AmountUnitProperty = P<BudgetChange>.RegisterReadOnly(
            e => e.AmountUnit, e => e.GetAmountUnit(), CurrencyProperty);
        /// <summary>
        /// 金额单位
        /// </summary>
        public AmountUnit? AmountUnit
        {
            get { return this.GetProperty(AmountUnitProperty); }
        }
        private AmountUnit? GetAmountUnit()
        {
            return (AmountUnit?)Currency;
        }
        #endregion

        #region 标的名称 TargetName
        /// <summary>
        /// 标的名称
        /// </summary>
        [Label("标的名称")]
        public static readonly Property<string> TargetNameProperty = P<BudgetChange>.RegisterView(e => e.TargetName, p => p.Budget.TargetName);

        /// <summary>
        /// 标的名称
        /// </summary>
        public string TargetName
        {
            get { return this.GetProperty(TargetNameProperty); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<int?> QtyProperty = P<BudgetChange>.RegisterView(e => e.Qty, p => p.Budget.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public int? Qty
        {
            get { return this.GetProperty(QtyProperty); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<BudgetChange>.RegisterView(e => e.UnitName, p => p.Budget.UnitName);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
        }
        #endregion

        #region 预估单价 Price
        /// <summary>
        /// 预估单价
        /// </summary>
        [Label("预估单价")]
        public static readonly Property<decimal?> PriceProperty = P<BudgetChange>.RegisterView(e => e.Price, p => p.Budget.Price);

        /// <summary>
        /// 预估单价
        /// </summary>
        public decimal? Price
        {
            get { return this.GetProperty(PriceProperty); }
        }
        #endregion

        #region 单价依据 PriceBasis
        /// <summary>
        /// 单价依据
        /// </summary>
        [Label("单价依据")]
        public static readonly Property<string> PriceBasisProperty = P<BudgetChange>.RegisterView(e => e.PriceBasis, p => p.Budget.PriceBasis);

        /// <summary>
        /// 单价依据
        /// </summary>
        public string PriceBasis
        {
            get { return this.GetProperty(PriceBasisProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 预算变更 实体配置
    /// </summary>
    internal class BudgetChangeConfig : EntityConfig<BudgetChange>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EARL_BUDGET_CHG").MapAllProperties();
            Meta.Property(BudgetChange.DescriptionProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }

        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="rules">验证规则</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(BudgetChange.NewAmountProperty, new PositiveNumberRule());
        }
    }
}