using Newtonsoft.Json;

using SIE.Domain;
using SIE.MES.Report.BatchTracebacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Report.BatchTracebacks
{
    /// <summary>
    /// 批次采集记录视图配置
    /// </summary>
    public class BatchTracebackReportDtlViewConfig : WebViewConfig<BatchTracebackReportDtl>
    {
        /// <summary>
        /// 总视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(BatchTracebackReport), typeof(BatchTracebackReportDtl));
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.BatchNo).Show().Readonly();
                View.Property(p => p.Qty).Show().Readonly();
                View.Property(p => p.ProcessCode).Show().Readonly();
                View.Property(p => p.ShiftAlgorithm).Show().Readonly();
                View.Property(p => p.ResourceCode).Show().Readonly();
                View.AttachChildrenProperty(typeof(KeyComponentPartDtl), (o) =>
                {
                    var args = o as ChildPagingDataWithParentEntityArgs;
                    var parent = JsonConvert.DeserializeObject<BatchTracebackReportDtl>(args.ParentEntity);
                    var list = RT.Service.Resolve<BatchTracebacksController>().GetBatchTracebackKeyDtlViewModelList(parent.BatchNo, parent.ReportRecord,parent.Qty, args.PagingInfo, args.SortInfo);
                    return list;
                }).Show(ChildShowInWhere.All).HasLabel("标签号对应关键件");

                View.AttachChildrenProperty(typeof(BatchTracebackKeyDtl), (o) =>
                {
                    //var parent = o.Parent as BatchTracebackReportDtl;
                    var args = o as ChildPagingDataWithParentEntityArgs;
                    var parent = JsonConvert.DeserializeObject<BatchTracebackReportDtl>(args.ParentEntity);
                    var list = RT.Service.Resolve<BatchTracebacksController>().GetBatchTracebackKeyDtlsByReportRecordId(parent.ReportRecordId, args.PagingInfo, args.SortInfo);
                    return list;
                }).Show(ChildShowInWhere.All).HasLabel("产品生产关键件");

            }
        }
    }
}
