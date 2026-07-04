using SIE.Domain;
using SIE.EMS.Projects.Enums;
using SIE.Equipments.Enums;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.EarlierStage.Projects
{
    /// <summary>
    /// 项目管理查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("项目管理查询实体")]
    public partial class ProjectCriteria : Criteria
    {
        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty = P<ProjectCriteria>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<ProjectCriteria>.RegisterRef(e => e.Factory, FactoryIdProperty);

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
        public static readonly IRefIdProperty DepartmentIdProperty = P<ProjectCriteria>.RegisterRefId(e => e.DepartmentId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> DepartmentProperty = P<ProjectCriteria>.RegisterRef(e => e.Department, DepartmentIdProperty);

        /// <summary>
        /// 部门
        /// </summary>
        public Enterprise Department
        {
            get { return GetRefEntity(DepartmentProperty); }
            set { SetRefEntity(DepartmentProperty, value); }
        }
        #endregion

        #region 项目编号 No
        /// <summary>
        /// 项目编号
        /// </summary>
        [Label("项目编号")]
        public static readonly Property<string> NoProperty = P<ProjectCriteria>.Register(e => e.No);

        /// <summary>
        /// 项目编号
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 预算编码 BudgetNo
        /// <summary>
        /// 预算编码
        /// </summary>
        [Label("预算编码")]
        public static readonly Property<string> BudgetNoProperty = P<ProjectCriteria>.Register(e => e.BudgetNo);

        /// <summary>
        /// 预算编码
        /// </summary>
        public string BudgetNo
        {
            get { return GetProperty(BudgetNoProperty); }
            set { SetProperty(BudgetNoProperty, value); }
        }
        #endregion

        #region 项目类别 ProjectType
        /// <summary>
        /// 项目类别
        /// </summary>
        [Label("项目类别")]
        public static readonly Property<string> ProjectTypeProperty = P<ProjectCriteria>.Register(e => e.ProjectType);

        /// <summary>
        /// 项目类别
        /// </summary>
        public string ProjectType
        {
            get { return this.GetProperty(ProjectTypeProperty); }
            set { this.SetProperty(ProjectTypeProperty, value); }
        }
        #endregion

        #region 年度 Year
        /// <summary>
        /// 年度
        /// </summary>
        [Label("年度")]
        public static readonly Property<DateTime?> YearProperty = P<ProjectCriteria>.Register(e => e.Year);

        /// <summary>
        /// 年度
        /// </summary>
        public DateTime? Year
        {
            get { return GetProperty(YearProperty); }
            set { SetProperty(YearProperty, value); }
        }
        #endregion

        #region 项目状态 ProjectStatus
        /// <summary>
        /// 项目状态
        /// </summary>
        [Label("项目状态")]
        public static readonly Property<ProjectStatus?> ProjectStatusProperty = P<ProjectCriteria>.Register(e => e.ProjectStatus);

        /// <summary>
        /// 项目状态
        /// </summary>
        public ProjectStatus? ProjectStatus
        {
            get { return this.GetProperty(ProjectStatusProperty); }
            set { this.SetProperty(ProjectStatusProperty, value); }
        }
        #endregion

        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<ApprovalStatus?> ApprovalStatusProperty = P<ProjectCriteria>.Register(e => e.ApprovalStatus);

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
        public static readonly Property<DateRange> CreateDateProperty = P<ProjectCriteria>.Register(e => e.CreateDate);

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
            return RT.Service.Resolve<ProjectController>().CriteriaProjects(this);
        }
    }
}
