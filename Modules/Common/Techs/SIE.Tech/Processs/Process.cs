using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.ProcessSegments;
using SIE.Tech.VictoryStandards;
using System;

namespace SIE.Tech.Processs
{
    /// <summary>
    /// 工序
    /// </summary>
    [RootEntity, Serializable]
    //[CriteriaQuery]
    [ConditionQueryType(typeof(ProcessCriteria))]
    [Label("工序")]
    [DisplayMember(nameof(Name))]
    public partial class Process : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [MaxLength(40)]
        [NotDuplicate]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<Process>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [MaxLength(40)]
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<Process>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 引用数量 ReferenceTimes
        /// <summary>
        /// 引用数量
        /// </summary>
        [Label("引用数量")]
        public static readonly Property<int> ReferenceTimesProperty = P<Process>.Register(e => e.ReferenceTimes);

        /// <summary>
        /// 引用数量
        /// </summary>
        public int ReferenceTimes
        {
            get { return GetProperty(ReferenceTimesProperty); }
            set { SetProperty(ReferenceTimesProperty, value); }
        }
        #endregion

        #region 类型 Type
        /// <summary>
        /// 类型
        /// </summary>
        [Required]
        [Label("工序类型")]
        public static readonly Property<ProcessType?> TypeProperty = P<Process>.Register(e => e.Type);

        /// <summary>
        /// 类型
        /// </summary>
        public ProcessType? Type
        {
            get { return GetProperty(TypeProperty); }
            set { SetProperty(TypeProperty, value); }
        }
        #endregion

        #region 产品族小类 ProductFamily
        /// <summary>
        /// 产品族小类Id
        /// </summary>
        [Label("产品族")]
        public static readonly IRefIdProperty ProductFamilyIdProperty = P<Process>.RegisterRefId(e => e.ProductFamilyId, ReferenceType.Normal);

        /// <summary>
        /// 产品族小类Id
        /// </summary>
        public double ProductFamilyId
        {
            get { return (double)GetRefId(ProductFamilyIdProperty); }
            set { SetRefId(ProductFamilyIdProperty, value); }
        }

        /// <summary>
        /// 产品族小类
        /// </summary>
        public static readonly RefEntityProperty<ProductFamily> ProductFamilyProperty = P<Process>.RegisterRef(e => e.ProductFamily, ProductFamilyIdProperty);

        /// <summary>
        /// 产品族小类
        /// </summary>
        public ProductFamily ProductFamily
        {
            get { return GetRefEntity(ProductFamilyProperty); }
            set { SetRefEntity(ProductFamilyProperty, value); }
        }
        #endregion

        #region 工段 Segment
        /// <summary>
        /// 工段Id
        /// </summary>    
        [Label("工段")]
        public static readonly IRefIdProperty SegmentIdProperty = P<Process>.RegisterRefId(e => e.SegmentId, ReferenceType.Normal);

        /// <summary>
        /// 工段Id
        /// </summary>
        public double? SegmentId
        {
            get { return (double?)GetRefNullableId(SegmentIdProperty); }
            set { SetRefNullableId(SegmentIdProperty, value); }
        }

        /// <summary>
        /// 工段
        /// </summary>
        public static readonly RefEntityProperty<ProcessSegment> SegmentProperty = P<Process>.RegisterRef(e => e.Segment, SegmentIdProperty);

        /// <summary>
        /// 工段
        /// </summary>
        public ProcessSegment Segment
        {
            get { return GetRefEntity(SegmentProperty); }
            set { SetRefEntity(SegmentProperty, value); }
        }
        #endregion

        #region 工序参数列表 ParameterList
        /// <summary>
        /// 工序参数列表
        /// </summary>
        [Label("工序参数")]
        public static readonly ListProperty<EntityList<ProcessParameter>> ParameterListProperty = P<Process>.RegisterList(e => e.ParameterList);

        /// <summary>
        /// 工序参数列表
        /// </summary>
        public EntityList<ProcessParameter> ParameterList
        {
            get { return this.GetLazyList(ParameterListProperty); }
        }
        #endregion

        #region 工序缺陷列表 DefectList
        /// <summary>
        /// 工序缺陷列表
        /// </summary>
        [Label("缺陷信息")]
        public static readonly ListProperty<EntityList<ProcessDefect>> DefectListProperty = P<Process>.RegisterList(e => e.DefectList);

        /// <summary>
        /// 工序缺陷列表
        /// </summary>
        public EntityList<ProcessDefect> DefectList
        {
            get { return this.GetLazyList(DefectListProperty); }
        }
        #endregion

        #region 工序 EmployeeList
        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        public static readonly ListProperty<EntityList<ProcessEmployee>> EmployeeListProperty = P<Process>.RegisterList(e => e.EmployeeList);

        /// <summary>
        /// 工序
        /// </summary>
        public EntityList<ProcessEmployee> EmployeeList
        {
            get { return this.GetLazyList(EmployeeListProperty); }
        }
        #endregion

        #region 工序对应包装 ProcessPackingUnitList
        /// <summary>
        /// 工序与包装单位关系
        /// </summary>
        [Label("工序对应包装")]
        public static readonly ListProperty<EntityList<ProcessPackingUnit>> ProcessPackingUnitListProperty = P<Process>.RegisterList(e => e.ProcessPackingUnitList);

        /// <summary>
        /// 工序与包装单位关系
        /// </summary>
        public EntityList<ProcessPackingUnit> ProcessPackingUnitList
        {
            get { return this.GetLazyList(ProcessPackingUnitListProperty); }
        }
        #endregion

        #region 采集步骤列表 CollectStepList
        /// <summary>
        /// 采集步骤列表
        /// </summary>
        [Label("采集步骤")]
        public static readonly ListProperty<EntityList<ProcessCollectStep>> CollectStepListProperty = P<Process>.RegisterList(e => e.CollectStepList);

        /// <summary>
        /// 采集步骤列表
        /// </summary>
        public EntityList<ProcessCollectStep> CollectStepList
        {
            get { return this.GetLazyList(CollectStepListProperty); }
        }
        #endregion

        #region 工序技能要求 SkillList
        /// <summary>
        /// 工序技能要求
        /// </summary>
        public static readonly ListProperty<EntityList<ProcessSkill>> SkillListProperty = P<Process>.RegisterList(e => e.SkillList);

        /// <summary>
        /// 工序技能要求
        /// </summary>
        public EntityList<ProcessSkill> SkillList
        {
            get { return this.GetLazyList(SkillListProperty); }
        }
        #endregion

        #region 工步 WorkStepList
        /// <summary>
        /// 工步
        /// </summary>
        public static readonly ListProperty<EntityList<WorkStep>> workStepListProperty = P<Process>.RegisterList(e => e.WorkStepList);

        /// <summary>
        /// 工步
        /// </summary>
        public EntityList<WorkStep> WorkStepList
        {
            get { return this.GetLazyList(workStepListProperty); }
        }
        #endregion

        #region 启用入站控制 EnableMoveInControl
        /// <summary>
        /// 启用入站控制
        /// </summary>
        [Label("启用入站控制")]
        public static readonly Property<bool?> EnableMoveInControlProperty = P<Process>.Register(e => e.EnableMoveInControl);

        /// <summary>
        /// 启用入站控制
        /// </summary>
        public bool? EnableMoveInControl
        {
            get { return this.GetProperty(EnableMoveInControlProperty); }
            set { this.SetProperty(EnableMoveInControlProperty, value); }
        }
        #endregion

        #region 是否可选 CanChoose
        /// <summary>
        /// 是否可选
        /// </summary>
        [Label("是否可选")]
        public static readonly Property<bool?> CanChooseProperty = P<Process>.Register(e => e.CanChoose);

        /// <summary>
        /// 是否可选
        /// </summary>
        public bool? CanChoose
        {
            get { return this.GetProperty(CanChooseProperty); }
            set { this.SetProperty(CanChooseProperty, value); }
        }
        #endregion

        #region 重复过站 IsRepeat
        /// <summary>
        /// 重复过站
        /// </summary>
        [Label("重复过站")]
        public static readonly Property<bool?> IsRepeatProperty = P<Process>.Register(e => e.IsRepeat);

        /// <summary>
        /// 重复过站
        /// </summary>
        public bool? IsRepeat
        {
            get { return this.GetProperty(IsRepeatProperty); }
            set { this.SetProperty(IsRepeatProperty, value); }
        }
        #endregion

        #region 创建SKU IsCreateSku
        /// <summary>
        /// 创建SKU
        /// </summary>
        [Label("创建SKU")]
        public static readonly Property<bool?> IsCreateSkuProperty = P<Process>.Register(e => e.IsCreateSku);

        /// <summary>
        /// 创建SKU
        /// </summary>
        public bool? IsCreateSku
        {
            get { return this.GetProperty(IsCreateSkuProperty); }
            set { this.SetProperty(IsCreateSkuProperty, value); }
        }
        #endregion

        #region 是否计产 IsCalculate
        /// <summary>
        /// 是否计产
        /// </summary>
        [Label("是否计产")]
        public static readonly Property<bool?> IsCalculateProperty = P<Process>.Register(e => e.IsCalculate);

        /// <summary>
        /// 是否计产
        /// </summary>
        public bool? IsCalculate
        {
            get { return this.GetProperty(IsCalculateProperty); }
            set { this.SetProperty(IsCalculateProperty, value); }
        }
        #endregion

        #region 是否生成任务单 IsGenerateTask
        /// <summary>
        /// 是否生成任务单
        /// </summary>
        [Label("是否生成任务单")]
        public static readonly Property<bool?> IsGenerateTaskProperty = P<Process>.Register(e => e.IsGenerateTask);

        /// <summary>
        /// 是否生成任务单
        /// </summary>
        public bool? IsGenerateTask
        {
            get { return this.GetProperty(IsGenerateTaskProperty); }
            set { this.SetProperty(IsGenerateTaskProperty, value); }
        }
        #endregion

        #region 是否需求任务清单 IsRequirementTask
        /// <summary>
        /// 是否需求任务清单
        /// </summary>
        [Label("是否需求任务清单")]
        public static readonly Property<bool?> IsRequirementTaskProperty = P<Process>.Register(e => e.IsRequirementTask);

        /// <summary>
        /// 是否生成任务单
        /// </summary>
        public bool? IsRequirementTask
        {
            get { return this.GetProperty(IsRequirementTaskProperty); }
            set { this.SetProperty(IsRequirementTaskProperty, value); }
        }
        #endregion

        #region 是否扣料 IsBuckleMaterial
        /// <summary>
        /// 是否扣料
        /// </summary>
        [Label("是否扣料")]
        public static readonly Property<bool?> IsBuckleMaterialProperty = P<Process>.Register(e => e.IsBuckleMaterial);

        /// <summary>
        /// 是否扣料
        /// </summary>
        public bool? IsBuckleMaterial
        {
            get { return this.GetProperty(IsBuckleMaterialProperty); }
            set { this.SetProperty(IsBuckleMaterialProperty, value); }
        }
        #endregion

        #region 正常胜制 VictoryStandard
        /// <summary>
        /// 正常胜制Id
        /// </summary>    
        [Label("正常胜制")]
        public static readonly IRefIdProperty VictoryStandardIdProperty = P<Process>.RegisterRefId(e => e.VictoryStandardId, ReferenceType.Normal);

        /// <summary>
        /// 正常胜制Id
        /// </summary>
        public double? VictoryStandardId
        {
            get { return (double?)GetRefNullableId(VictoryStandardIdProperty); }
            set { SetRefNullableId(VictoryStandardIdProperty, value); }
        }

        /// <summary>
        /// 正常胜制
        /// </summary>
        public static readonly RefEntityProperty<VictoryStandard> VictoryStandardProperty = P<Process>.RegisterRef(e => e.VictoryStandard, VictoryStandardIdProperty);

        /// <summary>
        /// 正常胜制
        /// </summary>
        public VictoryStandard VictoryStandard
        {
            get { return GetRefEntity(VictoryStandardProperty); }
            set { SetRefEntity(VictoryStandardProperty, value); }
        }
        #endregion


        #region 维修胜制 RepairVictory
        /// <summary>
        /// 维修胜制Id
        /// </summary>    
        [Label("维修胜制")]
        public static readonly IRefIdProperty RepairVictoryIdProperty = P<Process>.RegisterRefId(e => e.RepairVictoryId, ReferenceType.Normal);

        /// <summary>
        /// 维修胜制Id
        /// </summary>
        public double? RepairVictoryId
        {
            get { return (double?)GetRefNullableId(RepairVictoryIdProperty); }
            set { SetRefNullableId(RepairVictoryIdProperty, value); }
        }

        /// <summary>
        /// 维修胜制
        /// </summary>
        public static readonly RefEntityProperty<VictoryStandard> RepairVictoryProperty = P<Process>.RegisterRef(e => e.RepairVictory, RepairVictoryIdProperty);

        /// <summary>
        /// 维修胜制
        /// </summary>
        public VictoryStandard RepairVictory
        {
            get { return GetRefEntity(RepairVictoryProperty); }
            set { SetRefEntity(RepairVictoryProperty, value); }
        }
        #endregion

        #region 是否加严 IsStricter
        /// <summary>
        /// 是否加严
        /// </summary>
        [Label("是否加严")]
        public static readonly Property<bool?> IsStricterProperty = P<Process>.Register(e => e.IsStricter);

        /// <summary>
        /// 是否加严
        /// </summary>
        public bool? IsStricter
        {
            get { return this.GetProperty(IsStricterProperty); }
            set { this.SetProperty(IsStricterProperty, value); }
        }
        #endregion


        #region 超时时间（分钟） Overtime
        /// <summary>
        /// 超时时间（分钟）
        /// </summary>
        [Label("超时时间（分钟）")]
        public static readonly Property<int?> OvertimeProperty = P<Process>.Register(e => e.Overtime);

        /// <summary>
        /// 超时时间（分钟）
        /// </summary>
        public int? Overtime
        {
            get { return this.GetProperty(OvertimeProperty); }
            set { this.SetProperty(OvertimeProperty, value); }
        }
        #endregion

        #region 直通率取值 IsPassRate
        /// <summary>
        /// 直通率取值
        /// </summary>
        [Label("直通率取值")]
        public static readonly Property<bool?> IsPassRateProperty = P<Process>.Register(e => e.IsPassRate);

        /// <summary>
        /// 直通率取值
        /// </summary>
        public bool? IsPassRate
        {
            get { return this.GetProperty(IsPassRateProperty); }
            set { this.SetProperty(IsPassRateProperty, value); }
        }
        #endregion

        #region 绑定 IsBinding
        /// <summary>
        /// 绑定
        /// </summary>
        [Label("绑定")]
        public static readonly Property<bool?> IsBindingProperty = P<Process>.Register(e => e.IsBinding);

        /// <summary>
        /// 绑定
        /// </summary>
        public bool? IsBinding
        {
            get { return this.GetProperty(IsBindingProperty); }
            set { this.SetProperty(IsBindingProperty, value); }
        }
        #endregion

        #region 解绑 IsUnBinding
        /// <summary>
        /// 解绑
        /// </summary>
        [Label("解绑")]
        public static readonly Property<bool?> IsUnBindingProperty = P<Process>.Register(e => e.IsUnBinding);

        /// <summary>
        /// 解绑
        /// </summary>
        public bool? IsUnBinding
        {
            get { return this.GetProperty(IsUnBindingProperty); }
            set { this.SetProperty(IsUnBindingProperty, value); }
        }
        #endregion

        #region 最大过站次数 MaxPassNum
        /// <summary>
        /// 最大过站次数
        /// </summary>
        [Label("最大过站次数")]
        public static readonly Property<int?> MaxPassNumProperty = P<Process>.Register(e => e.MaxPassNum);

        /// <summary>
        /// 最大过站次数
        /// </summary>
        public int? MaxPassNum
        {
            get { return this.GetProperty(MaxPassNumProperty); }
            set { this.SetProperty(MaxPassNumProperty, value); }
        }
        #endregion

        #region 是否下工序入站 IsNextMoveIn
        /// <summary>
        /// 是否下工序入站
        /// </summary>
        [Label("是否下工序入站")]
        public static readonly Property<bool?> IsNextMoveInProperty = P<Process>.Register(e => e.IsNextMoveIn);

        /// <summary>
        /// 是否下工序入站
        /// </summary>
        public bool? IsNextMoveIn
        {
            get { return this.GetProperty(IsNextMoveInProperty); }
            set { this.SetProperty(IsNextMoveInProperty, value); }
        }
        #endregion

        #region 是否委外 IsOutsourcing
        /// <summary>
        /// 是否委外
        /// </summary>
        [Label("是否委外")]
        public static readonly Property<bool> IsOutsourcingProperty = P<Process>.Register(e => e.IsOutsourcing);

        /// <summary>
        /// 是否委外
        /// </summary>
        public bool IsOutsourcing
        {
            get { return this.GetProperty(IsOutsourcingProperty); }
            set { this.SetProperty(IsOutsourcingProperty, value); }
        }
        #endregion

        #region 交接类型 TransferType
        /// <summary>
        /// 交接类型
        /// </summary>
        [Label("交接类型")]
        public static readonly Property<TransferType?> TransferTypeProperty = P<Process>.Register(e => e.TransferType);

        /// <summary>
        /// 交接类型
        /// </summary>
        public TransferType? TransferType
        {
            get { return this.GetProperty(TransferTypeProperty); }
            set { this.SetProperty(TransferTypeProperty, value); }
        }
        #endregion

        #region 是否需要辅助设备 IsNeedEquipment
        /// <summary>
        /// 是否需要辅助设备
        /// </summary>
        [Label("是否需要辅助设备")]
        public static readonly Property<bool?> IsNeedEquipmentProperty = P<Process>.Register(e => e.IsNeedEquipment);

        /// <summary>
        /// 是否需要辅助设备
        /// </summary>
        public bool? IsNeedEquipment
        {
            get { return this.GetProperty(IsNeedEquipmentProperty); }
            set { this.SetProperty(IsNeedEquipmentProperty, value); }
        }
        #endregion

        #region 编码规则 NumberRule 
        /// <summary>
        /// 编码规则ID
        /// </summary>
        [Label("编码规则")]
        public static readonly IRefIdProperty NumberRuleIdProperty = P<Process>.RegisterRefId(e => e.NumberRuleId, ReferenceType.Normal);

        /// <summary>
        /// 编码规则ID
        /// </summary>
        public double? NumberRuleId
        {
            get { return (double?)this.GetRefNullableId(NumberRuleIdProperty); }
            set { this.SetRefNullableId(NumberRuleIdProperty, value); }
        }

        /// <summary>
        /// 编码规则
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> NumberRuleProperty = P<Process>.RegisterRef(e => e.NumberRule, NumberRuleIdProperty);

        /// <summary>
        /// 编码规则
        /// </summary>
        public NumberRule NumberRule
        {
            get { return this.GetRefEntity(NumberRuleProperty); }
            set { this.SetRefEntity(NumberRuleProperty, value); }
        }
        #endregion

        #region 打印模板 PrintTemplate
        /// <summary>
        /// 打印模板Id
        /// </summary>
        [Label("打印模板")]
        public static readonly IRefIdProperty PrintTemplateIdProperty =
            P<Process>.RegisterRefId(e => e.PrintTemplateId, ReferenceType.Normal);

        /// <summary>
        /// 打印模板Id
        /// </summary>
        public double? PrintTemplateId
        {
            get { return (double?)this.GetRefNullableId(PrintTemplateIdProperty); }
            set { this.SetRefNullableId(PrintTemplateIdProperty, value); }
        }

        /// <summary>
        /// 打印模板
        /// </summary>
        public static readonly RefEntityProperty<PrintTemplate> PrintTemplateProperty =
            P<Process>.RegisterRef(e => e.PrintTemplate, PrintTemplateIdProperty);

        /// <summary>
        /// 打印模板
        /// </summary>
        public PrintTemplate PrintTemplate
        {
            get { return this.GetRefEntity(PrintTemplateProperty); }
            set { this.SetRefEntity(PrintTemplateProperty, value); }
        }
        #endregion

        #region 是否按分单数量报工 IsReportByZcode
        /// <summary>
        /// 是否按分单数量报工
        /// </summary>
        [Label("是否按分单数量报工")]
        public static readonly Property<bool?> IsReportByZcodeProperty = P<Process>.Register(e => e.IsReportByZcode);

        /// <summary>
        /// 是否按分单数量报工
        /// </summary>
        public bool? IsReportByZcode
        {
            get { return this.GetProperty(IsReportByZcodeProperty); }
            set { this.SetProperty(IsReportByZcodeProperty, value); }
        }
        #endregion

        #region 报工分单数阀值(%) ZcodeThreshold
        /// <summary>
        /// 报工分单数阀值(%)
        /// </summary>
        [Label("报工分单数阀值(%)")]
        public static readonly Property<decimal> ZcodeThresholdProperty = P<Process>.Register(e => e.ZcodeThreshold);

        /// <summary>
        /// 报工分单数阀值(%)
        /// </summary>
        public decimal ZcodeThreshold
        {
            get { return this.GetProperty(ZcodeThresholdProperty); }
            set { this.SetProperty(ZcodeThresholdProperty, value); }
        }
        #endregion


        #region 注册视图属性(关联实体属性平铺显示，一般用于Web)  
        #region 产品族编码 CategoryCode
        /// <summary>
        /// 产品族编码
        /// </summary>
        [Label("产品族编码")]
        public static readonly Property<string> CategoryCodeProperty = P<Process>.RegisterView(e => e.CategoryCode, p => p.ProductFamily.Code);

        /// <summary>
        /// 产品族编码
        /// </summary>
        public string CategoryCode
        {
            get { return this.GetProperty(CategoryCodeProperty); }
        }
        #endregion

        #region 产品族 CategoryName
        /// <summary>
        /// 产品族
        /// </summary>
        [Label("产品族")]
        public static readonly Property<string> CategoryNameProperty = P<Process>.RegisterView(e => e.CategoryName, p => p.ProductFamily.Name);

        /// <summary>
        /// 产品族
        /// </summary>
        public string CategoryName
        {
            get { return this.GetProperty(CategoryNameProperty); }
        }
        #endregion

        #region 产品族编码 ProductFamilyCode
        /// <summary>
        /// 产品族编码
        /// </summary>
        [Label("产品族编码")]
        public static readonly Property<string> ProductFamilyCodeProperty = P<Process>.RegisterView(e => e.ProductFamilyCode, p => p.ProductFamily.Code);

        /// <summary>
        /// 产品族编码
        /// </summary>
        public string ProductFamilyCode
        {
            get { return this.GetProperty(ProductFamilyCodeProperty); }
        }
        #endregion

        #region 产品族名称 QualityCategoryName
        /// <summary>
        /// 产品族名称
        /// </summary>
        [Label("产品族名称")]
        public static readonly Property<string> ProductFamilyNameProperty = P<Process>.RegisterView(e => e.ProductFamilyName, p => p.ProductFamily.Name);

        /// <summary>
        /// 产品族名称
        /// </summary>
        public string ProductFamilyName
        {
            get { return this.GetProperty(ProductFamilyNameProperty); }
        }
        #endregion

        #region 产品族分类 ProductFamilyCategoryCode
        /// <summary>
        /// 产品族分类
        /// </summary>
        [Label("产品族分类")]
        public static readonly Property<string> ProductFamilyCategoryCodeProperty
            = P<Process>.RegisterView(e => e.ProductFamilyCategoryCode, p => p.ProductFamily.Category.Code);

        /// <summary>
        /// 产品族分类
        /// </summary>
        public string ProductFamilyCategoryCode
        {
            get { return this.GetProperty(ProductFamilyCategoryCodeProperty); }
        }
        #endregion


        #region 工段名称 SegmentName
        /// <summary>
        /// 工段名称
        /// </summary>
        [Label("工段名称")]
        public static readonly Property<string> SegmentNameProperty = P<Process>.RegisterView(e => e.SegmentName, p => p.Segment.Name);

        /// <summary>
        /// 工段名称
        /// </summary>
        public string SegmentName
        {
            get { return this.GetProperty(SegmentNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 工序 实体配置
    /// </summary>
    internal class ProcessConfig : EntityConfig<Process>
    {
        /// <summary>
        /// 配置数据库的映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TECH_PROCESS").MapAllProperties();
            Meta.Property(Process.NameProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}