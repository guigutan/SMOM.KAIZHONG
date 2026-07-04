using SIE.Common.Configs;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.ProcessSegments;
using SIE.Resources.ProcessTechs.Configs;
using SIE.Resources.ProcessTechTypes;
using System;

namespace SIE.Resources.ProcessTechs
{
    /// <summary>
    /// 制程工艺
    /// </summary>
    [RootEntity, Serializable]
    //[CriteriaQuery]
    [EntityWithConfig(typeof(ProcessTechNoConfig))]
    [ConditionQueryType(typeof(ProcessTechCriteria))]
    [DisplayMember(nameof(Code))]
    [Label("制程工艺")]
    public partial class ProcessTech : DataEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProcessTech()
        {
            CreateBy = RT.IdentityId;
            CreateDate = DateTime.Now;
            UpdateBy = RT.IdentityId;
            UpdateDate = DateTime.Now;
            IsScheduling = true;
        }

        #region 编号 Code
        /// <summary>
        /// 编号
        /// </summary>
        [Required]
        [NotDuplicate]
        [MaxLength(80)]
        [Label("编号")]
        public static readonly Property<string> CodeProperty = P<ProcessTech>.Register(e => e.Code);

        /// <summary>
        /// 编号
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
        [NotDuplicate]
        [MaxLength(80)]
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<ProcessTech>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 是否排产 IsScheduling
        /// <summary>
        /// 是否排产
        /// </summary>
        [Label("是否排产")]
        public static readonly Property<bool> IsSchedulingProperty = P<ProcessTech>.Register(e => e.IsScheduling);

        /// <summary>
        /// 是否排产
        /// </summary>
        public bool IsScheduling
        {
            get { return GetProperty(IsSchedulingProperty); }
            set { SetProperty(IsSchedulingProperty, value); }
        }
        #endregion

        #region 是否瓶颈 IsBottleneck
        /// <summary>
        /// 是否瓶颈
        /// </summary>
        [Label("是否瓶颈")]
        public static readonly Property<bool> IsBottleneckProperty = P<ProcessTech>.Register(e => e.IsBottleneck);

        /// <summary>
        /// 是否瓶颈
        /// </summary>
        public bool IsBottleneck
        {
            get { return GetProperty(IsBottleneckProperty); }
            set { SetProperty(IsBottleneckProperty, value); }
        }
        #endregion

        #region 偏移时间(s/单位) OffsetTime
        /// <summary>
        /// 偏移时间(s/单位)
        /// </summary>
        [MinValue(0)]
        [Label("偏移时间(s/单位)")]
        public static readonly Property<decimal?> OffsetTimeProperty = P<ProcessTech>.Register(e => e.OffsetTime);

        /// <summary>
        /// 偏移时间(s/单位)
        /// </summary>
        public decimal? OffsetTime
        {
            get { return GetProperty(OffsetTimeProperty); }
            set { SetProperty(OffsetTimeProperty, value); }
        }
        #endregion

        #region 转款时间(s/单位) TransferTime
        /// <summary>
        /// 转款时间(s/单位)
        /// </summary>
        [MinValue(0)]
        [Label("转款时间(s/单位)")]
        public static readonly Property<decimal?> TransferTimeProperty = P<ProcessTech>.Register(e => e.TransferTime);

        /// <summary>
        /// 转款时间(s/单位)
        /// </summary>
        public decimal? TransferTime
        {
            get { return GetProperty(TransferTimeProperty); }
            set { SetProperty(TransferTimeProperty, value); }
        }
        #endregion

        #region 标准工时(s/单位) SAM
        /// <summary>
        /// 标准工时(s/单位)
        /// </summary>
        [MinValue(0)]
        [Label("标准工时(s/单位)")]
        public static readonly Property<decimal?> SAMProperty = P<ProcessTech>.Register(e => e.SAM);

        /// <summary>
        /// 标准工时(s/单位)
        /// </summary>
        public decimal? SAM
        {
            get { return GetProperty(SAMProperty); }
            set { SetProperty(SAMProperty, value); }
        }
        #endregion

        #region 标记颜色 MarkColor
        /// <summary>
        /// 标记颜色
        /// </summary>
        [Label("标记颜色")]
        public static readonly Property<string> MarkColorProperty = P<ProcessTech>.Register(e => e.MarkColor);

        /// <summary>
        /// 标记颜色
        /// </summary>
        public string MarkColor
        {
            get { return GetProperty(MarkColorProperty); }
            set { SetProperty(MarkColorProperty, value); }
        }
        #endregion

        #region 工作时长（时） WorkingHours
        /// <summary>
        /// 工作时长(时)
        /// </summary>
        [MinValue(1)]
        [Label("工作时长(时)")]
        public static readonly Property<decimal> WorkingHoursProperty = P<ProcessTech>.Register(e => e.WorkingHours);

        /// <summary>
        /// 工作时长
        /// </summary>
        public decimal WorkingHours
        {
            get { return GetProperty(WorkingHoursProperty); }
            set { SetProperty(WorkingHoursProperty, value); }
        }
        #endregion

        #region 算法类型 AlgorithmMarking
        /// <summary>
        /// 算法类型
        /// </summary>
        [Label("算法类型")]
        public static readonly Property<AlgorithmMarking> AlgorithmMarkingProperty = P<ProcessTech>.Register(e => e.AlgorithmMarking);

        /// <summary>
        /// 算法类型
        /// </summary>
        public AlgorithmMarking AlgorithmMarking
        {
            get { return GetProperty(AlgorithmMarkingProperty); }
            set { SetProperty(AlgorithmMarkingProperty, value); }
        }
        #endregion

        #region 制程工艺类型 ProcessTechType
        /// <summary>
        /// 制程工艺类型Id
        /// </summary>
        [Label("制程工艺类型")]
        public static readonly IRefIdProperty ProcessTechTypeIdProperty = P<ProcessTech>.RegisterRefId(e => e.ProcessTechTypeId, ReferenceType.Normal);

        /// <summary>
        /// 制程工艺类型Id
        /// </summary>
        public double ProcessTechTypeId
        {
            get { return (double)GetRefId(ProcessTechTypeIdProperty); }
            set { SetRefId(ProcessTechTypeIdProperty, value); }
        }

        /// <summary>
        /// 制程工艺类型
        /// </summary>
        public static readonly RefEntityProperty<ProcessTechType> ProcessTechTypeProperty = P<ProcessTech>.RegisterRef(e => e.ProcessTechType, ProcessTechTypeIdProperty);

        /// <summary>
        /// 制程工艺类型
        /// </summary>
        public ProcessTechType ProcessTechType
        {
            get { return GetRefEntity(ProcessTechTypeProperty); }
            set { SetRefEntity(ProcessTechTypeProperty, value); }
        }
        #endregion

        #region 工段 ProcessSegment
        /// <summary>
        /// 工段Id
        /// </summary>
        [Label("工段")]
        public static readonly IRefIdProperty ProcessSegmentIdProperty = P<ProcessTech>.RegisterRefId(e => e.ProcessSegmentId, ReferenceType.Normal);

        /// <summary>
        /// 工段Id
        /// </summary>
        public double? ProcessSegmentId
        {
            get { return (double?)GetRefNullableId(ProcessSegmentIdProperty); }
            set { SetRefNullableId(ProcessSegmentIdProperty, value); }
        }

        /// <summary>
        /// 工段
        /// </summary>
        public static readonly RefEntityProperty<ProcessSegment> ProcessSegmentProperty = P<ProcessTech>.RegisterRef(e => e.ProcessSegment, ProcessSegmentIdProperty);

        /// <summary>
        /// 工段
        /// </summary>
        public ProcessSegment ProcessSegment
        {
            get { return GetRefEntity(ProcessSegmentProperty); }
            set { SetRefEntity(ProcessSegmentProperty, value); }
        }
        #endregion

        #region 工段名称 ProcessSegmentName
        /// <summary>
        /// 工段名称
        /// </summary>
        [Label("工段名称")]
        public static readonly Property<string> ProcessSegmentNameProperty = P<ProcessTech>.RegisterView(e => e.ProcessSegmentName, p => p.ProcessSegment.Name);

        /// <summary>
        /// 工段名称
        /// </summary>
        public string ProcessSegmentName
        {
            get { return this.GetProperty(ProcessSegmentNameProperty); }
        }
        #endregion

        #region 外协时长（天） OutAssistDay
        /// <summary>
        /// 外协时长（天）
        /// </summary>
        [Label("外协时长（天）")]
        public static readonly Property<decimal?> OutAssistDayProperty = P<ProcessTech>.Register(e => e.OutAssistDay);

        /// <summary>
        /// 外协时长（天）
        /// </summary>
        public decimal? OutAssistDay
        {
            get { return this.GetProperty(OutAssistDayProperty); }
            set { this.SetProperty(OutAssistDayProperty, value); }
        }
        #endregion


        /// <summary>
        /// 属性变更
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == nameof(ProcessTechTypeId) || propertyName == nameof(ProcessTechType))
            {
                if (ProcessTechType != null)
                    AlgorithmMarking = ProcessTechType.AlgorithmMarking;
            }
        }
    }

    /// <summary>
    /// 制程工艺 实体配置
    /// </summary>
    internal class ProcessTechConfig : EntityConfig<ProcessTech>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("APS_PROTECH").MapAllProperties();
            Meta.Property(ProcessTech.CodeProperty).ColumnMeta.HasLength(160);
            Meta.Property(ProcessTech.NameProperty).ColumnMeta.HasLength(160);
            Meta.EnablePhantoms();
        }
    }
}