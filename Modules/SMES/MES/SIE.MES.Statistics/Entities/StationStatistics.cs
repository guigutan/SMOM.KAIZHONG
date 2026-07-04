using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Statistics.Entities
{
    /// <summary>
    /// 工位采集统计
    /// </summary>
    [RootEntity, Serializable]
    [Label("工位采集统计")]
    public partial class StationStatistics : DataEntity
    {
        #region 工单ID WorkOrderId
        /// <summary>
        /// 工单ID
        /// </summary>
        [Label("工单ID")]
        public static readonly Property<double> WorkOrderIdProperty = P<StationStatistics>.Register(e => e.WorkOrderId);

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
        public static readonly Property<string> WorkOrderNoProperty = P<StationStatistics>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return GetProperty(WorkOrderNoProperty); }
            set { SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 车间ID WorkShopId
        /// <summary>
        /// 车间ID
        /// </summary>
        [Label("车间ID")]
        public static readonly Property<double> WorkShopIdProperty = P<StationStatistics>.Register(e => e.WorkShopId);

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
        public static readonly Property<string> WorkShopNameProperty = P<StationStatistics>.Register(e => e.WorkShopName);

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
        public static readonly Property<double> ResourceIdProperty = P<StationStatistics>.Register(e => e.ResourceId);

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
        public static readonly Property<string> ResourceNameProperty = P<StationStatistics>.Register(e => e.ResourceName);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return GetProperty(ResourceNameProperty); }
            set { SetProperty(ResourceNameProperty, value); }
        }
        #endregion

        #region 工序ID ProcessId
        /// <summary>
        /// 工序ID
        /// </summary>
        [Label("工序ID")]
        public static readonly Property<double> ProcessIdProperty = P<StationStatistics>.Register(e => e.ProcessId);

        /// <summary>
        /// 工序ID
        /// </summary>
        public double ProcessId
        {
            get { return GetProperty(ProcessIdProperty); }
            set { SetProperty(ProcessIdProperty, value); }
        }
        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<StationStatistics>.Register(e => e.ProcessName);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return GetProperty(ProcessNameProperty); }
            set { SetProperty(ProcessNameProperty, value); }
        }
        #endregion

        #region 工位ID StationId
        /// <summary>
        /// 工位ID
        /// </summary>
        [Label("工位ID")]
        public static readonly Property<double> StationIdProperty = P<StationStatistics>.Register(e => e.StationId);

        /// <summary>
        /// 工位ID
        /// </summary>
        public double StationId
        {
            get { return GetProperty(StationIdProperty); }
            set { SetProperty(StationIdProperty, value); }
        }
        #endregion

        #region 工位名称 StationName
        /// <summary>
        /// 工位名称
        /// </summary>
        [Label("工位名称")]
        public static readonly Property<string> StationNameProperty = P<StationStatistics>.Register(e => e.StationName);

        /// <summary>
        /// 工位名称
        /// </summary>
        public string StationName
        {
            get { return GetProperty(StationNameProperty); }
            set { SetProperty(StationNameProperty, value); }
        }
        #endregion

        #region 设备ID EquipmentId
        /// <summary>
        /// 设备ID
        /// </summary>
        [Label("设备ID")]
        public static readonly Property<double> EquipmentIdProperty = P<StationStatistics>.Register(e => e.EquipmentId);

        /// <summary>
        /// 设备ID
        /// </summary>
        public double EquipmentId
        {
            get { return GetProperty(EquipmentIdProperty); }
            set { SetProperty(EquipmentIdProperty, value); }
        }
        #endregion

        #region 设备名称 EquipmentName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipmentNameProperty = P<StationStatistics>.Register(e => e.EquipmentName);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName
        {
            get { return GetProperty(EquipmentNameProperty); }
            set { SetProperty(EquipmentNameProperty, value); }
        }
        #endregion

        #region 操作人ID OperatorId
        /// <summary>
        /// 操作人ID
        /// </summary>
        [Label("操作人ID")]
        public static readonly Property<double> OperatorIdProperty = P<StationStatistics>.Register(e => e.OperatorId);

        /// <summary>
        /// 操作人ID
        /// </summary>
        public double OperatorId
        {
            get { return GetProperty(OperatorIdProperty); }
            set { SetProperty(OperatorIdProperty, value); }
        }
        #endregion

        #region 操作人 OperatorName
        /// <summary>
        /// 操作人
        /// </summary>
        [Label("操作人")]
        public static readonly Property<string> OperatorNameProperty = P<StationStatistics>.Register(e => e.OperatorName);

        /// <summary>
        /// 操作人
        /// </summary>
        public string OperatorName
        {
            get { return GetProperty(OperatorNameProperty); }
            set { SetProperty(OperatorNameProperty, value); }
        }
        #endregion

        #region 班次Id ShiftId
        /// <summary>
        /// 班次Id
        /// </summary>
        [Label("班次Id")]
        public static readonly Property<double> ShiftIdProperty = P<StationStatistics>.Register(e => e.ShiftId);

        /// <summary>
        /// 班次Id
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
        public static readonly Property<string> ShiftNameProperty = P<StationStatistics>.Register(e => e.ShiftName);

        /// <summary>
        /// 班次名称
        /// </summary>
        public string ShiftName
        {
            get { return GetProperty(ShiftNameProperty); }
            set { SetProperty(ShiftNameProperty, value); }
        }
        #endregion

        #region 采集日期 CollectDate
        /// <summary>
        /// 采集日期
        /// </summary>
        [Label("采集日期")]
        public static readonly Property<DateTime> CollectDateProperty = P<StationStatistics>.Register(e => e.CollectDate);

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
        public static readonly Property<DateTime> ShiftDateProperty = P<StationStatistics>.Register(e => e.ShiftDate);

        /// <summary>
        /// 班次日期
        /// </summary>
        public DateTime ShiftDate
        {
            get { return GetProperty(ShiftDateProperty); }
            set { SetProperty(ShiftDateProperty, value); }
        }
        #endregion

        #region 产品ID ProductId
        /// <summary>
        /// 产品ID
        /// </summary>
        [Label("产品ID")]
        public static readonly Property<double> ProductIdProperty = P<StationStatistics>.Register(e => e.ProductId);

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
        public static readonly Property<string> ProductNameProperty = P<StationStatistics>.Register(e => e.ProductName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return GetProperty(ProductNameProperty); }
            set { SetProperty(ProductNameProperty, value); }
        }
        #endregion

        #region 失败数量 QtyFailed
        /// <summary>
        /// 失败数量
        /// </summary>
        [Label("失败数量")]
        public static readonly Property<decimal> QtyFailedProperty = P<StationStatistics>.Register(e => e.QtyFailed);

        /// <summary>
        /// 失败数量
        /// </summary>
        public decimal QtyFailed
        {
            get { return GetProperty(QtyFailedProperty); }
            set { SetProperty(QtyFailedProperty, value); }
        }
        #endregion

        #region 失败台数 QtyFailedTimes
        /// <summary>
        /// 失败台数
        /// </summary>
        [Label("失败台数")]
        public static readonly Property<decimal> QtyFailedTimesProperty = P<StationStatistics>.Register(e => e.QtyFailedTimes);

        /// <summary>
        /// 失败台数
        /// </summary>
        public decimal QtyFailedTimes
        {
            get { return GetProperty(QtyFailedTimesProperty); }
            set { SetProperty(QtyFailedTimesProperty, value); }
        }
        #endregion

        #region 成功数量 QtyPass
        /// <summary>
        /// 成功数量
        /// </summary>
        [Label("成功数量")]
        public static readonly Property<decimal> QtyPassProperty = P<StationStatistics>.Register(e => e.QtyPass);

        /// <summary>
        /// 成功数量
        /// </summary>
        public decimal QtyPass
        {
            get { return GetProperty(QtyPassProperty); }
            set { SetProperty(QtyPassProperty, value); }
        }
        #endregion

        #region 成功台数 QtyTimes
        /// <summary>
        /// 成功台数
        /// </summary>
        [Label("成功台数")]
        public static readonly Property<decimal> QtyTimesProperty = P<StationStatistics>.Register(e => e.QtyTimes);

        /// <summary>
        /// 成功台数
        /// </summary>
        public decimal QtyTimes
        {
            get { return GetProperty(QtyTimesProperty); }
            set { SetProperty(QtyTimesProperty, value); }
        }
        #endregion

        #region 过站数量 QtyMove
        /// <summary>
        /// 过站数量
        /// </summary>
        [Label("过站数量")]
        public static readonly Property<decimal> QtyMoveProperty = P<StationStatistics>.Register(e => e.QtyMove);

        /// <summary>
        /// 过站数量
        /// </summary>
        public decimal QtyMove
        {
            get { return GetProperty(QtyMoveProperty); }
            set { SetProperty(QtyMoveProperty, value); }
        }
        #endregion 
    }

    /// <summary>
    /// 工位采集统计 实体配置
    /// </summary>
    internal class StationStatisticsConfig : EntityConfig<StationStatistics>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_STATS_STATION").MapAllProperties();
            Meta.Property(StationStatistics.CollectDateProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
            Meta.IndexGroupOnProperties(StationStatistics.WorkOrderIdProperty, StationStatistics.StationIdProperty);
        }
    }
}