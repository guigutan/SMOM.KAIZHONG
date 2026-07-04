using SIE.Domain;
using SIE.EMS.Budgets;
using SIE.Equipments.Enums;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.EarlierStage.Budgets
{
    /// <summary>
    /// 预算查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("预算查询实体")]
    public class BudgetCriteria : Criteria
    {
        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty = P<BudgetCriteria>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double? FactoryId
        {
            get { return (double?)GetRefNullableId(FactoryIdProperty); }
            set { SetRefNullableId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<BudgetCriteria>.RegisterRef(e => e.Factory, FactoryIdProperty);

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
        public static readonly IRefIdProperty DepartmentIdProperty = P<BudgetCriteria>.RegisterRefId(e => e.DepartmentId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> DepartmentProperty = P<BudgetCriteria>.RegisterRef(e => e.Department, DepartmentIdProperty);

        /// <summary>
        /// 部门
        /// </summary>
        public Enterprise Department
        {
            get { return GetRefEntity(DepartmentProperty); }
            set { SetRefEntity(DepartmentProperty, value); }
        }
        #endregion

        #region 预算编号 BudgetNo
        /// <summary>
        /// 预算编号
        /// </summary>
        [Label("预算编号")]
        public static readonly Property<string> BudgetNoProperty = P<BudgetCriteria>.Register(e => e.BudgetNo);

        /// <summary>
        /// 预算编号
        /// </summary>
        public string BudgetNo
        {
            get { return GetProperty(BudgetNoProperty); }
            set { SetProperty(BudgetNoProperty, value); }
        }
        #endregion

        #region 预算等级 BudgeGrade
        /// <summary>
        /// 预算等级
        /// </summary>
        [Label("预算等级")]
        public static readonly Property<BudgeGrade?> BudgeGradeProperty = P<BudgetCriteria>.Register(e => e.BudgeGrade);

        /// <summary>
        /// 预算等级
        /// </summary>
        public BudgeGrade? BudgeGrade
        {
            get { return GetProperty(BudgeGradeProperty); }
            set { SetProperty(BudgeGradeProperty, value); }
        }
        #endregion

        #region 投资分类 InvestClass
        /// <summary>
        /// 投资分类
        /// </summary>
        [Label("投资分类")]
        public static readonly Property<string> InvestClassProperty = P<BudgetCriteria>.Register(e => e.InvestClass);

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
        public static readonly Property<DateTime?> YearProperty = P<BudgetCriteria>.Register(e => e.Year);

        /// <summary>
        /// 年度
        /// </summary>
        public DateTime? Year
        {
            get { return GetProperty(YearProperty); }
            set { SetProperty(YearProperty, value); }
        }
        #endregion

        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<ApprovalStatus?> ApprovalStatusProperty = P<BudgetCriteria>.Register(e => e.ApprovalStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus? ApprovalStatus
        {
            get { return GetProperty(ApprovalStatusProperty); }
            set { SetProperty(ApprovalStatusProperty, value); }
        }
        #endregion

        #region 创建日期 CreateDate
        /// <summary>
        /// 创建日期
        /// </summary>
        [Label("创建日期")]
        public static readonly Property<DateRange> CreateDateProperty = P<BudgetCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateRange CreateDate
        {
            get { return GetProperty(CreateDateProperty); }
            set { SetProperty(CreateDateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询逻辑
        /// </summary>
        /// <returns>返回查询后的数据</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<BudgetController>().CriteriaBudgets(this);
        }
    }
}
