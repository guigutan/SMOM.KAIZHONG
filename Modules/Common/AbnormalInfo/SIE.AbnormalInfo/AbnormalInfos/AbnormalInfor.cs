using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.EquipAccounts;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;

namespace SIE.AbnormalInfo.AbnormalInfos
{
    /// <summary>
    /// 异常信息
    /// </summary>
    [RootEntity, Serializable]
    [EntityWithConfig(typeof(NoConfig), "异常信息单号配置项", "异常信息单号配置规则")]
    [ConditionQueryType(typeof(AbnormalInforCriteria))]
    [BillPrintable(typeof(AbnormalInfoPrintable))]
    [Label("异常信息")]
    [DisplayMember(nameof(No))]
    public partial class AbnormalInfor : DataEntity
    {
        #region 异常单号 No
        /// <summary>
        /// 异常单号
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("异常单号")]
        public static readonly Property<string> NoProperty = P<AbnormalInfor>.Register(e => e.No);

        /// <summary>
        /// 异常单号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 缺陷代码Id清单 DefectIds
        /// <summary>
        /// 缺陷代码Id清单
        /// </summary>
        [Label("缺陷代码Id清单")]
        [MaxLength(2000)]
        public static readonly Property<string> DefectIdsProperty = P<AbnormalInfor>.Register(e => e.DefectIds);

        /// <summary>
        /// 缺陷代码Id清单
        /// </summary>
        public string DefectIds
        {
            get { return this.GetProperty(DefectIdsProperty); }
            set { this.SetProperty(DefectIdsProperty, value); }
        }
        #endregion

        #region 缺陷代码 JoinDefectCodes
        /// <summary>
        /// 缺陷代码
        /// </summary>
        [Label("缺陷代码")]
        public static readonly Property<string> JoinDefectCodesProperty = P<AbnormalInfor>.Register(e => e.JoinDefectCodes);

        /// <summary>
        /// 缺陷代码
        /// </summary>
        public string JoinDefectCodes
        {
            get { return GetProperty(JoinDefectCodesProperty); }
            set { SetProperty(JoinDefectCodesProperty, value); }
        }
        #endregion

        #region 缺陷代码描述 JoinDefectCodeDescriptions
        /// <summary>
        /// 缺陷代码描述
        /// </summary>
        [Label("缺陷描述")]
        public static readonly Property<string> JoinDefectCodeDescriptionsProperty = P<AbnormalInfor>.Register(e => e.JoinDefectCodeDescriptions);

        /// <summary>
        /// 缺陷代码描述
        /// </summary>
        public string JoinDefectCodeDescriptions
        {
            get { return GetProperty(JoinDefectCodeDescriptionsProperty); }
            set { SetProperty(JoinDefectCodeDescriptionsProperty, value); }
        }
        #endregion

        #region 异常信息定义 AbnormalInfoDefinition
        /// <summary>
        /// 异常信息定义Id
        /// </summary>
        public static readonly IRefIdProperty AbnormalInfoDefinitionIdProperty = P<AbnormalInfor>.RegisterRefId(e => e.AbnormalInfoDefinitionId, ReferenceType.Normal);

        /// <summary>
        /// 异常信息定义Id
        /// </summary>
        public double AbnormalInfoDefinitionId
        {
            get { return (double)GetRefId(AbnormalInfoDefinitionIdProperty); }
            set { SetRefId(AbnormalInfoDefinitionIdProperty, value); }
        }

        /// <summary>
        /// 异常信息定义
        /// </summary>
        public static readonly RefEntityProperty<AbnormalInfoDefinition> AbnormalInfoDefinitionProperty = P<AbnormalInfor>.RegisterRef(e => e.AbnormalInfoDefinition, AbnormalInfoDefinitionIdProperty);

        /// <summary>
        /// 异常信息定义
        /// </summary>
        public AbnormalInfoDefinition AbnormalInfoDefinition
        {
            get { return GetRefEntity(AbnormalInfoDefinitionProperty); }
            set { SetRefEntity(AbnormalInfoDefinitionProperty, value); }
        }
        #endregion

        #region 异常状态 AbnormalStatus
        /// <summary>
        /// 异常状态
        /// </summary>
        [Label("异常状态")]
        public static readonly Property<AbnormalStatus> AbnormalStatusProperty = P<AbnormalInfor>.Register(e => e.AbnormalStatus);

        /// <summary>
        /// 异常状态
        /// </summary>
        public AbnormalStatus AbnormalStatus
        {
            get { return GetProperty(AbnormalStatusProperty); }
            set { SetProperty(AbnormalStatusProperty, value); }
        }
        #endregion

        #region 是否已停线 IsStop
        /// <summary>
        /// 是否已停线
        /// </summary>
        [Label("是否已停线")]
        public static readonly Property<bool> IsStopProperty = P<AbnormalInfor>.Register(e => e.IsStop);

        /// <summary>
        /// 是否已停线
        /// </summary>
        public bool IsStop
        {
            get { return GetProperty(IsStopProperty); }
            set { SetProperty(IsStopProperty, value); }
        }
        #endregion

        #region 发起PDCA IsSendPdca
        /// <summary>
        /// 发起PDCA
        /// </summary>
        [Label("发起PDCA")]
        public static readonly Property<bool> IsSendPdcaProperty = P<AbnormalInfor>.Register(e => e.IsSendPdca);

        /// <summary>
        /// 发起PDCA
        /// </summary>
        public bool IsSendPdca
        {
            get { return GetProperty(IsSendPdcaProperty); }
            set { SetProperty(IsSendPdcaProperty, value); }
        }
        #endregion

        #region 是否过程整改任务 IsRectificationTask
        /// <summary>
        /// 是否过程整改任务
        /// </summary>
        [Label("是否过程整改任务")]
        public static readonly Property<bool> IsRectificationTaskProperty = P<AbnormalInfor>.Register(e => e.IsRectificationTask);

        /// <summary>
        /// 是否过程整改任务
        /// </summary>
        public bool IsRectificationTask
        {
            get { return GetProperty(IsRectificationTaskProperty); }
            set { SetProperty(IsRectificationTaskProperty, value); }
        }
        #endregion

        #region 原因分析 ReasonAnalysis
        /// <summary>
        /// 原因分析
        /// </summary>
        [Label("原因分析")]
        public static readonly Property<string> ReasonAnalysisProperty = P<AbnormalInfor>.Register(e => e.ReasonAnalysis);

        /// <summary>
        /// 原因分析
        /// </summary>
        public string ReasonAnalysis
        {
            get { return GetProperty(ReasonAnalysisProperty); }
            set { SetProperty(ReasonAnalysisProperty, value); }
        }
        #endregion

        #region 改善对策 Measure
        /// <summary>
        /// 改善对策
        /// </summary>
        [Label("改善对策")]
        public static readonly Property<string> MeasureProperty = P<AbnormalInfor>.Register(e => e.Measure);

        /// <summary>
        /// 改善对策
        /// </summary>
        public string Measure
        {
            get { return GetProperty(MeasureProperty); }
            set { SetProperty(MeasureProperty, value); }
        }
        #endregion

        #region 经验总结 Experience
        /// <summary>
        /// 经验总结
        /// </summary>
        [Label("经验总结")]
        public static readonly Property<string> ExperienceProperty = P<AbnormalInfor>.Register(e => e.Experience);

        /// <summary>
        /// 经验总结
        /// </summary>
        public string Experience
        {
            get { return GetProperty(ExperienceProperty); }
            set { SetProperty(ExperienceProperty, value); }
        }
        #endregion

        #region 产品 Item
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品")]
        public static readonly IRefIdProperty ItemIdProperty = P<AbnormalInfor>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double? ItemId
        {
            get { return (double?)GetRefNullableId(ItemIdProperty); }
            set { SetRefNullableId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<AbnormalInfor>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 设备 Equipment
        /// <summary>
        /// 设备Id
        /// </summary>
        [Label("设备")]
        public static readonly IRefIdProperty EquipmentIdProperty = P<AbnormalInfor>.RegisterRefId(e => e.EquipmentId, ReferenceType.Normal);

        /// <summary>
        /// 设备Id
        /// </summary>
        public double? EquipmentId
        {
            get { return (double?)GetRefNullableId(EquipmentIdProperty); }
            set { SetRefNullableId(EquipmentIdProperty, value); }
        }

        /// <summary>
        /// 设备
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipmentProperty = P<AbnormalInfor>.RegisterRef(e => e.Equipment, EquipmentIdProperty);

        /// <summary>
        /// 设备
        /// </summary>
        public EquipAccountSelect Equipment
        {
            get { return GetRefEntity(EquipmentProperty); }
            set { SetRefEntity(EquipmentProperty, value); }
        }
        #endregion

        #region 工序Id JoinProcessIds
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序Id")]
        public static readonly Property<string> JoinProcessIdsProperty = P<AbnormalInfor>.Register(e => e.JoinProcessIds);

        /// <summary>
        /// 工序Id
        /// </summary>
        public string JoinProcessIds
        {
            get { return GetProperty(JoinProcessIdsProperty); }
            set { SetProperty(JoinProcessIdsProperty, value); }
        }
        #endregion

        #region 工序 JoinProcessNames
        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        public static readonly Property<string> JoinProcessNamesProperty = P<AbnormalInfor>.Register(e => e.JoinProcessNames);

        /// <summary>
        /// 工序
        /// </summary>
        public string JoinProcessNames
        {
            get { return GetProperty(JoinProcessNamesProperty); }
            set { SetProperty(JoinProcessNamesProperty, value); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkShopIdProperty = P<AbnormalInfor>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkShopId
        {
            get { return (double?)GetRefNullableId(WorkShopIdProperty); }
            set { SetRefNullableId(WorkShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty = P<AbnormalInfor>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return GetRefEntity(WorkShopProperty); }
            set { SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 产线 Line
        /// <summary>
        /// 产线Id
        /// </summary>
        [Label("产线")]
        public static readonly IRefIdProperty LineIdProperty = P<AbnormalInfor>.RegisterRefId(e => e.LineId, ReferenceType.Normal);

        /// <summary>
        /// 产线Id
        /// </summary>
        public double? LineId
        {
            get { return (double?)GetRefNullableId(LineIdProperty); }
            set { SetRefNullableId(LineIdProperty, value); }
        }

        /// <summary>
        /// 产线
        /// </summary>
        public static readonly RefEntityProperty<WipResource> LineProperty = P<AbnormalInfor>.RegisterRef(e => e.Line, LineIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public WipResource Line
        {
            get { return GetRefEntity(LineProperty); }
            set { SetRefEntity(LineProperty, value); }
        }
        #endregion

        #region 检验单号 InspectionNo
        /// <summary>
        /// 检验单号
        /// </summary>
        [Label("检验单号")]
        public static readonly Property<string> InspectionNoProperty = P<AbnormalInfor>.Register(e => e.InspectionNo);

        /// <summary>
        /// 检验单号
        /// </summary>
        public string InspectionNo
        {
            get { return GetProperty(InspectionNoProperty); }
            set { SetProperty(InspectionNoProperty, value); }
        }
        #endregion

        #region 处理人清单 HandlersDisplay
        /// <summary>
        /// 处理人清单
        /// </summary>
        [Label("处理人")]
        public static readonly Property<string> HandlersDisplayProperty = P<AbnormalInfor>.Register(e => e.HandlersDisplay);

        /// <summary>
        /// 处理人清单
        /// </summary>
        public string HandlersDisplay
        {
            get { return this.GetProperty(HandlersDisplayProperty); }
            set { this.SetProperty(HandlersDisplayProperty, value); }
        }
        #endregion

        #region 项目(NG) ProjectNg
        /// <summary>
        /// 项目(NG)
        /// </summary>
        [Label("项目(NG)")]
        public static readonly Property<string> ProjectNgProperty = P<AbnormalInfor>.Register(e => e.ProjectNg);

        /// <summary>
        /// 项目(NG)
        /// </summary>
        public string ProjectNg
        {
            get { return this.GetProperty(ProjectNgProperty); }
            set { this.SetProperty(ProjectNgProperty, value); }
        }
        #endregion

        #region 缺陷描述(项目) ProjectDesc
        /// <summary>
        /// 缺陷描述(项目)
        /// </summary>
        [Label("缺陷描述(项目)")]
        public static readonly Property<string> ProjectDescProperty = P<AbnormalInfor>.Register(e => e.ProjectDesc);

        /// <summary>
        /// 缺陷描述(项目)
        /// </summary>
        public string ProjectDesc
        {
            get { return this.GetProperty(ProjectDescProperty); }
            set { this.SetProperty(ProjectDescProperty, value); }
        }
        #endregion

        #region 注册视图属性(关联实体属性平铺显示，一般用于Web)
        #region 异常信息定义 AbnormalInfoDefinitionDesc
        /// <summary>
        /// 异常信息定义
        /// </summary>
        [Label("异常信息定义")]
        public static readonly Property<string> AbnormalInfoDefinitionDescProperty = P<AbnormalInfor>.RegisterView(e => e.AbnormalInfoDefinitionDesc, p => p.AbnormalInfoDefinition.Desc);

        /// <summary>
        /// 异常信息定义
        /// </summary>
        public string AbnormalInfoDefinitionDesc
        {
            get { return this.GetProperty(AbnormalInfoDefinitionDescProperty); }
        }
        #endregion

        #region 异常信息定义编码 AbnormalInfoDefinitionCode
        /// <summary>
        ///异常信息定义编码
        /// </summary>
        [Label("异常信息定义编码")]
        public static readonly Property<string> AbnormalInfoDefinitionCodeProperty = P<AbnormalInfor>.RegisterView(e => e.AbnormalInfoDefinitionCode, p => p.AbnormalInfoDefinition.Code);

        /// <summary>
        ///异常信息定义编码
        /// </summary>
        public string AbnormalInfoDefinitionCode
        {
            get { return this.GetProperty(AbnormalInfoDefinitionCodeProperty); }
        }
        #endregion

        #region 异常分类 AbnormalInfoCategoryDesc
        /// <summary>
        /// 异常分类
        /// </summary>
        [Label("异常分类")]
        public static readonly Property<string> AbnormalInfoCategoryDescProperty = P<AbnormalInfor>.RegisterView(e => e.AbnormalInfoCategoryDesc, p => p.AbnormalInfoDefinition.AbnormalCategory.Desc);

        /// <summary>
        /// 异常分类
        /// </summary>
        public string AbnormalInfoCategoryDesc
        {
            get { return this.GetProperty(AbnormalInfoCategoryDescProperty); }
        }

        #region 异常定义来源 AbnormalInfoDefinitionSource
        /// <summary>
        /// 异常定义来源
        /// </summary>
        [Label("异常定义来源")]
        public static readonly Property<AbnormalSource> AbnormalInfoDefinitionSourceProperty = P<AbnormalInfor>.RegisterView(e => e.AbnormalInfoDefinitionSource, p => p.AbnormalInfoDefinition.AbnormalSource);

        /// <summary>
        /// 异常定义来源
        /// </summary>
        public AbnormalSource AbnormalInfoDefinitionSource
        {
            get { return this.GetProperty(AbnormalInfoDefinitionSourceProperty); }
        }
        #endregion

        #endregion

        #region 产品名称 ItemName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ItemNameProperty = P<AbnormalInfor>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 设备名称 EquipmentName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipmentNameProperty = P<AbnormalInfor>.RegisterView(e => e.EquipmentName, p => p.Equipment.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName
        {
            get { return this.GetProperty(EquipmentNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 异常信息 实体配置
    /// </summary>
    internal class AbnormalInfoConfig : EntityConfig<AbnormalInfor>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(AbnormalInfor.ProjectNgProperty, new StringLengthRangeRule() { Max = 2000 });
            rules.AddRule(AbnormalInfor.ProjectDescProperty, new StringLengthRangeRule() { Max = 2000 });
        }

        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("QMS_AbnormalInfo").MapAllPropertiesExcept(AbnormalInfor.JoinDefectCodeDescriptionsProperty, AbnormalInfor.JoinDefectCodesProperty);
            Meta.Property(AbnormalInfor.ReasonAnalysisProperty).ColumnMeta.HasLength(4000);
            Meta.Property(AbnormalInfor.MeasureProperty).ColumnMeta.HasLength(4000);
            Meta.Property(AbnormalInfor.ExperienceProperty).ColumnMeta.HasLength(4000);
            Meta.Property(AbnormalInfor.ProjectNgProperty).ColumnMeta.HasLength(4000);
            Meta.Property(AbnormalInfor.ProjectDescProperty).ColumnMeta.HasLength(4000);
            Meta.Property(AbnormalInfor.HandlersDisplayProperty).DontMapColumn();
            Meta.EnablePhantoms();
            Meta.Property(AbnormalInfor.AbnormalStatusProperty).ColumnMeta.HasIndex();
        }
    }
}