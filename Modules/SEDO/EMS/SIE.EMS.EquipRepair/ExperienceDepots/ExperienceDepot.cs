using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Domain;
using SIE.EMS.Devices.Abnormals;
using SIE.EMS.Faults;
using SIE.EMS.EquipRepair.ExperienceDepots.Attachments;
using SIE.EMS.EquipRepair.ExperienceDepots.Enums;
using SIE.EMS.SpareParts;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.EquipRepair.ExperienceDepots
{
    /// <summary>
    /// 维修经验库
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(ExperienceDepotCriteria))]
    [EntityWithConfig(typeof(NoConfig), "维修经验库生成规则配置项", "维修经验库生成规则")]
    [Label("维修经验库")]
    public class ExperienceDepot : DataEntity
    {
        /// <summary>
        /// 故障原因快码
        /// </summary>
        public static readonly String expFaultReson = "EXP_FAULT_RESON";

        #region 经验库编码 Code
        /// <summary>
        /// 经验库编码
        /// </summary>
        [Label("经验库编码")]
        public static readonly Property<string> CodeProperty = P<ExperienceDepot>.Register(e => e.Code);

        /// <summary>
        /// 注释
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
            set { this.SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 维修类型 RepairType
        /// <summary>
        /// 维修类型
        /// </summary>
        [Label("维修类型")]
        public static readonly Property<ExpRepairType> RepairTypeProperty = P<ExperienceDepot>.Register(e => e.RepairType);

        /// <summary>
        /// 维修类型
        /// </summary>
        public ExpRepairType RepairType
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
            P<ExperienceDepot>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

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
            P<ExperienceDepot>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

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
        public static readonly Property<string> EquipAccountNameProperty = P<ExperienceDepot>.RegisterView(e => e.EquipAccountName, p => p.EquipAccount.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipAccountName
        {
            get { return this.GetProperty(EquipAccountNameProperty); }
        }
        #endregion

        #region 设备型号 EquipModel
        /// <summary>
        /// 设备型号Id
        /// </summary>
        [Label("设备型号")]
        public static readonly IRefIdProperty EquipModelIdProperty =
            P<ExperienceDepot>.RegisterRefId(e => e.EquipModelId, ReferenceType.Normal);

        /// <summary>
        /// 设备型号Id
        /// </summary>
        public double? EquipModelId
        {
            get { return (double)this.GetRefId(EquipModelIdProperty); }
            set { this.SetRefId(EquipModelIdProperty, value); }
        }

        /// <summary>
        /// 设备型号
        /// </summary>
        public static readonly RefEntityProperty<EquipModel> EquipModelProperty =
            P<ExperienceDepot>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

        /// <summary>
        /// 设备型号
        /// </summary>
        public EquipModel EquipModel
        {
            get { return this.GetRefEntity(EquipModelProperty); }
            set { this.SetRefEntity(EquipModelProperty, value); }
        }
        #endregion

        #region 设备类型 EquipType
        /// <summary>
        /// 设备类型Id
        /// </summary>
        [Label("设备类型")]
        public static readonly IRefIdProperty EquipTypeIdProperty =
            P<ExperienceDepot>.RegisterRefId(e => e.EquipTypeId, ReferenceType.Normal);

        /// <summary>
        /// 设备类型Id
        /// </summary>
        public double? EquipTypeId
        {
            get { return (double)this.GetRefId(EquipTypeIdProperty); }
            set { this.SetRefId(EquipTypeIdProperty, value); }
        }

        /// <summary>
        /// 设备类型
        /// </summary>
        public static readonly RefEntityProperty<EquipType> EquipTypeProperty =
            P<ExperienceDepot>.RegisterRef(e => e.EquipType, EquipTypeIdProperty);

        /// <summary>
        /// 设备类型
        /// </summary>
        public EquipType EquipType
        {
            get { return this.GetRefEntity(EquipTypeProperty); }
            set { this.SetRefEntity(EquipTypeProperty, value); }
        }
        #endregion

        #region 备件编码 SparePart
        /// <summary>
        /// 备件编码Id
        /// </summary>
        [Label("备件编码")]
        public static readonly IRefIdProperty SparePartIdProperty =
            P<ExperienceDepot>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

        /// <summary>
        /// 备件编码Id
        /// </summary>
        public double? SparePartId
        {
            get { return (double)this.GetRefId(SparePartIdProperty); }
            set { this.SetRefId(SparePartIdProperty, value); }
        }

        /// <summary>
        /// 备件编码
        /// </summary>
        public static readonly RefEntityProperty<SparePart> SparePartProperty =
            P<ExperienceDepot>.RegisterRef(e => e.SparePart, SparePartIdProperty);

        /// <summary>
        /// 备件编码
        /// </summary>
        public SparePart SparePart
        {
            get { return this.GetRefEntity(SparePartProperty); }
            set { this.SetRefEntity(SparePartProperty, value); }
        }
        #endregion

        #region 备件名称 SparePartName
        /// <summary>
        /// 备件名称
        /// </summary>
        [Label("备件名称")]
        public static readonly Property<string> SparePartNameProperty = P<ExperienceDepot>.RegisterView(e => e.SparePartName, p => p.SparePart.SparePartName);

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName
        {
            get { return this.GetProperty(SparePartNameProperty); }
        }
        #endregion

        #region 维修单号 RepairNo
        /// <summary>
        /// 维修单号
        /// </summary>
        [Label("维修单号")]
        public static readonly Property<string> RepairNoProperty = P<ExperienceDepot>.Register(e => e.RepairNo);

        /// <summary>
        /// 维修单号
        /// </summary>
        public string RepairNo
        {
            get { return this.GetProperty(RepairNoProperty); }
            set { this.SetProperty(RepairNoProperty, value); }
        }
        #endregion

        #region 故障原因 FaultReson
        /// <summary>
        /// 故障原因
        /// </summary>
        [Label("故障原因")]
        public static readonly Property<string> FaultResonProperty = P<ExperienceDepot>.Register(e => e.FaultReson);

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
            P<ExperienceDepot>.RegisterRefId(e => e.EquipLargeFaultId, ReferenceType.Normal);

        /// <summary>
        /// 故障类别Id
        /// </summary>
        public double EquipLargeFaultId
        {
            get { return (double)this.GetRefId(EquipLargeFaultIdProperty); }
            set { this.SetRefId(EquipLargeFaultIdProperty, value); }
        }

        /// <summary>
        /// 故障类别
        /// </summary>
        public static readonly RefEntityProperty<EquipLargeFault> EquipLargeFaultProperty =
            P<ExperienceDepot>.RegisterRef(e => e.EquipLargeFault, EquipLargeFaultIdProperty);

        /// <summary>
        /// 故障类别
        /// </summary>
        public EquipLargeFault EquipLargeFault
        {
            get { return this.GetRefEntity(EquipLargeFaultProperty); }
            set { this.SetRefEntity(EquipLargeFaultProperty, value); }
        }
        #endregion

        #region 故障部位 FaultPart
        /// <summary>
        /// 故障部位
        /// </summary>
     //   [Required]
        [Label("故障部位")]
        public static readonly Property<string> FaultPartProperty = P<ExperienceDepot>.Register(e => e.FaultPart);

        /// <summary>
        /// 故障部位
        /// </summary>
        public string FaultPart
        {
            get { return this.GetProperty(FaultPartProperty); }
            set { this.SetProperty(FaultPartProperty, value); }
        }
        #endregion

        #region 详细信息 DetailedInfo

        #region 故障现象 FaultPhenomenon
        /// <summary>
        /// 故障现象Id
        /// </summary>
        [Label("故障现象")]
        public static readonly IRefIdProperty FaultPhenomenonIdProperty =
            P<ExperienceDepot>.RegisterRefId(e => e.FaultPhenomenonId, ReferenceType.Normal);

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
            P<ExperienceDepot>.RegisterRef(e => e.FaultPhenomenon, FaultPhenomenonIdProperty);

        /// <summary>
        /// 故障现象
        /// </summary>
        public DeviceAbnormal FaultPhenomenon
        {
            get { return this.GetRefEntity(FaultPhenomenonProperty); }
            set { this.SetRefEntity(FaultPhenomenonProperty, value); }
        }
        #endregion

        #region 故障现象（备注） FaultPhenomenonRemark
        /// <summary>
        /// 故障现象（备注）
        /// </summary>
        [Label("故障现象（备注）")]
        [MaxLength(480)]
        public static readonly Property<string> FaultPhenomenonRemarkProperty = P<ExperienceDepot>.Register(e => e.FaultPhenomenonRemark);

        /// <summary>
        /// 故障现象（备注）
        /// </summary>
        public string FaultPhenomenonRemark
        {
            get { return this.GetProperty(FaultPhenomenonRemarkProperty); }
            set { this.SetProperty(FaultPhenomenonRemarkProperty, value); }
        }
        #endregion

        #region 故障描述 FaultDescribe
        /// <summary>
        /// 故障描述Id
        /// </summary>
        [Label("故障描述")]
        public static readonly IRefIdProperty FaultDescribeIdProperty =
            P<ExperienceDepot>.RegisterRefId(e => e.FaultDescribeId, ReferenceType.Normal);

        /// <summary>
        /// 故障描述Id
        /// </summary>
        public double? FaultDescribeId
        {
            get { return (double?)this.GetRefNullableId(FaultDescribeIdProperty); }
            set { this.SetRefNullableId(FaultDescribeIdProperty, value); }
        }

        /// <summary>
        /// 故障描述
        /// </summary>
        public static readonly RefEntityProperty<DeviceAbnormal> FaultDescribeProperty =
            P<ExperienceDepot>.RegisterRef(e => e.FaultDescribe, FaultDescribeIdProperty);

        /// <summary>
        /// 故障描述
        /// </summary>
        public DeviceAbnormal FaultDescribe
        {
            get { return this.GetRefEntity(FaultDescribeProperty); }
            set { this.SetRefEntity(FaultDescribeProperty, value); }
        }
        #endregion

        #region 故障描述（备注） FaultDescribeRemark
        /// <summary>
        /// 故障描述（备注）
        /// </summary>
        [Label("故障描述（备注）")]
        [MaxLength(480)]
        public static readonly Property<string> FaultDescribeRemarkProperty = P<ExperienceDepot>.Register(e => e.FaultDescribeRemark);

        /// <summary>
        /// 故障描述（备注）
        /// </summary>
        public string FaultDescribeRemark
        {
            get { return this.GetProperty(FaultDescribeRemarkProperty); }
            set { this.SetProperty(FaultDescribeRemarkProperty, value); }
        }
        #endregion

        #region 维修方法 RepairWay
        /// <summary>
        /// 维修方法
        /// </summary>
        [Label("维修方法")]
        [Required]
        [MaxLength(480)]
        public static readonly Property<string> RepairWayProperty = P<ExperienceDepot>.Register(e => e.RepairWay);

        /// <summary>
        /// 维修方法
        /// </summary>
        public string RepairWay
        {
            get { return this.GetProperty(RepairWayProperty); }
            set { this.SetProperty(RepairWayProperty, value); }
        }
        #endregion

        #region 预防建议 PreventionAdvice
        /// <summary>
        /// 预防建议
        /// </summary>
        [Label("预防建议")]
        //[Required]
        [MaxLength(480)]
        public static readonly Property<string> PreventionAdviceProperty = P<ExperienceDepot>.Register(e => e.PreventionAdvice);

        /// <summary>
        /// 预防建议
        /// </summary>
        public string PreventionAdvice
        {
            get { return this.GetProperty(PreventionAdviceProperty); }
            set { this.SetProperty(PreventionAdviceProperty, value); }
        }
        #endregion

        #region 故障代码 FaultCode
        /// <summary>
        /// 故障代码
        /// </summary>
        [Label("故障代码")]
        [MaxLength(480)]
        public static readonly Property<string> FaultCodeProperty = P<ExperienceDepot>.Register(e => e.FaultCode);

        /// <summary>
        /// 故障代码
        /// </summary>
        public string FaultCode
        {
            get { return this.GetProperty(FaultCodeProperty); }
            set { this.SetProperty(FaultCodeProperty, value); }
        }
        #endregion

        #endregion

        #region 图片 ExperienceDepotAttachmentList
        /// <summary>
        /// 图片
        /// </summary>
        [Label("图片")]
        public static readonly ListProperty<EntityList<ExperienceDepotAttachment>> ExperienceDepotAttachmentListProperty = P<ExperienceDepot>.RegisterList(e => e.ExperienceDepotAttachmentList);

        /// <summary>
        /// 图片
        /// </summary>
        public EntityList<ExperienceDepotAttachment> ExperienceDepotAttachmentList
        {
            get { return this.GetLazyList(ExperienceDepotAttachmentListProperty); }
        }
        #endregion

    }

    internal class ExperienceDepotConfig : EntityConfig<ExperienceDepot>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_REPAIR_EXPER_DEPOT").MapAllProperties();
            Meta.Property(ExperienceDepot.FaultPhenomenonRemarkProperty).ColumnMeta.HasLength(1500);
            Meta.Property(ExperienceDepot.FaultDescribeRemarkProperty).ColumnMeta.HasLength(1500);
            Meta.Property(ExperienceDepot.RepairWayProperty).ColumnMeta.HasLength(1500);
            Meta.Property(ExperienceDepot.PreventionAdviceProperty).ColumnMeta.HasLength(1500);
            Meta.Property(ExperienceDepot.FaultCodeProperty).ColumnMeta.HasLength(1500);
            Meta.EnablePhantoms();
        }
    }
}
