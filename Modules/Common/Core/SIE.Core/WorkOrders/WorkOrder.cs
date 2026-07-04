using SIE.Core.Items;
using SIE.Core.ProjectMaintains;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Core.WorkOrders
{
    /// <summary>
    /// 工单
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(WorkOrderCriteria))]
    [Label("工单")]
    [DisplayMember(nameof(No))]
    public partial class WorkOrder : DataEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkOrder()
        {
            PlanBeginDate = DateTime.Today;
            PlanEndDate = DateTime.Today;
            Type = WorkOrderType.Mass;
            PlanQty = 0;
        }

        #region 工单号 No
        /// <summary>
        /// 工单号
        /// </summary>
        [Required]
        [Label("工单号")]
        public static readonly Property<string> NoProperty = P<WorkOrder>.Register(e => e.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 计划开始时间 PlanBeginDate
        /// <summary>
        /// 计划开始时间
        /// </summary>
        [Label("计划开始时间")]
        public static readonly Property<DateTime> PlanBeginDateProperty = P<WorkOrder>.Register(e => e.PlanBeginDate, new PropertyMetadata<DateTime>()
        {
            DateTimePart = DateTimePart.Date,
        });

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime PlanBeginDate
        {
            get { return GetProperty(PlanBeginDateProperty); }
            set { SetProperty(PlanBeginDateProperty, value); }
        }
        #endregion

        #region 计划完成时间 PlanEndDate
        /// <summary>
        /// 计划完成时间
        /// </summary>
        [Label("计划完成时间")]
        public static readonly Property<DateTime> PlanEndDateProperty = P<WorkOrder>.Register(e => e.PlanEndDate, new PropertyMetadata<DateTime>()
        {
            DateTimePart = DateTimePart.Date,
        });

        /// <summary>
        /// 计划完成时间
        /// </summary>
        public DateTime PlanEndDate
        {
            get { return GetProperty(PlanEndDateProperty); }
            set { SetProperty(PlanEndDateProperty, value); }
        }
        #endregion

        #region 计划数量 PlanQty
        /// <summary>
        /// 计划数量
        /// </summary>
        [MinValue(0)]
        [Label("计划数量")]
        public static readonly Property<decimal> PlanQtyProperty = P<WorkOrder>.Register(e => e.PlanQty);

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanQty
        {
            get { return GetProperty(PlanQtyProperty); }
            set { SetProperty(PlanQtyProperty, value); }
        }
        #endregion

        #region 实际开始时间 ActuStartDate
        /// <summary>
        /// 实际开始时间
        /// </summary>
        [Label("实际开始时间")]
        public static readonly Property<DateTime?> ActuStartDateProperty = P<WorkOrder>.Register(e => e.ActuStartDate);

        /// <summary>
        /// 实际开始时间
        /// </summary>
        public DateTime? ActuStartDate
        {
            get { return GetProperty(ActuStartDateProperty); }
            set { SetProperty(ActuStartDateProperty, value); }
        }
        #endregion

        #region 实际完成时间 ActuFinishDate
        /// <summary>
        /// 实际完成时间
        /// </summary>
        [Label("实际完成时间")]
        public static readonly Property<DateTime?> ActuFinishDateProperty = P<WorkOrder>.Register(e => e.ActuFinishDate);

        /// <summary>
        /// 实际完成时间
        /// </summary>
        public DateTime? ActuFinishDate
        {
            get { return GetProperty(ActuFinishDateProperty); }
            set { SetProperty(ActuFinishDateProperty, value); }
        }
        #endregion

        #region 产品 Product
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品编码")]
        public static readonly IRefIdProperty ProductIdProperty = P<WorkOrder>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ProductId
        {
            get { return (double)GetRefId(ProductIdProperty); }
            set { SetRefId(ProductIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public static readonly RefEntityProperty<Item> ProductProperty = P<WorkOrder>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Product
        {
            get { return GetRefEntity(ProductProperty); }
            set { SetRefEntity(ProductProperty, value); }
        }
        #endregion

        #region 工单状态 State
        /// <summary>
        /// 工单状态
        /// </summary>
        [Label("工单状态")]
        public static readonly Property<WorkOrderState> StateProperty = P<WorkOrder>.Register(e => e.State);

        /// <summary>
        /// 工单状态
        /// </summary>
        public WorkOrderState State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 工单类型 Type
        /// <summary>
        /// 工单类型
        /// </summary>
        [Label("工单类型")]
        public static readonly Property<WorkOrderType> TypeProperty = P<WorkOrder>.Register(e => e.Type);

        /// <summary>
        /// 工单类型
        /// </summary>
        public WorkOrderType Type
        {
            get { return GetProperty(TypeProperty); }
            set { SetProperty(TypeProperty, value); }
        }
        #endregion

        #region 工厂Id FactoryId
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂Id")]
        public static readonly Property<double?> FactoryIdProperty = P<WorkOrder>.Register(e => e.FactoryId);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double? FactoryId
        {
            get { return this.GetProperty(FactoryIdProperty); }
            set { this.SetProperty(FactoryIdProperty, value); }
        }
        #endregion

        #region 车间Id WorkShopId
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间Id")]
        public static readonly Property<double?> WorkShopIdProperty = P<WorkOrder>.Register(e => e.WorkShopId);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkShopId
        {
            get { return this.GetProperty(WorkShopIdProperty); }
            set { this.SetProperty(WorkShopIdProperty, value); }
        }
        #endregion

        #region 资源Id ResourceId
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源Id")]
        public static readonly Property<double?> ResourceIdProperty = P<WorkOrder>.Register(e => e.ResourceId);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double? ResourceId
        {
            get { return this.GetProperty(ResourceIdProperty); }
            set { this.SetProperty(ResourceIdProperty, value); }
        }
        #endregion

        #region 打印模板 Template
        /// <summary>
        /// 打印模板Id
        /// </summary>
        [Label("打印模板")]
        public static readonly IRefIdProperty TemplateIdProperty =
            P<WorkOrder>.RegisterRefId(e => e.TemplateId, ReferenceType.Normal);

        /// <summary>
        /// 打印模板Id
        /// </summary>
        public double? TemplateId
        {
            get { return (double?)this.GetRefNullableId(TemplateIdProperty); }
            set { this.SetRefNullableId(TemplateIdProperty, value); }
        }

        /// <summary>
        /// 打印模板
        /// </summary>
        public static readonly RefEntityProperty<LabelPrintTemplate> TemplateProperty =
            P<WorkOrder>.RegisterRef(e => e.Template, TemplateIdProperty);

        /// <summary>
        /// 打印模板
        /// </summary>
        public LabelPrintTemplate Template
        {
            get { return this.GetRefEntity(TemplateProperty); }
            set { this.SetRefEntity(TemplateProperty, value); }
        }
        #endregion

        #region 来源ID（手动：计划工单ID  自动：工序任务ID） SourceId
        /// <summary>
        /// 来源ID（手动：计划工单ID  自动：工序任务ID）
        /// </summary>
        [Label("来源ID（手动：计划工单ID  自动：工序任务ID）")]
        public static readonly Property<double?> SourceIdProperty = P<WorkOrder>.Register(e => e.SourceId);

        /// <summary>
        /// 来源ID（手动：计划工单ID  自动：工序任务ID）
        /// </summary>
        public double? SourceId
        {
            get { return GetProperty(SourceIdProperty); }
            set { SetProperty(SourceIdProperty, value); }
        }
        #endregion

        #region 来源单号（手动：计划工单号  自动：工序任务编号） SourceNo
        /// <summary>
        /// 来源单号（手动：计划工单号  自动：工序任务编号）
        /// </summary>
        [Label("来源单号（手动：计划工单号  自动：工序任务编号）")]
        public static readonly Property<string> SourceNoProperty = P<WorkOrder>.Register(e => e.SourceNo);

        /// <summary>
        /// 来源单号（手动：计划工单号  自动：工序任务编号）
        /// </summary>
        public string SourceNo
        {
            get { return GetProperty(SourceNoProperty); }
            set { SetProperty(SourceNoProperty, value); }
        }
        #endregion

        #region 销售订单号 SaleOrderNo
        /// <summary>
        /// 销售订单号
        /// </summary>
        [Label("销售订单号")]
        [MaxLength(2000)]
        public static readonly Property<string> SaleOrderNoProperty = P<WorkOrder>.Register(e => e.SaleOrderNo);

        /// <summary>
        /// 销售订单号
        /// </summary>
        public string SaleOrderNo
        {
            get { return GetProperty(SaleOrderNoProperty); }
            set { SetProperty(SaleOrderNoProperty, value); }
        }
        #endregion

        #region 客户订单号 CustomerOrderNo
        /// <summary>
        /// 客户订单号
        /// </summary>
        [Label("客户订单号")]
        public static readonly Property<string> CustomerOrderNoProperty = P<WorkOrder>.Register(e => e.CustomerOrderNo);

        /// <summary>
        /// 客户订单号
        /// </summary>
        public string CustomerOrderNo
        {
            get { return GetProperty(CustomerOrderNoProperty); }
            set { SetProperty(CustomerOrderNoProperty, value); }
        }
        #endregion

        #region 工单与工单BOM关系 BomList
        /// <summary>
        /// 工单与工单BOM关系
        /// </summary>
        [Label("工单与工单BOM关系")]
        public static readonly ListProperty<EntityList<WorkOrderBom>> BomListProperty = P<WorkOrder>.RegisterList(e => e.BomList);

        /// <summary>
        /// 工单与工单BOM关系
        /// </summary>
        public EntityList<WorkOrderBom> BomList
        {
            get { return this.GetLazyList(BomListProperty); }
        }
        #endregion

        #region 注册视图属性(关联实体属性平铺显示，一般用于Web)
        #region 工单产品编码 WorkOrderProductCode
        /// <summary>
        /// 工单产品编码
        /// </summary>
        [Label("工单产品编码")]
        public static readonly Property<string> WorkOrderProductCodeProperty = P<WorkOrder>.RegisterView(e => e.WorkOrderProductCode, p => p.Product.Code);

        /// <summary>
        /// 工单产品编码
        /// </summary>
        public string WorkOrderProductCode
        {
            get { return this.GetProperty(WorkOrderProductCodeProperty); }
        }
        #endregion

        #region 工单产品名称 WorkOrderProductName
        /// <summary>
        /// 工单产品名称
        /// </summary>
        [Label("工单产品名称")]
        public static readonly Property<string> WorkOrderProductNameProperty = P<WorkOrder>.RegisterView(e => e.WorkOrderProductName, p => p.Product.Name);

        /// <summary>
        /// 工单产品名称
        /// </summary>
        public string WorkOrderProductName
        {
            get { return this.GetProperty(WorkOrderProductNameProperty); }
        }
        #endregion

        #region 组合板工单号 PanelWorkOrderNo
        /// <summary>
        /// 组合板工单号
        /// </summary>
        [Label("组合板工单号")]
        public static readonly Property<string> PanelWorkOrderNoProperty = P<WorkOrder>.RegisterView(e => e.PanelWorkOrderNo, p => p.PanelWorkOrder.No);

        /// <summary>
        /// 组合板工单号
        /// </summary>
        public string PanelWorkOrderNo
        {
            get { return this.GetProperty(PanelWorkOrderNoProperty); }
        }
        #endregion       
        #endregion

        #region 已打印数量 PrintedQty
        /// <summary>
        /// 已打印数量
        /// </summary>
        [Label("已打印数量")]
        public static readonly Property<int> PrintedQtyProperty = P<WorkOrder>.Register(e => e.PrintedQty);

        /// <summary>
        /// 已打印数量
        /// </summary>
        public int PrintedQty
        {
            get { return this.GetProperty(PrintedQtyProperty); }
            set { this.SetProperty(PrintedQtyProperty, value); }
        }
        #endregion 

        #region 拼板数 PanelQty
        /// <summary>
        /// 拼板数
        /// </summary>
        [Label("拼板数")]
        [MinValue(0)]
        public static readonly Property<int> PanelQtyProperty = P<WorkOrder>.Register(e => e.PanelQty);

        /// <summary>
        /// 拼板数
        /// </summary>
        public int PanelQty
        {
            get { return this.GetProperty(PanelQtyProperty); }
            set { this.SetProperty(PanelQtyProperty, value); }
        }
        #endregion 

        #region 拼板码已打印数量 PanelPrintQty
        /// <summary>
        /// 拼板码已打印数量
        /// </summary>
        [Label("拼板码已打印数量")]
        public static readonly Property<decimal> PanelPrintQtyProperty = P<WorkOrder>.Register(e => e.PanelPrintQty);

        /// <summary>
        /// 拼板码已打印数量
        /// </summary>
        public decimal PanelPrintQty
        {
            get { return this.GetProperty(PanelPrintQtyProperty); }
            set { this.SetProperty(PanelPrintQtyProperty, value); }
        }
        #endregion 

        #region 是否使用旧条码 UseOldSn
        /// <summary>
        /// 是否使用旧条码
        /// </summary>
        [Label("是否使用旧条码")]
        public static readonly Property<bool> UseOldSnProperty = P<WorkOrder>.Register(e => e.UseOldSn);

        /// <summary>
        /// 是否使用旧条码
        /// </summary>
        public bool UseOldSn
        {
            get { return this.GetProperty(UseOldSnProperty); }
            set { this.SetProperty(UseOldSnProperty, value); }
        }
        #endregion 

        #region 是否暂停 IsPause
        /// <summary>
        /// 是否暂停
        /// </summary>
        [Label("是否暂停")]
        public static readonly Property<YesNo> IsPauseProperty = P<WorkOrder>.Register(e => e.IsPause);

        /// <summary>
        /// 是否暂停
        /// </summary>
        public YesNo IsPause
        {
            get { return GetProperty(IsPauseProperty); }
            set { SetProperty(IsPauseProperty, value); }
        }
        #endregion 

        #region 来源主键 SourceKey
        /// <summary>
        /// 来源主键, 一般用于接口平台事务上传
        /// </summary>
        [Label("来源主键")]
        public static readonly Property<string> SourceKeyProperty = P<WorkOrder>.Register(e => e.SourceKey);

        /// <summary>
        /// 来源主键
        /// </summary>
        public string SourceKey
        {
            get { return this.GetProperty(SourceKeyProperty); }
            set { this.SetProperty(SourceKeyProperty, value); }
        }
        #endregion

        #region 组合板工单 PanelWorkOrder
        /// <summary>
        /// 组合板工单Id
        /// </summary>
        [Label("组合板工单")]
        public static readonly IRefIdProperty PanelWorkOrderIdProperty = P<WorkOrder>.RegisterRefId(e => e.PanelWorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 组合板工单Id
        /// </summary>
        public double? PanelWorkOrderId
        {
            get { return (double?)GetRefNullableId(PanelWorkOrderIdProperty); }
            set { SetRefNullableId(PanelWorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 组合板工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> PanelWorkOrderProperty = P<WorkOrder>.RegisterRef(e => e.PanelWorkOrder, PanelWorkOrderIdProperty);

        /// <summary>
        /// 组合板工单
        /// </summary>
        public WorkOrder PanelWorkOrder
        {
            get { return GetRefEntity(PanelWorkOrderProperty); }
            set { SetRefEntity(PanelWorkOrderProperty, value); }
        }
        #endregion

        #region 是否组合板工单 IsPanelWorkOrder
        /// <summary>
        /// 是否组合板工单
        /// </summary>
        [Label("是否组合板工单")]
        public static readonly Property<bool> IsPanelWorkOrderProperty = P<WorkOrder>.Register(e => e.IsPanelWorkOrder);

        /// <summary>
        /// 是否组合板工单
        /// </summary>
        public bool IsPanelWorkOrder
        {
            get { return GetProperty(IsPanelWorkOrderProperty); }
            set { SetProperty(IsPanelWorkOrderProperty, value); }
        }
        #endregion

        #region 生产订单号 ProductionOrderCode
        /// <summary>
        /// 生产订单号
        /// </summary>
        [Label("生产订单号")]
        [MaxLength(2000)]
        public static readonly Property<string> ProductionOrderCodeProperty = P<WorkOrder>.Register(e => e.ProductionOrderCode);

        /// <summary>
        /// 生产订单号
        /// </summary>
        public string ProductionOrderCode
        {
            get { return GetProperty(ProductionOrderCodeProperty); }
            set { SetProperty(ProductionOrderCodeProperty, value); }
        }
        #endregion

        #region 扩展属性 ItemExtProp
        /// <summary>
        /// 扩展属性
        /// </summary>
        [Label("扩展属性")]
        public static readonly Property<string> ItemExtPropProperty = P<WorkOrder>.Register(e => e.ItemExtProp);

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
        public static readonly Property<string> ItemExtPropNameProperty = P<WorkOrder>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 扩展属性值
        /// </summary>
        public string ItemExtPropName
        {
            get { return GetProperty(ItemExtPropNameProperty); }
            set { SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 上传标识 UploadFlag
        /// <summary>
        /// 上传标识
        /// </summary>
        [Label("上传标识")]
        public static readonly Property<bool> UploadFlagProperty = P<WorkOrder>.Register(e => e.UploadFlag);

        /// <summary>
        /// 上传标识
        /// </summary>
        public bool UploadFlag
        {
            get { return GetProperty(UploadFlagProperty); }
            set { SetProperty(UploadFlagProperty, value); }
        }
        #endregion

        #region 完工数量 FinishQty
        /// <summary>
        /// 完工数量
        /// </summary>
        [Required]
        [MinValue(0)]
        [Label("完工数量")]
        public static readonly Property<decimal> FinishQtyProperty = P<WorkOrder>.Register(e => e.FinishQty);

        /// <summary>
        /// 完工数量
        /// </summary>
        public decimal FinishQty
        {
            get { return this.GetProperty(FinishQtyProperty); }
            set { this.SetProperty(FinishQtyProperty, value); }
        }
        #endregion

        #region 备料类型 WorkOrderMpType
        /// <summary>
        /// 备料类型
        /// </summary>
        [Label("备料类型")]
        public static readonly Property<WorkOrderMpType?> WorkOrderMpTypeProperty = P<WorkOrder>.Register(e => e.WorkOrderMpType);

        /// <summary>
        /// 备料类型
        /// </summary>
        public WorkOrderMpType? WorkOrderMpType
        {
            get { return this.GetProperty(WorkOrderMpTypeProperty); }
            set { this.SetProperty(WorkOrderMpTypeProperty, value); }
        }
        #endregion

        #region 项目号 ProjectMaintain
        /// <summary>
        /// 项目号Id
        /// </summary>
        [Label("项目号")]
        public static readonly IRefIdProperty ProjectMaintainIdProperty =
            P<WorkOrder>.RegisterRefId(e => e.ProjectMaintainId, ReferenceType.Normal);

        /// <summary>
        /// 项目号Id
        /// </summary>
        public double? ProjectMaintainId
        {
            get { return (double?)this.GetRefNullableId(ProjectMaintainIdProperty); }
            set { this.SetRefNullableId(ProjectMaintainIdProperty, value); }
        }

        /// <summary>
        /// 项目号
        /// </summary>
        public static readonly RefEntityProperty<ProjectMaintain> ProjectMaintainProperty =
            P<WorkOrder>.RegisterRef(e => e.ProjectMaintain, ProjectMaintainIdProperty);

        /// <summary>
        /// 项目号
        /// </summary>
        public ProjectMaintain ProjectMaintain
        {
            get { return this.GetRefEntity(ProjectMaintainProperty); }
            set { this.SetRefEntity(ProjectMaintainProperty, value); }
        }
        #endregion

        #region 初始入库数量 InitStorageQty
        /// <summary>
        /// 初始入库数量
        /// </summary>
        [Label("初始入库数量")]
        public static readonly Property<decimal?> InitStorageQtyProperty = P<WorkOrder>.Register(e => e.InitStorageQty);

        /// <summary>
        /// 初始入库数量
        /// </summary>
        public decimal? InitStorageQty
        {
            get { return this.GetProperty(InitStorageQtyProperty); }
            set { this.SetProperty(InitStorageQtyProperty, value); }
        }
        #endregion

        #region 申请入库数量(WMS) ApplyWmsStorageQty
        /// <summary>
        /// 申请入库数量(WMS)
        /// </summary>
        [Label("累申请入库数量")]
        public static readonly Property<decimal?> ApplyWmsStorageQtyProperty = P<WorkOrder>.Register(e => e.ApplyWmsStorageQty);

        /// <summary>
        /// 申请入库数量(WMS)
        /// </summary>
        public decimal? ApplyWmsStorageQty
        {
            get { return this.GetProperty(ApplyWmsStorageQtyProperty); }
            set { this.SetProperty(ApplyWmsStorageQtyProperty, value); }
        }
        #endregion

        #region 入库数量(WMS) WmsStorageQty
        /// <summary>
        /// 入库数量(WMS)
        /// </summary>
        [Label("入库数量")]
        public static readonly Property<decimal?> WmsStorageQtyProperty = P<WorkOrder>.Register(e => e.WmsStorageQty);

        /// <summary>
        /// 入库数量(WMS)
        /// </summary>
        public decimal? WmsStorageQty
        {
            get { return this.GetProperty(WmsStorageQtyProperty); }
            set { this.SetProperty(WmsStorageQtyProperty, value); }
        }
        #endregion

        #region 申请完工退库数(WMS) ApplyWmsReturnQty
        /// <summary>
        /// 申请完工退库数(WMS)
        /// </summary>
        [Label("申请完工退库数")]
        public static readonly Property<decimal?> ApplyWmsReturnQtyProperty = P<WorkOrder>.Register(e => e.ApplyWmsReturnQty);

        /// <summary>
        /// 申请完工退库数(WMS)
        /// </summary>
        public decimal? ApplyWmsReturnQty
        {
            get { return this.GetProperty(ApplyWmsReturnQtyProperty); }
            set { this.SetProperty(ApplyWmsReturnQtyProperty, value); }
        }
        #endregion

        #region 完工退库数(WMS) WmsReturnQty
        /// <summary>
        /// 完工退库数(WMS)
        /// </summary>
        [Label("完工退库数")]
        public static readonly Property<decimal?> WmsReturnQtyProperty = P<WorkOrder>.Register(e => e.WmsReturnQty);

        /// <summary>
        /// 完工退库数(WMS)
        /// </summary>
        public decimal? WmsReturnQty
        {
            get { return this.GetProperty(WmsReturnQtyProperty); }
            set { this.SetProperty(WmsReturnQtyProperty, value); }
        }
        #endregion

        #region 批次 BatchNo
        /// <summary>
        /// 批次
        /// </summary>
        [Label("批次")]
        public static readonly Property<string> BatchNoProperty = P<WorkOrder>.Register(e => e.BatchNo);

        /// <summary>
        /// 批次
        /// </summary>
        public string BatchNo
        {
            get { return this.GetProperty(BatchNoProperty); }
            set { this.SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 关闭前状态 ClosingState
        /// <summary>
        /// 关闭前状态
        /// </summary>
        [Label("关闭前状态")]
        public static readonly Property<WorkOrderState?> ClosingStateProperty = P<WorkOrder>.Register(e => e.ClosingState);

        /// <summary>
        /// 关闭前状态
        /// </summary>
        public WorkOrderState? ClosingState
        {
            get { return this.GetProperty(ClosingStateProperty); }
            set { this.SetProperty(ClosingStateProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 工单 实体配置
    /// </summary>
    internal class WorkOrderConfig : EntityConfig<WorkOrder>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(WorkOrder.NoProperty, new NotDuplicateRule());
            base.AddValidations(rules);
        }

        /// <summary>
        /// 对Meta属性的配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WO").MapAllProperties();
            Meta.Property(WorkOrder.NoProperty).ColumnMeta.HasIndex();
            Meta.Property(WorkOrder.ProductionOrderCodeProperty).ColumnMeta.HasIndex();
            Meta.Property(WorkOrder.PanelWorkOrderIdProperty).ColumnMeta.HasIndex();
            Meta.Property(WorkOrder.SaleOrderNoProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }

    /// <summary>
    /// 实体页面配置
    /// </summary>
    internal class WorkOrderViewConfig : WPFViewConfig<WorkOrder>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("工单").HasDelegate(WorkOrder.NoProperty);
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.No).HasLabel("工单编号").Show(ShowInWhere.All);
                View.Property(p => p.PlanBeginDate).HasLabel("计划开始时间").Show(ShowInWhere.All);
                View.Property(p => p.PlanEndDate).HasLabel("计划完成时间").Show(ShowInWhere.All);
                View.Property(p => p.PlanQty).HasLabel("计划数量").Show(ShowInWhere.All);
                View.Property(p => p.ActuFinishDate).HasLabel("实际完成时间").Show(ShowInWhere.All);
                View.Property(p => p.State).HasLabel("状态").Show(ShowInWhere.All);
                View.Property(p => p.WorkOrderProductCode).HasLabel("产品编码").Show(ShowInWhere.All);
                View.ChildrenProperty(p => p.BomList).Show(ChildShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 下拉选择视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.No).Show();
            View.Property(p => p.WorkOrderProductCode).HasLabel("产品编码").Show();
            View.Property(p => p.WorkOrderProductName).HasLabel("产品名称").Show();
            View.Property(p => p.PlanBeginDate).Show();
            View.Property(p => p.PlanEndDate).Show();
            View.Property(P => P.PlanQty).Show();
            View.Property(p => p.ActuFinishDate).Show();
            View.Property(p => p.State).Show();

            View.ChildrenProperty(p => p.BomList).Show(ChildShowInWhere.Hide);
        }
    }

    /// <summary>
    /// 工单Web实体页面配置
    /// </summary>
    internal class WorkOrderWebViewConfig : WebViewConfig<WorkOrder>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("工单").HasDelegate(WorkOrder.NoProperty);
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.No).HasLabel("工单编号").Show(ShowInWhere.All).FixColumn();
                View.Property(p => p.WorkOrderProductCode).HasLabel("产品编码").Show(ShowInWhere.All).FixColumn();
                View.Property(p => p.WorkOrderProductName).HasLabel("产品名称").Show(ShowInWhere.All).FixColumn();
                View.Property(p => p.PlanBeginDate).HasLabel("计划开始时间").Show(ShowInWhere.All);
                View.Property(p => p.PlanEndDate).HasLabel("计划完成时间").Show(ShowInWhere.All);
                View.Property(p => p.PlanQty).HasLabel("计划数量").Show(ShowInWhere.All);
                View.Property(p => p.ActuFinishDate).HasLabel("实际完成时间").Show(ShowInWhere.All);
                View.Property(p => p.State).HasLabel("状态").Show(ShowInWhere.All);
                View.ChildrenProperty(p => p.BomList).Show(ChildShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 下拉选择视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.No).Show();
            View.Property(p => p.WorkOrderProductCode).HasLabel("产品编码").Show();
            View.Property(p => p.WorkOrderProductName).HasLabel("产品名称").Show();
            View.Property(p => p.PlanBeginDate).Show();
            View.Property(p => p.PlanEndDate).Show();
            View.Property(P => P.PlanQty).Show();
            View.Property(p => p.ActuFinishDate).Show();
            View.Property(p => p.State).Show();
            View.ChildrenProperty(p => p.BomList).Show(ChildShowInWhere.Hide);
        }
    }
}