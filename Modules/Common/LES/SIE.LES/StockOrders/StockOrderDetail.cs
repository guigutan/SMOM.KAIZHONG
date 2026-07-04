using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.LES.Commons;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using SIE.Warehouses;
using System;

namespace SIE.LES.StockOrders
{
    /// <summary>
    /// 物料需求明细
    /// </summary>
    [ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("物料需求明细")]
    public partial class StockOrderDetail : DataEntity
    {
        #region 行号 LineNo
        /// <summary>
        /// 行号
        /// </summary>
        [Label("行号")]
        public static readonly Property<string> LineNoProperty = P<StockOrderDetail>.Register(e => e.LineNo);

        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo
        {
            get => GetProperty(LineNoProperty);
            set => SetProperty(LineNoProperty, value);
        }
        #endregion

        #region 本次备料量 Qty
        /// <summary>
        /// 本次备料量
        /// </summary>
        [Required]
        [MinValue(0)]
        [Label("本次备料量")]
        public static readonly Property<decimal> QtyProperty = P<StockOrderDetail>.Register(e => e.Qty);

        /// <summary>
        /// 本次备料量
        /// </summary>
        public decimal Qty
        {
            get => GetProperty(QtyProperty);
            set => SetProperty(QtyProperty, value);
        }
        #endregion

        #region 需求时间 DemandTime
        /// <summary>
        /// 需求时间
        /// </summary>
        [Required]
        [Label("需求时间")]
        public static readonly Property<DateTime> DemandTimeProperty = P<StockOrderDetail>.Register(e => e.DemandTime);

        /// <summary>
        /// 需求时间
        /// </summary>
        public DateTime DemandTime
        {
            get => GetProperty(DemandTimeProperty);
            set => SetProperty(DemandTimeProperty, value);
        }
        #endregion

        #region 已发运数量 ShipQty
        /// <summary>
        /// 已发运数量
        /// </summary>
        [Label("已发运数量")]
        public static readonly Property<decimal> ShipQtyProperty = P<StockOrderDetail>.Register(e => e.ShipQty);

        /// <summary>
        /// 已发运数量
        /// </summary>
        public decimal ShipQty
        {
            get => GetProperty(ShipQtyProperty);
            set => SetProperty(ShipQtyProperty, value);
        }
        #endregion

        #region 已接收数量 ReceiveQty
        /// <summary>
        /// 已接收数量
        /// </summary>
        [Label("已接收数量")]
        public static readonly Property<decimal> ReceiveQtyProperty = P<StockOrderDetail>.Register(e => e.ReceiveQty);

        /// <summary>
        /// 已接收数量
        /// </summary>
        public decimal ReceiveQty
        {
            get => GetProperty(ReceiveQtyProperty);
            set => SetProperty(ReceiveQtyProperty, value);
        }
        #endregion

        #region 取消数量 CancelQty
        /// <summary>
        /// 取消数量
        /// </summary>
        [Label("取消数量")]
        public static readonly Property<decimal> CancelQtyProperty = P<StockOrderDetail>.Register(e => e.CancelQty);

        /// <summary>
        /// 取消数量
        /// </summary>
        public decimal CancelQty
        {
            get => GetProperty(CancelQtyProperty);
            set => SetProperty(CancelQtyProperty, value);
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料编码")]
        public static readonly IRefIdProperty ItemIdProperty = P<StockOrderDetail>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get => (double)GetRefId(ItemIdProperty);
            set => SetRefId(ItemIdProperty, value);
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<StockOrderDetail>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get => GetRefEntity(ItemProperty);
            set => SetRefEntity(ItemProperty, value);
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<StockOrderDetail>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId
        {
            get => (double?)GetRefNullableId(ProcessIdProperty);
            set => SetRefNullableId(ProcessIdProperty, value);
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<StockOrderDetail>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get => GetRefEntity(ProcessProperty);
            set => SetRefEntity(ProcessProperty, value);
        }
        #endregion

        #region 备料接收仓库 Warehouse
        /// <summary>
        /// 备料接收仓库Id
        /// </summary>
        [Label("备料接收仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<StockOrderDetail>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 备料接收仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get => (double?)GetRefNullableId(WarehouseIdProperty);
            set => SetRefNullableId(WarehouseIdProperty, value);
        }

        /// <summary>
        /// 备料接收仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<StockOrderDetail>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 备料接收仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get => GetRefEntity(WarehouseProperty);
            set => SetRefEntity(WarehouseProperty, value);
        }
        #endregion

        #region 备料接收库位 StorageLocation
        /// <summary>
        /// 备料接收库位Id
        /// </summary>
        [Label("备料接收库位")]
        public static readonly IRefIdProperty StorageLocationIdProperty = P<StockOrderDetail>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

        /// <summary>
        /// 备料接收库位Id
        /// </summary>
        public double? StorageLocationId
        {
            get => (double?)GetRefNullableId(StorageLocationIdProperty);
            set => SetRefNullableId(StorageLocationIdProperty, value);
        }

        /// <summary>
        /// 备料接收库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty = P<StockOrderDetail>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 备料接收库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get => GetRefEntity(StorageLocationProperty);
            set => SetRefEntity(StorageLocationProperty, value);
        }
        #endregion

        #region 备料状态 StockState
        /// <summary>
        /// 备料状态
        /// </summary>
        [Label("备料状态")]
        public static readonly Property<StockState> StockStateProperty = P<StockOrderDetail>.Register(e => e.StockState);

        /// <summary>
        /// 备料状态
        /// </summary>
        public StockState StockState
        {
            get => GetProperty(StockStateProperty);
            set => SetProperty(StockStateProperty, value);
        }
        #endregion

        #region 备料单 StockOrder
        /// <summary>
        /// 备料单Id
        /// </summary>
        public static readonly IRefIdProperty StockOrderIdProperty = P<StockOrderDetail>.RegisterRefId(e => e.StockOrderId, ReferenceType.Parent);

        /// <summary>
        /// 备料单Id
        /// </summary>
        public double StockOrderId
        {
            get => (double)GetRefId(StockOrderIdProperty);
            set => SetRefId(StockOrderIdProperty, value);
        }

        /// <summary>
        /// 备料单
        /// </summary>
        public static readonly RefEntityProperty<StockOrder> StockOrderProperty = P<StockOrderDetail>.RegisterRef(e => e.StockOrder, StockOrderIdProperty);

        /// <summary>
        /// 备料单
        /// </summary>
        public StockOrder StockOrder
        {
            get => GetRefEntity(StockOrderProperty);
            set => SetRefEntity(StockOrderProperty, value);
        }
        #endregion

        #region 是否启用手工物料接收 IsManualRec
        /// <summary>
        /// 是否启用手工物料接收
        /// </summary>
        [Label("是否启用手工物料接收")]
        public static readonly Property<bool> IsManualRecProperty = P<StockOrderDetail>.Register(e => e.IsManualRec);

        /// <summary>
        /// 是否启用手工物料接收
        /// </summary>
        public bool IsManualRec
        {
            get => GetProperty(IsManualRecProperty);
            set => SetProperty(IsManualRecProperty, value);
        }
        #endregion

        #region 工单总需求 WoTotalQty
        /// <summary>
        /// 工单总需求
        /// </summary>
        [Label("工单总需求")]
        public static readonly Property<decimal> WoTotalQtyProperty = P<StockOrderDetail>.Register(e => e.WoTotalQty);

        /// <summary>
        /// 工单总需求
        /// </summary>
        public decimal WoTotalQty
        {
            get => GetProperty(WoTotalQtyProperty);
            set => SetProperty(WoTotalQtyProperty, value);
        }
        #endregion

        #region 物料扩展属性 ItemExtProp
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        public static readonly Property<string> ItemExtPropProperty = P<StockOrderDetail>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get => GetProperty(ItemExtPropProperty);
            set => SetProperty(ItemExtPropProperty, value);
        }
        #endregion         

        #region 物料扩展属性 ItemExtPropName
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性名称")]
        public static readonly Property<string> ItemExtPropNameProperty = P<StockOrderDetail>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName
        {
            get => GetProperty(ItemExtPropNameProperty);
            set => SetProperty(ItemExtPropNameProperty, value);
        }
        #endregion

        #region 视图属性

        #region 物料编号 ItemCode
        /// <summary>
        /// 物料编号
        /// </summary>
        [Label("物料编号")]
        public static readonly Property<string> ItemCodeProperty = P<StockOrderDetail>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 物料编号
        /// </summary>
        public string ItemCode { get => GetProperty(ItemCodeProperty); set => SetProperty(ItemCodeProperty, value); }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<StockOrderDetail>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get => GetProperty(ItemNameProperty); set => SetProperty(ItemNameProperty, value); }
        #endregion

        #region 物料规格 SpecificationModel
        /// <summary>
        /// 物料规格r
        /// </summay>
        [Label("物料规格")]
        public static readonly Property<string> SpecificationModelProperty = P<StockOrderDetail>.RegisterView(e => e.SpecificationModel, p => p.Item.SpecificationModel);

        /// <summary>
        /// 物料规格
        /// </summary>
        public string SpecificationModel => GetProperty(SpecificationModelProperty);
        #endregion

        #region 物料消耗方式 ConsumeMode
        /// <summary>
        /// 物料消耗方式
        /// </summary>
        [Label("物料消耗方式")]
        public static readonly Property<ConsumeMode> ConsumeModeProperty = P<StockOrderDetail>.RegisterView(e => e.ConsumeMode, p => p.Item.ConsumeMode);

        /// <summary>
        /// 物料消耗方式
        /// </summary>
        public ConsumeMode ConsumeMode
        {
            get { return this.GetProperty(ConsumeModeProperty); }
            set { this.SetProperty(ConsumeModeProperty, value); }
        }
        #endregion

        #region 需求计算方式 DemandMode
        /// <summary>
        /// 需求计算方式
        /// </summary>
        [Label("需求计算方式")]
        public static readonly Property<DemandMode> DemandModeProperty = P<StockOrderDetail>.RegisterView(e => e.DemandMode, p => p.StockOrder.DemandMode);

        /// <summary>
        /// 需求计算方式
        /// </summary>
        public DemandMode DemandMode => GetProperty(DemandModeProperty);
        #endregion

        #region 备料模式 StockType
        /// <summary>
        /// 备料模式
        /// </summary>
        [Label("备料模式")]
        public static readonly Property<PrepareItemType> StockTypeProperty = P<StockOrderDetail>.RegisterView(e => e.StockType, p => p.StockOrder.StockType);

        /// <summary>
        /// 备料模式
        /// </summary>
        public PrepareItemType StockType => GetProperty(StockTypeProperty);
        #endregion

        #region 工单ID WorkOrderId
        /// <summary>
        /// 工单ID
        /// </summary>
        [Label("工单ID")]
        public static readonly Property<double?> WorkOrderIdProperty = P<StockOrderDetail>.RegisterView(e => e.WorkOrderId, p => p.StockOrder.WorkOrderId);

        /// <summary>
        /// 工单ID
        /// </summary>
        public double? WorkOrderId => GetProperty(WorkOrderIdProperty);
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<StockOrderDetail>.RegisterView(e => e.WorkOrderNo, p => p.StockOrder.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo => GetProperty(WorkOrderNoProperty);
        #endregion

        #region 启用扩展属性 IsEnableItemExtProp
        /// <summary>
        /// 启用扩展属性
        /// </summary>
        [Label("启用扩展属性")]
        public static readonly Property<bool> IsEnableItemExtPropProperty = P<StockOrderDetail>.RegisterView(e => e.IsEnableItemExtProp, p => p.Item.EnableExtendProperty);

        /// <summary>
        /// 启用扩展属性
        /// </summary>
        public bool IsEnableItemExtProp { get => GetProperty(IsEnableItemExtPropProperty); set => SetProperty(IsEnableItemExtPropProperty, value); }
        #endregion

        #region 生产资源ID ResourceId
        /// <summary>
        /// 生产资源ID
        /// </summary>
        [Label("生产资源")]
        public static readonly Property<double?> ResourceIdProperty = P<StockOrderDetail>.RegisterView(e => e.ResourceId, p => p.StockOrder.Resource.Id);

        /// <summary>
        /// 生产资源ID
        /// </summary>
        public double? ResourceId => GetProperty(ResourceIdProperty);
        #endregion

        #region 备料单单号 StockOrderNo
        /// <summary>
        /// 备料单单号
        /// </summary>
        [Label("备料单单号")]
        public static readonly Property<string> StockOrderNoProperty = P<StockOrderDetail>.RegisterView(e => e.StockOrderNo, p => p.StockOrder.No);

        /// <summary>
        /// 备料单单号
        /// </summary>
        public string StockOrderNo => GetProperty(StockOrderNoProperty);
        #endregion

        #region 生产部门 EnterpriseCode
        /// <summary>
        /// 生产部门
        /// </summary>
        [Label("生产部门")]
        public static readonly Property<string> EnterpriseCodeProperty = P<StockOrderDetail>.RegisterView(e => e.EnterpriseCode, p => p.StockOrder.WorkShop.Code);

        /// <summary>
        /// 生产部门
        /// </summary>
        public string EnterpriseCode => GetProperty(EnterpriseCodeProperty);
        #endregion

        #region 物料单位 ItemUnit
        /// <summary>
        /// 物料单位
        /// </summary>
        [Label("物料单位")]
        public static readonly Property<string> ItemUnitProperty = P<StockOrderDetail>.RegisterView(e => e.ItemUnit, p => p.Item.Unit.Name);

        /// <summary>
        /// 物料单位
        /// </summary>
        public string ItemUnit
        {
            get { return this.GetProperty(ItemUnitProperty); }
        }
        #endregion


        #endregion

        #region 不映射数据栏位
        #region 是否允许编辑物料扩展属性 IsAllowEdit
        /// <summary>
        /// 是否允许编辑物料扩展属性
        /// </summary>
        [Label("是否允许编辑物料扩展属性")]
        public static readonly Property<bool> IsAllowEditProperty = P<StockOrderDetail>.Register(e => e.IsAllowEdit);

        /// <summary>
        /// 是否允许编辑物料扩展属性
        /// </summary>
        public bool IsAllowEdit
        {
            get => GetProperty(IsAllowEditProperty);
            set => SetProperty(IsAllowEditProperty, value);
        }
        #endregion

        #region 仓库名称 WareName
        /// <summary>
        /// 仓库名称
        /// </summary>
        [Label("仓库名称")]
        public static readonly Property<string> WareNameProperty = P<StockOrderDetail>.Register(e => e.WareName);

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WareName
        {
            get { return this.GetProperty(WareNameProperty); }
            set { this.SetProperty(WareNameProperty, value); }
        }
        #endregion

        #region 库位名称 StorName
        /// <summary>
        /// 库位名称
        /// </summary>
        [Label("库位名称")]
        public static readonly Property<string> StorNameProperty = P<StockOrderDetail>.Register(e => e.StorName);

        /// <summary>
        /// 库位名称
        /// </summary>
        public string StorName
        {
            get { return this.GetProperty(StorNameProperty); }
            set { this.SetProperty(StorNameProperty, value); }
        }
        #endregion

        #endregion

        #region 未完成数量 UnFinishQty
        /// <summary>
        /// 未完成数量
        /// </summary>
        [Label("未完成数量")]
        public static readonly Property<decimal> UnFinishQtyProperty = P<StockOrderDetail>.RegisterReadOnly(
            e => e.UnFinishQty, e => e.GetUnFinishQty());
        /// <summary>
        /// 未完成数量
        /// </summary>

        public decimal UnFinishQty
        {
            get { return this.GetProperty(UnFinishQtyProperty); }
        }
        private decimal GetUnFinishQty()
        {
            var unfinQty = Qty - ReceiveQty - CancelQty;
            if (unfinQty > 0)
                return unfinQty;
            else
                return 0;
        }
        #endregion

    }

    /// <summary>
    /// 物料需求明细 实体配置
    /// </summary>
    internal class StockOrderDetailConfig : EntityConfig<StockOrderDetail>
    {
        /// <summary>
        /// 增加验证逻辑
        /// </summary>
        /// <param name="rules">验证集合</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(new NotDuplicateRule()
            {
                Properties =
                {
                    StockOrderDetail.StockOrderIdProperty,
                    StockOrderDetail.LineNoProperty,
                },
                MessageBuilder = o =>
                {
                    return "已经存在相同的需求明细行号".L10N();
                }
            }, new RuleMeta { Scope = EntityStatusScopes.Add | EntityStatusScopes.Update });
        }

        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("STOCK_ORDER_DTL").MapAllProperties();
            Meta.Property(StockOrderDetail.IsAllowEditProperty).DontMapColumn();
            Meta.Property(StockOrderDetail.WareNameProperty).DontMapColumn();
            Meta.Property(StockOrderDetail.StorNameProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}