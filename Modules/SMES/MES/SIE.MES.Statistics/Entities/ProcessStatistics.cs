using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Statistics.Entities
{
    /// <summary>
    /// 工序采集统计
    /// </summary>
    [RootEntity, Serializable]
    [Label("工序采集统计")]
    public partial class ProcessStatistics : DataEntity
    {
        #region 工序顺序 ProcessIndex
        /// <summary>
        /// 工序顺序
        /// </summary>
        [Label("工序顺序")]
        public static readonly Property<int> ProcessIndexProperty = P<ProcessStatistics>.Register(e => e.ProcessIndex);

        /// <summary>
        /// 工序顺序
        /// </summary>
        public int ProcessIndex
        {
            get { return this.GetProperty(ProcessIndexProperty); }
            set { this.SetProperty(ProcessIndexProperty, value); }
        }
        #endregion


        #region 工单Id WorkOrderId
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单Id")]
        public static readonly Property<double> WorkOrderIdProperty = P<ProcessStatistics>.Register(e => e.WorkOrderId);

        /// <summary>
        /// 工单Id
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
        public static readonly Property<string> WorkOrderNoProperty = P<ProcessStatistics>.Register(e => e.WorkOrderNo);

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
        public static readonly Property<double> WorkShopIdProperty = P<ProcessStatistics>.Register(e => e.WorkShopId);

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
        public static readonly Property<string> WorkShopNameProperty = P<ProcessStatistics>.Register(e => e.WorkShopName);

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
        public static readonly Property<double> ResourceIdProperty = P<ProcessStatistics>.Register(e => e.ResourceId);

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
        public static readonly Property<string> ResourceNameProperty = P<ProcessStatistics>.Register(e => e.ResourceName);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return GetProperty(ResourceNameProperty); }
            set { SetProperty(ResourceNameProperty, value); }
        }
        #endregion 

        #region 工序Id ProcessId
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序Id")]
        public static readonly Property<double> ProcessIdProperty = P<ProcessStatistics>.Register(e => e.ProcessId);

        /// <summary>
        /// 工序Id
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
        public static readonly Property<string> ProcessNameProperty = P<ProcessStatistics>.Register(e => e.ProcessName);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return GetProperty(ProcessNameProperty); }
            set { SetProperty(ProcessNameProperty, value); }
        }
        #endregion

        #region 班次ID ShiftId
        /// <summary>
        /// 班次ID
        /// </summary>
        [Label("班次ID")]
        public static readonly Property<double> ShiftIdProperty = P<ProcessStatistics>.Register(e => e.ShiftId);

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
        public static readonly Property<string> ShiftNameProperty = P<ProcessStatistics>.Register(e => e.ShiftName);

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
        public static readonly Property<double> ProductIdProperty = P<ProcessStatistics>.Register(e => e.ProductId);

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
        public static readonly Property<string> ProductNameProperty = P<ProcessStatistics>.Register(e => e.ProductName);

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
        public static readonly Property<DateTime> CollectDateProperty = P<ProcessStatistics>.Register(e => e.CollectDate);

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
        public static readonly Property<DateTime> ShiftDateProperty = P<ProcessStatistics>.Register(e => e.ShiftDate);

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
        public static readonly Property<decimal> QtyPassProperty = P<ProcessStatistics>.Register(e => e.QtyPass);

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
        public static readonly Property<decimal> QtyFailedProperty = P<ProcessStatistics>.Register(e => e.QtyFailed);

        /// <summary>
        /// 失败数量
        /// </summary>
        public decimal QtyFailed
        {
            get { return GetProperty(QtyFailedProperty); }
            set { SetProperty(QtyFailedProperty, value); }
        }
        #endregion

        #region 过站数量 QtyMove
        /// <summary>
        /// 过站数量
        /// </summary>
        [Label("过站数量")]
        public static readonly Property<decimal> QtyMoveProperty = P<ProcessStatistics>.Register(e => e.QtyMove);

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
    /// 工序采集统计 实体配置
    /// </summary>
    internal class ProcessStatisticsConfig : EntityConfig<ProcessStatistics>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_STATS_PROCESS").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.IndexGroupOnProperties(ProcessStatistics.WorkOrderIdProperty, ProcessStatistics.ProcessIdProperty);
        }
    }
}