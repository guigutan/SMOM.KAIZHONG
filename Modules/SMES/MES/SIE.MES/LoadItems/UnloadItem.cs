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
    /// 下料
    /// </summary>
    [RootEntity, Serializable]
    [Label("下料")]
    public partial class UnloadItem : DataEntity
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public UnloadItem()
        {
            this.IsNg = false;
        }
        #endregion

        #region 来源条码 SourceCode
        /// <summary>
        /// 来源条码
        /// </summary>
        [Label("来源条码")]
        public static readonly Property<string> SourceCodeProperty = P<UnloadItem>.Register(e => e.SourceCode);

        /// <summary>
        /// 来源条码
        /// </summary>
        public string SourceCode
        {
            get { return this.GetProperty(SourceCodeProperty); }
            set { this.SetProperty(SourceCodeProperty, value); }
        }
        #endregion

        #region 来源ID SourceId
        /// <summary>
        /// 来源ID
        /// </summary>
        [Label("来源ID")]
        public static readonly Property<double> SourceIdProperty = P<UnloadItem>.Register(e => e.SourceId);

        /// <summary>
        /// 来源ID
        /// </summary>
        public double SourceId
        {
            get { return this.GetProperty(SourceIdProperty); }
            set { this.SetProperty(SourceIdProperty, value); }
        }
        #endregion

        #region 来源类型 SourceType
        /// <summary>
        /// 来源类型
        /// </summary>
        [Label("来源类型")]
        public static readonly Property<LoadItemSourceType> SourceTypeProperty = P<UnloadItem>.Register(e => e.SourceType);

        /// <summary>
        /// 来源类型
        /// </summary>
        public LoadItemSourceType SourceType
        {
            get { return this.GetProperty(SourceTypeProperty); }
            set { this.SetProperty(SourceTypeProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<UnloadItem>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return (double)this.GetRefId(ItemIdProperty); }
            set { this.SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<UnloadItem>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion 

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<UnloadItem>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

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
            P<UnloadItem>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 上料数量 LoadItemQty
        /// <summary>
        /// 上料数量
        /// </summary>
        [Label("上料数量")]
        public static readonly Property<decimal> LoadItemQtyProperty = P<UnloadItem>.Register(e => e.LoadItemQty);

        /// <summary>
        /// 上料数量
        /// </summary>
        public decimal LoadItemQty
        {
            get { return this.GetProperty(LoadItemQtyProperty); }
            set { this.SetProperty(LoadItemQtyProperty, value); }
        }
        #endregion

        #region 下料数量 Qty
        /// <summary>
        /// 下料数量
        /// </summary>
        [Required]
        [Label("下料数量")]
        public static readonly Property<decimal> QtyProperty = P<UnloadItem>.Register(e => e.Qty);

        /// <summary>
        /// 下料数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 剩余数量 RemainderQty
        /// <summary>
        /// 剩余数量
        /// </summary>
        [Label("剩余数量")]
        public static readonly Property<decimal> RemainderQtyProperty = P<UnloadItem>.Register(e => e.RemainderQty);

        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal RemainderQty
        {
            get { return this.GetProperty(RemainderQtyProperty); }
            set { this.SetProperty(RemainderQtyProperty, value); }
        }
        #endregion

        #region 班次 Shift
        /// <summary>
        /// 班次Id
        /// </summary>
        [Label("班次Id")]
        public static readonly IRefIdProperty ShiftIdProperty = P<UnloadItem>.RegisterRefId(e => e.ShiftId, ReferenceType.Normal);

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
        [Label("班次")]
        public static readonly RefEntityProperty<Shift> ShiftProperty = P<UnloadItem>.RegisterRef(e => e.Shift, ShiftIdProperty);

        /// <summary>
        /// 班次
        /// </summary>
        public Shift Shift
        {
            get { return GetRefEntity(ShiftProperty); }
            set { SetRefEntity(ShiftProperty, value); }
        }
        #endregion

        #region 不良列表 DefectList
        /// <summary>
        /// 不良列表
        /// </summary>
        [Label("不良列表")]
        public static readonly ListProperty<EntityList<UnloadItemDefect>> DefectListProperty = P<UnloadItem>.RegisterList(e => e.DefectList);

        /// <summary>
        /// 不良列表
        /// </summary>
        public EntityList<UnloadItemDefect> DefectList
        {
            get { return this.GetLazyList(DefectListProperty); }
        }
        #endregion

        #region 下料接收状态 State
        /// <summary>
        /// 下料接收状态
        /// </summary>
        [Label("下料接收状态")]
        public static readonly Property<UnloadState> StateProperty = P<UnloadItem>.Register(e => e.State);

        /// <summary>
        /// 下料接收状态
        /// </summary>
        public UnloadState State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 产线 Resource
        /// <summary>
        /// 产线Id
        /// </summary>
        [Label("产线Id")]
        public static readonly IRefIdProperty ResourceIdProperty = P<UnloadItem>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 产线Id
        /// </summary>
        public double ResourceId
        {
            get { return (double)GetRefId(ResourceIdProperty); }
            set { SetRefId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 产线
        /// </summary>
        [Label("下料")]
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<UnloadItem>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 是否不良下料 IsNg
        /// <summary>
        /// 是否不良下料
        /// </summary>
        [Label("是否不良下料")]
        public static readonly Property<bool> IsNgProperty = P<UnloadItem>.Register(e => e.IsNg);

        /// <summary>
        /// 是否不良下料
        /// </summary>
        public bool IsNg
        {
            get { return this.GetProperty(IsNgProperty); }
            set { this.SetProperty(IsNgProperty, value); }
        }
        #endregion

        #region 工位 Station
        /// <summary>
        /// 工位Id
        /// </summary>
        [Label("工位Id")]
        public static readonly IRefIdProperty StationIdProperty =
            P<UnloadItem>.RegisterRefId(e => e.StationId, ReferenceType.Normal);

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
            P<UnloadItem>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 工位
        /// </summary>
        public Station Station
        {
            get { return this.GetRefEntity(StationProperty); }
            set { this.SetRefEntity(StationProperty, value); }
        }
        #endregion  

        //#region 下料属性值 PropertyValueList 
        ///// <summary>
        ///// 下料属性值
        ///// </summary>
        //[Label("下料属性值")]
        //public static readonly ListProperty<EntityList<UnLoadItemPropertyValue>> PropertyValueListProperty = P<UnloadItem>.RegisterList(e => e.PropertyValueList);

        ///// <summary>
        ///// 下料属性值 
        ///// </summary>
        //public EntityList<UnLoadItemPropertyValue> PropertyValueList
        //{
        //    get { return this.GetLazyList(PropertyValueListProperty); }
        //}
        //#endregion


        #region 扩展属性 ItemExtProp
        /// <summary>
        /// 扩展属性
        /// </summary>
        [Label("扩展属性")]
        public static readonly Property<string> ItemExtPropProperty = P<UnloadItem>.Register(e => e.ItemExtProp);

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
        public static readonly Property<string> ItemExtPropNameProperty = P<UnloadItem>.Register(e => e.ItemExtPropName);

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
        public static readonly Property<string> ProjectNoProperty = P<UnloadItem>.Register(e => e.ProjectNo);

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectNo
        {
            get { return this.GetProperty(ProjectNoProperty); }
            set { this.SetProperty(ProjectNoProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("属性名")]
        public static readonly Property<string> ItemCodeProperty = P<UnloadItem>.RegisterView(e => e.ItemCode, p => p.Item.Code);

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
        public static readonly Property<string> ItemNameProperty = P<UnloadItem>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 工厂名称 工厂名称
        /// <summary>
        /// 工厂名称
        /// </summary>
        [Label("工厂名称")]
        public static readonly Property<string> FactoryNameProperty = P<UnloadItem>.RegisterView(e => e.FactoryName, p => p.Resource.Factory.Name);

        /// <summary>
        /// 工厂名称
        /// </summary>
        public string FactoryName
        {
            get { return this.GetProperty(FactoryNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 下料 实体配置
    /// </summary>
    internal class UnloadItemConfig : EntityConfig<UnloadItem>
    {
        /// <summary>
        /// 实体数据库表映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_UNLOAD_ITEM").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.Property(UnloadItem.ResourceIdProperty).ColumnMeta.HasIndex();
            Meta.Property(UnloadItem.WorkOrderIdProperty).ColumnMeta.HasIndex();
            Meta.Property(UnloadItem.SourceIdProperty).ColumnMeta.HasIndex();
        }
    }
}