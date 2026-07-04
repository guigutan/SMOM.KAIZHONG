using SIE.DataAuth;
using SIE.Domain;
using SIE.EMS.DataAuth;
using SIE.EMS.Projects.Enums;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.Projects
{
    /// <summary>
    /// 项目
    /// </summary>
    [RootEntity, Serializable]
    [Label("项目管理")]
    [DisplayMember(nameof(Code))]
    [EntityDataAuth(typeof(EmployeeEnterprise), nameof(FactoryId), true)]
    [BudgetDepartmentAuth(nameof(DepartmentId), true)]
    public partial class Project : DataEntity
    {
        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty = P<Project>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<Project>.RegisterRef(e => e.Factory, FactoryIdProperty);

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
        public static readonly IRefIdProperty DepartmentIdProperty = P<Project>.RegisterRefId(e => e.DepartmentId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> DepartmentProperty = P<Project>.RegisterRef(e => e.Department, DepartmentIdProperty);

        /// <summary>
        /// 部门
        /// </summary>
        public Enterprise Department
        {
            get { return GetRefEntity(DepartmentProperty); }
            set { SetRefEntity(DepartmentProperty, value); }
        }
        #endregion

        #region 项目编码 Code
        /// <summary>
        /// 项目编码
        /// </summary>
        [Label("项目编码")]
        [Required]
        [NotDuplicate]
        public static readonly Property<string> CodeProperty = P<Project>.Register(e => e.Code);

        /// <summary>
        /// 项目编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 项目名称 Name
        /// <summary>
        /// 项目名称
        /// </summary>
        [Label("项目名称")]
        [Required]
        public static readonly Property<string> NameProperty = P<Project>.Register(e => e.Name);

        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<Project>.Register(e => e.ApprovalStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus ApprovalStatus
        {
            get { return GetProperty(ApprovalStatusProperty); }
            set { SetProperty(ApprovalStatusProperty, value); }
        }
        #endregion

        #region 项目状态 ProjectStatus
        /// <summary>
        /// 项目状态
        /// </summary>
        [Label("项目状态")]
        public static readonly Property<ProjectStatus> ProjectStatusProperty = P<Project>.Register(e => e.ProjectStatus);

        /// <summary>
        /// 项目状态
        /// </summary>
        public ProjectStatus ProjectStatus
        {
            get { return GetProperty(ProjectStatusProperty); }
            set { SetProperty(ProjectStatusProperty, value); }
        }
        #endregion

        #region 上一项目状态 PreviousStatus
        /// <summary>
        /// 上一项目状态
        /// </summary>
        [Label("项目状态")]
        public static readonly Property<ProjectStatus?> PreviousStatusProperty = P<Project>.Register(e => e.PreviousStatus);

        /// <summary>
        /// 上一项目状态
        /// </summary>
        public ProjectStatus? PreviousStatus
        {
            get { return GetProperty(PreviousStatusProperty); }
            set { SetProperty(PreviousStatusProperty, value); }
        }
        #endregion

        #region 项目负责人 Principal
        /// <summary>
        /// 项目负责人Id
        /// </summary>
        [Label("项目负责人")]
        public static readonly IRefIdProperty PrincipalIdProperty = P<Project>.RegisterRefId(e => e.PrincipalId, ReferenceType.Normal);

        /// <summary>
        /// 项目负责人Id
        /// </summary>
        public double PrincipalId
        {
            get { return (double)GetRefId(PrincipalIdProperty); }
            set { SetRefId(PrincipalIdProperty, value); }
        }

        /// <summary>
        /// 项目负责人
        /// </summary>
        public static readonly RefEntityProperty<Employee> PrincipalProperty = P<Project>.RegisterRef(e => e.Principal, PrincipalIdProperty);

        /// <summary>
        /// 项目负责人
        /// </summary>
        public Employee Principal
        {
            get { return GetRefEntity(PrincipalProperty); }
            set { SetRefEntity(PrincipalProperty, value); }
        }
        #endregion

        #region 预算金额 Amount
        /// <summary>
        /// 预算金额
        /// </summary>
        [Label("预算金额")]
        [MinValue(0)]
        public static readonly Property<decimal> AmountProperty = P<Project>.Register(e => e.Amount);

        /// <summary>
        /// 预算金额
        /// </summary>
        public decimal Amount
        {
            get { return GetProperty(AmountProperty); }
            set { SetProperty(AmountProperty, value); }
        }
        #endregion

        #region 中标金额 ActualAmount
        /// <summary>
        /// 中标金额
        /// </summary>
        [Label("中标金额")]
        public static readonly Property<decimal?> ActualAmountProperty = P<Project>.Register(e => e.ActualAmount);

        /// <summary>
        /// 中标金额
        /// </summary>
        public decimal? ActualAmount
        {
            get { return GetProperty(ActualAmountProperty); }
            set { SetProperty(ActualAmountProperty, value); }
        }
        #endregion

        #region 项目事项 KeyItemList
        /// <summary>
        /// 项目事项
        /// </summary>
        [Label("项目事项")]
        public static readonly ListProperty<EntityList<ProjectKeyItem>> KeyItemListProperty = P<Project>.RegisterList(e => e.KeyItemList);

        /// <summary>
        /// 项目事项
        /// </summary>
        public EntityList<ProjectKeyItem> KeyItemList
        {
            get { return this.GetLazyList(KeyItemListProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 项目 实体配置
    /// </summary>
    internal class ProjectConfig : EntityConfig<Project>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EARL_PROJ").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
