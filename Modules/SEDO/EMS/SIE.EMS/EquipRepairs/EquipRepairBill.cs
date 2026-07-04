using SIE.Domain;
using SIE.EMS.EquipRepairs.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.EMS.EquipRepairs
{
    /// <summary>
    /// 设备维修单
    /// </summary>
    [RootEntity, Serializable]
    [DisplayMember(nameof(RepairNo))]
    [Label("设备维修单")]
    public partial class EquipRepairBill : DataEntity
    {
        #region 维修单号 RepairNo
        /// <summary>
        /// 维修单号
        /// </summary>
        [Label("维修单号")]
        public static readonly Property<string> RepairNoProperty = P<EquipRepairBill>.Register(e => e.RepairNo);

        /// <summary>
        /// 维修单号
        /// </summary>
        public string RepairNo
        {
            get { return this.GetProperty(RepairNoProperty); }
            set { this.SetProperty(RepairNoProperty, value); }
        }
        #endregion

        #region 维修状态 RepairState
        /// <summary>
        /// 维修状态
        /// </summary>
        [Label("维修状态")]
        public static readonly Property<EquipRepairState> RepairStateProperty = P<EquipRepairBill>.Register(e => e.RepairState);

        /// <summary>
        /// 维修状态
        /// </summary>
        public EquipRepairState RepairState
        {
            get { return this.GetProperty(RepairStateProperty); }
            set { this.SetProperty(RepairStateProperty, value); }
        }
        #endregion

        #region 来源单号 SourceNo
        /// <summary>
        /// 来源单号
        /// </summary>
        [Label("来源单号")]
        public static readonly Property<string> SourceNoProperty = P<EquipRepairBill>.Register(e => e.SourceNo);

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
        public static readonly Property<RepairSourceType> SourceTypeProperty = P<EquipRepairBill>.Register(e => e.SourceType);

        /// <summary>
        /// 来源类型
        /// </summary>
        public RepairSourceType SourceType
        {
            get { return this.GetProperty(SourceTypeProperty); }
            set { this.SetProperty(SourceTypeProperty, value); }
        }
        #endregion

        #region 维修类型 RepairType
        /// <summary>
        /// 维修类型
        /// </summary>
        [Required]
        [Label("维修类型")]
        public static readonly Property<EquipRepairType> RepairTypeProperty = P<EquipRepairBill>.Register(e => e.RepairType);

        /// <summary>
        /// 维修类型
        /// </summary>
        public EquipRepairType RepairType
        {
            get { return this.GetProperty(RepairTypeProperty); }
            set { this.SetProperty(RepairTypeProperty, value); }
        }
        #endregion

        #region 派工类型 RepairWay
        /// <summary>
        /// 派工类型
        /// </summary>
        [Label("派工类型")]
        public static readonly Property<EquipRepairWay?> RepairWayProperty = P<EquipRepairBill>.Register(e => e.RepairWay);

        /// <summary>
        /// 派工类型
        /// </summary>
        public EquipRepairWay? RepairWay
        {
            get { return this.GetProperty(RepairWayProperty); }
            set { this.SetProperty(RepairWayProperty, value); }
        }
        #endregion

        #region 设备 EquipAccount
        /// <summary>
        /// 设备Id
        /// </summary>
        [Label("设备")]
        public static readonly IRefIdProperty EquipAccountIdProperty =
            P<EquipRepairBill>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

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
            P<EquipRepairBill>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return this.GetRefEntity(EquipAccountProperty); }
            set { this.SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 生产状态 ProduceState
        /// <summary>
        /// 生产状态
        /// </summary>
        [Required]
        [Label("生产状态")]
        public static readonly Property<ProduceState> ProduceStateProperty = P<EquipRepairBill>.Register(e => e.ProduceState);

        /// <summary>
        /// 生产状态
        /// </summary>
        public ProduceState ProduceState
        {
            get { return this.GetProperty(ProduceStateProperty); }
            set { this.SetProperty(ProduceStateProperty, value); }
        }
        #endregion

        #region 紧急程度 UrgentDegree
        /// <summary>
        /// 紧急程度
        /// </summary>
        [Required]
        [Label("紧急程度")]
        public static readonly Property<UrgentDegree> UrgentDegreeProperty = P<EquipRepairBill>.Register(e => e.UrgentDegree);

        /// <summary>
        /// 紧急程度
        /// </summary>
        public UrgentDegree UrgentDegree
        {
            get { return this.GetProperty(UrgentDegreeProperty); }
            set { this.SetProperty(UrgentDegreeProperty, value); }
        }
        #endregion

        #region 故障现象(备注) DeviceAbnormalRemark
        /// <summary>
        /// 故障现象(备注)
        /// </summary>
        [Label("故障现象(备注)")]
        public static readonly Property<string> DeviceAbnormalRemarkProperty = P<EquipRepairBill>.Register(e => e.DeviceAbnormalRemark);

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
        public static readonly Property<string> DeviceAbnormalCodeProperty = P<EquipRepairBill>.Register(e => e.DeviceAbnormalCode);

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
            P<EquipRepairBill>.RegisterRefId(e => e.ApplyRepairEmployeeId, ReferenceType.Normal);

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
            P<EquipRepairBill>.RegisterRef(e => e.ApplyRepairEmployee, ApplyRepairEmployeeIdProperty);

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
        public static readonly Property<DateTime> ApplyRepairDateProperty = P<EquipRepairBill>.Register(e => e.ApplyRepairDate);

        /// <summary>
        /// 报修时间
        /// </summary>
        public DateTime ApplyRepairDate
        {
            get { return this.GetProperty(ApplyRepairDateProperty); }
            set { this.SetProperty(ApplyRepairDateProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 设备维修单 实体配置
    /// </summary>
    internal class EquipRepairBillConfig : EntityConfig<EquipRepairBill>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_REPAIR").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
