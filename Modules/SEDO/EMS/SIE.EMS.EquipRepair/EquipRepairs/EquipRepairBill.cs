using SIE.Common.Configs;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Devices.Abnormals;
using SIE.EMS.EquipRepair.EquipRepairs.Configs;
using SIE.EMS.EquipRepair.EquipRepairs.Enums;
using SIE.EMS.Faults;
using SIE.EMS.Projects;
using SIE.EMS.SpareParts;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.EMS.EquipRepair.EquipRepairs
{
    /// <summary>
    /// 设备维修单
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(EquipRepairCriteria))]
    [EntityWithConfig(typeof(EquipRepairNoConfig))]
    [EntityWithConfig(typeof(IsEngineerConfirmConfig))]
    [EntityWithConfig(typeof(IsHandoverConfirmConfig))]
    [Label("设备维修单")]
    public partial class EquipRepairBill : SIE.EMS.EquipRepairs.EquipRepairBill
    {
        /// <summary>
        /// EXP_FAULT_RESON
        /// </summary>
        public static readonly string CatalogExpFaultReson = "EXP_FAULT_RESON";

        #region 备件 SparePart
        /// <summary>
        /// 备件Id
        /// </summary>
        [Label("备件")]
        public static readonly IRefIdProperty SparePartIdProperty =
            P<EquipRepairBill>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

        /// <summary>
        /// 备件Id
        /// </summary>
        public double? SparePartId
        {
            get { return (double?)this.GetRefId(SparePartIdProperty); }
            set { this.SetRefId(SparePartIdProperty, value); }
        }

        /// <summary>
        /// 备件
        /// </summary>
        public static readonly RefEntityProperty<SparePart> SparePartProperty =
            P<EquipRepairBill>.RegisterRef(e => e.SparePart, SparePartIdProperty);

        /// <summary>
        /// 备件
        /// </summary>
        public SparePart SparePart
        {
            get { return this.GetRefEntity(SparePartProperty); }
            set { this.SetRefEntity(SparePartProperty, value); }
        }
        #endregion

        #region 故障现象 DeviceAbnormal
        /// <summary>
        /// 故障现象Id
        /// </summary>
        [Label("故障现象")]
        public static readonly IRefIdProperty DeviceAbnormalIdProperty =
            P<EquipRepairBill>.RegisterRefId(e => e.DeviceAbnormalId, ReferenceType.Normal);

        /// <summary>
        /// 故障现象Id
        /// </summary>
        public double? DeviceAbnormalId
        {
            get { return (double?)this.GetRefId(DeviceAbnormalIdProperty); }
            set { this.SetRefId(DeviceAbnormalIdProperty, value); }
        }

        /// <summary>
        /// 故障现象
        /// </summary>
        public static readonly RefEntityProperty<DeviceAbnormal> DeviceAbnormalProperty =
            P<EquipRepairBill>.RegisterRef(e => e.DeviceAbnormal, DeviceAbnormalIdProperty);

        /// <summary>
        /// 故障现象
        /// </summary>
        public DeviceAbnormal DeviceAbnormal
        {
            get { return this.GetRefEntity(DeviceAbnormalProperty); }
            set { this.SetRefEntity(DeviceAbnormalProperty, value); }
        }
        #endregion

        #region 维修责任人 RepairMaster
        /// <summary>
        /// 维修责任人Id
        /// </summary>
        [Label("维修责任人")]
        public static readonly IRefIdProperty RepairMasterIdProperty =
            P<EquipRepairBill>.RegisterRefId(e => e.RepairMasterId, ReferenceType.Normal);

        /// <summary>
        /// 维修责任人Id
        /// </summary>
        public double? RepairMasterId
        {
            get { return (double?)this.GetRefNullableId(RepairMasterIdProperty); }
            set { this.SetRefNullableId(RepairMasterIdProperty, value); }
        }

        /// <summary>
        /// 维修责任人
        /// </summary>
        public static readonly RefEntityProperty<Employee> RepairMasterProperty =
            P<EquipRepairBill>.RegisterRef(e => e.RepairMaster, RepairMasterIdProperty);

        /// <summary>
        /// 维修责任人
        /// </summary>
        public Employee RepairMaster
        {
            get { return this.GetRefEntity(RepairMasterProperty); }
            set { this.SetRefEntity(RepairMasterProperty, value); }
        }
        #endregion

        #region 维修人员 RepairEmployees
        /// <summary>
        /// 维修人员
        /// </summary>
        [Label("其他维修人员")]
        [MaxLength(4000)]
        public static readonly Property<string> RepairEmployeesProperty = P<EquipRepairBill>.Register(e => e.RepairEmployees);

        /// <summary>
        /// 维修人员
        /// </summary>
        public string RepairEmployees
        {
            get { return this.GetProperty(RepairEmployeesProperty); }
            set { this.SetProperty(RepairEmployeesProperty, value); }
        }
        #endregion

        #region 维修人员Id RepairEmployeeIds
        /// <summary>
        /// 维修人员Id
        /// </summary>
        [Label("维修人员")]
        [MaxLength(4000)]
        public static readonly Property<string> RepairEmployeeIdsProperty = P<EquipRepairBill>.Register(e => e.RepairEmployeeIds);

        /// <summary>
        /// 维修人员Id
        /// </summary>
        public string RepairEmployeeIds
        {
            get { return this.GetProperty(RepairEmployeeIdsProperty); }
            set { this.SetProperty(RepairEmployeeIdsProperty, value); }
        }
        #endregion

        #region 接单/派工时间 ReceiveOrderDate
        /// <summary>
        /// 接单/派工时间
        /// </summary>
        [Label("接单/派工时间")]
        public static readonly Property<DateTime?> ReceiveOrderDateProperty = P<EquipRepairBill>.Register(e => e.ReceiveOrderDate);

        /// <summary>
        /// 接单/派工时间
        /// </summary>
        public DateTime? ReceiveOrderDate
        {
            get { return this.GetProperty(ReceiveOrderDateProperty); }
            set { this.SetProperty(ReceiveOrderDateProperty, value); }
        }
        #endregion

        #region 转派时间 TransferOrderDate
        /// <summary>
        /// 转派时间
        /// </summary>
        [Label("转派时间")]
        public static readonly Property<DateTime?> TransferOrderDateProperty = P<EquipRepairBill>.Register(e => e.TransferOrderDate);

        /// <summary>
        /// 转派时间
        /// </summary>
        public DateTime? TransferOrderDate
        {
            get { return this.GetProperty(TransferOrderDateProperty); }
            set { this.SetProperty(TransferOrderDateProperty, value); }
        }
        #endregion

        #region 转派原因 TransferReason
        /// <summary>
        /// 转派原因
        /// </summary>
        [Label("转派原因")]
        public static readonly Property<string> TransferReasonProperty = P<EquipRepairBill>.Register(e => e.TransferReason);

        /// <summary>
        /// 转派原因
        /// </summary>
        public string TransferReason
        {
            get { return this.GetProperty(TransferReasonProperty); }
            set { this.SetProperty(TransferReasonProperty, value); }
        }
        #endregion

        #region 原维修责任人 OriginalRepairMaster
        /// <summary>
        /// 原维修责任人Id
        /// </summary>
        [Label("原维修责任人")]
        public static readonly IRefIdProperty OriginalRepairMasterIdProperty =
            P<EquipRepairBill>.RegisterRefId(e => e.OriginalRepairMasterId, ReferenceType.Normal);

        /// <summary>
        /// 原维修责任人Id
        /// </summary>
        public double? OriginalRepairMasterId
        {
            get { return (double?)this.GetRefId(OriginalRepairMasterIdProperty); }
            set { this.SetRefId(OriginalRepairMasterIdProperty, value); }
        }

        /// <summary>
        /// 原维修责任人
        /// </summary>
        public static readonly RefEntityProperty<Employee> OriginalRepairMasterProperty =
            P<EquipRepairBill>.RegisterRef(e => e.OriginalRepairMaster, OriginalRepairMasterIdProperty);

        /// <summary>
        /// 原维修责任人
        /// </summary>
        public Employee OriginalRepairMaster
        {
            get { return this.GetRefEntity(OriginalRepairMasterProperty); }
            set { this.SetRefEntity(OriginalRepairMasterProperty, value); }
        }
        #endregion

        #region 原维修人员 OriginalRepairEmployees
        /// <summary>
        /// 原维修人员
        /// </summary>
        [Label("原维修人员")]
        [MaxLength(4000)]
        public static readonly Property<string> OriginalRepairEmployeesProperty = P<EquipRepairBill>.Register(e => e.OriginalRepairEmployees);

        /// <summary>
        /// 原维修人员
        /// </summary>
        public string OriginalRepairEmployees
        {
            get { return this.GetProperty(OriginalRepairEmployeesProperty); }
            set { this.SetProperty(OriginalRepairEmployeesProperty, value); }
        }
        #endregion

        #region 原维修人员Id OriginalRepairEmployeeIds
        /// <summary>
        /// 原维修人员Id
        /// </summary>
        [Label("原维修人员")]
        [MaxLength(4000)]
        public static readonly Property<string> OriginalRepairEmployeeIdsProperty = P<EquipRepairBill>.Register(e => e.OriginalRepairEmployeeIds);

        /// <summary>
        /// 原维修人员Id
        /// </summary>
        public string OriginalRepairEmployeeIds
        {
            get { return this.GetProperty(OriginalRepairEmployeeIdsProperty); }
            set { this.SetProperty(OriginalRepairEmployeeIdsProperty, value); }
        }
        #endregion

        #region 预计完成时间 EstimateFinishDate
        /// <summary>
        /// 预计完成时间
        /// </summary>
        [Label("预计完成时间")]
        public static readonly Property<DateTime?> EstimateFinishDateProperty = P<EquipRepairBill>.Register(e => e.EstimateFinishDate);

        /// <summary>
        /// 预计完成时间
        /// </summary>
        public DateTime? EstimateFinishDate
        {
            get { return this.GetProperty(EstimateFinishDateProperty); }
            set { this.SetProperty(EstimateFinishDateProperty, value); }
        }
        #endregion

        #region 维修开始时间 RepairBeginDate
        /// <summary>
        /// 维修开始时间
        /// </summary>
        [Label("维修开始时间")]
        public static readonly Property<DateTime?> RepairBeginDateProperty = P<EquipRepairBill>.Register(e => e.RepairBeginDate);

        /// <summary>
        /// 维修开始时间
        /// </summary>
        public DateTime? RepairBeginDate
        {
            get { return this.GetProperty(RepairBeginDateProperty); }
            set { this.SetProperty(RepairBeginDateProperty, value); }
        }
        #endregion

        #region 维修完成时间 RepairFinishDate
        /// <summary>
        /// 维修完成时间
        /// </summary>
        [Label("维修完成时间")]
        public static readonly Property<DateTime?> RepairFinishDateProperty = P<EquipRepairBill>.Register(e => e.RepairFinishDate);

        /// <summary>
        /// 维修完成时间
        /// </summary>
        public DateTime? RepairFinishDate
        {
            get { return this.GetProperty(RepairFinishDateProperty); }
            set { this.SetProperty(RepairFinishDateProperty, value); }
        }
        #endregion

        #region 交机确认结果 HandoverConfirmResult
        /// <summary>
        /// 交机确认结果
        /// </summary>
        [Label("交机确认结果")]
        public static readonly Property<HandoverConfirmResult?> HandoverConfirmResultProperty = P<EquipRepairBill>.Register(e => e.HandoverConfirmResult);

        /// <summary>
        /// 交机确认结果
        /// </summary>
        public HandoverConfirmResult? HandoverConfirmResult
        {
            get { return this.GetProperty(HandoverConfirmResultProperty); }
            set { this.SetProperty(HandoverConfirmResultProperty, value); }
        }
        #endregion

        #region 工程确认结果 EngineerConfirmResult
        /// <summary>
        /// 工程确认结果
        /// </summary>
        [Label("工程确认结果")]
        public static readonly Property<EngineerConfirmResult?> EngineerConfirmResultProperty = P<EquipRepairBill>.Register(e => e.EngineerConfirmResult);

        /// <summary>
        /// 工程确认结果
        /// </summary>
        public EngineerConfirmResult? EngineerConfirmResult
        {
            get { return this.GetProperty(EngineerConfirmResultProperty); }
            set { this.SetProperty(EngineerConfirmResultProperty, value); }
        }
        #endregion

        #region 维修时长(H) RepairTime
        /// <summary>
        /// 维修时长(H)
        /// </summary>
        [Label("维修时长(H)")]
        public static readonly Property<decimal?> RepairTimeProperty = P<EquipRepairBill>.Register(e => e.RepairTime);

        /// <summary>
        /// 维修时长(H)
        /// </summary>
        public decimal? RepairTime
        {
            get { return this.GetProperty(RepairTimeProperty); }
            set { this.SetProperty(RepairTimeProperty, value); }
        }
        #endregion

        #region 是否需要补录 IsSupplement
        /// <summary>
        /// 是否需要补录
        /// </summary>
        [Label("是否需要补录")]
        public static readonly Property<bool?> IsSupplementProperty = P<EquipRepairBill>.Register(e => e.IsSupplement);

        /// <summary>
        /// 是否需要补录
        /// </summary>
        public bool? IsSupplement
        {
            get { return this.GetProperty(IsSupplementProperty); }
            set { this.SetProperty(IsSupplementProperty, value); }
        }
        #endregion

        #region 取消原因 CancelReason
        /// <summary>
        /// 取消原因
        /// </summary>
        [Label("取消原因")]
        public static readonly Property<string> CancelReasonProperty = P<EquipRepairBill>.Register(e => e.CancelReason);

        /// <summary>
        /// 取消原因
        /// </summary>
        public string CancelReason
        {
            get { return this.GetProperty(CancelReasonProperty); }
            set { this.SetProperty(CancelReasonProperty, value); }
        }
        #endregion

        #region 附件列表 AttachmentList
        /// <summary>
        /// 附件列表
        /// </summary>
        public static readonly ListProperty<EntityList<EquipRepairAttachment>> AttachmentListProperty = P<EquipRepairBill>.RegisterList(e => e.AttachmentList);
        /// <summary>
        /// 附件列表
        /// </summary>
        public EntityList<EquipRepairAttachment> AttachmentList
        {
            get { return this.GetLazyList(AttachmentListProperty); }
        }
        #endregion

        #region 操作记录 EquipRepairOperationRecList
        /// <summary>
        /// 操作记录
        /// </summary>
        [Label("操作记录")]
        public static readonly ListProperty<EntityList<EquipRepairOperationRec>> EquipRepairOperationRecListProperty = P<EquipRepairBill>.RegisterList(e => e.EquipRepairOperationRecList);

        /// <summary>
        /// 操作记录
        /// </summary>
        public EntityList<EquipRepairOperationRec> EquipRepairOperationRecList
        {
            get { return this.GetLazyList(EquipRepairOperationRecListProperty); }
        }
        #endregion

        #region 维修工时 EquipRepairWorkingHoursList
        /// <summary>
        /// 维修工时
        /// </summary>
        [Label("维修工时")]
        public static readonly ListProperty<EntityList<EquipRepairWorkingHours>> EquipRepairWorkingHoursListProperty = P<EquipRepairBill>.RegisterList(e => e.EquipRepairWorkingHoursList);

        /// <summary>
        /// 维修工时
        /// </summary>
        public EntityList<EquipRepairWorkingHours> EquipRepairWorkingHoursList
        {
            get { return this.GetLazyList(EquipRepairWorkingHoursListProperty); }
        }
        #endregion

        #region 备件申请 EquipRepairSparePartAplList
        /// <summary>
        /// 备件申请
        /// </summary>
        [Label("备件申请")]
        public static readonly ListProperty<EntityList<EquipRepairSparePartApl>> EquipRepairSparePartAplListProperty = P<EquipRepairBill>.RegisterList(e => e.EquipRepairSparePartAplList);

        /// <summary>
        /// 备件申请
        /// </summary>
        public EntityList<EquipRepairSparePartApl> EquipRepairSparePartAplList
        {
            get { return this.GetLazyList(EquipRepairSparePartAplListProperty); }
        }
        #endregion

        #region 备件更换 EquipRepairSparePartChgList
        /// <summary>
        /// 备件更换
        /// </summary>
        [Label("备件更换")]
        public static readonly ListProperty<EntityList<EquipRepairSparePartChg>> EquipRepairSparePartChgListProperty = P<EquipRepairBill>.RegisterList(e => e.EquipRepairSparePartChgList);

        /// <summary>
        /// 备件更换
        /// </summary>
        public EntityList<EquipRepairSparePartChg> EquipRepairSparePartChgList
        {
            get { return this.GetLazyList(EquipRepairSparePartChgListProperty); }
        }
        #endregion

        #region 交机确认 HandoverConfirmDetailList
        /// <summary>
        /// 交机确认
        /// </summary>
        [Label("交机确认")]
        public static readonly ListProperty<EntityList<HandoverConfirmDetail>> HandoverConfirmDetailListProperty = P<EquipRepairBill>.RegisterList(e => e.HandoverConfirmDetailList);

        /// <summary>
        /// 交机确认
        /// </summary>
        public EntityList<HandoverConfirmDetail> HandoverConfirmDetailList
        {
            get { return this.GetLazyList(HandoverConfirmDetailListProperty); }
        }
        #endregion

        #region 工程确认 EngineerConfirmDetailList
        /// <summary>
        /// 工程确认
        /// </summary>
        [Label("工程确认")]
        public static readonly ListProperty<EntityList<EngineerConfirmDetail>> EngineerConfirmDetailListProperty = P<EquipRepairBill>.RegisterList(e => e.EngineerConfirmDetailList);

        /// <summary>
        /// 工程确认
        /// </summary>
        public EntityList<EngineerConfirmDetail> EngineerConfirmDetailList
        {
            get { return this.GetLazyList(EngineerConfirmDetailListProperty); }
        }
        #endregion

        #region 维修规程 EquipRepairBillProjectList
        /// <summary>
        /// 维修规程
        /// </summary>
        [Label("维修规程")]
        public static readonly ListProperty<EntityList<EquipRepairBillProject>> EquipRepairBillProjectListProperty = P<EquipRepairBill>.RegisterList(e => e.EquipRepairBillProjectList);
        /// <summary>
        /// 维修规程
        /// </summary>
        public EntityList<EquipRepairBillProject> EquipRepairBillProjectList
        {
            get { return this.GetLazyList(EquipRepairBillProjectListProperty); }
        }
        #endregion

        #region 视图属性

        #region 设备编码 EquipAccountCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipAccountCodeProperty = P<EquipRepairBill>.RegisterView(e => e.EquipAccountCode, p => p.EquipAccount.Code);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipAccountCode
        {
            get { return this.GetProperty(EquipAccountCodeProperty); }
        }
        #endregion

        #region 设备名称 EquipAccountName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipAccountNameProperty = P<EquipRepairBill>.RegisterView(e => e.EquipAccountName, p => p.EquipAccount.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipAccountName
        {
            get { return this.GetProperty(EquipAccountNameProperty); }
        }
        #endregion

        #region 设备型号编码 EquipModelCode
        /// <summary>
        /// 设备型号编码
        /// </summary>
        [Label("设备型号编码")]
        public static readonly Property<string> EquipModelCodeProperty = P<EquipRepairBill>.RegisterView(e => e.EquipModelCode, p => p.EquipAccount.EquipModel.Code);

        /// <summary>
        /// 设备型号编码
        /// </summary>
        public string EquipModelCode
        {
            get { return this.GetProperty(EquipModelCodeProperty); }
        }
        #endregion

        #region 设备型号名称 EquipAccountMode
        /// <summary>
        /// 设备型号名称
        /// </summary>
        [Label("设备型号名称")]
        public static readonly Property<string> EquipAccountModeProperty = P<EquipRepairBill>.RegisterView(e => e.EquipAccountMode, p => p.EquipAccount.EquipModel.Name);

        /// <summary>
        /// 设备型号名称
        /// </summary>
        public string EquipAccountMode
        {
            get { return this.GetProperty(EquipAccountModeProperty); }
        }
        #endregion

        #region 设备型号 EquipModelId
        /// <summary>
        /// 
        /// </summary>
        [Label("设备型号")]
        public static readonly Property<double> EquipModelIdProperty = P<EquipRepairBill>.RegisterView(e => e.EquipModelId, p => p.EquipAccount.EquipModelId);

        /// <summary>
        /// 设备型号
        /// </summary>
        public double EquipModelId
        {
            get { return this.GetProperty(EquipModelIdProperty); }
        }
        #endregion

        #region 设备类型Id EquipTypeId
        /// <summary>
        /// 设备类型Id
        /// </summary>
        [Label("设备类型Id")]
        public static readonly Property<double> EquipTypeIdProperty = P<EquipRepairBill>.RegisterView(e => e.EquipTypeId, p => p.EquipAccount.EquipModel.EquipTypeId);

        /// <summary>
        /// 设备类型Id
        /// </summary>
        public double EquipTypeId
        {
            get { return this.GetProperty(EquipTypeIdProperty); }
        }
        #endregion

        #region 设备类型编码 EquipAccountTypeCode
        /// <summary>
        /// 设备类型编码
        /// </summary>
        [Label("设备类型编码")]
        public static readonly Property<string> EquipAccountTypeCodeProperty = P<EquipRepairBill>.RegisterView(e => e.EquipAccountTypeCode, p => p.EquipAccount.EquipModel.EquipType.TypeCode);

        /// <summary>
        /// 设备类型编码
        /// </summary>
        public string EquipAccountTypeCode
        {
            get { return this.GetProperty(EquipAccountTypeCodeProperty); }
        }
        #endregion


        #region 设备类型 EquipAccountTypeName
        /// <summary>
        /// 设备类型
        /// </summary>
        [Label("设备类型")]
        public static readonly Property<string> EquipAccountTypeNameProperty = P<EquipRepairBill>.RegisterView(e => e.EquipAccountTypeName, p => p.EquipAccount.EquipModel.EquipType.TypeName);

        /// <summary>
        /// 设备类型
        /// </summary>
        public string EquipAccountTypeName
        {
            get { return this.GetProperty(EquipAccountTypeNameProperty); }
        }
        #endregion

        #region 备件编码 SparePartCode
        /// <summary>
        /// 备件编码
        /// </summary>
        [Label("备件编码")]
        public static readonly Property<string> SparePartCodeProperty = P<EquipRepairBill>.RegisterView(e => e.SparePartCode, p => p.SparePart.SparePartCode);

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparePartCode
        {
            get { return this.GetProperty(SparePartCodeProperty); }
        }
        #endregion

        #region 备件名称 SparePartName
        /// <summary>
        /// 备件名称
        /// </summary>
        [Label("备件名称")]
        public static readonly Property<string> SparePartNameProperty = P<EquipRepairBill>.RegisterView(e => e.SparePartName, p => p.SparePart.SparePartName);

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName
        {
            get { return this.GetProperty(SparePartNameProperty); }
        }
        #endregion

        #region 备件类型 SparePartType
        /// <summary>
        /// 备件类型
        /// </summary>
        [Label("备件类型")]
        public static readonly Property<string> SparePartTypeProperty = P<EquipRepairBill>.RegisterView(e => e.SparePartType, p => p.SparePart.ItemCategory.Name);

        /// <summary>
        /// 备件类型
        /// </summary>
        public string SparePartType
        {
            get { return this.GetProperty(SparePartTypeProperty); }
        }
        #endregion

        #region 车间 WorkShopName
        /// <summary>
        /// 车间
        /// </summary>
        [Label("车间")]
        public static readonly Property<string> WorkShopNameProperty = P<EquipRepairBill>.RegisterView(e => e.WorkShopName, p => p.EquipAccount.WorkShop.Name);

        /// <summary>
        /// 车间
        /// </summary>
        public string WorkShopName
        {
            get { return this.GetProperty(WorkShopNameProperty); }
        }
        #endregion

        #region 产线 ResourceName
        /// <summary>
        /// 产线
        /// </summary>
        [Label("产线")]
        public static readonly Property<string> ResourceNameProperty = P<EquipRepairBill>.RegisterView(e => e.ResourceName, p => p.EquipAccount.Resource.Name);

        /// <summary>
        /// 产线
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
        }
        #endregion

        #region 工序 ProcessName
        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        public static readonly Property<string> ProcessNameProperty = P<EquipRepairBill>.RegisterView(e => e.ProcessName, p => p.EquipAccount.Process.Name);

        /// <summary>
        /// 工序
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }
        #endregion

        #region 使用部门Id UseDepartmentId
        /// <summary>
        /// 使用部门Id
        /// </summary>
        [Label("使用部门Id")]
        public static readonly Property<double?> UseDepartmentIdProperty = P<EquipRepairBill>.RegisterView(e => e.UseDepartmentId, p => p.EquipAccount.UseDepartmentId);

        /// <summary>
        /// 使用部门Id
        /// </summary>
        public double? UseDepartmentId
        {
            get { return this.GetProperty(UseDepartmentIdProperty); }
        }
        #endregion

        #region 使用部门 UseDepartment
        /// <summary>
        /// 使用部门
        /// </summary>
        [Label("使用部门")]
        public static readonly Property<string> UseDepartmentProperty = P<EquipRepairBill>.RegisterView(e => e.UseDepartment, p => p.EquipAccount.UseDepartment.Name);

        /// <summary>
        /// 使用部门
        /// </summary>
        public string UseDepartment
        {
            get { return this.GetProperty(UseDepartmentProperty); }
        }
        #endregion

        #region 安装位置 InstallationLocation
        /// <summary>
        /// 安装位置
        /// </summary>
        [Label("安装位置")]
        public static readonly Property<string> InstallationLocationProperty = P<EquipRepairBill>.RegisterView(e => e.InstallationLocation, p => p.EquipAccount.InstallationLocation);

        /// <summary>
        /// 安装位置
        /// </summary>
        public string InstallationLocation
        {
            get { return this.GetProperty(InstallationLocationProperty); }
        }
        #endregion

        #region 供应商名称 SupplierName
        /// <summary>
        /// 供应商名称
        /// </summary>
        [Label("供应商名称")]
        public static readonly Property<string> SupplierNameProperty = P<EquipRepairBill>.RegisterView(e => e.SupplierName, p => p.Supplier.Name);

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName
        {
            get { return this.GetProperty(SupplierNameProperty); }
        }
        #endregion

        #region 设备保修期限 WarrantyPeriod
        /// <summary>
        /// 设备保修期限
        /// </summary>
        [Label("设备保修期限")]
        public static readonly Property<DateTime?> WarrantyPeriodProperty = P<EquipRepairBill>.RegisterView(e => e.WarrantyPeriod, p => p.EquipAccount.WarrantyPeriod);

        /// <summary>
        /// 设备保修期限
        /// </summary>
        public DateTime? WarrantyPeriod
        {
            get { return this.GetProperty(WarrantyPeriodProperty); }
        }
        #endregion

        #region 设备使用部门 UseDepartmentName
        /// <summary>
        /// 设备使用部门
        /// </summary>
        [Label("设备使用部门")]
        public static readonly Property<string> UseDepartmentNameProperty = P<EquipRepairBill>.RegisterView(e => e.UseDepartmentName, p => p.EquipAccount.UseDepartment.Name);

        /// <summary>
        /// 设备使用部门
        /// </summary>
        public string UseDepartmentName
        {
            get { return this.GetProperty(UseDepartmentNameProperty); }
        }
        #endregion

        #region 报修人姓名 ApplyRepairEmployeeName
        /// <summary>
        /// 报修人姓名
        /// </summary>
        [Label("报修人姓名")]
        public static readonly Property<string> ApplyRepairEmployeeNameProperty = P<EquipRepairBill>.RegisterView(e => e.ApplyRepairEmployeeName, p => p.ApplyRepairEmployee.Name);

        /// <summary>
        /// 报修人姓名
        /// </summary>
        public string ApplyRepairEmployeeName
        {
            get { return this.GetProperty(ApplyRepairEmployeeNameProperty); }
        }
        #endregion

        #endregion

        #region 故障信息 FaultInfo

        #region 供应商 Supplier
        /// <summary>
        /// 供应商Id
        /// </summary>
        [Label("供应商")]
        public static readonly IRefIdProperty SupplierIdProperty =
            P<EquipRepairBill>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

        /// <summary>
        /// 供应商Id
        /// </summary>
        public double? SupplierId
        {
            get { return (double?)this.GetRefNullableId(SupplierIdProperty); }
            set { this.SetRefNullableId(SupplierIdProperty, value); }
        }

        /// <summary>
        /// 供应商
        /// </summary>
        public static readonly RefEntityProperty<Supplier> SupplierProperty =
            P<EquipRepairBill>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier Supplier
        {
            get { return this.GetRefEntity(SupplierProperty); }
            set { this.SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 送修方式 SendRepairWay
        /// <summary>
        /// 送修方式
        /// </summary>
        [Label("送修方式")]
        public static readonly Property<SendRepairWay?> SendRepairWayProperty = P<EquipRepairBill>.Register(e => e.SendRepairWay);

        /// <summary>
        /// 送修方式
        /// </summary>
        public SendRepairWay? SendRepairWay
        {
            get { return this.GetProperty(SendRepairWayProperty); }
            set { this.SetProperty(SendRepairWayProperty, value); }
        }
        #endregion

        #region 快递/出厂单号 DeliveryNo
        /// <summary>
        /// 快递/出厂单号
        /// </summary>
        [Label("快递/出厂单号")]
        public static readonly Property<string> DeliveryNoProperty = P<EquipRepairBill>.Register(e => e.DeliveryNo);

        /// <summary>
        /// 快递/出厂单号
        /// </summary>
        public string DeliveryNo
        {
            get { return this.GetProperty(DeliveryNoProperty); }
            set { this.SetProperty(DeliveryNoProperty, value); }
        }
        #endregion

        #region 联系人 ContactPerson
        /// <summary>
        /// 联系人
        /// </summary>
        [Label("联系人")]
        public static readonly Property<string> ContactPersonProperty = P<EquipRepairBill>.Register(e => e.ContactPerson);

        /// <summary>
        /// 联系人
        /// </summary>
        public string ContactPerson
        {
            get { return this.GetProperty(ContactPersonProperty); }
            set { this.SetProperty(ContactPersonProperty, value); }
        }
        #endregion

        #region 联系电话 ContactPhone
        /// <summary>
        /// 联系电话
        /// </summary>
        [Label("联系电话")]
        public static readonly Property<string> ContactPhoneProperty = P<EquipRepairBill>.Register(e => e.ContactPhone);

        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactPhone
        {
            get { return this.GetProperty(ContactPhoneProperty); }
            set { this.SetProperty(ContactPhoneProperty, value); }
        }
        #endregion

        #region 外修时间 SendRepairDate
        /// <summary>
        /// 外修时间
        /// </summary>
        [Label("外修时间")]
        public static readonly Property<DateTime?> SendRepairDateProperty = P<EquipRepairBill>.Register(e => e.SendRepairDate);

        /// <summary>
        /// 外修时间
        /// </summary>
        public DateTime? SendRepairDate
        {
            get { return this.GetProperty(SendRepairDateProperty); }
            set { this.SetProperty(SendRepairDateProperty, value); }
        }
        #endregion

        #region 预计返厂时间 PredictBackDate
        /// <summary>
        /// 预计返厂时间
        /// </summary>
        [Label("预计返厂时间")]
        public static readonly Property<DateTime?> PredictBackDateProperty = P<EquipRepairBill>.Register(e => e.PredictBackDate);

        /// <summary>
        /// 预计返厂时间
        /// </summary>
        public DateTime? PredictBackDate
        {
            get { return this.GetProperty(PredictBackDateProperty); }
            set { this.SetProperty(PredictBackDateProperty, value); }
        }
        #endregion

        #region 设备保修状态 EquipWarrantyState
        /// <summary>
        /// 设备保修状态
        /// </summary>
        [Label("设备保修状态")]
        public static readonly Property<string> EquipWarrantyStateProperty = P<EquipRepairBill>.Register(e => e.EquipWarrantyState);

        /// <summary>
        /// 设备保修状态
        /// </summary>
        public string EquipWarrantyState
        {
            get { return this.GetProperty(EquipWarrantyStateProperty); }
            set { this.SetProperty(EquipWarrantyStateProperty, value); }
        }
        #endregion

        #endregion

        #region 维修报告 RepairRepoter

        #region 委外维修报告 OutsourcedMaintenanceReport
        /// <summary>
        /// 委外维修报告
        /// </summary>
        [Label("委外维修报告")]
        [MaxLength(1000)]
        public static readonly Property<string> OutsourcedMaintenanceReportProperty = P<EquipRepairBill>.Register(e => e.OutsourcedMaintenanceReport);

        /// <summary>
        /// 委外维修报告
        /// </summary>
        public string OutsourcedMaintenanceReport
        {
            get { return this.GetProperty(OutsourcedMaintenanceReportProperty); }
            set { this.SetProperty(OutsourcedMaintenanceReportProperty, value); }
        }
        #endregion

        #region 故障描述 FaultDescription
        /// <summary>
        /// 故障描述Id
        /// </summary>
        [Label("故障描述")]
        public static readonly IRefIdProperty FaultDescriptionIdProperty =
            P<EquipRepairBill>.RegisterRefId(e => e.FaultDescriptionId, ReferenceType.Normal);

        /// <summary>
        /// 故障描述Id
        /// </summary>
        public double? FaultDescriptionId
        {
            get { return (double?)this.GetRefNullableId(FaultDescriptionIdProperty); }
            set { this.SetRefNullableId(FaultDescriptionIdProperty, value); }
        }

        /// <summary>
        /// 故障描述
        /// </summary>
        public static readonly RefEntityProperty<DeviceAbnormal> FaultDescriptionProperty =
            P<EquipRepairBill>.RegisterRef(e => e.FaultDescription, FaultDescriptionIdProperty);

        /// <summary>
        /// 故障描述
        /// </summary>
        public DeviceAbnormal FaultDescription
        {
            get { return this.GetRefEntity(FaultDescriptionProperty); }
            set { this.SetRefEntity(FaultDescriptionProperty, value); }
        }
        #endregion

        #region 故障描述（备注） FaultDescriptionRemark
        /// <summary>
        /// 故障描述（备注）
        /// </summary>
        [Label("故障描述（备注）")]
        [MaxLength(4000)]
        public static readonly Property<string> FaultDescriptionRemarkProperty = P<EquipRepairBill>.Register(e => e.FaultDescriptionRemark);

        /// <summary>
        /// 故障描述（备注）
        /// </summary>
        public string FaultDescriptionRemark
        {
            get { return this.GetProperty(FaultDescriptionRemarkProperty); }
            set { this.SetProperty(FaultDescriptionRemarkProperty, value); }
        }
        #endregion

        #region 故障原因 FaultReason
        /// <summary>
        /// 故障原因
        /// </summary>
        [Label("故障原因")]
        public static readonly Property<string> FaultReasonProperty = P<EquipRepairBill>.Register(e => e.FaultReason);

        /// <summary>
        /// 故障原因
        /// </summary>
        public string FaultReason
        {
            get { return this.GetProperty(FaultReasonProperty); }
            set { this.SetProperty(FaultReasonProperty, value); }
        }
        #endregion

        #region 故障部位 FaultPart
        /// <summary>
        /// 故障部位
        /// </summary>
        [Label("故障部位")]
        public static readonly Property<string> FaultPartProperty = P<EquipRepairBill>.Register(e => e.FaultPart);

        /// <summary>
        /// 故障部位
        /// </summary>
        public string FaultPart
        {
            get { return this.GetProperty(FaultPartProperty); }
            set { this.SetProperty(FaultPartProperty, value); }
        }
        #endregion

        #region 故障类别 FaultCategory
        /// <summary>
        /// 故障类别Id
        /// </summary>
        [Label("故障类别")]
        public static readonly IRefIdProperty FaultCategoryIdProperty =
            P<EquipRepairBill>.RegisterRefId(e => e.FaultCategoryId, ReferenceType.Normal);

        /// <summary>
        /// 故障类别Id
        /// </summary>
        public double? FaultCategoryId
        {
            get { return (double?)this.GetRefNullableId(FaultCategoryIdProperty); }
            set { this.SetRefNullableId(FaultCategoryIdProperty, value); }
        }

        /// <summary>
        /// 故障类别
        /// </summary>
        public static readonly RefEntityProperty<EquipLargeFault> FaultCategoryProperty =
            P<EquipRepairBill>.RegisterRef(e => e.FaultCategory, FaultCategoryIdProperty);

        /// <summary>
        /// 故障类别
        /// </summary>
        public EquipLargeFault FaultCategory
        {
            get { return this.GetRefEntity(FaultCategoryProperty); }
            set { this.SetRefEntity(FaultCategoryProperty, value); }
        }
        #endregion

        #region 故障等级 FaultLevel
        /// <summary>
        /// 故障等级
        /// </summary>
        [Label("故障等级")]
        public static readonly Property<FaultLevel?> FaultLevelProperty = P<EquipRepairBill>.Register(e => e.FaultLevel);

        /// <summary>
        /// 故障等级
        /// </summary>
        public FaultLevel? FaultLevel
        {
            get { return this.GetProperty(FaultLevelProperty); }
            set { this.SetProperty(FaultLevelProperty, value); }
        }
        #endregion

        #region 维修类别 RepairCategory
        /// <summary>
        /// 维修类别
        /// </summary>
        [Label("维修类别")]
        public static readonly Property<RepairCategory?> RepairCategoryProperty = P<EquipRepairBill>.Register(e => e.RepairCategory);

        /// <summary>
        /// 维修类别
        /// </summary>
        public RepairCategory? RepairCategory
        {
            get { return this.GetProperty(RepairCategoryProperty); }
            set { this.SetProperty(RepairCategoryProperty, value); }
        }
        #endregion

        #region 维修等级 RepairLevel
        /// <summary>
        /// 维修等级
        /// </summary>
        [Label("维修等级")]
        public static readonly Property<RepairLevel?> RepairLevelProperty = P<EquipRepairBill>.Register(e => e.RepairLevel);

        /// <summary>
        /// 维修等级
        /// </summary>
        public RepairLevel? RepairLevel
        {
            get { return this.GetProperty(RepairLevelProperty); }
            set { this.SetProperty(RepairLevelProperty, value); }
        }
        #endregion

        #region 维修费用 RepairCosts
        /// <summary>
        /// 维修费用
        /// </summary>
        [Label("维修费用")]
        public static readonly Property<decimal?> RepairCostsProperty = P<EquipRepairBill>.Register(e => e.RepairCosts);

        /// <summary>
        /// 维修费用
        /// </summary>
        public decimal? RepairCosts
        {
            get { return this.GetProperty(RepairCostsProperty); }
            set { this.SetProperty(RepairCostsProperty, value); }
        }
        #endregion

        #region 停机维修 RepairDowntime
        /// <summary>
        /// 停机维修
        /// </summary>
        [Label("停机维修")]
        public static readonly Property<bool?> RepairDowntimeProperty = P<EquipRepairBill>.Register(e => e.RepairDowntime);

        /// <summary>
        /// 停机维修
        /// </summary>
        public bool? RepairDowntime
        {
            get { return this.GetProperty(RepairDowntimeProperty); }
            set { this.SetProperty(RepairDowntimeProperty, value); }
        }
        #endregion

        #region 维修方法 RepairMethod
        /// <summary>
        /// 维修方法
        /// </summary>
        [Label("维修方法")]
        public static readonly Property<string> RepairMethodProperty = P<EquipRepairBill>.Register(e => e.RepairMethod);

        /// <summary>
        /// 维修方法
        /// </summary>
        public string RepairMethod
        {
            get { return this.GetProperty(RepairMethodProperty); }
            set { this.SetProperty(RepairMethodProperty, value); }
        }
        #endregion

        #region 预防建议 PreventionAdvice
        /// <summary>
        /// 预防建议
        /// </summary>
        [Label("预防建议")]
        public static readonly Property<string> PreventionAdviceProperty = P<EquipRepairBill>.Register(e => e.PreventionAdvice);

        /// <summary>
        /// 预防建议
        /// </summary>
        public string PreventionAdvice
        {
            get { return this.GetProperty(PreventionAdviceProperty); }
            set { this.SetProperty(PreventionAdviceProperty, value); }
        }
        #endregion

        #region 备件费用 SparePartsCost
        /// <summary>
        /// 备件费用
        /// </summary>
        [Label("备件费用")]
        public static readonly Property<decimal> SparePartsCostProperty = P<EquipRepairBill>.Register(e => e.SparePartsCost);

        /// <summary>
        /// 备件费用
        /// </summary>
        public decimal SparePartsCost
        {
            get { return GetProperty(SparePartsCostProperty); }
            set { SetProperty(SparePartsCostProperty, value); }
        }
        #endregion

        #region 维修工时 RepairHours
        /// <summary>
        /// 维修工时
        /// </summary>
        [Label("维修工时")]
        public static readonly Property<decimal> RepairHoursProperty = P<EquipRepairBill>.Register(e => e.RepairHours);

        /// <summary>
        /// 维修工时
        /// </summary>
        public decimal RepairHours
        {
            get { return GetProperty(RepairHoursProperty); }
            set { SetProperty(RepairHoursProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 故障描述编码 FaultDescriptionCode
        /// <summary>
        /// 故障描述编码
        /// </summary>
        [Label("故障描述编码")]
        public static readonly Property<string> FaultDescriptionCodeProperty = P<EquipRepairBill>.RegisterView(e => e.FaultDescriptionCode, p => p.FaultDescription.Code);

        /// <summary>
        /// 故障描述编码
        /// </summary>
        public string FaultDescriptionCode
        {
            get { return this.GetProperty(FaultDescriptionCodeProperty); }
        }
        #endregion

        #region 故障描述名称 FaultDescriptionDesc
        /// <summary>
        /// 故障描述名称
        /// </summary>
        [Label("故障描述名称")]
        public static readonly Property<string> FaultDescriptionDescProperty = P<EquipRepairBill>.RegisterView(e => e.FaultDescriptionDesc, p => p.FaultDescription.Description);

        /// <summary>
        /// 故障描述名称
        /// </summary>
        public string FaultDescriptionDesc
        {
            get { return this.GetProperty(FaultDescriptionDescProperty); }
        }
        #endregion


        #region 故障类别编码 FaultCategoryCode
        /// <summary>
        /// 故障类别编码
        /// </summary>
        [Label("故障类别编码")]
        public static readonly Property<string> FaultCategoryCodeProperty = P<EquipRepairBill>.RegisterView(e => e.FaultCategoryCode, p => p.FaultCategory.Code);

        /// <summary>
        /// 故障类别编码
        /// </summary>
        public string FaultCategoryCode
        {
            get { return this.GetProperty(FaultCategoryCodeProperty); }
        }
        #endregion

        #region 故障类别名称 FaultCategoryName
        /// <summary>
        /// 故障类别名称
        /// </summary>
        [Label("故障类别名称")]
        public static readonly Property<string> FaultCategoryNameProperty = P<EquipRepairBill>.RegisterView(e => e.FaultCategoryName, p => p.FaultCategory.Name);

        /// <summary>
        /// 故障类别名称
        /// </summary>
        public string FaultCategoryName
        {
            get { return this.GetProperty(FaultCategoryNameProperty); }
        }
        #endregion
        #endregion


        #endregion

        #region 交机确认 HandoverConfirm

        #region 确认人员 HandoverConfirmEmployee
        /// <summary>
        /// 确认人员Id
        /// </summary>
        [Label("确认人员")]
        public static readonly IRefIdProperty HandoverConfirmEmployeeIdProperty =
            P<EquipRepairBill>.RegisterRefId(e => e.HandoverConfirmEmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 确认人员Id
        /// </summary>
        public double? HandoverConfirmEmployeeId
        {
            get { return (double?)this.GetRefNullableId(HandoverConfirmEmployeeIdProperty); }
            set { this.SetRefNullableId(HandoverConfirmEmployeeIdProperty, value); }
        }

        /// <summary>
        /// 确认人员
        /// </summary>
        public static readonly RefEntityProperty<Employee> HandoverConfirmEmployeeProperty =
            P<EquipRepairBill>.RegisterRef(e => e.HandoverConfirmEmployee, HandoverConfirmEmployeeIdProperty);

        /// <summary>
        /// 确认人员
        /// </summary>
        public Employee HandoverConfirmEmployee
        {
            get { return this.GetRefEntity(HandoverConfirmEmployeeProperty); }
            set { this.SetRefEntity(HandoverConfirmEmployeeProperty, value); }
        }
        #endregion

        #region 异常情况 HandoverConfirmAbnormal
        /// <summary>
        /// 异常情况
        /// </summary>
        [Label("异常情况")]
        public static readonly Property<HandoverConfirmAbnormal?> HandoverConfirmAbnormalProperty = P<EquipRepairBill>.Register(e => e.HandoverConfirmAbnormal);

        /// <summary>
        /// 异常情况
        /// </summary>
        public HandoverConfirmAbnormal? HandoverConfirmAbnormal
        {
            get { return this.GetProperty(HandoverConfirmAbnormalProperty); }
            set { this.SetProperty(HandoverConfirmAbnormalProperty, value); }
        }
        #endregion

        #region 故障现象 HandoverDeviceAbnormal
        /// <summary>
        /// 故障现象Id
        /// </summary>
        [Label("故障现象")]
        public static readonly IRefIdProperty HandoverDeviceAbnormalIdProperty =
            P<EquipRepairBill>.RegisterRefId(e => e.HandoverDeviceAbnormalId, ReferenceType.Normal);

        /// <summary>
        /// 故障现象Id
        /// </summary>
        public double? HandoverDeviceAbnormalId
        {
            get { return (double?)this.GetRefId(HandoverDeviceAbnormalIdProperty); }
            set { this.SetRefId(HandoverDeviceAbnormalIdProperty, value); }
        }

        /// <summary>
        /// 故障现象
        /// </summary>
        public static readonly RefEntityProperty<DeviceAbnormal> HandoverDeviceAbnormalProperty =
            P<EquipRepairBill>.RegisterRef(e => e.HandoverDeviceAbnormal, HandoverDeviceAbnormalIdProperty);

        /// <summary>
        /// 故障现象
        /// </summary>
        public DeviceAbnormal HandoverDeviceAbnormal
        {
            get { return this.GetRefEntity(HandoverDeviceAbnormalProperty); }
            set { this.SetRefEntity(HandoverDeviceAbnormalProperty, value); }
        }
        #endregion

        #region 故障现象(备注) HandoverDeviceAbnormalRem
        /// <summary>
        /// 故障现象(备注)
        /// </summary>
        [Label("故障现象(备注)")]
        public static readonly Property<string> HandoverDeviceAbnormalRemProperty = P<EquipRepairBill>.Register(e => e.HandoverDeviceAbnormalRem);

        /// <summary>
        /// 故障现象(备注)
        /// </summary>
        public string HandoverDeviceAbnormalRem
        {
            get { return this.GetProperty(HandoverDeviceAbnormalRemProperty); }
            set { this.SetProperty(HandoverDeviceAbnormalRemProperty, value); }
        }
        #endregion

        #region 文件路径 HandoverAttachment
        /// <summary>
        /// 文件路径
        /// </summary>
        [Label("文件路径")]
        [MaxLength(4000)]
        public static readonly Property<string> HandoverAttachmentProperty = P<EquipRepairBill>.Register(e => e.HandoverAttachment);

        /// <summary>
        /// 文件路径
        /// </summary>
        public string HandoverAttachment
        {
            get { return this.GetProperty(HandoverAttachmentProperty); }
            set { this.SetProperty(HandoverAttachmentProperty, value); }
        }
        #endregion

        #endregion

        #region 工程确认 EngineerConfirm

        #region 响应时间 RespondTime
        /// <summary>
        /// 响应时间
        /// </summary>
        [Label("维修响应时间(H)")]
        public static readonly Property<decimal?> RespondTimeProperty = P<EquipRepairBill>.Register(e => e.RespondTime);

        /// <summary>
        /// 响应时间
        /// </summary>
        public decimal? RespondTime
        {
            get { return this.GetProperty(RespondTimeProperty); }
            set { this.SetProperty(RespondTimeProperty, value); }
        }
        #endregion

        #region 执行时间 ExecuteTime
        /// <summary>
        /// 执行时间
        /// </summary>
        [Label("维修执行时间(H)")]
        public static readonly Property<decimal?> ExecuteTimeProperty = P<EquipRepairBill>.Register(e => e.ExecuteTime);

        /// <summary>
        /// 执行时间
        /// </summary>
        public decimal? ExecuteTime
        {
            get { return this.GetProperty(ExecuteTimeProperty); }
            set { this.SetProperty(ExecuteTimeProperty, value); }
        }
        #endregion

        #region 维修总工时 RepairTotalTime
        /// <summary>
        /// 维修总工时
        /// </summary>
        [Label("维修总工时(H)")]
        public static readonly Property<decimal?> RepairTotalTimeProperty = P<EquipRepairBill>.Register(e => e.RepairTotalTime);

        /// <summary>
        /// 维修总工时
        /// </summary>
        public decimal? RepairTotalTime
        {
            get { return this.GetProperty(RepairTotalTimeProperty); }
            set { this.SetProperty(RepairTotalTimeProperty, value); }
        }
        #endregion

        #endregion

        #region 项目 Project
        /// <summary>
        /// 项目Id
        /// </summary>
        [Label("项目")]
        public static readonly IRefIdProperty ProjectIdProperty = P<EquipRepairBill>.RegisterRefId(e => e.ProjectId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Project> ProjectProperty = P<EquipRepairBill>.RegisterRef(e => e.Project, ProjectIdProperty);

        /// <summary>
        /// 项目
        /// </summary>
        public Project Project
        {
            get { return GetRefEntity(ProjectProperty); }
            set { SetRefEntity(ProjectProperty, value); }
        }
        #endregion

        #region 项目编码 ProjectCode
        /// <summary>
        /// 项目编码
        /// </summary>
        [Label("项目编码")]
        public static readonly Property<string> ProjectCodeProperty = P<EquipRepairBill>.RegisterView(e => e.ProjectCode, p => p.Project.Code);

        /// <summary>
        /// 项目编码
        /// </summary>
        public string ProjectCode
        {
            get { return this.GetProperty(ProjectCodeProperty); }
        }
        #endregion

        #region 项目事项 ProjectKeyItem
        /// <summary>
        /// 项目事项Id
        /// </summary>
        [Label("项目事项")]
        public static readonly IRefIdProperty ProjectKeyItemIdProperty = P<EquipRepairBill>.RegisterRefId(e => e.ProjectKeyItemId, ReferenceType.Normal);

        /// <summary>
        /// 项目事项Id
        /// </summary>
        public double? ProjectKeyItemId
        {
            get { return (double?)GetRefNullableId(ProjectKeyItemIdProperty); }
            set { SetRefNullableId(ProjectKeyItemIdProperty, value); }
        }

        /// <summary>
        /// 项目事项
        /// </summary>
        public static readonly RefEntityProperty<ProjectKeyItem> ProjectKeyItemProperty = P<EquipRepairBill>.RegisterRef(e => e.ProjectKeyItem, ProjectKeyItemIdProperty);

        /// <summary>
        /// 项目事项
        /// </summary>
        public ProjectKeyItem ProjectKeyItem
        {
            get { return GetRefEntity(ProjectKeyItemProperty); }
            set { SetRefEntity(ProjectKeyItemProperty, value); }
        }
        #endregion

        #region 项目事项描述 ProjectKeyItemDesc
        /// <summary>
        /// 项目事项描述
        /// </summary>
        [Label("项目事项描述")]
        public static readonly Property<string> ProjectKeyItemDescProperty = P<EquipRepairBill>.RegisterView(e => e.ProjectKeyItemDesc, p => p.ProjectKeyItem.Description);

        /// <summary>
        /// 项目事项描述
        /// </summary>
        public string ProjectKeyItemDesc
        {
            get { return this.GetProperty(ProjectKeyItemDescProperty); }
        }
        #endregion

    }

    /// <summary>
    /// 设备维修单 实体配置
    /// </summary>
    internal class EquipRepairBillConfig : EntityConfig<EquipRepairBill>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(EquipRepairBill.RepairEmployeeIdsProperty, new StringLengthRangeRule() { Max = 2000 });
            rules.AddRule(EquipRepairBill.RepairEmployeesProperty, new StringLengthRangeRule() { Max = 2000 });
            rules.AddRule(EquipRepairBill.OutsourcedMaintenanceReportProperty, new StringLengthRangeRule() { Max = 2000 });
            rules.AddRule(EquipRepairBill.RepairMethodProperty, new StringLengthRangeRule() { Max = 2000 });
            rules.AddRule(EquipRepairBill.PreventionAdviceProperty, new StringLengthRangeRule() { Max = 2000 });
            rules.AddRule(EquipRepairBill.FaultDescriptionRemarkProperty, new StringLengthRangeRule() { Max = 2000 });
        }

        /// <summary>
        /// 配置元数据
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_REPAIR").MapAllProperties();
            Meta.Property(EquipRepairBill.RepairEmployeeIdsProperty).ColumnMeta.HasLength(4000);
            Meta.Property(EquipRepairBill.RepairEmployeesProperty).ColumnMeta.HasLength(4000);
            Meta.Property(EquipRepairBill.OutsourcedMaintenanceReportProperty).ColumnMeta.HasLength(4000);
            Meta.Property(EquipRepairBill.RepairMethodProperty).ColumnMeta.HasLength(4000);
            Meta.Property(EquipRepairBill.PreventionAdviceProperty).ColumnMeta.HasLength(4000);
            Meta.Property(EquipRepairBill.HandoverAttachmentProperty).ColumnMeta.HasLength(4000);
            Meta.Property(EquipRepairBill.FaultDescriptionRemarkProperty).ColumnMeta.HasLength(4000);
            Meta.Property(EquipRepairBill.EquipWarrantyStateProperty).DontMapColumn();
            Meta.Property(EquipRepairBill.RespondTimeProperty).DontMapColumn();
            Meta.Property(EquipRepairBill.ExecuteTimeProperty).DontMapColumn();
            Meta.Property(EquipRepairBill.RepairTotalTimeProperty).DontMapColumn();

            Meta.Property(EquipRepairBill.OriginalRepairMasterIdProperty).DontMapColumn();
            Meta.Property(EquipRepairBill.OriginalRepairMasterProperty).DontMapColumn();
            Meta.Property(EquipRepairBill.OriginalRepairEmployeesProperty).DontMapColumn();
            Meta.Property(EquipRepairBill.OriginalRepairEmployeeIdsProperty).DontMapColumn();

            Meta.EnablePhantoms();
        }
    }
}
