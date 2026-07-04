using SIE.Domain;
using SIE.Domain.Query;
using SIE.MES.TaskManagement.Reports;
using SIE.MES.TaskManagement.SuspectProductLabels;
using SIE.MetaModel;
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
    /// 产品缺陷记录
    /// </summary>
    [RootEntity, Serializable]
    [Label("产品缺陷记录")]
    public class BatchTracebackDefetctLabelDtl : Entity<double>
    {
        #region 标签号 BatchNo
        /// <summary>
        /// 标签号
        /// </summary>
        [Label("标签号")]
        public static readonly Property<string> BatchNoProperty = P<BatchTracebackDefetctLabelDtl>.Register(e => e.BatchNo);

        /// <summary>
        /// 标签号
        /// </summary>
        public string BatchNo
        {
            get { return this.GetProperty(BatchNoProperty); }
            set { this.SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 批次数量 Qty
        /// <summary>
        /// 批次数量
        /// </summary>
        [Label("批次数量")]
        public static readonly Property<decimal> QtyProperty = P<BatchTracebackDefetctLabelDtl>.Register(e => e.Qty);

        /// <summary>
        /// 批次数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 可疑品标签 SuspectProductLabel
        /// <summary>
        /// 可疑品标签Id
        /// </summary>
        [Label("可疑品标签")]
        public static readonly IRefIdProperty SuspectProductLabelIdProperty =
            P<BatchTracebackDefetctLabelDtl>.RegisterRefId(e => e.SuspectProductLabelId, ReferenceType.Normal);

        /// <summary>
        /// 可疑品标签Id
        /// </summary>
        public double SuspectProductLabelId
        {
            get { return (double)this.GetRefId(SuspectProductLabelIdProperty); }
            set { this.SetRefId(SuspectProductLabelIdProperty, value); }
        }

        /// <summary>
        /// 可疑品标签
        /// </summary>
        public static readonly RefEntityProperty<SuspectProductLabel> SuspectProductLabelProperty =
            P<BatchTracebackDefetctLabelDtl>.RegisterRef(e => e.SuspectProductLabel, SuspectProductLabelIdProperty);

        /// <summary>
        /// 可疑品标签
        /// </summary>
        public SuspectProductLabel SuspectProductLabel
        {
            get { return this.GetRefEntity(SuspectProductLabelProperty); }
            set { this.SetRefEntity(SuspectProductLabelProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<BatchTracebackDefetctLabelDtl>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId
        {
            get { return (double)this.GetRefId(ProcessIdProperty); }
            set { this.SetRefId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty =
            P<BatchTracebackDefetctLabelDtl>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

    }

    internal class BatchTracebackDefetctLabelDtlConfig : EntityConfig<BatchTracebackDefetctLabelDtl>
    {
        protected override void ConfigMeta()
        {
            //Func<IQuery> view = () =>
            //DB.Query<SuspectProductLabelDetail>("spld")
            //.Select(spld => new
            //{
            //    Id = spld.Id,
            //    Batch_No = spld.SubBatchNo,
            //    Qty = spld.Qty,
            //    Suspect_Product_Label_Id = spld.SuspectProductLabelId

            //}).Where(spld => spld.SQL<int?>("spld.INV_ORG_ID") == RT.InvOrg && spld.SQL<int>("spld.IS_PHANTOM") == 0).ToQuery();

            Func<IQuery> view = () => DB.Query<ReportWipBatch>("rwb")
.Join<ReportRecord>("rr", (x, y) => x.ReportRecordId == y.Id)
.Join<ReportRecord, ReportDispatchTask>("rdt", (rr, rdt) => rr.DispatchTaskId == rdt.Id && rdt.SQL<int>("rdt.IS_PHANTOM") == 0 && rdt.SQL<int?>("rdt.INV_ORG_ID") == RT.InvOrg)
.Join<SuspectProductLabelDetail>("spld", (x, y) => y.SubBatchNo == x.BatchNo)
.Select<ReportRecord, ReportDispatchTask, SuspectProductLabelDetail>((rwb, rr, rdt, spld) => new
{
    Id = rwb.Id,
    Batch_No = rwb.BatchNo,
    Qty = rwb.Qty,
    Suspect_Product_Label_Id = spld.SuspectProductLabelId,
    Process_Id = rr.ProcessId
})
.Where<ReportRecord>((rwb, rr) => rr.SourceId == null && rr.SQL<int>("rr.IS_PHANTOM") == 0 && rwb.SQL<int?>("rwb.INV_ORG_ID") == RT.InvOrg && rwb.SQL<int>("rdt.IS_PHANTOM") == 0)
.ToQuery();

            Meta.MapView(view).MapAllProperties();
        }
    }
}
