using SIE.Domain;
using SIE.Items;
using SIE.MES.SingleLabels;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.ShiftTypes;
using SIE.Resources.WipResources;
using SIE.Tech.Stations;
using System;

namespace SIE.MES.LoadItems
{
    /// <summary>
    /// 上料
    /// </summary>
    [RootEntity, Serializable]
    [Label("上料")]
    public partial class LoadItem : DataEntity
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public LoadItem()
        {
            this.LoadQty = 0m;
            this.Qty = 0m;
            this.UnloadQty = 0m;
            this.NgQty = 0m;

            //默认挪料上料为否
            this.IsMoveItem = YesNo.No;
        }
        #endregion


        #region 来源条码  SourceCode
        /// <summary>
        /// 来源条码 
        /// </summary>
        [Label("来源条码")]
        public static readonly Property<string> SourceCodeProperty = P<LoadItem>.Register(e => e.SourceCode);

        /// <summary>
        /// 来源条码 
        /// </summary>
        public string SourceCode
        {
            get { return GetProperty(SourceCodeProperty); }
            set { SetProperty(SourceCodeProperty, value); }
        }
        #endregion

        #region 来源ID SourceId
        /// <summary>
        /// 来源ID
        /// </summary>
        [Label("来源ID")]
        public static readonly Property<double> SourceIdProperty = P<LoadItem>.Register(e => e.SourceId);

        /// <summary>
        /// 来源ID
        /// </summary>
        public double SourceId
        {
            get { return GetProperty(SourceIdProperty); }
            set { SetProperty(SourceIdProperty, value); }
        }
        #endregion

        #region 上料数量 Qty
        /// <summary>
        /// 上料数量
        /// </summary>
        [Required]
        [MinValue(0)]
        [Label("上料数量")]
        public static readonly Property<decimal> LoadQtyProperty = P<LoadItem>.Register(e => e.LoadQty);

        /// <summary>
        /// 上料数量
        /// </summary>
        public decimal LoadQty
        {
            get { return GetProperty(LoadQtyProperty); }
            set { SetProperty(LoadQtyProperty, value); }
        }
        #endregion

        #region 可用数量 Qty
        /// <summary>
        /// 可用数量
        /// </summary>
        [Required]
        ////[MinValue(0)]
        [Label("可用数量")]
        public static readonly Property<decimal> QtyProperty = P<LoadItem>.Register(e => e.Qty);

        /// <summary>
        /// 可用数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 下料数量 UnloadQty
        /// <summary>
        /// 下料数量
        /// </summary>
        [Required]
        [MinValue(0)]
        [Label("下料数量")]
        public static readonly Property<decimal> UnloadQtyProperty = P<LoadItem>.Register(e => e.UnloadQty);

        /// <summary>
        /// 下料数量
        /// </summary>
        public decimal UnloadQty
        {
            get { return GetProperty(UnloadQtyProperty); }
            set { SetProperty(UnloadQtyProperty, value); }
        }
        #endregion

        #region 不良数量 NgQty
        /// <summary>
        /// 不良数量
        /// </summary>
        [Required]
        [MinValue(0)]
        [Label("不良数量")]
        public static readonly Property<decimal> NgQtyProperty = P<LoadItem>.Register(e => e.NgQty);

        /// <summary>
        /// 不良数量
        /// </summary>
        public decimal NgQty
        {
            get { return GetProperty(NgQtyProperty); }
            set { SetProperty(NgQtyProperty, value); }
        }
        #endregion

        #region 班次 Shift
        /// <summary>
        /// 班次Id
        /// </summary>
        [Required]
        [Label("班次Id")]
        public static readonly IRefIdProperty ShiftIdProperty = P<LoadItem>.RegisterRefId(e => e.ShiftId, ReferenceType.Normal);

        /// <summary>
        /// 班次Id
        /// </summary>
        public double ShiftId
        {
            get { return (double)GetRefId(ShiftIdProperty); }
            set { SetRefId(ShiftIdProperty, value); }
        }

        /// <summary>
        /// 班次
        /// </summary>
        [Required]
        [Label("班次")]
        public static readonly RefEntityProperty<Shift> ShiftProperty = P<LoadItem>.RegisterRef(e => e.Shift, ShiftIdProperty);

        /// <summary>
        /// 班次
        /// </summary>
        public Shift Shift
        {
            get { return GetRefEntity(ShiftProperty); }
            set { SetRefEntity(ShiftProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源
        /// </summary>
        [Required]
        [Label("资源Id")]
        public static readonly IRefIdProperty ResourceIdProperty = P<LoadItem>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源
        /// </summary>
        public double ResourceId
        {
            get { return (double)GetRefId(ResourceIdProperty); }
            set { SetRefId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        [Label("资源")]
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<LoadItem>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 工位 Station
        /// <summary>
        /// 工位Id
        /// </summary>
        [Label("工位Id")]
        public static readonly IRefIdProperty StationIdProperty =
            P<LoadItem>.RegisterRefId(e => e.StationId, ReferenceType.Normal);

        /// <summary>
        /// 工位Id
        /// </summary>
        public double StationId
        {
            get { return (double)this.GetRefId(StationIdProperty); }
            set { this.SetRefId(StationIdProperty, value); }
        }

        /// <summary>
        /// 工位
        /// </summary>
        [Label("工位")]
        public static readonly RefEntityProperty<Station> StationProperty =
            P<LoadItem>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 工位
        /// </summary>
        public Station Station
        {
            get { return this.GetRefEntity(StationProperty); }
            set { this.SetRefEntity(StationProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<LoadItem>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Item> ItemProperty = P<LoadItem>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion 

        #region 扩展属性 ItemExtProp
        /// <summary>
        /// 扩展属性
        /// </summary>
        [Label("扩展属性")]
        public static readonly Property<string> ItemExtPropProperty = P<LoadItem>.Register(e => e.ItemExtProp);

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
        public static readonly Property<string> ItemExtPropNameProperty = P<LoadItem>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 扩展属性值
        /// </summary>
        public string ItemExtPropName
        {
            get { return GetProperty(ItemExtPropNameProperty); }
            set { SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 项目号 ProjectNo
        /// <summary>
        /// 项目号
        /// </summary>
        [Label("项目号")]
        public static readonly Property<string> ProjectNoProperty = P<LoadItem>.Register(e => e.ProjectNo);

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectNo
        {
            get { return this.GetProperty(ProjectNoProperty); }
            set { this.SetProperty(ProjectNoProperty, value); }
        }
        #endregion

        #region 上料来源类型 SourceType
        /// <summary>
        /// 上料来源类型
        /// </summary>
        [Label("上料来源类型")]
        public static readonly Property<LoadItemSourceType> SourceTypeProperty = P<LoadItem>.Register(e => e.SourceType);

        /// <summary>
        /// 上料来源类型
        /// </summary>
        public LoadItemSourceType SourceType
        {
            get { return GetProperty(SourceTypeProperty); }
            set { SetProperty(SourceTypeProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<LoadItem>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId
        {
            get { return (double?)GetRefNullableId(WorkOrderIdProperty); }
            set { SetRefNullableId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<LoadItem>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 挪料上料 IsMoveItem
        /// <summary>
        /// 挪料上料
        /// </summary>
        [Label("挪料上料")]
        public static readonly Property<YesNo> IsMoveItemProperty = P<LoadItem>.Register(e => e.IsMoveItem);

        /// <summary>
        /// 挪料上料
        /// </summary>
        public YesNo IsMoveItem
        {
            get { return this.GetProperty(IsMoveItemProperty); }
            set { this.SetProperty(IsMoveItemProperty, value); }
        }
        #endregion

        #region 替代组合分组 AlterGroup
        /// <summary>
        /// 替代组合分组
        /// </summary>
        [Label("替代组合分组")]
        public static readonly Property<string> AlterGroupProperty = P<LoadItem>.Register(e => e.AlterGroup);

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
        public static readonly Property<string> AlterProperty = P<LoadItem>.Register(e => e.Alter);

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
        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("属性名")]
        public static readonly Property<string> ItemCodeProperty = P<LoadItem>.RegisterView(e => e.ItemCode, p => p.Item.Code);

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
        public static readonly Property<string> ItemNameProperty = P<LoadItem>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 物料消耗属性 ItemConsumeMode
        /// <summary>
        /// 物料消耗属性
        /// </summary>
        [Label("物料消耗属性")]
        public static readonly Property<ConsumeMode> ItemConsumeModeProperty = P<LoadItem>.RegisterView(e => e.ItemConsumeMode, p => p.Item.ConsumeMode);

        /// <summary>
        /// 物料消耗属性
        /// </summary>
        public ConsumeMode ItemConsumeMode
        {
            get { return this.GetProperty(ItemConsumeModeProperty); }
        }
        #endregion

        #region 工位名称 StationName
        /// <summary>
        /// 工位名称
        /// </summary>
        [Label("工位名称")]
        public static readonly Property<string> StationNameProperty = P<LoadItem>.RegisterView(e => e.StationName, p => p.Station.Name);

        /// <summary>
        /// 工位名称
        /// </summary>
        public string StationName
        {
            get { return this.GetProperty(StationNameProperty); }
        }
        #endregion


        #region 单位精度 UnitPrecision
        /// <summary>
        /// 单位精度
        /// </summary>
        [Label("单位精度")]
        public static readonly Property<int?> UnitPrecisionProperty = P<LoadItem>.RegisterView(e => e.UnitPrecision, p => p.Item.Unit.Precision);

        /// <summary>
        /// 单位精度
        /// </summary>
        public int? UnitPrecision
        {
            get { return this.GetProperty(UnitPrecisionProperty); }
        }
        #endregion


        #endregion
    }

    /// <summary>
    /// 上料 实体配置
    /// </summary>
    internal class LoadItemConfig : EntityConfig<LoadItem>
    {
        /// <summary>
        /// 配置数据库映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_LOAD_ITEM").MapAllProperties();
            Meta.IndexGroupOnProperties(LoadItem.ResourceIdProperty, LoadItem.StationIdProperty);
            Meta.IndexGroupOnProperties(LoadItem.WorkOrderIdProperty, LoadItem.ItemIdProperty);
            Meta.DisablePhantoms();
            Meta.EnableSort();
        }
    }
}