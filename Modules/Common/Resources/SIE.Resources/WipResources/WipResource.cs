using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.CalendarSchemes;
using SIE.Resources.Enterprises;
using SIE.Resources.ProcessTechTypes;
using System;

namespace SIE.Resources.WipResources
{
    /// <summary>
    /// 生产资源
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(WipResourceCriteria))]
    [Label("生产资源")]
    //[EntityWithConfig(typeof(ResourceNoConfig))]
    [DisplayMember(nameof(Code))]
    public partial class WipResource : DataEntity
    {
        /// <summary>
        /// 快码类型：资源类型
        /// </summary>
        public static string ResourceTypeString { get; set; } = "RESOURCE_TYPE";

        #region 资源编号 Code
        /// <summary>
        /// 资源编号
        /// </summary>
        [Label("资源编号")]
        [Required]
        [NotDuplicate]
        [MaxLength(40)]
        public static readonly Property<string> CodeProperty = P<WipResource>.Register(e => e.Code);

        /// <summary>
        /// 资源编号
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 资源名称 Name
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源名称")]
        [Required]
        //[NotDuplicate]
        [MaxLength(40)]
        public static readonly Property<string> NameProperty = P<WipResource>.Register(e => e.Name);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 资源数量 Qty
        /// <summary>
        /// 资源数量
        /// </summary>
        [Label("资源数量")]
        [MinValue(0)]
        public static readonly Property<int> QtyProperty = P<WipResource>.Register(e => e.Qty);

        /// <summary>
        /// 资源数量
        /// </summary>
        public int Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 是否外协 IsOutMade
        /// <summary>
        /// 是否外协
        /// </summary>
        [Label("是否外协")]
        public static readonly Property<bool> IsOutMadeProperty = P<WipResource>.Register(e => e.IsOutMade);

        /// <summary>
        /// 是否外协
        /// </summary>
        public bool IsOutMade
        {
            get { return GetProperty(IsOutMadeProperty); }
            set { SetProperty(IsOutMadeProperty, value); }
        }
        #endregion

        #region 启用状态 ResourceState
        /// <summary>
        /// 启用状态
        /// </summary>
        [Label("启用状态")]
        public static readonly Property<ResourceState> ResourceStateProperty = P<WipResource>.Register(e => e.ResourceState);

        /// <summary>
        /// 启用状态
        /// </summary>
        public ResourceState ResourceState
        {
            get { return GetProperty(ResourceStateProperty); }
            set { SetProperty(ResourceStateProperty, value); }
        }
        #endregion

        #region 日历方案 Scheme
        /// <summary>
        /// 日历方案Id
        /// </summary>
        [Label("日历方案")]
        public static readonly IRefIdProperty SchemeIdProperty = P<WipResource>.RegisterRefId(e => e.SchemeId, ReferenceType.Normal);

        /// <summary>
        /// 日历方案Id
        /// </summary>
        public double SchemeId
        {
            get { return (double)GetRefId(SchemeIdProperty); }
            set { SetRefId(SchemeIdProperty, value); }
        }

        /// <summary>
        /// 日历方案
        /// </summary>
        public static readonly RefEntityProperty<CalendarScheme> SchemeProperty = P<WipResource>.RegisterRef(e => e.Scheme, SchemeIdProperty);

        /// <summary>
        /// 日历方案
        /// </summary>
        public CalendarScheme Scheme
        {
            get { return GetRefEntity(SchemeProperty); }
            set { SetRefEntity(SchemeProperty, value); }
        }
        #endregion

        #region 算法类型 AlgorithmMarking
        /// <summary>
        /// 算法类型
        /// </summary>
        [Label("算法类型")]
        public static readonly Property<AlgorithmMarking?> AlgorithmMarkingProperty = P<WipResource>.Register(e => e.AlgorithmMarking);

        /// <summary>
        /// 算法类型
        /// </summary>
        public AlgorithmMarking? AlgorithmMarking
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
        public static readonly IRefIdProperty ProcessTechTypeIdProperty = P<WipResource>.RegisterRefId(e => e.ProcessTechTypeId, ReferenceType.Normal);

        /// <summary>
        /// 制程工艺类型Id
        /// </summary>
        public double? ProcessTechTypeId
        {
            get { return (double?)GetRefNullableId(ProcessTechTypeIdProperty); }
            set { SetRefNullableId(ProcessTechTypeIdProperty, value); }
        }

        /// <summary>
        /// 制程工艺类型
        /// </summary>
        public static readonly RefEntityProperty<ProcessTechType> ProcessTechTypeProperty = P<WipResource>.RegisterRef(e => e.ProcessTechType, ProcessTechTypeIdProperty);

        /// <summary>
        /// 制程工艺类型
        /// </summary>
        public ProcessTechType ProcessTechType
        {
            get { return GetRefEntity(ProcessTechTypeProperty); }
            set { SetRefEntity(ProcessTechTypeProperty, value); }
        }
        #endregion

        #region 所属车间 WorkShop
        /// <summary>
        /// 所属车间Id
        /// </summary>
        [Label("所属车间")]
        public static readonly IRefIdProperty WorkShopIdProperty = P<WipResource>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

        /// <summary>
        /// 所属车间Id
        /// </summary>
        public double? WorkShopId
        {
            get { return (double?)GetRefNullableId(WorkShopIdProperty); }
            set { SetRefNullableId(WorkShopIdProperty, value); }
        }

        /// <summary>
        /// 所属车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty = P<WipResource>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 所属车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return GetRefEntity(WorkShopProperty); }
            set { SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 所属工厂 Factory
        /// <summary>
        /// 所属工厂Id
        /// </summary>
        [Label("所属工厂")]
        public static readonly IRefIdProperty FactoryIdProperty = P<WipResource>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 所属工厂Id
        /// </summary>
        public double? FactoryId
        {
            get { return (double?)GetRefNullableId(FactoryIdProperty); }
            set { SetRefNullableId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 所属工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<WipResource>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 所属工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return GetRefEntity(FactoryProperty); }
            set { SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 来源类型 SourceType
        /// <summary>
        /// 来源类型
        /// </summary>
        [Label("来源类型")]
        public static readonly Property<SyncSourceType> SourceTypeProperty = P<WipResource>.Register(e => e.SourceType);

        /// <summary>
        /// 来源类型
        /// </summary>
        public SyncSourceType SourceType
        {
            get { return this.GetProperty(SourceTypeProperty); }
            set { this.SetProperty(SourceTypeProperty, value); }
        }
        #endregion

        #region 来源ID SourceId
        /// <summary>
        /// 来源ID
        /// </summary>
        [Label("来源ID")]
        public static readonly Property<double?> SourceIdProperty = P<WipResource>.Register(e => e.SourceId);

        /// <summary>
        /// 来源ID
        /// </summary>
        public double? SourceId
        {
            get { return this.GetProperty(SourceIdProperty); }
            set { this.SetProperty(SourceIdProperty, value); }
        }
        #endregion

        #region 是否小单线 IsSmallSingle
        /// <summary>
        /// 是否小单线
        /// </summary>
        [Label("是否小单线")]
        public static readonly Property<bool> IsSmallSingleProperty = P<WipResource>.Register(e => e.IsSmallSingle);

        /// <summary>
        /// 是否小单线
        /// </summary>
        public bool IsSmallSingle
        {
            get { return this.GetProperty(IsSmallSingleProperty); }
            set { this.SetProperty(IsSmallSingleProperty, value); }
        }
        #endregion

        #region 节拍（秒） TaktTime
        /// <summary>
        /// 节拍（秒）
        /// </summary>
        [Label("节拍（秒）")]
        [MinValue(0)]
        public static readonly Property<int?> TaktTimeProperty = P<WipResource>.Register(e => e.TaktTime);

        /// <summary>
        /// 节拍（秒）
        /// </summary>
        public int? TaktTime
        {
            get { return GetProperty(TaktTimeProperty); }
            set { SetProperty(TaktTimeProperty, value); }
        }
        #endregion

        #region 显示顺序 Sequence
        /// <summary>
        /// 显示顺序
        /// </summary>
        [Label("显示顺序")]
        [MinValue(0)]
        public static readonly Property<int> SequenceProperty = P<WipResource>.Register(e => e.Sequence);

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int Sequence
        {
            get { return GetProperty(SequenceProperty); }
            set { SetProperty(SequenceProperty, value); }
        }
        #endregion

        #region 自动化类型 AutomationType
        /// <summary>
        /// 自动化类型
        /// </summary>
        [Label("自动化类型")]
        public static readonly Property<AutomationType?> AutomationTypeProperty = P<WipResource>.Register(e => e.AutomationType);

        /// <summary>
        /// 自动化类型
        /// </summary>
        public AutomationType? AutomationType
        {
            get { return GetProperty(AutomationTypeProperty); }
            set { SetProperty(AutomationTypeProperty, value); }
        }
        #endregion

        #region 父生产资源ID ParentResourceId
        /// <summary>
        /// 父生产资源ID
        /// </summary>
        [Label("父生产资源ID")]
        public static readonly Property<double?> ParentResourceIdProperty = P<WipResource>.Register(e => e.ParentResourceId);

        /// <summary>
        /// 父生产资源ID
        /// </summary>
        public double? ParentResourceId
        {
            get { return GetProperty(ParentResourceIdProperty); }
            set { SetProperty(ParentResourceIdProperty, value); }
        }
        #endregion

        #region 资源类型 ResourceType
        /// <summary>
        /// 资源类型
        /// </summary>
        [Label("资源类型")]
        public static readonly Property<string> ResourceTypeProperty = P<WipResource>.Register(e => e.ResourceType);

        /// <summary>
        /// 资源类型
        /// </summary>
        public string ResourceType
        {
            get { return this.GetProperty(ResourceTypeProperty); }
            set { this.SetProperty(ResourceTypeProperty, value); }
        }
        #endregion

        #region 区域描述 AndonUphold
        /// <summary>
        /// 区域描述Id
        /// </summary>
        [Label("区域描述")]
        public static readonly IRefIdProperty AndonUpholdIdProperty =
            P<WipResource>.RegisterRefId(e => e.AndonUpholdId, ReferenceType.Normal);

        /// <summary>
        /// 区域描述Id
        /// </summary>
        public double? AndonUpholdId
        {
            get { return (double?)this.GetRefNullableId(AndonUpholdIdProperty); }
            set { this.SetRefNullableId(AndonUpholdIdProperty, value); }
        }

        /// <summary>
        /// 区域描述
        /// </summary>
        public static readonly RefEntityProperty<AndonUphold> AndonUpholdProperty =
            P<WipResource>.RegisterRef(e => e.AndonUphold, AndonUpholdIdProperty);

        /// <summary>
        /// 区域描述
        /// </summary>
        public AndonUphold AndonUphold
        {
            get { return this.GetRefEntity(AndonUpholdProperty); }
            set { this.SetRefEntity(AndonUpholdProperty, value); }
        }
        #endregion

        #region 安灯编码 AndonCode
        /// <summary>
        /// 安灯编码
        /// </summary>
        [Label("安灯编码")]
        public static readonly Property<string> AndonCodeProperty = P<WipResource>.Register(e => e.AndonCode);

        /// <summary>
        /// 安灯编码
        /// </summary>
        public string AndonCode
        {
            get { return this.GetProperty(AndonCodeProperty); }
            set { this.SetProperty(AndonCodeProperty, value); }
        }
        #endregion


        #region 注册视图属性(关联实体属性平铺显示，一般用于Web)
        #region 车间名称 WorkShopName
        /// <summary>
        /// 车间名称
        /// </summary>
        [Label("车间名称")]
        public static readonly Property<string> WorkShopNameProperty = P<WipResource>.RegisterView(e => e.WorkShopName, p => p.WorkShop.Name);

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkShopName
        {
            get { return this.GetProperty(WorkShopNameProperty); }
        }
        #endregion

        #region 生产日历名称 SchemeName
        /// <summary>
        /// 生产日历名称
        /// </summary>
        [Label("生产日历名称")]
        public static readonly Property<string> SchemeNameProperty = P<WipResource>.RegisterView(e => e.SchemeName, p => p.Scheme.Name);

        /// <summary>
        /// 生产日历名称
        /// </summary>
        public string SchemeName
        {
            get { return this.GetProperty(SchemeNameProperty); }
        }
        #endregion

        #region 车间编码 WorkShopCode
        /// <summary>
        /// 车间编码
        /// </summary>
        [Label("车间编码")]
        public static readonly Property<string> WorkShopCodeProperty = P<WipResource>.RegisterView(e => e.WorkShopCode, p => p.WorkShop.Code);

        /// <summary>
        /// 车间编码
        /// </summary>
        public string WorkShopCode
        {
            get { return this.GetProperty(WorkShopCodeProperty); }
        }
        #endregion

        #region 工厂编码 FactoryName
        /// <summary>
        /// 工厂编码
        /// </summary>
        [Label("工厂编码")]
        public static readonly Property<string> FactoryNameProperty = P<WipResource>.RegisterView(e => e.FactoryName, p => p.Factory.Name);

        /// <summary>
        /// 工厂编码
        /// </summary>
        public string FactoryName
        {
            get { return this.GetProperty(FactoryNameProperty); }
        }
        #endregion

        #endregion

        /// <summary>
        /// 属性变更
        /// </summary>
        /// <param name="propertyName"></param>
        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            if ((propertyName == nameof(ProcessTechTypeId) || propertyName == nameof(ProcessTechType)) && (ProcessTechTypeId != null || ProcessTechType != null))
            {
                if (ProcessTechType != null)
                    AlgorithmMarking = ProcessTechType.AlgorithmMarking;
            }
        }
    }

    /// <summary>
    /// 生产资源 实体配置
    /// </summary>
    internal class WipResourceConfig : EntityConfig<WipResource>
    {
        /// <summary>
        /// 实体配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("RES_WIP_SCHE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}