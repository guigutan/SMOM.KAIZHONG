using SIE.Domain;
using SIE.MES.QTimes.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.QTimes.ViewModels
{
    /// <summary>
    /// QTime超时报表
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(QTimeReportViewModelCriteria))]
    [Label("QTime超时报表")]
    public class QTimeReportViewModel : ViewModel
    {
        #region QTime(min) Qtime
        /// <summary>
        /// QTime(min)
        /// </summary>
        [Label("QTime(min)")]
        public static readonly Property<decimal> QtimeProperty = P<QTimeReportViewModel>.Register(e => e.Qtime);

        /// <summary>
        /// QTime(min)
        /// </summary>
        public decimal Qtime
        {
            get { return GetProperty(QtimeProperty); }
            set { SetProperty(QtimeProperty, value); }
        }
        #endregion

        #region 标准QTime(min) QTStandard
        /// <summary>
        /// 标准QTime(min)
        /// </summary>
        [Label("标准QTime(min)")]
        public static readonly Property<decimal> QTStandardProperty = P<QTimeReportViewModel>.Register(e => e.QTStandard);

        /// <summary>
        /// 标准QTime(min)
        /// </summary>
        public decimal QTStandard
        {
            get { return GetProperty(QTStandardProperty); }
            set { SetProperty(QTStandardProperty, value); }
        }
        #endregion

        #region 条码 Barcode
        /// <summary>
        /// 条码
        /// </summary>
        [Label("条码")]
        public static readonly Property<string> BarcodeProperty = P<QTimeReportViewModel>.Register(e => e.Barcode);

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode
        {
            get { return GetProperty(BarcodeProperty); }
            set { SetProperty(BarcodeProperty, value); }
        }
        #endregion

        #region 工单 WoNo
        /// <summary>
        /// 工单
        /// </summary>
        [Label("工单")]
        public static readonly Property<string> WoNoProperty = P<QTimeReportViewModel>.Register(e => e.WoNo);

        /// <summary>
        /// 工单
        /// </summary>
        public string WoNo
        {
            get { return GetProperty(WoNoProperty); }
            set { SetProperty(WoNoProperty, value); }
        }
        #endregion

        #region 产线 WipResource
        /// <summary>
        /// 产线
        /// </summary>
        [Label("产线")]
        public static readonly Property<string> WipResourceProperty = P<QTimeReportViewModel>.Register(e => e.WipResource);

        /// <summary>
        /// 产线
        /// </summary>
        public string WipResource
        {
            get { return GetProperty(WipResourceProperty); }
            set { SetProperty(WipResourceProperty, value); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<QTimeReportViewModel>.Register(e => e.ProductCode);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return GetProperty(ProductCodeProperty); }
            set { SetProperty(ProductCodeProperty, value); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<QTimeReportViewModel>.Register(e => e.ProductName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return GetProperty(ProductNameProperty); }
            set { SetProperty(ProductNameProperty, value); }
        }
        #endregion

        #region 条码数量 BarcodeQty
        /// <summary>
        /// 条码数量
        /// </summary>
        [Label("条码数量")]
        public static readonly Property<decimal> BarcodeQtyProperty = P<QTimeReportViewModel>.Register(e => e.BarcodeQty);

        /// <summary>
        /// 条码数量
        /// </summary>
        public decimal BarcodeQty
        {
            get { return GetProperty(BarcodeQtyProperty); }
            set { SetProperty(BarcodeQtyProperty, value); }
        }
        #endregion

        #region 开始工序 StartProcess
        /// <summary>
        /// 开始工序
        /// </summary>
        [Label("开始工序")]
        public static readonly Property<string> StartProcessProperty = P<QTimeReportViewModel>.Register(e => e.StartProcess);

        /// <summary>
        /// 开始工序
        /// </summary>
        public string StartProcess
        {
            get { return GetProperty(StartProcessProperty); }
            set { SetProperty(StartProcessProperty, value); }
        }
        #endregion

        #region 结束工序 EndProcess
        /// <summary>
        /// 结束工序
        /// </summary>
        [Label("结束工序")]
        public static readonly Property<string> EndProcessProperty = P<QTimeReportViewModel>.Register(e => e.EndProcess);

        /// <summary>
        /// 结束工序
        /// </summary>
        public string EndProcess
        {
            get { return GetProperty(EndProcessProperty); }
            set { SetProperty(EndProcessProperty, value); }
        }
        #endregion

        #region 开始状态 StartState
        /// <summary>
        /// 开始状态
        /// </summary>
        [Label("开始状态")]
        public static readonly Property<QTProcessState> StartStateProperty = P<QTimeReportViewModel>.Register(e => e.StartState);

        /// <summary>
        /// 开始状态
        /// </summary>
        public QTProcessState StartState
        {
            get { return GetProperty(StartStateProperty); }
            set { SetProperty(StartStateProperty, value); }
        }
        #endregion

        #region 结束状态 EndState
        /// <summary>
        /// 结束状态
        /// </summary>
        [Label("结束状态")]
        public static readonly Property<QTProcessState> EndStateProperty = P<QTimeReportViewModel>.Register(e => e.EndState);

        /// <summary>
        /// 结束状态
        /// </summary>
        public QTProcessState EndState
        {
            get { return GetProperty(EndStateProperty); }
            set { SetProperty(EndStateProperty, value); }
        }
        #endregion

        #region 开始采集时间 StartCollectTime
        /// <summary>
        /// 开始采集时间
        /// </summary>
        [Label("开始采集时间")]
        public static readonly Property<DateTime?> StartCollectTimeProperty = P<QTimeReportViewModel>.Register(e => e.StartCollectTime);

        /// <summary>
        /// 开始采集时间
        /// </summary>
        public DateTime? StartCollectTime
        {
            get { return GetProperty(StartCollectTimeProperty); }
            set { SetProperty(StartCollectTimeProperty, value); }
        }
        #endregion

        #region 结束采集时间 EndCollectTime
        /// <summary>
        /// 结束采集时间
        /// </summary>
        [Label("结束采集时间")]
        public static readonly Property<DateTime?> EndCollectTimeProperty = P<QTimeReportViewModel>.Register(e => e.EndCollectTime);

        /// <summary>
        /// 结束采集时间
        /// </summary>
        public DateTime? EndCollectTime
        {
            get { return GetProperty(EndCollectTimeProperty); }
            set { SetProperty(EndCollectTimeProperty, value); }
        }
        #endregion

        #region 捕获时间 QueryTime
        /// <summary>
        /// 捕获时间
        /// </summary>
        [Label("捕获时间")]
        public static readonly Property<DateTime> QueryTimeProperty = P<QTimeReportViewModel>.Register(e => e.QueryTime);

        /// <summary>
        /// 捕获时间
        /// </summary>
        public DateTime QueryTime
        {
            get { return GetProperty(QueryTimeProperty); }
            set { SetProperty(QueryTimeProperty, value); }
        }
        #endregion

        #region 是否超时 IsOverTime
        /// <summary>
        /// 是否超时
        /// </summary>
        [Label("是否超时")]
        public static readonly Property<bool> IsOverTimeProperty = P<QTimeReportViewModel>.Register(e => e.IsOverTime);

        /// <summary>
        /// 是否超时
        /// </summary>
        public bool IsOverTime
        {
            get { return GetProperty(IsOverTimeProperty); }
            set { SetProperty(IsOverTimeProperty, value); }
        }
        #endregion

        #region QT标准维护Id QTId
        /// <summary>
        /// QT标准维护Id
        /// </summary>
        [Label("QT标准维护Id")]
        public static readonly Property<double> QTIdProperty = P<QTimeReportViewModel>.Register(e => e.QTId);

        /// <summary>
        /// QT标准维护Id
        /// </summary>
        public double QTId
        {
            get { return GetProperty(QTIdProperty); }
            set { SetProperty(QTIdProperty, value); }
        }
        #endregion

        #region 过滤字段
        #region 开始工序Id StartProcessId
        /// <summary>
        /// 开始工序Id
        /// </summary>
        [Label("开始工序Id")]
        public static readonly Property<double?> StartProcessIdProperty = P<QTimeReportViewModel>.Register(e => e.StartProcessId);

        /// <summary>
        /// 开始工序Id
        /// </summary>
        public double? StartProcessId
        {
            get { return this.GetProperty(StartProcessIdProperty); }
            set { this.SetProperty(StartProcessIdProperty, value); }
        }
        #endregion

        #region 结束工序Id EndProcessId
        /// <summary>
        /// 结束工序Id
        /// </summary>
        [Label("结束工序Id")]
        public static readonly Property<double?> EndProcessIdProperty = P<QTimeReportViewModel>.Register(e => e.EndProcessId);

        /// <summary>
        /// 结束工序Id
        /// </summary>
        public double? EndProcessId
        {
            get { return this.GetProperty(EndProcessIdProperty); }
            set { this.SetProperty(EndProcessIdProperty, value); }
        }
        #endregion

        #endregion
    }
}
