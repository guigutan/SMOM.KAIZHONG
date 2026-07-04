using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.DataAuth;
using SIE.Domain;
using SIE.Domain.Validation;
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
    /// 预算
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(BudgetCriteria))]
    [Label("预算")]
    [EntityWithConfig(typeof(ApprovalConfig))]
    [DisplayMember(nameof(BudgetNo))]
    [EntityWithConfig(typeof(NoConfig), "预算编号配置项", "预算编号生成规则")]
    [EntityDataAuth(typeof(Resources.Employees.EmployeeEnterprise), nameof(FactoryId), true)]
    [BudgetDepartmentAuth(nameof(DepartmentId), true)]
    public partial class Budget : SIE.EMS.Budgets.Budget
    {
        /// <summary>
        /// 快码类型:投资分类
        /// </summary>
        public static string InvestClassify { get { return "INVEST_CLASSIFY"; } }

        /// <summary>
        /// 快码类型：预算分类
        /// </summary>
        public static string BudgetClassify { get { return "BUDGET_CLASSIFY"; } }

        #region 投资分类 InvestClass
        /// <summary>
        /// 投资分类
        /// </summary>
        [Label("投资分类")]
        [Required]
        public static readonly Property<string> InvestClassProperty = P<Budget>.Register(e => e.InvestClass);

        /// <summary>
        /// 投资分类
        /// </summary>
        public string InvestClass
        {
            get { return GetProperty(InvestClassProperty); }
            set { SetProperty(InvestClassProperty, value); }
        }
        #endregion

        #region 年度 Year
        /// <summary>
        /// 年度
        /// </summary>
        [Label("年度")]
        [Required]
        public static readonly Property<DateTime> YearProperty = P<Budget>.Register(e => e.Year);

        /// <summary>
        /// 年度
        /// </summary>
        public DateTime Year
        {
            get { return GetProperty(YearProperty); }
            set { SetProperty(YearProperty, value); }
        }
        #endregion

        #region 预算内容 BudgetContent
        /// <summary>
        /// 预算内容
        /// </summary>
        [MaxLength(1000)]
        [Label("预算内容")]
        [Required]
        public static readonly Property<string> BudgetContentProperty = P<Budget>.Register(e => e.BudgetContent);

        /// <summary>
        /// 预算内容
        /// </summary>
        public string BudgetContent
        {
            get { return GetProperty(BudgetContentProperty); }
            set { SetProperty(BudgetContentProperty, value); }
        }
        #endregion

        #region 预算分类 BudgetClass
        /// <summary>
        /// 预算分类
        /// </summary>
        [Label("预算分类")]
        [Required]
        public static readonly Property<string> BudgetClassProperty = P<Budget>.Register(e => e.BudgetClass);

        /// <summary>
        /// 预算分类
        /// </summary>
        public string BudgetClass
        {
            get { return GetProperty(BudgetClassProperty); }
            set { SetProperty(BudgetClassProperty, value); }
        }
        #endregion

        #region 标的名称 TargetName
        /// <summary>
        /// 标的名称
        /// </summary>
        [Label("标的名称")]
        public static readonly Property<string> TargetNameProperty = P<Budget>.Register(e => e.TargetName);

        /// <summary>
        /// 标的名称
        /// </summary>
        public string TargetName
        {
            get { return GetProperty(TargetNameProperty); }
            set { SetProperty(TargetNameProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        [Required]
        public static readonly Property<int> QtyProperty = P<Budget>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public int Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<Budget>.Register(e => e.UnitName);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
            set { this.SetProperty(UnitNameProperty, value); }
        }
        #endregion

        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<Budget>.Register(e => e.ApprovalStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus ApprovalStatus
        {
            get { return GetProperty(ApprovalStatusProperty); }
            set { SetProperty(ApprovalStatusProperty, value); }
        }
        #endregion

        #region 币种 Currency
        /// <summary>
        /// 币种
        /// </summary>
        [Label("币种")]
        public static readonly Property<Currency> CurrencyProperty = P<Budget>.Register(e => e.Currency);

        /// <summary>
        /// 币种
        /// </summary>
        public Currency Currency
        {
            get { return GetProperty(CurrencyProperty); }
            set { SetProperty(CurrencyProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty = P<Budget>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<Budget>.RegisterRef(e => e.Factory, FactoryIdProperty);

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
        public static readonly IRefIdProperty DepartmentIdProperty = P<Budget>.RegisterRefId(e => e.DepartmentId, ReferenceType.Normal);

        /// <summary>
        /// 部门Id
        /// </summary>
        public double DepartmentId
        {
            get { return (double)GetRefId(DepartmentIdProperty); }
            set { SetRefId(DepartmentIdProperty, value); }
        }

        /// <summary>
        /// 部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> DepartmentProperty = P<Budget>.RegisterRef(e => e.Department, DepartmentIdProperty);

        /// <summary>
        /// 部门
        /// </summary>
        public Enterprise Department
        {
            get { return GetRefEntity(DepartmentProperty); }
            set { SetRefEntity(DepartmentProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 金额单位 AmountUnit
        /// <summary>
        /// 金额单位
        /// </summary>
        [Label("金额单位")]
        public static readonly Property<AmountUnit> AmountUnitProperty = P<Budget>.RegisterReadOnly(
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

        #region 预算年度(不映射数据库，供导入时使用） BudgetYear
        /// <summary>
        /// 预算年度(不映射数据库，供导入时使用）
        /// </summary>
        [Label("预算年度")]
        public static readonly Property<int> BudgetYearProperty = P<Budget>.Register(e => e.BudgetYear);

        /// <summary>
        /// 预算年度(不映射数据库，供导入时使用）
        /// </summary>
        public int BudgetYear
        {
            get { return this.GetProperty(BudgetYearProperty); }
            set { this.SetProperty(BudgetYearProperty, value); }
        }
        #endregion

        #region 工厂 FactoryName
        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        public static readonly Property<string> FactoryNameProperty = P<Budget>.RegisterView(e => e.FactoryName, p => p.Factory.Name);

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
        public static readonly Property<string> DepartmentNameProperty = P<Budget>.RegisterView(e => e.DepartmentName, p => p.Department.Name);

        /// <summary>
        /// 部门
        /// </summary>
        public string DepartmentName
        {
            get { return this.GetProperty(DepartmentNameProperty); }
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
            Meta.MapTable("EMS_EARL_BUDGET").MapAllPropertiesExcept(Budget.BudgetYearProperty);
            Meta.Property(Budget.BudgetContentProperty).ColumnMeta.HasLength(2000);
            Meta.Property(Budget.PriceBasisProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }

        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="rules">验证规则</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(Budget.QtyProperty, new PositiveNumberRule());
            rules.AddRule(Budget.PriceProperty, new PositiveNumberRule());
            rules.AddRule(Budget.BudgetAmountProperty, new PositiveNumberRule());
        }
    }
}