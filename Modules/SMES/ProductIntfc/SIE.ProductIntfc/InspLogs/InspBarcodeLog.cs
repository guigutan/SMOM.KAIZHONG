using SIE.Common;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.ProductIntfc.InspSettings;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using System;

namespace SIE.ProductIntfc.InspLogs
{
    /// <summary>
    /// 报检条码日志明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("报检条码日志明细")]
    public partial class InspBarcodeLog : DataEntity
    {
        #region 采集时间 CollectionDate
        /// <summary>
        /// 采集时间
        /// </summary>
        [Label("采集时间")]
        public static readonly Property<DateTime?> CollectionDateProperty = P<InspBarcodeLog>.Register(e => e.CollectionDate);

        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime? CollectionDate
        {
            get { return GetProperty(CollectionDateProperty); }
            set { SetProperty(CollectionDateProperty, value); }
        }
        #endregion

        #region 条码 Barcode
        /// <summary>
        /// 条码
        /// </summary>
        [Label("生产条码")]
        public static readonly Property<string> BarcodeProperty = P<InspBarcodeLog>.Register(e => e.Barcode);

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode
        {
            get { return this.GetProperty(BarcodeProperty); }
            set { this.SetProperty(BarcodeProperty, value); }
        }
        #endregion 

        #region 批次号 BatchNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> BatchNoProperty = P<InspBarcodeLog>.Register(e => e.BatchNo);

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo
        {
            get { return GetProperty(BatchNoProperty); }
            set { SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("报检工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<InspBarcodeLog>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)this.GetRefNullableId(ProcessIdProperty); }
            set { this.SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty =
            P<InspBarcodeLog>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 工位 Station
        /// <summary>
        /// 工位Id
        /// </summary>
        [Label("工位")]
        public static readonly IRefIdProperty StationIdProperty =
            P<InspBarcodeLog>.RegisterRefId(e => e.StationId, ReferenceType.Normal);

        /// <summary>
        /// 工位Id
        /// </summary>
        public double? StationId
        {
            get { return (double?)this.GetRefNullableId(StationIdProperty); }
            set { this.SetRefNullableId(StationIdProperty, value); }
        }

        /// <summary>
        /// 工位
        /// </summary>
        public static readonly RefEntityProperty<Station> StationProperty =
            P<InspBarcodeLog>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 工位
        /// </summary>
        public Station Station
        {
            get { return this.GetRefEntity(StationProperty); }
            set { this.SetRefEntity(StationProperty, value); }
        }
        #endregion

        #region 状态 InspState
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<InspState> InspStateProperty = P<InspBarcodeLog>.Register(e => e.InspState);

        /// <summary>
        /// 状态
        /// </summary>
        public InspState InspState
        {
            get { return this.GetProperty(InspStateProperty); }
            set { this.SetProperty(InspStateProperty, value); }
        }
        #endregion

        #region 报检时间 InspectionDate
        /// <summary>
        /// 报检时间
        /// </summary>
        [Label("报检时间")]
        public static readonly Property<DateTime?> InspectionDateProperty = P<InspBarcodeLog>.Register(e => e.InspectionDate);

        /// <summary>
        /// 报检时间
        /// </summary>
        public DateTime? InspectionDate
        {
            get { return GetProperty(InspectionDateProperty); }
            set { SetProperty(InspectionDateProperty, value); }
        }
        #endregion

        #region 报检日志 InspLog
        /// <summary>
        /// 报检日志Id
        /// </summary>
        public static readonly IRefIdProperty InspLogIdProperty = P<InspBarcodeLog>.RegisterRefId(e => e.InspLogId, ReferenceType.Parent);

        /// <summary>
        /// 报检日志Id
        /// </summary>
        public double InspLogId
        {
            get { return (double)GetRefId(InspLogIdProperty); }
            set { SetRefId(InspLogIdProperty, value); }
        }

        /// <summary>
        /// 报检日志
        /// </summary>
        public static readonly RefEntityProperty<InspLog> InspLogProperty = P<InspBarcodeLog>.RegisterRef(e => e.InspLog, InspLogIdProperty);

        /// <summary>
        /// 报检日志
        /// </summary>
        public InspLog InspLog
        {
            get { return GetRefEntity(InspLogProperty); }
            set { SetRefEntity(InspLogProperty, value); }
        }
        #endregion

        #region 注释 IsSelect
        /// <summary>
        /// 注释
        /// </summary>
        [Label("属性名")]
        public static readonly Property<bool> IsSelectProperty = P<InspBarcodeLog>.Register(e => e.IsSelect);

        /// <summary>
        /// 注释
        /// </summary>
        public bool IsSelect
        {
            get { return this.GetProperty(IsSelectProperty); }
            set { this.SetProperty(IsSelectProperty, value); }
        }
        #endregion

        #region 检验结果 InspectionResult
        /// <summary>
        /// 检验结果
        /// </summary>
        [Label("检验结果")]
        public static readonly Property<InspectionResult?> InspectionResultProperty = P<InspBarcodeLog>.Register(e => e.InspectionResult);

        /// <summary>
        /// 检验结果
        /// </summary>
        public InspectionResult? InspectionResult
        {
            get { return this.GetProperty(InspectionResultProperty); }
            set { this.SetProperty(InspectionResultProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 检验单号 CheckNo
        /// <summary>
        /// 检验单号
        /// </summary>
        [Label("检验单号")]
        public static readonly Property<string> CheckNoProperty = P<InspBarcodeLog>.RegisterView(e => e.CheckNo, p => p.InspLog.CheckNo);

        /// <summary>
        /// 检验单号
        /// </summary>
        public string CheckNo
        {
            get { return this.GetProperty(CheckNoProperty); }
        }
        #endregion

        #region 检验类型 InspType
        /// <summary>
        /// 检验类型
        /// </summary>
        [Label("检验类型")]
        public static readonly Property<InspType> InspTypeProperty = P<InspBarcodeLog>.RegisterView(e => e.InspType, p => p.InspLog.InspType);

        /// <summary>
        /// 检验类型
        /// </summary>
        public InspType InspType
        {
            get { return this.GetProperty(InspTypeProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 报检条码日志明细 实体配置
    /// </summary>
    internal class InspBarcodeLogConfig : EntityConfig<InspBarcodeLog>
    {
        /// <summary>
        /// 子类重写此方法，并完成对 Meta 属性的配置。
        /// 注意：
        /// * 为了给当前类的子类也运行同样的配置，这个方法可能会被调用多次。
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INF_INSP_BARCODE_LOG").MapAllPropertiesExcept(InspBarcodeLog.IsSelectProperty);
            Meta.EnablePhantoms();
        }
    }
}