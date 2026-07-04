using SIE.Domain;
using SIE.MES.BatchWIP.Products.SplitAndMerge;
using SIE.MES.BatchWIP.Products.SplitAndMerge.Enums;
using SIE.MES.WIP;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using System;

namespace SIE.MES.BatchWIP.Products
{
    /// <summary>
    /// 生产批次版本
    /// </summary>
    [ChildEntity, Serializable]
    [Label("生产批次版本")]
    [CriteriaQuery(typeof(BatchCriteriaProvider))]
    public partial class BatchWipProductVersion : DataEntity
    {
        #region 生产批次 BatchNo
        /// <summary>
        /// 生产批次
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> BatchNoProperty = P<BatchWipProductVersion>.Register(e => e.BatchNo);

        /// <summary>
        /// 生产批次
        /// </summary>
        public string BatchNo
        {
            get { return GetProperty(BatchNoProperty); }
            set { SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 批次数量 Qty
        /// <summary>
        /// 批次数量
        /// </summary>
        [Label("批次数量")]
        public static readonly Property<decimal> QtyProperty = P<BatchWipProductVersion>.Register(e => e.Qty);

        /// <summary>
        /// 批次数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 完工数量 FinishQty
        /// <summary>
        /// 完工数量
        /// </summary>
        [Label("完工数量")]
        public static readonly Property<decimal> FinishQtyProperty = P<BatchWipProductVersion>.Register(e => e.FinishQty);

        /// <summary>
        /// 完工数量
        /// </summary>
        public decimal FinishQty
        {
            get { return this.GetProperty(FinishQtyProperty); }
            set { this.SetProperty(FinishQtyProperty, value); }
        }
        #endregion 

        #region 不合格数量 NgQty
        /// <summary>
        /// 不合格数量
        /// </summary>
        [Label("不合格数量")]
        public static readonly Property<decimal> NgQtyProperty = P<BatchWipProductVersion>.Register(e => e.NgQty);

        /// <summary>
        /// 不合格数量
        /// </summary>
        public decimal NgQty
        {
            get { return this.GetProperty(NgQtyProperty); }
            set { this.SetProperty(NgQtyProperty, value); }
        }
        #endregion

        #region 报废数量 ScrapQty
        /// <summary>
        /// 报废数量
        /// </summary>
        [Label("报废数量")]
        public static readonly Property<decimal> ScrapQtyProperty = P<BatchWipProductVersion>.Register(e => e.ScrapQty);

        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal ScrapQty
        {
            get { return this.GetProperty(ScrapQtyProperty); }
            set { this.SetProperty(ScrapQtyProperty, value); }
        }
        #endregion

        #region 是否保留,保留状态不能过终检 IsHold
        /// <summary>
        /// 是否保留,保留状态不能过终检
        /// </summary>
        [Label("是否保留,保留状态不能过终检")]
        public static readonly Property<bool> IsHoldProperty = P<BatchWipProductVersion>.Register(e => e.IsHold);

        /// <summary>
        /// 是否保留,保留状态不能过终检
        /// </summary>
        public bool IsHold
        {
            get { return GetProperty(IsHoldProperty); }
            set { SetProperty(IsHoldProperty, value); }
        }
        #endregion

        #region 是否已完工下线 IsFinish
        /// <summary>
        /// 是否已完工下线
        /// </summary>
        [Label("是否已完工下线")]
        public static readonly Property<bool> IsFinishProperty = P<BatchWipProductVersion>.Register(e => e.IsFinish);

        /// <summary>
        /// 是否已完工下线
        /// </summary>
        public bool IsFinish
        {
            get { return GetProperty(IsFinishProperty); }
            set { SetProperty(IsFinishProperty, value); }
        }
        #endregion

        #region 是否暂停 IsPause
        /// <summary>
        /// 是否暂停
        /// </summary>
        [Label("是否暂停")]
        public static readonly Property<YesNo> IsPauseProperty = P<BatchWipProductVersion>.Register(e => e.IsPause);

        /// <summary>
        /// 是否暂停
        /// </summary>
        public YesNo IsPause
        {
            get { return this.GetProperty(IsPauseProperty); }
            set { this.SetProperty(IsPauseProperty, value); }
        }
        #endregion

        #region 当前采集记录 CurrentProcess
        /// <summary>
        /// 当前采集记录Id
        /// </summary>
        public static readonly IRefIdProperty CurrentProcessIdProperty = P<BatchWipProductVersion>.RegisterRefId(e => e.CurrentProcessId, ReferenceType.Normal);

        /// <summary>
        /// 当前采集记录Id
        /// </summary>
        public double? CurrentProcessId
        {
            get { return (double?)GetRefNullableId(CurrentProcessIdProperty); }
            set { SetRefNullableId(CurrentProcessIdProperty, value); }
        }

        /// <summary>
        /// 当前采集记录
        /// </summary>
        public static readonly RefEntityProperty<BatchWipRecord> CurrentProcessProperty = P<BatchWipProductVersion>.RegisterRef(e => e.CurrentProcess, CurrentProcessIdProperty);

        /// <summary>
        /// 当前采集记录
        /// </summary>
        public BatchWipRecord CurrentProcess
        {
            get { return GetRefEntity(CurrentProcessProperty); }
            set { SetRefEntity(CurrentProcessProperty, value); }
        }
        #endregion

        #region 工位 Station
        /// <summary>
        /// 工位Id
        /// </summary>
        [Label("工位")]
        public static readonly IRefIdProperty StationIdProperty =
            P<BatchWipProductVersion>.RegisterRefId(e => e.StationId, ReferenceType.Normal);

        /// <summary>
        /// 工位Id
        /// </summary>
        public double? StationId
        {
            get { return (double?)this.GetRefNullableId(StationIdProperty); }
            set { this.SetRefNullableId(StationIdProperty, value); }
        }

        /// <summary>
        /// 工位
        /// </summary>
        public static readonly RefEntityProperty<Station> StationProperty =
            P<BatchWipProductVersion>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 工位
        /// </summary>
        public Station Station
        {
            get { return this.GetRefEntity(StationProperty); }
            set { this.SetRefEntity(StationProperty, value); }
        }
        #endregion

        #region 当前工序 Process
        /// <summary>
        /// 当前工序Id
        /// </summary>
        [Label("当前工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<BatchWipProductVersion>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 当前工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)this.GetRefNullableId(ProcessIdProperty); }
            set { this.SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 当前工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty =
            P<BatchWipProductVersion>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 当前工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty =
            P<BatchWipProductVersion>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)this.GetRefNullableId(ResourceIdProperty); }
            set { this.SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty =
            P<BatchWipProductVersion>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 下一工序 NextProcess
        /// <summary>
        /// 下一工序Id
        /// </summary>
        [Label("下一工序")]
        public static readonly IRefIdProperty NextProcessIdProperty
            = P<BatchWipProductVersion>.RegisterRefId(e => e.NextProcessId, ReferenceType.Normal);

        /// <summary>
        /// 下一工序Id
        /// </summary>
        public double? NextProcessId
        {
            get { return (double?)GetRefNullableId(NextProcessIdProperty); }
            set { SetRefNullableId(NextProcessIdProperty, value); }
        }

        /// <summary>
        /// 下一工序
        /// </summary>
        public static readonly RefEntityProperty<Process> NextProcessProperty
            = P<BatchWipProductVersion>.RegisterRef(e => e.NextProcess, NextProcessIdProperty);

        /// <summary>
        /// 下一工序
        /// </summary>
        public Process NextProcess
        {
            get { return GetRefEntity(NextProcessProperty); }
            set { SetRefEntity(NextProcessProperty, value); }
        }
        #endregion

        #region 下工序顺序号 NextProcessIndex
        /// <summary>
        /// 下工序顺序号
        /// </summary>
        [Label("下工序顺序号")]
        public static readonly Property<int?> NextProcessIndexProperty
            = P<BatchWipProductVersion>.Register(e => e.NextProcessIndex);

        /// <summary>
        /// 下工序顺序号
        /// </summary>
        public int? NextProcessIndex
        {
            get { return this.GetProperty(NextProcessIndexProperty); }
            set { this.SetProperty(NextProcessIndexProperty, value); }
        }
        #endregion


        #region 当前工序顺序号 CurrentProcessIndex
        /// <summary>
        /// 当前工序顺序号
        /// </summary>
        [Label("当前工序顺序号")]
        public static readonly Property<double?> CurrentProcessIndexProperty
            = P<BatchWipProductVersion>.Register(e => e.CurrentProcessIndex);

        /// <summary>
        /// 当前工序顺序号
        /// </summary>
        public double? CurrentProcessIndex
        {
            get { return this.GetProperty(CurrentProcessIndexProperty); }
            set { this.SetProperty(CurrentProcessIndexProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<BatchWipProductVersion>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId
        {
            get { return (double)GetRefId(WorkOrderIdProperty); }
            set { SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<BatchWipProductVersion>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 批次生产产品 Product
        /// <summary>
        /// 批次生产产品Id
        /// </summary>
		[Label("产品")]
        public static readonly IRefIdProperty ProductIdProperty = P<BatchWipProductVersion>.RegisterRefId(e => e.ProductId, ReferenceType.Parent);

        /// <summary>
        /// 批次生产产品Id
        /// </summary>
        public double ProductId
        {
            get { return (double)GetRefId(ProductIdProperty); }
            set { SetRefId(ProductIdProperty, value); }
        }

        /// <summary>
        /// 批次生产产品
        /// </summary>
        public static readonly RefEntityProperty<BatchWipProduct> ProductProperty = P<BatchWipProductVersion>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 批次生产产品
        /// </summary>
        public BatchWipProduct Product
        {
            get { return GetRefEntity(ProductProperty); }
            set { SetRefEntity(ProductProperty, value); }
        }
        #endregion

        #region 不良记录 DefectList
        /// <summary>
        /// 不良记录
        /// </summary>
        public static readonly ListProperty<EntityList<BatchWipProductDefect>> DefectListProperty = P<BatchWipProductVersion>.RegisterList(e => e.DefectList);

        /// <summary>
        /// 不良记录
        /// </summary>
        public EntityList<BatchWipProductDefect> DefectList
        {
            get { return this.GetLazyList(DefectListProperty); }
        }
        #endregion

        #region 批次合并拆分记录 BatchSplitMergeList
        /// <summary>
        /// 批次合并拆分记录
        /// </summary>
        public static readonly ListProperty<EntityList<BatchSplitMergeRecord>> BatchSplitMergeListProperty = P<BatchWipProductVersion>.RegisterList(e => e.BatchSplitMergeList);

        /// <summary>
        /// 批次合并拆分记录
        /// </summary>
        public EntityList<BatchSplitMergeRecord> BatchSplitMergeList
        {
            get { return this.GetLazyList(BatchSplitMergeListProperty); }
        }
        #endregion
        
        #region 采集记录列表(旧) ProcessList
        /// <summary>
        /// 采集记录列表
        /// </summary>
        public static readonly ListProperty<EntityList<BatchWipProductProcess>> ProcessListProperty = P<BatchWipProductVersion>.RegisterList(e => e.ProcessList);

        /// <summary>
        /// 采集记录列表
        /// </summary>
        public EntityList<BatchWipProductProcess> ProcessList
        {
            get { return this.GetLazyList(ProcessListProperty); }
        }
        #endregion

        #region 采集记录(新) BatchWipRecordList
        /// <summary>
        /// 采集记录
        /// </summary>
        [Label("采集记录")]
        public static readonly ListProperty<EntityList<BatchWipRecord>> BatchWipRecordListProperty = P<BatchWipProductVersion>.RegisterList(e => e.BatchWipRecordList);

        /// <summary>
        /// 采集记录
        /// </summary>
        public EntityList<BatchWipRecord> BatchWipRecordList
        {
            get { return this.GetLazyList(BatchWipRecordListProperty); }
        }
        #endregion

        #region 维修记录列表 RepaireList
        /// <summary>
        /// 维修记录列表
        /// </summary>
        public static readonly ListProperty<EntityList<BatchWipProductRepaire>> RepaireListProperty = P<BatchWipProductVersion>.RegisterList(e => e.RepaireList);

        /// <summary>
        /// 维修记录列表
        /// </summary>
        public EntityList<BatchWipProductRepaire> RepaireList
        {
            get { return this.GetLazyList(RepaireListProperty); }
        }
        #endregion

        #region 是否已生成过半成品入线边仓 IsJoinLineWarehouse
        /// <summary>
        /// 是否已生成过半成品入线边仓(-1：已处理不入线边仓 0:待入线边仓1:已入线边仓)
        /// </summary>
        [Label("是否已生成过半成品入线边仓")]
        public static readonly Property<int> IsJoinLineWarehouseProperty = P<BatchWipProductVersion>.Register(e => e.IsJoinLineWarehouse);

        /// <summary>
        /// 是否已生成过半成品入线边仓
        /// </summary>
        public int IsJoinLineWarehouse
        {
            get { return this.GetProperty(IsJoinLineWarehouseProperty); }
            set { this.SetProperty(IsJoinLineWarehouseProperty, value); }
        }
        #endregion

        #region 委外中 IsOutsourcing
        /// <summary>
        /// 委外中
        /// </summary>
        [Label("委外中")]
        public static readonly Property<bool> IsOutsourcingProperty
            = P<BatchWipProductVersion>.Register(e => e.IsOutsourcing);

        /// <summary>
        /// 委外中
        /// </summary>
        public bool IsOutsourcing
        {
            get { return this.GetProperty(IsOutsourcingProperty); }
            set { this.SetProperty(IsOutsourcingProperty, value); }
        }
        #endregion

        #region 视图属性


        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<BatchWipProductVersion>.RegisterView(e => e.ProductCode, p => p.Product.Item.Code);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
        }
        #endregion


        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<BatchWipProductVersion>.RegisterView(e => e.ProductName, p => p.Product.Item.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }
        #endregion


        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<BatchWipProductVersion>.RegisterView(e => e.WorkOrderNo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
        }
        #endregion

        #region 工单类型 WoType
        /// <summary>
        /// 工单类型
        /// </summary>
        [Label("工单类型")]
        public static readonly Property<Core.WorkOrders.WorkOrderType> WoTypeProperty = P<BatchWipProductVersion>.RegisterView(e => e.WoType, p => p.WorkOrder.Type);

        /// <summary>
        /// 工单类型
        /// </summary>
        public Core.WorkOrders.WorkOrderType WoType
        {
            get { return this.GetProperty(WoTypeProperty); }
        }
        #endregion

        #region 工单数量 WoPlanQty
        /// <summary>
        /// 工单数量
        /// </summary>
        [Label("工单数量")]
        public static readonly Property<decimal> WoPlanQtyProperty = P<BatchWipProductVersion>.RegisterView(e => e.WoPlanQty, p => p.WorkOrder.PlanQty);

        /// <summary>
        /// 工单数量
        /// </summary>
        public decimal WoPlanQty
        {
            get { return this.GetProperty(WoPlanQtyProperty); }
        }
        #endregion

        #region 工单完工数 WoFinishQty
        /// <summary>
        /// 工单完工数
        /// </summary>
        [Label("工单完工数")]
        public static readonly Property<decimal> WoFinishQtyProperty = P<BatchWipProductVersion>.RegisterView(e => e.WoFinishQty, p => p.WorkOrder.FinishQty);

        /// <summary>
        /// 工单完工数
        /// </summary>
        public decimal WoFinishQty
        {
            get { return this.GetProperty(WoFinishQtyProperty); }
        }
        #endregion

        #region 工单报废数 WoScrapQty
        /// <summary>
        /// 工单报废数
        /// </summary>
        [Label("工单报废数")]
        public static readonly Property<decimal> WoScrapQtyProperty = P<BatchWipProductVersion>.RegisterView(e => e.WoScrapQty, p => p.WorkOrder.ScrapQty);

        /// <summary>
        /// 工单报废数
        /// </summary>
        public decimal WoScrapQty
        {
            get { return this.GetProperty(WoScrapQtyProperty); }
        }
        #endregion

        #region 工艺流程名称 VersionName
        /// <summary>
        /// 工艺流程名称
        /// </summary>
        [Label("工艺流程名称")]
        public static readonly Property<string> VersionNameProperty = P<BatchWipProductVersion>.RegisterView(e => e.VersionName, p => p.WorkOrder.Version.Name);

        /// <summary>
        /// 工艺流程名称
        /// </summary>
        public string VersionName
        {
            get { return this.GetProperty(VersionNameProperty); }
        }
        #endregion

        #region 车间名称 WorkShopName
        /// <summary>
        /// 车车间名称间
        /// </summary>
        [Label("车间")]
        public static readonly Property<string> WorkShopNameProperty = P<BatchWipProductVersion>.RegisterView(e => e.WorkShopName, p => p.WorkOrder.WorkShop.Name);

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkShopName
        {
            get { return this.GetProperty(WorkShopNameProperty); }
        }
        #endregion

        #region 当前工位资源 ResourceNeme
        /// <summary>
        /// 当前工位资源
        /// </summary>
        [Label("当前工位资源")]
        public static readonly Property<string> ResourceNameProperty = P<BatchWipProductVersion>.RegisterView(e => e.ResourceName, p => p.Resource.Name);

        /// <summary>
        /// 当前工位资源
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
        }
        #endregion

        #region 当前工位 ResourceNeme
        /// <summary>
        /// 当前工位
        /// </summary>
        [Label("当前工位")]
        public static readonly Property<string> StationNameProperty = P<BatchWipProductVersion>.RegisterView(e => e.StationName, p => p.Station.Name);

        /// <summary>
        /// 当前工位
        /// </summary>
        public string StationName
        {
            get { return this.GetProperty(StationNameProperty); }
        }
        #endregion

        #region 当前工序名称 ProcessName
        /// <summary>
        /// 当前工序名称
        /// </summary>
        [Label("当前工序")]
        public static readonly Property<string> ProcessNameProperty = P<BatchWipProductVersion>.RegisterView(e => e.ProcessName, p => p.Process.Name);

        /// <summary>
        /// 当前工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }
        #endregion

        #region 产品机型名称 ModelName
        /// <summary>
        /// 产品机型名称
        /// </summary>
        [Label("产品机型")]
        public static readonly Property<string> ModelNameProperty = P<BatchWipProductVersion>.RegisterView(e => e.ModelName, p => p.WorkOrder.Product.Model.Name);

        /// <summary>
        /// 产品机型名称
        /// </summary>
        public string ModelName
        {
            get { return this.GetProperty(ModelNameProperty); }
        }
        #endregion

        #region 产品型号 ProductModelName
        /// <summary>
        /// 产品型号名称
        /// </summary>
        [Label("产品型号")]
        public static readonly Property<string> ProductModelNameProperty = P<BatchWipProductVersion>.RegisterView(e => e.ProductModelName, p => p.Product.Model);

        /// <summary>
        /// 产品机型名称
        /// </summary>
        public string ProductModelName
        {
            get { return this.GetProperty(ProductModelNameProperty); }
        }
        #endregion

        #region 载具号 ContainerNo
        /// <summary>
        /// 载具号
        /// </summary>
        [Label("载具号")]
        public static readonly Property<string> ContainerNoProperty = P<BatchWipProductVersion>.RegisterView(e => e.ContainerNo, p => p.CurrentProcess.ContainerNo);

        /// <summary>
        /// 载具号
        /// </summary>
        public string ContainerNo
        {
            get { return this.GetProperty(ContainerNoProperty); }
        }
        #endregion

        #endregion

        #region 当前数量 RemainQty
        /// <summary>
        /// 当前数量
        /// </summary>
        [Label("当前数量")]
        public static readonly Property<decimal> RemainQtyProperty = P<BatchWipProductVersion>.Register(e => e.RemainQty);

        /// <summary>
        /// 当前数量
        /// </summary>
        public decimal RemainQty
        {
            get { return this.GetProperty(RemainQtyProperty); }
            set { this.SetProperty(RemainQtyProperty, value); }
        }
        #endregion

        #region 质量状态 DefectState
        /// <summary>
        /// 质量状态
        /// </summary>
        [Label("质量状态")]
        public static readonly Property<QState> DefectStateProperty = P<BatchWipProductVersion>.Register(e => e.DefectState);

        /// <summary>
        /// 质量状态
        /// </summary>
        public QState DefectState
        {
            get { return this.GetProperty(DefectStateProperty); }
            set { this.SetProperty(DefectStateProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 生产批次版本 实体配置
    /// </summary>
    internal class BatchWipProductVersionConfig : EntityConfig<BatchWipProductVersion>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("BWIP_PROD_VER").MapAllProperties();
            Meta.Property(BatchWipProductVersion.CurrentProcessIdProperty).ColumnMeta.IgnoreFK().IsNullable();
            Meta.Property(BatchWipProductVersion.IsJoinLineWarehouseProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}