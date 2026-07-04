using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Kit.MES.StationStorages
{
    /// <summary>
    /// 工单工位库存
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工单工位库存")]
    public partial class WoStationStorage : DataEntity
    {
        #region 工位库存 StationStorage
        /// <summary>
        /// 工位库存Id
        /// </summary>
        [Label("工位库存")]
        public static readonly IRefIdProperty StationStorageIdProperty =
            P<WoStationStorage>.RegisterRefId(e => e.StationStorageId, ReferenceType.Parent);

        /// <summary>
        /// 工位库存Id
        /// </summary>
        public double StationStorageId
        {
            get { return (double)this.GetRefId(StationStorageIdProperty); }
            set { this.SetRefId(StationStorageIdProperty, value); }
        }

        /// <summary>
        /// 工位库存
        /// </summary>
        public static readonly RefEntityProperty<StationStorage> StationStorageProperty =
            P<WoStationStorage>.RegisterRef(e => e.StationStorage, StationStorageIdProperty);

        /// <summary>
        /// 工位库存
        /// </summary>
        public StationStorage StationStorage
        {
            get { return this.GetRefEntity(StationStorageProperty); }
            set { this.SetRefEntity(StationStorageProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<WoStationStorage>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId
        {
            get { return (double)this.GetRefId(WorkOrderIdProperty); }
            set { this.SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty =
            P<WoStationStorage>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 物料库存 ItemStorage
        /// <summary>
        /// 物料库存
        /// </summary>
        [Label("物料库存")]
        public static readonly ListProperty<EntityList<StationItemStorage>> ItemStorageProperty = P<WoStationStorage>.RegisterList(e => e.ItemStorage);

        /// <summary>
        /// 物料库存
        /// </summary>
        public EntityList<StationItemStorage> ItemStorage
        {
            get { return this.GetLazyList(ItemStorageProperty); }
        }
        #endregion

        #region 视图属性
        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<WoStationStorage>.RegisterView(e => e.WorkOrderNo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 工单工位库存 实体配置
    /// </summary>
    internal class WoStationStorageConfig : EntityConfig<WoStationStorage>
    {
        /// <summary>
        /// 配置数据库的映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_WO_STATION_STORAGE").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.Property(WoStationStorage.WorkOrderIdProperty).ColumnMeta.HasIndex();
            Meta.Property(WoStationStorage.StationStorageIdProperty).ColumnMeta.HasIndex();
        }
    }
}