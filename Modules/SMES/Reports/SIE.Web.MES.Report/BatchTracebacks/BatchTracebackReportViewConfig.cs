using DevExpress.DataProcessing;
using Newtonsoft.Json;
using SIE.Core.QmsStaticConst;
using SIE.MES.PackingQC;
using SIE.MES.Report.BatchTracebacks;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Report.BatchTracebacks
{
    public class BatchTracebackReportViewConfig : WebViewConfig<BatchTracebackReport>
    {
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(BatchTracebackReport));
        }

        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.BatchNo).ShowInList(150).Readonly();
                View.Property(p => p.BlueLabel).ShowInList(150).Readonly();
                View.Property(p => p.BatchType).Show().Readonly();
                View.Property(p => p.Qty).Show().Readonly();
                View.Property(p => p.WorkOrderNo).Show().Readonly();
                View.Property(p => p.Type).Show().Readonly();
                View.Property(p => p.PlanQty).Show().Readonly();
                View.Property(p => p.FinishQty).Show().Readonly();
                View.Property(p => p.ScrapQty).Show().Readonly();
                View.Property(p => p.ReworkQty).Show().Readonly();
                View.Property(p => p.Fevor).Show().Readonly();
                View.Property(p => p.SuspectQty).Show().Readonly();
                View.Property(p => p.ProductCode).Show().Readonly();
                View.Property(p => p.ProductName).Show().Readonly();
                View.Property(p => p.ShortDescription).Show().Readonly();
                View.Property(p => p.ProcessCode).Show().Readonly();
                View.Property(p => p.NextProcessCode).Show().Readonly();
                View.Property(p => p.IsOutsourcing).Show().Readonly();
                View.AttachChildrenProperty(typeof(BatchTracebackReportDtl), (o) =>
                {
                    var parent = o.Parent as BatchTracebackReport;
                    var args = o as ChildPagingDataWithParentEntityArgs;
                    var list = RT.Service.Resolve<BatchTracebacksController>().GetBatchTracebackReportDtlsByIds(parent.Id, args.PagingInfo, args.SortInfo);
                    return list;
                }).Show(ChildShowInWhere.All).HasLabel("批次采集记录");
                View.AttachChildrenProperty(typeof(BatchTracebackDefetctLabelDtl), (o) =>
                {
                    var args = o as ChildPagingDataWithParentEntityArgs;
                    var parent = JsonConvert.DeserializeObject<BatchTracebackReport>(args.ParentEntity);//o.Parent as BatchTracebackReport;
                    var list = RT.Service.Resolve<BatchTracebacksController>().GetBatchTracebackDefetctLabelDtlsById(parent.BatchNo, parent.ProcessCode, args.PagingInfo, args.SortInfo);
                    return list;
                }).Show(ChildShowInWhere.All).HasLabel("产品缺陷记录");
                View.AttachChildrenProperty(typeof(BatchTracebackPreSetup), (o) =>
                {
                    var args = o as ChildPagingDataWithParentEntityArgs;
                    var parent = JsonConvert.DeserializeObject<BatchTracebackReport>(args.ParentEntity);//o.Parent as BatchTracebackReport;
                    var list = RT.Service.Resolve<BatchTracebacksController>().GetBatchTracebackPreSetups(parent.BatchNo, parent.ProcessCode, args.PagingInfo, args.SortInfo);
                    return list;
                }).Show(ChildShowInWhere.All).HasLabel("开机准备记录");
                View.AttachChildrenProperty(typeof(PackingDetail), (o) =>
                {
                    var args = o as ChildPagingDataWithParentEntityArgs;
                    var parent = JsonConvert.DeserializeObject<BatchTracebackReport>(args.ParentEntity);
                    var list = RT.Service.Resolve<BatchTracebacksController>().GetPackingDetailsByBatchNoAndWoNo(parent.BatchNo, parent.WorkOrderNo, args.PagingInfo, args.SortInfo);
                    return list;
                }, "SnView").Show(ChildShowInWhere.All).HasLabel("SN");
            }
        }
    }
}
