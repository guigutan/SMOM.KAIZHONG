using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Statistics.Fpy
{
    /// <summary>
    /// 直通率统计
    /// </summary>
    [RootEntity, Serializable]
    [DisplayMember("直通率统计")]
    public partial class FpyStatistics : DataEntity
    {
        #region 工单ID WorkOrderId
        /// <summary>
        /// 工单ID
        /// </summary>
        [Label("工单ID")]
        public static readonly Property<double> WorkOrderIdProperty = P<FpyStatistics>.Register(e => e.WorkOrderId);

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
        public static readonly Property<string> WorkOrderNoProperty = P<FpyStatistics>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return GetProperty(WorkOrderNoProperty); }
            set { SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 产品机型ID ModelId
        /// <summary>
        /// 产品机型ID
        /// </summary>
        [Label("产品机型ID")]
        public static readonly Property<double?> ModelIdProperty = P<FpyStatistics>.Register(e => e.ModelId);

        /// <summary>
        /// 产品机型ID
        /// </summary>
        public double? ModelId
        {
            get { return GetProperty(ModelIdProperty); }
            set { SetProperty(ModelIdProperty, value); }
        }
        #endregion

        #region 产品机型名称 ModelName
        /// <summary>
        /// 产品机型名称
        /// </summary>
        [Label("产品机型名称")]
        public static readonly Property<string> ModelNameProperty = P<FpyStatistics>.Register(e => e.ModelName);

        /// <summary>
        /// 产品机型名称
        /// </summary>
        public string ModelName
        {
            get { return GetProperty(ModelNameProperty); }
            set { SetProperty(ModelNameProperty, value); }
        }
        #endregion

        #region 车间ID WorkShopId
        /// <summary>
        /// 车间ID
        /// </summary>
        [Label("车间ID")]
        public static readonly Property<double> WorkShopIdProperty = P<FpyStatistics>.Register(e => e.WorkShopId);

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
        public static readonly Property<string> WorkShopNameProperty = P<FpyStatistics>.Register(e => e.WorkShopName);

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkShopName
        {
            get { return GetProperty(WorkShopNameProperty); }
            set { SetProperty(WorkShopNameProperty, value); }
        }
        #endregion

        #region 产线ID ResourceId
        /// <summary>
        /// 产线ID
        /// </summary>
        [Label("产线ID")]
        public static readonly Property<double> ResourceIdProperty = P<FpyStatistics>.Register(e => e.ResourceId);

        /// <summary>
        /// 产线ID
        /// </summary>
        public double ResourceId
        {
            get { return GetProperty(ResourceIdProperty); }
            set { SetProperty(ResourceIdProperty, value); }
        }
        #endregion

        #region 产线名称 ResourceName
        /// <summary>
        /// 产线名称
        /// </summary>
        [Label("产线名称")]
        public static readonly Property<string> ResourceNameProperty = P<FpyStatistics>.Register(e => e.ResourceName);

        /// <summary>
        /// 产线名称
        /// </summary>
        public string ResourceName
        {
            get { return GetProperty(ResourceNameProperty); }
            set { SetProperty(ResourceNameProperty, value); }
        }
        #endregion

        #region 产品ID ProductId
        /// <summary>
        /// 产品ID
        /// </summary>
        [Label("产品ID")]
        public static readonly Property<double> ProductIdProperty = P<FpyStatistics>.Register(e => e.ProductId);

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
        public static readonly Property<string> ProductNameProperty = P<FpyStatistics>.Register(e => e.ProductName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return GetProperty(ProductNameProperty); }
            set { SetProperty(ProductNameProperty, value); }
        }
        #endregion

        #region 班次Id ShiftId
        /// <summary>
        /// 班次Id
        /// </summary>
        [Label("班次Id")]
        public static readonly Property<double> ShiftIdProperty = P<FpyStatistics>.Register(e => e.ShiftId);

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
        /// 工序名称
        /// </summary>
        [Label("班次名称")]
        public static readonly Property<string> ShiftNameProperty = P<FpyStatistics>.Register(e => e.ShiftName);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ShiftName
        {
            get { return GetProperty(ShiftNameProperty); }
            set { SetProperty(ShiftNameProperty, value); }
        }
        #endregion

        #region 班次日期 ShiftDate
        /// <summary>
        /// 班次日期
        /// </summary>
        [Label("班次日期")]
        public static readonly Property<DateTime> ShiftDateProperty = P<FpyStatistics>.Register(e => e.ShiftDate);

        /// <summary>
        /// 班次日期
        /// </summary>
        public DateTime ShiftDate
        {
            get { return GetProperty(ShiftDateProperty); }
            set { SetProperty(ShiftDateProperty, value); }
        }
        #endregion

        #region 采集日期 CollectedDate
        /// <summary>
        /// 采集日期
        /// </summary>
        [Label("采集日期")]
        public static readonly Property<DateTime> CollectedDateProperty = P<FpyStatistics>.Register(e => e.CollectedDate);

        /// <summary>
        /// 采集日期
        /// </summary>
        public DateTime CollectedDate
        {
            get { return GetProperty(CollectedDateProperty); }
            set { SetProperty(CollectedDateProperty, value); }
        }
        #endregion

        #region 小时 Hour
        /// <summary>
        /// 小时
        /// </summary>
        [Label("小时")]
        public static readonly Property<int> HourProperty = P<FpyStatistics>.Register(e => e.Hour);

        /// <summary>
        /// 小时
        /// </summary>
        public int Hour
        {
            get { return GetProperty(HourProperty); }
            set { SetProperty(HourProperty, value); }
        }
        #endregion 

        #region 投入数量 InputQty
        /// <summary>
        /// 投入数量
        /// </summary>
        [Label("投入数量")]
        public static readonly Property<decimal> InputQtyProperty = P<FpyStatistics>.Register(e => e.InputQty);

        /// <summary>
        /// 投入数量
        /// </summary>
        public decimal InputQty
        {
            get { return GetProperty(InputQtyProperty); }
            set { SetProperty(InputQtyProperty, value); }
        }
        #endregion

        #region 一次良品数量(首次过站) PassQty
        /// <summary>
        /// 一次良品数量(首次过站)
        /// </summary>
        [Label("一次良品数量(首次过站)")]
        public static readonly Property<decimal> PassQtyProperty = P<FpyStatistics>.Register(e => e.PassQty);

        /// <summary>
        /// 一次良品数量(首次过站)
        /// </summary>
        public decimal PassQty
        {
            get { return GetProperty(PassQtyProperty); }
            set { SetProperty(PassQtyProperty, value); }
        }
        #endregion

        #region 一次不良数量(首次过站) FailedQty
        /// <summary>
        /// 一次不良数量(首次过站)
        /// </summary>
        [Label("一次不良数量(首次过站)")]
        public static readonly Property<decimal> FailedQtyProperty = P<FpyStatistics>.Register(e => e.FailedQty);

        /// <summary>
        /// 一次不良数量(首次过站)
        /// </summary>
        public decimal FailedQty
        {
            get { return GetProperty(FailedQtyProperty); }
            set { SetProperty(FailedQtyProperty, value); }
        }
        #endregion
    }
}