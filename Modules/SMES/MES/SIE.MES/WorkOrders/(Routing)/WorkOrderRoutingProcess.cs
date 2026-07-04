using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.ProcessSegments;
using SIE.Tech.Processs;
using SIE.Tech.Routings;
using SIE.Tech.VictoryStandards;
using System;

namespace SIE.MES.WorkOrders
{
    /// <summary>
    /// 工单工序清单
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工单工序清单")]
    [DisplayMember(nameof(Name))]
    public partial class WorkOrderRoutingProcess : DataEntity
    {
        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<WorkOrderRoutingProcess>.Register(e => e.Name);

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
        [Label("标识ID")]
        [MaxLength(80)]
        public static readonly Property<string> ActivityIdProperty = P<WorkOrderRoutingProcess>.Register(e => e.ActivityId);

        /// <summary>
        /// 标识ID
        /// </summary>
        public string ActivityId
        {
            get { return GetProperty(ActivityIdProperty); }
            set { SetProperty(ActivityIdProperty, value); }
        }
        #endregion

        #region 是否可选 IsOptional
        /// <summary>
        /// 是否可选
        /// </summary>
        [Label("是否可选")]
        public static readonly Property<bool> IsOptionalProperty = P<WorkOrderRoutingProcess>.Register(e => e.IsOptional);

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
        [Label("是否重复过站")]
        public static readonly Property<bool> IsRepeatProperty = P<WorkOrderRoutingProcess>.Register(e => e.IsRepeat);

        /// <summary>
        /// 是否重复过站
        /// </summary>
        public bool IsRepeat
        {
            get { return GetProperty(IsRepeatProperty); }
            set { SetProperty(IsRepeatProperty, value); }
        }
        #endregion

        #region 是否生成任务单 IsGenerateTask
        /// <summary>
        /// 是否生成任务单
        /// </summary>
        [Label("是否生成任务单")]
        public static readonly Property<bool> IsGenerateTaskProperty = P<WorkOrderRoutingProcess>.Register(e => e.IsGenerateTask);

        /// <summary>
        /// 是否生成任务单
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
        [Label("是否需求任务清单")]
        public static readonly Property<bool> IsRequirementTaskProperty = P<WorkOrderRoutingProcess>.Register(e => e.IsRequirementTask);

        /// <summary>
        /// 是否需求任务清单
        /// </summary>
        public bool IsRequirementTask
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
        public static readonly Property<bool> IsBuckleMaterialProperty = P<WorkOrderRoutingProcess>.Register(e => e.IsBuckleMaterial);

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
        public static readonly Property<double?> StartProcessProperty = P<WorkOrderRoutingProcess>.Register(e => e.StartProcess);

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
            P<WorkOrderRoutingProcess>.RegisterRefId(e => e.NormalVictoryId, ReferenceType.Normal);

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
            P<WorkOrderRoutingProcess>.RegisterRef(e => e.NormalVictory, NormalVictoryIdProperty);

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
            P<WorkOrderRoutingProcess>.RegisterRefId(e => e.RepairVictoryId, ReferenceType.Normal);

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
            P<WorkOrderRoutingProcess>.RegisterRef(e => e.RepairVictory, RepairVictoryIdProperty);

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
        public static readonly Property<bool> IsStricterProperty = P<WorkOrderRoutingProcess>.Register(e => e.IsStricter);

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
        public static readonly Property<int?> OvertimeProperty = P<WorkOrderRoutingProcess>.Register(e => e.Overtime);

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
        public static readonly Property<bool> IsPassRateProperty = P<WorkOrderRoutingProcess>.Register(e => e.IsPassRate);

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
        [Label("绑定")]
        public static readonly Property<bool> IsBindingProperty = P<WorkOrderRoutingProcess>.Register(e => e.IsBinding);

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
        [Label("解绑")]
        public static readonly Property<bool> IsUnBindingProperty = P<WorkOrderRoutingProcess>.Register(e => e.IsUnBinding);

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
        public static readonly Property<int> IndexProperty = P<WorkOrderRoutingProcess>.Register(e => e.Index);

        /// <summary>
        /// 顺序
        /// </summary>
        public int Index
        {
            get { return this.GetProperty(IndexProperty); }
            set { this.SetProperty(IndexProperty, value); }
        }
        #endregion

        #region 清单标记 Sign
        /// <summary>
        /// 清单标记
        /// </summary>
        [Label("清单标记")]
        public static readonly Property<RoutingProcessSign> SignProperty = P<WorkOrderRoutingProcess>.Register(e => e.Sign);

        /// <summary>
        /// 清单标记
        /// </summary>
        public RoutingProcessSign Sign
        {
            get { return GetProperty(SignProperty); }
            set { SetProperty(SignProperty, value); }
        }
        #endregion

        #region 工段 Segment
        /// <summary>
        /// 工段Id
        /// </summary>
        [Label("工段")]
        public static readonly IRefIdProperty SegmentIdProperty = P<WorkOrderRoutingProcess>.RegisterRefId(e => e.SegmentId, ReferenceType.Normal);

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
        [Label("工段")]
        public static readonly RefEntityProperty<ProcessSegment> SegmentProperty = P<WorkOrderRoutingProcess>.RegisterRef(e => e.Segment, SegmentIdProperty);

        /// <summary>
        /// 工段
        /// </summary>
        public ProcessSegment Segment
        {
            get { return GetRefEntity(SegmentProperty); }
            set { SetRefEntity(SegmentProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<WorkOrderRoutingProcess>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

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
        [Label("工序")]
        public static readonly RefEntityProperty<Process> ProcessProperty = P<WorkOrderRoutingProcess>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 工艺路线工序 RoutingProcess
        /// <summary>
        /// 工艺路线工序Id
        /// </summary>
        [Label("工艺路线工序")]
        public static readonly IRefIdProperty RoutingProcessIdProperty =
            P<WorkOrderRoutingProcess>.RegisterRefId(e => e.RoutingProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工艺路线工序Id
        /// </summary>
        public double? RoutingProcessId
        {
            get { return (double?)this.GetRefNullableId(RoutingProcessIdProperty); }
            set { this.SetRefNullableId(RoutingProcessIdProperty, value); }
        }

        /// <summary>
        /// 工艺路线工序
        /// </summary>
        public static readonly RefEntityProperty<RoutingProcess> RoutingProcessProperty =
            P<WorkOrderRoutingProcess>.RegisterRef(e => e.RoutingProcess, RoutingProcessIdProperty);

        /// <summary>
        /// 工艺路线工序
        /// </summary>
        public RoutingProcess RoutingProcess
        {
            get { return this.GetRefEntity(RoutingProcessProperty); }
            set { this.SetRefEntity(RoutingProcessProperty, value); }
        }
        #endregion

        #region 类型 ProcessType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<ProcessType> ProcessTypeProperty = P<WorkOrderRoutingProcess>.Register(e => e.ProcessType);

        /// <summary>
        /// 类型
        /// </summary>
        public ProcessType ProcessType
        {
            get { return GetProperty(ProcessTypeProperty); }
            set { SetProperty(ProcessTypeProperty, value); }
        }
        #endregion

        #region 工单工序清单与BOM关系 BomConfigList
        /// <summary>
        /// 工单工序清单与BOM关系
        /// </summary>
        [Label("BOM关系")]
        public static readonly ListProperty<EntityList<WorkOrderRoutingProcessBom>> BomConfigListProperty = P<WorkOrderRoutingProcess>.RegisterList(e => e.BomConfigList);

        /// <summary>
        /// 工单工序清单与BOM关系
        /// </summary>
        public EntityList<WorkOrderRoutingProcessBom> BomConfigList
        {
            get { return this.GetLazyList(BomConfigListProperty); }
        }
        #endregion

        #region 工单工序清单与参数关系 ParameterList
        /// <summary>
        /// 工单工序清单与参数关系
        /// </summary>
        [Label("参数关系")]
        public static readonly ListProperty<EntityList<WorkOrderRoutingProcessParameter>> ParameterListProperty = P<WorkOrderRoutingProcess>.RegisterList(e => e.ParameterList);

        /// <summary>
        /// 工单工序清单与参数关系
        /// </summary>
        public EntityList<WorkOrderRoutingProcessParameter> ParameterList
        {
            get { return this.GetLazyList(ParameterListProperty); }
        }
        #endregion

        #region 工单与工序清单关系 WorkOrder
        /// <summary>
        /// 工单与工序清单关系Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<WorkOrderRoutingProcess>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Parent);

        /// <summary>
        /// 工单与工序清单关系Id
        /// </summary>
        public double WorkOrderId
        {
            get { return (double)GetRefId(WorkOrderIdProperty); }
            set { SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单与工序清单关系
        /// </summary>
        [Label("工单")]
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<WorkOrderRoutingProcess>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单与工序清单关系
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 是否创建Sku CreateSku
        /// <summary>
        /// 是否创建Sku
        /// </summary>
        [Label("是否创建Sku")]
        public static readonly Property<bool> CreateSkuProperty = P<WorkOrderRoutingProcess>.Register(e => e.CreateSku);

        /// <summary>
        /// 是否创建Sku
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
        public static readonly Property<bool?> IsCalculateProperty = P<WorkOrderRoutingProcess>.Register(e => e.IsCalculate);

        /// <summary>
        /// 是否计产
        /// </summary>
        public bool? IsCalculate
        {
            get { return this.GetProperty(IsCalculateProperty); }
            set { this.SetProperty(IsCalculateProperty, value); }
        }
        #endregion

        #region 最大过站次数 MaxPassNum
        /// <summary>
        /// 最大过站次数
        /// </summary>
        [Label("最大过站次数")]
        public static readonly Property<int?> MaxPassNumProperty = P<WorkOrderRoutingProcess>.Register(e => e.MaxPassNum);

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
        public static readonly Property<bool?> EnableMoveInControlProperty = P<WorkOrderRoutingProcess>.Register(e => e.EnableMoveInControl);

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
        public static readonly Property<TransferType?> TransferTypeProperty = P<WorkOrderRoutingProcess>.Register(e => e.TransferType);

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
        public static readonly Property<string> ParentNodeIdProperty = P<WorkOrderRoutingProcess>.Register(e => e.ParentNodeId);

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
        public static readonly Property<bool?> IsGroupProperty = P<WorkOrderRoutingProcess>.Register(e => e.IsGroup);

        /// <summary>
        /// 是否工序组
        /// </summary>
        public bool? IsGroup
        {
            get { return this.GetProperty(IsGroupProperty); }
            set { this.SetProperty(IsGroupProperty, value); }
        }
        #endregion

        #region 工序组Id GroupId
        /// <summary>
        /// 工序组Id
        /// </summary>
        [Label("工序组Id")]
        public static readonly Property<string> GroupIdProperty = P<WorkOrderRoutingProcess>.Register(e => e.GroupId);

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
        public static readonly Property<bool> OutsourcingProperty = P<WorkOrderRoutingProcess>.Register(e => e.Outsourcing);

        /// <summary>
        /// 是否委外
        /// </summary>
        public bool Outsourcing
        {
            get { return this.GetProperty(OutsourcingProperty); }
            set { this.SetProperty(OutsourcingProperty, value); }
        }
        #endregion

        #region 末工序 EndProcess
        /// <summary>
        /// 末工序
        /// </summary>
        [Label("末工序")]
        public static readonly Property<double?> EndProcessProperty = P<WorkOrderRoutingProcess>.Register(e => e.EndProcess);

        /// <summary>
        /// 末工序
        /// </summary>
        public double? EndProcess
        {
            get { return this.GetProperty(EndProcessProperty); }
            set { this.SetProperty(EndProcessProperty, value); }
        }
        #endregion


        #region 是否下工序入站 IsNextMoveIn
        /// <summary>
        /// 是否下工序入站
        /// </summary>
        [Label("是否下工序入站")]
        public static readonly Property<bool?> IsNextMoveInProperty = P<WorkOrderRoutingProcess>.Register(e => e.IsNextMoveIn);

        /// <summary>
        /// 是否下工序入站
        /// </summary>
        public bool? IsNextMoveIn
        {
            get { return this.GetProperty(IsNextMoveInProperty); }
            set { this.SetProperty(IsNextMoveInProperty, value); }
        }
        #endregion

        #region 工艺路线信息Id LayoutInfo
        /// <summary>
        /// 工艺路线信息IdId
        /// </summary>
        [Label("工艺路线信息Id")]
        public static readonly IRefIdProperty LayoutInfoIdProperty =
            P<WorkOrderRoutingProcess>.RegisterRefId(e => e.LayoutInfoId, ReferenceType.Normal);

        /// <summary>
        /// 工艺路线信息IdId
        /// </summary>
        public double? LayoutInfoId
        {
            get { return (double?)this.GetRefNullableId(LayoutInfoIdProperty); }
            set { this.SetRefNullableId(LayoutInfoIdProperty, value); }
        }

        /// <summary>
        /// 工艺路线信息Id
        /// </summary>
        public static readonly RefEntityProperty<LayoutInfo> LayoutInfoProperty =
            P<WorkOrderRoutingProcess>.RegisterRef(e => e.LayoutInfo, LayoutInfoIdProperty);

        /// <summary>
        /// 工艺路线信息Id
        /// </summary>
        public LayoutInfo LayoutInfo
        {
            get { return this.GetRefEntity(LayoutInfoProperty); }
            set { this.SetRefEntity(LayoutInfoProperty, value); }
        }
        #endregion



        #region 注册视图 
        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<WorkOrderRoutingProcess>.RegisterView(e => e.ProcessName, p => p.Process.Name);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }
        #endregion

        #region 正常胜制编码 NormalVictoryCode
        /// <summary>
        /// 正常胜制编码
        /// </summary>
        [Label("正常胜制")]
        public static readonly Property<string> NormalVictoryCodeProperty = P<WorkOrderRoutingProcess>.RegisterView(e => e.NormalVictoryCode, p => p.NormalVictory.Code);

        /// <summary>
        /// 正常胜制编码
        /// </summary>
        public string NormalVictoryCode
        {
            get { return this.GetProperty(NormalVictoryCodeProperty); }
        }
        #endregion

        #region 维修胜制编码 RepairVictoryCode
        /// <summary>
        /// 维修胜制编码
        /// </summary>
        [Label("维修胜制")]
        public static readonly Property<string> RepairVictoryCodeProperty = P<WorkOrderRoutingProcess>.RegisterView(e => e.RepairVictoryCode, p => p.RepairVictory.Code);

        /// <summary>
        /// 维修胜制编码
        /// </summary>
        public string RepairVictoryCode
        {
            get { return this.GetProperty(RepairVictoryCodeProperty); }
        }
        #endregion                
        #endregion
    }

    /// <summary>
    /// 工单工序清单 实体配置
    /// </summary>
    internal class WorkOrderRoutingProcessConfig : EntityConfig<WorkOrderRoutingProcess>
    {
        /// <summary>
        /// 属性元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WO_RT_PROC").MapAllProperties();
            Meta.Property(WorkOrderRoutingProcess.WorkOrderIdProperty).ColumnMeta.IgnoreFK();
            Meta.Property(WorkOrderRoutingProcess.ActivityIdProperty).ColumnMeta.HasLength(320);
            Meta.EnablePhantoms();
        }
    }
}