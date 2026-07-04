using SIE.Common;
using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Core.Enums;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.EMS.Enums;
using SIE.EMS.InspectionRules;
using SIE.EMS.SpecialEquipment.RegularInspections.Configs;
using SIE.EMS.SpecialEquipment.RegularInspections.Criterias;
using SIE.EMS.SpecialEquipment.SpecialEquipmentAcounts;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;


namespace SIE.EMS.SpecialEquipment.RegularInspections
{
    /// <summary>
    /// 特种设备定检
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(RegularInspectionCriteria))]
    [EntityWithConfig(typeof(NoConfig))]
    [EntityWithConfig(typeof(ApprovalConfig))]
    [EntityWithConfig(typeof(ValueConfig))]
    [Label("特种设备定检")]
    public partial class RegularInspection : DataEntity
    {
        #region 检验单号 InspectionNo
        /// <summary>
        /// 检验单号
        /// </summary>
        [Label("检验单号")]
        public static readonly Property<string> InspectionNoProperty = P<RegularInspection>.Register(e => e.InspectionNo);

        /// <summary>
        /// 检验单号
        /// </summary>
        public string InspectionNo
        {
            get { return GetProperty(InspectionNoProperty); }
            set { SetProperty(InspectionNoProperty, value); }
        }
        #endregion

        #region 计划检验日期 PlanInspectionDate
        /// <summary>
        /// 计划检验日期
        /// </summary>
        [Label("计划检验日期")]
        public static readonly Property<DateTime> PlanInspectionDateProperty = P<RegularInspection>.Register(e => e.PlanInspectionDate);

        /// <summary>
        /// 计划检验日期
        /// </summary>
        public DateTime PlanInspectionDate
        {
            get { return GetProperty(PlanInspectionDateProperty); }
            set { SetProperty(PlanInspectionDateProperty, value); }
        }
        #endregion

        #region 实际检验日期 ActualInspectionDate
        /// <summary>
        /// 实际检验日期
        /// </summary>
        [Label("实际检验日期")]
        public static readonly Property<DateTime?> ActualInspectionDateProperty = P<RegularInspection>.Register(e => e.ActualInspectionDate);

        /// <summary>
        /// 实际检验日期
        /// </summary>
        public DateTime? ActualInspectionDate
        {
            get { return GetProperty(ActualInspectionDateProperty); }
            set { SetProperty(ActualInspectionDateProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(1000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<RegularInspection>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 检验说明 InspectionRemark
        /// <summary>
        /// 检验说明
        /// </summary>
        [MaxLength(1000)]
        [Label("检验说明")]
        public static readonly Property<string> InspectionRemarkProperty = P<RegularInspection>.Register(e => e.InspectionRemark);

        /// <summary>
        /// 检验说明
        /// </summary>
        public string InspectionRemark
        {
            get { return GetProperty(InspectionRemarkProperty); }
            set { SetProperty(InspectionRemarkProperty, value); }
        }
        #endregion

        #region 检验人 Inspector
        /// <summary>
        /// 检验人Id
        /// </summary>
        public static readonly IRefIdProperty InspectorIdProperty = P<RegularInspection>.RegisterRefId(e => e.InspectorId, ReferenceType.Normal);

        /// <summary>
        /// 检验人Id
        /// </summary>
        public double? InspectorId
        {
            get { return (double?)GetRefNullableId(InspectorIdProperty); }
            set { SetRefNullableId(InspectorIdProperty, value); }
        }

        /// <summary>
        /// 检验人
        /// </summary>
        public static readonly RefEntityProperty<Employee> InspectorProperty = P<RegularInspection>.RegisterRef(e => e.Inspector, InspectorIdProperty);

        /// <summary>
        /// 检验人
        /// </summary>
        public Employee Inspector
        {
            get { return GetRefEntity(InspectorProperty); }
            set { SetRefEntity(InspectorProperty, value); }
        }
        #endregion

        #region 检验明细列表 RegularInspectionDetailList
        /// <summary>
        /// 检验明细列表
        /// </summary>
        public static readonly ListProperty<EntityList<RegularInspectionDetail>> RegularInspectionDetailListProperty = P<RegularInspection>.RegisterList(e => e.RegularInspectionDetailList);
        /// <summary>
        /// 检验明细列表
        /// </summary>
        public EntityList<RegularInspectionDetail> RegularInspectionDetailList
        {
            get { return this.GetLazyList(RegularInspectionDetailListProperty); }
        }
        #endregion

        #region 操作记录列表 RegularInspectionResumeList
        /// <summary>
        /// 操作记录列表
        /// </summary>
        public static readonly ListProperty<EntityList<RegularInspectionResume>> RegularInspectionResumeListProperty = P<RegularInspection>.RegisterList(e => e.RegularInspectionResumeList);
        /// <summary>
        /// 操作记录列表
        /// </summary>
        public EntityList<RegularInspectionResume> RegularInspectionResumeList
        {
            get { return this.GetLazyList(RegularInspectionResumeListProperty); }
        }
        #endregion

        #region 检验结果 InspectionResult
        /// <summary>
        /// 检验结果
        /// </summary>
        [Label("检验结果")]
        public static readonly Property<InspectionResult?> InspectionResultProperty = P<RegularInspection>.Register(e => e.InspectionResult);

        /// <summary>
        /// 检验结果
        /// </summary>
        public InspectionResult? InspectionResult
        {
            get { return GetProperty(InspectionResultProperty); }
            set { SetProperty(InspectionResultProperty, value); }
        }
        #endregion

        #region 检验状态 InspectionStatus
        /// <summary>
        /// 检验状态
        /// </summary>
        [Label("检验状态")]
        public static readonly Property<InspectionStatus> InspectionStatusProperty = P<RegularInspection>.Register(e => e.InspectionStatus);

        /// <summary>
        /// 检验状态
        /// </summary>
        public InspectionStatus InspectionStatus
        {
            get { return GetProperty(InspectionStatusProperty); }
            set { SetProperty(InspectionStatusProperty, value); }
        }
        #endregion

        #region 单据来源 BillSourceType
        /// <summary>
        /// 单据来源
        /// </summary>
        [Label("单据来源")]
        public static readonly Property<BillSourceType> BillSourceTypeProperty = P<RegularInspection>.Register(e => e.BillSourceType);

        /// <summary>
        /// 单据来源
        /// </summary>
        public BillSourceType BillSourceType
        {
            get { return GetProperty(BillSourceTypeProperty); }
            set { SetProperty(BillSourceTypeProperty, value); }
        }
        #endregion

        #region 检验机构 Agency
        /// <summary>
        /// 检验机构Id
        /// </summary>
        public static readonly IRefIdProperty AgencyIdProperty = P<RegularInspection>.RegisterRefId(e => e.AgencyId, ReferenceType.Normal);

        /// <summary>
        /// 检验机构Id
        /// </summary>
        public double? AgencyId
        {
            get { return (double?)GetRefNullableId(AgencyIdProperty); }
            set { SetRefNullableId(AgencyIdProperty, value); }
        }

        /// <summary>
        /// 检验机构
        /// </summary>
        public static readonly RefEntityProperty<Supplier> AgencyProperty = P<RegularInspection>.RegisterRef(e => e.Agency, AgencyIdProperty);

        /// <summary>
        /// 检验机构
        /// </summary>
        public Supplier Agency
        {
            get { return GetRefEntity(AgencyProperty); }
            set { SetRefEntity(AgencyProperty, value); }
        }
        #endregion

        #region 设备定检附件列表 RegularInspectionAttachmentList
        /// <summary>
        /// 设备定检附件列表
        /// </summary>
        public static readonly ListProperty<EntityList<RegularInspectionAttachment>> RegularInspectionAttachmentListProperty = P<RegularInspection>.RegisterList(e => e.RegularInspectionAttachmentList);
        /// <summary>
        /// 设备定检附件列表
        /// </summary>
        public EntityList<RegularInspectionAttachment> RegularInspectionAttachmentList
        {
            get { return this.GetLazyList(RegularInspectionAttachmentListProperty); }
        }
        #endregion

        #region 特种设备台账 SpecialEquipmentAccount
        /// <summary>
        /// 特种设备台账Id
        /// </summary>
        public static readonly IRefIdProperty SpecialEquipmentAccountIdProperty = P<RegularInspection>.RegisterRefId(e => e.SpecialEquipmentAccountId, ReferenceType.Normal);

        /// <summary>
        /// 特种设备台账Id
        /// </summary>
        public double SpecialEquipmentAccountId
        {
            get { return (double)GetRefId(SpecialEquipmentAccountIdProperty); }
            set { SetRefId(SpecialEquipmentAccountIdProperty, value); }
        }

        /// <summary>
        /// 特种设备台账
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> SpecialEquipmentAccountProperty = P<RegularInspection>.RegisterRef(e => e.SpecialEquipmentAccount, SpecialEquipmentAccountIdProperty);

        /// <summary>
        /// 特种设备台账
        /// </summary>
        public EquipAccountSelect SpecialEquipmentAccount
        {
            get { return GetRefEntity(SpecialEquipmentAccountProperty); }
            set { SetRefEntity(SpecialEquipmentAccountProperty, value); }
        }
        #endregion

        #region 特种设备台账名称  SpecialEquipmentAccountName
        /// <summary>
        /// 特种设备台账名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> SpecialEquipmentAccountNameProperty = P<RegularInspection>.RegisterView(e => e.SpecialEquipmentAccountName, p => p.SpecialEquipmentAccount.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string SpecialEquipmentAccountName
        {
            get { return this.GetProperty(SpecialEquipmentAccountNameProperty); }
        }
        #endregion

        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ApprovalStatus?> ApprovalStatusProperty = P<RegularInspection>.Register(e => e.ApprovalStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus? ApprovalStatus
        {
            get { return GetProperty(ApprovalStatusProperty); }
            set { SetProperty(ApprovalStatusProperty, value); }
        }
        #endregion

        #region 审核意见 ApprovalInfo
        /// <summary>
        /// 审核意见
        /// </summary>
        [MaxLength(2000)]
        [Label("审核意见")]
        public static readonly Property<string> ApprovalInfoProperty = P<RegularInspection>.Register(e => e.ApprovalInfo);

        /// <summary>
        /// 审核意见
        /// </summary>
        public string ApprovalInfo
        {
            get { return this.GetProperty(ApprovalInfoProperty); }
            set { this.SetProperty(ApprovalInfoProperty, value); }
        }
        #endregion

        #region 检验规程 InspectionRule
        /// <summary>
        /// 检验规程Id
        /// </summary>
        public static readonly IRefIdProperty InspectionRuleIdProperty = P<RegularInspection>.RegisterRefId(e => e.InspectionRuleId, ReferenceType.Normal);

        /// <summary>
        /// 检验规程Id
        /// </summary>
        public double InspectionRuleId
        {
            get { return (double)GetRefId(InspectionRuleIdProperty); }
            set { SetRefId(InspectionRuleIdProperty, value); }
        }

        /// <summary>
        /// 检验规程
        /// </summary>
        public static readonly RefEntityProperty<InspectionRule> InspectionRuleProperty = P<RegularInspection>.RegisterRef(e => e.InspectionRule, InspectionRuleIdProperty);

        /// <summary>
        /// 检验规程
        /// </summary>
        public InspectionRule InspectionRule
        {
            get { return GetRefEntity(InspectionRuleProperty); }
            set { SetRefEntity(InspectionRuleProperty, value); }
        }
        #endregion

        #region 校验类型 CheckCategory
        /// <summary>
        /// 校验类型
        /// </summary>
        [Label("校验类型")]
        public static readonly Property<CheckCategory> CheckCategoryProperty = P<RegularInspection>.RegisterView(e => e.CheckCategory, p => p.InspectionRule.CheckCategory);

        /// <summary>
        /// 校验类型
        /// </summary>
        public CheckCategory CheckCategory
        {
            get { return GetProperty(CheckCategoryProperty); }
            set { SetProperty(CheckCategoryProperty, value); }
        }
        #endregion

        #region 采购订单号 PoNo
        /// <summary>
        /// 采购订单号
        /// </summary>
        [Label("采购订单号")]
        public static readonly Property<string> PoNoProperty = P<RegularInspection>.Register(e => e.PoNo);

        /// <summary>
        /// 采购订单号
        /// </summary>
        public string PoNo
        {
            get { return GetProperty(PoNoProperty); }
            set { SetProperty(PoNoProperty, value); }
        }
        #endregion

        #region 设备类型  EquipTypeName
        /// <summary>
        /// 设备类型
        /// </summary>
        [Label("设备类型")]
        public static readonly Property<string> EquipTypeNameProperty = P<RegularInspection>.RegisterView(e => e.EquipTypeName, p => p.SpecialEquipmentAccount.EquipModel.EquipType.TypeName);

        /// <summary>
        /// 设备类型
        /// </summary>
        public string EquipTypeName
        {
            get { return this.GetProperty(EquipTypeNameProperty); }
        }
        #endregion

        #region 资产责任人  ResPerson
        /// <summary>
        /// 资产责任人
        /// </summary>
        [Label("资产责任人")]
        public static readonly Property<string> ResPersonProperty = P<RegularInspection>.RegisterView(e => e.ResPerson, p => p.SpecialEquipmentAccount.ResPerson.Name);

        /// <summary>
        /// 资产责任人
        /// </summary>
        public string ResPerson
        {
            get { return this.GetProperty(ResPersonProperty); }
        }
        #endregion

        #region 使用部门  UseDepartment
        /// <summary>
        /// 使用部门
        /// </summary>
        [Label("使用部门")]
        public static readonly Property<string> UseDepartmentProperty = P<RegularInspection>.RegisterView(e => e.UseDepartment, p => p.SpecialEquipmentAccount.UseDepartment.Name);

        /// <summary>
        /// 使用部门
        /// </summary>
        public string UseDepartment
        {
            get { return this.GetProperty(UseDepartmentProperty); }
        }
        #endregion

        #region 生产厂商  Manufacturer
        /// <summary>
        /// 生产厂商
        /// </summary>
        [Label("生产厂商")]
        public static readonly Property<string> ManufacturerProperty = P<RegularInspection>.RegisterView(e => e.Manufacturer, p => p.SpecialEquipmentAccount.Manufacturer);

        /// <summary>
        /// 生产厂商
        /// </summary>
        public string Manufacturer
        {
            get { return this.GetProperty(ManufacturerProperty); }
        }
        #endregion

        #region 设备型号  EquipModelName
        /// <summary>
        /// 设备型号
        /// </summary>
        [Label("设备型号")]
        public static readonly Property<string> EquipModelNameProperty = P<RegularInspection>.RegisterView(e => e.EquipModelName, p => p.SpecialEquipmentAccount.EquipModel.Name);

        /// <summary>
        /// 设备型号
        /// </summary>
        public string EquipModelName
        {
            get { return this.GetProperty(EquipModelNameProperty); }
        }
        #endregion

        #region 入厂日期  EnterDate
        /// <summary>
        /// 入厂日期
        /// </summary>
        [Label("入厂日期")]
        public static readonly Property<DateTime?> EnterDateProperty = P<RegularInspection>.RegisterView(e => e.EnterDate, p => p.SpecialEquipmentAccount.EnterDate);

        /// <summary>
        /// 入厂日期
        /// </summary>
        public DateTime? EnterDate
        {
            get { return this.GetProperty(EnterDateProperty); }
        }
        #endregion

        #region 检验机构名称 AgencyName
        /// <summary>
        /// 检验机构名称
        /// </summary>
        [Label("检验机构名称")]
        public static readonly Property<string> AgencyNameProperty = P<RegularInspection>.RegisterView(e => e.AgencyName, p => p.Agency.Name);

        /// <summary>
        /// 检验机构名称
        /// </summary>
        public string AgencyName
        {
            get { return this.GetProperty(AgencyNameProperty); }
        }
        #endregion

    }

    /// <summary>
    /// 特种设备定检 实体配置
    /// </summary>
    internal class RegularInspectionConfig : EntityConfig<RegularInspection>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_REG_INS").MapAllProperties();
            Meta.Property(RegularInspection.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.Property(RegularInspection.InspectionRemarkProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}