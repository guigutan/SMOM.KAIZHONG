using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Domain;
using SIE.EMS.SpareParts.Applys.Criterias;
using SIE.EMS.SpareParts.Applys.Details;
using SIE.EMS.SpareParts.Applys.Enums;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.SpareParts.Applys
{
    /// <summary>
    /// 备件申请单
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(SparePartAppCriteria))]
    [EntityWithConfig(typeof(NoConfig), "备件申请单生成规则配置项", "备件申请单生成规则")]
    [EntityWithConfig(typeof(ApprovalConfig))]
    [Label("备件申请单")]
    public class SparePartApp : DataEntity
    {

        #region 备件申请单号 No
        /// <summary>
        /// 备件申请单号
        /// </summary>
        [Required]
        [Label("备件申请单号")]
        public static readonly Property<string> NoProperty = P<SparePartApp>.Register(e => e.No);

        /// <summary>
        /// 备件申请单号
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 来源单号 FromNo
        /// <summary>
        /// 来源单号
        /// </summary>
        [Label("来源单号")]
        public static readonly Property<string> FromNoProperty = P<SparePartApp>.Register(e => e.FromNo);

        /// <summary>
        /// 来源单号
        /// </summary>
        public string FromNo
        {
            get { return this.GetProperty(FromNoProperty); }
            set { this.SetProperty(FromNoProperty, value); }
        }
        #endregion

        #region 来源类型 FromType
        /// <summary>
        /// 来源类型
        /// </summary>
        [Label("来源类型")]
        public static readonly Property<FromType> FromTypeProperty = P<SparePartApp>.Register(e => e.FromType);

        /// <summary>
        /// 来源类型
        /// </summary>
        public FromType FromType
        {
            get { return this.GetProperty(FromTypeProperty); }
            set { this.SetProperty(FromTypeProperty, value); }
        }
        #endregion

        #region 需求时间 DemandDate
        /// <summary>
        /// 需求时间
        /// </summary>
        [Label("需求时间")]
        public static readonly Property<DateTime?> DemandDateProperty = P<SparePartApp>.Register(e => e.DemandDate);

        /// <summary>
        /// 需求时间
        /// </summary>
        public DateTime? DemandDate
        {
            get { return this.GetProperty(DemandDateProperty); }
            set { this.SetProperty(DemandDateProperty, value); }
        }
        #endregion

        #region 状态 AuditState
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<AuditState> AuditStateProperty = P<SparePartApp>.Register(e => e.AuditState);

        /// <summary>
        /// 状态
        /// </summary>
        public AuditState AuditState
        {
            get { return this.GetProperty(AuditStateProperty); }
            set { this.SetProperty(AuditStateProperty, value); }
        }
        #endregion

        #region 质量状态 QualityStatus
        /// <summary>
        /// 质量状态
        /// </summary>
        [Label("质量状态")]
        public static readonly Property<QualityStatus> QualityStatusProperty = P<SparePartApp>.Register(e => e.QualityStatus);

        /// <summary>
        /// 质量状态
        /// </summary>
        public QualityStatus QualityStatus
        {
            get { return this.GetProperty(QualityStatusProperty); }
            set { this.SetProperty(QualityStatusProperty, value); }
        }
        #endregion

        #region 设备编码 EquipAccount
        /// <summary>
        /// 设备编码Id
        /// </summary>
        [Label("设备编码")]
        public static readonly IRefIdProperty EquipAccountIdProperty =
            P<SparePartApp>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备编码Id
        /// </summary>
        public double? EquipAccountId
        {
            get { return (double)this.GetRefId(EquipAccountIdProperty); }
            set { this.SetRefId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备编码
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty =
            P<SparePartApp>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

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
        public static readonly Property<string> EquipAccountNameProperty = P<SparePartApp>.RegisterView(e => e.EquipAccountName, p => p.EquipAccount.Name);

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
            P<SparePartApp>.RegisterRefId(e => e.EquipModelId, ReferenceType.Normal);

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
            P<SparePartApp>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

        /// <summary>
        /// 设备型号
        /// </summary>
        public EquipModel EquipModel
        {
            get { return this.GetRefEntity(EquipModelProperty); }
            set { this.SetRefEntity(EquipModelProperty, value); }
        }
        #endregion

        #region 领用部门 GetDepartment
        /// <summary>
        /// 领用部门Id
        /// </summary>
        [Label("领用部门")]
        public static readonly IRefIdProperty GetDepartmentIdProperty =
            P<SparePartApp>.RegisterRefId(e => e.GetDepartmentId, ReferenceType.Normal);

        /// <summary>
        /// 领用部门Id
        /// </summary>
        public double? GetDepartmentId
        {
            get { return (double?)this.GetRefNullableId(GetDepartmentIdProperty); }
            set { this.SetRefNullableId(GetDepartmentIdProperty, value); }
        }

        /// <summary>
        /// 领用部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> GetDepartmentProperty =
            P<SparePartApp>.RegisterRef(e => e.GetDepartment, GetDepartmentIdProperty);

        /// <summary>
        /// 领用部门
        /// </summary>
        public Enterprise GetDepartment
        {
            get { return this.GetRefEntity(GetDepartmentProperty); }
            set { this.SetRefEntity(GetDepartmentProperty, value); }
        }
        #endregion


        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<SparePartApp>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 申请单明细 ApplyDetailList
        /// <summary>
        /// 申请单明细
        /// </summary>
        [Label("申请单明细")]
        public static readonly ListProperty<EntityList<ApplyDetail>> ApplyDetailListProperty = P<SparePartApp>.RegisterList(e => e.ApplyDetailList);

        /// <summary>
        /// 申请单明细
        /// </summary>
        public EntityList<ApplyDetail> ApplyDetailList
        {
            get { return this.GetLazyList(ApplyDetailListProperty); }
        }
        #endregion

    }

    /// <summary>
    /// 备件申请 实体配置
    /// </summary>
    internal class SparePartAppConfig : EntityConfig<SparePartApp>
    {

        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_SPARE_PART_APP").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}

