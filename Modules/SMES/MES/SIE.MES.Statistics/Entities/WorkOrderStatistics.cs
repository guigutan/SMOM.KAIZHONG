using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Statistics.Entities
{
    /// <summary>
    /// 工单采集统计
    /// </summary>
    [RootEntity, Serializable]
    [Label("工单采集统计")]
    public partial class WorkOrderStatistics : DataEntity
    {
        #region 工单ID WorkOrderId
        /// <summary>
        /// 工单ID
        /// </summary>
        [Label("工单ID")]
        public static readonly Property<double> WorkOrderIdProperty = P<WorkOrderStatistics>.Register(e => e.WorkOrderId);

        /// <summary>
        /// 工单ID
        /// </summary>
        public double WorkOrderId
        {
            get { return GetProperty(WorkOrderIdProperty); }
            set { SetProperty(WorkOrderIdProperty, value); }
        }
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<WorkOrderStatistics>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return GetProperty(WorkOrderNoProperty); }
            set { SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 客户ID CustomerId
        /// <summary>
        /// 客户ID
        /// </summary>
        [Label("客户ID")]
        public static readonly Property<double> CustomerIdProperty = P<WorkOrderStatistics>.Register(e => e.CustomerId);

        /// <summary>
        /// 客户ID
        /// </summary>
        public double CustomerId
        {
            get { return this.GetProperty(CustomerIdProperty); }
            set { this.SetProperty(CustomerIdProperty, value); }
        }
        #endregion

        #region 客户名称 CustomerName
        /// <summary>
        /// 客户名称
        /// </summary>
        [Label("客户名称")]
        public static readonly Property<string> CustomerNameProperty = P<WorkOrderStatistics>.Register(e => e.CustomerName);

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName
        {
            get { return this.GetProperty(CustomerNameProperty); }
            set { this.SetProperty(CustomerNameProperty, value); }
        }
        #endregion

        #region 车间ID WorkShopId
        /// <summary>
        /// 车间ID
        /// </summary>
        [Label("车间ID")]
        public static readonly Property<double> WorkShopIdProperty = P<WorkOrderStatistics>.Register(e => e.WorkShopId);

        /// <summary>
        /// 车间ID
        /// </summary>
        public double WorkShopId
        {
            get { return GetProperty(WorkShopIdProperty); }
            set { SetProperty(WorkShopIdProperty, value); }
        }
        #endregion

        #region 车间名称 WorkShopName
        /// <summary>
        /// 车间名称
        /// </summary>
        [Label("车间名称")]
        public static readonly Property<string> WorkShopNameProperty = P<WorkOrderStatistics>.Register(e => e.WorkShopName);

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkShopName
        {
            get { return GetProperty(WorkShopNameProperty); }
            set { SetProperty(WorkShopNameProperty, value); }
        }
        #endregion 

        #region 资源ID ResourceId
        /// <summary>
        /// 资源ID
        /// </summary>
        [Label("资源ID")]
        public static readonly Property<double> ResourceIdProperty = P<WorkOrderStatistics>.Register(e => e.ResourceId);

        /// <summary>
        /// 资源ID
        /// </summary>
        public double ResourceId
        {
            get { return GetProperty(ResourceIdProperty); }
            set { SetProperty(ResourceIdProperty, value); }
        }
        #endregion

        #region 资源名称 ResourceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源名称")]
        public static readonly Property<string> ResourceNameProperty = P<WorkOrderStatistics>.Register(e => e.ResourceName);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return GetProperty(ResourceNameProperty); }
            set { SetProperty(ResourceNameProperty, value); }
        }
        #endregion

        #region 班次ID ShiftId
        /// <summary>
        /// 班次ID
        /// </summary>
        [Label("班次ID")]
        public static readonly Property<double> ShiftIdProperty = P<WorkOrderStatistics>.Register(e => e.ShiftId);

        /// <summary>
        /// 班次ID
        /// </summary>
        public double ShiftId
        {
            get { return GetProperty(ShiftIdProperty); }
            set { SetProperty(ShiftIdProperty, value); }
        }
        #endregion

        #region 班次名称 ShiftName
        /// <summary>
        /// 班次名称
        /// </summary>
        [Label("班次名称")]
        public static readonly Property<string> ShiftNameProperty = P<WorkOrderStatistics>.Register(e => e.ShiftName);

        /// <summary>
        /// 班次名称
        /// </summary>
        public string ShiftName
        {
            get { return GetProperty(ShiftNameProperty); }
            set { SetProperty(ShiftNameProperty, value); }
        }
        #endregion

        #region 产品ID ProductId
        /// <summary>
        /// 产品ID
        /// </summary>
        [Label("产品ID")]
        public static readonly Property<double> ProductIdProperty = P<WorkOrderStatistics>.Register(e => e.ProductId);

        /// <summary>
        /// 产品ID
        /// </summary>
        public double ProductId
        {
            get { return GetProperty(ProductIdProperty); }
            set { SetProperty(ProductIdProperty, value); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<WorkOrderStatistics>.Register(e => e.ProductName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return GetProperty(ProductNameProperty); }
            set { SetProperty(ProductNameProperty, value); }
        }
        #endregion

        #region 采集日期 CollectDate
        /// <summary>
        /// 采集日期
        /// </summary>
        [Label("采集日期")]
        public static readonly Property<DateTime> CollectDateProperty = P<WorkOrderStatistics>.Register(e => e.CollectDate);

        /// <summary>
        /// 采集日期
        /// </summary>
        public DateTime CollectDate
        {
            get { return GetProperty(CollectDateProperty); }
            set { SetProperty(CollectDateProperty, value); }
        }
        #endregion

        #region 班次日期 ShiftDate
        /// <summary>
        /// 班次日期
        /// </summary>
        [Label("班次日期")]
        public static readonly Property<DateTime> ShiftDateProperty = P<WorkOrderStatistics>.Register(e => e.ShiftDate);

        /// <summary>
        /// 班次日期
        /// </summary>
        public DateTime ShiftDate
        {
            get { return GetProperty(ShiftDateProperty); }
            set { SetProperty(ShiftDateProperty, value); }
        }
        #endregion

        #region 成功数量 QtyPass
        /// <summary>
        /// 成功数量
        /// </summary>
        [Label("成功数量")]
        public static readonly Property<decimal> QtyPassProperty = P<WorkOrderStatistics>.Register(e => e.QtyPass);

        /// <summary>
        /// 成功数量
        /// </summary>
        public decimal QtyPass
        {
            get { return GetProperty(QtyPassProperty); }
            set { SetProperty(QtyPassProperty, value); }
        }
        #endregion

        #region 失败数量 QtyFailed
        /// <summary>
        /// 失败数量
        /// </summary>
        [Label("失败数量")]
        public static readonly Property<decimal> QtyFailedProperty = P<WorkOrderStatistics>.Register(e => e.QtyFailed);

        /// <summary>
        /// 失败数量
        /// </summary>
        public decimal QtyFailed
        {
            get { return GetProperty(QtyFailedProperty); }
            set { SetProperty(QtyFailedProperty, value); }
        }
        #endregion

        #region 上线数 QtyOnline
        /// <summary>
        /// 上线数
        /// </summary>
        [Label("上线数")]
        public static readonly Property<decimal> QtyOnlineProperty = P<WorkOrderStatistics>.Register(e => e.QtyOnline);

        /// <summary>
        /// 上线数
        /// </summary>
        public decimal QtyOnline
        {
            get { return GetProperty(QtyOnlineProperty); }
            set { SetProperty(QtyOnlineProperty, value); }
        }
        #endregion

        #region 小时值 Hour
        /// <summary>
        /// 小时值
        /// </summary>
        [Label("小时值")]
        public static readonly Property<int> HourProperty = P<WorkOrderStatistics>.Register(e => e.Hour);

        /// <summary>
        /// 小时值
        /// </summary>
        public int Hour
        {
            get { return this.GetProperty(HourProperty); }
            set { this.SetProperty(HourProperty, value); }
        }
        #endregion

        #region 采集开始时间 BeginCollectTime
        /// <summary>
        /// 采集开始时间
        /// </summary>
        [Label("采集开始时间")]
        public static readonly Property<DateTime> BeginCollectTimeProperty = P<WorkOrderStatistics>.Register(e => e.BeginCollectTime);

        /// <summary>
        /// 采集开始时间
        /// </summary>
        public DateTime BeginCollectTime
        {
            get { return this.GetProperty(BeginCollectTimeProperty); }
            set { this.SetProperty(BeginCollectTimeProperty, value); }
        }
        #endregion

        #region 采集结束时间 EndCollectTime
        /// <summary>
        /// 采集结束时间
        /// </summary>
        [Label("采集结束时间")]
        public static readonly Property<DateTime> EndCollectTimeProperty = P<WorkOrderStatistics>.Register(e => e.EndCollectTime);

        /// <summary>
        /// 采集结束时间
        /// </summary>
        public DateTime EndCollectTime
        {
            get { return this.GetProperty(EndCollectTimeProperty); }
            set { this.SetProperty(EndCollectTimeProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 工单采集统计 实体配置
    /// </summary>
    internal class WorkOrderStatisticsConfig : EntityConfig<WorkOrderStatistics>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_STATS_WO").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.IndexGroupOnProperties(WorkOrderStatistics.WorkOrderIdProperty, WorkOrderStatistics.ResourceIdProperty);
        }
    }
}