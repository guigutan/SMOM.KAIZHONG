using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using System;

namespace SIE.ProductIntfc.InspRecords
{
    /// <summary>
    /// 报检条码
    /// </summary>
    [ChildEntity, Serializable]
    [Label("报检条码")]
    public partial class InspBarcode : DataEntity
    {
        #region 生产条码 Barcode
        /// <summary>
        /// 条码
        /// </summary>
        [Label("生产条码")]
        public static readonly Property<string> BarcodeProperty = P<InspBarcode>.Register(e => e.Barcode);

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode
        {
            get { return this.GetProperty(BarcodeProperty); }
            set { this.SetProperty(BarcodeProperty, value); }
        }
        #endregion

        #region 报检时间 InspDate
        /// <summary>
        /// 报检时间
        /// </summary>
        [Label("报检时间")]
        public static readonly Property<DateTime> InspDateProperty = P<InspBarcode>.Register(e => e.InspDate);

        /// <summary>
        /// 报检时间
        /// </summary>
        public DateTime InspDate
        {
            get { return GetProperty(InspDateProperty); }
            set { SetProperty(InspDateProperty, value); }
        }
        #endregion

        #region 采集时间 CollectionDate
        /// <summary>
        /// 采集时间
        /// </summary>
        [Label("采集时间")]
        public static readonly Property<DateTime?> CollectionDateProperty = P<InspBarcode>.Register(e => e.CollectionDate);

        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime? CollectionDate
        {
            get { return GetProperty(CollectionDateProperty); }
            set { SetProperty(CollectionDateProperty, value); }
        }
        #endregion

        #region 报检工序 Process
        /// <summary>
        /// 报检工序Id
        /// </summary>
        [Label("报检工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<InspBarcode>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 报检工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)this.GetRefNullableId(ProcessIdProperty); }
            set { this.SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 报检工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty =
            P<InspBarcode>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 报检工序
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
            P<InspBarcode>.RegisterRefId(e => e.StationId, ReferenceType.Normal);

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
            P<InspBarcode>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 工位
        /// </summary>
        public Station Station
        {
            get { return this.GetRefEntity(StationProperty); }
            set { this.SetRefEntity(StationProperty, value); }
        }
        #endregion

        #region 报检记录 InspRecord
        /// <summary>
        /// 报检记录Id
        /// </summary>
        [Label("报检记录")]
        public static readonly IRefIdProperty InspRecordIdProperty = P<InspBarcode>.RegisterRefId(e => e.InspRecordId, ReferenceType.Parent);

        /// <summary>
        /// 报检记录Id
        /// </summary>
        public double InspRecordId
        {
            get { return (double)GetRefId(InspRecordIdProperty); }
            set { SetRefId(InspRecordIdProperty, value); }
        }

        /// <summary>
        /// 报检记录
        /// </summary>
        public static readonly RefEntityProperty<InspRecord> InspRecordProperty = P<InspBarcode>.RegisterRef(e => e.InspRecord, InspRecordIdProperty);

        /// <summary>
        /// 报检记录
        /// </summary>
        public InspRecord InspRecord
        {
            get { return GetRefEntity(InspRecordProperty); }
            set { SetRefEntity(InspRecordProperty, value); }
        }
        #endregion

        #region 生产批号 BatchNo
        /// <summary>
        /// 生产批号
        /// </summary>
        [Label("生产批号")]
        public static readonly Property<string> BatchNoProperty = P<InspBarcode>.Register(e => e.BatchNo);

        /// <summary>
        /// 生产批号
        /// </summary>
        public string BatchNo
        {
            get { return this.GetProperty(BatchNoProperty); }
            set { this.SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 操作人 OperateBy
        /// <summary>
        /// 操作人Id
        /// </summary>
        [Label("操作人")]
        public static readonly IRefIdProperty OperateByIdProperty = P<InspBarcode>.RegisterRefId(e => e.OperateById, ReferenceType.Normal);

        /// <summary>
        /// 操作人Id
        /// </summary>
        public double OperateById
        {
            get { return (double)GetRefId(OperateByIdProperty); }
            set { SetRefId(OperateByIdProperty, value); }
        }

        /// <summary>
        /// 操作人
        /// </summary>
        public static readonly RefEntityProperty<Employee> OperateByProperty = P<InspBarcode>.RegisterRef(e => e.OperateBy, OperateByIdProperty);

        /// <summary>
        /// 操作人
        /// </summary>
        public Employee OperateBy
        {
            get { return GetRefEntity(OperateByProperty); }
            set { SetRefEntity(OperateByProperty, value); }
        }
        #endregion

        #region 任务单Id DispatchTaskId
        /// <summary>
        /// 任务单Id
        /// </summary>
        [Label("任务单Id")]
        public static readonly Property<double?> DispatchTaskIdProperty = P<InspBarcode>.Register(e => e.DispatchTaskId);

        /// <summary>
        /// 任务单Id
        /// </summary>
        public double? DispatchTaskId
        {
            get { return this.GetProperty(DispatchTaskIdProperty); }
            set { this.SetProperty(DispatchTaskIdProperty, value); }
        }
        #endregion

        #region 报工记录Id ReportRecordId
        /// <summary>
        /// 报工记录Id
        /// </summary>
        [Label("报工记录Id")]
        public static readonly Property<double?> ReportRecordIdProperty = P<InspBarcode>.Register(e => e.ReportRecordId);

        /// <summary>
        /// 报工记录Id
        /// </summary>
        public double? ReportRecordId
        {
            get { return this.GetProperty(ReportRecordIdProperty); }
            set { this.SetProperty(ReportRecordIdProperty, value); }
        }
        #endregion

        #region 报检数量 InspQty
        /// <summary>
        /// 报检数量
        /// </summary>
        [Label("报检数量")]
        public static readonly Property<decimal> InspQtyProperty = P<InspBarcode>.Register(e => e.InspQty);

        /// <summary>
        /// 报检数量
        /// </summary>
        public decimal InspQty
        {
            get { return this.GetProperty(InspQtyProperty); }
            set { this.SetProperty(InspQtyProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 报检条码 实体配置
    /// </summary>
    internal class InspBarcodeConfig : EntityConfig<InspBarcode>
    {
        /// <summary>
        /// 子类重写此方法，并完成对 Meta 属性的配置。
        /// 注意：
        /// * 为了给当前类的子类也运行同样的配置，这个方法可能会被调用多次。
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INF_INSP_BARCODE").MapAllProperties();
        }
    }
}