using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using SIE.Tech.VictoryStandards;
using System; 

namespace SIE.Tech.Routings
{
    /// <summary>
    /// 工序清单
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工序清单")]
    [DisplayMember(nameof(Name))]
    public partial class RoutingProcess : DataEntity
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public RoutingProcess()
        {
            this.Index = -1;
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(40)]
        public static readonly Property<string> NameProperty = P<RoutingProcess>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 标识ID ActivityId
        /// <summary>
        /// 标识ID
        /// </summary>
        [MaxLength(80)]
        public static readonly Property<string> ActivityIdProperty = P<RoutingProcess>.Register(e => e.ActivityId);

        /// <summary>
        /// 标识ID
        /// </summary>
        public string ActivityId
        {
            get { return GetProperty(ActivityIdProperty); }
            set { SetProperty(ActivityIdProperty, value); }
        }
        #endregion

        #region 引用数量 ReferenceQty
        /// <summary>
        /// 引用数量
        /// </summary>
        public static readonly Property<int> ReferenceQtyProperty = P<RoutingProcess>.Register(e => e.ReferenceQty);

        /// <summary>
        /// 引用数量
        /// </summary>
        public int ReferenceQty
        {
            get { return GetProperty(ReferenceQtyProperty); }
            set { SetProperty(ReferenceQtyProperty, value); }
        }
        #endregion

        #region 工段ID ProcessSegmentId
        /// <summary>
        /// 工段ID
        /// </summary>
        public static readonly Property<double?> ProcessSegmentIdProperty = P<RoutingProcess>.Register(e => e.ProcessSegmentId);

        /// <summary>
        /// 工段ID
        /// </summary>
        public double? ProcessSegmentId
        {
            get { return GetProperty(ProcessSegmentIdProperty); }
            set { SetProperty(ProcessSegmentIdProperty, value); }
        }
        #endregion

        #region 是否可选 IsOptional
        /// <summary>
        /// 是否可选
        /// </summary>
        public static readonly Property<bool> IsOptionalProperty = P<RoutingProcess>.Register(e => e.IsOptional);

        /// <summary>
        /// 是否可选
        /// </summary>
        public bool IsOptional
        {
            get { return GetProperty(IsOptionalProperty); }
            set { SetProperty(IsOptionalProperty, value); }
        }
        #endregion

        #region 是否重复过站 IsRepeat
        /// <summary>
        /// 是否重复过站
        /// </summary>
        public static readonly Property<bool> IsRepeatProperty = P<RoutingProcess>.Register(e => e.IsRepeat);

        /// <summary>
        /// 是否重复过站
        /// </summary>
        public bool IsRepeat
        {
            get { return GetProperty(IsRepeatProperty); }
            set { SetProperty(IsRepeatProperty, value); }
        }
        #endregion

        #region 是否生成工序任务 IsGenerateTask
        /// <summary>
        /// 是否生成工序任务
        /// </summary>        
        public static readonly Property<bool> IsGenerateTaskProperty = P<RoutingProcess>.Register(e => e.IsGenerateTask);

        /// <summary>
        /// 是否生成工序任务
        /// </summary>
        public bool IsGenerateTask
        {
            get { return this.GetProperty(IsGenerateTaskProperty); }
            set { this.SetProperty(IsGenerateTaskProperty, value); }
        }
        #endregion 
        #region 是否需求任务清单 IsRequirementTask
        /// <summary>
        /// 是否需求任务清单
        /// </summary>        
        public static readonly Property<bool> IsRequirementTaskProperty = P<RoutingProcess>.Register(e => e.IsRequirementTask);

        /// <summary>
        /// 是否需求任务清单
        /// </summary>
        public bool IsRequirementTask
        {
            get { return this.GetProperty(IsRequirementTaskProperty); }
            set { this.SetProperty(IsRequirementTaskProperty, value); }
        }
        #endregion 

        

        #region 清单标记 ProcessSign
        /// <summary>
        /// 清单标记
        /// </summary>
        public static readonly Property<RoutingProcessSign> ProcessSignProperty = P<RoutingProcess>.Register(e => e.ProcessSign);

        /// <summary>
        /// 清单标记
        /// </summary>
        public RoutingProcessSign ProcessSign
        {
            get { return GetProperty(ProcessSignProperty); }
            set { SetProperty(ProcessSignProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        public static readonly IRefIdProperty ProcessIdProperty = P<RoutingProcess>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)GetRefNullableId(ProcessIdProperty); }
            set { SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<RoutingProcess>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 工序类型 Type
        /// <summary>
        /// 工序类型
        /// </summary>
        public static readonly Property<ProcessType> TypeProperty = P<RoutingProcess>.Register(e => e.Type);

        /// <summary>
        /// 工序类型
        /// </summary>
        public ProcessType Type
        {
            get { return GetProperty(TypeProperty); }
            set { SetProperty(TypeProperty, value); }
        }
        #endregion

        #region 参数列表 ParameterList
        /// <summary>
        /// 参数列表
        /// </summary>
        public static readonly ListProperty<EntityList<RoutingProcessParameter>> ParameterListProperty = P<RoutingProcess>.RegisterList(e => e.ParameterList);

        /// <summary>
        /// 参数列表
        /// </summary>
        public EntityList<RoutingProcessParameter> ParameterList
        {
            get { return this.GetLazyList(ParameterListProperty); }
        }
        #endregion

        #region 缺陷列表 DefectList
        /// <summary>
        /// 缺陷列表
        /// </summary>
        public static readonly ListProperty<EntityList<RoutingProcessDefect>> DefectListProperty = P<RoutingProcess>.RegisterList(e => e.DefectList);

        /// <summary>
        /// 缺陷列表
        /// </summary>
        public EntityList<RoutingProcessDefect> DefectList
        {
            get { return this.GetLazyList(DefectListProperty); }
        }
        #endregion

        #region BOM配置列表 BomConfigList
        /// <summary>
        /// BOM配置列表
        /// </summary>
        public static readonly ListProperty<EntityList<RoutingProcessBomConfig>> BomConfigListProperty = P<RoutingProcess>.RegisterList(e => e.BomConfigList);

        /// <summary>
        /// BOM配置列表
        /// </summary>
        public EntityList<RoutingProcessBomConfig> BomConfigList
        {
            get { return this.GetLazyList(BomConfigListProperty); }
        }
        #endregion

        #region 采集步骤列表 CollectStepList
        /// <summary>
        /// 采集步骤列表
        /// </summary>
        public static readonly ListProperty<EntityList<RoutingProcessCollectStep>> CollectStepListProperty = P<RoutingProcess>.RegisterList(e => e.CollectStepList);

        /// <summary>
        /// 采集步骤列表
        /// </summary>
        public EntityList<RoutingProcessCollectStep> CollectStepList
        {
            get { return this.GetLazyList(CollectStepListProperty); }
        }
        #endregion

        #region 工艺路线版本 Version
        /// <summary>
        /// 工艺路线版本Id
        /// </summary>
        public static readonly IRefIdProperty VersionIdProperty = P<RoutingProcess>.RegisterRefId(e => e.VersionId, ReferenceType.Parent);

        /// <summary>
        /// 工艺路线版本Id
        /// </summary>
        public double? VersionId
        {
            get { return (double?)GetRefNullableId(VersionIdProperty); }
            set { SetRefNullableId(VersionIdProperty, value); }
        }

        /// <summary>
        /// 工艺路线版本
        /// </summary>
        public static readonly RefEntityProperty<RoutingVersion> VersionProperty = P<RoutingProcess>.RegisterRef(e => e.Version, VersionIdProperty);

        /// <summary>
        /// 工艺路线版本
        /// </summary>
        public RoutingVersion Version
        {
            get { return GetRefEntity(VersionProperty); }
            set { SetRefEntity(VersionProperty, value); }
        }
        #endregion

        #region 是否创建SKU CreateSku
        /// <summary>
        /// 是否创建SKU
        /// </summary>
        public static readonly Property<bool> CreateSkuProperty = P<RoutingProcess>.Register(e => e.CreateSku);

        /// <summary>
        /// 是否创建SKU
        /// </summary>
        public bool CreateSku
        {
            get { return this.GetProperty(CreateSkuProperty); }
            set { this.SetProperty(CreateSkuProperty, value); }
        }
        #endregion

        #region 是否计产 IsCalculate
        /// <summary>
        /// 是否计产
        /// </summary>
        [Label("是否计产")]
        public static readonly Property<bool?> IsCalculateProperty = P<RoutingProcess>.Register(e => e.IsCalculate);

        /// <summary>   
        /// 是否计产
        /// </summary>
        public bool? IsCalculate
        {
            get { return this.GetProperty(IsCalculateProperty); }
            set { this.SetProperty(IsCalculateProperty, value); }
        }
        #endregion

        #region 是否扣料 IsBuckleMaterial
        /// <summary>
        /// 是否扣料
        /// </summary>
        [Label("是否扣料")]
        public static readonly Property<bool> IsBuckleMaterialProperty = P<RoutingProcess>.Register(e => e.IsBuckleMaterial);

        /// <summary>
        /// 是否扣料
        /// </summary>
        public bool IsBuckleMaterial
        {
            get { return this.GetProperty(IsBuckleMaterialProperty); }
            set { this.SetProperty(IsBuckleMaterialProperty, value); }
        }
        #endregion

        #region 起始工序 StartProcess
        /// <summary>
        /// 起始工序
        /// </summary>
        [Label("起始工序")]
        public static readonly Property<double?> StartProcessProperty = P<RoutingProcess>.Register(e => e.StartProcess);

        /// <summary>
        /// 起始工序
        /// </summary>
        public double? StartProcess
        {
            get { return this.GetProperty(StartProcessProperty); }
            set { this.SetProperty(StartProcessProperty, value); }
        }
        #endregion

        #region 正常胜制 NormalVictory
        /// <summary>
        /// 正常胜制Id
        /// </summary>
        [Label("正常胜制")]
        public static readonly IRefIdProperty NormalVictoryIdProperty =
            P<RoutingProcess>.RegisterRefId(e => e.NormalVictoryId, ReferenceType.Normal);

        /// <summary>
        /// 正常胜制Id
        /// </summary>
        public double? NormalVictoryId
        {
            get { return (double?)this.GetRefNullableId(NormalVictoryIdProperty); }
            set { this.SetRefNullableId(NormalVictoryIdProperty, value); }
        }

        /// <summary>
        /// 正常胜制
        /// </summary>
        public static readonly RefEntityProperty<VictoryStandard> NormalVictoryProperty =
            P<RoutingProcess>.RegisterRef(e => e.NormalVictory, NormalVictoryIdProperty);

        /// <summary>
        /// 正常胜制
        /// </summary>
        public VictoryStandard NormalVictory
        {
            get { return this.GetRefEntity(NormalVictoryProperty); }
            set { this.SetRefEntity(NormalVictoryProperty, value); }
        }
        #endregion 

        #region 维修胜制 RepairVictory
        /// <summary>
        /// 维修胜制Id
        /// </summary>
        [Label("维修胜制")]
        public static readonly IRefIdProperty RepairVictoryIdProperty =
            P<RoutingProcess>.RegisterRefId(e => e.RepairVictoryId, ReferenceType.Normal);

        /// <summary>
        /// 维修胜制Id
        /// </summary>
        public double? RepairVictoryId
        {
            get { return (double?)this.GetRefNullableId(RepairVictoryIdProperty); }
            set { this.SetRefNullableId(RepairVictoryIdProperty, value); }
        }

        /// <summary>
        /// 维修胜制
        /// </summary>
        public static readonly RefEntityProperty<VictoryStandard> RepairVictoryProperty =
            P<RoutingProcess>.RegisterRef(e => e.RepairVictory, RepairVictoryIdProperty);

        /// <summary>
        /// 维修胜制
        /// </summary>
        public VictoryStandard RepairVictory
        {
            get { return this.GetRefEntity(RepairVictoryProperty); }
            set { this.SetRefEntity(RepairVictoryProperty, value); }
        }
        #endregion

        #region 是否加严 IsStricter
        /// <summary>
        /// 是否加严
        /// </summary>
        [Label("加严")]
        public static readonly Property<bool> IsStricterProperty = P<RoutingProcess>.Register(e => e.IsStricter);

        /// <summary>
        /// 是否加严
        /// </summary>
        public bool IsStricter
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
        public static readonly Property<int?> OvertimeProperty = P<RoutingProcess>.Register(e => e.Overtime);

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
        public static readonly Property<bool> IsPassRateProperty = P<RoutingProcess>.Register(e => e.IsPassRate);

        /// <summary>
        /// 直通率取值
        /// </summary>
        public bool IsPassRate
        {
            get { return GetProperty(IsPassRateProperty); }
            set { SetProperty(IsPassRateProperty, value); }
        }
        #endregion

        #region 绑定 IsBinding
        /// <summary>
        /// 绑定
        /// </summary>
        public static readonly Property<bool> IsBindingProperty = P<RoutingProcess>.Register(e => e.IsBinding);

        /// <summary>
        /// 绑定
        /// </summary>
        public bool IsBinding
        {
            get { return GetProperty(IsBindingProperty); }
            set { SetProperty(IsBindingProperty, value); }
        }
        #endregion

        #region 解绑 IsPassRate
        /// <summary>
        /// 解绑
        /// </summary>
        public static readonly Property<bool> IsUnBindingProperty = P<RoutingProcess>.Register(e => e.IsUnBinding);

        /// <summary>
        /// 解绑
        /// </summary>
        public bool IsUnBinding
        {
            get { return GetProperty(IsUnBindingProperty); }
            set { SetProperty(IsUnBindingProperty, value); }
        }
        #endregion

        #region 顺序 Index
        /// <summary>
        /// 顺序
        /// </summary>
        [Label("顺序")]
        public static readonly Property<int> IndexProperty = P<RoutingProcess>.Register(e => e.Index);

        /// <summary>
        /// 顺序
        /// </summary>
        public int Index
        {
            get { return this.GetProperty(IndexProperty); }
            set { this.SetProperty(IndexProperty, value); }
        }
        #endregion

        #region 最大过站次数 MaxPassNum
        /// <summary>
        /// 最大过站次数
        /// </summary>
        [Label("最大过站次数")]
        public static readonly Property<int?> MaxPassNumProperty = P<RoutingProcess>.Register(e => e.MaxPassNum);

        /// <summary>
        /// 最大过站次数
        /// </summary>
        public int? MaxPassNum
        {
            get { return this.GetProperty(MaxPassNumProperty); }
            set { this.SetProperty(MaxPassNumProperty, value); }
        }
        #endregion

        #region 启用入站控制 EnableMoveInControl
        /// <summary>
        /// 启用入站控制
        /// </summary>
        [Label("启用入站控制")]
        public static readonly Property<bool?> EnableMoveInControlProperty = P<RoutingProcess>.Register(e => e.EnableMoveInControl);

        /// <summary>
        /// 启用入站控制
        /// </summary>
        public bool? EnableMoveInControl
        {
            get { return this.GetProperty(EnableMoveInControlProperty); }
            set { this.SetProperty(EnableMoveInControlProperty, value); }
        }
        #endregion

        #region 工序交接 TransferType
        /// <summary>
        /// 工序交接
        /// </summary>
        [Label("工序交接")]
        public static readonly Property<TransferType?> TransferTypeProperty = P<RoutingProcess>.Register(e => e.TransferType);

        /// <summary>
        /// 工序交接
        /// </summary>
        public TransferType? TransferType
        {
            get { return this.GetProperty(TransferTypeProperty); }
            set { this.SetProperty(TransferTypeProperty, value); }
        }
        #endregion

        #region 父节点标识ID ParentNodeId
        /// <summary>
        /// 父节点标识ID
        /// </summary>
        [Label("父节点标识ID")]
        public static readonly Property<string> ParentNodeIdProperty = P<RoutingProcess>.Register(e => e.ParentNodeId);

        /// <summary>
        /// 父节点标识ID
        /// </summary>
        public string ParentNodeId
        {
            get { return this.GetProperty(ParentNodeIdProperty); }
            set { this.SetProperty(ParentNodeIdProperty, value); }
        }
        #endregion


        #region 是否工序组 IsGroup
        /// <summary>
        /// 是否工序组
        /// </summary>
        [Label("是否工序组")]
        public static readonly Property<bool?> IsGroupProperty = P<RoutingProcess>.Register(e => e.IsGroup);

        /// <summary>
        /// 是否工序组
        /// </summary>
        public bool? IsGroup
        {
            get { return this.GetProperty(IsGroupProperty); }
            set { this.SetProperty(IsGroupProperty, value); }
        }
        #endregion

        #region 是否下工序入站 IsNextMoveIn
        /// <summary>
        /// 是否下工序入站
        /// </summary>
        [Label("是否下工序入站")]
        public static readonly Property<bool?> IsNextMoveInProperty = P<RoutingProcess>.Register(e => e.IsNextMoveIn);

        /// <summary>
        /// 是否下工序入站
        /// </summary>
        public bool? IsNextMoveIn
        {
            get { return this.GetProperty(IsNextMoveInProperty); }
            set { this.SetProperty(IsNextMoveInProperty, value); }
        }
        #endregion
        

        #region 工序组Id GroupId
        /// <summary>
        /// 工序组Id
        /// </summary>
        [Label("工序组Id")]
        public static readonly Property<string> GroupIdProperty = P<RoutingProcess>.Register(e => e.GroupId);

        /// <summary>
        /// 工序组Id
        /// </summary>
        public string GroupId
        {
            get { return this.GetProperty(GroupIdProperty); }
            set { this.SetProperty(GroupIdProperty, value); }
        }
        #endregion


        #region 是否委外 Outsourcing
        /// <summary>
        /// 是否委外
        /// </summary>
        [Label("是否委外")]
        public static readonly Property<bool> OutsourcingProperty = P<RoutingProcess>.Register(e => e.Outsourcing);

        /// <summary>
        /// 是否委外
        /// </summary>
        public bool Outsourcing
        {
            get { return this.GetProperty(OutsourcingProperty); }
            set { this.SetProperty(OutsourcingProperty, value); }
        }
        #endregion

        #region 工艺路线信息Id LayoutInfoId
        /// <summary>
        /// 工艺路线信息Id
        /// </summary>
        [Label("工艺路线信息Id")]
        public static readonly Property<double?> LayoutInfoIdProperty = P<RoutingProcess>.Register(e => e.LayoutInfoId);

        /// <summary>
        /// 工艺路线信息Id
        /// </summary>
        public double? LayoutInfoId
        {
            get { return this.GetProperty(LayoutInfoIdProperty); }
            set { this.SetProperty(LayoutInfoIdProperty, value); }
        }
        #endregion

        #region 工序流水码 Vornr
        /// <summary>
        /// 工序流水码
        /// </summary>
        [Label("工序流水码")]
        public static readonly Property<string> VornrProperty = P<RoutingProcess>.Register(e => e.Vornr);

        /// <summary>
        /// 工序流水码
        /// </summary>
        public string Vornr
        {
            get { return this.GetProperty(VornrProperty); }
            set { this.SetProperty(VornrProperty, value); }
        }
        #endregion

        #region 控制码 Steus
        /// <summary>
        /// 控制码
        /// </summary>
        [Label("控制码")]
        public static readonly Property<string> SteusProperty = P<RoutingProcess>.Register(e => e.Steus);

        /// <summary>
        /// 控制码
        /// </summary>
        public string Steus
        {
            get { return this.GetProperty(SteusProperty); }
            set { this.SetProperty(SteusProperty, value); }
        }
        #endregion

        #region 注册视图

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<RoutingProcess>.RegisterView(e => e.ProcessName, p => p.Process.Name);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }
        #endregion

        #region 工序编码 ProcessCode
        /// <summary>
        /// 工序编码
        /// </summary>
        [Label("工序编码")]
		public static readonly Property<string> ProcessCodeProperty = P<RoutingProcess>.RegisterView(e => e.ProcessCode, p => p.Process.Code);

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode
        {
			get { return this.GetProperty(ProcessCodeProperty); }
		}
		#endregion

		#region 工段 ProcessSegmentName
		/// <summary>
		/// 工段
		/// </summary>
		[Label("工段")]
        public static readonly Property<string> ProcessSegmentNameProperty = P<RoutingProcess>.RegisterView(e => e.ProcessSegmentName, p => p.Process.Segment.Name);

        /// <summary>
        /// 工段
        /// </summary>
        public string ProcessSegmentName
        {
            get { return this.GetProperty(ProcessSegmentNameProperty); }
        }
        #endregion

        #region 工序类型 ProcessType
        /// <summary>
        /// 工序类型
        /// </summary>
        [Label("工序类型")]
        public static readonly Property<ProcessType?> ProcessTypeProperty = P<RoutingProcess>.RegisterView(e => e.ProcessType, p => p.Process.Type);

        /// <summary>
        /// 工序类型
        /// </summary>
        public ProcessType? ProcessType
        {
            get { return this.GetProperty(ProcessTypeProperty); }
        }
        #endregion


        #endregion
    }

    /// <summary>
    /// 工序清单 实体配置
    /// </summary>
    internal class RoutingProcessConfig : EntityConfig<RoutingProcess>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TECH_RT_PROC").MapAllProperties();
            Meta.Property(RoutingProcess.VersionIdProperty).ColumnMeta.HasIndex();
            Meta.Property(RoutingProcess.ActivityIdProperty).ColumnMeta.HasLength(320);
            Meta.EnablePhantoms();
        }
    }
}