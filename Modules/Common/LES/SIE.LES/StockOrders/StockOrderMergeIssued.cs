using SIE.Domain;
using SIE.LES.LinesideWarehouses;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.LES.StockOrders
{
    /// <summary>
    /// 备料单合并下发规则
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(StockOrderMergeIssuedCriteria))]
    [Label("备料单合并下发规则")]
    public partial class StockOrderMergeIssued : DataEntity, IStateEntity
    {
        #region 产线线边仓 LinesideWarehouse
        /// <summary>
        /// 产线线边仓Id
        /// </summary>
        [Label("产线线边仓")]
        public static readonly IRefIdProperty LinesideWarehouseIdProperty =
            P<StockOrderMergeIssued>.RegisterRefId(e => e.LinesideWarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 产线线边仓Id
        /// </summary>
        public double? LinesideWarehouseId
        {
            get { return (double?)this.GetRefNullableId(LinesideWarehouseIdProperty); }
            set { this.SetRefNullableId(LinesideWarehouseIdProperty, value); }
        }

        /// <summary>
        /// 产线线边仓
        /// </summary>
        public static readonly RefEntityProperty<LinesideWarehouse> LinesideWarehouseProperty =
            P<StockOrderMergeIssued>.RegisterRef(e => e.LinesideWarehouse, LinesideWarehouseIdProperty);

        /// <summary>
        /// 产线线边仓
        /// </summary>
        public LinesideWarehouse LinesideWarehouse
        {
            get { return this.GetRefEntity(LinesideWarehouseProperty); }
            set { this.SetRefEntity(LinesideWarehouseProperty, value); }
        }
        #endregion

        #region 备料模式 StockModel
        /// <summary>
        /// 备料模式
        /// </summary>
        [Label("备料模式")]
        public static readonly Property<PrepareItemType?> StockModelProperty = P<StockOrderMergeIssued>.Register(e => e.StockModel);

        /// <summary>
        /// 备料模式
        /// </summary>
        public PrepareItemType? StockModel
        {
            get => GetProperty(StockModelProperty);
            set => SetProperty(StockModelProperty, value);
        }
        #endregion      

        #region 状态 State  
        /// <summary>  
        /// 状态  
        /// </summary>  
        [Label("状态")]
        public static readonly Property<State> StateProperty = P<StockOrderMergeIssued>.Register(e => e.State);

        /// <summary>  
        /// 状态  
        /// </summary>  
        public State State
        {
            get => GetProperty(StateProperty);
            set => SetProperty(StateProperty, value);
        }
        #endregion

        #region 合并时间段 StockOrderMergeTimesList
        /// <summary>
        /// 合并时间段
        /// </summary>
        public static readonly ListProperty<EntityList<StockOrderMergeTimes>> StockOrderMergeTimesListProperty = P<StockOrderMergeIssued>.RegisterList(e => e.StockOrderMergeTimesList);
        /// <summary>
        /// 合并时间段
        /// </summary>
        public EntityList<StockOrderMergeTimes> StockOrderMergeTimesList
        {
            get { return this.GetLazyList(StockOrderMergeTimesListProperty); }
        }
        #endregion

        #region 视图属性

        #region 生产资源Id WipResourceId
        /// <summary>
        /// 生产资源Id
        /// </summary>
        [Label("生产资源Id")]
        public static readonly Property<double?> WipResourceIdProperty = P<StockOrderMergeIssued>.RegisterView(e => e.WipResourceId, p => p.LinesideWarehouse.WipResouce.Id);

        /// <summary>
        /// 生产资源Id
        /// </summary>
        public double? WipResourceId
        {
            get { return this.GetProperty(WipResourceIdProperty); }
        }
        #endregion

        #region 生产资源 WipResourceName
        /// <summary>
        /// 生产资源
        /// </summary>
        [Label("生产资源")]
        public static readonly Property<string> WipResourceNameProperty = P<StockOrderMergeIssued>.RegisterView(e => e.WipResourceName, p => p.LinesideWarehouse.WipResouce.Name);

        /// <summary>
        /// 生产资源
        /// </summary>
        public string WipResourceName
        {
            get { return this.GetProperty(WipResourceNameProperty); }
        }
        #endregion

        #region 接收仓库 WarehouseId
        /// <summary>
        /// 接收仓库
        /// </summary>
        [Label("接收仓库")]
        public static readonly Property<double?> WarehouseIdProperty = P<StockOrderMergeIssued>.RegisterView(e => e.WarehouseId, p => p.LinesideWarehouse.Warehouse.Id);

        /// <summary>
        /// 接收仓库
        /// </summary>
        public double? WarehouseId
        {
            get { return this.GetProperty(WarehouseIdProperty); }
        }
        #endregion

        #region 接收仓库 WarehouseName
        /// <summary>
        /// 接收仓库
        /// </summary>
        [Label("接收仓库")]
        public static readonly Property<string> WarehouseNameProperty = P<StockOrderMergeIssued>.RegisterView(e => e.WarehouseName, p => p.LinesideWarehouse.Warehouse.Name);

        /// <summary>
        /// 接收仓库
        /// </summary>
        public string WarehouseName
        {
            get { return this.GetProperty(WarehouseNameProperty); }
        }
        #endregion

        #region 接收仓库编码 WarehouseCode
        /// <summary>
        /// 接收仓库编码
        /// </summary>
        [Label("接收仓库编码")]
        public static readonly Property<string> WarehouseCodeProperty = P<StockOrderMergeIssued>.RegisterView(e => e.WarehouseCode, p => p.LinesideWarehouse.Warehouse.Code);

        /// <summary>
        /// 接收仓库编码
        /// </summary>
        public string WarehouseCode
        {
            get { return this.GetProperty(WarehouseCodeProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 备料单合并下发规则 实体配置
    /// </summary>
    internal class StockOrderMergeIssuedConfig : EntityConfig<StockOrderMergeIssued>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("STOCK_ORDER_MERGE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }

}
