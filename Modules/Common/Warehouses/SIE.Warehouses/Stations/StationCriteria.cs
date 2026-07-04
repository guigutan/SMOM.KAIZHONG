using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Warehouses.Stations
{
    /// <summary>
    /// 站台查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("站台查询实体")]
    public partial class StationCriteria : Criteria
    {
        #region 站台代码 Code
        /// <summary>
        /// 站台代码，规则：站台类型名：排_层_列_库区_仓库
        /// </summary>
        [MaxLength(80)]
        [Label("编码")]
        [Required, NotDuplicate]
        public static readonly Property<string> CodeProperty = P<StationCriteria>.Register(e => e.Code);

        /// <summary>
        /// 站台代码，规则：站台类型名：排_层_列_库区_仓库
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(80)]
        [Label("名称")]
        [Required, NotDuplicate]
        public static readonly Property<string> NameProperty = P<StationCriteria>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 站台类型 StationType
        /// <summary>
        /// 站台类型
        /// </summary>
        [Label("站台类型")]
        public static readonly Property<StationType?> StationTypeProperty = P<StationCriteria>.Register(e => e.StationType);

        /// <summary>
        /// 站台类型
        /// </summary>
        public StationType? StationType
        {
            get { return GetProperty(StationTypeProperty); }
            set { SetProperty(StationTypeProperty, value); }
        }
        #endregion

        #region LED屏幕基础数据 Led
        /// <summary>
        /// LED屏幕基础数据Id
        /// </summary>
        [Label("LED屏幕基础数据")]
        public static readonly IRefIdProperty LedIdProperty =
            P<StationCriteria>.RegisterRefId(e => e.LedId, ReferenceType.Normal);

        /// <summary>
        /// LED屏幕基础数据Id
        /// </summary>
        public double? LedId
        {
            get { return (double?)this.GetRefNullableId(LedIdProperty); }
            set { this.SetRefNullableId(LedIdProperty, value); }
        }

        /// <summary>
        /// LED屏幕基础数据
        /// </summary>
        public static readonly RefEntityProperty<LED> LedProperty =
            P<StationCriteria>.RegisterRef(e => e.Led, LedIdProperty);

        /// <summary>
        /// LED屏幕基础数据
        /// </summary>
        public LED Led
        {
            get { return this.GetRefEntity(LedProperty); }
            set { this.SetRefEntity(LedProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty =
            P<StationCriteria>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
            P<StationCriteria>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return this.GetRefEntity(WarehouseProperty); }
            set { this.SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 巷道 Routeway
        /// <summary>
        /// 巷道Id
        /// </summary>
        [Label("巷道")]
        public static readonly IRefIdProperty RoutewayIdProperty =
            P<StationCriteria>.RegisterRefId(e => e.RoutewayId, ReferenceType.Normal);

        /// <summary>
        /// 巷道Id
        /// </summary>
        public double? RoutewayId
        {
            get { return (double?)this.GetRefNullableId(RoutewayIdProperty); }
            set { this.SetRefNullableId(RoutewayIdProperty, value); }
        }

        /// <summary>
        /// 巷道
        /// </summary>
        public static readonly RefEntityProperty<Routeway> RoutewayProperty =
            P<StationCriteria>.RegisterRef(e => e.Routeway, RoutewayIdProperty);

        /// <summary>
        /// 巷道
        /// </summary>
        public Routeway Routeway
        {
            get { return this.GetRefEntity(RoutewayProperty); }
            set { this.SetRefEntity(RoutewayProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State?> StateProperty = P<StationCriteria>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State? State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 是否入库站台 IsInStation
        /// <summary>
        /// 是否入库站台
        /// </summary>
        [Label("是否入库站台")]
        public static readonly Property<bool> IsInStationProperty = P<StationCriteria>.Register(e => e.IsInStation);

        /// <summary>
        /// 是否入库站台
        /// </summary>
        public bool IsInStation
        {
            get { return GetProperty(IsInStationProperty); }
            set { SetProperty(IsInStationProperty, value); }
        }
        #endregion

        #region 是否出库站台 IsOutStation
        /// <summary>
        /// 是否出库站台
        /// </summary>
        [Label("是否出库站台")]
        public static readonly Property<bool> IsOutStationProperty = P<StationCriteria>.Register(e => e.IsOutStation);

        /// <summary>
        /// 是否出库站台
        /// </summary>
        public bool IsOutStation
        {
            get { return GetProperty(IsOutStationProperty); }
            set { SetProperty(IsOutStationProperty, value); }
        }
        #endregion

        #region 是否拣选站台 IsPickStation
        /// <summary>
        /// 是否拣选站台
        /// </summary>
        [Label("是否拣选站台")]
        public static readonly Property<bool> IsPickStationProperty = P<StationCriteria>.Register(e => e.IsPickStation);

        /// <summary>
        /// 是否拣选站台
        /// </summary>
        public bool IsPickStation
        {
            get { return GetProperty(IsPickStationProperty); }
            set { SetProperty(IsPickStationProperty, value); }
        }
        #endregion

        #region 是否盘点站台 IsCountStation
        /// <summary>
        /// 是否盘点站台
        /// </summary>
        [Label("是否盘点站台")]
        public static readonly Property<bool> IsCountStationProperty = P<StationCriteria>.Register(e => e.IsCountStation);

        /// <summary>
        /// 是否盘点站台
        /// </summary>
        public bool IsCountStation
        {
            get { return GetProperty(IsCountStationProperty); }
            set { SetProperty(IsCountStationProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns>计划资料列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<StationController>().GetStations(this);
        }
    }
}
