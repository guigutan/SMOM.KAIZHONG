using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.EMS.Devices.Abnormals;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.EquipRepair.EquipRepairs.Enums;
using SIE.EMS.EquipRepairs.Enums;
using SIE.EMS.SpareParts;
using SIE.Equipments.EquipAccounts;
using SIE.ObjectModel;
using SIE.Resources;
using System;

namespace SIE.EMS.EquipRepair.EquipRepairs.ViewModels
{
    /// <summary>
    ///点检保养执行报修
    /// </summary>
    [RootEntity, Serializable]
    [Label("设备报修")]
    public class EquipRepairViewModel : ViewModel
    {

        #region 维修状态 RepairState
        /// <summary>
        /// 维修状态
        /// </summary>
        [Label("维修状态")]
        public static readonly Property<EquipRepairState> RepairStateProperty = P<EquipRepairViewModel>.Register(e => e.RepairState);

        /// <summary>
        /// 维修状态
        /// </summary>
        public EquipRepairState RepairState
        {
            get { return this.GetProperty(RepairStateProperty); }
            set { this.SetProperty(RepairStateProperty, value); }
        }
        #endregion

        #region 维修单号 RepairNo
        /// <summary>
        /// 维修单号
        /// </summary>
        [Label("维修单号")]
        public static readonly Property<string> RepairNoProperty = P<EquipRepairViewModel>.Register(e => e.RepairNo);

        /// <summary>
        /// 维修单号
        /// </summary>
        public string RepairNo
        {
            get { return this.GetProperty(RepairNoProperty); }
            set { this.SetProperty(RepairNoProperty, value); }
        }
        #endregion

        #region 来源单号 SourceNo
        /// <summary>
        /// 来源单号
        /// </summary>
        [Label("来源单号")]
        public static readonly Property<string> SourceNoProperty = P<EquipRepairViewModel>.Register(e => e.SourceNo);

        /// <summary>
        /// 来源单号
        /// </summary>
        public string SourceNo
        {
            get { return this.GetProperty(SourceNoProperty); }
            set { this.SetProperty(SourceNoProperty, value); }
        }
        #endregion

        #region 来源类型 SourceType
        /// <summary>
        /// 来源类型
        /// </summary>
        [Label("来源类型")]
        public static readonly Property<RepairSourceType> SourceTypeProperty = P<EquipRepairViewModel>.Register(e => e.SourceType);

        /// <summary>
        /// 来源类型
        /// </summary>
        public RepairSourceType SourceType
        {
            get { return this.GetProperty(SourceTypeProperty); }
            set { this.SetProperty(SourceTypeProperty, value); }
        }
        #endregion

        #region 设备 EquipAccount
        /// <summary>
        /// 设备Id
        /// </summary>
        [Label("设备")]
        public static readonly IRefIdProperty EquipAccountIdProperty =
            P<EquipRepairViewModel>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备Id
        /// </summary>
        public double? EquipAccountId
        {
            get { return (double?)this.GetRefId(EquipAccountIdProperty); }
            set { this.SetRefId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty =
            P<EquipRepairViewModel>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return this.GetRefEntity(EquipAccountProperty); }
            set { this.SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 备件 SparePart
        /// <summary>
        /// 备件Id
        /// </summary>
        [Label("备件")]
        public static readonly IRefIdProperty SparePartIdProperty =
            P<EquipRepairViewModel>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

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
            P<EquipRepairViewModel>.RegisterRef(e => e.SparePart, SparePartIdProperty);

        /// <summary>
        /// 备件
        /// </summary>
        public SparePart SparePart
        {
            get { return this.GetRefEntity(SparePartProperty); }
            set { this.SetRefEntity(SparePartProperty, value); }
        }
        #endregion

        #region 紧急程度 UrgentDegree
        /// <summary>
        /// 紧急程度
        /// </summary>
        [Required]
        [Label("紧急程度")]
        public static readonly Property<UrgentDegree> UrgentDegreeProperty = P<EquipRepairViewModel>.Register(e => e.UrgentDegree);

        /// <summary>
        /// 紧急程度
        /// </summary>
        public UrgentDegree UrgentDegree
        {
            get { return this.GetProperty(UrgentDegreeProperty); }
            set { this.SetProperty(UrgentDegreeProperty, value); }
        }
        #endregion

        #region 生产状态 ProduceState
        /// <summary>
        /// 生产状态
        /// </summary>
        [Required]
        [Label("生产状态")]
        public static readonly Property<ProduceState> ProduceStateProperty = P<EquipRepairViewModel>.Register(e => e.ProduceState);

        /// <summary>
        /// 生产状态
        /// </summary>
        public ProduceState ProduceState
        {
            get { return this.GetProperty(ProduceStateProperty); }
            set { this.SetProperty(ProduceStateProperty, value); }
        }
        #endregion

        #region 故障现象 DeviceAbnormal
        /// <summary>
        /// 故障现象Id
        /// </summary>
        [Label("故障现象")]
        public static readonly IRefIdProperty DeviceAbnormalIdProperty =
            P<EquipRepairViewModel>.RegisterRefId(e => e.DeviceAbnormalId, ReferenceType.Normal);

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
            P<EquipRepairViewModel>.RegisterRef(e => e.DeviceAbnormal, DeviceAbnormalIdProperty);

        /// <summary>
        /// 故障现象
        /// </summary>
        public DeviceAbnormal DeviceAbnormal
        {
            get { return this.GetRefEntity(DeviceAbnormalProperty); }
            set { this.SetRefEntity(DeviceAbnormalProperty, value); }
        }
        #endregion

        #region 故障现象(备注) DeviceAbnormalRemark
        /// <summary>
        /// 故障现象(备注)
        /// </summary>
        [Label("故障现象(备注)")]
        public static readonly Property<string> DeviceAbnormalRemarkProperty = P<EquipRepairViewModel>.Register(e => e.DeviceAbnormalRemark);

        /// <summary>
        /// 故障现象(备注)
        /// </summary>
        public string DeviceAbnormalRemark
        {
            get { return this.GetProperty(DeviceAbnormalRemarkProperty); }
            set { this.SetProperty(DeviceAbnormalRemarkProperty, value); }
        }
        #endregion

        #region 故障代码(选填) DeviceAbnormalCode
        /// <summary>
        /// 故障代码
        /// </summary>
        [Label("故障代码")]
        public static readonly Property<string> DeviceAbnormalCodeProperty = P<EquipRepairViewModel>.Register(e => e.DeviceAbnormalCode);

        /// <summary>
        /// 故障代码
        /// </summary>
        public string DeviceAbnormalCode
        {
            get { return this.GetProperty(DeviceAbnormalCodeProperty); }
            set { this.SetProperty(DeviceAbnormalCodeProperty, value); }
        }
        #endregion

        #region 报修人 ApplyRepairEmployee
        /// <summary>
        /// 报修人Id
        /// </summary>
        [Label("报修人")]
        public static readonly IRefIdProperty ApplyRepairEmployeeIdProperty =
            P<EquipRepairViewModel>.RegisterRefId(e => e.ApplyRepairEmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 报修人Id
        /// </summary>
        public double ApplyRepairEmployeeId
        {
            get { return (double)this.GetRefId(ApplyRepairEmployeeIdProperty); }
            set { this.SetRefId(ApplyRepairEmployeeIdProperty, value); }
        }

        /// <summary>
        /// 报修人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ApplyRepairEmployeeProperty =
            P<EquipRepairViewModel>.RegisterRef(e => e.ApplyRepairEmployee, ApplyRepairEmployeeIdProperty);

        /// <summary>
        /// 报修人
        /// </summary>
        public Employee ApplyRepairEmployee
        {
            get { return this.GetRefEntity(ApplyRepairEmployeeProperty); }
            set { this.SetRefEntity(ApplyRepairEmployeeProperty, value); }
        }
        #endregion

        #region 报修时间 ApplyRepairDate
        /// <summary>
        /// 报修时间
        /// </summary>
        [Label("报修时间")]
        public static readonly Property<DateTime> ApplyRepairDateProperty = P<EquipRepairViewModel>.Register(e => e.ApplyRepairDate);

        /// <summary>
        /// 报修时间
        /// </summary>
        public DateTime ApplyRepairDate
        {
            get { return this.GetProperty(ApplyRepairDateProperty); }
            set { this.SetProperty(ApplyRepairDateProperty, value); }
        }
        #endregion

        #region 附件列表 AttachmentList
        /// <summary>
        /// 附件列表
        /// </summary>
        public static readonly ListProperty<EntityList<EquipRepairViewModelAttachment>> AttachmentListProperty = P<EquipRepairViewModel>.RegisterList(e => e.AttachmentList);
        /// <summary>
        /// 附件列表
        /// </summary>
        public EntityList<EquipRepairViewModelAttachment> AttachmentList
        {
            get { return this.GetLazyList(AttachmentListProperty); }
        }
        #endregion

        #region 视图属性
        #region 设备编码 EquipAccountCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipAccountCodeProperty = P<EquipRepairViewModel>.RegisterView(e => e.EquipAccountCode, p => p.EquipAccount.Code);

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
        public static readonly Property<string> EquipAccountNameProperty = P<EquipRepairViewModel>.RegisterView(e => e.EquipAccountName, p => p.EquipAccount.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipAccountName
        {
            get { return this.GetProperty(EquipAccountNameProperty); }
        }
        #endregion

        #region 设备型号 EquipAccountMode
        /// <summary>
        /// 
        /// </summary>
        [Label("设备型号")]
        public static readonly Property<string> EquipAccountModeProperty = P<EquipRepairViewModel>.RegisterView(e => e.EquipAccountMode, p => p.EquipAccount.EquipModel.Name);

        /// <summary>
        /// 设备型号
        /// </summary>
        public string EquipAccountMode
        {
            get { return this.GetProperty(EquipAccountModeProperty); }
        }
        #endregion

        #region 设备类型 EquipAccountType
        /// <summary>
        /// 设备类型
        /// </summary>
        [Label("设备类型")]
        public static readonly Property<string> EquipAccountTypeProperty = P<EquipRepairViewModel>.RegisterView(e => e.EquipAccountType, p => p.EquipAccount.EquipModel.EquipType.TypeName);

        /// <summary>
        /// 设备类型
        /// </summary>
        public string EquipAccountType
        {
            get { return this.GetProperty(EquipAccountTypeProperty); }
        }
        #endregion

        #region 备件编码 SparePartCode
        /// <summary>
        /// 备件编码
        /// </summary>
        [Label("备件编码")]
        public static readonly Property<string> SparePartCodeProperty = P<EquipRepairViewModel>.RegisterView(e => e.SparePartCode, p => p.SparePart.SparePartCode);

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
        public static readonly Property<string> SparePartNameProperty = P<EquipRepairViewModel>.RegisterView(e => e.SparePartName, p => p.SparePart.SparePartName);

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
        public static readonly Property<string> SparePartTypeProperty = P<EquipRepairViewModel>.RegisterView(e => e.SparePartType, p => p.SparePart.ItemCategory.Name);

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
        public static readonly Property<string> WorkShopNameProperty = P<EquipRepairViewModel>.RegisterView(e => e.WorkShopName, p => p.EquipAccount.WorkShop.Name);

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
        public static readonly Property<string> ResourceNameProperty = P<EquipRepairViewModel>.RegisterView(e => e.ResourceName, p => p.EquipAccount.Resource.Name);

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
        public static readonly Property<string> ProcessNameProperty = P<EquipRepairViewModel>.RegisterView(e => e.ProcessName, p => p.EquipAccount.Process.Name);

        /// <summary>
        /// 工序
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }
        #endregion

        #region 使用部门 UseDepartment
        /// <summary>
        /// 使用部门
        /// </summary>
        [Label("使用部门")]
        public static readonly Property<string> UseDepartmentProperty = P<EquipRepairViewModel>.RegisterView(e => e.UseDepartment, p => p.EquipAccount.UseDepartment.Name);

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
        public static readonly Property<string> InstallationLocationProperty = P<EquipRepairViewModel>.RegisterView(e => e.InstallationLocation, p => p.EquipAccount.InstallationLocation);

        /// <summary>
        /// 安装位置
        /// </summary>
        public string InstallationLocation
        {
            get { return this.GetProperty(InstallationLocationProperty); }
        }
        #endregion

        #endregion

    }
}
