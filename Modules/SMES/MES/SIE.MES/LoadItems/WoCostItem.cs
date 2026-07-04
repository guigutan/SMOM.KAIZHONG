using SIE.Common.Configs;
using SIE.Domain;
using SIE.Items;
using SIE.MES.LoadItems.Configs;
using SIE.MES.LoadItems.Enum;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Packages.ItemLabels;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using SIE.Warehouses;
using System;
using static IronPython.Modules._ast;

namespace SIE.MES.LoadItems
{
    /// <summary>
    /// 工单耗用单
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(WoCostItemCriterial))]
    [EntityWithConfig(typeof(WoCostItemNoConfig))]
    [Label("工单耗用单")]
    public partial class WoCostItem : DataEntity
    {
        #region 耗用单号 CostNo
        /// <summary>
        /// 耗用单号
        /// </summary>
        [Label("耗用单号")]
        public static readonly Property<string> CostNoProperty = P<WoCostItem>.Register(e => e.CostNo);

        /// <summary>
        /// 耗用单号
        /// </summary>
        public string CostNo
        {
            get { return this.GetProperty(CostNoProperty); }
            set { this.SetProperty(CostNoProperty, value); }
        }
        #endregion

        #region 单据类型 RecordType
        /// <summary>
        /// 单据类型
        /// </summary>
        [Label("单据类型")]
        public static readonly Property<WoCostItemType> RecordTypeProperty = P<WoCostItem>.Register(e => e.RecordType);

        /// <summary>
        /// 单据类型
        /// </summary>
        public WoCostItemType RecordType
        {
            get { return this.GetProperty(RecordTypeProperty); }
            set { this.SetProperty(RecordTypeProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<WoCostItemState> StateProperty = P<WoCostItem>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public WoCostItemState State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<WoCostItem>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<WoCostItem>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 耗用物料 Item
        /// <summary>
        /// 耗用物料Id
        /// </summary>
        [Label("耗用物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<WoCostItem>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 耗用物料Id
        /// </summary>
        public double ItemId
        {
            get { return (double)this.GetRefId(ItemIdProperty); }
            set { this.SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 耗用物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<WoCostItem>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 耗用物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 物料拓展属性 ItemExtProp
        /// <summary>
        /// 物料拓展属性
        /// </summary>
        [Label("物料拓展属性")]
        public static readonly Property<string> ItemExtPropProperty = P<WoCostItem>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 物料拓展属性
        /// </summary>
        public string ItemExtProp
        {
            get { return this.GetProperty(ItemExtPropProperty); }
            set { this.SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 物料拓展属性名称 ItemExtPropName
        /// <summary>
        /// 物料拓展属性名称
        /// </summary>
        [Label("物料拓展属性名称")]
        public static readonly Property<string> ItemExtPropNameProperty = P<WoCostItem>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 物料拓展属性名称
        /// </summary>
        public string ItemExtPropName
        {
            get { return this.GetProperty(ItemExtPropNameProperty); }
            set { this.SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 扣料记录消耗方式 ConsumeType
        /// <summary>
        /// 扣料记录消耗方式
        /// </summary>
        [Label("扣料记录消耗方式")]
        public static readonly Property<ConsumeMode> ConsumeTypeProperty = P<WoCostItem>.Register(e => e.ConsumeType);

        /// <summary>
        /// 扣料记录消耗方式
        /// </summary>
        public ConsumeMode ConsumeType
        {
            get { return this.GetProperty(ConsumeTypeProperty); }
            set { this.SetProperty(ConsumeTypeProperty, value); }
        }
        #endregion

        #region 物料标签 CostItemLabel
        /// <summary>
        /// 物料标签Id
        /// </summary>
        [Label("物料标签")]
        public static readonly IRefIdProperty CostItemLabelIdProperty =
            P<WoCostItem>.RegisterRefId(e => e.CostItemLabelId, ReferenceType.Normal);

        /// <summary>
        /// 物料标签Id
        /// </summary>
        public double? CostItemLabelId
        {
            get { return (double?)this.GetRefNullableId(CostItemLabelIdProperty); }
            set { this.SetRefNullableId(CostItemLabelIdProperty, value); }
        }

        /// <summary>
        /// 物料标签
        /// </summary>
        public static readonly RefEntityProperty<ItemLabel> CostItemLabelProperty =
            P<WoCostItem>.RegisterRef(e => e.CostItemLabel, CostItemLabelIdProperty);

        /// <summary>
        /// 物料标签
        /// </summary>
        public ItemLabel CostItemLabel
        {
            get { return this.GetRefEntity(CostItemLabelProperty); }
            set { this.SetRefEntity(CostItemLabelProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<WoCostItem>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<WoCostItem>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Process> ProcessProperty = P<WoCostItem>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 工位 Station
        /// <summary>
        /// 工位Id
        /// </summary>
        [Label("工位")]
        public static readonly IRefIdProperty StationIdProperty =
            P<WoCostItem>.RegisterRefId(e => e.StationId, ReferenceType.Normal);

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
            P<WoCostItem>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 工位
        /// </summary>
        public Station Station
        {
            get { return this.GetRefEntity(StationProperty); }
            set { this.SetRefEntity(StationProperty, value); }
        }
        #endregion

        #region 失败信息 FailMsg
        /// <summary>
        /// 失败信息
        /// </summary>
        [MaxLength(4000)]
        [Label("失败信息")]
        public static readonly Property<string> FailMsgProperty = P<WoCostItem>.Register(e => e.FailMsg);

        /// <summary>
        /// 失败信息
        /// </summary>
        public string FailMsg
        {
            get { return this.GetProperty(FailMsgProperty); }
            set { this.SetProperty(FailMsgProperty, value); }
        }
        #endregion

        #region 产品条码 BarCode
        /// <summary>
        /// 产品条码
        /// </summary>
        [Label("产品条码")]
        public static readonly Property<string> BarCodeProperty = P<WoCostItem>.Register(e => e.BarCode);

        /// <summary>
        /// 产品条码
        /// </summary>
        public string BarCode
        {
            get { return this.GetProperty(BarCodeProperty); }
            set { this.SetProperty(BarCodeProperty, value); }
        }
        #endregion

        #region 产品批次 BatchNo
        /// <summary>
        /// 产品批次
        /// </summary>
        [Label("产品批次")]
        public static readonly Property<string> BatchNoProperty = P<WoCostItem>.Register(e => e.BatchNo);

        /// <summary>
        /// 产品批次
        /// </summary>
        public string BatchNo
        {
            get { return this.GetProperty(BatchNoProperty); }
            set { this.SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 项目号 ProjectNo
        /// <summary>
        /// 项目号
        /// </summary>
        [Label("项目号")]
        public static readonly Property<string> ProjectNoProperty = P<WoCostItem>.Register(e => e.ProjectNo);

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectNo
        {
            get { return this.GetProperty(ProjectNoProperty); }
            set { this.SetProperty(ProjectNoProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty FactoryIdProperty =
            P<WoCostItem>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double FactoryId
        {
            get { return (double)this.GetRefId(FactoryIdProperty); }
            set { this.SetRefId(FactoryIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> FactoryProperty =
            P<WoCostItem>.RegisterRef(e => e.Factory, FactoryIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Factory
        {
            get { return this.GetRefEntity(FactoryProperty); }
            set { this.SetRefEntity(FactoryProperty, value); }
        }
        #endregion

        #region 资源 WipResource
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty WipResourceIdProperty =
            P<WoCostItem>.RegisterRefId(e => e.WipResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double? WipResourceId
        {
            get { return (double?)this.GetRefNullableId(WipResourceIdProperty); }
            set { this.SetRefNullableId(WipResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> WipResourceProperty =
            P<WoCostItem>.RegisterRef(e => e.WipResource, WipResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource WipResource
        {
            get { return this.GetRefEntity(WipResourceProperty); }
            set { this.SetRefEntity(WipResourceProperty, value); }
        }
        #endregion

        #region 提交人 Submiter
        /// <summary>
        /// 提交人Id
        /// </summary>
        [Label("提交人")]
        public static readonly IRefIdProperty SubmiterIdProperty =
            P<WoCostItem>.RegisterRefId(e => e.SubmiterId, ReferenceType.Normal);

        /// <summary>
        /// 提交人Id
        /// </summary>
        public double? SubmiterId
        {
            get { return (double?)this.GetRefNullableId(SubmiterIdProperty); }
            set { this.SetRefNullableId(SubmiterIdProperty, value); }
        }

        /// <summary>
        /// 提交人
        /// </summary>
        public static readonly RefEntityProperty<Employee> SubmiterProperty =
            P<WoCostItem>.RegisterRef(e => e.Submiter, SubmiterIdProperty);

        /// <summary>
        /// 提交人
        /// </summary>
        public Employee Submiter
        {
            get { return this.GetRefEntity(SubmiterProperty); }
            set { this.SetRefEntity(SubmiterProperty, value); }
        }
        #endregion

        #region 提交时间 SubmitTime
        /// <summary>
        /// 提交时间
        /// </summary>
        [Label("提交时间")]
        public static readonly Property<DateTime?> SubmitTimeProperty = P<WoCostItem>.Register(e => e.SubmitTime);

        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime? SubmitTime
        {
            get { return this.GetProperty(SubmitTimeProperty); }
            set { this.SetProperty(SubmitTimeProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty =
            P<WoCostItem>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double? WarehouseId
        {
            get { return (double?)this.GetRefNullableId(WarehouseIdProperty); }
            set { this.SetRefNullableId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty =
            P<WoCostItem>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return this.GetRefEntity(WarehouseProperty); }
            set { this.SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 库位 Storage
        /// <summary>
        /// 库位Id
        /// </summary>
        [Label("库位")]
        public static readonly IRefIdProperty StorageIdProperty =
            P<WoCostItem>.RegisterRefId(e => e.StorageId, ReferenceType.Normal);

        /// <summary>
        /// 库位Id
        /// </summary>
        public double? StorageId
        {
            get { return (double?)this.GetRefNullableId(StorageIdProperty); }
            set { this.SetRefNullableId(StorageIdProperty, value); }
        }

        /// <summary>
        /// 库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> StorageProperty =
            P<WoCostItem>.RegisterRef(e => e.Storage, StorageIdProperty);

        /// <summary>
        /// 库位
        /// </summary>
        public StorageLocation Storage
        {
            get { return this.GetRefEntity(StorageProperty); }
            set { this.SetRefEntity(StorageProperty, value); }
        }
        #endregion

        #region 替代料 AlternativeItem
        /// <summary>
        /// 替代料Id
        /// </summary>
        [Label("替代料")]
        public static readonly IRefIdProperty AlternativeItemIdProperty =
            P<WoCostItem>.RegisterRefId(e => e.AlternativeItemId, ReferenceType.Normal);

        /// <summary>
        /// 替代料Id
        /// </summary>
        public double? AlternativeItemId
        {
            get { return (double?)this.GetRefNullableId(AlternativeItemIdProperty); }
            set { this.SetRefNullableId(AlternativeItemIdProperty, value); }
        }

        /// <summary>
        /// 替代料
        /// </summary>
        public static readonly RefEntityProperty<Item> AlternativeItemProperty =
            P<WoCostItem>.RegisterRef(e => e.AlternativeItem, AlternativeItemIdProperty);

        /// <summary>
        /// 替代料
        /// </summary>
        public Item AlternativeItem
        {
            get { return this.GetRefEntity(AlternativeItemProperty); }
            set { this.SetRefEntity(AlternativeItemProperty, value); }
        }
        #endregion

        #region 工单BOM WorkOrderBom
        /// <summary>
        /// 工单BOMId
        /// </summary>
        [Label("工单BOM")]
        public static readonly IRefIdProperty WorkOrderBomIdProperty =
            P<WoCostItem>.RegisterRefId(e => e.WorkOrderBomId, ReferenceType.Normal);

        /// <summary>
        /// 工单BOMId
        /// </summary>
        public double? WorkOrderBomId
        {
            get { return (double?)this.GetRefNullableId(WorkOrderBomIdProperty); }
            set { this.SetRefNullableId(WorkOrderBomIdProperty, value); }
        }

        /// <summary>
        /// 工单BOM
        /// </summary>
        public static readonly RefEntityProperty<WorkOrderBom> WorkOrderBomProperty =
            P<WoCostItem>.RegisterRef(e => e.WorkOrderBom, WorkOrderBomIdProperty);

        /// <summary>
        /// 工单BOM
        /// </summary>
        public WorkOrderBom WorkOrderBom
        {
            get { return this.GetRefEntity(WorkOrderBomProperty); }
            set { this.SetRefEntity(WorkOrderBomProperty, value); }
        }
        #endregion

        #region 替代组合分组 AlterGroup
        /// <summary>
        /// 替代组合分组
        /// </summary>
        [Label("替代组合分组")]
        public static readonly Property<string> AlterGroupProperty = P<WoCostItem>.Register(e => e.AlterGroup);

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
        public static readonly Property<string> AlterProperty = P<WoCostItem>.Register(e => e.Alter);

        /// <summary>
        /// 替代组
        /// </summary>
        public string Alter
        {
            get { return this.GetProperty(AlterProperty); }
            set { this.SetProperty(AlterProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<WoCostItem>.RegisterView(e => e.ProductName, p => p.WorkOrder.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }

        #endregion

        #region 耗用物料名称 CostItemName
        /// <summary>
        /// 耗用物料名称
        /// </summary>
        [Label("耗用物料名称")]
        public static readonly Property<string> CostItemNameProperty = P<WoCostItem>.RegisterView(e => e.CostItemName, p => p.Item.Name);

        /// <summary>
        /// 耗用物料名称
        /// </summary>
        public string CostItemName
        {
            get { return this.GetProperty(CostItemNameProperty); }
        }

        #endregion


        #region 批次号 Lot
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> LotProperty = P<WoCostItem>.RegisterView(e => e.Lot, p => p.CostItemLabel.Lot);

        /// <summary>
        /// 批次号
        /// </summary>
        public string Lot
        {
            get { return this.GetProperty(LotProperty); }
        }
        #endregion

        #region 工厂名称 FactoryName
        /// <summary>
        /// 工厂名称
        /// </summary>
        [Label("工厂名称")]
        public static readonly Property<string> FactoryNameProperty = P<WoCostItem>.RegisterView(e => e.FactoryName, p => p.Factory.Name);

        /// <summary>
        /// 工厂名称
        /// </summary>
        public string FactoryName
        {
            get { return this.GetProperty(FactoryNameProperty); }
        }
        #endregion

        #region 资源名称 WipResourceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源名称")]
        public static readonly Property<string> WipResourceNameProperty = P<WoCostItem>.RegisterView(e => e.WipResourceName, p => p.WipResource.Name);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string WipResourceName
        {
            get { return this.GetProperty(WipResourceNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 扣料记录 实体配置
    /// </summary>
    internal class WoCostItemConfig : EntityConfig<WoCostItem>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_DEDUCT_ITEM").MapAllProperties();
            Meta.Property(WoCostItem.FailMsgProperty).ColumnMeta.HasLength(4000);            
            Meta.EnablePhantoms();
        }
    }
}