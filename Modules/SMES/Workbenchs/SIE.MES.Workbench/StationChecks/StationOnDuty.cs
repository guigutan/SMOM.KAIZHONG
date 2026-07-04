using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Tech.Stations;
using System;

namespace SIE.MES.Workbench.StationChecks
{
    /// <summary>
    /// 工位当班人员
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("工位当班")]
    public class StationOnDuty : DataEntity
    {
        #region 工位 Station
        /// <summary>
        /// 工位Id
        /// </summary>
        [Label("工位")]
        public static readonly IRefIdProperty StationIdProperty =
            P<StationOnDuty>.RegisterRefId(e => e.StationId, ReferenceType.Normal);

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
            P<StationOnDuty>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 工位
        /// </summary>
        public Station Station
        {
            get { return this.GetRefEntity(StationProperty); }
            set { this.SetRefEntity(StationProperty, value); }
        }
        #endregion

        #region 当班员工 OnDuty
        /// <summary>
        /// 当班员工Id
        /// </summary>
        [Label("当班员工")]
        public static readonly IRefIdProperty OnDutyIdProperty =
            P<StationOnDuty>.RegisterRefId(e => e.OnDutyId, ReferenceType.Normal);

        /// <summary>
        /// 当班员工Id
        /// </summary>
        public double? OnDutyId
        {
            get { return (double?)this.GetRefNullableId(OnDutyIdProperty); }
            set { this.SetRefNullableId(OnDutyIdProperty, value); }
        }

        /// <summary>
        /// 当班员工
        /// </summary>
        public static readonly RefEntityProperty<Employee> OnDutyProperty =
            P<StationOnDuty>.RegisterRef(e => e.OnDuty, OnDutyIdProperty);

        /// <summary>
        /// 当班员工
        /// </summary>
        public Employee OnDuty
        {
            get { return this.GetRefEntity(OnDutyProperty); }
            set { this.SetRefEntity(OnDutyProperty, value); }
        }
        #endregion

        #region 实际当班员工 ActualOnDuty
        /// <summary>
        /// 实际当班员工Id
        /// </summary>
        [Label("实际当班员工")]
        public static readonly IRefIdProperty ActualOnDutyIdProperty =
            P<StationOnDuty>.RegisterRefId(e => e.ActualOnDutyId, ReferenceType.Normal);

        /// <summary>
        /// 实际当班员工Id
        /// </summary>
        public double? ActualOnDutyId
        {
            get { return (double?)this.GetRefNullableId(ActualOnDutyIdProperty); }
            set { this.SetRefNullableId(ActualOnDutyIdProperty, value); }
        }

        /// <summary>
        /// 实际当班员工
        /// </summary>
        public static readonly RefEntityProperty<Employee> ActualOnDutyProperty =
            P<StationOnDuty>.RegisterRef(e => e.ActualOnDuty, ActualOnDutyIdProperty);

        /// <summary>
        /// 实际当班员工
        /// </summary>
        public Employee ActualOnDuty
        {
            get { return this.GetRefEntity(ActualOnDutyProperty); }
            set { this.SetRefEntity(ActualOnDutyProperty, value); }
        }
        #endregion

        #region 日期 OnDutyDate
        /// <summary>
        /// 日期
        /// </summary>
        [Label("属性名")]
        public static readonly Property<DateTime> OnDutyDateProperty = P<StationOnDuty>.Register(e => e.OnDutyDate, new PropertyMetadata<DateTime> { DateTimePart = DateTimePart.Date });

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime OnDutyDate
        {
            get { return this.GetProperty(OnDutyDateProperty); }
            set { this.SetProperty(OnDutyDateProperty, value); }
        }
        #endregion 
    }

    internal class StationOnDutyEntityConfig : EntityConfig<StationOnDuty>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("STATION_ON_DUTY").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}