using SIE.Domain;
using SIE.EMS.Devices.Abnormals;
using SIE.EMS.EquipRepairs.Enums;
using SIE.EMS.Faults;
using SIE.Equipments.EquipAccounts;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.EquipRepair.EquipRepairs.ViewModels
{
    /// <summary>
    /// 维修经验库ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [Label("维修经验库")]
    //[DisplayMember(nameof())]
    public class ExperienceDepotViewModel : ViewModel
    {
        #region 维修单号 RepairNo
        /// <summary>
        /// 维修单号
        /// </summary>
        [Label("维修单号")]
        public static readonly Property<string> RepairNoProperty = P<ExperienceDepotViewModel>.Register(e => e.RepairNo);

        /// <summary>
        /// 维修单号
        /// </summary>
        public string RepairNo
        {
            get { return this.GetProperty(RepairNoProperty); }
            set { this.SetProperty(RepairNoProperty, value); }
        }
        #endregion

        #region 维修类型 RepairType
        /// <summary>
        /// 维修类型
        /// </summary>
        [Required]
        [Label("维修类型")]
        public static readonly Property<EquipRepairType> RepairTypeProperty = P<ExperienceDepotViewModel>.Register(e => e.RepairType);

        /// <summary>
        /// 维修类型
        /// </summary>
        public EquipRepairType RepairType
        {
            get { return this.GetProperty(RepairTypeProperty); }
            set { this.SetProperty(RepairTypeProperty, value); }
        }
        #endregion

        #region 设备编码 EquipAccount
        /// <summary>
        /// 设备编码Id
        /// </summary>
        [Label("设备编码")]
        public static readonly IRefIdProperty EquipAccountIdProperty =
            P<ExperienceDepotViewModel>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备编码Id
        /// </summary>
        public double? EquipAccountId
        {
            get { return (double?)this.GetRefId(EquipAccountIdProperty); }
            set { this.SetRefId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备编码
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty =
            P<ExperienceDepotViewModel>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备编码
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return this.GetRefEntity(EquipAccountProperty); }
            set { this.SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 设备名称 EquipAccountName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipAccountNameProperty = P<ExperienceDepotViewModel>.RegisterView(e => e.EquipAccountName, p => p.EquipAccount.Name);

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
        public static readonly Property<string> EquipAccountModeProperty = P<ExperienceDepotViewModel>.RegisterView(e => e.EquipAccountMode, p => p.EquipAccount.EquipModel.Name);

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
        public static readonly Property<string> EquipAccountTypeProperty = P<ExperienceDepotViewModel>.RegisterView(e => e.EquipAccountType, p => p.EquipAccount.EquipModel.EquipType.TypeName);

        /// <summary>
        /// 设备类型
        /// </summary>
        public string EquipAccountType
        {
            get { return this.GetProperty(EquipAccountTypeProperty); }
        }
        #endregion

        #region 故障现象 DeviceAbnormal
        /// <summary>
        /// 故障现象Id
        /// </summary>
        [Label("故障现象")]
        public static readonly IRefIdProperty DeviceAbnormalIdProperty =
            P<ExperienceDepotViewModel>.RegisterRefId(e => e.DeviceAbnormalId, ReferenceType.Normal);

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
            P<ExperienceDepotViewModel>.RegisterRef(e => e.DeviceAbnormal, DeviceAbnormalIdProperty);

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
        public static readonly Property<string> DeviceAbnormalRemarkProperty = P<ExperienceDepotViewModel>.Register(e => e.DeviceAbnormalRemark);

        /// <summary>
        /// 故障现象(备注)
        /// </summary>
        public string DeviceAbnormalRemark
        {
            get { return this.GetProperty(DeviceAbnormalRemarkProperty); }
            set { this.SetProperty(DeviceAbnormalRemarkProperty, value); }
        }
        #endregion

        #region 故障描述 FaultDescription
        /// <summary>
        /// 故障描述Id
        /// </summary>
        [Label("故障描述")]
        public static readonly IRefIdProperty FaultDescriptionIdProperty =
            P<ExperienceDepotViewModel>.RegisterRefId(e => e.FaultDescriptionId, ReferenceType.Normal);

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
            P<ExperienceDepotViewModel>.RegisterRef(e => e.FaultDescription, FaultDescriptionIdProperty);

        /// <summary>
        /// 故障描述
        /// </summary>
        public DeviceAbnormal FaultDescription
        {
            get { return this.GetRefEntity(FaultDescriptionProperty); }
            set { this.SetRefEntity(FaultDescriptionProperty, value); }
        }
        #endregion

        #region 故障描述(备注) FaultDescriptionRemark
        /// <summary>
        /// 故障描述(备注)
        /// </summary>
        [Label("故障描述(备注)")]
        public static readonly Property<string> FaultDescriptionRemarkProperty = P<ExperienceDepotViewModel>.Register(e => e.FaultDescriptionRemark);

        /// <summary>
        /// 故障描述(备注)
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
        public static readonly Property<string> FaultReasonProperty = P<ExperienceDepotViewModel>.Register(e => e.FaultReason);

        /// <summary>
        /// 故障原因
        /// </summary>
        public string FaultReason
        {
            get { return this.GetProperty(FaultReasonProperty); }
            set { this.SetProperty(FaultReasonProperty, value); }
        }
        #endregion

        #region 故障类别 FaultCategory
        /// <summary>
        /// 故障类别Id
        /// </summary>
        [Label("故障类别")]
        public static readonly IRefIdProperty FaultCategoryIdProperty =
            P<ExperienceDepotViewModel>.RegisterRefId(e => e.FaultCategoryId, ReferenceType.Normal);

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
            P<ExperienceDepotViewModel>.RegisterRef(e => e.FaultCategory, FaultCategoryIdProperty);

        /// <summary>
        /// 故障类别
        /// </summary>
        public EquipLargeFault FaultCategory
        {
            get { return this.GetRefEntity(FaultCategoryProperty); }
            set { this.SetRefEntity(FaultCategoryProperty, value); }
        }
        #endregion

        #region 故障部位 FaultPart
        /// <summary>
        /// 故障部位
        /// </summary>
        [Label("故障部位")]
        public static readonly Property<string> FaultPartProperty = P<ExperienceDepotViewModel>.Register(e => e.FaultPart);

        /// <summary>
        /// 故障部位
        /// </summary>
        public string FaultPart
        {
            get { return this.GetProperty(FaultPartProperty); }
            set { this.SetProperty(FaultPartProperty, value); }
        }
        #endregion

        #region 维修方法 RepairMethod
        /// <summary>
        /// 维修方法
        /// </summary>
        [Label("维修方法")]
        public static readonly Property<string> RepairMethodProperty = P<ExperienceDepotViewModel>.Register(e => e.RepairMethod);

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
        public static readonly Property<string> PreventionAdviceProperty = P<ExperienceDepotViewModel>.Register(e => e.PreventionAdvice);

        /// <summary>
        /// 预防建议
        /// </summary>
        public string PreventionAdvice
        {
            get { return this.GetProperty(PreventionAdviceProperty); }
            set { this.SetProperty(PreventionAdviceProperty, value); }
        }
        #endregion

        #region 故障代码(选填) DeviceAbnormalCode
        /// <summary>
        /// 故障代码
        /// </summary>
        [Label("故障代码")]
        public static readonly Property<string> DeviceAbnormalCodeProperty = P<ExperienceDepotViewModel>.Register(e => e.DeviceAbnormalCode);

        /// <summary>
        /// 故障代码
        /// </summary>
        public string DeviceAbnormalCode
        {
            get { return this.GetProperty(DeviceAbnormalCodeProperty); }
            set { this.SetProperty(DeviceAbnormalCodeProperty, value); }
        }
        #endregion



    }
}
