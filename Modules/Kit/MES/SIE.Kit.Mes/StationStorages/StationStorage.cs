using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Stations;
using System;

namespace SIE.Kit.MES.StationStorages
{
    /// <summary>
    /// 工位库存
    /// </summary>
    [RootEntity, Serializable]
    [Label("工位库存")]
    [ConditionQueryType(typeof(StationStorageCriteria))]
    public class StationStorage : DataEntity
    {
        #region 工位 Station
        /// <summary>
        /// 工位Id
        /// </summary>
        [Label("工位")]
        public static readonly IRefIdProperty StationIdProperty =
            P<StationStorage>.RegisterRefId(e => e.StationId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Station> StationProperty =
            P<StationStorage>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 工位
        /// </summary>
        public Station Station
        {
            get { return this.GetRefEntity(StationProperty); }
            set { this.SetRefEntity(StationProperty, value); }
        }
        #endregion

        #region 工单工位库存列表 WoStorageList
        /// <summary>
        /// 工单工位库存列表
        /// </summary>
        [Label("工单工位库存列表")]
        public static readonly ListProperty<EntityList<WoStationStorage>> WoStorageListProperty = P<StationStorage>.RegisterList(e => e.WoStorageList);

        /// <summary>
        /// 工单工位库存列表
        /// </summary>
        public EntityList<WoStationStorage> WoStorageList
        {
            get { return this.GetLazyList(WoStorageListProperty); }
        }
        #endregion

        #region 视图属性
        #region 工位编码 StationCode
        /// <summary>
        /// 工位编码
        /// </summary>
        [Label("工位编码")]
        public static readonly Property<string> StationCodeProperty = P<StationStorage>.RegisterView(e => e.StationCode, p => p.Station.Code);

        /// <summary>
        /// 工位编码
        /// </summary>
        public string StationCode
        {
            get { return this.GetProperty(StationCodeProperty); }
        }
        #endregion 

        #region 工位名称 StationName
        /// <summary>
        /// 工位名称
        /// </summary>
        [Label("工位名称")]
        public static readonly Property<string> StationNameProperty = P<StationStorage>.RegisterView(e => e.StationName, p => p.Station.Name);

        /// <summary>
        /// 工位名称
        /// </summary>
        public string StationName
        {
            get { return this.GetProperty(StationNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 工位库存 实体配置
    /// </summary>
    internal class StationStorageEntityConfig : EntityConfig<StationStorage>
    {
        /// <summary>
        /// 配置数据库的映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_STATION_STORAGE").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.Property(StationStorage.StationIdProperty).ColumnMeta.HasIndex();
        }
    }
}