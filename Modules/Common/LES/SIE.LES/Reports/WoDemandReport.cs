using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.LES.Reports
{
    /// <summary>
    /// 工单需求汇总报表
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(WoDemandReportCriteria))]
    public class WoDemandReport: DataEntity
    {
        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty =
            P<WoDemandReport>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
            P<WoDemandReport>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return this.GetRefEntity(WarehouseProperty); }
            set { this.SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<WoDemandReport>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId
        {
            get { return (double?)this.GetRefNullableId(WorkOrderIdProperty); }
            set { this.SetRefNullableId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty =
            P<WoDemandReport>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkShopIdProperty =
            P<WoDemandReport>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkShopId
        {
            get { return (double?)this.GetRefNullableId(WorkShopIdProperty); }
            set { this.SetRefNullableId(WorkShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty =
            P<WoDemandReport>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return this.GetRefEntity(WorkShopProperty); }
            set { this.SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty =
            P<WoDemandReport>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

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
            P<WoDemandReport>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料编码")]
        public static readonly IRefIdProperty ItemIdProperty = P<WoDemandReport>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Item> ItemProperty = P<WoDemandReport>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 物料扩展属性名称 ItemExtPropName
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性名称")]
        public static readonly Property<string> ItemExtPropNameProperty = P<WoDemandReport>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName
        {
            get { return this.GetProperty(ItemExtPropNameProperty); }
            set { this.SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 物料扩展属性 ItemExtProp
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        public static readonly Property<string> ItemExtPropProperty = P<WoDemandReport>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get { return this.GetProperty(ItemExtPropProperty); }
            set { this.SetProperty(ItemExtPropProperty, value); }
        }
        #endregion        

        #region 接收数 ReceivedQty
        /// <summary>
        /// 接收数
        /// </summary>
        [Label("接收数")]
        public static readonly Property<decimal> ReceivedQtyProperty = P<WoDemandReport>.Register(e => e.ReceivedQty);

        /// <summary>
        /// 接收数量
        /// </summary>
        public decimal ReceivedQty
        {
            get { return this.GetProperty(ReceivedQtyProperty); }
            set { this.SetProperty(ReceivedQtyProperty, value); }
        }
        #endregion

        #region 上料数 FeedQty
        /// <summary>
        /// 上料数
        /// </summary>
        [Label("上料数")]
        public static readonly Property<decimal> FeedQtyProperty = P<WoDemandReport>.Register(e => e.FeedQty);

        /// <summary>
        /// 上料数
        /// </summary>
        public decimal FeedQty
        {
            get { return this.GetProperty(FeedQtyProperty); }
            set { this.SetProperty(FeedQtyProperty, value); }
        }
        #endregion

        //#region 可用数 AvailableQty
        ///// <summary>
        ///// 可用数
        ///// </summary>
        //[Label("可用数")]
        //public static readonly Property<decimal> AvailableQtyProperty = P<WoDemandReport>.Register(e => e.AvailableQty);

        ///// <summary>
        ///// 可用数
        ///// </summary>
        //public decimal AvailableQty
        //{
        //    get { return this.GetProperty(AvailableQtyProperty); }
        //    set { this.SetProperty(AvailableQtyProperty, value); }
        //}
        //#endregion

        #region 可用数 AvailableQty
        /// <summary>
        /// 可用数
        /// </summary>
        [Label("可用数")]
        public static readonly Property<decimal> AvailableQtyProperty = P<WoDemandReport>.RegisterReadOnly(
            e => e.AvailableQty, e => e.GetAvailableQty(), ReceivedQtyProperty);
        /// <summary>
        /// 可用数 (可用数=接收数-上料数-不良数-挪出数+挪入数-正常退料在途数-不良退料在途数-正常退料数-不良退料数)
        /// </summary>

        public decimal AvailableQty
        {
            get { return this.GetProperty(AvailableQtyProperty); }
        }
        private decimal GetAvailableQty()
        {
            return ReceivedQty + MovedInQty - FeedQty - NgQty - MovedOutQty - ReturnQtyInTransit - NgReturnQtyInTransit - ReturnQty - NgReturnQty;
        }
        #endregion

        #region 不良数 NgQty
        /// <summary>
        /// 不良数
        /// </summary>
        [Label("不良数")]
        public static readonly Property<decimal> NgQtyProperty = P<WoDemandReport>.Register(e => e.NgQty);

        /// <summary>
        /// 不良数
        /// </summary>
        public decimal NgQty
        {
            get { return this.GetProperty(NgQtyProperty); }
            set { this.SetProperty(NgQtyProperty, value); }
        }
        #endregion

        #region 挪出数 MovedOutQty
        /// <summary>
        /// 挪出数
        /// </summary>
        [Label("挪出数")]
        public static readonly Property<decimal> MovedOutQtyProperty = P<WoDemandReport>.Register(e => e.MovedOutQty);

        /// <summary>
        /// 挪出数
        /// </summary>
        public decimal MovedOutQty
        {
            get { return this.GetProperty(MovedOutQtyProperty); }
            set { this.SetProperty(MovedOutQtyProperty, value); }
        }
        #endregion

        #region 挪入数 MovedInQty
        /// <summary>
        /// 挪入数
        /// </summary>
        [Label("挪入数")]
        public static readonly Property<decimal> MovedInQtyProperty = P<WoDemandReport>.Register(e => e.MovedInQty);

        /// <summary>
        /// 挪入数
        /// </summary>
        public decimal MovedInQty
        {
            get { return this.GetProperty(MovedInQtyProperty); }
            set { this.SetProperty(MovedInQtyProperty, value); }
        }
        #endregion

        #region 正常退料在途数 ReturnQtyInTransit
        /// <summary>
        /// 正常退料在途数
        /// </summary>
        [Label("正常退料在途数")]
        public static readonly Property<decimal> ReturnQtyInTransitProperty = P<WoDemandReport>.Register(e => e.ReturnQtyInTransit);

        /// <summary>
        /// 正常退料在途数
        /// </summary>
        public decimal ReturnQtyInTransit
        {
            get { return this.GetProperty(ReturnQtyInTransitProperty); }
            set { this.SetProperty(ReturnQtyInTransitProperty, value); }
        }
        #endregion

        #region 不良退料在途数 NgReturnQtyInTransit
        /// <summary>
        /// 不良退料在途数
        /// </summary>
        [Label("不良退料在途数")]
        public static readonly Property<decimal> NgReturnQtyInTransitProperty = P<WoDemandReport>.Register(e => e.NgReturnQtyInTransit);

        /// <summary>
        /// 不良退料在途数
        /// </summary>
        public decimal NgReturnQtyInTransit
        {
            get { return this.GetProperty(NgReturnQtyInTransitProperty); }
            set { this.SetProperty(NgReturnQtyInTransitProperty, value); }
        }
        #endregion

        #region 正常退料数 ReturnQty
        /// <summary>
        /// 正常退料数
        /// </summary>
        [Label("正常退料数")]
        public static readonly Property<decimal> ReturnQtyProperty = P<WoDemandReport>.Register(e => e.ReturnQty);

        /// <summary>
        /// ReturnQty
        /// </summary>
        public decimal ReturnQty
        {
            get { return this.GetProperty(ReturnQtyProperty); }
            set { this.SetProperty(ReturnQtyProperty, value); }
        }
        #endregion

        #region 不良退料数 NgReturnQty
        /// <summary>
        /// 不良退料数
        /// </summary>
        [Label("不良退料数")]
        public static readonly Property<decimal> NgReturnQtyProperty = P<WoDemandReport>.Register(e => e.NgReturnQty);

        /// <summary>
        /// 不良退料数
        /// </summary>
        public decimal NgReturnQty
        {
            get { return this.GetProperty(NgReturnQtyProperty); }
            set { this.SetProperty(NgReturnQtyProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 仓库编码 WarehouseCode
        /// <summary>
        /// 仓库编码
        /// </summary>
        [Label("仓库编码")]
        public static readonly Property<string> WarehouseCodeProperty = P<WoDemandReport>.RegisterView(e => e.WarehouseCode, p => p.Warehouse.Code);

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode
        {
            get { return this.GetProperty(WarehouseCodeProperty); }
        }
        #endregion

        #region 仓库名称 WarehouseName
        /// <summary>
        /// 仓库名称
        /// </summary>
        [Label("仓库名称")]
        public static readonly Property<string> WarehouseNameProperty = P<WoDemandReport>.RegisterView(e => e.WarehouseName, p => p.Warehouse.Name);

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName
        {
            get { return this.GetProperty(WarehouseNameProperty); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<WoDemandReport>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 物料名称
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
        public static readonly Property<string> ItemNameProperty = P<WoDemandReport>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 物料类型 ItemType
        /// <summary>
        /// 物料类型
        /// </summary>
        [Label("物料类型")]
        public static readonly Property<ItemType> ItemTypeProperty = P<WoDemandReport>.RegisterView(e => e.ItemType, p => p.Item.Type);

        /// <summary>
        /// 物料类型
        /// </summary>
        public ItemType ItemType
        {
            get { return this.GetProperty(ItemTypeProperty); }
        }

        #endregion

        #region 单位 ItemUnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> ItemUnitNameProperty = P<WoDemandReport>.RegisterView(e => e.ItemUnitName, p => p.Item.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string ItemUnitName
        {
            get { return this.GetProperty(ItemUnitNameProperty); }
        }

        #endregion

        #region 工单 WoNo
        /// <summary>
        /// 工单
        /// </summary>
        [Label("工单")]
        public static readonly Property<string> WoNoProperty = P<WoDemandReport>.RegisterView(e => e.WoNo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单
        /// </summary>
        public string WoNo
        {
            get { return this.GetProperty(WoNoProperty); }
        }
        #endregion

        #region 车间 WorkShopName
        /// <summary>
        /// 车间
        /// </summary>
        [Label("车间")]
        public static readonly Property<string> WorkShopNameProperty = P<WoDemandReport>.RegisterView(e => e.WorkShopName, p => p.WorkShop.Name);

        /// <summary>
        /// 车间
        /// </summary>
        public string WorkShopName
        {
            get { return this.GetProperty(WorkShopNameProperty); }
        }
        #endregion

        #region 资源 ResourceName
        /// <summary>
        /// 资源
        /// </summary>
        [Label("资源")]
        public static readonly Property<string> ResourceNameProperty = P<WoDemandReport>.RegisterView(e => e.ResourceName, p => p.Resource.Name);

        /// <summary>
        /// 资源
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
        }
        #endregion

        #endregion
    }


    /// <summary>
    ///  实体配置
    /// </summary>
    internal class WoDemandReportConfig : EntityConfig<WoDemandReport>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("LES_WO_DEMAND_REPORT").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
