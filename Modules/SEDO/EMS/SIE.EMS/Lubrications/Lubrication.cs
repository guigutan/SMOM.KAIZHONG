using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Domain;
using SIE.EMS.Enums;
using SIE.EMS.Lubrications.Configs;
using SIE.EMS.MainenanceProjects;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.Lubrications
{
    /// <summary>
    /// 润滑记录
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(LubricationCriteria))]
    [EntityWithConfig(typeof(NoConfig))]
    [EntityWithConfig(typeof(ApprovalConfig))]
    [EntityWithConfig(typeof(PlanTypeConfig))]
    [Label("润滑记录")]
    public partial class Lubrication : DataEntity
    {
        #region 设备台账 EquipAccount
        /// <summary>
        /// 设备台账Id
        /// </summary>
        public static readonly IRefIdProperty EquipAccountIdProperty = P<Lubrication>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备台账Id
        /// </summary>
        public double EquipAccountId
        {
            get { return (double)GetRefId(EquipAccountIdProperty); }
            set { SetRefId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备台账
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> SpecialEquipmentAccountProperty = P<Lubrication>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备台账
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return GetRefEntity(SpecialEquipmentAccountProperty); }
            set { SetRefEntity(SpecialEquipmentAccountProperty, value); }
        }
        #endregion

        #region 润滑单号 LubricationNo
        /// <summary>
        /// 润滑单号
        /// </summary>
        [Label("润滑单号")]
        [Required]
        public static readonly Property<string> LubricationNoProperty = P<Lubrication>.Register(e => e.LubricationNo);

        /// <summary>
        /// 润滑单号
        /// </summary>
        public string LubricationNo
        {
            get { return GetProperty(LubricationNoProperty); }
            set { SetProperty(LubricationNoProperty, value); }
        }
        #endregion

        #region 计划日期 PlanDate
        /// <summary>
        /// 计划日期
        /// </summary>
        [Label("计划日期")]
        public static readonly Property<DateTime> PlanDateProperty = P<Lubrication>.Register(e => e.PlanDate);

        /// <summary>
        /// 计划日期
        /// </summary>
        public DateTime PlanDate
        {
            get { return GetProperty(PlanDateProperty); }
            set { SetProperty(PlanDateProperty, value); }
        }
        #endregion

        #region 润滑开始日期 StartDateTime
        /// <summary>
        /// 润滑开始日期
        /// </summary>
        [Label("润滑开始日期")]
        public static readonly Property<DateTime?> StartDateTimeProperty = P<Lubrication>.Register(e => e.StartDateTime);

        /// <summary>
        /// 润滑开始日期
        /// </summary>
        public DateTime? StartDateTime
        {
            get { return GetProperty(StartDateTimeProperty); }
            set { SetProperty(StartDateTimeProperty, value); }
        }
        #endregion

        #region 润滑结束日期 EndDateTime
        /// <summary>
        /// 润滑结束日期
        /// </summary>
        [Label("润滑结束日期")]
        public static readonly Property<DateTime?> EndDateTimeProperty = P<Lubrication>.Register(e => e.EndDateTime);

        /// <summary>
        /// 润滑结束日期
        /// </summary>
        public DateTime? EndDateTime
        {
            get { return GetProperty(EndDateTimeProperty); }
            set { SetProperty(EndDateTimeProperty, value); }
        }
        #endregion

        #region 总工时 TotalHours
        /// <summary>
        /// 总工时
        /// </summary>
        [Label("总工时")]
        public static readonly Property<decimal> TotalHoursProperty = P<Lubrication>.Register(e => e.TotalHours);

        /// <summary>
        /// 总工时
        /// </summary>
        public decimal TotalHours
        {
            get { return GetProperty(TotalHoursProperty); }
            set { SetProperty(TotalHoursProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(1000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<Lubrication>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 工时登记 LubricationWorkHourList
        /// <summary>
        /// 工时登记
        /// </summary>
        public static readonly ListProperty<EntityList<LubricationWorkHour>> LubricationWorkHourListProperty = P<Lubrication>.RegisterList(e => e.LubricationWorkHourList);
        /// <summary>
        /// 工时登记
        /// </summary>
        public EntityList<LubricationWorkHour> LubricationWorkHourList
        {
            get { return this.GetLazyList(LubricationWorkHourListProperty); }
        }
        #endregion

        #region 单据来源 BillSourceType
        /// <summary>
        /// 单据来源
        /// </summary>
        [Label("单据来源")]
        public static readonly Property<BillSourceType> BillSourceTypeProperty = P<Lubrication>.Register(e => e.BillSourceType);

        /// <summary>
        /// 单据来源
        /// </summary>
        public BillSourceType BillSourceType
        {
            get { return GetProperty(BillSourceTypeProperty); }
            set { SetProperty(BillSourceTypeProperty, value); }
        }
        #endregion

        #region 附件 LubricationAttachmentList
        /// <summary>
        /// 附件
        /// </summary>
        public static readonly ListProperty<EntityList<LubricationAttachment>> LubricationAttachmentListProperty = P<Lubrication>.RegisterList(e => e.LubricationAttachmentList);
        /// <summary>
        /// 附件
        /// </summary>
        public EntityList<LubricationAttachment> LubricationAttachmentList
        {
            get { return this.GetLazyList(LubricationAttachmentListProperty); }
        }
        #endregion

        #region 责任部门 Department
        /// <summary>
        /// 责任部门Id
        /// </summary>
        public static readonly IRefIdProperty DepartmentIdProperty = P<Lubrication>.RegisterRefId(e => e.DepartmentId, ReferenceType.Normal);

        /// <summary>
        /// 责任部门Id
        /// </summary>
        public double? DepartmentId
        {
            get { return (double?)GetRefNullableId(DepartmentIdProperty); }
            set { SetRefNullableId(DepartmentIdProperty, value); }
        }

        /// <summary>
        /// 责任部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> DepartmentProperty = P<Lubrication>.RegisterRef(e => e.Department, DepartmentIdProperty);

        /// <summary>
        /// 责任部门
        /// </summary>
        public Enterprise Department
        {
            get { return GetRefEntity(DepartmentProperty); }
            set { SetRefEntity(DepartmentProperty, value); }
        }
        #endregion

        #region 执行人 Executor
        /// <summary>
        /// 执行人Id
        /// </summary>
        public static readonly IRefIdProperty ExecutorIdProperty = P<Lubrication>.RegisterRefId(e => e.ExecutorId, ReferenceType.Normal);

        /// <summary>
        /// 执行人Id
        /// </summary>
        public double? ExecutorId
        {
            get { return (double?)GetRefNullableId(ExecutorIdProperty); }
            set { SetRefNullableId(ExecutorIdProperty, value); }
        }

        /// <summary>
        /// 执行人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ExecutorProperty = P<Lubrication>.RegisterRef(e => e.Executor, ExecutorIdProperty);

        /// <summary>
        /// 执行人
        /// </summary>
        public Employee Executor
        {
            get { return GetRefEntity(ExecutorProperty); }
            set { SetRefEntity(ExecutorProperty, value); }
        }
        #endregion

        #region 执行人名称 ExecutorName
        /// <summary>
        /// 执行人名称
        /// </summary>
        [Label("执行人")]
        public static readonly Property<string> ExecutorNameProperty = P<Lubrication>.RegisterView(e => e.ExecutorName, p => p.Executor.Name);

        /// <summary>
        /// 执行人
        /// </summary>
        public string ExecutorName
        {
            get { return this.GetProperty(ExecutorNameProperty); }
        }
        #endregion

        #region 润滑项目 LubricationDetailList
        /// <summary>
        /// 润滑项目
        /// </summary>
        public static readonly ListProperty<EntityList<LubricationDetail>> LubricationDetailListProperty = P<Lubrication>.RegisterList(e => e.LubricationDetailList);
        /// <summary>
        /// 润滑项目
        /// </summary>
        public EntityList<LubricationDetail> LubricationDetailList
        {
            get { return this.GetLazyList(LubricationDetailListProperty); }
        }
        #endregion

        #region 周期类型 CycleType
        /// <summary>
        /// 周期类型
        /// </summary>
        [Label("周期类型")]
        public static readonly Property<CycleType> CycleTypeProperty = P<Lubrication>.Register(e => e.CycleType);

        /// <summary>
        /// 周期类型
        /// </summary>
        public CycleType CycleType
        {
            get { return GetProperty(CycleTypeProperty); }
            set { SetProperty(CycleTypeProperty, value); }
        }
        #endregion

        #region 执行状态 LubricationStatus
        /// <summary>
        /// 执行状态
        /// </summary>
        [Label("执行状态")]
        public static readonly Property<LubricationStatus> LubricationStatusProperty = P<Lubrication>.Register(e => e.LubricationStatus);

        /// <summary>
        /// 执行状态
        /// </summary>
        public LubricationStatus LubricationStatus
        {
            get { return GetProperty(LubricationStatusProperty); }
            set { SetProperty(LubricationStatusProperty, value); }
        }
        #endregion

        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<Lubrication>.Register(e => e.ApprovalStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus ApprovalStatus
        {
            get { return GetProperty(ApprovalStatusProperty); }
            set { SetProperty(ApprovalStatusProperty, value); }
        }
        #endregion

        #region 润滑小结 LubricationSummary
        /// <summary>
        /// 润滑小结
        /// </summary>
        [MaxLength(1000)]
        [Label("润滑小结")]
        public static readonly Property<string> LubricationSummaryProperty = P<Lubrication>.Register(e => e.LubricationSummary);

        /// <summary>
        /// 润滑小结
        /// </summary>
        public string LubricationSummary
        {
            get { return this.GetProperty(LubricationSummaryProperty); }
            set { this.SetProperty(LubricationSummaryProperty, value); }
        }

        #endregion

        #region 备件更换 LubricationSparePartList
        /// <summary>
        /// 备件更换
        /// </summary>
        public static readonly ListProperty<EntityList<LubricationSparePart>> LubricationSparePartListProperty = P<Lubrication>.RegisterList(e => e.LubricationSparePartList);
        /// <summary>
        /// 备件更换
        /// </summary>
        public EntityList<LubricationSparePart> LubricationSparePartList
        {
            get { return this.GetLazyList(LubricationSparePartListProperty); }
        }
        #endregion

        #region 备件申请 LubricationSparePartApplyList
        /// <summary>
        /// 备件申请
        /// </summary>
        public static readonly ListProperty<EntityList<LubricationSparePartApply>> LubricationSparePartApplyListProperty = P<Lubrication>.RegisterList(e => e.LubricationSparePartApplyList);
        /// <summary>
        /// 备件申请
        /// </summary>
        public EntityList<LubricationSparePartApply> LubricationSparePartApplyList
        {
            get { return this.GetLazyList(LubricationSparePartApplyListProperty); }
        }
        #endregion

        #region  视图属性
        #region 设备台账编码 EquipAccountCode
        /// <summary>
        /// 设备台账编码
        /// </summary>
        [Label("设备台账编码")]
        public static readonly Property<string> EquipAccountCodeProperty = P<Lubrication>.RegisterView(e => e.EquipAccountCode, p => p.EquipAccount.Code);

        /// <summary>
        /// 设备台账编码
        /// </summary>
        public string EquipAccountCode
        {
            get { return this.GetProperty(EquipAccountCodeProperty); }
        }
        #endregion

        #region 设备台账名称 EquipAccountName
        /// <summary>
        /// 设备台账名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipAccountNameProperty = P<Lubrication>.RegisterView(e => e.EquipAccountName, p => p.EquipAccount.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipAccountName
        {
            get { return this.GetProperty(EquipAccountNameProperty); }
        }
        #endregion

        #region 设备型号  EquipModelCode
        /// <summary>
        /// 设备型号
        /// </summary>
        [Label("设备型号")]
        public static readonly Property<string> EquipModelCodeProperty = P<Lubrication>.RegisterView(e => e.EquipModelCode, p => p.EquipAccount.EquipModel.Code);

        /// <summary>
        /// 设备型号
        /// </summary>
        public string EquipModelCode
        {
            get { return this.GetProperty(EquipModelCodeProperty); }
        }
        #endregion

        #region 型号名称  EquipModelName
        /// <summary>
        /// 型号名称
        /// </summary>
        [Label("型号名称")]
        public static readonly Property<string> EquipModelNameProperty = P<Lubrication>.RegisterView(e => e.EquipModelName, p => p.EquipAccount.EquipModel.Name);

        /// <summary>
        /// 型号名称
        /// </summary>
        public string EquipModelName
        {
            get { return this.GetProperty(EquipModelNameProperty); }
        }
        #endregion

        #region 设备类型id EquipTypeId
        /// <summary>
        /// 设备类型id
        /// </summary>
        [Label("设备类型id")]
        public static readonly Property<double> EquipTypeIdProperty = P<Lubrication>.RegisterView(e => e.EquipTypeId, p => p.EquipAccount.EquipModel.EquipTypeId);

        /// <summary>
        /// 设备类型id
        /// </summary>
        public double EquipTypeId
        {
            get { return this.GetProperty(EquipTypeIdProperty); }
        }
        #endregion

        #region 设备类型  EquipTypeCode
        /// <summary>
        /// 设备类型
        /// </summary>
        [Label("设备类型")]
        public static readonly Property<string> EquipTypeCodeProperty = P<Lubrication>.RegisterView(e => e.EquipTypeCode, p => p.EquipAccount.EquipModel.EquipType.TypeCode);

        /// <summary>
        /// 设备类型
        /// </summary>
        public string EquipTypeCode
        {
            get { return this.GetProperty(EquipTypeCodeProperty); }
        }
        #endregion

        #region 类型名称  EquipTypeName
        /// <summary>
        /// 类型名称
        /// </summary>
        [Label("类型名称")]
        public static readonly Property<string> EquipTypeNameProperty = P<Lubrication>.RegisterView(e => e.EquipTypeName, p => p.EquipAccount.EquipModel.EquipType.TypeName);

        /// <summary>
        /// 类型名称
        /// </summary>
        public string EquipTypeName
        {
            get { return this.GetProperty(EquipTypeNameProperty); }
        }
        #endregion

        #region 使用部门  UseDepartment
        /// <summary>
        /// 使用部门
        /// </summary>
        [Label("部门")]
        public static readonly Property<string> UseDepartmentProperty = P<Lubrication>.RegisterView(e => e.UseDepartment, p => p.EquipAccount.UseDepartment.Name);

        /// <summary>
        /// 使用部门
        /// </summary>
        public string UseDepartment
        {
            get { return this.GetProperty(UseDepartmentProperty); }
        }
        #endregion

        #region 车间名称 WorkShopName
        /// <summary>
        /// 车间名称
        /// </summary>
        [Label("车间")]
        public static readonly Property<string> WorkShopNameProperty = P<Lubrication>.RegisterView(e => e.WorkShopName, p => p.EquipAccount.WorkShop.Name);

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkShopName
        {
            get { return GetProperty(WorkShopNameProperty); }
        }
        #endregion

        #region 产线名称 ResourceName
        /// <summary>
        /// 产线名称
        /// </summary>
        [Label("产线名称")]
        public static readonly Property<string> ResourceNameProperty = P<Lubrication>.RegisterView(e => e.ResourceName, p => p.EquipAccount.Resource.Name);

        /// <summary>
        /// 产线名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
        }
        #endregion

        #region 位置 InstallationLocation
        /// <summary>
        /// 位置
        /// </summary>
        [Label("位置")]
        public static readonly Property<string> InstallationLocationProperty = P<Lubrication>.RegisterView(e => e.InstallationLocation, p => p.EquipAccount.InstallationLocation);

        /// <summary>
        /// 位置
        /// </summary>
        public string InstallationLocation
        {
            get { return this.GetProperty(InstallationLocationProperty); }
        }
        #endregion

        #region 责任部门编码 DepartmentCode
        /// <summary>
        /// 责任部门编码
        /// </summary>
        [Label("责任部门编码")]
        public static readonly Property<string> DepartmentCodeProperty = P<Lubrication>.RegisterView(e => e.DepartmentCode, p => p.Department.Code);

        /// <summary>
        /// 责任部门编码
        /// </summary>
        public string DepartmentCode
        {
            get { return this.GetProperty(DepartmentCodeProperty); }
        }
        #endregion

        #region 责任部门名称 DepartmentName
        /// <summary>
        /// 责任部门名称
        /// </summary>
        [Label("责任部门名称")]
        public static readonly Property<string> DepartmentNameProperty = P<Lubrication>.RegisterView(e => e.DepartmentName, p => p.Department.Name);

        /// <summary>
        /// 责任部门名称
        /// </summary>
        public string DepartmentName
        {
            get { return this.GetProperty(DepartmentNameProperty); }
        }
        #endregion
        #endregion

        #region  不映射数据库字段
        #region 上一次润滑小结 LastLubricationSummary
        /// <summary>
        /// 上一次润滑小结
        /// </summary>
        [Label("上一次润滑小结")]
        public static readonly Property<string> LastLubricationSummaryProperty = P<Lubrication>.Register(e => e.LastLubricationSummary);

        /// <summary>
        /// 上一次润滑小结
        /// </summary>
        public string LastLubricationSummary
        {
            get { return this.GetProperty(LastLubricationSummaryProperty); }
            set { this.SetProperty(LastLubricationSummaryProperty, value); }
        }

        #endregion
        #endregion

    }

    /// <summary>
    /// 设备润滑 实体配置
    /// </summary>
    internal class LubricationConfig : EntityConfig<Lubrication>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_LUBR").MapAllProperties();
            Meta.Property(Lubrication.LastLubricationSummaryProperty).DontMapColumn();
            Meta.Property(Lubrication.RemarkProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}