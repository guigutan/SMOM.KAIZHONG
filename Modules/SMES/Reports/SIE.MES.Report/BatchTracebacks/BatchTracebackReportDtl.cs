using SIE.Barcodes.WipBatchs;
using SIE.Core.Algorithms.KZ;
using SIE.Domain;
using SIE.Domain.Query;
using SIE.MES.TaskManagement.Reports;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Report.BatchTracebacks
{
    /// <summary>
    /// 批次采集记录
    /// </summary>
    [RootEntity,Serializable]
    [Label("批次采集记录")]
    public class BatchTracebackReportDtl : Entity<double>
    {
        #region 批次标签 WipBatch
        /// <summary>
        /// 批次标签Id
        /// </summary>
        [Label("标签号")]
        public static readonly IRefIdProperty WipBatchIdProperty =
            P<BatchTracebackReportDtl>.RegisterRefId(e => e.WipBatchId, ReferenceType.Normal);

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
            P<BatchTracebackReportDtl>.RegisterRef(e => e.WipBatch, WipBatchIdProperty);

        /// <summary>
        /// 批次标签
        /// </summary>
        public WipBatch WipBatch
        {
            get { return this.GetRefEntity(WipBatchProperty); }
            set { this.SetRefEntity(WipBatchProperty, value); }
        }
        #endregion

        #region 批次号 BatchNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> BatchNoProperty = P<BatchTracebackReportDtl>.Register(e => e.BatchNo);

        /// <summary>
        /// 批次号
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
        public static readonly IRefIdProperty ReportRecordIdProperty = P<BatchTracebackReportDtl>.RegisterRefId(e => e.ReportRecordId, ReferenceType.Parent);

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
        public static readonly RefEntityProperty<ReportRecord> ReportRecordProperty = P<BatchTracebackReportDtl>.RegisterRef(e => e.ReportRecord, ReportRecordIdProperty);

        /// <summary>
        /// 报工记录
        /// </summary>
        public ReportRecord ReportRecord
        {
            get { return GetRefEntity(ReportRecordProperty); }
            set { SetRefEntity(ReportRecordProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<BatchTracebackReportDtl>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 班次 ShiftAlgorithm
        /// <summary>
        /// 班次
        /// </summary>
        [Label("班次")]
        public static readonly Property<string> ShiftAlgorithmProperty = P<BatchTracebackReportDtl>.Register(e => e.ShiftAlgorithm);

        /// <summary>
        /// 班次
        /// </summary>
        public string ShiftAlgorithm
        {
            get { return this.GetProperty(ShiftAlgorithmProperty); }
            set { this.SetProperty(ShiftAlgorithmProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 工序 ProcessCode
        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        public static readonly Property<string> ProcessCodeProperty = P<BatchTracebackReportDtl>.RegisterView(e => e.ProcessCode, p => p.ReportRecord.Process.Code);

        /// <summary>
        /// 工序
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
        }
        #endregion

        #region 资源编码 ResourceCode
        /// <summary>
        /// 资源编码
        /// </summary>
        [Label("资源编码")]
        public static readonly Property<string> ResourceCodeProperty = P<BatchTracebackReportDtl>.RegisterView(e => e.ResourceCode, p => p.ReportRecord.DispatchTask.Resource.Code);

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode
        {
            get { return this.GetProperty(ResourceCodeProperty); }
        }
        #endregion

        #endregion
    }

    internal class BatchTracebackReportDtlConfig : EntityConfig<BatchTracebackReportDtl>
    {
        protected override void ConfigMeta()
        {
            Func<IQuery> view = () => DB.Query<ReportWipBatch>("rwb")
.Join<ReportRecord>("rr", (x, y) => x.ReportRecordId == y.Id && y.SQL<int>("rr.IS_PHANTOM") == 0)
.Join<ReportRecord, ReportDispatchTask>("rdt", (rr, rdt) => rr.DispatchTaskId == rdt.Id && rdt.SQL<int>("rdt.IS_PHANTOM") == 0 && rdt.SQL<int?>("rdt.INV_ORG_ID") == RT.InvOrg)
.Select<ReportRecord, ReportDispatchTask>((rwb, rr, rdt) => new
{
Id = rwb.Id,
Wip_Batch_Id = rwb.WipBatchId,
Batch_No = rwb.BatchNo,
Report_Record_Id = rwb.ReportRecordId,
Qty = rwb.Qty,
Work_Order_Id = rdt.WorkOrderId,
Next_Process_Id = rwb.SQL<string>("(select s.process_id from WO_RT_PROC s inner join PROCESS_PTY p on p.is_phantom = 0 and p.process_id = s.process_id and (p.Scheduling = 1 or p.Dispatch_Work = 1) where s.is_phantom = 0 and s.work_order_id = rdt.work_order_id and s.index_ > (select s1.index_ from WO_RT_PROC s1 where s1.is_phantom = 0 and s1.work_order_id = rdt.work_order_id and s1.process_Id = rr.process_id and rownum = 1) order by s.index_ FETCH FIRST 1 ROW ONLY) Next_Process_Id"),
    Shift_Algorithm = rr.SQL("case when TO_NUMBER(to_char(rr.Report_Time,'HH24')) BETWEEN 8 and 19 then '早班' else '晚班' end as Shift_Algorithm")
})
.Where<ReportRecord>((rwb, rr) => rr.SourceId == null && rr.SQL<int>("rwb.IS_PHANTOM") == 0 && rr.SQL<int?>("rwb.INV_ORG_ID") == RT.InvOrg)
.ToQuery();
            Meta.MapView(view).MapAllProperties();
        }
    }
}
