using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.BatchWIP.Products.ViewModels.BatchWipProductReport
{
    /// <summary>
    /// 分配明细库存报表
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(BatchWipProductReportCriteria))]
    [Label("工位库存库龄查询")]
    public class BatchWipProductReport: ViewModel
    {
        #region 批次号 BatchNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> BatchNoProperty = P<BatchWipProductReport>.Register(e => e.BatchNo);

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo
        {
            get { return GetProperty(BatchNoProperty); }
            set { SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 工位 StationName
        /// <summary>
        /// 工位
        /// </summary>
        [Label("工位")]
        public static readonly Property<string> StationNameProperty = P<BatchWipProductReport>.Register(e => e.StationName);

        /// <summary>
        /// 工位
        /// </summary>
        public string StationName
        {
            get { return GetProperty(StationNameProperty); }
            set { SetProperty(StationNameProperty, value); }
        }
        #endregion

        #region 当前工序 ProcessName
        /// <summary>
        /// 当前工序
        /// </summary>
        [Label("当前工序")]
        public static readonly Property<string> ProcessNameProperty = P<BatchWipProductReport>.Register(e => e.ProcessName);

        /// <summary>
        /// 当前工序
        /// </summary>
        public string ProcessName
        {
            get { return GetProperty(ProcessNameProperty); }
            set { SetProperty(ProcessNameProperty, value); }
        }
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<BatchWipProductReport>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return GetProperty(WorkOrderNoProperty); }
            set { SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<BatchWipProductReport>.Register(e => e.ProductCode);

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
        public static readonly Property<string> ProductNameProperty = P<BatchWipProductReport>.Register(e => e.ProductName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return GetProperty(ProductNameProperty); }
            set { SetProperty(ProductNameProperty, value); }
        }
        #endregion

        #region 批次数量 BatchQty
        /// <summary>
        /// 批次数量
        /// </summary>
        [Label("批次数量")]
        public static readonly Property<decimal> BatchQtyProperty = P<BatchWipProductReport>.Register(e => e.BatchQty);

        /// <summary>
        /// 批次数量
        /// </summary>
        public decimal BatchQty
        {
            get { return GetProperty(BatchQtyProperty); }
            set { SetProperty(BatchQtyProperty, value); }
        }
        #endregion

        #region 呆滞库龄天数 SluggishStorageAgeDays
        /// <summary>
        /// 呆滞库龄天数
        /// </summary>
        [Label("呆滞库龄天数")]
        public static readonly Property<double> SluggishStorageAgeDaysProperty = P<BatchWipProductReport>.Register(e => e.SluggishStorageAgeDays);

        /// <summary>
        /// 呆滞库龄天数
        /// </summary>
        public double SluggishStorageAgeDays
        {
            get { return GetProperty(SluggishStorageAgeDaysProperty); }
            set { SetProperty(SluggishStorageAgeDaysProperty, value); }
        }
        #endregion
    }
}
