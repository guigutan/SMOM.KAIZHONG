using SIE.Domain;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Stations;
using System;

namespace SIE.MES.Workbench.StationChecks
{
    /// <summary>
    /// 工位点检设备
    /// </summary>
    [RootEntity, Serializable]
    [Label("工位点检设备")]
    public class CheckEquipment : DataEntity
    {
        #region 设备编号 Code
        /// <summary>
        /// 设备编号
        /// </summary>
        [Label("设备编号")]
        public static readonly Property<string> CodeProperty = P<CheckEquipment>.Register(e => e.Code);

        /// <summary>
        /// 设备编号
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
            set { this.SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 设备名称 Name
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> NameProperty = P<CheckEquipment>.Register(e => e.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion

        #region 设备状态 State
        /// <summary>
        /// 设备状态
        /// </summary>
        [Label("设备状态")]
        public static readonly Property<State> StateProperty = P<CheckEquipment>.Register(e => e.State);

        /// <summary>
        /// 设备状态
        /// </summary>
        public State State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 保养周期 Period
        /// <summary>
        /// 保养周期
        /// </summary>
        [Label("保养周期")]
        public static readonly Property<int> PeriodProperty = P<CheckEquipment>.Register(e => e.Period);

        /// <summary>
        /// 保养周期
        /// </summary>
        public int Period
        {
            get { return this.GetProperty(PeriodProperty); }
            set { this.SetProperty(PeriodProperty, value); }
        }
        #endregion

        #region 工位 Station
        /// <summary>
        /// 工位Id
        /// </summary>
        [Label("工位")]
        public static readonly IRefIdProperty StationIdProperty =
            P<CheckEquipment>.RegisterRefId(e => e.StationId, ReferenceType.Parent);

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
            P<CheckEquipment>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 工位
        /// </summary>
        public Station Station
        {
            get { return this.GetRefEntity(StationProperty); }
            set { this.SetRefEntity(StationProperty, value); }
        }
        #endregion

        #region 工位点检设备项列表 CheckEquipmentDetail
        /// <summary>
        /// 工位点检设备项列表
        /// </summary>
        [Label("工位点检设备项列表")]
        public static readonly ListProperty<EntityList<CheckEquipmentDetail>> CheckEquipmentDetailProperty = P<CheckEquipment>.RegisterList(e => e.CheckEquipmentDetail);

        /// <summary>
        /// 工位点检设备项列表
        /// </summary>
        public EntityList<CheckEquipmentDetail> CheckEquipmentDetail
        {
            get { return this.GetLazyList(CheckEquipmentDetailProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 工位点检设备实体配置
    /// </summary>
    internal class CheckEquipmentEntityConfig : EntityConfig<CheckEquipment>
    {
        /// <summary>
        /// 配置实体元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("CHK_STATION_EQT").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }

    /// <summary>
    /// 工位扩展点检设备列表
    /// </summary>
    [CompiledPropertyDeclarer]
    public class StationExtCheckEquipmentListProperty
    {
        /// <summary>
        /// 工位点检设备扩展列表属性
        /// </summary>
        public static readonly ListProperty<EntityList<CheckEquipment>> StationCheckEquipmentListProperty =
            P<Station>.RegisterExtensionList<EntityList<CheckEquipment>>("StationCheckEquipmentList", typeof(StationExtCheckEquipmentListProperty));

        /// <summary>
        /// 工位点检设备扩展列表属性
        /// </summary>
        /// <param name="me">工位对象</param>
        /// <returns>工位点检设备列表</returns>
        public static EntityList<CheckEquipment> GetStationCheckEquipmentList(Station me)
        {
            return me.GetProperty(StationCheckEquipmentListProperty);
        }

        /// <summary>
        /// 工位点检设备扩展列表属性
        /// </summary>
        /// <param name="me">工位</param>
        /// <param name="value">工位点检设备列表</param>
        public static void SetStationCheckEquipmentList(Station me, EntityList<CheckEquipment> value)
        {
            me.SetProperty(StationCheckEquipmentListProperty, value);
        }

        /// <summary>
        /// 工位点检设备列表实体配置
        /// </summary>
        internal class StationExtCheckEquipmentListPropertyConfig : EntityConfig<Station>
        {
            /// <summary>
            /// 属性元数据配置
            /// </summary>
            protected override void ConfigMeta()
            {
                Meta.Property(StationCheckEquipmentListProperty).DontMapColumn();
            }
        }
    }
}