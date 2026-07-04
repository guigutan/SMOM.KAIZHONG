using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Statistics.Entities
{
    /// <summary>
    /// 资源半小时产能采集统计
    /// </summary>
    [RootEntity, Serializable]
    [Label("资源半小时产能采集统计")]
    public partial class ResourceStatistics : DataEntity
    {
        #region 资源ID ResourceId
        /// <summary>
        /// 资源ID
        /// </summary>
        [Label("资源ID")]
        public static readonly Property<double> ResourceIdProperty = P<ResourceStatistics>.Register(e => e.ResourceId);

        /// <summary>
        /// 资源ID
        /// </summary>
        public double ResourceId
        {
            get { return GetProperty(ResourceIdProperty); }
            set { SetProperty(ResourceIdProperty, value); }
        }
        #endregion

        #region 班次日期 ShiftDate
        /// <summary>
        /// 班次日期
        /// </summary>
        [Label("班次日期")]
        public static readonly Property<DateTime> ShiftDateProperty = P<ResourceStatistics>.Register(e => e.ShiftDate);

        /// <summary>
        /// 班次日期
        /// </summary>
        public DateTime ShiftDate
        {
            get { return GetProperty(ShiftDateProperty); }
            set { SetProperty(ShiftDateProperty, value); }
        }
        #endregion

        #region 采集日期 CollectDate
        /// <summary>
        /// 采集日期
        /// </summary>
        [Label("采集日期")]
        public static readonly Property<DateTime> CollectDateProperty = P<ResourceStatistics>.Register(e => e.CollectDate);

        /// <summary>
        /// 采集日期
        /// </summary>
        public DateTime CollectDate
        {
            get { return GetProperty(CollectDateProperty); }
            set { SetProperty(CollectDateProperty, value); }
        }
        #endregion

        #region 上线数 OnlineQty
        /// <summary>
        /// 上线数
        /// </summary>
        [Label("上线数")]
        public static readonly Property<decimal> OnlineQtyProperty = P<ResourceStatistics>.Register(e => e.OnlineQty);

        /// <summary>
        /// 上线数
        /// </summary>
        public decimal OnlineQty
        {
            get { return GetProperty(OnlineQtyProperty); }
            set { SetProperty(OnlineQtyProperty, value); }
        }
        #endregion

        #region 下线数 OfflineQty
        /// <summary>
        /// 下线数
        /// </summary>
        [Label("下线数")]
        public static readonly Property<decimal?> OfflineQtyProperty = P<ResourceStatistics>.Register(e => e.OfflineQty);

        /// <summary>
        /// 下线数
        /// </summary>
        public decimal? OfflineQty
        {
            get { return GetProperty(OfflineQtyProperty); }
            set { SetProperty(OfflineQtyProperty, value); }
        }
        #endregion

        #region 不良数 NgQty
        /// <summary>
        /// 不良数
        /// </summary>
        [Label("不良数")]
        public static readonly Property<decimal?> NgQtyProperty = P<ResourceStatistics>.Register(e => e.NgQty);

        /// <summary>
        /// 不良数
        /// </summary>
        public decimal? NgQty
        {
            get { return GetProperty(NgQtyProperty); }
            set { SetProperty(NgQtyProperty, value); }
        }
        #endregion

        #region 开始时间 StartTime
        /// <summary>
        /// 开始时间
        /// </summary>
        [Label("开始时间")]
        public static readonly Property<DateTime> StartTimeProperty = P<ResourceStatistics>.Register(e => e.StartTime);

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime
        {
            get { return GetProperty(StartTimeProperty); }
            set { SetProperty(StartTimeProperty, value); }
        }
        #endregion

        #region 结束时间 EndTime
        /// <summary>
        /// 结束时间
        /// </summary>
        [Label("结束时间")]
        public static readonly Property<DateTime> EndTimeProperty = P<ResourceStatistics>.Register(e => e.EndTime);

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime
        {
            get { return GetProperty(EndTimeProperty); }
            set { SetProperty(EndTimeProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 资源半小时产能采集统计 实体配置
    /// </summary>
    internal class ResourceStatisticsConfig : EntityConfig<ResourceStatistics>
    {
        /// <summary>
        /// 子类重写此方法，并完成对 Meta 属性的配置。
        /// 注意：
        /// * 为了给当前类的子类也运行同样的配置，这个方法可能会被调用多次。
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_STATS_RESOURCE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}