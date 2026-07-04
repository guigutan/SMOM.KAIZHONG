using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Warehouses.Stations
{
    /// <summary>
    /// 站台组明细
    /// </summary>
    [ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("站台组明细")]
    public partial class StationGroupLine : DataEntity
    {
        #region 序号 SequenceNo
        /// <summary>
        /// 站台组内站台的序列号,用于策略使用,默认为0
        /// </summary>
        [Label("序号")]
        public static readonly Property<int> SequenceNoProperty = P<StationGroupLine>.Register(e => e.SequenceNo);

        /// <summary>
        /// 站台组内站台的序列号,用于策略使用,默认为0
        /// </summary>
        public int SequenceNo
        {
            get { return GetProperty(SequenceNoProperty); }
            set { SetProperty(SequenceNoProperty, value); }
        }
        #endregion

        #region 备注信息 Note
        /// <summary>
        /// 备注信息
        /// </summary>
        [MaxLength(128)]
        [Label("备注信息")]
        public static readonly Property<string> NoteProperty = P<StationGroupLine>.Register(e => e.Note);

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Note
        {
            get { return GetProperty(NoteProperty); }
            set { SetProperty(NoteProperty, value); }
        }
        #endregion

        #region 站台 Station
        /// <summary>
        /// 站台Id
        /// </summary>
        [Label("站台")]
        public static readonly IRefIdProperty StationIdProperty = P<StationGroupLine>.RegisterRefId(e => e.StationId, ReferenceType.Normal);

        /// <summary>
        /// 站台Id
        /// </summary>
        public double StationId
        {
            get { return (double)GetRefId(StationIdProperty); }
            set { SetRefId(StationIdProperty, value); }
        }

        /// <summary>
        /// 站台
        /// </summary>
        public static readonly RefEntityProperty<Station> StationProperty = P<StationGroupLine>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 站台
        /// </summary>
        public Station Station
        {
            get { return GetRefEntity(StationProperty); }
            set { SetRefEntity(StationProperty, value); }
        }
        #endregion

        #region 站台组 StationGroup
        /// <summary>
        /// 站台组Id
        /// </summary>
        public static readonly IRefIdProperty StationGroupIdProperty = P<StationGroupLine>.RegisterRefId(e => e.StationGroupId, ReferenceType.Parent);

        /// <summary>
        /// 站台组Id
        /// </summary>
        public double StationGroupId
        {
            get { return (double)GetRefId(StationGroupIdProperty); }
            set { SetRefId(StationGroupIdProperty, value); }
        }

        /// <summary>
        /// 站台组
        /// </summary>
        public static readonly RefEntityProperty<StationGroup> StationGroupProperty = P<StationGroupLine>.RegisterRef(e => e.StationGroup, StationGroupIdProperty);

        /// <summary>
        /// 站台组
        /// </summary>
        public StationGroup StationGroup
        {
            get { return GetRefEntity(StationGroupProperty); }
            set { SetRefEntity(StationGroupProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 站台编码 StationCode
        /// <summary>
        /// 站台编码
        /// </summary>
        [Label("站台编码")]
        public static readonly Property<string> StationCodeProperty = P<StationGroupLine>.RegisterView(e => e.StationCode, p => p.Station.Code);

        /// <summary>
        /// 站台编码
        /// </summary>
        public string StationCode
        {
            get { return this.GetProperty(StationCodeProperty); }
        }
        #endregion


        #region 站台名称 StationName
        /// <summary>
        /// 站台名称
        /// </summary>
        [Label("站台名称")]
        public static readonly Property<string> StationNameProperty = P<StationGroupLine>.RegisterView(e => e.StationName, p => p.Station.Name);

        /// <summary>
        /// 站台名称
        /// </summary>
        public string StationName
        {
            get { return this.GetProperty(StationNameProperty); }
        }
        #endregion

        #region 站台类型 StationType
        /// <summary>
        /// 站台类型
        /// </summary>
        [Label("站台类型")]
        public static readonly Property<StationType> StationTypeProperty = P<StationGroupLine>.RegisterView(e => e.StationType, p => p.Station.StationType);

        /// <summary>
        /// 站台类型
        /// </summary>
        public StationType StationType
        {
            get { return this.GetProperty(StationTypeProperty); }
        }
        #endregion

        #region 站台状态 StationState
        /// <summary>
        /// 站台状态
        /// </summary>
        [Label("站台状态")]
        public static readonly Property<State> StationStateProperty = P<StationGroupLine>.RegisterView(e => e.StationState, p => p.Station.State);

        /// <summary>
        /// 站台状态
        /// </summary>
        public State StationState
        {
            get { return this.GetProperty(StationStateProperty); }
        }
        #endregion

        #region 仓库编码 WarehouseCode
        /// <summary>
        /// 仓库编码
        /// </summary>
        [Label("仓库编码")]
        public static readonly Property<string> WarehouseCodeProperty = P<StationGroupLine>.RegisterView(e => e.WarehouseCode, p => p.Station.Warehouse.Code);

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode
        {
            get { return this.GetProperty(WarehouseCodeProperty); }
        }
        #endregion

        #region 所在楼层 StationFloor
        /// <summary>
        /// 所在楼层
        /// </summary>
        [Label("所在楼层")]
        public static readonly Property<string> StationFloorProperty = P<StationGroupLine>.RegisterView(e => e.StationFloor, p => p.Station.Floor);

        /// <summary>
        /// 所在楼层
        /// </summary>
        public string StationFloor
        {
            get { return this.GetProperty(StationFloorProperty); }
        }
        #endregion

        #region 巷道 RoutewayCode
        /// <summary>
        /// 巷道
        /// </summary>
        [Label("巷道")]
        public static readonly Property<string> RoutewayCodeProperty = P<StationGroupLine>.RegisterView(e => e.RoutewayCode, p => p.Station.Routeway.Code);

        /// <summary>
        /// 巷道
        /// </summary>
        public string RoutewayCode
        {
            get { return this.GetProperty(RoutewayCodeProperty); }
        }
        #endregion

        #region 托盘型号 TurnoverBoxModelCode
        /// <summary>
        /// 托盘型号
        /// </summary>
        [Label("托盘型号")]
        public static readonly Property<string> TurnoverBoxModelCodeProperty = P<StationGroupLine>.RegisterView(e => e.TurnoverBoxModelCode, p => p.StationGroup.TurnoverBoxModel.Code);

        /// <summary>
        /// 托盘型号
        /// </summary>
        public string TurnoverBoxModelCode
        {
            get { return this.GetProperty(TurnoverBoxModelCodeProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 站台组明细 实体配置
    /// </summary>
    internal class StationGroupLineConfig : EntityConfig<StationGroupLine>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WCS_STATION_GROUP_LINE").MapAllProperties();
            Meta.Property(StationGroupLine.NoteProperty).ColumnMeta.HasLength(256);
            Meta.EnablePhantoms();
            Meta.EnableSort();
            Meta.DisableDataSync();
        }
    }
}