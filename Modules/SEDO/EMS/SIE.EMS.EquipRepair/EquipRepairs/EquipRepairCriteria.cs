using SIE.Domain;
using SIE.EMS.EquipRepair.Controller;
using SIE.EMS.EquipRepairs.Enums;
using SIE.EMS.SpareParts;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Tech.Processs;
using System;

namespace SIE.EMS.EquipRepair.EquipRepairs
{
    /// <summary>
    /// 设备维修单查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("设备维修单查询实体")]
    public partial class EquipRepairCriteria : Criteria
    {
        #region 设备编码 EquipAccount
        /// <summary>
        /// 设备编码Id
        /// </summary>
        [Label("设备编码")]
        public static readonly IRefIdProperty EquipAccountIdProperty =
            P<EquipRepairCriteria>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备编码Id
        /// </summary>
        public double? EquipAccountId
        {
            get { return (double?)this.GetRefNullableId(EquipAccountIdProperty); }
            set { this.SetRefNullableId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备编码
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty =
            P<EquipRepairCriteria>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备编码
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return this.GetRefEntity(EquipAccountProperty); }
            set { this.SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 备件编码 SparePart
        /// <summary>
        /// 备件编码Id
        /// </summary>
        [Label("备件编码")]
        public static readonly IRefIdProperty SparePartIdProperty =
            P<EquipRepairCriteria>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

        /// <summary>
        /// 备件编码Id
        /// </summary>
        public double? SparePartId
        {
            get { return (double?)this.GetRefNullableId(SparePartIdProperty); }
            set { this.SetRefNullableId(SparePartIdProperty, value); }
        }

        /// <summary>
        /// 备件编码
        /// </summary>
        public static readonly RefEntityProperty<SparePart> SparePartProperty =
            P<EquipRepairCriteria>.RegisterRef(e => e.SparePart, SparePartIdProperty);

        /// <summary>
        /// 备件编码
        /// </summary>
        public SparePart SparePart
        {
            get { return this.GetRefEntity(SparePartProperty); }
            set { this.SetRefEntity(SparePartProperty, value); }
        }
        #endregion

        #region 设备/备件名称 EquipOrSparePartName
        /// <summary>
        /// 设备/备件名称
        /// </summary>
        [Label("设备/备件名称")]
        public static readonly Property<string> EquipOrSparePartNameProperty = P<EquipRepairCriteria>.Register(e => e.EquipOrSparePartName);

        /// <summary>
        /// 设备/备件名称
        /// </summary>
        public string EquipOrSparePartName
        {
            get { return this.GetProperty(EquipOrSparePartNameProperty); }
            set { this.SetProperty(EquipOrSparePartNameProperty, value); }
        }
        #endregion

        #region 设备类型 EquipType
        /// <summary>
        /// 设备类型Id
        /// </summary>
        [Label("设备类型")]
        public static readonly IRefIdProperty EquipTypeIdProperty =
            P<EquipRepairCriteria>.RegisterRefId(e => e.EquipTypeId, ReferenceType.Normal);

        /// <summary>
        /// 设备类型Id
        /// </summary>
        public double? EquipTypeId
        {
            get { return (double?)this.GetRefNullableId(EquipTypeIdProperty); }
            set { this.SetRefNullableId(EquipTypeIdProperty, value); }
        }

        /// <summary>
        /// 设备类型
        /// </summary>
        public static readonly RefEntityProperty<EquipType> EquipTypeProperty =
            P<EquipRepairCriteria>.RegisterRef(e => e.EquipType, EquipTypeIdProperty);

        /// <summary>
        /// 设备类型
        /// </summary>
        public EquipType EquipType
        {
            get { return this.GetRefEntity(EquipTypeProperty); }
            set { this.SetRefEntity(EquipTypeProperty, value); }
        }
        #endregion

        #region 设备型号 EquipModel
        /// <summary>
        /// 设备型号Id
        /// </summary>
        [Label("设备型号")]
        public static readonly IRefIdProperty EquipModelIdProperty =
            P<EquipRepairCriteria>.RegisterRefId(e => e.EquipModelId, ReferenceType.Normal);

        /// <summary>
        /// 设备型号Id
        /// </summary>
        public double? EquipModelId
        {
            get { return (double?)this.GetRefNullableId(EquipModelIdProperty); }
            set { this.SetRefNullableId(EquipModelIdProperty, value); }
        }

        /// <summary>
        /// 设备型号
        /// </summary>
        public static readonly RefEntityProperty<EquipModel> EquipModelProperty =
            P<EquipRepairCriteria>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

        /// <summary>
        /// 设备型号
        /// </summary>
        public EquipModel EquipModel
        {
            get { return this.GetRefEntity(EquipModelProperty); }
            set { this.SetRefEntity(EquipModelProperty, value); }
        }
        #endregion

        #region 维修单号 RepairNo
        /// <summary>
        /// 维修单号
        /// </summary>
        [Label("维修单号")]
        public static readonly Property<string> RepairNoProperty = P<EquipRepairCriteria>.Register(e => e.RepairNo);

        /// <summary>
        /// 维修单号
        /// </summary>
        public string RepairNo
        {
            get { return this.GetProperty(RepairNoProperty); }
            set { this.SetProperty(RepairNoProperty, value); }
        }
        #endregion

        #region 报修人 ApplyRepairEmployee
        /// <summary>
        /// 报修人Id
        /// </summary>
        [Label("报修人")]
        public static readonly IRefIdProperty ApplyRepairEmployeeIdProperty =
            P<EquipRepairCriteria>.RegisterRefId(e => e.ApplyRepairEmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 报修人Id
        /// </summary>
        public double? ApplyRepairEmployeeId
        {
            get { return (double?)this.GetRefNullableId(ApplyRepairEmployeeIdProperty); }
            set { this.SetRefNullableId(ApplyRepairEmployeeIdProperty, value); }
        }

        /// <summary>
        /// 报修人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ApplyRepairEmployeeProperty =
            P<EquipRepairCriteria>.RegisterRef(e => e.ApplyRepairEmployee, ApplyRepairEmployeeIdProperty);

        /// <summary>
        /// 报修人
        /// </summary>
        public Employee ApplyRepairEmployee
        {
            get { return this.GetRefEntity(ApplyRepairEmployeeProperty); }
            set { this.SetRefEntity(ApplyRepairEmployeeProperty, value); }
        }
        #endregion

        #region 维修责任人 RepairMaster
        /// <summary>
        /// 维修责任人Id
        /// </summary>
        [Label("维修责任人")]
        public static readonly IRefIdProperty RepairMasterIdProperty =
            P<EquipRepairCriteria>.RegisterRefId(e => e.RepairMasterId, ReferenceType.Normal);

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
            P<EquipRepairCriteria>.RegisterRef(e => e.RepairMaster, RepairMasterIdProperty);

        /// <summary>
        /// 维修责任人
        /// </summary>
        public Employee RepairMaster
        {
            get { return this.GetRefEntity(RepairMasterProperty); }
            set { this.SetRefEntity(RepairMasterProperty, value); }
        }
        #endregion

        #region 维修人员 RepairEmployee
        /// <summary>
        /// 维修人员Id
        /// </summary>
        [Label("其他维修人员")]
        public static readonly IRefIdProperty RepairEmployeeIdProperty =
            P<EquipRepairCriteria>.RegisterRefId(e => e.RepairEmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 维修人员Id
        /// </summary>
        public double? RepairEmployeeId
        {
            get { return (double?)this.GetRefNullableId(RepairEmployeeIdProperty); }
            set { this.SetRefNullableId(RepairEmployeeIdProperty, value); }
        }

        /// <summary>
        /// 维修人员
        /// </summary>
        public static readonly RefEntityProperty<Employee> RepairEmployeeProperty =
            P<EquipRepairCriteria>.RegisterRef(e => e.RepairEmployee, RepairEmployeeIdProperty);

        /// <summary>
        /// 维修人员
        /// </summary>
        public Employee RepairEmployee
        {
            get { return this.GetRefEntity(RepairEmployeeProperty); }
            set { this.SetRefEntity(RepairEmployeeProperty, value); }
        }
        #endregion

        #region 维修类型 RepairType
        /// <summary>
        /// 维修类型
        /// </summary>
        [Label("维修类型")]
        public static readonly Property<EquipRepairType?> RepairTypeProperty = P<EquipRepairCriteria>.Register(e => e.RepairType);

        /// <summary>
        /// 维修类型
        /// </summary>
        public EquipRepairType? RepairType
        {
            get { return this.GetProperty(RepairTypeProperty); }
            set { this.SetProperty(RepairTypeProperty, value); }
        }
        #endregion

        #region 维修状态 RepairState
        /// <summary>
        /// 维修状态
        /// </summary>
        [Label("维修状态")]
        public static readonly Property<EquipRepairState?> RepairStateProperty = P<EquipRepairCriteria>.Register(e => e.RepairState);

        /// <summary>
        /// 维修状态
        /// </summary>
        public EquipRepairState? RepairState
        {
            get { return this.GetProperty(RepairStateProperty); }
            set { this.SetProperty(RepairStateProperty, value); }
        }
        #endregion

        #region 派工类型 RepairWay
        /// <summary>
        /// 派工类型
        /// </summary>
        [Label("派工类型")]
        public static readonly Property<EquipRepairWay?> RepairWayProperty = P<EquipRepairCriteria>.Register(e => e.RepairWay);

        /// <summary>
        /// 派工类型
        /// </summary>
        public EquipRepairWay? RepairWay
        {
            get { return this.GetProperty(RepairWayProperty); }
            set { this.SetProperty(RepairWayProperty, value); }
        }
        #endregion

        #region 车间 Workshop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkshopIdProperty = P<EquipRepairCriteria>.RegisterRefId(e => e.WorkshopId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> WorkshopProperty = P<EquipRepairCriteria>.RegisterRef(e => e.Workshop, WorkshopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise Workshop
        {
            get { return GetRefEntity(WorkshopProperty); }
            set { SetRefEntity(WorkshopProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<EquipRepairCriteria>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)GetRefNullableId(ProcessIdProperty); }
            set { SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<EquipRepairCriteria>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 报修时间 ApplyRepairDate
        /// <summary>
        /// 报修时间
        /// </summary>
        [Label("报修时间")]
        public static readonly Property<DateRange> ApplyRepairDateProperty = P<EquipRepairCriteria>.Register(e => e.ApplyRepairDate);

        /// <summary>
        /// 报修时间
        /// </summary>
        public DateRange ApplyRepairDate
        {
            get { return this.GetProperty(ApplyRepairDateProperty); }
            set { this.SetProperty(ApplyRepairDateProperty, value); }
        }
        #endregion

        #region 未完成 IsToFinish
        /// <summary>
        /// 未完成
        /// </summary>
        [Label("未完成")]
        public static readonly Property<bool> IsToFinishProperty = P<EquipRepairCriteria>.Register(e => e.IsToFinish);

        /// <summary>
        /// 未完成
        /// </summary>
        public bool IsToFinish
        {
            get { return this.GetProperty(IsToFinishProperty); }
            set { this.SetProperty(IsToFinishProperty, value); }
        }
        #endregion         

        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<RepairController>().CriteriaEquipRepairBills(this);
        }
    }
}
