using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.DataAuth;
using SIE.Domain;
using SIE.EMS.DataAuth;
using SIE.EMS.Projects;
using SIE.EMS.Purchases.Enums;
using SIE.EMS.Purchases.EquipmentSetups.Configs;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.Purchases.EquipmentSetups
{
    /// <summary>
    /// 安装调试
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(EquipmentSetupCriteria))]
    [Label("安装调试")]
    [EntityWithConfig(typeof(RelationOutDepotConfig))]
    [DisplayMember(nameof(SetupNo))]
    [EntityWithConfig(typeof(ApprovalConfig))]
    [EntityWithConfig(typeof(NoConfig), "安装调试单号配置项", "安装调试单号生成规则")]
    [EntityDataAuth(typeof(EmployeeEnterprise), nameof(FactoryId), true)]
    [BudgetDepartmentAuth(nameof(DepartmentId), true)]
    public partial class EquipmentSetup : DataEntity
    {
        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty = P<EquipmentSetup>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<EquipmentSetup>.RegisterRef(e => e.Factory, FactoryIdProperty);

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
        public static readonly IRefIdProperty DepartmentIdProperty = P<EquipmentSetup>.RegisterRefId(e => e.DepartmentId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> DepartmentProperty = P<EquipmentSetup>.RegisterRef(e => e.Department, DepartmentIdProperty);

        /// <summary>
        /// 部门
        /// </summary>
        public Enterprise Department
        {
            get { return GetRefEntity(DepartmentProperty); }
            set { SetRefEntity(DepartmentProperty, value); }
        }
        #endregion

        #region 安装调试单号 SetupNo
        /// <summary>
        /// 安装调试单号
        /// </summary>
        [Label("安装调试单号")]
        [Required]
        [NotDuplicate]
        public static readonly Property<string> SetupNoProperty = P<EquipmentSetup>.Register(e => e.SetupNo);

        /// <summary>
        /// 安装调试单号
        /// </summary>
        public string SetupNo
        {
            get { return GetProperty(SetupNoProperty); }
            set { SetProperty(SetupNoProperty, value); }
        }
        #endregion

        #region 位置 Location
        /// <summary>
        /// 位置
        /// </summary>
        [Label("位置")]
        public static readonly Property<string> LocationProperty = P<EquipmentSetup>.Register(e => e.Location);

        /// <summary>
        /// 位置
        /// </summary>
        public string Location
        {
            get { return GetProperty(LocationProperty); }
            set { SetProperty(LocationProperty, value); }
        }
        #endregion

        #region 计划开始日期 PlanStartDate
        /// <summary>
        /// 计划开始日期
        /// </summary>
        [Label("计划开始日期")]
        public static readonly Property<DateTime> PlanStartDateProperty = P<EquipmentSetup>.Register(e => e.PlanStartDate);

        /// <summary>
        /// 计划开始日期
        /// </summary>
        public DateTime PlanStartDate
        {
            get { return GetProperty(PlanStartDateProperty); }
            set { SetProperty(PlanStartDateProperty, value); }
        }
        #endregion

        #region 计划结束日期 PlanEndDate
        /// <summary>
        /// 计划结束日期
        /// </summary>
        [Label("计划结束日期")]
        public static readonly Property<DateTime> PlanEndDateProperty = P<EquipmentSetup>.Register(e => e.PlanEndDate);

        /// <summary>
        /// 计划结束日期
        /// </summary>
        public DateTime PlanEndDate
        {
            get { return GetProperty(PlanEndDateProperty); }
            set { SetProperty(PlanEndDateProperty, value); }
        }
        #endregion

        #region 委外 OutSource
        /// <summary>
        /// 委外
        /// </summary>
        [Label("委外")]
        public static readonly Property<bool> OutSourceProperty = P<EquipmentSetup>.Register(e => e.OutSource);

        /// <summary>
        /// 委外
        /// </summary>
        public bool OutSource
        {
            get { return GetProperty(OutSourceProperty); }
            set { SetProperty(OutSourceProperty, value); }
        }
        #endregion

        #region 安装说明 SetupNote
        /// <summary>
        /// 安装说明
        /// </summary>
        [MaxLength(1000)]
        [Label("安装说明")]
        public static readonly Property<string> SetupNoteProperty = P<EquipmentSetup>.Register(e => e.SetupNote);

        /// <summary>
        /// 安装说明
        /// </summary>
        public string SetupNote
        {
            get { return GetProperty(SetupNoteProperty); }
            set { SetProperty(SetupNoteProperty, value); }
        }
        #endregion

        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<EquipmentSetup>.Register(e => e.ApprovalStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus ApprovalStatus
        {
            get { return GetProperty(ApprovalStatusProperty); }
            set { SetProperty(ApprovalStatusProperty, value); }
        }
        #endregion

        #region 项目 Project
        /// <summary>
        /// 项目Id
        /// </summary>
        [Label("项目")]
        public static readonly IRefIdProperty ProjectIdProperty = P<EquipmentSetup>.RegisterRefId(e => e.ProjectId, ReferenceType.Normal);

        /// <summary>
        /// 项目Id
        /// </summary>
        public double? ProjectId
        {
            get { return (double?)GetRefNullableId(ProjectIdProperty); }
            set { SetRefNullableId(ProjectIdProperty, value); }
        }

        /// <summary>
        /// 项目
        /// </summary>
        public static readonly RefEntityProperty<Project> ProjectProperty = P<EquipmentSetup>.RegisterRef(e => e.Project, ProjectIdProperty);

        /// <summary>
        /// 项目
        /// </summary>
        public Project Project
        {
            get { return GetRefEntity(ProjectProperty); }
            set { SetRefEntity(ProjectProperty, value); }
        }
        #endregion

        #region 状态 SetupStatus
        /// <summary>
        /// 状态
        /// </summary>
        [Label("安装调试状态")]
        public static readonly Property<SetupStatus> SetupStatusProperty = P<EquipmentSetup>.Register(e => e.SetupStatus);

        /// <summary>
        /// 状态
        /// </summary>
        public SetupStatus SetupStatus
        {
            get { return GetProperty(SetupStatusProperty); }
            set { SetProperty(SetupStatusProperty, value); }
        }
        #endregion

        #region 负责人 Principal
        /// <summary>
        /// 负责人Id
        /// </summary>
        [Label("负责人")]
        public static readonly IRefIdProperty PrincipalIdProperty = P<EquipmentSetup>.RegisterRefId(e => e.PrincipalId, ReferenceType.Normal);

        /// <summary>
        /// 负责人Id
        /// </summary>
        public double PrincipalId
        {
            get { return (double)GetRefId(PrincipalIdProperty); }
            set { SetRefId(PrincipalIdProperty, value); }
        }

        /// <summary>
        /// 负责人
        /// </summary>
        public static readonly RefEntityProperty<Employee> PrincipalProperty = P<EquipmentSetup>.RegisterRef(e => e.Principal, PrincipalIdProperty);

        /// <summary>
        /// 负责人
        /// </summary>
        public Employee Principal
        {
            get { return GetRefEntity(PrincipalProperty); }
            set { SetRefEntity(PrincipalProperty, value); }
        }
        #endregion

        #region 车间 Workshop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkshopIdProperty = P<EquipmentSetup>.RegisterRefId(e => e.WorkshopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkshopId
        {
            get { return (double?)GetRefNullableId(WorkshopIdProperty); }
            set { SetRefNullableId(WorkshopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkshopProperty = P<EquipmentSetup>.RegisterRef(e => e.Workshop, WorkshopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise Workshop
        {
            get { return GetRefEntity(WorkshopProperty); }
            set { SetRefEntity(WorkshopProperty, value); }
        }
        #endregion

        #region 工作计划 EquipmentSetupPlanList
        /// <summary>
        /// 工作计划
        /// </summary>
        public static readonly ListProperty<EntityList<EquipmentSetupPlan>> EquipmentSetupPlanListProperty = P<EquipmentSetup>.RegisterList(e => e.EquipmentSetupPlanList);
        /// <summary>
        /// 工作计划
        /// </summary>
        public EntityList<EquipmentSetupPlan> EquipmentSetupPlanList
        {
            get { return this.GetLazyList(EquipmentSetupPlanListProperty); }
        }
        #endregion

        #region 设备明细 EquipmentDetailList
        /// <summary>
        /// 设备明细
        /// </summary>
        public static readonly ListProperty<EntityList<EquipmentDetail>> EquipmentDetailListProperty = P<EquipmentSetup>.RegisterList(e => e.EquipmentDetailList);
        /// <summary>
        /// 设备明细
        /// </summary>
        public EntityList<EquipmentDetail> EquipmentDetailList
        {
            get { return this.GetLazyList(EquipmentDetailListProperty); }
        }
        #endregion

        #region 备件申请 SetupSparePartApplyList
        /// <summary>
        /// 备件申请
        /// </summary>
        public static readonly ListProperty<EntityList<SetupSparePartApply>> SetupSparePartApplyListProperty = P<EquipmentSetup>.RegisterList(e => e.SetupSparePartApplyList);
        /// <summary>
        /// 备件申请
        /// </summary>
        public EntityList<SetupSparePartApply> SetupSparePartApplyList
        {
            get { return this.GetLazyList(SetupSparePartApplyListProperty); }
        }
        #endregion

        #region 备件使用 SetupSparePartList
        /// <summary>
        /// 备件使用
        /// </summary>
        public static readonly ListProperty<EntityList<SetupSparePart>> SetupSparePartListProperty = P<EquipmentSetup>.RegisterList(e => e.SetupSparePartList);
        /// <summary>
        /// 备件使用
        /// </summary>
        public EntityList<SetupSparePart> SetupSparePartList
        {
            get { return this.GetLazyList(SetupSparePartListProperty); }
        }
        #endregion

        #region 工时 SetupWorkHourList
        /// <summary>
        /// 工时
        /// </summary>
        public static readonly ListProperty<EntityList<SetupWorkHour>> SetupWorkHourListProperty = P<EquipmentSetup>.RegisterList(e => e.SetupWorkHourList);
        /// <summary>
        /// 工时
        /// </summary>
        public EntityList<SetupWorkHour> SetupWorkHourList
        {
            get { return this.GetLazyList(SetupWorkHourListProperty); }
        }
        #endregion

        #region 操作记录 SetupLogList
        /// <summary>
        /// 操作记录
        /// </summary>
        public static readonly ListProperty<EntityList<SetupLog>> SetupLogListProperty = P<EquipmentSetup>.RegisterList(e => e.SetupLogList);
        /// <summary>
        /// 操作记录
        /// </summary>
        public EntityList<SetupLog> SetupLogList
        {
            get { return this.GetLazyList(SetupLogListProperty); }
        }
        #endregion

        #region 附件 SetupAttachmentList
        /// <summary>
        /// 附件
        /// </summary>
        public static readonly ListProperty<EntityList<SetupAttachment>> SetupAttachmentListProperty = P<EquipmentSetup>.RegisterList(e => e.SetupAttachmentList);
        /// <summary>
        /// 附件
        /// </summary>
        public EntityList<SetupAttachment> SetupAttachmentList
        {
            get { return this.GetLazyList(SetupAttachmentListProperty); }
        }
        #endregion

        #region 视图属性
        #region 项目名称 ProjectName
        /// <summary>
        /// 项目名称
        /// </summary>
        [Label("项目名称")]
        public static readonly Property<string> ProjectNameProperty = P<EquipmentSetup>.RegisterView(e => e.ProjectName, p => p.Project.Name);

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName
        {
            get { return this.GetProperty(ProjectNameProperty); }
        }
        #endregion

        #region 负责人编码 PrincipalCode
        /// <summary>
        /// 负责人编码
        /// </summary>
        [Label("负责人编码")]
        public static readonly Property<string> PrincipalCodeProperty = P<EquipmentSetup>.RegisterView(e => e.PrincipalCode, p => p.Principal.Code);

        /// <summary>
        /// 负责人编码
        /// </summary>
        public string PrincipalCode
        {
            get { return this.GetProperty(PrincipalCodeProperty); }
        }
        #endregion

        #region 负责人名称 PrincipalName
        /// <summary>
        /// 负责人名称
        /// </summary>
        [Label("负责人名称")]
        public static readonly Property<string> PrincipalNameProperty = P<EquipmentSetup>.RegisterView(e => e.PrincipalName, p => p.Principal.Name);

        /// <summary>
        /// 负责人名称
        /// </summary>
        public string PrincipalName
        {
            get { return this.GetProperty(PrincipalNameProperty); }
        }
        #endregion

        #region 负责人 PrincipalShow
        /// <summary>
        /// 负责人
        /// </summary>
        [Label("负责人")]
        public static readonly Property<string> PrincipalShowProperty = P<EquipmentSetup>.RegisterReadOnly(
            e => e.PrincipalShow, e => e.GetPrincipalShow(), PrincipalCodeProperty);
        /// <summary>
        /// 负责人
        /// </summary>

        public string PrincipalShow
        {
            get { return this.GetProperty(PrincipalShowProperty); }
        }
        private string GetPrincipalShow()
        {
            return PrincipalName + "(" + PrincipalCode + ")";
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 安装调试 实体配置
    /// </summary>
    internal class EquipmentSetupConfig : EntityConfig<EquipmentSetup>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_SETUP").MapAllProperties();
            Meta.Property(EquipmentSetup.SetupNoteProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}