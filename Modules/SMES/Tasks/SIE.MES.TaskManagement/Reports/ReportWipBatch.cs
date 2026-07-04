using SIE.Barcodes.WipBatchs;
using SIE.Defects;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TaskManagement.Reports
{
    /// <summary>
    /// 报工批次标签
    /// </summary>
    [ChildEntity, Serializable]
    [Label("报工批次标签")]
    public partial class ReportWipBatch : DataEntity
    {
        #region 批次标签 WipBatch
        /// <summary>
        /// 批次标签Id
        /// </summary>
        [Label("标签号")]
        public static readonly IRefIdProperty WipBatchIdProperty =
            P<ReportWipBatch>.RegisterRefId(e => e.WipBatchId, ReferenceType.Normal);

        /// <summary>
        /// 批次标签Id
        /// </summary>
        public double? WipBatchId
        {
            get { return (double?)this.GetRefNullableId(WipBatchIdProperty); }
            set { this.SetRefNullableId(WipBatchIdProperty, value); }
        }

        /// <summary>
        /// 批次标签
        /// </summary>
        public static readonly RefEntityProperty<WipBatch> WipBatchProperty =
            P<ReportWipBatch>.RegisterRef(e => e.WipBatch, WipBatchIdProperty);

        /// <summary>
        /// 批次标签
        /// </summary>
        public WipBatch WipBatch
        {
            get { return this.GetRefEntity(WipBatchProperty); }
            set { this.SetRefEntity(WipBatchProperty, value); }
        }
        #endregion

        #region 批次标签号 BatchNo
        /// <summary>
        /// 批次标签号
        /// </summary>
        [Label("标签号")]
        public static readonly Property<string> BatchNoProperty = P<ReportWipBatch>.Register(e => e.BatchNo);

        /// <summary>
        /// 批次标签号
        /// </summary>
        public string BatchNo
        {
            get { return this.GetProperty(BatchNoProperty); }
            set { this.SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 报工记录 ReportRecord
        /// <summary>
        /// 报工记录Id
        /// </summary>
        [Label("报工记录")]
        public static readonly IRefIdProperty ReportRecordIdProperty = P<ReportWipBatch>.RegisterRefId(e => e.ReportRecordId, ReferenceType.Parent);

        /// <summary>
        /// 报工记录Id
        /// </summary>
        public double ReportRecordId
        {
            get { return (double)GetRefId(ReportRecordIdProperty); }
            set { SetRefId(ReportRecordIdProperty, value); }
        }

        /// <summary>
        /// 报工记录
        /// </summary>
        public static readonly RefEntityProperty<ReportRecord> ReportRecordProperty = P<ReportWipBatch>.RegisterRef(e => e.ReportRecord, ReportRecordIdProperty);

        /// <summary>
        /// 报工记录
        /// </summary>
        public ReportRecord ReportRecord
        {
            get { return GetRefEntity(ReportRecordProperty); }
            set { SetRefEntity(ReportRecordProperty, value); }
        }
        #endregion

        #region 报工数量 Qty
        /// <summary>
        /// 报工数量
        /// </summary>
        [Label("报工数量")]
        public static readonly Property<decimal> QtyProperty = P<ReportWipBatch>.Register(e => e.Qty);

        /// <summary>
        /// 报工数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 来源标签 SourceNo
        /// <summary>
        /// 来源标签
        /// </summary>
        [Label("来源标签")]
        public static readonly Property<string> SourceNoProperty = P<ReportWipBatch>.Register(e => e.SourceNo);

        /// <summary>
        /// 来源标签
        /// </summary>
        public string SourceNo
        {
            get { return this.GetProperty(SourceNoProperty); }
            set { this.SetProperty(SourceNoProperty, value); }
        }
        #endregion


        #region 视图属性

        #region 工单Id WorkOrderId
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单Id")]
        public static readonly Property<double> WorkOrderIdProperty = P<ReportWipBatch>.RegisterView(e => e.WorkOrderId, p => p.ReportRecord.WorkOrderId);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId
        {
            get { return this.GetProperty(WorkOrderIdProperty); }
        }

        #endregion

        #region 任务单Id DispatchTaskId
        /// <summary>
        /// 任务单Id
        /// </summary>
        [Label("任务单Id")]
        public static readonly Property<double> DispatchTaskIdProperty = P<ReportWipBatch>.RegisterView(e => e.DispatchTaskId, p => p.ReportRecord.DispatchTaskId);

        /// <summary>
        /// 任务单Id
        /// </summary>
        public double DispatchTaskId
        {
            get { return this.GetProperty(DispatchTaskIdProperty); }
        }

        #endregion

        #region 工序编码 ProcessCode
        /// <summary>
        /// 工序编码
        /// </summary>
        [Label("工序编码")]
        public static readonly Property<string> ProcessCodeProperty = P<ReportWipBatch>.RegisterView(e => e.ProcessCode, p => p.ReportRecord.Process.Code);

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
        }
        #endregion

        #region 工序模板 PrintTemplateId
        /// <summary>
        /// 工序模板
        /// </summary>
        [Label("工序模板")]
        public static readonly Property<double> PrintTemplateIdProperty = P<ReportWipBatch>.RegisterView(e => e.PrintTemplateId, p => p.ReportRecord.Process.PrintTemplateId);

        /// <summary>
        /// 工序模板
        /// </summary>
        public double PrintTemplateId
        {
            get { return this.GetProperty(PrintTemplateIdProperty); }
        }

        #endregion

        #region 资源编码 ResourceCode
        /// <summary>
        /// 资源编码
        /// </summary>
        [Label("资源编码")]
        public static readonly Property<string> ResourceCodeProperty = P<ReportWipBatch>.RegisterView(e => e.ResourceCode, p => p.ReportRecord.DispatchTask.Resource.Code);

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ResourceCode
        {
            get { return this.GetProperty(ResourceCodeProperty); }
        }
        #endregion

        #region 班次 ShiftName
        /// <summary>
        /// 班次
        /// </summary>
        [Label("班次")]
        public static readonly Property<string> ShiftNameProperty = P<ReportWipBatch>.RegisterView(e => e.ShiftName, p => p.ReportRecord.Shift.Name);

        /// <summary>
        /// 班次
        /// </summary>
        public string ShiftName
        {
            get { return this.GetProperty(ShiftNameProperty); }
        }
        #endregion

        #endregion

        #region 不映射数据库

        #region BsColor Color
        /// <summary>
        /// BsColor
        /// </summary>
        [Label("BsColor")]
        public static readonly Property<string> ColorProperty = P<ReportWipBatch>.Register(e => e.Color);

        /// <summary>
        /// BsColor
        /// </summary>
        public string Color
        {
            get { return this.GetProperty(ColorProperty); }
            set { this.SetProperty(ColorProperty, value); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 报工缺陷 实体配置
    /// </summary>
    internal class ReportWipBatchConfig : EntityConfig<ReportWipBatch>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TM_REPORT_WIP_BATCH").MapAllProperties();
            Meta.Property(ReportWipBatch.ColorProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}