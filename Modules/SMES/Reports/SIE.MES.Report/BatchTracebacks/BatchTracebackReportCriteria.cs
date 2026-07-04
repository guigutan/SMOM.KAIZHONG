using SIE.Barcodes.WipBatchs;
using SIE.Domain;
using SIE.MES.Report.BatchWipProducts;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Report.BatchTracebacks
{
    /// <summary>
    /// 批次追溯通用报表查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("批次追溯通用报表查询实体")]
    public class BatchTracebackReportCriteria : Criteria
    {
        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<BatchTracebackReportCriteria>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
            set { this.SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<BatchTracebackReportCriteria>.Register(e => e.ProductCode);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
            set { this.SetProperty(ProductCodeProperty, value); }
        }
        #endregion

        #region 旧料号 ShortDescription
        /// <summary>
        /// 旧料号
        /// </summary>
        [Label("旧料号")]
        public static readonly Property<string> ShortDescriptionProperty = P<BatchTracebackReportCriteria>.Register(e => e.ShortDescription);

        /// <summary>
        /// 旧料号
        /// </summary>
        public string ShortDescription
        {
            get { return this.GetProperty(ShortDescriptionProperty); }
            set { this.SetProperty(ShortDescriptionProperty, value); }
        }
        #endregion

        #region 批次类型 BatchType
        /// <summary>
        /// 批次类型
        /// </summary>
        [Label("批次类型")]
        public static readonly Property<BatchType?> BatchTypeProperty = P<BatchTracebackReportCriteria>.Register(e => e.BatchType);

        /// <summary>
        /// 批次类型
        /// </summary>
        public BatchType? BatchType
        {
            get { return this.GetProperty(BatchTypeProperty); }
            set { this.SetProperty(BatchTypeProperty, value); }
        }
        #endregion

        #region 车间名称 WorkShopName
        /// <summary>
        /// 车间名称
        /// </summary>
        [Label("车间名称")]
        public static readonly Property<string> WorkShopNameProperty = P<BatchTracebackReportCriteria>.Register(e => e.WorkShopName);

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkShopName
        {
            get { return this.GetProperty(WorkShopNameProperty); }
            set { this.SetProperty(WorkShopNameProperty, value); }
        }
        #endregion

        #region 当前工序 Process
        /// <summary>
        /// 当前工序Id
        /// </summary>
        [Label("当前工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<BatchTracebackReportCriteria>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 当前工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)this.GetRefNullableId(ProcessIdProperty); }
            set { this.SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 当前工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty =
            P<BatchTracebackReportCriteria>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 当前工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 下一工序 NextProcess
        /// <summary>
        /// 下一工序Id
        /// </summary>
        [Label("下一工序")]
        public static readonly IRefIdProperty NextProcessIdProperty
            = P<BatchTracebackReportCriteria>.RegisterRefId(e => e.NextProcessId, ReferenceType.Normal);

        /// <summary>
        /// 下一工序Id
        /// </summary>
        public double? NextProcessId
        {
            get { return (double?)GetRefNullableId(NextProcessIdProperty); }
            set { SetRefNullableId(NextProcessIdProperty, value); }
        }

        /// <summary>
        /// 下一工序
        /// </summary>
        public static readonly RefEntityProperty<Process> NextProcessProperty
            = P<BatchTracebackReportCriteria>.RegisterRef(e => e.NextProcess, NextProcessIdProperty);

        /// <summary>
        /// 下一工序
        /// </summary>
        public Process NextProcess
        {
            get { return GetRefEntity(NextProcessProperty); }
            set { SetRefEntity(NextProcessProperty, value); }
        }
        #endregion

        #region 是否完工下线 IsFinish
        /// <summary>
        /// 是否完工下线
        /// </summary>
        [Label("是否完工下线")]
        public static readonly Property<YesNo?> IsFinishProperty = P<BatchTracebackReportCriteria>.Register(e => e.IsFinish);

        /// <summary>
        /// 是否完工下线
        /// </summary>
        public YesNo? IsFinish
        {
            get { return this.GetProperty(IsFinishProperty); }
            set { this.SetProperty(IsFinishProperty, value); }
        }
        #endregion

        #region 批次标签 ItemLabelLot
        /// <summary>
        /// 批次标签
        /// </summary>
        [Label("批次标签")]
        public static readonly Property<string> ItemLabelLotProperty = P<BatchTracebackReportCriteria>.Register(e => e.ItemLabelLot);

        /// <summary>
        /// 批次标签
        /// </summary>
        public string ItemLabelLot
        {
            get { return this.GetProperty(ItemLabelLotProperty); }
            set { this.SetProperty(ItemLabelLotProperty, value); }
        }
        #endregion

        #region 标签号 BatchNo
        /// <summary>
        /// 标签号
        /// </summary>
        [Label("标签号")]
        public static readonly Property<string> BatchNoProperty = P<BatchTracebackReportCriteria>.Register(e => e.BatchNo);

        /// <summary>
        /// 标签号
        /// </summary>
        public string BatchNo
        {
            get { return this.GetProperty(BatchNoProperty); }
            set { this.SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 编码 ToolCode
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> ToolCodeProperty = P<BatchTracebackReportCriteria>.Register(e => e.ToolCode);

        /// <summary>
        /// 编码
        /// </summary>
        public string ToolCode
        {
            get { return this.GetProperty(ToolCodeProperty); }
            set { this.SetProperty(ToolCodeProperty, value); }
        }
        #endregion

        #region 单体条码 UnitBarcode
        /// <summary>
        /// 单体条码
        /// </summary>
        [Label("单体条码")]
        public static readonly Property<string> UnitBarcodeProperty = P<BatchTracebackReportCriteria>.Register(e => e.UnitBarcode);

        /// <summary>
        /// 单体条码
        /// </summary>
        public string UnitBarcode
        {
            get { return this.GetProperty(UnitBarcodeProperty); }
            set { this.SetProperty(UnitBarcodeProperty, value); }
        }
        #endregion

        #region 蓝标号 BlueLabel
        /// <summary>
        /// 蓝标号
        /// </summary>
        [Label("蓝标号")]
        public static readonly Property<string> BlueLabelProperty = P<BatchTracebackReportCriteria>.Register(e => e.BlueLabel);

        /// <summary>
        /// 蓝标号
        /// </summary>
        public string BlueLabel
        {
            get { return this.GetProperty(BlueLabelProperty); }
            set { this.SetProperty(BlueLabelProperty, value); }
        }
        #endregion
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<BatchTracebacksController>().CriteriaBatchTracebackReports(this);
        }

    }
}
