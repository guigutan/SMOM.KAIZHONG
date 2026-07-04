using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;

namespace SIE.MES.WorkOrders
{
    /// <summary>
    /// 工序Bom
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工单工序BOM")]
    public partial class WorkOrderProcessBom : DataEntity
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkOrderProcessBom()
        {
            this.SingleQty = 0m;
            this.Weight = 0m;
        }
        #endregion

        #region 单位 Unit
        /// <summary>
        /// 单位Id
        /// </summary>
        [Label("单位")]
        public static readonly IRefIdProperty UnitIdProperty =
            P<WorkOrderProcessBom>.RegisterRefId(e => e.UnitId, ReferenceType.Normal);

        /// <summary>
        /// 单位Id
        /// </summary>
        public double? UnitId
        {
            get { return (double?)this.GetRefNullableId(UnitIdProperty); }
            set { this.SetRefNullableId(UnitIdProperty, value); }
        }

        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly RefEntityProperty<Unit> UnitProperty =
            P<WorkOrderProcessBom>.RegisterRef(e => e.Unit, UnitIdProperty);

        /// <summary>
        /// 单位
        /// </summary>
        public Unit Unit
        {
            get { return this.GetRefEntity(UnitProperty); }
            set { this.SetRefEntity(UnitProperty, value); }
        }
        #endregion

        #region 单位耗用量 SingleQty
        /// <summary>
        /// 单位耗用量
        /// </summary>
        [MinValue(0)]
        [Label("单位耗用量")]
        public static readonly Property<decimal> SingleQtyProperty = P<WorkOrderProcessBom>.Register(e => e.SingleQty);

        /// <summary>
        /// 单位耗用量
        /// </summary>
        public decimal SingleQty
        {
            get { return GetProperty(SingleQtyProperty); }
            set { SetProperty(SingleQtyProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<WorkOrderProcessBom>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)this.GetRefNullableId(ProcessIdProperty); }
            set { this.SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        public static readonly RefEntityProperty<Process> ProcessProperty =
            P<WorkOrderProcessBom>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 工步 WorkStep
        /// <summary>
        /// 工步Id
        /// </summary>
        [Label("工步")]
        public static readonly IRefIdProperty WorkStepIdProperty =
            P<WorkOrderProcessBom>.RegisterRefId(e => e.WorkStepId, ReferenceType.Normal);

        /// <summary>
        /// 工步Id
        /// </summary>
        public double? WorkStepId
        {
            get { return (double?)this.GetRefNullableId(WorkStepIdProperty); }
            set { this.SetRefNullableId(WorkStepIdProperty, value); }
        }

        /// <summary>
        /// 工步
        /// </summary>
        public static readonly RefEntityProperty<WorkStep> WorkStepProperty =
            P<WorkOrderProcessBom>.RegisterRef(e => e.WorkStep, WorkStepIdProperty);

        /// <summary>
        /// 工步
        /// </summary>
        public WorkStep WorkStep
        {
            get { return this.GetRefEntity(WorkStepProperty); }
            set { this.SetRefEntity(WorkStepProperty, value); }
        }
        #endregion

        #region 工序序列 Seq
        /// <summary>
        /// 工序序列
        /// </summary>
        [Label("工序序列")]
        public static readonly Property<int> SeqProperty = P<WorkOrderProcessBom>.Register(e => e.Seq);

        /// <summary>
        /// 工序序列
        /// </summary>
        public int Seq
        {
            get { return GetProperty(SeqProperty); }
            set { SetProperty(SeqProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<WorkOrderProcessBom>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return (double)GetRefId(ItemIdProperty); }
            set { SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        [Label("物料")]
        public static readonly RefEntityProperty<Item> ItemProperty = P<WorkOrderProcessBom>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<WorkOrderProcessBom>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<WorkOrderProcessBom>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 单位 ItemUnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> ItemUnitNameProperty = P<WorkOrderProcessBom>.RegisterView(e => e.ItemUnitName, p => p.Item.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string ItemUnitName
        {
            get { return this.GetProperty(ItemUnitNameProperty); }
            set { SetProperty(ItemUnitNameProperty, value); }
        }
        #endregion

        #region 工单工序清单工序名称 RoutingProcessName
        /// <summary>
        /// 工单工序清单工序名称
        /// </summary>
        [Label("工单工序清单工序名称")]
        public static readonly Property<string> RoutingProcessNameProperty = P<WorkOrderProcessBom>.RegisterView(e => e.RoutingProcessName, p => p.RoutingProcess.Name);

        /// <summary>
        /// 工单工序清单工序名称
        /// </summary>
        public string RoutingProcessName
        {
            get { return this.GetProperty(RoutingProcessNameProperty); }
        }
        #endregion

        #region 替代组合分组 AlterGroup
        /// <summary>
        /// 替代组合分组
        /// </summary>
        [Label("替代组合分组")]
        public static readonly Property<string> AlterGroupProperty = P<WorkOrderProcessBom>.Register(e => e.AlterGroup);

        /// <summary>
        /// 替代组合分组
        /// </summary>
        public string AlterGroup
        {
            get { return this.GetProperty(AlterGroupProperty); }
            set { this.SetProperty(AlterGroupProperty, value); }
        }
        #endregion

        #region 替代组 Alter
        /// <summary>
        /// 替代组
        /// </summary>
        [Label("替代组")]
        public static readonly Property<string> AlterProperty = P<WorkOrderProcessBom>.Register(e => e.Alter);

        /// <summary>
        /// 替代组
        /// </summary>
        public string Alter
        {
            get { return this.GetProperty(AlterProperty); }
            set { this.SetProperty(AlterProperty, value); }
        }
        #endregion

        #region 优先级 Priority
        /// <summary>
        /// 优先级
        /// </summary>
        [Label("优先级")]
        public static readonly Property<int> PriorityProperty = P<WorkOrderProcessBom>.Register(e => e.Priority);

        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority
        {
            get { return this.GetProperty(PriorityProperty); }
            set { this.SetProperty(PriorityProperty, value); }
        }
        #endregion

        #region 规格型号 ItemSpecificationModel
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> ItemSpecificationModelProperty = P<WorkOrderProcessBom>.RegisterView(e => e.ItemSpecificationModel, p => p.Item.SpecificationModel);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string ItemSpecificationModel
        {
            get { return this.GetProperty(ItemSpecificationModelProperty); }
            set { SetProperty(ItemSpecificationModelProperty, value); }
        }
        #endregion 

        #region 工序名称 ProcessNameView
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameViewProperty = P<WorkOrderProcessBom>.RegisterView(e => e.ProcessNameView, p => p.Process.Name);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessNameView
        {
            get { return this.GetProperty(ProcessNameViewProperty); }
        }
        #endregion

        #region 工段名称 SegmentName
        /// <summary>
        /// 工段名称
        /// </summary>
        [Label("工段名称")]
        public static readonly Property<string> SegmentNameProperty = P<WorkOrderProcessBom>.RegisterView(e => e.SegmentName, p => p.Process.Segment.Name);

        /// <summary>
        /// 工段名称
        /// </summary>
        public string SegmentName
        {
            get { return this.GetProperty(SegmentNameProperty); }
        }
        #endregion

        #region 工步名称 WorkStepName
        /// <summary>
        /// 注释
        /// </summary>
        [Label("工步名称")]
        public static readonly Property<string> WorkStepNameProperty = P<WorkOrderProcessBom>.RegisterView(e => e.WorkStepName, p => p.WorkStep.Name);

        /// <summary>
        /// 注释
        /// </summary>
        public string WorkStepName
        {
            get { return this.GetProperty(WorkStepNameProperty); }
        }
        #endregion

        #region 工单工序清单 RoutingProcess
        /// <summary>
        /// 工单工序清单Id
        /// </summary>
        [Label("工单工序清单")]
        [Required]
        public static readonly IRefIdProperty RoutingProcessIdProperty = P<WorkOrderProcessBom>.RegisterRefId(e => e.RoutingProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工单工序清单Id
        /// </summary>
        public double? RoutingProcessId
        {
            get { return (double?)GetRefNullableId(RoutingProcessIdProperty); }
            set { SetRefNullableId(RoutingProcessIdProperty, value); }
        }

        /// <summary>
        /// 工单工序清单
        /// </summary>
        [Label("工单工序清单")]
        public static readonly RefEntityProperty<WorkOrderRoutingProcess> RoutingProcessProperty = P<WorkOrderProcessBom>.RegisterRef(e => e.RoutingProcess, RoutingProcessIdProperty);

        /// <summary>
        /// 工单工序清单
        /// </summary>
        public WorkOrderRoutingProcess RoutingProcess
        {
            get { return GetRefEntity(RoutingProcessProperty); }
            set { SetRefEntity(RoutingProcessProperty, value); }
        }
        #endregion

        #region 工单与工序BOM关系 WorkOrder
        /// <summary>
        /// 工单与工序BOM关系Id
        /// </summary>
        [Label("工序BOM")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<WorkOrderProcessBom>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Parent);

        /// <summary>
        /// 工单与工序BOM关系Id
        /// </summary>
        public double WorkOrderId
        {
            get { return (double)GetRefId(WorkOrderIdProperty); }
            set { SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单与工序BOM关系
        /// </summary>
        [Label("工序BOM")]
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<WorkOrderProcessBom>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单与工序BOM关系
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 计划数量 PlanQty    
        /// <summary>
        /// 计划数量
        /// </summary>
        [Label("计划数量")]
        public static readonly Property<decimal> PlanQtyProperty = P<WorkOrderProcessBom>.RegisterView(e => e.PlanQty, p => p.WorkOrder.PlanQty);

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanQty
        {
            get { return this.GetProperty(PlanQtyProperty); }
            set { SetProperty(PlanQtyProperty, value); }
        }
        #endregion

        #region 扩展属性 ItemExtProp
        /// <summary>
        /// 扩展属性
        /// </summary>
        [Label("扩展属性")]
        public static readonly Property<string> ItemExtPropProperty = P<WorkOrderProcessBom>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get { return GetProperty(ItemExtPropProperty); }
            set { SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 扩展属性值 ItemExtPropName
        /// <summary>
        /// 扩展属性值
        /// </summary>
        [Label("扩展属性值")]
        public static readonly Property<string> ItemExtPropNameProperty = P<WorkOrderProcessBom>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 扩展属性值
        /// </summary>
        public string ItemExtPropName
        {
            get { return GetProperty(ItemExtPropNameProperty); }
            set { SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 是替代料 IsAlternative
        /// <summary>
        /// 是替代料
        /// </summary>
        [Label("是替代料")]
        public static readonly Property<bool> IsAlternativeProperty
            = P<WorkOrderProcessBom>.Register(e => e.IsAlternative);

        /// <summary>
        /// 是替代料
        /// </summary>
        public bool IsAlternative
        {
            get { return this.GetProperty(IsAlternativeProperty); }
            set { this.SetProperty(IsAlternativeProperty, value); }
        }
        #endregion

        #region 是否上料关闭 IsFeedingClose
        /// <summary>
        /// 是否上料关闭(以后在PDA上料上不再显示这工序BOM)
        /// </summary>
        [Label("是否上料关闭")]
        public static readonly Property<bool?> IsFeedingCloseProperty = P<WorkOrderProcessBom>.Register(e => e.IsFeedingClose);

        /// <summary>
        /// 是否上料关闭
        /// </summary>
        public bool? IsFeedingClose
        {
            get { return this.GetProperty(IsFeedingCloseProperty); }
            set { this.SetProperty(IsFeedingCloseProperty, value); }
        }
        #endregion

        #region 发料工厂 Werks
        /// <summary>
        /// 发料工厂
        /// </summary>
        [Label("发料工厂")]
        public static readonly Property<string> WerksProperty = P<WorkOrderProcessBom>.Register(e => e.Werks);

        /// <summary>
        /// 发料工厂
        /// </summary>
        public string Werks
        {
            get { return this.GetProperty(WerksProperty); }
            set { this.SetProperty(WerksProperty, value); }
        }
        #endregion

        #region 单位 Meins
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> MeinsProperty = P<WorkOrderProcessBom>.Register(e => e.Meins);

        /// <summary>
        /// 单位
        /// </summary>
        public string Meins
        {
            get { return this.GetProperty(MeinsProperty); }
            set { this.SetProperty(MeinsProperty, value); }
        }
        #endregion

        #region 取样净重 Weight
        /// <summary>
        /// 取样净重
        /// </summary>
        [Label("取样净重")]
        public static readonly Property<decimal?> WeightProperty = P<WorkOrderProcessBom>.Register(e => e.Weight);

        /// <summary>
        /// 取样净重
        /// </summary>
        public decimal? Weight
        {
            get { return this.GetProperty(WeightProperty); }
            set { this.SetProperty(WeightProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 工单工序BOM 实体配置
    /// </summary>
    internal class WorkOrderProcessBomConfig : EntityConfig<WorkOrderProcessBom>
    {
        /// <summary>
        /// 属性元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WO_PROC_BOM").MapAllProperties();
            Meta.Property(WorkOrderProcessBom.WorkOrderIdProperty).ColumnMeta.IgnoreFK();
            Meta.Property(WorkOrderProcessBom.RoutingProcessIdProperty).ColumnMeta.IgnoreFK();
            Meta.EnablePhantoms();
            Meta.IndexGroupOnProperties(WorkOrderProcessBom.WorkOrderIdProperty, WorkOrderProcessBom.ItemIdProperty);
        }
    }
}