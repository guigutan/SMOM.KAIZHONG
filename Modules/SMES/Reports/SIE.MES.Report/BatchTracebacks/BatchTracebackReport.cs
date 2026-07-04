using SIE.Barcodes.WipBatchs;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Query;
using SIE.MES.PackingQC;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Reports;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrder = SIE.MES.WorkOrders.WorkOrder;

namespace SIE.MES.Report.BatchTracebacks
{
    /// <summary>
    /// 批次追溯通用报表
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(BatchTracebackReportCriteria))]
    [Label("批次追溯通用报表")]
    public class BatchTracebackReport : Entity<double>
    {
        #region 批次标签 WipBatch
        /// <summary>
        /// 批次标签Id
        /// </summary>
        [Label("标签号")]
        public static readonly IRefIdProperty WipBatchIdProperty =
            P<BatchTracebackReport>.RegisterRefId(e => e.WipBatchId, ReferenceType.Normal);

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
            P<BatchTracebackReport>.RegisterRef(e => e.WipBatch, WipBatchIdProperty);

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
        public static readonly Property<string> BatchNoProperty = P<BatchTracebackReport>.Register(e => e.BatchNo);

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
        public static readonly IRefIdProperty ReportRecordIdProperty = P<BatchTracebackReport>.RegisterRefId(e => e.ReportRecordId, ReferenceType.Parent);

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
        public static readonly RefEntityProperty<ReportRecord> ReportRecordProperty = P<BatchTracebackReport>.RegisterRef(e => e.ReportRecord, ReportRecordIdProperty);

        /// <summary>
        /// 报工记录
        /// </summary>
        public ReportRecord ReportRecord
        {
            get { return GetRefEntity(ReportRecordProperty); }
            set { SetRefEntity(ReportRecordProperty, value); }
        }
        #endregion

        #region 批次数量 Qty
        /// <summary>
        /// 批次数量
        /// </summary>
        [Label("批次数量")]
        public static readonly Property<decimal> QtyProperty = P<BatchTracebackReport>.Register(e => e.Qty);

        /// <summary>
        /// 批次数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<BatchTracebackReport>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId
        {
            get { return (double?)GetRefNullableId(WorkOrderIdProperty); }
            set { SetRefNullableId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<BatchTracebackReport>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 下一工序 NextProcessId
        /// <summary>
        /// 下一工序
        /// </summary>
        [Label("下一工序")]
        public static readonly Property<double?> NextProcessIdProperty = P<BatchTracebackReport>.Register(e => e.NextProcessId);

        /// <summary>
        /// 下一工序
        /// </summary>
        public double? NextProcessId
        {
            get { return this.GetProperty(NextProcessIdProperty); }
            set { this.SetProperty(NextProcessIdProperty, value); }
        }
        #endregion


        #region 蓝标号 BlueLabel
        /// <summary>
        /// 蓝标号
        /// </summary>
        [Label("蓝标号")]
        public static readonly Property<string> BlueLabelProperty = P<BatchTracebackReport>.Register(e => e.BlueLabel);

        /// <summary>
        /// 蓝标号
        /// </summary>
        public string BlueLabel
        {
            get { return this.GetProperty(BlueLabelProperty); }
            set { this.SetProperty(BlueLabelProperty, value); }
        }
        #endregion

        #region 不映射数据库

        #region 工单返工数 ReworkQty
        /// <summary>
        /// 工单返工数
        /// </summary>
        [Label("工单返工数")]
        public static readonly Property<decimal> ReworkQtyProperty = P<BatchTracebackReport>.Register(e => e.ReworkQty);

        /// <summary>
        /// 工单返工数
        /// </summary>
        public decimal ReworkQty
        {
            get { return this.GetProperty(ReworkQtyProperty); }
            set { this.SetProperty(ReworkQtyProperty, value); }
        }
        #endregion

        #region 工单可疑品数 SuspectQty
        /// <summary>
        /// 工单可疑品数
        /// </summary>
        [Label("工单可疑品数")]
        public static readonly Property<decimal> SuspectQtyProperty = P<BatchTracebackReport>.Register(e => e.SuspectQty);

        /// <summary>
        /// 工单可疑品数
        /// </summary>
        public decimal SuspectQty
        {
            get { return this.GetProperty(SuspectQtyProperty); }
            set { this.SetProperty(SuspectQtyProperty, value); }
        }
        #endregion

        #region 报废数 ScrapQty
        /// <summary>
        /// 报废数
        /// </summary>
        [Label("报废数")]
        public static readonly Property<decimal> ScrapQtyProperty = P<BatchTracebackReport>.Register(e => e.ScrapQty);

        /// <summary>
        /// 报废数
        /// </summary>
        public decimal ScrapQty
        {
            get { return this.GetProperty(ScrapQtyProperty); }
            set { this.SetProperty(ScrapQtyProperty, value); }
        }
        #endregion

        #region 委外中 IsOutsourcing
        /// <summary>
        /// 委外中
        /// </summary>
        [Label("委外中")]
        public static readonly Property<bool> IsOutsourcingProperty
            = P<BatchTracebackReport>.Register(e => e.IsOutsourcing);

        /// <summary>
        /// 委外中
        /// </summary>
        public bool IsOutsourcing
        {
            get { return this.GetProperty(IsOutsourcingProperty); }
            set { this.SetProperty(IsOutsourcingProperty, value); }
        }
        #endregion

        #region 批次类型 BatchType
        /// <summary>
        /// 批次类型
        /// </summary>
        [Label("批次类型")]
        public static readonly Property<string> BatchTypeProperty = P<BatchTracebackReport>.Register(e => e.BatchType);

        /// <summary>
        /// 批次类型
        /// </summary>
        public string BatchType
        {
            get { return this.GetProperty(BatchTypeProperty); }
            set { this.SetProperty(BatchTypeProperty, value); }
        }
        #endregion

        #region 是否完工下线 IsFinish
        /// <summary>
        /// 是否完工下线
        /// </summary>
        [Label("是否完工下线")]
        public static readonly Property<bool> IsFinishProperty = P<BatchTracebackReport>.RegisterReadOnly(
            e => e.IsFinish, e => e.GetIsFinish(), NextProcessIdProperty);
        /// <summary>
        /// 是否完工下线
        /// </summary>

        public bool IsFinish
        {
            get { return this.GetProperty(IsFinishProperty); }
        }
        private bool GetIsFinish()
        {
            if (NextProcessId == 0)
                return true;
            return false;
        }
        #endregion

        #region 下一工序 NextProcessCode
        /// <summary>
        /// 下一工序
        /// </summary>
        [Label("下一工序")]
        public static readonly Property<string> NextProcessCodeProperty = P<BatchTracebackReport>.Register(e => e.NextProcessCode);

        /// <summary>
        /// 下一工序
        /// </summary>
        public string NextProcessCode
        {
            get { return this.GetProperty(NextProcessCodeProperty); }
            set { this.SetProperty(NextProcessCodeProperty, value); }
        }
        #endregion

        #endregion

        #region 视图属性

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<BatchTracebackReport>.RegisterView(e => e.WorkOrderNo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
        }
        #endregion

        #region 工单类型 Type
        /// <summary>
        /// 工单类型
        /// </summary>
        [Label("工单类型")]
        public static readonly Property<WorkOrderType> TypeProperty = P<BatchTracebackReport>.RegisterView(e => e.Type, p => p.WorkOrder.Type);

        /// <summary>
        /// 工单类型
        /// </summary>
        public WorkOrderType Type
        {
            get { return GetProperty(TypeProperty); }
        }
        #endregion

        #region 工单数量 PlanQty
        /// <summary>
        /// 工单数量
        /// </summary>
        [Label("工单数量")]
        public static readonly Property<decimal> PlanQtyProperty = P<BatchTracebackReport>.RegisterView(e => e.PlanQty, p => p.WorkOrder.PlanQty);

        /// <summary>
        /// 工单数量
        /// </summary>
        public decimal PlanQty
        {
            get { return this.GetProperty(PlanQtyProperty); }
        }
        #endregion

        #region 工单完工数 FinishQty
        /// <summary>
        /// 工单完工数
        /// </summary>
        [Label("工单完工数")]
        public static readonly Property<decimal> FinishQtyProperty = P<BatchTracebackReport>.RegisterView(e => e.FinishQty, p => p.WorkOrder.FinishQty);

        /// <summary>
        /// 工单完工数
        /// </summary>
        public decimal FinishQty
        {
            get { return this.GetProperty(FinishQtyProperty); }
        }
        #endregion

        #region 车间 Fevor
        /// <summary>
        /// 车间
        /// </summary>
        [Label("车间")]
        public static readonly Property<string> FevorProperty = P<BatchTracebackReport>.RegisterView(e => e.Fevor, p => p.WorkOrder.Fevor);

        /// <summary>
        /// 车间
        /// </summary>
        public string Fevor
        {
            get { return this.GetProperty(FevorProperty); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<BatchTracebackReport>.RegisterView(e => e.ProductCode, p => p.WorkOrder.Product.Code);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<BatchTracebackReport>.RegisterView(e => e.ProductName, p => p.WorkOrder.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }
        #endregion

        #region 旧料号 ShortDescription
        /// <summary>
        /// 旧料号
        /// </summary>
        [Label("旧料号")]
        public static readonly Property<string> ShortDescriptionProperty = P<BatchTracebackReport>.RegisterView(e => e.ShortDescription, p => p.WorkOrder.Product.ShortDescription);

        /// <summary>
        /// 旧料号
        /// </summary>
        public string ShortDescription
        {
            get { return this.GetProperty(ShortDescriptionProperty); }
        }
        #endregion

        #region 当前工序 ProcessCode
        /// <summary>
        /// 当前工序
        /// </summary>
        [Label("当前工序")]
        public static readonly Property<string> ProcessCodeProperty = P<BatchTracebackReport>.RegisterView(e => e.ProcessCode, p => p.ReportRecord.Process.Code);

        /// <summary>
        /// 当前工序
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
        }
        #endregion

        #endregion
    }

    internal class BatchTracebackReportConfig : EntityConfig<BatchTracebackReport>
    {
        protected override void ConfigMeta()
        {
            Func<IQuery> view = () => DB.Query<ReportWipBatch>("rwb")
            .Join<ReportRecord>("rr", (x, y) => x.ReportRecordId == y.Id)
.Join<ReportRecord, ReportDispatchTask>("rdt", (rr, rdt) => rr.DispatchTaskId == rdt.Id && rdt.SQL<int>("rdt.IS_PHANTOM") == 0 && rdt.SQL<int?>("rdt.INV_ORG_ID") == RT.InvOrg)
.LeftJoin<PackingDetail>("pd", (rwb, pd) => (pd.LabelType == LabelTypeEnum.BatchLabel && pd.ProductLabel == rwb.BatchNo)
|| (pd.LabelType != LabelTypeEnum.BatchLabel && pd.BatchLabel == rwb.BatchNo))
.LeftJoin<PackingDetail, PackingQc>("pq", (pd, pq) => pd.PackingQcId == pq.Id)
.Select<ReportRecord, ReportDispatchTask, PackingDetail, PackingQc>((rwb, rr, rdt, pd, pq) => new
{
    Id = rwb.Id,
    Wip_Batch_Id = rwb.WipBatchId,
    Batch_No = rwb.BatchNo,
    Report_Record_Id = rwb.ReportRecordId,
    Qty = rwb.Qty,
    Work_Order_Id = rdt.WorkOrderId,
    Blue_Label = pq.BlueLabel,
    Next_Process_Id = rwb.SQL<string>("(select s.process_id from WO_RT_PROC s inner join PROCESS_PTY p on p.is_phantom = 0 and p.process_id = s.process_id and (p.Scheduling = 1 or p.Dispatch_Work = 1) where s.is_phantom = 0 and s.work_order_id = rdt.work_order_id and s.index_ > (select s1.index_ from WO_RT_PROC s1 where s1.is_phantom = 0 and s1.work_order_id = rdt.work_order_id and s1.process_Id = rr.process_id and rownum = 1) order by s.index_ FETCH FIRST 1 ROW ONLY) Next_Process_Id")
})
.Distinct()
.Where<ReportRecord>((rwb, rr) => rr.SourceId == null && rr.SQL<int>("rwb.IS_PHANTOM") == 0 && rr.SQL<int?>("rwb.INV_ORG_ID") == RT.InvOrg)
.ToQuery();
            Meta.MapView(view).MapAllProperties();
            Meta.Property(BatchTracebackReport.ReworkQtyProperty).DontMapColumn();
            Meta.Property(BatchTracebackReport.SuspectQtyProperty).DontMapColumn();
            Meta.Property(BatchTracebackReport.IsOutsourcingProperty).DontMapColumn();
            Meta.Property(BatchTracebackReport.BatchTypeProperty).DontMapColumn();
            Meta.Property(BatchTracebackReport.NextProcessCodeProperty).DontMapColumn();
            Meta.Property(BatchTracebackReport.ScrapQtyProperty).DontMapColumn();
        }
    }
}
