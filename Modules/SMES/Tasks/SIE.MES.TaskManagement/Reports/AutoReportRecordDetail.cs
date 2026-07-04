using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TaskManagement.Reports
{
    /// <summary>
    /// 自动报工记录明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("自动报工记录明细")]
    public partial class AutoReportRecordDetail : DataEntity
    {
        #region 自动报工记录 Record
        /// <summary>
        /// 自动报工记录Id
        /// </summary>
        [Label("自动报工记录")]
        public static readonly IRefIdProperty RecordIdProperty =
            P<AutoReportRecordDetail>.RegisterRefId(e => e.RecordId, ReferenceType.Normal);

        /// <summary>
        /// 自动报工记录Id
        /// </summary>
        public double RecordId
        {
            get { return (double)this.GetRefId(RecordIdProperty); }
            set { this.SetRefId(RecordIdProperty, value); }
        }

        /// <summary>
        /// 自动报工记录
        /// </summary>
        public static readonly RefEntityProperty<AutoReportRecord> RecordProperty =
            P<AutoReportRecordDetail>.RegisterRef(e => e.Record, RecordIdProperty);

        /// <summary>
        /// 自动报工记录
        /// </summary>
        public AutoReportRecord Record
        {
            get { return this.GetRefEntity(RecordProperty); }
            set { this.SetRefEntity(RecordProperty, value); }
        }
        #endregion

        #region 条码 Barcode
        /// <summary>
        /// 条码
        /// </summary>
        [Label("条码")]
        [Required]
        public static readonly Property<string> BarcodeProperty = P<AutoReportRecordDetail>.Register(e => e.Barcode);

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode
        {
            get { return this.GetProperty(BarcodeProperty); }
            set { this.SetProperty(BarcodeProperty, value); }
        }
        #endregion

        #region 合格数量 OkQty
        /// <summary>
        /// 合格数量
        /// </summary>
        [Label("合格数量")]
        [MinValue(1)]
        public static readonly Property<decimal> OkQtyProperty = P<AutoReportRecordDetail>.Register(e => e.OkQty);

        /// <summary>
        /// 合格数量
        /// </summary>
        public decimal OkQty
        {
            get { return this.GetProperty(OkQtyProperty); }
            set { this.SetProperty(OkQtyProperty, value); }
        }
        #endregion

        #region 不合格数量 NgQty
        /// <summary>
        /// 不合格数量
        /// </summary>
        [Label("不合格数量")]
        [MinValue(0)]
        public static readonly Property<decimal> NgQtyProperty = P<AutoReportRecordDetail>.Register(e => e.NgQty);

        /// <summary>
        /// 不合格数量
        /// </summary>
        public decimal NgQty
        {
            get { return this.GetProperty(NgQtyProperty); }
            set { this.SetProperty(NgQtyProperty, value); }
        }
        #endregion

        #region 是否已报工 IsReport
        /// <summary>
        /// 是否已报工
        /// </summary>
        [Label("是否已报工")]
        public static readonly Property<bool> IsReportProperty = P<AutoReportRecordDetail>.Register(e => e.IsReport);

        /// <summary>
        /// 是否已报工
        /// </summary>
        public bool IsReport
        {
            get { return this.GetProperty(IsReportProperty); }
            set { this.SetProperty(IsReportProperty, value); }
        }
        #endregion

        #region 采集时间 CollectDate
        /// <summary>
        /// 采集时间
        /// </summary>
        [Label("采集时间")]
        public static readonly Property<DateTime> CollectDateProperty = P<AutoReportRecordDetail>.Register(e => e.CollectDate);

        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime CollectDate
        {
            get { return this.GetProperty(CollectDateProperty); }
            set { this.SetProperty(CollectDateProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 自动报工记录明细 实体配置
    /// </summary>
    internal class AutoReportRecordDetailConfig : EntityConfig<AutoReportRecordDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TM_AUTO_REPORT_RECORD_DTL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}