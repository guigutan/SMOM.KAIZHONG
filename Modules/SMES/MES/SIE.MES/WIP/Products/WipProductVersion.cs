using SIE.Common;
using SIE.Domain;
using SIE.MES.BatchWIP.Products;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;

namespace SIE.MES.WIP.Products
{
    /// <summary>
    /// 生产产品版本
    /// </summary>
    [ChildEntity, Serializable]
    [Label("生产产品版本")]
    [ConditionQueryType(typeof(WipProductVersionCriteria))]
    public partial class WipProductVersion : DataEntity
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public WipProductVersion()
        {
            this.IsPause = YesNo.No;
        }
        #endregion

        #region 生产周转箱条码 BoxNo
        /// <summary>
        /// 容器条码
        /// </summary>
        [MaxLength(40)]
        [Label("容器条码")]
        public static readonly Property<string> BoxNoProperty = P<WipProductVersion>.Register(e => e.BoxNo);

        /// <summary>
        /// 容器条码
        /// </summary>
        public string BoxNo
        {
            get { return GetProperty(BoxNoProperty); }
            set { SetProperty(BoxNoProperty, value); }
        }
        #endregion

        #region 条码 Sn
        /// <summary>
        /// 条码
        /// </summary>
        [MaxLength(40)]
        [Label("条码")]
        public static readonly Property<string> SnProperty = P<WipProductVersion>.Register(e => e.Sn);

        /// <summary>
        /// 条码
        /// </summary>
        public string Sn
        {
            get { return GetProperty(SnProperty); }
            set { SetProperty(SnProperty, value); }
        }
        #endregion

        #region 客户条码 Csns
        /// <summary>
        /// 客户条码
        /// </summary>
        [MaxLength(40)]
        [Label("客户条码")]
        public static readonly Property<string> CsnsProperty = P<WipProductVersion>.Register(e => e.Csns);

        /// <summary>
        /// 客户条码
        /// </summary>
        public string Csns
        {
            get { return GetProperty(CsnsProperty); }
            set { SetProperty(CsnsProperty, value); }
        }
        #endregion

        #region 关键件 KeyLabel
        /// <summary>
        /// 关键件
        /// </summary>
        [MaxLength(40)]
        [Label("关键件")]
        public static readonly Property<string> KeyLabelProperty = P<WipProductVersion>.Register(e => e.KeyLabel);

        /// <summary>
        /// 关键件
        /// </summary>
        public string KeyLabel
        {
            get { return GetProperty(KeyLabelProperty); }
            set { SetProperty(KeyLabelProperty, value); }
        }
        #endregion

        #region 拼板码 CombinedCode
        /// <summary>
        /// 拼板码
        /// </summary>
        [MaxLength(40)]
        [Label("拼板码")]
        public static readonly Property<string> CombinedCodeProperty = P<WipProductVersion>.Register(e => e.CombinedCode);

        /// <summary>
        /// 拼板码
        /// </summary>
        public string CombinedCode
        {
            get { return GetProperty(CombinedCodeProperty); }
            set { SetProperty(CombinedCodeProperty, value); }
        }
        #endregion

        #region 关联条码 RelevanceSn
        /// <summary>
        /// 关联条码
        /// </summary>
        [Label("关联条码")]
        public static readonly Property<string> RelevanceSnProperty = P<WipProductVersion>.Register(e => e.RelevanceSn);

        /// <summary>
        /// 关联条码
        /// </summary>
        public string RelevanceSn
        {
            get { return this.GetProperty(RelevanceSnProperty); }
            set { this.SetProperty(RelevanceSnProperty, value); }
        }
        #endregion 

        #region 版本 Version
        /// <summary>
        /// 版本
        /// </summary>
        [MinValue(0)]
        [Label("版本")]
        public static readonly Property<int> VersionProperty = P<WipProductVersion>.Register(e => e.Version);

        /// <summary>
        /// 版本
        /// </summary>
        public int Version
        {
            get { return GetProperty(VersionProperty); }
            set { SetProperty(VersionProperty, value); }
        }
        #endregion

        #region 是否已完工下线 IsFinish
        /// <summary>
        /// 是否已完工下线
        /// </summary>
        [Required]
        [Label("是否已完工下线")]
        public static readonly Property<bool> IsFinishProperty = P<WipProductVersion>.Register(e => e.IsFinish);

        /// <summary>
        /// 是否已完工下线
        /// </summary>
        public bool IsFinish
        {
            get { return GetProperty(IsFinishProperty); }
            set { SetProperty(IsFinishProperty, value); }
        }
        #endregion

        #region 完工时间 FinishDateTime
        /// <summary>
        /// 完工时间
        /// </summary>
        [Label("完工时间")]
        public static readonly Property<DateTime?> FinishDateTimeProperty 
            = P<WipProductVersion>.Register(e => e.FinishDateTime);

        /// <summary>
        /// 完工时间
        /// </summary>
        public DateTime? FinishDateTime
        {
            get { return this.GetProperty(FinishDateTimeProperty); }
            set { this.SetProperty(FinishDateTimeProperty, value); }
        }
        #endregion


        #region 型号 Model
        /// <summary>
        /// 型号
        /// </summary>
        [MaxLength(40)]
        [Label("型号")]
        public static readonly Property<string> ModelProperty = P<WipProductVersion>.Register(e => e.Model);

        /// <summary>
        /// 型号
        /// </summary>
        public string Model
        {
            get { return GetProperty(ModelProperty); }
            set { SetProperty(ModelProperty, value); }
        }
        #endregion

        #region 是否保留,保留状态不能过终检 IsHold
        /// <summary>
        /// 是否保留,保留状态不能过终检
        /// </summary>
        [Required]
        [Label("是否hold")]
        public static readonly Property<bool> IsHoldProperty = P<WipProductVersion>.Register(e => e.IsHold);

        /// <summary>
        /// 是否保留,保留状态不能过终检
        /// </summary>
        public bool IsHold
        {
            get { return GetProperty(IsHoldProperty); }
            set { SetProperty(IsHoldProperty, value); }
        }
        #endregion

        #region 是否返修过 IsFixed
        /// <summary>
        /// 是否返修过
        /// </summary>
        [Required]
        [Label("是否返修")]
        public static readonly Property<bool> IsFixedProperty = P<WipProductVersion>.Register(e => e.IsFixed);

        /// <summary>
        /// 是否返修过
        /// </summary>
        public bool IsFixed
        {
            get { return GetProperty(IsFixedProperty); }
            set { SetProperty(IsFixedProperty, value); }
        }
        #endregion

        #region 是否让步 IsConcession
        /// <summary>
        /// 是否让步
        /// </summary>
        [Required]
        [Label("是否让步")]
        public static readonly Property<bool> IsConcessionProperty = P<WipProductVersion>.Register(e => e.IsConcession);

        /// <summary>
        /// 是否让步
        /// </summary>
        public bool IsConcession
        {
            get { return GetProperty(IsConcessionProperty); }
            set { SetProperty(IsConcessionProperty, value); }
        }
        #endregion

        #region 是否报废 IsScrapped
        /// <summary>
        /// 是否报废
        /// </summary>
        [Required]
        [Label("是否报废")]
        public static readonly Property<bool> IsScrappedProperty = P<WipProductVersion>.Register(e => e.IsScrapped);

        /// <summary>
        /// 是否报废
        /// </summary>
        public bool IsScrapped
        {
            get { return GetProperty(IsScrappedProperty); }
            set { SetProperty(IsScrappedProperty, value); }
        }
        #endregion

        #region 是否能报废重用 CanScrapReuse
        /// <summary>
        /// 是否能报废重用
        /// </summary>
        [Required]
        [Label("是否重用")]
        public static readonly Property<bool> CanScrapReuseProperty = P<WipProductVersion>.Register(e => e.CanScrapReuse);

        /// <summary>
        /// 是否能报废重用
        /// </summary>
        public bool CanScrapReuse
        {
            get { return GetProperty(CanScrapReuseProperty); }
            set { SetProperty(CanScrapReuseProperty, value); }
        }
        #endregion 

        #region 产品等级 Grade
        /// <summary>
        /// 产品等级
        /// </summary>
        [Label("产品等级")]
        public static readonly Property<ProductGrade> GradeProperty = P<WipProductVersion>.Register(e => e.Grade);

        /// <summary>
        /// 产品等级
        /// </summary>
        public ProductGrade Grade
        {
            get { return GetProperty(GradeProperty); }
            set { SetProperty(GradeProperty, value); }
        }
        #endregion

        #region 当前工序 CurrentProcess
        /// <summary>
        /// 当前工序Id
        /// </summary>
        public static readonly IRefIdProperty CurrentProcessIdProperty = P<WipProductVersion>.RegisterRefId(e => e.CurrentProcessId, ReferenceType.Normal);

        /// <summary>
        /// 当前工序Id
        /// </summary>
        public double? CurrentProcessId
        {
            get { return (double?)GetRefNullableId(CurrentProcessIdProperty); }
            set { SetRefNullableId(CurrentProcessIdProperty, value); }
        }

        /// <summary>
        /// 当前工序
        /// </summary>
        public static readonly RefEntityProperty<WipProductProcess> CurrentProcessProperty = P<WipProductVersion>.RegisterRef(e => e.CurrentProcess, CurrentProcessIdProperty);

        /// <summary>
        /// 当前工序
        /// </summary>
        public WipProductProcess CurrentProcess
        {
            get { return GetRefEntity(CurrentProcessProperty); }
            set { SetRefEntity(CurrentProcessProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<WipProductVersion>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<WipProductVersion>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 采集结果 Result
        /// <summary>
        /// 采集结果
        /// </summary>
        [Label("采集结果")]
        private static readonly Property<ResultType> ResultProperty = P<WipProductVersion>.Register(e => e.Result);

        /// <summary>
        /// 采集结果
        /// </summary>
        public ResultType Result
        {
            get { return GetProperty(ResultProperty); }
            set { SetProperty(ResultProperty, value); }
        }
        #endregion

        #region 版本列表 Product
        /// <summary>
        /// 版本列表Id
        /// </summary>
        public static readonly IRefIdProperty ProductIdProperty = P<WipProductVersion>.RegisterRefId(e => e.ProductId, ReferenceType.Parent);

        /// <summary>
        /// 版本列表Id
        /// </summary>
        public double ProductId
        {
            get { return (double)GetRefId(ProductIdProperty); }
            set { SetRefId(ProductIdProperty, value); }
        }

        /// <summary>
        /// 版本列表
        /// </summary>
        public static readonly RefEntityProperty<WipProduct> ProductProperty = P<WipProductVersion>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 版本列表
        /// </summary>
        public WipProduct Product
        {
            get { return GetRefEntity(ProductProperty); }
            set { SetRefEntity(ProductProperty, value); }
        }
        #endregion

        #region 是否暂停 IsPause
        /// <summary>
        /// 是否暂停
        /// </summary>
        [Label("是否暂停")]
        public static readonly Property<YesNo> IsPauseProperty = P<WipProductVersion>.Register(e => e.IsPause);

        /// <summary>
        /// 是否暂停
        /// </summary>
        public YesNo IsPause
        {
            get { return GetProperty(IsPauseProperty); }
            set { SetProperty(IsPauseProperty, value); }
        }
        #endregion

        #region 检验记录 InspectionItemList
        /// <summary>
        /// 检验记录
        /// </summary>
        public static readonly ListProperty<EntityList<WipProductInspectionItem>> InspectionItemListProperty = P<WipProductVersion>.RegisterList(e => e.InspectionItemList);

        /// <summary>
        /// 检验记录
        /// </summary>
        public EntityList<WipProductInspectionItem> InspectionItemList
        {
            get { return this.GetLazyList(InspectionItemListProperty); }
        }
        #endregion

        #region 采集记录 ProcessList
        /// <summary>
        /// 采集记录
        /// </summary>
        public static readonly ListProperty<EntityList<WipProductProcess>> ProcessListProperty = P<WipProductVersion>.RegisterList(e => e.ProcessList);

        /// <summary>
        /// 采集记录
        /// </summary>
        public EntityList<WipProductProcess> ProcessList
        {
            get { return this.GetLazyList(ProcessListProperty); }
        }
        #endregion

        #region 维修记录 RepaireList
        /// <summary>
        /// 维修记录
        /// </summary>
        public static readonly ListProperty<EntityList<WipProductRepair>> RepaireListProperty = P<WipProductVersion>.RegisterList(e => e.RepaireList);

        /// <summary>
        /// 维修记录
        /// </summary>
        public EntityList<WipProductRepair> RepaireList
        {
            get { return this.GetLazyList(RepaireListProperty); }
        }
        #endregion

        #region 不良记录 DefectList
        /// <summary>
        /// 不良记录
        /// </summary>
        public static readonly ListProperty<EntityList<WipProductDefect>> DefectListProperty = P<WipProductVersion>.RegisterList(e => e.DefectList);

        /// <summary>
        /// 不良记录
        /// </summary>
        public EntityList<WipProductDefect> DefectList
        {
            get { return this.GetLazyList(DefectListProperty); }
        }
        #endregion

        #region 下一工序 NextProcess
        /// <summary>
        /// 下一工序Id
        /// </summary>
        [Label("下一工序")]
        public static readonly IRefIdProperty NextProcessIdProperty = P<WipProductVersion>.RegisterRefId(e => e.NextProcessId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Process> NextProcessProperty = P<WipProductVersion>.RegisterRef(e => e.NextProcess, NextProcessIdProperty);

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
            = P<WipProductVersion>.Register(e => e.NextProcessIndex);

        /// <summary>
        /// 下工序顺序号
        /// </summary>
        public int? NextProcessIndex
        {
            get { return this.GetProperty(NextProcessIndexProperty); }
            set { this.SetProperty(NextProcessIndexProperty, value); }
        }
        #endregion

        #region 当前工序顺序号 CurrenrProcessIndex
        /// <summary>
        /// 当前工序顺序号
        /// </summary>
        [Label("当前工序顺序号")]
        public static readonly Property<int?> CurrenrProcessIndexProperty
            = P<WipProductVersion>.Register(e => e.CurrenrProcessIndex);

        /// <summary>
        /// 当前工序顺序号
        /// </summary>
        public int? CurrenrProcessIndex
        {
            get { return this.GetProperty(CurrenrProcessIndexProperty); }
            set { this.SetProperty(CurrenrProcessIndexProperty, value); }
        }
        #endregion


        #region 是否已生成过半成品入线边仓 IsJoinLineWarehouse
        /// <summary>
        /// 是否已生成过半成品入线边仓(-1：已处理不入线边仓 0:待入线边仓1:已入线边仓)
        /// </summary>
        [Label("是否已生成过半成品入线边仓")]
        public static readonly Property<int> IsJoinLineWarehouseProperty = P<WipProductVersion>.Register(e => e.IsJoinLineWarehouse);

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
            = P<WipProductVersion>.Register(e => e.IsOutsourcing);

        /// <summary>
        /// 委外中
        /// </summary>
        public bool IsOutsourcing
        {
            get { return this.GetProperty(IsOutsourcingProperty); }
            set { this.SetProperty(IsOutsourcingProperty, value); }
        }
        #endregion

        #region 扩展属性 ItemExtProp
        /// <summary>
        /// 扩展属性
        /// </summary>
        [Label("扩展属性")]
        public static readonly Property<string> ItemExtPropProperty = P<WipProductVersion>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get { return this.GetProperty(ItemExtPropProperty); }
            set { this.SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 物料扩展属性 ItemExtPropName
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        public static readonly Property<string> ItemExtPropNameProperty = P<WipProductVersion>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName
        {
            get { return this.GetProperty(ItemExtPropNameProperty); }
            set { this.SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 当前工序 NowProcess
        /// <summary>
        /// 当前工序Id
        /// </summary>
        [Label("当前工序")]
        public static readonly IRefIdProperty NowProcessIdProperty =
            P<WipProductVersion>.RegisterRefId(e => e.NowProcessId, ReferenceType.Normal);

        /// <summary>
        /// 当前工序Id
        /// </summary>
        public double? NowProcessId
        {
            get { return (double?)this.GetRefNullableId(NowProcessIdProperty); }
            set { this.SetRefNullableId(NowProcessIdProperty, value); }
        }

        /// <summary>
        /// 当前工序
        /// </summary>
        public static readonly RefEntityProperty<Process> NowProcessProperty =
            P<WipProductVersion>.RegisterRef(e => e.NowProcess, NowProcessIdProperty);

        /// <summary>
        /// 当前工序
        /// </summary>
        public Process NowProcess
        {
            get { return this.GetRefEntity(NowProcessProperty); }
            set { this.SetRefEntity(NowProcessProperty, value); }
        }
        #endregion


        #region 视图属性
        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<WipProductVersion>.RegisterView(e => e.WorkOrderNo, p => p.WorkOrder.No);

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
        public static readonly Property<Core.WorkOrders.WorkOrderType> WoTypeProperty = P<WipProductVersion>.RegisterView(e => e.WoType, p => p.WorkOrder.Type);

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
        public static readonly Property<decimal> WoPlanQtyProperty = P<WipProductVersion>.RegisterView(e => e.WoPlanQty, p => p.WorkOrder.PlanQty);

        /// <summary>
        /// 工单数量
        /// </summary>
        public decimal WoPlanQty
        {
            get { return this.GetProperty(WoPlanQtyProperty); }
        }
        #endregion

        #region 工艺流程名称 VersionName
        /// <summary>
        /// 工艺流程名称
        /// </summary>
        [Label("工艺流程名称")]
        public static readonly Property<string> VersionNameProperty = P<WipProductVersion>.RegisterView(e => e.VersionName, p => p.WorkOrder.Version.Name);

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
        public static readonly Property<string> WorkShopNameProperty = P<WipProductVersion>.RegisterView(e => e.WorkShopName, p => p.WorkOrder.WorkShop.Name);

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
        public static readonly Property<string> ResourceNameProperty = P<WipProductVersion>.RegisterView(e => e.ResourceName, p => p.CurrentProcess.Resource.Name);

        /// <summary>
        /// 当前工位资源
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
        }
        #endregion

        #region 当前工序名称 ProcessName
        /// <summary>
        /// 当前工序名称
        /// </summary>
        [Label("当前工序")]
        public static readonly Property<string> ProcessNameProperty = P<WipProductVersion>.RegisterView(e => e.ProcessName, p => p.CurrentProcess.Process.Name);

        /// <summary>
        /// 当前工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }
        #endregion

        #region 当前工序名称 NowProcessName
        /// <summary>
        /// 当前工序名称
        /// </summary>
        [Label("当前工序名称")]
        public static readonly Property<string> NowProcessNameProperty = P<WipProductVersion>.RegisterView(e => e.NowProcessName, p => p.NowProcess.Name);

        /// <summary>
        /// 当前工序名称
        /// </summary>
        public string NowProcessName
        {
            get { return this.GetProperty(NowProcessNameProperty); }
        }
        #endregion

        #region 产品机型名称 ModelName
        /// <summary>
        /// 产品机型名称
        /// </summary>
        [Label("产品机型")]
        public static readonly Property<string> ModelNameProperty = P<WipProductVersion>.RegisterView(e => e.ModelName, p => p.WorkOrder.Product.Model.Name);

        /// <summary>
        /// 产品机型名称
        /// </summary>
        public string ModelName
        {
            get { return this.GetProperty(ModelNameProperty); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<WipProductVersion>.RegisterView(e => e.ProductCode, p => p.WorkOrder.Product.Code);

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
        public static readonly Property<string> ProductNameProperty = P<WipProductVersion>.RegisterView(e => e.ProductName, p => p.WorkOrder.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }
        #endregion

        #region 工单扩展属性 WorkOrderExtPropName
        /// <summary>
        /// 工单扩展属性
        /// </summary>
        [Label("工单扩展属性")]
        public static readonly Property<string> WorkOrderExtPropNameProperty = P<WipProductVersion>.RegisterView(e => e.WorkOrderExtPropName, p => p.WorkOrder.ItemExtPropName);

        /// <summary>
        /// 工单扩展属性
        /// </summary>
        public string WorkOrderExtPropName
        {
            get { return this.GetProperty(WorkOrderExtPropNameProperty); }
        }
        #endregion

        #region 产品Id WorkOrderProductId
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品Id")]
        public static readonly Property<double> WorkOrderProductIdProperty = P<WipProductVersion>.RegisterView(e => e.WorkOrderProductId, p => p.WorkOrder.ProductId);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double WorkOrderProductId
        {
            get { return this.GetProperty(WorkOrderProductIdProperty); }
        }
        #endregion

        #region 组合板工单号 PanelWorkOrderNo
        /// <summary>
        /// 组合板工单号
        /// </summary>
        [Label("组合板工单号")]
        public static readonly Property<string> PanelWorkOrderNoProperty = P<WipProductVersion>.RegisterView(e => e.PanelWorkOrderNo, p => p.WorkOrder.PanelWorkOrder.No);

        /// <summary>
        /// 组合板工单号
        /// </summary>
        public string PanelWorkOrderNo
        {
            get { return this.GetProperty(PanelWorkOrderNoProperty); }
        }
        #endregion

        #region 数量 BatchQty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> BatchQtyProperty = P<WipProductVersion>.RegisterView(e => e.BatchQty, p => p.Product.BatchQty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal BatchQty
        {
            get { return this.GetProperty(BatchQtyProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 生产产品版本 实体配置
    /// </summary>
    internal class WipProductVersionConfig : EntityConfig<WipProductVersion>
    {
        /// <summary>
        /// 数据库的映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_PROD_VER").MapAllProperties();
            Meta.Property(WipProductVersion.CurrentProcessIdProperty).ColumnMeta.IgnoreFK().IsNullable();
            Meta.Property(WipProductVersion.SnProperty).ColumnMeta.HasIndex();
            Meta.Property(WipProductVersion.ProductIdProperty).ColumnMeta.HasIndex();
            Meta.Property(WipProductVersion.WorkOrderIdProperty).ColumnMeta.HasIndex();
            Meta.Property(WipProductVersion.CombinedCodeProperty).ColumnMeta.HasIndex();
            Meta.Property(WipProductVersion.CurrentProcessIdProperty).ColumnMeta.HasIndex();
            Meta.Property(WipProductVersion.IsJoinLineWarehouseProperty).ColumnMeta.HasIndex();
            //Meta.IndexGroupOnProperties(WipProductVersion.UpdateDateProperty,Desc,WipProductVersion.IdProperty);
            Meta.EnablePhantoms();
        }
    }
}