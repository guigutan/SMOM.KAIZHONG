using SIE.Domain;
using SIE.EMS.Devices.Abnormals;
using SIE.EMS.EquipRepair.ExperienceDepots.Controllers;
using SIE.EMS.Faults;
using SIE.EMS.SpareParts;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.EquipRepair.ExperienceDepots
{
    /// <summary>
    /// 经验库查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("经验库查询实体")]
    public partial class ExperienceDepotCriteria : Criteria
    {

        /// <summary>
        /// 故障原因快码
        /// </summary>
        public static readonly String expFaultReson = "EXP_FAULT_RESON";

        #region 设备编码 EquipAccountCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipAccountCodeProperty = P<ExperienceDepotCriteria>.Register(e => e.EquipAccountCode);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipAccountCode
        {
            get { return this.GetProperty(EquipAccountCodeProperty); }
            set { this.SetProperty(EquipAccountCodeProperty, value); }
        }
        #endregion

        #region 设备名称 EquipAccountName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipAccountNameProperty = P<ExperienceDepotCriteria>.Register(e => e.EquipAccountName);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipAccountName
        {
            get { return this.GetProperty(EquipAccountNameProperty); }
            set { this.SetProperty(EquipAccountNameProperty, value); }
        }
        #endregion

        #region 设备型号 EquipModelCode
        /// <summary>
        /// 设备型号
        /// </summary>
        [Label("设备型号")]
        public static readonly Property<string> EquipModelCodeProperty = P<ExperienceDepotCriteria>.Register(e => e.EquipModelCode);

        /// <summary>
        /// 设备型号
        /// </summary>
        public string EquipModelCode
        {
            get { return this.GetProperty(EquipModelCodeProperty); }
            set { this.SetProperty(EquipModelCodeProperty, value); }
        }
        #endregion

        #region 备件编码 SparePartCode
        /// <summary>
        /// 备件编码
        /// </summary>
        [Label("备件编码")]
        public static readonly Property<string> SparePartCodeProperty = P<ExperienceDepotCriteria>.Register(e => e.SparePartCode);

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparePartCode
        {
            get { return this.GetProperty(SparePartCodeProperty); }
            set { this.SetProperty(SparePartCodeProperty, value); }
        }
        #endregion

        #region 备件名称 SparePartName
        /// <summary>
        /// 备件名称
        /// </summary>
        [Label("备件名称")]
        public static readonly Property<string> SparePartNameProperty = P<ExperienceDepotCriteria>.Register(e => e.SparePartName);

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName
        {
            get { return this.GetProperty(SparePartNameProperty); }
            set { this.SetProperty(SparePartNameProperty, value); }
        }
        #endregion

        #region 维修单号 RepairNo
        /// <summary>
        /// 维修单号
        /// </summary>
        [Label("维修单号")]
        public static readonly Property<string> RepairNoProperty = P<ExperienceDepotCriteria>.Register(e => e.RepairNo);

        /// <summary>
        /// 维修单号
        /// </summary>
        public string RepairNo
        {
            get { return this.GetProperty(RepairNoProperty); }
            set { this.SetProperty(RepairNoProperty, value); }
        }
        #endregion

        #region 故障现象 FaultPhenomenon
        /// <summary>
        /// 故障现象Id
        /// </summary>
        [Label("故障现象")]
        public static readonly IRefIdProperty FaultPhenomenonIdProperty =
            P<ExperienceDepotCriteria>.RegisterRefId(e => e.FaultPhenomenonId, ReferenceType.Normal);

        /// <summary>
        /// 故障现象Id
        /// </summary>
        public double? FaultPhenomenonId
        {
            get { return (double?)this.GetRefNullableId(FaultPhenomenonIdProperty); }
            set { this.SetRefNullableId(FaultPhenomenonIdProperty, value); }
        }

        /// <summary>
        /// 故障现象
        /// </summary>
        public static readonly RefEntityProperty<DeviceAbnormal> FaultPhenomenonProperty =
            P<ExperienceDepotCriteria>.RegisterRef(e => e.FaultPhenomenon, FaultPhenomenonIdProperty);

        /// <summary>
        /// 故障现象
        /// </summary>
        public DeviceAbnormal FaultPhenomenon
        {
            get { return this.GetRefEntity(FaultPhenomenonProperty); }
            set { this.SetRefEntity(FaultPhenomenonProperty, value); }
        }
        #endregion

        #region 故障原因 FaultReson
        /// <summary>
        /// 故障原因
        /// </summary>
        [Label("故障原因")]
        public static readonly Property<string> FaultResonProperty = P<ExperienceDepotCriteria>.Register(e => e.FaultReson);

        /// <summary>
        /// 故障原因
        /// </summary>
        public string FaultReson
        {
            get { return this.GetProperty(FaultResonProperty); }
            set { this.SetProperty(FaultResonProperty, value); }
        }
        #endregion

        #region 故障类别 EquipLargeFault
        /// <summary>
        /// 故障类别Id
        /// </summary>
        [Label("故障类别")]
        public static readonly IRefIdProperty EquipLargeFaultIdProperty =
            P<ExperienceDepotCriteria>.RegisterRefId(e => e.EquipLargeFaultId, ReferenceType.Normal);

        /// <summary>
        /// 故障类别Id
        /// </summary>
        public double? EquipLargeFaultId
        {
            get { return (double?)this.GetRefNullableId(EquipLargeFaultIdProperty); }
            set { this.SetRefNullableId(EquipLargeFaultIdProperty, value); }
        }

        /// <summary>
        /// 故障类别
        /// </summary>
        public static readonly RefEntityProperty<EquipLargeFault> EquipLargeFaultProperty =
            P<ExperienceDepotCriteria>.RegisterRef(e => e.EquipLargeFault, EquipLargeFaultIdProperty);

        /// <summary>
        /// 故障类别
        /// </summary>
        public EquipLargeFault EquipLargeFault
        {
            get { return this.GetRefEntity(EquipLargeFaultProperty); }
            set { this.SetRefEntity(EquipLargeFaultProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ExperienceDepotController>().GetExperienceDepotList(this);
        }
    }
}
