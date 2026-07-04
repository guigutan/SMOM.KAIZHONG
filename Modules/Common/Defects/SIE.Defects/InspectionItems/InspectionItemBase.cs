using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Defects.InspectionItems
{
    /// <summary>
    /// 检验项目基类
    /// </summary>
    [RootEntity, Serializable]
    [Label("检验项目基类")]
    public partial class InspectionItemBase : DataEntity
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        protected InspectionItemBase()
        {
            this.EffectiveStartTime = DateTime.Now;
        }
        #endregion

        #region 检验项目 Name
        /// <summary>
        /// 检验项目
        /// </summary>
        [Required]
        [Label("检验项目")]
        [MaxLength(200)]
        public static readonly Property<string> NameProperty = P<InspectionItemBase>.Register(e => e.Name);

        /// <summary>
        /// 检验项目
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 检验类别 Category
        /// <summary>
        /// 检验类别
        /// </summary>
        [Required]
        [Label("检验类别")]
        [MaxLength(200)]
        public static readonly Property<string> CategoryProperty = P<InspectionItemBase>.Register(e => e.Category);

        /// <summary>
        /// 检验类别
        /// </summary>
        public string Category
        {
            get { return GetProperty(CategoryProperty); }
            set { SetProperty(CategoryProperty, value); }
        }
        #endregion

        #region 检验工具 TestTool
        /// <summary>
        /// 检验工具
        /// </summary>
        [Required]
        [Label("检验工具")]
        [MaxLength(200)]
        public static readonly Property<string> TestToolProperty = P<InspectionItemBase>.Register(e => e.TestTool);

        /// <summary>
        /// 检验工具
        /// </summary>
        public string TestTool
        {
            get { return GetProperty(TestToolProperty); }
            set { SetProperty(TestToolProperty, value); }
        }
        #endregion

        #region 检验依据 InspectionBasis
        /// <summary>
        /// 检验依据
        /// </summary>
        [Required]
        [Label("检验依据")]
        [MaxLength(1000)]
        public static readonly Property<string> InspectionBasisProperty = P<InspectionItemBase>.Register(e => e.InspectionBasis);

        /// <summary>
        /// 检验依据
        /// </summary>
        public string InspectionBasis
        {
            get { return GetProperty(InspectionBasisProperty); }
            set { SetProperty(InspectionBasisProperty, value); }
        }
        #endregion

        #region 规格下限 LimitLow
        /// <summary>
        /// 规格下限
        /// </summary>
        [Label("规格下限")]
        public static readonly Property<decimal?> LimitLowProperty = P<InspectionItemBase>.Register(e => e.LimitLow);

        /// <summary>
        /// 规格下限
        /// </summary>
        public decimal? LimitLow
        {
            get { return GetProperty(LimitLowProperty); }
            set { SetProperty(LimitLowProperty, value); }
        }
        #endregion

        #region 规格上限 LimitMax
        /// <summary>
        /// 规格上限
        /// </summary>
        [Label("规格上限")]
        public static readonly Property<decimal?> LimitMaxProperty = P<InspectionItemBase>.Register(e => e.LimitMax);

        /// <summary>
        /// 规格上限
        /// </summary>
        public decimal? LimitMax
        {
            get { return GetProperty(LimitMaxProperty); }
            set { SetProperty(LimitMaxProperty, value); }
        }
        #endregion

        #region 技术要求 TechnicalRequirements
        /// <summary>
        /// 技术要求
        /// </summary>
        [Required]
        [Label("技术要求")]
        [MaxLength(1000)]
        public static readonly Property<string> TechnicalRequirementsProperty = P<InspectionItemBase>.Register(e => e.TechnicalRequirements);

        /// <summary>
        /// 技术要求
        /// </summary>
        public string TechnicalRequirements
        {
            get { return GetProperty(TechnicalRequirementsProperty); }
            set { SetProperty(TechnicalRequirementsProperty, value); }
        }
        #endregion

        #region 检验方式 InspectionMode
        /// <summary>
        /// 检验方式Id
        /// </summary>
        [Label("检验方式")]
        public static readonly IRefIdProperty InspectionModeIdProperty =
            P<InspectionItemBase>.RegisterRefId(e => e.InspectionModeId, ReferenceType.Normal);

        /// <summary>
        /// 检验方式Id
        /// </summary>
        public double InspectionModeId
        {
            get { return (double)this.GetRefId(InspectionModeIdProperty); }
            set { this.SetRefId(InspectionModeIdProperty, value); }
        }

        /// <summary>
        /// 检验方式
        /// </summary>
        public static readonly RefEntityProperty<InspectionMode> InspectionModeProperty =
            P<InspectionItemBase>.RegisterRef(e => e.InspectionMode, InspectionModeIdProperty);

        /// <summary>
        /// 检验方式
        /// </summary>
        public InspectionMode InspectionMode
        {
            get { return this.GetRefEntity(InspectionModeProperty); }
            set { this.SetRefEntity(InspectionModeProperty, value); }
        }
        #endregion

        #region 是否必检 IsSuitable
        /// <summary>
        /// 是否必检
        /// </summary>
        [Label("是否必检")]
        public static readonly Property<bool> IsSuitableProperty = P<InspectionItemBase>.Register(e => e.IsSuitable);

        /// <summary>
        /// 是否必检
        /// </summary>
        public bool IsSuitable
        {
            get { return GetProperty(IsSuitableProperty); }
            set { SetProperty(IsSuitableProperty, value); }
        }
        #endregion

        #region 生效日期 EffectiveStartTime
        /// <summary>
        /// 生效日期
        /// </summary>
        [Required]
        [Label("生效日期")]
        public static readonly Property<DateTime> EffectiveStartTimeProperty = P<InspectionItemBase>.Register(e => e.EffectiveStartTime, new PropertyMetadata<DateTime>() { DateTimePart = DateTimePart.Date });

        /// <summary>
        /// 生效日期
        /// </summary>
        public DateTime EffectiveStartTime
        {
            get { return GetProperty(EffectiveStartTimeProperty); }
            set { SetProperty(EffectiveStartTimeProperty, value); }
        }
        #endregion

        #region 失效日期 EffectiveEndTime
        /// <summary>
        /// 失效日期
        /// </summary>
        [Label("失效日期")]
        public static readonly Property<DateTime?> EffectiveEndTimeProperty = P<InspectionItemBase>.Register(e => e.EffectiveEndTime, new PropertyMetadata<DateTime?>() { DateTimePart = DateTimePart.Date });

        /// <summary>
        /// 失效日期
        /// </summary>
        public DateTime? EffectiveEndTime
        {
            get { return GetProperty(EffectiveEndTimeProperty); }
            set { SetProperty(EffectiveEndTimeProperty, value); }
        }
        #endregion

        #region 单位 Unit
        /// <summary>
        /// 单位Id
        /// </summary>
        [Label("单位名称")]
        public static readonly IRefIdProperty UnitIdProperty = P<InspectionItemBase>.RegisterRefId(e => e.UnitId, ReferenceType.Normal);

        /// <summary>
        /// 单位Id
        /// </summary>
        public double? UnitId
        {
            get { return (double?)GetRefNullableId(UnitIdProperty); }
            set { SetRefNullableId(UnitIdProperty, value); }
        }

        /// <summary>
        /// 单位
        /// </summary>
        public static readonly RefEntityProperty<Unit> UnitProperty = P<InspectionItemBase>.RegisterRef(e => e.Unit, UnitIdProperty);

        /// <summary>
        /// 单位
        /// </summary>
        public Unit Unit
        {
            get { return GetRefEntity(UnitProperty); }
            set { SetRefEntity(UnitProperty, value); }
        }
        #endregion

        #region 缺陷等级 DefectGrade
        /// <summary>
        /// 缺陷等级Id
        /// </summary>
        [Required]
        [Label("缺陷等级")]
        public static readonly IRefIdProperty DefectGradeIdProperty = P<InspectionItemBase>.RegisterRefId(e => e.DefectGradeId, ReferenceType.Normal);

        /// <summary>
        /// 缺陷等级Id
        /// </summary>
        public double? DefectGradeId
        {
            get { return (double?)GetRefId(DefectGradeIdProperty); }
            set { SetRefId(DefectGradeIdProperty, value); }
        }

        /// <summary>
        /// 缺陷等级
        /// </summary>
        public static readonly RefEntityProperty<DefectGrade> DefectGradeProperty = P<InspectionItemBase>.RegisterRef(e => e.DefectGrade, DefectGradeIdProperty);

        /// <summary>
        /// 缺陷等级
        /// </summary>
        public DefectGrade DefectGrade
        {
            get { return GetRefEntity(DefectGradeProperty); }
            set { SetRefEntity(DefectGradeProperty, value); }
        }
        #endregion

        #region 检验标识 CheckTag
        /// <summary>
        /// 检验标识
        /// </summary>
        [Required]
        [Label("检验标识")]
        public static readonly Property<CheckTag> CheckTagProperty = P<InspectionItemBase>.Register(e => e.CheckTag);

        /// <summary>
        /// 检验标识
        /// </summary>
        public CheckTag CheckTag
        {
            get { return GetProperty(CheckTagProperty); }
            set { SetProperty(CheckTagProperty, value); }
        }
        #endregion

        #region 规格下限判断符号 LimitLowCompare
        /// <summary>
        /// 规格下限判断符号
        /// </summary>
        [Label("规格下限判断符号")]
        public static readonly Property<CompareType?> LimitLowCompareProperty = P<InspectionItemBase>.Register(e => e.LimitLowCompare);

        /// <summary>
        /// 规格下限判断符号
        /// </summary>
        public CompareType? LimitLowCompare
        {
            get { return this.GetProperty(LimitLowCompareProperty); }
            set { this.SetProperty(LimitLowCompareProperty, value); }
        }
        #endregion

        #region 规格上限判断符号 LimitMaxCompare
        /// <summary>
        /// 规格上限判断符号
        /// </summary>
        [Label("规格上限判断符号")]
        public static readonly Property<CompareType?> LimitMaxCompareProperty = P<InspectionItemBase>.Register(e => e.LimitMaxCompare);

        /// <summary>
        /// 规格上限判断符号
        /// </summary>
        public CompareType? LimitMaxCompare
        {
            get { return this.GetProperty(LimitMaxCompareProperty); }
            set { this.SetProperty(LimitMaxCompareProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 检验项目基类配置
    /// </summary>
    internal class InspectionStandardDetailBaseConfig : EntityConfig<InspectionItemBase>
    {
        /// <summary>
        /// 添加验证规则
        /// </summary>
        /// <param name="rules">验证规则声明器</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(new HandlerRule
            {
                Handler = (o, e) =>
                {
                    var detailBase = o.CastTo<InspectionItemBase>();
                    if (detailBase.LimitLowCompare.HasValue
                        && detailBase.LimitLow.HasValue
                        && detailBase.LimitMaxCompare.HasValue
                        && detailBase.LimitMax.HasValue
                        && ((detailBase.LimitLowCompare == CompareType.GreaterThan && detailBase.LimitMaxCompare == CompareType.LessThan)
                            || detailBase.LimitLowCompare == CompareType.GreaterThan && detailBase.LimitMaxCompare == CompareType.LessThanOrEqual
                            || detailBase.LimitLowCompare == CompareType.GreaterThanOrEqual && detailBase.LimitMaxCompare == CompareType.LessThan)
                        && detailBase.LimitLow == detailBase.LimitMax)
                    {
                        e.BrokenDescription = "规格下限不能等于规格上限".L10N();
                    }

                    if (detailBase.LimitLow > detailBase.LimitMax)
                    {
                        e.BrokenDescription = "规格下限不能大于规格上限".L10N();
                    }
                }
            });
            rules.AddRule(new HandlerRule
            {
                Handler = (o, e) =>
                {
                    var detailBase = o.CastTo<InspectionItemBase>();
                    if (detailBase.EffectiveStartTime >= detailBase.EffectiveEndTime)
                    {
                        e.BrokenDescription = "失效日期必须大于生效日期".L10N();
                    }
                }
            });
            rules.AddRule(new HandlerRule
            {
                Handler = (o, e) =>
                {
                    var detailBase = o.CastTo<InspectionItemBase>();
                    if (detailBase.CheckTag == CheckTag.Quantitative)
                    {
                        if (detailBase.LimitMax == null && detailBase.LimitLow == null)
                        {
                            e.BrokenDescription = "当检验标识为定量时，规格上限和规格下限不能同时为空".L10N();
                        }
                        else if (detailBase.LimitLowCompare == null && detailBase.LimitLow != null)
                        {
                            e.BrokenDescription = "当规格下限有值时，必须选择规格下限符号".L10N();
                        }
                        else if (detailBase.LimitLowCompare != null && detailBase.LimitLow == null)
                        {
                            e.BrokenDescription = "当规格下限符号不为空时，规格下限必须有值".L10N();
                        }
                        else if (detailBase.LimitMaxCompare == null && detailBase.LimitMax != null)
                        {
                            e.BrokenDescription = "当规格上限有值时，必须选择规格上限符号".L10N();
                        }
                        else if (detailBase.LimitMaxCompare != null && detailBase.LimitMax == null)
                        {
                            e.BrokenDescription = "当规格上限符号不为空时，规格上限必须有值".L10N();
                        }
                        else if (detailBase.Unit == null)
                        {
                            e.BrokenDescription = "当检验标识为定量时，单位不能为空".L10N();
                        }
                    }
                }
            });
        }
    }
}